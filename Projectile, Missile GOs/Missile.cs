using UnityEngine;
using System.Collections;

public class Missile : Projectile {

    public float maneuverRate = 0.00001f;

    private Ship target;

    private Vector3 desiredDestination; //compensate for enemy velocity


    // Use this for references
    void Awake()
    {
        // DEBUG -- REMOVE THIS LATER
        var temp = GameObject.FindGameObjectWithTag("Nanite");

        if(temp != null)
            target = temp.GetComponent<Ship>();
    }

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (deathTime < 0.0f)
        {
            Debug.Log("missile ran out of fuel");
            Destroy(gameObject);
        }

        if (target != null)
        {
            Vector3 relativePos = target.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * maneuverRate);
        }
        
        rigidbody.velocity = (transform.forward * bulletSpeed);



        --deathTime;
	}
}
