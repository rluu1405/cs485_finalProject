using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeting : MonoBehaviour {


    internal List<Transform> targetableFriendlies = new List<Transform>();
    int friendIndex;
    internal List<Transform> targetableHostiles = new List<Transform>();
    int hostileIndex;
    internal Transform currentTarget;


    #region Attached GO References
    Ship ship;
    #endregion

    #region External GO References
    GameManager gm;
    InputManager input;
    #endregion




    // Use this for references
    void Awake()
    {
        var temp = GameObject.FindGameObjectWithTag("GameManager");
        gm = temp.GetComponent<GameManager>();
        input = temp.GetComponent<InputManager>();
    }

    // Use this for initialization
    void Start()
    {
        targetableFriendlies = new List<Transform>();
        targetableHostiles = new List<Transform>();
    }

    void Update()
    {
        //Set currentTarget to null if the ship its targetting is destroyed
    }

    // Add targets on Line Of Sight "collision"
    void OnTriggerEnter(Collider col)
    {
        // If target is of friendly/neutral faction, add to list of friendlies
        var colReputation = gm.factions.GetReputation(tag, col.tag);
        //Debug.Log(name + ": " + tag + " likes " + col.tag + ": " + colReputation);
        if (colReputation == int.MaxValue) {
						// Do nothing if not a faction
				} else if (colReputation >= 0) {
						targetableFriendlies.Add (col.transform);
						//FriendlyAdded EVENT fired (caught by ai, ?flockLeader, ui)
				} else {  // Otherwise, add to list of hostiles
						targetableHostiles.Add (col.transform);
						//HostileAdded EVENT fired (caught by ai, ?flockLeader, ui)
				}

    }

    // Remove targets upon exiting Line Of Sight "collision"
    void OnTriggerExit(Collider col)
    {
            // Remove from the list of targetables but do NOT change the current Target
        var colReputation = gm.factions.GetReputation(tag, col.tag); ;
        if (colReputation == int.MaxValue)
        {
            // Do nothing if not a faction
        }
        else if (colReputation >= 0)
            targetableFriendlies.Remove(col.transform);
        else
            targetableHostiles.Remove(col.transform);
    }



    // Set currentTarget to a valid transform.
    // If not possible, let currentTarget remain NULL
    public void NextFriendly()
    {
        // Verify that element pulled from the list is not NULL
        do
        {
            // Wrap around the list
            if (friendIndex < targetableFriendlies.Count - 1)
                ++friendIndex;
            else if (targetableFriendlies.Count == 1)// equal to Count
                friendIndex = 0;
            else
                break;


            if (targetableFriendlies[friendIndex] == null) //if NULL, remove and move to next
                targetableFriendlies.RemoveAt(friendIndex);
            else
            {
                currentTarget = targetableFriendlies[friendIndex];
                break; //otherwise, you've acquired a valid transform, exit loop
            }
        }
        while (targetableFriendlies.Count != 0);
    }

    // Set currentTarget to a valid transform.
    // If not possible, let currentTarget remain NULL
    public void NextHostile()
    {
        // Verify that element pulled from the list is not NULL
        do
        {
            // Wrap around the list
            if (hostileIndex < targetableHostiles.Count - 1)
                ++hostileIndex;
            else if (targetableHostiles.Count == 1) // equal to Count
                hostileIndex = 0;
            else
                break;

            if (targetableHostiles[hostileIndex] == null) //if NULL, remove and move to next
                targetableHostiles.RemoveAt(hostileIndex);
            else
            {
                currentTarget = targetableHostiles[hostileIndex];
                break; //otherwise, you've acquired a valid transform, exit loop
            }
        }
        while (targetableFriendlies.Count != 0);
    }

    // possibly use this to acquire other things (with renderers!)
    public void RayCastAcquireTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            currentTarget = hit.transform.transform;
        }
    }

    // Cannot overload the == operator because relative distance not stored on each ship ... 
        // Need to figure out something for this rather than manually sorting a list (of all the things to sort manually..!)
    public void SortByDistance()
    {
        throw new System.NotImplementedException();
    }


    // From http://wiki.unity3d.com/index.php/Calculating_Lead_For_Projectiles
    // Under MIT license
    // Assumes the ships velocity will remain the same until impact 
    //********** Use d / 50 = r * t for the proper units
    public Vector3 CalculateInterceptPoint(
        Vector3 position, 
        Vector3 velocity, 
        float shotSpeed, 
        Vector3 targetPosition, 
        Vector3 targetVelocity)
    {
        Vector3 relativePosition = targetPosition - position;
        Vector3 relativeVelocity = targetVelocity - velocity;
        float time = InterceptTime(shotSpeed, relativePosition, relativeVelocity);
        return targetPosition + time * relativeVelocity;
    }

    private float InterceptTime(float shotSpeed, Vector3 relativePosition, Vector3 relativeVelocity)
    {
        float vSquared = relativeVelocity.sqrMagnitude;
        // If very close to zero, return 0
        if (vSquared < 0.001f)
            return 0f;


        float a = vSquared - shotSpeed * shotSpeed; // ???

        // Handle case where shotSpeed and relativeSpeed are the same
            // i.e. time = infinite
        if (Mathf.Abs(a) < 0.001f)
        {
            float time = -relativePosition.sqrMagnitude / (2f * Vector3.Dot(relativeVelocity, relativePosition));
            return Mathf.Max(time, 0f); // return targets current pos if no positive solns
        }

        float b = 2f * Vector3.Dot(relativeVelocity, relativePosition);
        float c = relativePosition.sqrMagnitude;
        float determinant = (b * b) - (4f * a * c);

        if (determinant > 0f) // two intercept paths
        {
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a);
            float t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); // both solns are positive
                else
                    return t1; // only positive soln is t1
            }
            else
                return Mathf.Max(t2, 0f); // return targets current pos if no positive solns
        }
        else if (determinant < 0f) // no solns -> no intercept point
            return 0f;
        else // one soln
            return Mathf.Max(-b / (2f * a), 0f); // return targets current pos if no positive solns
    }


}
