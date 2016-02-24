using UnityEngine;
using System.Collections;
using System.Text;

public class CoalitionSceneControl : MonoBehaviour {

    GameManager gm;
    internal uint killCount = 0;
    public int requiredKills = 40;
    WarpGate warpGate;
    bool ShowPrompt = true;
    StringBuilder str = new StringBuilder();
    public float attackerSpawnTimerBound = 60f;
    public int attackerSpawnDistance = 400;
    public int nAttackersSpawned = 3;
    public string attackerType = "EarthFighters";
    float attackerSpawnTimer;


    void Awake()
    {
        gm = GetComponent<GameManager>();
    }

    void Start()
    {
        var temp = GameObject.FindGameObjectWithTag("WarpGate");
        warpGate = temp.GetComponent<WarpGate>();
        if (warpGate == null)
            Debug.Log("Warpgate not found in Coalition Scene");

        StartCoroutine("OccasionalAttackerSpawn");
    }

    IEnumerator OccasionalAttackerSpawn()
    {
        while (!warpGate.IsActive())
        {
            if (attackerSpawnTimer > attackerSpawnTimerBound && gm)
            {
                gm.SpawnAttackers(attackerType, (uint)nAttackersSpawned, (uint)attackerSpawnDistance);
                attackerSpawnTimer = 0f;
            }

            attackerSpawnTimer += Time.deltaTime;
            yield return null;
        }
    }

    void OnGUI()
    {
        if (ShowPrompt)
        {
            GUI.Box(new Rect(400, 10, 480, 120), "Computer:");
            GUI.Label(new Rect(410, 40, Screen.width, Screen.height), "ALERT ALERT, Enemy formations detected! Emergency! Emergency! Low Fuel.\n Enenmy forces closing....Coalition nearby.....suggested course of action:\n *Escape Via Alien Warpgate in sector 3.  Destroy enemy ships interfering......*");
            Screen.showCursor = true;
        }

        if (ShowPrompt && GUI.Button(new Rect(550, 150, 100, 40), "Okay..."))
        {
            ShowPrompt = false;
            Screen.showCursor = false; // hide cursor
        }


        /* Used to display values until we have nice gui elements for the following*/
        /*Used to display values for easy debugging.*/
        str.Remove(0, str.Length);
        str.Append("\n\n\n\n\n\n");
        if (!warpGate.IsActive())
        {
            str.Append("Enemies Killed:\t");
            str.Append(killCount);
            str.Append("/");
            str.Append(requiredKills);
        }
        else
            str.Append("The warpgate is active");

        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), str.ToString());
    }

    public void IncrementKillCount()
    {
        ++killCount;

        if (killCount >= requiredKills)
            warpGate.Activate();
    }
}
