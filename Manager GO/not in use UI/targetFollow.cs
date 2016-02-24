//there is still the problem of when the player turns 180 the target indicator is still visible
//mathf.infinity may need to be changed to a limited range
//may switch out the point to target with raycast to using maybe colliders
//bool targetAcquire will be used for the distance display that is tied with the target
//need to implement a toggle for target reticule, turning off when the user doesnt need to target anymore
//reticule is still kind of glitchy

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class targetFollow: MonoBehaviour {

	public LayerMask targetMask = -1;
	public float scaleMultiplier = 1.0f;

	public Transform target = null; //don't add anything from the inspector

	public bool targetAcquire; //if the target is currently selected

	Image im;

	void Start () {
		targetAcquire = false;
		im = GetComponent<Image> ();
		im.enabled = false; //will have the targeting reticule off at the start
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, targetMask) && 
		   Input.GetKey(KeyCode.T))
		{
			target = hit.transform;
		
			//fixed the glitchiness of the target reticule
			if(!targetAcquire && !im.enabled)
			{
				im.enabled = true; //turns on the targeting 
		
			}
		}

		if(target != null)
		{
			transform.position = Camera.main.WorldToScreenPoint (target.position); //transform position of the target sprite
			targetAcquire = true;
		}
		else
		{
			targetAcquire = false;

			//if the target is destroyed the reticule will turn off
			if(!targetAcquire && im.enabled)
			{
				im.enabled = false;
			}
		}

		//==============================================================================
		//testing camera frustum

		if(target != null)
		{
			if(target.renderer.isVisible)
			{
				Debug.Log ("is visible");

				if(!im.enabled)
				{
					im.enabled = true;
				}

			}
			else
			{
				Debug.Log ("not visible");
				im.enabled = false;
			}
		}


	}
}


