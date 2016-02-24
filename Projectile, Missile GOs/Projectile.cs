using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float bulletSpeed = 300f; // 13.5f maximum speed detectable by collisions!!
	public float deathTime = 100F;
    public float damage = 20;
	public GameObject impactEffect;
    public int coolDown = 1; // how many frames between shots
    public AudioSource weaponSound;

    public string whoFiredMe = "";




    void Start()
    {
        //random range later for better effect
        AudioSource.PlayClipAtPoint(weaponSound.clip, transform.position, 1f);
    }

	// Update is called once per frame
	void FixedUpdate () 
    {


		if(deathTime > 0.0f)
		{
			//Debug.Log(deathTime);
			deathTime -= Time.deltaTime;
		}
		else
		{
			//Debug.Log("destroyed projectile");
			Destroy(gameObject);
		}

		rigidbody.velocity = (transform.up * bulletSpeed);

	}

	void OnCollisionEnter(Collision other)
	{
            //Debug.Log("projectile collided with " + other.gameObject.tag);
			//change out rotation to give the impact a different projection rotation
			//transform.rotation = Quaternion.Inverse(transform.rotation);
			Instantiate(impactEffect, transform.position, Quaternion.Inverse(transform.rotation));
            var aShip = other.collider.GetComponent<Ship>();
            if (aShip) //if a ship, then communicate who damaged it
                aShip.SetLastAttacker(whoFiredMe);
            
            var notAShip = other.collider.GetComponent<SpaceEntity>();
            if (notAShip != null)
                notAShip.TakeDamage(damage); // damage entity if it can be damaged

			Destroy(gameObject); // destroy projectile
	}
}
