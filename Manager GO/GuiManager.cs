using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/* Note:
 * make gui static
 * have a bool that signals trader ship to stop when the player request trade
 */

public class GuiManager : MonoBehaviour {
	
	//static class for everyone to see
	public static GuiManager guiMgr;
	
	private Transform primaryTarget; //target assigned from player script
	private GameObject gui; //grabs the UI gameObject
	[HideInInspector]
	public GameObject player;
	private GameObject nonTargetUI;
	
	//for calculating when the target indicator displays or not
	private Vector3 relativePos; 
	private float angle;
	private Image im;
	
	//for sub/secondary target
	/*
	public GameObject nonToi;
	private GameObject nt;
	private float i; //for tracking the number of instantiate
	private int tracking;
	public List<GameObject> subTar; //<--using new vs not using new??
	*/
	
	
	//for trader
	private GameObject tradeDisplay;
	private GameObject traderInventory;
	private Text tradeResponse;
	private Image tradeInventoryImage;
	private bool tradeAccept;
	private int tId;
	
	//private bool playerTradeAccept;
	private bool tradeInvenOpen; //us this to tell trade ship to stop
	
	//public Transform to;
	public float speed;
	
	//player
	private GameObject playerInventory;
	//private Image playerInventoryImage;
	private GameObject playerEquip;
	//private Image playerEquipmentImage;
	//private GameObject hud;
	public bool playerInvenOpen;
	
	//testing inventory
	
	//when moving input to player script, move these too
	PlayerMovement movement;
	
	//testing filling out table, try to find a better implementation of this
	public GameObject ylasericons;
	public GameObject glasericons;
	public GameObject blasericons;
	public GameObject rlasericons;
	public GameObject plasericons;
	
	public GameObject weedicons;
	public GameObject methicons;
	public GameObject heroinicons;
	public GameObject eicons;
	
	public GameObject shipPapericons;
	public GameObject shieldIcon;

	GameObject temp;
	Item fromTrader;
	int traderCargoCount;

	//bool mouseClick;
	//display item info
	GameObject displayValue;
	Text valueText;
	string value;


	void Awake()
	{
		//targeting
		gui = GameObject.Find ("PrimaryTarget"); //target indicator that will follow the target of choice
		im = gui.GetComponent<Image>();
		player = GameObject.FindGameObjectWithTag ("Player");
		
		
		//trader
		//trader = GetComponent<Trader> ();
		tradeDisplay = GameObject.Find ("tradeWithMe");
		traderInventory = GameObject.Find ("traderInventory");
		tradeResponse = tradeDisplay.GetComponent<Text> ();
		tradeResponse.text = "Press T to trade";
		
		//player
		playerInventory = GameObject.Find("playerInventory");
		//playerInventoryImage = playerInventory.GetComponent<Image>();
		
		//playerEquip = GameObject.Find ("playerEquipment");
		//playerEquipmentImage = playerInventory.GetComponent<Image>();
		
		displayValue = GameObject.Find ("currentPrice");
		
		//when moving input to player script, move these too
		movement = GetComponent<PlayerMovement>();
	}
	
	void Start () 
	{
		//Screen.lockCursor = true; // Dont do this, force player to use fullscreen.  or strongly suggest.
		//Screen.showCursor = false; // hide cursor
		
		//targeting
		im.enabled = false;
		
		
		
		//trader
		traderInventory.SetActive (false);
		tradeResponse.enabled = false;
		tradeInvenOpen = false;
		//playerTradeAccept = false;
		
		
		//player
		
		playerInventory.SetActive (false);
		playerInvenOpen = false;
		
		//when moving input to player script, move these too (Put them all in the same place so controlling these is sane   )
        //movement.EnableMouseControl(true);
        //Screen.showCursor = false;
		
		//fillInventory (); 
		//display text
		valueText = displayValue.GetComponent<Text> ();

	}
	
	void Update()
	{

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		PrimaryTarget ();
		traderMessageDisplay ();
		tradeInv ();//currently test, better implimentation will be made
		playerInv ();//currently test, better implimentation will be made
		
		
	}
	
	
	public void SetCurrentTarget(Transform transform)
	{	
		primaryTarget = transform;
	}
	
