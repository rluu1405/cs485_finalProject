using UnityEngine;
using System.Collections;

public class ToEarthSceneControl : MonoBehaviour 
{

    GameManager gm;
    WarpGate warpGate;
    bool ShowPrompt = true;
    public float attackerSpawnTimerBound = 20f;
    public int attackerSpawnDistance = 400;
    public int nAttackersSpawned = 1;
    public string attackerType = "EarthFighters";
    float attackerSpawnTimer;
    uint waveCount = 0;

    // For awarding rewards from previous map
    Player player;
    Ship playerShip;
    int reward = 8000;

    void Awake()
    {
        gm = GetComponent<GameManager>();
    }


	// Use this for initialization
	void Start () 
    {
        var temp = GameObject.FindGameObjectWithTag("WarpGate");
        warpGate = temp.GetComponent<WarpGate>();
        if (warpGate == null)
            Debug.Log("Warpgate not found in ToEarth Scene");
        
        StartCoroutine("OccasionalAttackerSpawn");
        StartCoroutine("GetPreviousReward");
        Invoke("BeefUpTraders", 2.5f);
    }

    IEnumerator GetPreviousReward()
    {
        while (!player || !playerShip)
        {
            player = GetComponent<Player>();
            if (player)
            {
                playerShip = player.currentShip;
                if (player.currentShip)
                {
                    player.currentShip.Cash += 8000;
                    StopCoroutine("GetPreviousReward");
                }
            }
            yield return null;
        }
    }

    IEnumerator OccasionalAttackerSpawn()
    {
        while (true) // always spawn things
        {
            if (attackerSpawnTimer > attackerSpawnTimerBound && gm)
            {
                gm.SpawnAttackers(attackerType, (uint)(nAttackersSpawned + (waveCount % 5)), (uint)attackerSpawnDistance);
                attackerSpawnTimer = 0f;
                ++waveCount;
            }

            attackerSpawnTimer += Time.deltaTime;
            yield return null;
        }
    }


    void OnGUI()
    {
        if (ShowPrompt)
        {
            GUI.Box(new Rect(400, 10, 150, 100), "Earth");
            GUI.Label(new Rect(410, 40, 400, 300), "You'll never make it ..");
        }

        if (ShowPrompt && GUI.Button(new Rect(425, 120, 100, 40), "Okay..."))
        {
            Screen.showCursor = false;
            ShowPrompt = false;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (true)
        {
            warpGate.Activate();
        }


	}

    void BeefUpTraders()
    {
        int[] items = { 104, 500, 502, 505}; // purple laser, Alien Interceptor, Transport, Earth Fighter
        foreach (var x in items)
            EconomyManager.Economy.SpawnedTraders[0].AddToCargo(x);
    }
}
