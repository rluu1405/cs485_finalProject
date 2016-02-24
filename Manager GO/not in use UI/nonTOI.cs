using UnityEngine;
using System.Collections;

public class nonTOI : MonoBehaviour {

	public GameObject nonTargetOfInterest = null;
	private bool mainTarget;

	// Use this for initialization
	void Start () {
		mainTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Camera.main.WorldToScreenPoint (nonTargetOfInterest.transform.position);
	}
}
