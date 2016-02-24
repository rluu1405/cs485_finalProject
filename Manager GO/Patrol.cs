using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Patrol : BasicAi {

	// Use this for initialization

    int Index;
    public List<Transform> Waypoints;
    Vector3 Direction;
    public float Speed;
    float PatrolSpeed;
    float WaitTime;
    bool Detour;
    public float ArrivalOffset = 20f;

    new void Awake()
    {
        Index = 0;
        if (Waypoints.Count <= 0)
            Debug.Log("Patrol script has no waypoints set in inspector");
        Detour = false;
    }
	void Start () 
    {
        Direction = Waypoints[Index].position - transform.position;
        FindWaypointsInScene();   
	}

    override public void FindWaypointsInScene()
    {
        if(Waypoints.Count > 0)
            StartCoroutine("TradePatrol");
    }

    IEnumerator TradePatrol()
    {

        while (Vector3.Distance(transform.position, Waypoints[Index].position) > ArrivalOffset) // while distance between them is greater than 5
        {
            PatrolSpeed = Speed /Mathf.Abs(Direction.x);
            Direction = Waypoints[Index].position - transform.position;

            Quaternion CurrentRot = transform.localRotation;
            Quaternion Towards = Quaternion.LookRotation(Direction);
            transform.localRotation = Quaternion.Slerp(CurrentRot, Towards, Time.deltaTime*0.5f);

            transform.position = Vector3.Lerp(transform.position, Waypoints[Index].position, Time.deltaTime * PatrolSpeed);
   
            yield return null;
        }


        if (Index == Waypoints.Count - 1) Index = 0;
        else Index++;

        if (Detour)
        {
            WaitTime = 0f;
        }

        else WaitTime = 2f;
        yield return new WaitForSeconds(WaitTime);  // wait at waypoint..after 2 seconds go to next
        StartCoroutine("TradePatrol");

    }

    void OnTriggerEnter(Collider other)
    {
       

    }
	// Update is called once per frame
	new void Update () 
    {
	
	}
}
