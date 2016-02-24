using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class SpaceEntity : MonoBehaviour, IDamageable , IKillable
{
	public int cargoCapacity = 3;
	public float health = 100f;
	public bool isAlive = true; //necessary for signalling that its dead before the GameObject itself is destroyed
	internal GameManager gm;
    const int ARRAY_CAP = 20;
	internal int [] cargo = new int[ARRAY_CAP]; //List of references (i.d's) to EconManager.
    internal uint cargoCount = 0;
    protected string lastAttacker;

    public GameObject droppedLoot;
    public GameObject onDeathEffect;
    //ParticleAnimator deathEffectAnimator;
    //Transform hull; // for access to scale

	//Use this for references
	virtual protected void Awake()
	{
        StartCoroutine("GetGameManager");
        /*// For     scaling explosion to ship size
        deathEffectAnimator = GetComponentInChildren<ParticleAnimator>();
        var children = this.GetComponentsInChildren<Transform>();
        foreach (var x in children)
            if (x.name == "hull")
                hull = x.transform;
         */
	}

    IEnumerator GetGameManager()
    {
        while (gm == null)
        {
            var temp = GameObject.FindGameObjectWithTag("GameManager");
            gm = temp.GetComponent<GameManager>();
            yield return null;
        }
    }

	virtual protected void Update() 
	{
		if (gm && health <= 0)
		{
			Killed();
		}
       // Debug.Log("cargoCapacity " + cargoCount);
	}
	
	virtual public void TakeDamage(float damage)
	{
        //Debug.Log("Something is actually calling SpaceEntity TakeDamage()");
		health -= damage;
	}
	
	virtual public void Killed()
	{
		gm.EntityDied(gameObject, lastAttacker); // inform gm that this ship died
        var loot = Instantiate(droppedLoot, transform.position, transform.rotation) as GameObject;
        var explosion = Instantiate(onDeathEffect, transform.position, transform.rotation) as GameObject;
        Destroy(explosion, 4);
        // modify the size of dust cloud and explosion by health of the entity
        float mod = health * 0.02f;
        Vector3 scaleModifier = new Vector3(mod, mod, mod);
        explosion.transform.localScale += scaleModifier;
        explosion.particleSystem.startSize += mod;
        var explosionItself = explosion.GetComponentInChildren<ParticleRenderer>();
        if (explosionItself)
            explosionItself.maxParticleSize += mod * 0.5f;

		Destroy(gameObject);
	}


    public void AddToCargo(int itemId)
    {
        if (cargoCount < cargoCapacity && cargoCount < ARRAY_CAP)
        {
            cargo[cargoCount] = itemId;
            //Debug.Log(name + " added " + cargo[cargoCount] + " to cargo");
            cargoCount++;
            //Debug.Log("ya bitch, the caro count is " + this.name + " " + cargoCount);
        }
        //else
        //    Debug.Log("Cargo of " + name + " is full; cannot add item " + itemId);
    }

    public void RemoveFromCargo(int itemId)
    {
        if (cargoCount > 0)
            --cargoCount;
    }
}
