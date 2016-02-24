    using UnityEngine;
using System.Collections;

public class DrugsSceneControl : MonoBehaviour 
{

    Player ThePlayer;
    GameObject P;

    public GameObject BlackCache;

    int Merchant1DrugCount = 0;
    int Merchant2DrugCOunt = 0;

    int NewDrugs = 0;
    int NewDrugs2 = 0;


    WarpGate warpGate;
    bool ShowPrompt = true;

    GameManager Gm;

    bool GotDrugs = false;
    bool SoldToMerchant1 = false;
    bool SoldToMerchant2 = false;
    bool DrugCount = false;

    int Pweed = 0;
    int Pdrug1 = 0;
    int Pdrug2 = 0;
    int Pmilk = 0;

    public float attackerSpawnTimerBound = 60f;
    public int attackerSpawnDistance = 400;
    public int nAttackersSpawned = 3;
    public string attackerType = "EarthFighters";
    float attackerSpawnTimer;

	// Use this for initialization
	void Start () 
    {
        Gm = GetComponent<GameManager>();
        ThePlayer = GetComponent<Player>();
        if(ThePlayer == null)
        {
            Debug.Log("cant find player");
        }
        Instantiate(BlackCache, new Vector3(1f, 1.7f, 74.7f), Quaternion.identity);

        var temp = GameObject.FindGameObjectWithTag("WarpGate");
        warpGate = temp.GetComponent<WarpGate>();
        if (warpGate == null)
            Debug.Log("Warpgate not found in Drugs Scene");

        Invoke("FillBlackTraders", 2.5f);

       // Gm.FACTION_SPAWN_LIMIT = 50;
        //Gm.factions.IncrementReputation("Nanite", "Player", -9999);

        StartCoroutine("BuyingDrugs");
        StartCoroutine("OccasionalAttackerSpawn");
	}

    IEnumerator OccasionalAttackerSpawn()
    {
        while (!warpGate.IsActive())
        {
            if (attackerSpawnTimer > attackerSpawnTimerBound && Gm)
            {
                Gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned, (uint)attackerSpawnDistance);
                attackerSpawnTimer = 0f;
            }

            attackerSpawnTimer += Time.deltaTime;
            yield return null;
        }
    }

    void FillBlackTraders()
    {
        foreach (var T in EconomyManager.Economy.SpawnedTraders)
        {
            T.AddToCargo(420);
            T.AddToCargo(420);
            T.AddToCargo(420);
            T.AddToCargo(423);
            T.AddToCargo(423);
            T.AddToCargo(422);
            T.AddToCargo(422);
            T.AddToCargo(422);
            T.AddToCargo(421);
            T.AddToCargo(421);
            T.AddToCargo(421);

            T.BlackDollar += 150;

        }

    }

    void OnGUI()
    {
        GUI.Box(new Rect(200, 5, 200, 100), "\"Favor\" Status");
        GUI.Label(new Rect(215, 40, Screen.width, Screen.height), "Drugs Obtained: " + DrugCount + "\n" + "Sold To Doug: " + SoldToMerchant1 + "\nSold TO Julian: " + SoldToMerchant2);

        if (ShowPrompt)
        {
            GUI.Box(new Rect(400, 10, 480, 200), "Peculiar Merchant:");
            GUI.Label(new Rect(410, 40, Screen.width, Screen.height), "Hello traveler.....looks like you have seen better days.\n  You looking to make some dosh?....I've got a job for you if interested\n......I got all this...\"product\" to move, but my buddies are beyond\n the nanite asteroid belt.  Serious dosh to be made from a simple transport.\n  **Pick up that cache and buy 3 spaceweed's, oops I mean canadian maple\n leaves, and 2 Milkyway meth's, I mean space candy,  from me and bring em\n to my two buddies.  Sell the space weed, I mean  canadian maple\n leaves, to my first bud, he is straight towards that planet, you can't miss him.\n  After him, go straight towards the red nebula and sell the milky way meths\n, I mean space candy,  to him.(NEAR THE WAYPOINT)**\n  You and me pal, comon what do you say? ");
        }

        if (ShowPrompt && GUI.Button(new Rect(550, 350, 110, 50), "Lets do it to it."))
        {
            ShowPrompt = false;
        }

        if (DrugCount && SoldToMerchant1 && SoldToMerchant2)
        {
            GUI.Box(new Rect(400, 20 , 120, 25), "Warpgate is open");
        }

    }
	// Update is called once per frame
	void Update () 
    {
        if(GotDrugs && SoldToMerchant1 && SoldToMerchant2)
        {
            warpGate.Activate();
        }
        if (EconomyManager.Economy.SpawnedTraders.Count > 0 && ThePlayer.currentShip)
        {
            if (Vector3.Distance(ThePlayer.currentShip.transform.position, EconomyManager.Economy.SpawnedTraders[1].transform.position) < 100 && !SoldToMerchant1)
            {
                CountDrugs(EconomyManager.Economy.SpawnedTraders[1]);
            }
            else if (Vector3.Distance(ThePlayer.currentShip.transform.position, EconomyManager.Economy.SpawnedTraders[2].transform.position) < 100 && !SoldToMerchant2)
            {
                CountDrugs(EconomyManager.Economy.SpawnedTraders[2]);
            }
        }
       
	}

    void CountDrugs(Trader T)
    {
        //Debug.Log("calling drug count " + Merchant1DrugCount + " 2 is " + Merchant2DrugCOunt);
        switch(T.TraderId)
        {
            case 1:
                {
                    for(int i = 0; i<T.cargoCount; i++)
                    {
                        if(T.cargo[i] == 420 || T.cargo[i] == 421 || T.cargo[i] == 422 || T.cargo[i] == 423)
                        {
                            NewDrugs++;                         
                        }
                        if(NewDrugs >= Merchant1DrugCount)
                        {
                            SoldToMerchant1 = true;
                        }
                    }

                }
                break;
            case 2:
                {
                    for (int i = 0; i < T.cargoCount; i++)
                    {
                        if (T.cargo[i] == 420 || T.cargo[i] == 421 || T.cargo[i] == 422 || T.cargo[i] == 423)
                        {
                            NewDrugs2++;
                        }
                        if (NewDrugs2 >= Merchant2DrugCOunt)
                        {
                            SoldToMerchant2 = true;
                        }
                    }
                }
                break;
            default: Debug.Log("uh o");
                break;
        }
    }

    IEnumerator BuyingDrugs()
    {
        //Debug.Log("checking player " + Pweed + " " + Pmilk + DrugCount); // its not entering the loop
        while(!DrugCount)
        {
            //Debug.Log("checking player222 " + Pweed + " " + Pmilk + " " + Pdrug1);
            if (ThePlayer && ThePlayer.currentShip)
            {
                for (int i = 0; i < ThePlayer.currentShip.cargoCount; i++)
                {
                    //Debug.Log("checking player333 " + Pweed + " " + Pmilk + " " + Pdrug1 + "size: " + ThePlayer.currentShip.cargoCount);
                    if (ThePlayer.currentShip.cargo[i] == 420) Pweed = 1;
                    if (ThePlayer.currentShip.cargo[i] == 423) Pmilk = 1;
                    if (ThePlayer.currentShip.cargo[i] == 421) Pdrug1 = 1;
                    if (ThePlayer.currentShip.cargo[i] == 422) Pdrug2 = 1;

                    if (Pweed + Pmilk + Pdrug1 + Pdrug2 >= 3)
                    {
                        DrugCount = true;
                    }
                }
            }
            yield return null;
        }
        GotDrugs = true;
        StopCoroutine("BuyingDrugs");
    }
}
