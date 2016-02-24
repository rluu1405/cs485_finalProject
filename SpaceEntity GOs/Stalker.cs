//using UnityEngine;
//using System.Collections;

///* DIRECT_NAV   - Navigating directly to a target.
// * SUB_WAYPOINT - Navigating to a subwaypoint.
// * FOLLOWING    - Following a target. */
//enum State { DIRECT_NAV, SUB_WAYPOINT, FOLLOWING, IDLE };
//public class Stalker : MonoBehaviour
//{
//    State mode;
//    Player target;
//    Ship mobility;
//    float aggro; //Range to start looking at and following.
//    float[] b_room;//Range of acceptable distance.
//    Ray vision;//Detect obstacles in front.
//    Vector3 coordinate_waypoint;
//    const float MAGIC_NUMBER = 0.6f;
//    void Awake()
//    {
//        mobility = GetComponent<Ship>();
//        var temp = GameObject.FindGameObjectWithTag("Player");
//        target = temp.GetComponent<Player>();
//    }

//    void Start()
//    {// Use this for initialization
//        mode = State.DIRECT_NAV;
//        aggro = 30.0f;
//        b_room = new float[2];
//        b_room[0] = 7.0f;  //distance just considered to be too close
//        b_room[1] = 10.0f; //max distance considered to be close enough
//    }

//    void Update()
//    {// Update is called once per 
//        vision = new Ray(transform.position, transform.forward);
//        RaycastHit hit;
//        if (!Physics.Raycast(vision, out hit, 5) && (mode != State.IDLE || mode != State.SUB_WAYPOINT))
//        {
//            face(target.transform.position);
//            mobility.velocity.z = 2f;
//        }
//        else if (mode != State.IDLE || mode != State.SUB_WAYPOINT)
//        {
//            Debug.DrawLine(vision.origin, hit.point, Color.red, 1f);
//            Debug.Log("Now moving to sub-waypoint.");
//            mode = State.SUB_WAYPOINT;
//            coordinate_waypoint = new Vector3(hit.transform.position.x,
//                                               hit.transform.position.y + hit.transform.localScale.y * MAGIC_NUMBER + transform.localScale.y,
//                                               hit.transform.position.z);
//        }
//        //Above manages and sets mode.
//        if (mode == State.FOLLOWING)
//        {
//            face(target.transform.position);
//            follow(Vector3.Distance(target.transform.position, transform.position));
//            if (Mathf.Abs(Vector3.Distance(target.transform.position, transform.position)) <= 1f)
//            {
//                mode = State.IDLE;
//            }
//        }
//        else if (mode == State.SUB_WAYPOINT)
//        {
//            face(coordinate_waypoint);
//            if (Mathf.Abs(Vector3.Distance(coordinate_waypoint, transform.position)) <= 1f)
//            {
//                mode = State.FOLLOWING;
//            }
//        }
//        if (mode == State.IDLE)
//        {
//            mobility.velocity.z = 0f;
//        }
//        string dbg_msg = "T_T";
//        switch (mode)
//        {
//            case State.DIRECT_NAV:
//                dbg_msg = "navigating directly to target";
//                break;
//            case State.FOLLOWING:
//                dbg_msg = "following";
//                break;
//            case State.SUB_WAYPOINT:
//                dbg_msg = "pursuing a sub-waypoint.";
//                break;
//            case State.IDLE:
//                dbg_msg = "idle";
//                break;
//        }
//        Debug.Log("I am currently " + dbg_msg);
//    }//end function

//    void FixedUpdate()
//    {

//    }

//    /*Move to a coordinate.*/
//    void face(Vector3 location)
//    {
//        Vector3 rotate_angle = location - transform.position;
//        mobility.rotate_until(Quaternion.LookRotation(rotate_angle));
//    }

//    /*Follow a moving target, and match the speed.*/
//    void follow(float distance)
//    {
//        if (distance > b_room[1] || distance < 0.0f)
//        { //follow
//            /*Too far to be in breathing room.
//              Accelerate until matching the same velocity as the player.*/
//            if (mobility.velocity.magnitude < target.mobility.velocity.magnitude)
//            {
//                mobility.change_velocity(1.0f); //if not fast enough, accelerate.
//            }
//            else if (mobility.velocity.magnitude > target.mobility.velocity.magnitude)
//            {
//                mobility.change_velocity(-1.0f);//if too fast, decelerate.
//            }
//        }
//        else if (distance > b_room[0] || distance < b_room[1])
//        {
//            //within acceptable range.
//            mobility.accelerate_until(target.mobility.velocity.magnitude);
//            if (target.mobility.velocity.magnitude == 0.0f)
//            {
//                mobility.accelerate_until(0f);
//            }
//            else if (target.mobility.velocity.magnitude < 0.0f)
//            {
//                mobility.accelerate_until(target.mobility.velocity.magnitude);
//            }
//        }
//    }

//    void OnGUI()
//    {
//        string display = (
//            "\nCurrent Velocity: \t" + mobility.velocity.z.ToString() +
//            "\nCurrent Acceleration: \t" + mobility.acceleration.ToString() +
//            "\nCurrent Position X: \t" + transform.position.x.ToString() +
//            "\nCurrent Position Y: \t" + transform.position.y.ToString() +
//            "\nCurrent Position Z: \t" + transform.position.z.ToString()
//        );
//    }
//}
