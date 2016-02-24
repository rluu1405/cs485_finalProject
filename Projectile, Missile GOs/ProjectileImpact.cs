/* Attach this to particle system:  Instantiate effects of projectile impact */

using UnityEngine;
using System.Collections;

public class ProjectileImpact : MonoBehaviour {

	public float dtime = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(dtime != 0.0f)
		{
			dtime--;
		}
		else
		{
			Destroy(gameObject);
		}

	
	}
}
