using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class traderTest : MonoBehaviour {

	public GameObject testInstantiate;
	GameObject trader;
	int i;
	// Use this for initialization

	void Start () {

		trader = GameObject.Find ("trader");
		test ();
	}

	void test()
	{

		if(trader != null)
		{
			print("trader exist");
		}
		else
		{
			print("there is no trader");
		}

		//there should be only 20 prints which is ths slots in the trader inventory
		//if there is an item inside the slot and is inactive the childCount will still register as 1
		foreach(Transform trade in trader.transform) 
		{
			if(trade.childCount > 0)
			{
				//print (trade.GetComponentInChildren<DragHandler>().itemID);
			}
			else
			{
				//print ("false");
				populate (trade);
			}


		}
	}

	void populate(Transform tradeParent)
	{
		GameObject temp;

		temp = (GameObject) Instantiate (testInstantiate, transform.position, transform.rotation);
		temp.transform.parent = tradeParent;
		temp.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f); //use this to rescale the items
	}
}
