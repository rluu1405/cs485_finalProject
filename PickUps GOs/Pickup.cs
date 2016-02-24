using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour 
{


    public int Value;
    public int LootId;

	// Use this for initialization
	void Start () 
    {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {

        }

    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
