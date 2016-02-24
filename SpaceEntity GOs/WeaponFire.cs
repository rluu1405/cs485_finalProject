using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponFire : MonoBehaviour {

    public GameObject weaponSpawnPoints;
    [HideInInspector]
    public List<Transform> weaponSpawns;
    float timer = 0;
    internal GameObject bulletParent;

    public int equippedWeaponId = 100; // defaults to weak laser
    Weapon weapon;                // Weapon attached




	// Use this for initialization
	void Start () 
    {
        bulletParent = GameObject.Find("Bullets");
        if (bulletParent == null)
            Debug.Log("WeaponFire script on GO " + gameObject.name + " has no bulletParent set");

        for (int i = 0; i < weaponSpawnPoints.gameObject.transform.childCount; i++)
        {
            weaponSpawns.Add(weaponSpawnPoints.gameObject.transform.GetChild(i));
        }

        EquipWeapon(equippedWeaponId);
	}
	

	// Update is called once per frame
	void Update () 
    {
        timer++;
	}

    public void EquipWeapon(int WeaponId)
    {
        weapon = EconomyManager.Economy.GetItem(WeaponId) as Weapon;
    }

    public void Fire()
    {
        if (weaponSpawns != null)
        {
            if (timer >= weapon.bullet.coolDown)
            {
                foreach (var x in weaponSpawns)
                {
                    if (x != null)
                    {
                        //float RandomVolume = Random.Range(0.1f, 0.4f);
                        Projectile P = Instantiate(weapon.bullet, x.position, x.rotation) as Projectile;

                        P.transform.parent = bulletParent.transform;
                        P.whoFiredMe = tag;
                    }
                }

                timer = 0;
            }
        }
        else
            Debug.Log("A weapon spawn point is not attached to " + gameObject.name);
    }
}
