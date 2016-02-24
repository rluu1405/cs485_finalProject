/* I want to keep the FSM structure that I have here but not necessarily how it calculates movement
 *   -Jeff
 * 
 * Ideally, we want AI to avoid collisions, avoid obstacles, flock with teammates, priority-based arbitration
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicAi : MonoBehaviour
{
    // states/attributes
    public float rangeOfCollisionAvoidance = 100f;
    // enums are annoying in Unity inspector .. need class or other soln
    public enum State { idle = 0, travelling = 1, fighting = 2, flocking = 3};
    public State state = State.idle; // default state
    public float shieldActiveTime = 100f;
	public float flockingTendency = 0.6f; // probability
	public FlockLeader gloriousLeader;
    public float baseTendencyToFire = 0.5f;
    protected float tendencyToFire;

    // waypoints/movement
    internal Vector3 heading; // current destination (not always a waypoint or enemy)
    protected List<Vector3> waypoints = new List<Vector3>();
    protected int waypointIndex;
    protected float maneuverTimer = 0f;
    protected float maneuverTimerBound = 2f; //how long ships will wait between maneuvers
    protected float idleTimer = 0f;
    public float idleTimerBound = 60f; //how long ships will wait at a destination before moving on

    // enemy target
    public float optimalRange = 200f;
    public float minimumRange = 160f;




    #region Reference to Attached GO
    protected Ship aiShip;
    protected Targeting targeting;
    protected WeaponFire weapon;
    #endregion


    #region References to External GO
    //protected GameObject playerGO;
    protected Ship targetShip;
    #endregion



    // Use this for references
    protected void Awake()
    {
        aiShip = GetComponent<Ship>();
        targeting = GetComponent<Targeting>();
        weapon = GetComponent<WeaponFire>();
    }   

    // Use this for initialization
    protected void Start()
    {
        tendencyToFire = baseTendencyToFire;
    }

    // Update is called once per frame
    protected void Update()
    {
        // behavior shared by all states
            // ....

        // behavior specific to a single state
        switch (state)
        {
            case State.idle:
                UpdateIdleState();
                break;
            case State.travelling:
                UpdateTravelState();
                break;
            case State.fighting:
                UpdateFightState();
                break;
        }
    }




    // generally, decide what to do
    //   then calculate move (apply modifiers to heading)
    //   then apply move
    #region Update State Methods

    protected void UpdateIdleState()
    {
        if (HasAcquiredTarget())
        {
            state = State.fighting;
            return;
        }

        // If economic reason to move to waypoint (currently just waits a while then moves)
        // add waypoint, switch to travel state
        if (idleTimer > idleTimerBound) {
						//Debug.Log("Acquiring waypoints..");
						FindWaypointsInScene ();
                        //foreach (var x in waypoints)
                        //    Debug.Log("waypoint at " + x);
						idleTimer = 0f;
						state = State.travelling;
						return;
		} else if (gloriousLeader != null) {
			//Debug.Log ("Leader acquired by " + gameObject.name);
						idleTimer = 0f;
						state = State.travelling;
						return;
		}


        idleTimer += Time.deltaTime;
    }

    protected void UpdateTravelState()
    {
        if (HasAcquiredTarget())
        {
            state = State.fighting;
            return;
        }
        // Find path to end waypoint
            // Currently FCFS

        // Pick first waypoint or follow leader
        if (gloriousLeader != null)
        {
            heading = gloriousLeader.GetHeading(this);
            //Debug.Log(name + " following to " + heading);
        }
        else
        {

            if ((waypoints.Count != 0) && (waypoints[waypointIndex] != heading))
            {
                heading = GetNextWaypoint();
            }

            // If ship reached a waypoint, set next waypoint
            float distance = (heading - transform.position).magnitude;
            //Debug.Log(heading + " - " + transform.position + " = " + distance);
            if (distance <= optimalRange && waypoints.Count > 1)
            {
                RemoveCurrentWaypoint();
                if (waypoints.Count <= 0) // If ship has arrived at end waypoint
                {
                    // set state to idle
                    state = State.idle;
                }
                else
                    heading = GetNextWaypoint();
            }
        }
        // Modify heading to avoid collisions
        heading += ModifyForCollisionAvoidance();
        //Debug.Log(name + " actually moving to " + heading);
        // Move Towards end waypoint
        MoveTo(heading);
    }
    
    // For ships that aren't fighters, fight means Run Away!
    virtual protected void UpdateFightState()
    {
        //Debug.Log("AI's target:  " + targetShip.name);

        if (targetShip != null)
        {
            // Not a fighter; run away
            if (maneuverTimer > maneuverTimerBound)
            {
                heading = Flee();
                maneuverTimer = 0;
            }

            // Check for collisions
            if (maneuverTimer > maneuverTimerBound)
            {
                heading += ModifyForCollisionAvoidance();
                maneuverTimer = 0;
            }

            // Activate shield when target is looking at this
            if (IsFacing(targetShip.transform.forward, transform.position))
                aiShip.Shield(true);
            else
                aiShip.Shield(false);



            maneuverTimer += Time.deltaTime;

            // Move to that point
            MoveTo(heading);

            // Update timer to prevent setting new maneuvers every frame


            // Fire if AI thinks it should
            FireLasers();
        }
        else
            state = State.travelling;

    }
	
    #endregion


    #region Helper Methods

    // Shoot if facing target
    protected void FireLasers()
    {
        if (IsFacing(transform.forward, targetShip.transform.position)
                && Random.Range(0f, 1f) > (tendencyToFire-1))
        {
            //Debug.Log("should be firing");
            weapon.Fire();
        }
    }

    // Set heading to target ship (or a point of interception)
    // Assumes the target is a ship
    protected Vector3 Seek()
    {
        // Set heading to target ship (could be more intelligent)
        return targeting.currentTarget.position;
    }

    // Set heading to anywhere that moves ship out of targets view
    // Assumes the target is a ship
    protected Vector3 Maneuver()
    {
        return -targeting.currentTarget.position + Vector3.Cross(targeting.currentTarget.position, targetShip.transform.forward);
    }

    // Set heading to direction to negative vector towards enemy
    // Assumes the target is a ship
    protected Vector3 Flee()
    {
        return (- targeting.currentTarget.position);
    }

    protected bool HasAcquiredTarget()
    {
        if (targeting && targeting.targetableHostiles.Count > 0 && targetShip == null)
        {
            targeting.NextHostile();
            if (targeting.currentTarget)
                targetShip = targeting.currentTarget.GetComponent<Ship>();
            //Debug.Log(name + " targeting has acquired target " + targetShip.name + " of faction " + targetShip.tag);    
            return targetShip != null;
        }
        else
            return true;
    }

//	// Called by a friendly entering the LOS collider?
//	// CAUTION:  Race condition; possibly concurrent with another AI
//	protected void AttemptJoinFlock()
//	{
//		if(ShouldJoin())
//		{
//			var newMate = x.gameObject.GetComponent<BasicAi>();
//			if(newMate)
//				flockMates.Add(newMate);
//
//			//pick a leader (ensure 1)
//			var possibleLeader = newMate.leader;
//			if(possibleLeader)
//				leader = possibleLeader;
//			else
//				leader = newMate.DecideLeader();
//			 
//			return true;
//		}
//		else
//			return false;
//	}
//
//
//	// If returns an AI, set that as the leader
//	// if returns become the leader by ?remaining in travel state
//	public BasicAi DecideLeader()
//	{
//		// Ensure only one of the possibly two AI calling this becomes the leader
//	}


    //Returns the direction you need to move to avoid collision
    protected Vector3 ModifyForCollisionAvoidance()
    {
        Ray forward = new Ray(transform.position, aiShip.velocity);
        RaycastHit hit;
        if (Physics.Raycast(forward, out hit, rangeOfCollisionAvoidance))
        {
            //Debug.Log("Avoiding collision...");

            Ship hitShip = hit.collider.gameObject.GetComponent<Ship>();
            if (hitShip) // if it was a ship
            {
                return (-hitShip.velocity.normalized); // move whatever direction they're not moving
            }
            else // not a ship; avoid based on collider bounds
            {
                var angle = Vector3.Angle(hit.point, hit.collider.bounds.center);
                var cross = Vector3.Cross(hit.point, hit.collider.bounds.center);
                angle = (cross.y > 0) ? 30 : -30;
                return new Vector3(angle, 0, 0);
            }
        }
        else // no need to avoid collision
            return Vector3.zero;
    }

    // Return true if heading is pointing towards position of other entity
    protected bool IsFacing(Vector3 forwardDirection, Vector3 otherPosition)
    {
        //var result = Mathf.Acos(Vector3.Dot(forwardDirection.normalized, otherPosition.normalized));
        //Debug.Log("Acos(Dot(v1,v2)) returns: " + result);
        var result = Vector3.Angle(forwardDirection, otherPosition);
        //Debug.Log("Vector3.Angle(v1,v2) returns: " + result);
        return result < 70f;
    }

    // Move to a point in space
    public void MoveTo(Vector3 heading)
    {
        if ((heading - transform.position).magnitude < 10)
            aiShip.rotate_until(Quaternion.LookRotation(Vector3.one, Vector3.up));
        else
        {
            //Debug.Log("Rotating towards");
            aiShip.rotate_until(Quaternion.LookRotation(heading-transform.position, Vector3.up));
        }

        float distance = (heading - transform.position).magnitude;
        if (distance < minimumRange)
            aiShip.accelerate_until(aiShip.MAX_NEGATIVE_SPEED);
        else if (distance > optimalRange)
            aiShip.accelerate_until(aiShip.MAX_SPEED);
        else // breathing room
            aiShip.accelerate_until(0f);
    }

    // Do more sophisticated things later
    virtual public void FindWaypointsInScene()
    {
        var temp = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (var x in temp)
        {
            waypoints.Add(x.transform.position);
        }
    }

    // Remove the waypoint from the current set of waypoints
    void RemoveCurrentWaypoint()
    {
        waypoints.RemoveAt(waypointIndex);
        //waypoints.RemoveAt(0);
    }

    //Get next waypoint
    Vector3 GetNextWaypoint()
    {
        waypointIndex = Random.Range(0, waypoints.Count-1);
        var point = waypoints[waypointIndex];
        //Debug.Log(name + " chose waypoint: " + point);
        return point;
        //return waypoints[0];
    }

    #endregion



}
