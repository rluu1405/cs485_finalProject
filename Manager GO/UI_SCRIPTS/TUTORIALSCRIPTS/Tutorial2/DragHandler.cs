using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static GameObject itemBeginDragged;
	Vector3 startPosition;
	Transform startParent;
	
	//for changing player items
	GameObject gm;
	GameObject player;
	GuiManager gui;
	Player p;
	
	bool playerExist;
	public int itemID;
	public int itemValue;
	public Item tradeItem;
	public int trId;
	
    bool inPlayer;
    Transform previousPosition;


	void Start()
	{
        inPlayer = false;
		Invoke ("findPlayer", 1);
		Invoke ("test", 2);
       // previousPosition.position = transform.parent.position;
	}
	
	void findPlayer()
	{
		gm = GameObject.FindGameObjectWithTag("GameManager");
		gui = gm.GetComponent<GuiManager>();
		player = GameObject.FindGameObjectWithTag("Player");
		p = gm.GetComponent<Player> ();
	}
	
	void test()
	{
		itemID = tradeItem.ItemId;
		itemValue = tradeItem.ItemValue;
		//print ("item id is " + itemID);
		
	}
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		itemBeginDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;

	}
	
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
		gui.displayCurrentItem (tradeItem.ItemValue);
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeginDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
    
        //inPlayer = true;
        if (transform.parent == startParent)
		{
            //Debug.Log("FUCK");
			transform.position = startPosition;
		}
       // Debug.Log("checking equp");
		checkEquip ();
		gui.displayCurrentItem (0);
	}
	
	void checkEquip() 
	{
		if(transform.parent.CompareTag("equipWeap") && inPlayer)
		{
			print ("this is the equip slot");
			equip ();
			//Invoke ("equip", 1);
		}
		else if (transform.parent.CompareTag("traderInven") && inPlayer)
		{
			print("player is selling to " + trId);
			EconomyManager.Economy.SpawnedTraders[trId].Trade(ref p, tradeItem, false);
			Destroy(this.gameObject);
		}
        else if (transform.parent.CompareTag("playerInven") && !inPlayer)
        {
                //inPlayer = true;
                //Debug.Log("trying to buy");
                // if (!EconomyManager.Economy.SpawnedTraders[trId].Trade(ref p, tradeItem, inPlayer))
                // {
                //Debug.Log("ttttttttt");
                //  inPlayer = false;
                // transform.parent.position = previousPosition.position;
                // }

			//print("this is traders inventory");
			//Invoke ("sellTrade", 1);
			print("player is selling to " + trId);

			if(transform.parent.CompareTag("equipWeap") && !inPlayer)
			{
				print ("YOU BROKE!!");
				if(!EconomyManager.Economy.SpawnedTraders[trId].Trade(ref p, tradeItem, true))
				{
					
					gui.replaceItem(tradeItem);
					Destroy(this.gameObject);
				}
				else
				{
					inPlayer = true;
				}
			}
			else
			{
				if(!EconomyManager.Economy.SpawnedTraders[trId].Trade(ref p, tradeItem, true))
				{
					print ("YOU BROKE!!");
					gui.replaceItem(tradeItem);
					Destroy(this.gameObject);
				}
				else
				{
					inPlayer = true;
				}
			}

		}
	}

	void equip()
	{
		//there is a syncing issue with the equiping
		//owned object switching to player does not register
		if (player != null) 
		{
			print("player is here (DragHandler)");
			
			//maybe use a switch statement
			if(tradeItem.ItemId >= 100 && tradeItem.ItemId <= 104)
			{
				//print (player.GetComponent<WeaponFire> ().equippedWeaponId);
				player.GetComponent<WeaponFire> ().EquipWeapon (tradeItem.ItemId);
				//print (player.GetComponent<WeaponFire> ().equippedWeaponId);
				print ("equipped (DragHandler)");
			}
		} 
		
	}


}
