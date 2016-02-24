using UnityEngine;
using System.Collections;

public class Flocker : MonoBehaviour 
{

    public GameObject FlockPoint;
    public Vector3 FollowOffset;
    public float FollowSpeed = 0.01f;
    Vector3 FollowDistance;

    BasicAi Follower;

    void Start()
    {
        Follower = gameObject.GetComponent<BasicAi>();
        FollowDistance = FlockPoint.transform.position + FollowOffset;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Follower.state == BasicAi.State.idle) StartCoroutine("Follow");
	}


    IEnumerator Follow()
    {
        if (!(Follower.state == BasicAi.State.idle)) StopCoroutine("Follow");  //basic ai not idle or w/e

        FollowDistance = FlockPoint.transform.position + FollowOffset;
        Follower.MoveTo(FollowDistance);  // havent tested yet
       // transform.position = Vector3.Lerp(transform.position, FollowDistance, Time.deltaTime*FollowSpeed);
        yield return null;
    }
}
