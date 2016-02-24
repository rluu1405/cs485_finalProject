using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {




	// Use this for initialization
    //void Start () 
    //{
    //       //might want to change scale randomly or some shit
    //}
	
	// Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
    }
}
