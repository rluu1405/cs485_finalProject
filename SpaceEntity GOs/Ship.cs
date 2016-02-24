using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Targeting))]
[RequireComponent(typeof(AudioSource))]




public class Ship : ShieldedEntity
{

    // Different ship prefabs should set different rates
    public float ROLL_FACTOR = 80f;  //rotate right and left at this rate
    public float YAW_FACTOR = 30f;   //move nose left and right at this rate
    public float PITCH_FACTOR = 75f; //move nose up and down at this rate
    public float MAX_SPEED = 75f;
    public float MAX_NEGATIVE_SPEED = -5f;
    public float MAX_ACCEL = 0.4f;
    public Vector3 CAMERA_OFFSET = new Vector3(0, 5, -14);

    internal Vector3 velocity = new Vector3();
    protected float acceleration = 0.0f; //affects z velocity. Why a float...odd
    public int ShieldId;
    public float shieldDischargeRate = 0.8f;
    Shader shieldInactive;
    Shader shieldActive;

    #region Attached to GO Component References
    Rigidbody rigidbody;
    Shield ShipShield;
    internal AudioSource EngineSoundSource;
    #endregion


    #region External to GO References
    MeshRenderer hullRenderer;
    #endregion

    public int Cash = 100;
    public int BlackDollar = 0;




    // use this for references
    virtual protected void Awake()
    {
        base.Awake();
        StartCoroutine("GetRigidBody");
        StartCoroutine("GetMeshRenderer");
        shieldInactive = Shader.Find("Diffuse");
        shieldActive = Shader.Find("Specular");
    }

    #region Get Reference Coroutines
    IEnumerator GetRigidBody()
    {
        while (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody)
                rigidbody.maxAngularVelocity = 0f;

            yield return null;
        }
    }

    IEnumerator GetMeshRenderer()
    {
        while( hullRenderer == null)
        {
            hullRenderer = GetComponentInChildren<MeshRenderer>();
            if (hullRenderer == null)
                hullRenderer = GetComponent<MeshRenderer>();

            yield return null;
        }
    }
    #endregion

    virtual protected void Start()
    {
        EquipShield(ShieldId);
        EngineSoundSource = GetComponent<AudioSource>();
        while(rigidbody == null) // so long as rigidbody is required, this loop is safe!
            rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 0f; // prevent ship from rotating in addition to transform rotation
        while(hullRenderer == null)
            hullRenderer = GetComponentInChildren<MeshRenderer>();
    }

    // Look-up shield item from managers, set up shield
    void EquipShield(int NewId)
    {
        ShipShield = EconomyManager.Economy.GetItem(NewId) as Shield;
        //ShipShield = Instantiate(ShipShield) as Shield; //opting for render tweaks to make shield effect
        //ShipShield.transform.parent = gameObject.transform;
        ShieldSetUp();
        if (hullRenderer && shieldInactive != null)
            hullRenderer.material.shader = shieldInactive;
        else
            Debug.Log("Problem with shield on " + name);
    }

    // Get shield attributes from shield item
    void ShieldSetUp()
    {
        MAX_SHIELD = ShipShield.ShieldPower;
        shieldRechargeTimerBound = ShipShield.ReboundTimer;
        ShieldStrength = ShipShield.ShieldResistance;
    }

    void OnGUI()
    {
        if (tag == "Player")
        {
            /* Used to display values until we have nice gui elements for the following*/
            /*Used to display values for easy debugging.*/
            StringBuilder str = new StringBuilder();
            str.Append("Health:\t\t");
            str.Append(health);
            str.Append("\nShield:\t\t");
            str.Append(ShieldStrength);
            str.Append("\nCash:\t\t");
            str.Append(Cash);
            str.Append("\nBlack Dollars:\t");
            str.Append(BlackDollar);
            str.Append("\n\nSpeed:\t\t");
            str.Append(velocity.magnitude);

            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), str.ToString());
        }
    }

    // Update is called once per frame
    virtual public void Update()
    {
        base.Update();

        // Shield section
        if (ShipShield)
        {
            if (isShielded && hullRenderer)
            {
                hullRenderer.material.shader = shieldActive;
                ShieldStrength -= shieldDischargeRate;
            }
            else
                hullRenderer.material.shader = shieldInactive;
        }
        else
            Debug.Log(name + " ShipShield not acquired");
        RegenerateShield();

        // Apply Velocity to Position
        transform.position += velocity.z * Time.deltaTime * transform.forward;
        if (rigidbody)
            rigidbody.velocity = Vector3.zero; ; // prevent ship from moving in addition to transform rotation
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log(gameObject.name + " collided with " + col.gameObject.name);
        Ship shipCollidedWith = col.gameObject.GetComponent<Ship>();
        SpaceEntity entityCollidedWith = col.gameObject.GetComponent<SpaceEntity>();
        if (col.gameObject.tag == "Loot" && gameObject.tag == "Player") // loot for now, more specific later
        {
            // need to play sounds here
            AudioSource.PlayClipAtPoint(col.gameObject.audio.clip, transform.position, 0.6f);
            switch (col.gameObject.name)
            {
                case "Ore(Clone)":
                    Cash += 50;
                    break;
                case "Ore":
                    Cash += 50;
                    break;
                case "Cargo(Clone)":
                    Cash += Random.Range(50, 200);
                    break;
                case "Cargo":
                    Cash += Random.Range(50, 200);
                    break;
                default:
                    Cash += 20;
                    break;
            }
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "BlackCache" || col.gameObject.name == "BlackCache(clone)")
        {
            BlackDollar += 80;
            Destroy(col.gameObject);
            AudioSource.PlayClipAtPoint(col.gameObject.audio.clip, transform.position, 0.6f);
        }
        else if (shipCollidedWith != null)
        {
            shipCollidedWith.SetLastAttacker(tag); //set this ship as that ships last attacker
            if (tag != "Player")
                health -= (velocity - shipCollidedWith.velocity).magnitude * 5; //head-on collision causes most damage
            else
                health -= (velocity - shipCollidedWith.velocity).magnitude; //less death by kamikazse for the player
        }
        else if (entityCollidedWith != null)
            health -= velocity.magnitude * 5; //if collided with something else, just use this ships velocity for damage calculation
    }

    public void accelerate_until(float target_veloc)
    {
        const float delta = 1f;
        if ((velocity.z <= target_veloc + delta)
            && (velocity.z >= target_veloc - delta))
        {
            //Debug.Log("at velocity");
            velocity.z = target_veloc;
            acceleration = 0f;
        }
        else if (velocity.z < target_veloc)
        {
            //Debug.Log("speeding up");
            acceleration += 0.00125f * MAX_ACCEL;
        }
        else if (velocity.z > target_veloc)
        {
            //Debug.Log("slowing down");
            acceleration -= 0.00125f * MAX_ACCEL;
        }


        //Debug.Log("Speed: " + velocity.z);
        //Debug.Log("Accel: " + acceleration);
        velocity.z += acceleration;
    }


    /*Used primarily by AI. Won't be used by the player unless we implement Auto-piloting.*/
    public void rotate_until(Quaternion target_rotation)
    {
        //Debug.Log ("Rotate to: " + target_rotation.eulerAngles.x + " " + target_rotation.eulerAngles.y + " " + target_rotation.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                               target_rotation,
                                               Time.deltaTime * (YAW_FACTOR + PITCH_FACTOR) * 0.0125f);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetLastAttacker(string attacker)
    {
        lastAttacker = attacker;
    }

}