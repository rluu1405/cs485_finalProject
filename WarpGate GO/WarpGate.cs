using UnityEngine;
using System.Collections;

public class WarpGate : MonoBehaviour 
{
    GameObject Gm;
    bool isActive = false;

    void Awake()
    {
        Gm = GameObject.FindGameObjectWithTag("GameManager");
    }

	void OnTriggerEnter(Collider col)
    {
        // If is player AND is not a LOS collider
        if(isActive && col.isTrigger == false && col.gameObject.tag == "Player")
        {
            Gm.GetComponent<GameManager>().LevelTransition();
        }
    }



    public void Activate()
    {
        isActive = true;
    }

    public bool IsActive()
    {
        return isActive;
    }


}
