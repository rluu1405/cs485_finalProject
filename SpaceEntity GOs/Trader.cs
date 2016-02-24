using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UI;

public class Trader : Ship {
	
	public int TraderId = 0;
	//[HideInInspector]
	//public int BlackMarketMoney;
	//[HideInInspector]
	//public int Cash;
	
	bool Moving;
	GameObject P;
	Player ThePlayer;
	
	[HideInInspector]
	public bool InTradeReach = false;
	[HideInInspector]
	public bool playerAcceptTrade = false;
	
	//======================================
	private GameObject manager;
	private GuiManager gui;
	bool playerAlive;
	Ship pl;
	int cashGain;
	//======================================
	
	//public static Trader tradeNPC;
	new void Awake()
	{
		BlackDollar = (int)Random.Range(2, 4);
		Cash = (int)Random.Range(2000, 5000);
		isShielded = true; // merchants always have their shield up
		
		manager = GameObject.Find ("MANAGERS");
		gui = manager.GetComponent<GuiManager>();
		
		
		
		
		//gui.fillInventory ();
	}
	
	void Start()
	{
		//Invoke ("findPlayer", 3);
		
		P = GameObject.FindGameObjectWithTag("GameManager"); // more race condition bull shit, maybe just get player from gui
		ThePlayer = P.GetComponent<Player> ();
		if (ThePlayer == null) {
			Debug.Log ("player not found.");
			
		}
		//Invoke ("testPlayer", 1);
	}
	
	void testPlayer()
	{
		
		if(P != null)
		{
			
			//P.GetComponent<Player>().test();
			print ("player is not null");
			cashGain = pl.Cash;
			print ("testing player alive " + pl.Cash + " from local " + cashGain);
			
		}
		else
		{
			print ("player is null");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player" && Vector3.Distance(transform.position, other.transform.position) < 170)
		{
			Debug.Log("this trader sees player " + TraderId);
			InTradeReach = true;
			//playerResponse ();
			gui.SetTradeResponse (InTradeReach, TraderId);
			//StartCoroutine("CheckForTrading");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
       
		if(other.gameObject.tag == "Player" && Vector3.Distance(other.transform.position, transform.position) > 50)
		{
			InTradeReach = false;
			//playerResponse();
			gui.SetTradeResponse (InTradeReach, TraderId);
			
		}
	}
	
    //IEnumerator CheckForTrading()
    //{
    //    while(InTradeReach)
    //    {
    //        yield return null;
    //    }
		
    //}
	
	
	// Update is called once per frame
	new void Update () 
	{
		
		//if (health < 0)
		//{
		//    Killed();
		//}
		//RegenerateShield();		
		
	}
	
	public bool Trade(ref Player p, Item TradeItem, bool Buying) // item or id param, w/e is better
	{
        if (Buying)
		{
			//Debug.Log("buying: " + TradeItem.name + " " + TradeItem.OnBlackMarket + " " + TradeItem.ItemValue);
            //Debug.Log("player has: " + p.currentShip.BlackDollar + "black dollars");

			if (TradeItem.OnBlackMarket) 
            {
				if (p.currentShip.BlackDollar >= TradeItem.ItemValue) 
                { // have enuf money
					//Debug.Log ("THE player2 blacker: " + p.currentShip.BlackDollar);
					//Debug.Log ("THE trader2 cash: " + BlackDollar);

					UpdateCargosB(TradeItem, ref p);
					return true;
				}
			} 
            else if (p.currentShip.Cash >= TradeItem.ItemValue) 
            {
                //Debug.Log("buying1: " + p.currentShip.Cash);
				UpdateCargosB(TradeItem, ref p);
				return true;
			}
            //Debug.Log("returing false not enuf doeshhh" + TradeItem.ItemValue);
            return false; // not enuf monies
		}
		else // selling
		{
            //Debug.Log("seling: item: " + TradeItem.ItemId + " bb status " + Cash + " player cash:" + p.currentShip.Cash);
			if(TradeItem.OnBlackMarket)
			{
                //Debug.Log("seling1: item: " + TradeItem.ItemId + " player cash:" + p.currentShip.Cash);
				if (BlackDollar >= TradeItem.ItemValue) // if trader has enough money to give
				{
					BlackDollar -= TradeItem.ItemValue; // take from trader
					p.currentShip.RemoveFromCargo(TradeItem.ItemId);
                    p.currentShip.BlackDollar += TradeItem.ItemValue;
                    //Debug.Log("after seling: item: " + TradeItem.ItemId + " player cash:" + p.currentShip.Cash);
                   // AddToCargo(TradeItem.ItemId);
                    return true;
				}
                else return false;
			}
			else if (Cash >= TradeItem.ItemValue) // can buy from you, can sell to him
			{			
				Cash -= TradeItem.ItemValue; // take money from trader
				p.currentShip.Cash += TradeItem.ItemValue;
                p.currentShip.RemoveFromCargo(TradeItem.ItemId);
                //Debug.Log("afterss seling: item: " + TradeItem.ItemId + " player cash:" + p.currentShip.Cash);
                return true;
			}
			else return false; // trader doesnt have the dosh
		}
		return false; // final case, something went wrong
		
	}
	

	
	void UpdateCargosB(Item I, ref Player p)
	{
        if (I.OnBlackMarket)
        {
            p.currentShip.BlackDollar -= I.ItemValue;
            BlackDollar += I.ItemValue;

            //Debug.Log("after updated cargos the player CARGO HAS : ");
            PrintCargo(ref p.currentShip);
            //Debug.Log("after updated cargos the player has: " + p.currentShip.Cash + " " + p.currentShip.BlackDollar);
        }
        else
        {
            p.currentShip.Cash -= I.ItemValue;
            Cash += I.ItemValue;
        }
        if (I is ShipTitle) p.pocketedShips.Add(EconomyManager.Economy.GetItem(I.ItemId) as ShipTitle);
        else p.currentShip.AddToCargo(I.ItemId);  
   
        RemoveFromCargo(I.ItemId);
	}

    void PrintCargo(ref Ship s)
    {
        for(int i = 0; i<s.cargoCount; i++)
        {
            Debug.Log(s.cargo[i] + " " );
        }
    }

	
	public override void Killed()
	{
		
	}
	
	public void OnGUI()
	{
		// just so it doesn't show up
	}
	
	void playerResponse()
	{
		if (Input.GetKeyDown (KeyCode.T) && playerAcceptTrade == false) 
		{
			playerAcceptTrade = true;
		} 
		else if(Input.GetKeyDown (KeyCode.T) && playerAcceptTrade == true)
		{
			playerAcceptTrade = false;
		}
	}
	
}
