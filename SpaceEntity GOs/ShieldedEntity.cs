using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldedEntity : SpaceEntity 
{
	public   float MAX_SHIELD     = 100f;
	internal float ShieldStrength = 0;
	internal float shieldRechargeTimer    = 0f;
	public float shieldRechargeTimerBound = 3f;
	public float damageReductionDampener    = 0.5f;
	protected bool isShielded = false;

	// Update is called once per frame
	virtual protected void Update () 
	{
		base.Update();
		RegenerateShield();
	}

    virtual public void Shield(bool setActive)
    {
        if (ShieldStrength > 0)
            isShielded = setActive;
        else
            isShielded = false;
    }

	override public void TakeDamage(float damage)
	{
        if (isShielded)
        {
            // if shield on, take no hp loss and take 1/2 dmg from shield
            Debug.Log("Taking shieled dmg" + ShieldStrength + " " + isShielded);
            ShieldStrength -= damage * damageReductionDampener;
            shieldRechargeTimer = 0; //reset shield regen timer
        } // kinda point less but w/e
        else // if shield is zero or input is off, take hp damage
        {                       //ensure that shield stays non-negative
            health -= damage;
        }

        if (ShieldStrength < 0)
            ShieldStrength = 0;
	}

	virtual protected void RegenerateShield()
	{
		if (shieldRechargeTimer <= shieldRechargeTimerBound)
			shieldRechargeTimer += Time.deltaTime;
		else if (ShieldStrength < MAX_SHIELD && !isShielded)
			ShieldStrength += 0.1f;
		else if (ShieldStrength >= MAX_SHIELD)
			ShieldStrength = MAX_SHIELD;
		else if (ShieldStrength <= 0)
		{
			ShieldStrength = 0;
			shieldRechargeTimer = 0;
		}
	}
}
