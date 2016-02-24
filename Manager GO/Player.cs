using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {


    float targetTimer = 0;
    Vector3 respawnPosition = Vector3.zero; // initially spawn at 0,0,0

	#region References to Attached GO
	InputManager input;
	GuiManager gui;
    GameManager gm;
	#endregion

	#region References to External GO Components
	[HideInInspector]
	public GameObject currentShipGO;
    float shipAcquireTimerLimit= 2; //2 seconds max
    float shipAcquireTimer = 0;
	[HideInInspector]
	public Ship currentShip;
	private Targeting targeting;
	private WeaponFire weapon;
	public List<ShipTitle> pocketedShips = new List<ShipTitle>(); // only contains uninstantiated ships
	#endregion
    


	// Use this for references
	void Awake()
	{
		input = GetComponent<InputManager>();
		gui = GetComponent<GuiManager>();
        gm = GetComponent<GameManager>();
	}

	// Use for initialization
	void Start()
	{
        //var property = GameObject.FindGameObjectsWithTag ("Owned");
        //foreach (var ship in property) {
        //    ownedShips.Add(ship.GetComponent<Ship>());
        //}
		GetCurrentShip ();
	}

    IEnumerator ReacquireShip()
    {
        for (; shipAcquireTimer < shipAcquireTimerLimit; shipAcquireTimer += Time.deltaTime )
        {
            if (GetCurrentShip())
            {
                shipAcquireTimer = 0f;
                StopCoroutine("ReacquireShip");
            }
            yield return null;
        }
        gm.AllPlayerShipsAreDead();
    }

	void Update()
	{
		if (!currentShip && shipAcquireTimer == 0) //timer is 0 only when it hasnt been started
        {
            StartCoroutine("ReacquireShip");
            return;
        }

		if (targetTimer > 0.08f) {
			if (input.targetNextEnemy > 0 && targeting.targetableHostiles.Count > 0) {
				targeting.NextHostile ();
				gui.SetCurrentTarget (targeting.currentTarget);
				targetTimer = 0f;
			}  

			if (input.targetNextFriendly > 0 && targeting.targetableFriendlies.Count > 0) {
				targeting.NextFriendly ();
               gui.SetCurrentTarget(targeting.currentTarget);
				targetTimer = 0f;
			}
		}


        if (input.mouseLeft > 0 && !gui.playerInvenOpen && weapon)
			weapon.Fire();
        

		if (input.shieldOn > 0 && currentShip)
			currentShip.Shield (true);
		else if (currentShip)
			currentShip.Shield (false);

        targetTimer += Time.deltaTime;
	}


	/* This is called by GameManager.
	 * One ship tagged player at a time, potentially many tagged owned.
	 * If ship tagged player goes null (dies), then reassign one of the owned ship as player.*/
    // RETURNS FALSE WHEN PLAYER HAS NO MORE SHIPS
	public bool GetCurrentShip()
	{
        if (currentShipGO && currentShip && currentShip.isAlive)
        {
            Debug.Log("Player already has access to all parts of ship");
            return true;
        }



		/*Scenario with game start up/instantiation- workaround race condition.*/
		currentShipGO = GameObject.FindGameObjectWithTag("Player");
        if (!currentShipGO)
        {
            currentShipGO = GameObject.FindGameObjectWithTag("Owned");
            if (!currentShipGO && pocketedShips.Count > 0)
            {
                var r = Random.Range(100f, 200f);
                Vector3 offset = new Vector3(r,r,r);
                currentShipGO = Instantiate(pocketedShips[0].gameObject, respawnPosition + offset, Quaternion.identity) as GameObject;
                pocketedShips.RemoveAt(0);
            }
        }


        if (currentShipGO)
        {
            //get needed references
            currentShip = currentShipGO.GetComponent<Ship>();
            targeting = currentShipGO.GetComponent<Targeting>();
            weapon = currentShipGO.GetComponent<WeaponFire>();
            var basicAi = currentShipGO.GetComponent<BasicAi>();
            //disable AI (if they exist)
            if (basicAi)
                basicAi.enabled = false;
            var fighterAi = currentShipGO.GetComponent<FighterAi>();
            if (fighterAi)
                fighterAi.enabled = false;

            currentShipGO.tag = "Player";
            currentShip.enabled = true;
            //Debug.Log("grabbed new attributes from the gameobject " + currentShipGO.name);
            return true;
        }
        else
            return false; // signal that game is over

	}


    // Set the respawn position where the player died
    public void SetRespawnPosition(Vector3 point)
    {
        respawnPosition = point;
    }

	// Changes currentShip to newShip (must be in the list)
	void SwitchToShip(Ship ownedShip)
	{
		throw new System.NotImplementedException();
	}

	// Add newShip to list of owned ships
	void AcquireShip(Ship newShip)
	{
		throw new System.NotImplementedException();
	}
}