	public void SetTradeResponse(bool tradeRe, int id) //gives the signal if the player is close to the ship to trade
	{
		tradeAccept = tradeRe;
		tId = id;
		print ("trader id from gui " + id);
		
	}
	
	public void MouseEnable(bool enable)
	{
		//Screen.showCursor = enable;
	}
	
	void findTrader()
	{
		for(int i = 0; i < 3; i++)
		{
			if(EconomyManager.Economy.Traders[i].InTradeReach == false)
			{
				//traderId = EconomyManager.Economy.Traders[i].TraderId;
				print ("THIS IS THE TRADER WE WANT " + EconomyManager.Economy.Traders[i].TraderId);
			}
			
			//traderId = EconomyManager.Economy.Traders[i].TraderId;
			//print ("THIS IS THE TRADER WE WANT " + traderId);
		}
	}
	
	//Target of interest is selected
	void PrimaryTarget()
	{
		if (primaryTarget) 
		{
			im.enabled = true;
			gui.transform.position = Camera.main.WorldToScreenPoint (primaryTarget.position);
			displayTarget();
		}
		else if (im)
			im.enabled = false;
	}
	
	//targets that are surrounding the player 
	
	void displayTarget()
	{
		if (primaryTarget != null) {
			
			//Debug.Log (primaryTarget.position);
			
			relativePos = primaryTarget.position - Camera.main.transform.position;
			
			angle = Vector3.Angle(relativePos, Camera.main.transform.forward); 
			
			if(angle < 45.0f)
			{
				
				//Debug.Log("visible");
				
				im.enabled = true;
				
				//visible = true;
				
			}
			else
			{		
				//Debug.Log ("not visible");
				
				im.enabled = false;
				
				//visible = false;		
			}
		}
	}
	
	//trader temporary in use
	void traderMessageDisplay()
	{
		if(tradeAccept)
		{
			tradeResponse.enabled = true;
			
		}
		else
		{
			tradeResponse.enabled = false;
		}
	}
	
	
	void tradeInv() //testing the inventory display, better implementation will be made
	{
		//move the input key into the trader, and use a bool
		
		if(Input.GetKeyDown(KeyCode.T) && tradeInvenOpen == false && tradeAccept == true) //tradeAccept == true
		{
			//print ("trader id in gui is " + traderId);
			tradeInvenOpen = true;
			tradeResponse.enabled = false;
			playerInventory.SetActive(true);
			traderInventory.SetActive(true);
			fillInventory();
            movement.EnableMouseControl(false);
            Screen.showCursor = true;
		}else if(Input.GetKeyDown(KeyCode.T) && tradeInvenOpen == true){
			
			//print ("trader id in gui is " + traderId);
			tradeInvenOpen = false;
			tradeResponse.enabled = true;
			playerInventory.SetActive(false);
			traderInventory.SetActive(false);
			removeInventory();
            movement.EnableMouseControl(true);
            Screen.showCursor = false;
		}
	}
	
	
	void playerInv() //testing the inventory display, better implementation will be made
	{
		if(Input.GetKeyDown(KeyCode.I) && playerInvenOpen == false){
			
			playerInvenOpen = true;
			//playerInventoryImage.enabled = true;
			playerInventory.SetActive(true);
			//when moving input to player script, move these too
			movement.EnableMouseControl(false);
            Screen.showCursor = true;
			
		}else if(Input.GetKeyDown(KeyCode.I) && playerInvenOpen == true){
			
			playerInvenOpen = false;
			//playerInventoryImage.enabled = false;
			playerInventory.SetActive(false);
			//when moving input to player script, move these too
			movement.EnableMouseControl(true);
            Screen.showCursor = false;
		}
		
	}
	
