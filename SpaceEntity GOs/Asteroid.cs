using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Asteroid : SpaceEntity {

    float RotationSpeed;

    protected void Start()
    {   
        if (gameObject.transform.localScale.magnitude >= 2)
        {
            health *= 2;
        }
        RotationSpeed = Random.Range(0.5f, 1.0f);
        Rotate();
    }
 

    private void Rotate()
    {
        rigidbody.angularVelocity = Random.insideUnitCircle * RotationSpeed;
    }


    public override void Killed()
    {
        //Debug.Log("Asteroid destroyed");
           
        Instantiate(droppedLoot, transform.position, transform.rotation);
        Instantiate(onDeathEffect, transform.position, transform.rotation);
        RotationSpeed = Random.Range(0.5f, 1.0f);
        droppedLoot.rigidbody.angularVelocity = Random.insideUnitCircle * RotationSpeed;

        if (gameObject.transform.localScale.magnitude >= 2)
        {
            //Instantiate(AssDebris, transform.position, transform.rotation); //game breaking code....WIP
            //Instantiate(AssDebris, transform.position, transform.rotation);
            //AssDebris.rigidbody.AddForce(Random.insideUnitCircle * RotationSpeed);
        }

        Destroy(gameObject);
    }

}
