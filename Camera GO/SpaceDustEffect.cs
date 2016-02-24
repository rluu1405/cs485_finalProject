using UnityEngine;
using System.Collections;

public class SpaceDustEffect : MonoBehaviour
{



    public float speedMultiplier = -4; // negative for direction
    private float minimum = 10f; // for floating point comparision


    #region References to Attached GO Components
    public ParticleSystem spaceDustEffect;
    #endregion


    #region External GO References
    Player player;
    #endregion




    // For references
    void Awake()
    {
        var temp = GameObject.FindGameObjectWithTag("GameManager");
        player = temp.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player-controlled ship stops, disable effect
        if (player && player.currentShip) // check for player and currentShip existence
        {
            if (player.currentShip.velocity.z <= minimum)//&& player.currentShip.velocity.z >= -delta)
            {
                spaceDustEffect.enableEmission = false;
            }
            else // If player-controlled ship is moving, then activate effect and change "start speed" of the effect 
            {
                spaceDustEffect.enableEmission = true;
                spaceDustEffect.startSpeed = player.currentShip.velocity.z * speedMultiplier;
                spaceDustEffect.emissionRate = player.currentShip.velocity.z / 4f;
            }
        }
        else
            spaceDustEffect.enableEmission = false;
    }



}
