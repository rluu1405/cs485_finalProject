//current this will only work for one equipment, redesign to make this work for multiple items
using UnityEngine;
using System.Collections;

public class equipmentScript : MonoBehaviour {

	bool equipped;
	//public Transform equipmentSpawn;
	public GameObject equipment;
	// Update is called once per frame
	void Start()
	{
		equipped = false;
		//equipment.SetActive (false);
	}

	public void equipShip()
	{
		if(!equipped)
		{
			equipment.SetActive(true);
			equipped = true;
		}
		else
		{
			equipment.SetActive(false);
			equipped = false;
		}
	}
}
