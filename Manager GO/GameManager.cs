/* Handles instantiation and destruction of NPCs */

/* May eventually move much of this into Player and NpcPilot classes
 *  (that derive from a base Pilot class).  These members include 
 *   faction relationsips, money 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float spawnTimerBound = 300; //updates each second
    public FactionRelationships factions = new FactionRelationships();
	float spawnTimer;
	delegate GameObject GetRandomDelegate();
	GetRandomDelegate GetRandom;
	delegate void AssignParentDelegate(GameObject newObject);
	AssignParentDelegate AssignParent;
    bool isGameOver = false;
    public int maxSpawnAttackers = 8;
    int nAttackers = 0;


    #region Faction Counts
    uint[] nCoalition = new uint[2]; //0th is fighters count, 1st is civilians count
    uint[] nAlien = new uint[2];
    uint[] nEarth = new uint[2];
    uint[] nNanite = new uint[2];
    public int FACTION_SPAWN_LIMIT = 20; //Max number of troops spawned per faction
    #endregion


	#region Inspector Assigned References
	public GameObject coalitionShipParent;
	public List<GameObject> coalitionFighters;
	public List<GameObject> coalitionCivilians;
	public GameObject alienShipParent;
	public List<GameObject> alienFighters;
	public List<GameObject> alienCivilians;
	public GameObject earthShipsParent;
	public List<GameObject> earthFighters;
	public GameObject naniteShipParent;
	public List<GameObject> naniteFighters;
	public List<GameObject> naniteCivilians;
	public GameObject AsteroidFieldsParent;
	public List<GameObject> asteroids;
    public GameObject debrisParent;
    public List<GameObject> debris;
	public GameObject ownedShipParent; //for player-owned ships
	#endregion


    #region Attached GO Component References
    EconomyManager economy; // this is not used....(CJ)
	Player player;
    CoalitionSceneControl coalitionScene;
    AlienSceneControl alienScene;
    DrugsSceneControl drug;
    ToEarthSceneControl toEarth;
    InputManager input;
    #endregion


	#region Waypoint GO Lists
	List<Transform> coalitionWaypoints = new List<Transform>();
	List<Transform> alienWaypoints = new List<Transform>();
	List<Transform> earthWaypoints = new List<Transform>();
	List<Transform> naniteWaypoints = new List<Transform>();
	List<Transform> asteroidWaypoints = new List<Transform>();
    List<Transform> debrisWaypoints = new List<Transform>();
	#endregion





    #region Awake, Start, Update, Collisions, Etc
    // Use this for references
    void Awake()
    {
        //ENSURE SINGLETON  (works around unity bug where multiple objects WITH SAME ID!!!!)
        var temp = GameObject.FindGameObjectsWithTag("GameManager");
        if (temp.Length > 1) //If there are more than one 
        {
            foreach (var x in temp)
            {
                Debug.Log("This ID: " + gameObject.GetInstanceID() + " and that ID: " + x.GetInstanceID());
                // .... and this one is newer (greater id), then destroy this one
                if (GetInstanceID() > x.GetInstanceID())
                    Destroy(gameObject);
            }
        }
        //AND PREVENT DESTRUCTION ON LEVEL LOAD   
        //DontDestroyOnLoad(gameObject);

        player = GetComponent<Player>();
        input = GetComponent<InputManager>();
        coalitionScene = GetComponent<CoalitionSceneControl>();
        GetSceneReady();
    }

    // Use this for initialization
    void Start() 
    {
        System.GC.Collect(); // Force garbage collection
        Configure();
        Random.seed = (int)System.DateTime.Today.Ticks;

		FindWaypoints (); 			  //grab all spawn points
        InitialPopulateSpawnPoints(); //populate the level

        switch (Application.loadedLevelName)
        {
            case "Coalition":
                coalitionScene.enabled = true;
                break;
            case "Alien":
                alienScene.enabled = true;
                break;
            case "Drugs":
                drug.enabled = true;
                break;
            case "ToEarth":
                toEarth.enabled = true;
                break;  
            default:
                Debug.Log("Make a scene manager for the scene " + Application.loadedLevelName);
                break;
        }
    }

    // Updates each frame
    void Update()
    {
        // FOR PRESENTATION/DEBUG ONLY
        //if (input.nextLevel > 0)
        //    LevelTransition();

        if (spawnTimer > spawnTimerBound)
        {
            RepopulateSpawnPoints();
            spawnTimer = 0;
        }

        spawnTimer += Time.deltaTime;
    }
    #endregion


    #region Configuration and Initialization Functions

    void OnApplicationQuit()
    {
        //Debug.Log("GameManager is forcing collection of garbage on QUIT");
        System.GC.Collect(); // Force garbage collection when it actually matters
    }

    // Game-wide and Unity configuration options
    private void Configure()
    {
        Application.targetFrameRate = 60;
    }


    void GetSceneReady()
    {
        switch (Application.loadedLevelName)
        {
            case "Coalition":
                {
                    coalitionScene = GetComponent<CoalitionSceneControl>();
                    if (coalitionScene)
                        coalitionScene.enabled = true;
                    else Debug.Log("coalition component not found");
                }
                break;
            case "Alien":
                {

                    alienScene = GetComponent<AlienSceneControl>();
                    if (alienScene)
                        alienScene.enabled = true;
                    else Debug.Log("alien component not found.");
                }
                break;
            case "Drugs":
                {
                    drug = GetComponent<DrugsSceneControl>();
                    if (drug)
                        drug.enabled = true;
                    else Debug.Log("drug component not found");
                }
                break;
            case "ToEarth":
                {
                    toEarth = GetComponent<ToEarthSceneControl>();
                    if (toEarth)
                        toEarth.enabled = true;
                    else Debug.Log("earth component not found");
                }
                break;
            default:
                Debug.Log("Make a scene manager for the scene " + Application.loadedLevelName);
                break;
        }
    }

    // get all waypoints
    // Parse out waypoints by faction and location type (military, trade, etc)
    void FindWaypoints()
    {
        var gameObjectList = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (var point in gameObjectList)
        {
            switch (point.name)
            {
                case "CoalitionWaypoint":
                    coalitionWaypoints.Add(point.transform);
                    break;
                case "AlienWaypoint":
                    alienWaypoints.Add(point.transform);
                    break;
                case "EarthWaypoint":
                    earthWaypoints.Add(point.transform);
                    break;
                case "NaniteWaypoint":
                    naniteWaypoints.Add(point.transform);
                    break;
                case "AsteroidWaypoint":
                    asteroidWaypoints.Add(point.transform);
                    break;
                case "DebrisWaypoint":
                    asteroidWaypoints.Add(point.transform);
                    break;
            }
        }

    }
    #endregion


    #region Spawn Functions

    // Spawn attackers that target player
    public void SpawnAttackers(string enemyType, uint nToBeSpawned, uint distance)
    {
        if (player && player.currentShip)
        {
            if (nToBeSpawned + nAttackers < maxSpawnAttackers)
            {
                //Debug.Log("Trying to SpawnAttackers: " + enemyType);
                // Spawn attackers
                var attackers = SpawnEntities(enemyType, nToBeSpawned, nToBeSpawned, player.currentShip.transform.position, distance, distance, true);
                // (?? Possibly Set tag to Earth)

                // Set target to player
                foreach (var x in attackers)
                {
                    ++nAttackers;

                    //var targeting = x.GetComponent<Targeting>();
                    //if (targeting && player.currentShip )
                    //    targeting.targetableHostiles.Add(player.currentShip.transform);
                    // once assigned target, state assumed to switch to fighting
                }
            }
            else
                Debug.Log("Failed to spawn " + nToBeSpawned + " attackers because " + (nAttackers + nToBeSpawned) + " exceeds limit of " + maxSpawnAttackers);
        }
        else
            Debug.Log("Player and/or player.current ship are not yet accessible when calling SpawnAttackers");
    }

    private void InitialPopulateSpawnPoints()
    {
        foreach (var point in asteroidWaypoints)
            SpawnEntities("Asteroids", 7, 15, point.position, 0, 1000, false);
        foreach (var point in debrisWaypoints)
            SpawnEntities("Debris", 80, 100, point.position, 400, 500, false);

        RepopulateSpawnPoints();
    }

    private void RepopulateSpawnPoints()
    {
        foreach (var point in coalitionWaypoints)
        {
            SpawnEntities("CoalitionFighters", 2, 3, point.position, 100, 200, false); //more distanced spread AROUND space station, structures, etc
            SpawnEntities("CoalitionCivilians", 1, 2, point.position, 100, 150, false);
        }
        foreach (var point in alienWaypoints)
        {
            SpawnEntities("AlienFighters", 2, 3, point.position, 100, 200, false);
            SpawnEntities("AlienCivilians", 1, 2, point.position, 100, 150, false);
        }
        foreach (var point in naniteWaypoints)
        {
            SpawnEntities("NaniteCivilians", 1, 1, point.position, 100, 150, false);
            SpawnEntities("NaniteFighters", 2, 3, point.position, 100, 200, false);
        }
        foreach (var point in earthWaypoints)
            SpawnEntities("EarthFighters", 2, 3, point.position, 50, 100, false); //tighter, squad spawn

    }



    // Spawns specified entities at appropriate waypoints
    // Inspired by Connor
    List<GameObject> SpawnEntities(string entitySet, uint minNum, uint maxNum, Vector3 position, uint minRadius, uint radiusOffset, bool isForced)
    {
        List<GameObject> newEntities = new List<GameObject>();
        float nEntities = Random.Range((int)minNum, (int)maxNum);

        switch (entitySet)
        {
            case "CoalitionFighters":
                GetRandom = RandomCoalitionFighter;
                AssignParent = ParentToCoalitionShips;
                if (nCoalition[0] >= FACTION_SPAWN_LIMIT && !isForced)
                    return newEntities;
                nCoalition[0] += (uint)nEntities;
                break;
            case "CoalitionCivilians":
                GetRandom = RandomCoalitionCivilian;
                AssignParent = ParentToCoalitionShips;
                if (nCoalition[1] >= FACTION_SPAWN_LIMIT && !isForced)
                    return newEntities;
                nCoalition[1] += (uint)nEntities;
                break;
            case "AlienFighters":
                GetRandom = RandomAlienFighter;
                AssignParent = ParentToAlienShips;
                if (nAlien[0] >= FACTION_SPAWN_LIMIT && !isForced)
                    return newEntities;
                nAlien[0] += (uint)nEntities;
                break;
            case "AlienCivilians":
                GetRandom = RandomAlienCivilian;
                AssignParent = ParentToAlienShips;
                if (nAlien[1] >= FACTION_SPAWN_LIMIT && !isForced)
                    return newEntities;
                nAlien[1] += (uint)nEntities;
                break;
            case "NaniteFighters":
                GetRandom = RandomNaniteFighter;
                AssignParent = ParentToNaniteShips;
                if (nNanite[0] >= FACTION_SPAWN_LIMIT && !isForced)
                    return newEntities;
                nNanite[0] += (uint)nEntities;
                break;
            case "NaniteCivilians":
                GetRandom = RandomNaniteCivilian;
                AssignParent = ParentToNaniteShips;
                if (nNanite[1] >= FACTION_SPAWN_LIMIT && !isForced)
                    return newEntities;
                nNanite[1] += (uint)nEntities;
                break;
            case "EarthFighters":
                GetRandom = RandomEarthFighter;
                AssignParent = ParentToEarthShips;
                if (nEarth[0] >= FACTION_SPAWN_LIMIT && !isForced)
                    return newEntities;
                nEarth[0] += (uint)nEntities;
                break;
            case "Asteroids":
                GetRandom = RandomAsteroid;
                AssignParent = ParentToAsteroid;
                break;
            case "Debris":
                GetRandom = RandomDebris;
                AssignParent = ParentToDebris;
                break;
        }

        //if(isForced)
        //    Debug.Log("GameManager forced " + nEntities + " To Spawn " + GetRandom.Method.Name); 
        GameObject selected;
        for (int i = 0; i < nEntities; i++)
        {

            selected = GetRandom();
            GameObject newEntity = Instantiate(selected, position + RandomPosition(minRadius, radiusOffset), Quaternion.identity) as GameObject;
            newEntities.Add(newEntity);
            AssignParent(newEntity);
        }
        return newEntities;
    }

    #region Spawn Helper Functions

    #region Select random configuration from inspector-assigned list
    GameObject RandomCoalitionFighter()
    {
        return coalitionFighters[(Random.Range(0, coalitionFighters.Count - 1))];
    }
    GameObject RandomCoalitionCivilian()
    {
        return coalitionCivilians[(Random.Range(0, coalitionCivilians.Count - 1))];
    }
    GameObject RandomAlienFighter()
    {
        return alienFighters[(Random.Range(0, alienFighters.Count - 1))];
    }
    GameObject RandomAlienCivilian()
    {
        return alienCivilians[(Random.Range(0, alienCivilians.Count - 1))];
    }
    GameObject RandomNaniteFighter()
    {
        return naniteFighters[(Random.Range(0, naniteFighters.Count - 1))];
    }
    GameObject RandomNaniteCivilian()
    {
        return naniteCivilians[(Random.Range(0, naniteCivilians.Count - 1))];
    }
    GameObject RandomEarthFighter()
    {
        return earthFighters[(Random.Range(0, earthFighters.Count - 1))];
    }
    GameObject RandomAsteroid()
    {
        return asteroids[(Random.Range(0, asteroids.Count - 1))];
    }
    GameObject RandomDebris()
    {
        return debris[(Random.Range(0, asteroids.Count - 1))];
    }
    #endregion


    #region Parent for the sake of keeping inspector clean
    void ParentToCoalitionShips(GameObject newObject)
    {
        newObject.transform.parent = coalitionShipParent.transform;
    }
    void ParentToAlienShips(GameObject newObject)
    {
        newObject.transform.parent = alienShipParent.transform;
    }
    void ParentToEarthShips(GameObject newObject)
    {
        newObject.transform.parent = earthShipsParent.transform;
    }
    void ParentToNaniteShips(GameObject newObject)
    {
        newObject.transform.parent = naniteShipParent.transform;
    }
    void ParentToOwned(GameObject newObject)
    {
        newObject.transform.parent = ownedShipParent.transform;
    }
    void ParentToAsteroid(GameObject newObject)
    {
        newObject.transform.parent = AsteroidFieldsParent.transform;
    }
    void ParentToDebris(GameObject newObject)
    {
        newObject.transform.parent = debrisParent.transform;
    }
    #endregion


    // Returns random position outside a sphere but inside a larger sphere
	Vector3 RandomPosition(uint minRadius,uint radiusOffset)
	{
		Vector3 offsetAndDirection = new Vector3 (Random.Range (-radiusOffset, radiusOffset), 
		            							  Random.Range (-radiusOffset, radiusOffset), 
		           								  Random.Range (-radiusOffset, radiusOffset));
		return offsetAndDirection + (offsetAndDirection.normalized * minRadius);
	}
#endregion

    #endregion


    #region Death and Level Load Functions

    // Needs to be to updated use FactionRelationships
	public void EntityDied(GameObject deadObject, string killer)
	{
        //Debug.Log(deadObject.name + " killed by " + killer);

        if (coalitionScene && coalitionScene.enabled && killer == "Player" || killer == "Owned")
            coalitionScene.IncrementKillCount();

        if (!string.IsNullOrEmpty(killer))
        {
            factions.IncrementReputation(deadObject.tag, killer, -50);
            //Debug.Log(deadObject.tag + " likes " + killer + " " + factions.GetReputation(deadObject.tag, killer));
        }

        if (deadObject.tag == "Player")
            player.SetRespawnPosition(deadObject.transform.position);
	}

    // Upon entering a level transition area, call this
    public void LevelTransition()
    {
        Debug.Log("Loading next level");

        // Set faction counts to zero if not destroying on load
        //nCoalition[0] = 0;
        //nCoalition[1] = 0;
        //nAlien[0] = 0;
        //nAlien[1] = 0;
        //nNanite[0] = 0;
        //nNanite[1] = 0;
        //nEarth[0] = 0;
        //nEarth[1] = 0;

        // SHOULD DO THIS, fix the GameManager not carrying over issue!
        //var playerTaggedShips = GameObject.FindGameObjectsWithTag("Player");
        //var playerOwnedShips = GameObject.FindGameObjectsWithTag("Owned");
        //foreach (var x in playerTaggedShips)
        //    DontDestroyOnLoad(x.gameObject);
        //foreach (var y in playerOwnedShips)
        //    DontDestroyOnLoad(y.gameObject);

        switch(Application.loadedLevelName)
        {
            case "Coalition": Application.LoadLevel("Alien");
                break;
            case "Alien": Application.LoadLevel("Drugs");
                break;
            case "Drugs": Application.LoadLevel("ToEarth");
                break;
            case "ToEarth":
                Screen.showCursor = true;
                Application.LoadLevel("Credits");
                break;
            default: Debug.LogError("the scene " + Application.loadedLevelName + " is not accounted for in LevelTransition in GameManager script");
                break;
        }
    }

    public void AllPlayerShipsAreDead()
    {
        isGameOver = true;
    }

    void LoadMenu()
    {
        isGameOver = false;
        Application.LoadLevel("MainMenu");
    }

    void OnGUI()
    {
        if (isGameOver)
        {
            var centerX = Screen.width * 0.5f;
            var centerY = Screen.height * 0.5f;

            GUI.Box(new Rect(centerX - 50, centerY - 100, 100, 40), "Game Over");
            if (GUI.Button(new Rect(centerX - 100, centerY - 50, 200, 100), "Restart Level?"))
            {
                isGameOver = false;
                Application.LoadLevel(Application.loadedLevelName);
            }
            
            if (GUI.Button(new Rect(centerX - 100, centerY + 50, 200, 100), "Exit to Menu?"))
            {
                isGameOver = false;
                Application.LoadLevel("MainMenu");
            }
        }
    }

    #endregion


    #region Deprecated
    //	// Created by Connor
	//	//Considering the possibility of a negative radius? :^)
	//	void SpawnAsteroidField(Vector3 position, uint minNum, uint maxNum, uint radius)
	//	{
	//		float nCreatedAsteroids = Random.Range(minNum, maxNum); 
//		long asteroidValue = 20; // This should be stored on the asteroid
//		GameObject selectedAsteroid;
//		
//		for (int i = 0; i < nCreatedAsteroids; i++)
//		{
//			
//			selectedAsteroid = RandomAsteroid(asteroids);
//			GameObject AsteroidChild = Instantiate(selectedAsteroid, position + RandomPosition(0f, radius), Quaternion.identity) as GameObject;
//			AsteroidChild.transform.parent = AsteroidFieldsParent.transform;
//			
//			economy.marketBaseAmount += asteroidValue;
//		}
    //	}
    #endregion
}