	public void fillInventory()
	{
		foreach (Transform emptySlot in traderInventory.transform) 
		{
			//print ("TEST Parent " + traderInventory.transform.childCount);
			//print ("TEST Child " + emptySlot.childCount);
			
			foreach(Transform icon in emptySlot)
			{
				if(icon.childCount > 0)
				{
					print("there is something already there");
				}
				else
				{
                   // print(EconomyManager.Economy.SpawnedTraders[tId].cargo.Length);
					//only create based on how many cargo the trader has
                    if (traderCargoCount != EconomyManager.Economy.SpawnedTraders[tId].cargoCount) //traderCargoCount != EconomyManager.Economy.Traders[tId].cargo.Length
					{
						int cargoID;


                        print("crgo count for trader " + tId + " " + EconomyManager.Economy.SpawnedTraders[tId].cargoCount);
						cargoID = EconomyManager.Economy.SpawnedTraders [tId].cargo[traderCargoCount]; //change the traders so it gets the traderId from trader
						fromTrader = EconomyManager.Economy.GetItem(cargoID);
						
						//creates the new cargo
						//temp = (GameObject) Instantiate(shipPapericons, traderInventory.transform.position, traderInventory.transform.rotation);
						
						createIcon();
						
						temp.transform.parent = icon;
						temp.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
						
						DragHandler dh = temp.AddComponent<DragHandler>();
						temp.GetComponent<DragHandler>().tradeItem = fromTrader;
						temp.GetComponent<DragHandler>().trId = tId;
						traderCargoCount++;
						//print (traderCargoCount);
					}
					//need to grab the items
					
				}
			}
		}
		traderCargoCount = 0;
	}
	
	void removeInventory()
	{
		foreach (Transform emptySlot in traderInventory.transform) 
		{
			//print ("TEST Parent " + traderInventory.transform.childCount);
			//print ("TEST Child " + emptySlot.childCount);
			
			foreach(Transform icon in emptySlot)
			{
				if(icon.childCount > 0)
				{
					foreach(Transform ic in icon)
					{
						
						Destroy(ic.gameObject);
					}
				}
				//need to grab the items
			}
		}
	}

	public void replaceItem(Item oldItem)
	{
		fromTrader = oldItem;
		
		foreach (Transform emptySlot in traderInventory.transform) {
			//print ("TEST Parent " + traderInventory.transform.childCount);
			//print ("TEST Child " + emptySlot.childCount);
			
			foreach (Transform icon in emptySlot) 
			{
				if (icon.childCount > 0) 
				{
					//print ("there is something already there");
				} 
				else {
					// print(EconomyManager.Economy.SpawnedTraders[tId].cargo.Length);
					//only create based on how many cargo the trader has
				
					createIcon ();
					print ("replacing item");
					temp.transform.parent = icon;
					temp.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
					
					DragHandler dh = temp.AddComponent<DragHandler> ();
					temp.GetComponent<DragHandler> ().tradeItem = fromTrader;
					temp.GetComponent<DragHandler> ().trId = tId;
					break;
					
				}
			}
		}
	}
	
	void createIcon () //maybe change name later if it works
	{
		int tempId;
		tempId = fromTrader.ItemId;
		print("the item is " + fromTrader.ItemId);
		switch (tempId)
		{
			//weapons
		case 100:
			temp = (GameObject) Instantiate(ylasericons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
		case 101:
            temp = (GameObject)Instantiate(rlasericons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
		case 103:
			temp = (GameObject) Instantiate(blasericons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
		case 102:
            temp = (GameObject)Instantiate(glasericons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
		case 104:
			temp = (GameObject) Instantiate(plasericons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
			
			//drugs
		case 420:
			temp = (GameObject) Instantiate(weedicons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
		case 421:
			temp = (GameObject) Instantiate(methicons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
		case 422:
			temp = (GameObject) Instantiate(heroinicons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
		case 423:
			temp = (GameObject) Instantiate(eicons, traderInventory.transform.position, traderInventory.transform.rotation);
			break;
			
			//ship paper?
		default:

			if(tempId >= 500 && tempId <= 509)
			{
				temp = (GameObject) Instantiate(shipPapericons, traderInventory.transform.position, traderInventory.transform.rotation);
			}
			else if(tempId >= 200 && tempId <= 202)
			{
				temp = (GameObject) Instantiate(shieldIcon, traderInventory.transform.position, traderInventory.transform.rotation);
			}

			break;
		}
	}
	public void displayCurrentItem(int v)
	{
		value = "Item Value: \t\t" + v.ToString ();
		valueText.text = value;
	}

}
