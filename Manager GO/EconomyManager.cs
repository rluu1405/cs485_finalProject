using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EconomyManager : MonoBehaviour {
	
	public static EconomyManager Economy;
    Dictionary<int, Item> ItemTable = new Dictionary<int, Item>();
	
	public List<Item> AllItems;
	public List<Trader> SpawnedTraders;
	
	// the id will point to the specific item in a DEFINED table that this script has
	// using this id it will get values such as...well value..rarity...weapon strength or mineral rarity or whatever
	
	public List<Trader> Traders; // more like merchant ships
	public List<Vector3> SpawnPoints; // location to spawn the merchant ships
	
	public readonly string blackMarketCurrencyName = "BlackDollar";
	public long blackMarketBaseAmount = 500;
	public readonly string currencyName = "Lumioun";
	public long marketBaseAmount = 10000;
	
	// For a true market, all goods must be kept track of by this script.
	// All goods are referenced from a table of all goods help by this script.
	// Using the lists of all goods, demand and supply can be monitored while applying our own variables for the market
	// such as disasters, DOW, market crashes ect.....
	//	GameObject P;
	//	Player ThePlayer;
	
	internal int Id = 0; // for merchants
	
	
	void Awake()
	{
		if(Economy == null)
		{
			Economy = this;
		}

		
	}
	
	void Start () 
	{
		FillItemTable();
		Invoke("SpawnTraders", 2);
	}
	
	
	void SpawnTraders()
	{
		foreach (Trader T1 in Traders)
		{
            Trader T;
			T = Instantiate(T1, SpawnPoints[Id], Quaternion.identity) as Trader; // should work with id....
			T.TraderId = Id;
			Id++;
			
			T.AddToCargo(AllItems[0].ItemId); // always has the yellow lazer to sell
			T.AddToCargo((int)Random.Range(101, 104)); // add a random weapon to cargo
			T.AddToCargo((int)Random.Range(200, 202)); // add random shield to cargo
			T.AddToCargo((int)Random.Range(500, 509)); // adds random shiptitle to cargo
			T.AddToCargo((int)Random.Range(500, 509));
			
			SpawnedTraders.Add(T);
		}
	}
	
	public void FillItemTable()
	{
		foreach(var I in AllItems)
		{
			ItemTable.Add(I.ItemId, I);
		}
	}
	
	
	public Item GetItem(int Id)
	{
		//Debug.Log("Item requested: " + Id);
		return ItemTable[Id];
	}
	
	
	
	
}
