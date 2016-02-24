using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileFire : MonoBehaviour
{
    public float coolDown = 0.5f;
    public GameObject bullet;
    public AudioClip weaponSound;
    public List<Transform> weaponSpawns;

    private float timer = 0;
    private InputManager input;

    //dealing with equipment
    private GameObject addon;
    //private bool addOn;




    // Update is called once per frame
    void Update()
    {
        if (input != null)
        {
            Debug.Log("MissileFire Referenced InputManager");
            if (timer >= coolDown && input.mouseRight > 0)
            {
                float RandomVolume = Random.Range(0.2f, 0.7f);
                audio.PlayOneShot(weaponSound, RandomVolume);
                foreach (var weapon in weaponSpawns)
                    Instantiate(bullet, weapon.position, transform.rotation);
                timer = 0f;
            }
            timer += Time.deltaTime;
        }
        else
        {
            var temp = GameObject.FindGameObjectWithTag("GameManager");
            input = temp.GetComponent<InputManager>();
            Debug.Log("MissileFire waiting for GameManager GO to instantiate");
        }
    }

}

