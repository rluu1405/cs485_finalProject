using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlienSceneControl : MonoBehaviour {

	//richard was here
	GameObject timer; //grabs the timer gameObject
	Text timerText; //grabs the timer gameObject's text component
	public int roundedTime;

	//system message
	GameObject dialogueBorder;
	GameObject dialogue;
	Text questDialogue;

	//transmission
	GameObject dialogueBorderA;
	GameObject dialogueA;
	Text questDialogueA;

	//objective
	GameObject dialogueB;
	Text questDialogueB;
	string questText;

	//jeff made this
    public float timeAttackTimer = 600; // how long time attack should last
    internal bool timeAttackIsActive = false;
    WarpGate warpGate;
    bool ShowPrompt = true;
    public int attackerSpawnDistance = 400;
    public int nAttackersSpawned = 1;
    public string attackerType = "EarthFighters";
    GameManager gm;
    int current; // for hacky workaround
    bool hackyWorkaround = true;

    void Awake()
    {
        gm = GetComponent<GameManager>();
    }

    // Runs when the script is enabled
    void Start()
    {
        var temp = GameObject.FindGameObjectWithTag("WarpGate");
        warpGate = temp.GetComponent<WarpGate>();
        if (warpGate == null)
            Debug.Log("Warpgate not found in Alien Scene");

        ActivateTimer();
		findTimer ();
    }

	void findTimer()
	{
		timer = GameObject.Find("Timer");
		timerText = timer.GetComponent<Text> ();

		dialogueBorder = GameObject.Find ("quest");
		dialogue = GameObject.Find ("questText");
		questDialogue = dialogue.GetComponent<Text>();
		questDialogue.text = "";

		dialogueBorderA = GameObject.Find ("transmission");
		dialogueA = GameObject.Find ("transmissionText");
		questDialogueA = dialogueA.GetComponent<Text>();
		questDialogueA.text = "";
		dialogueBorderA.SetActive (false);

		dialogueB = GameObject.Find ("Objective");
		questDialogueB = dialogueB.GetComponent<Text>();
		questDialogueB.text = "";
		questText = "";
	}

//    void OnGUI()
//    {
//        if (ShowPrompt)
//        {
//            GUI.Box(new Rect(400, 10, 400, 300), "Alien");
//            GUI.Label(new Rect(410, 40, Screen.width, Screen.height), "");
//        }
//
//        if (ShowPrompt && GUI.Button(new Rect(550, 350, 100, 40), "Okay..."))
//        {
//            ShowPrompt = false;
//        }
//    }

    // Updated each frame
    void Update()
    {
        if (timeAttackIsActive)
        {
            timeAttackTimer -= Time.deltaTime;


			if (timeAttackTimer <= 0)
            {
                warpGate.Activate();
            }

			roundedTime = Mathf.RoundToInt(timeAttackTimer);
			timerText.text = roundedTime.ToString();
			timeEvent();
            //Spawn attackers periodically

            if (timeAttackTimer < current)
            {
                current = (int)timeAttackTimer;
                hackyWorkaround = true;
            }
        }
    }


    public void ActivateTimer()
    {
        timeAttackIsActive = true;
    }

	void timeEvent()
	{
		switch(roundedTime)
		{
		case 599:
			questDialogue.text = "Warning: Engine system critical!";
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned, (uint)attackerSpawnDistance);
            }
			//Invoke("turnOff", 5);
			break;
			
		case 593:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Mission Update: \n\n-Wait until Jump Systems are repaired.";
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.";
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned, (uint)attackerSpawnDistance);
            }
			Invoke("turnOff", 6);
			break;

		case 586:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Jump System Repair Engaging";
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Repair Engaging";
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned, (uint)attackerSpawnDistance);
            }
			//Invoke("turnOff", 7);
			break;

		case 585:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Jump System Repair Engaging.";
			questDialogueB.text = "Objectives: \n\n\tWait until Jump Systems are repaired.\n\n\t-Jump System Repair Engaging.";
			//Invoke("turnOff", 7);
			break;
			
		case 584:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Jump System Repair Engaging..";
			questDialogueB.text = "Objectives: \n\n\tWait until Jump Systems are repaired.\n\n\t-Jump System Repair Engaging..";
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned, (uint)attackerSpawnDistance);
            }
			//Invoke("turnOff", 7);
			break;

		case 583:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Jump System Repair Engaging...";
			questDialogueB.text = "Objectives: \n\n\tWait until Jump Systems are repaired.\n\n\t-Jump System Repair Engaging...";
			Invoke("turnOff", 3);

			questDialogueB.text = "Objectives: \n\n\tWait until Jump Systems are repaired.\n\n\t-Jump System Status: %0";
			break;
			
		case 567:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Jump System Status: %3";
			questDialogueB.text = "Objectives: \n\n\tWait until Jump Systems are repaired.\n\n\t-Jump System Status: %3";
			Invoke("turnOff", 5);
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned, (uint)attackerSpawnDistance);
            }
			break;

		case 550:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Jump System Status: %8";
			questDialogueB.text = "Objectives: \n\n\tWait until Jump Systems are repaired.\n\n\t-Jump System Status: %8";
			Invoke("turnOff", 5);
			break;

		case 544:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Incoming Transmission...";
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
            }
			Invoke("turnOff", 3);
			break;

		case 539:
			dialogueBorderA.SetActive (true);
			questDialogueA.text = "  Enemy Captain: \n\tThought you could tip-toe your way out eh...?";
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
            }
			Invoke("turnOffA", 6);
			break;

		case 532:
			dialogueBorderA.SetActive (true);
			questDialogueA.text = "  Enemy Captain: \n\tThink again... YOU'RE DEAD!";
			questDialogueB.text = "Objectives: \n\n\tWait until Jump Systems are repaired.\n\n\t-Jump System Status: %15";
			Invoke("turnOffA", 6);
			break;

		case 526:
			dialogueBorderA.SetActive (true);
			questDialogueA.text = "  Enemy Captain: \n\tLIGHT 'EM UP!";
			Invoke("turnOffA", 6);
            if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
            }

			break;

		case 518:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Mission Update: \n\n-Fight or Evade new Hostile Forces";
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %16 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			if (hackyWorkaround)
            {
                hackyWorkaround = false;
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
            }
			//Invoke("turnOff", 6);
			break;

		case 500:
			//dialogueBorder.SetActive (true);
			//questDialogue.text = "Mission Update: \n\n-Fight or Evade new Hostile Forces";
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %20 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			//Invoke("turnOff", 6);
			break;

		case 450:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %25 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;
		
		case 400:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %30 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 350:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %40 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 300:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %50 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 250:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %60 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 200:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %70 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 150:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %80 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 100:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %90 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 50:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Warning: \n\n-Incoming Hostile Forces";
			if (hackyWorkaround)
			{
				hackyWorkaround = false;
				gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned * 2, (uint)attackerSpawnDistance);
			}
			questDialogueB.text = "Objectives: \n\n\t-Wait until Jump Systems are repaired.\n\n\t-Jump System Status: %95 \n\n\t-Fight or Evade new Hostile Forces";
			Invoke("turnOff", 6);
			break;

		case 0:
			dialogueBorder.SetActive (true);
			questDialogue.text = "Mission Update: \n\n-Jump System Repair Complete \n\n-Escape Through Warp Gate";
			questDialogueB.text = "Objectives:\n\n\t-Jump System Status: %100 \n\n\t-Escape Through Warp Gate";
			Invoke("turnOff", 6);
			break;
			
		default:
			break;
		}
	}

	void turnOff()
	{
		dialogueBorder.SetActive (false);
	}

	void turnOffA()
	{
		dialogueBorderA.SetActive (false);
	}
	
}
