using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public float sensitivity_x = 70;
    public float sensitivity_y = 70;


    internal float mouseHorizontal { get; private set; }
    internal float mouseVertical { get; private set; }
    internal float mouseLeft { get; private set; }
    internal float mouseRight { get; private set; }

    internal float roll { get; private set; }
    internal float pitch  { get; private set; }
    internal float yaw { get; private set; }
    internal float throttle  { get; private set; }
    internal float cutEngine  { get; private set; }
    internal float maxEngine { get; private set; }
    internal float setZeroVelocity { get; private set; }

    internal float targetNextEnemy { get; private set; }
    internal float targetNextFriendly { get; private set; }
    internal float acquireTargetAtMouse { get; private set; }
    internal float communicate { get; private set; }
    internal float shieldOn { get; private set; }

    internal float inventoryToggle { get; private set; }
    internal float cancel { get; private set; }
    internal float acquireTargetAtReticle { get; private set; }
    internal float tradeToggle { get; private set; }
    internal float nextLevel { get; private set; }


	
	// Update is called once per frame
	void Update () 
    {
        // get mouse input
        mouseHorizontal = Input.GetAxis("MouseX") * sensitivity_x;
        mouseVertical = Input.GetAxis("MouseY") * sensitivity_y;
        mouseLeft = Input.GetAxis("Fire1");
        mouseRight = Input.GetAxis("Fire2");

        // get keypresses
        roll = Input.GetAxis("Roll"); // rotate about x
        yaw = Input.GetAxis("Yaw"); // rotate about z
        pitch = Input.GetAxis("Pitch"); // rotate about z
        throttle = Input.GetAxis("Throttle");
        cutEngine = Input.GetAxis("Halt");  
        maxEngine = Input.GetAxis("FullThrottle");
        targetNextEnemy = Input.GetAxis("TargetNextEnemy");
        targetNextFriendly = Input.GetAxis("TargetNextFriendly");
        acquireTargetAtMouse = Input.GetAxis("AcquireTargetAtReticle");
        communicate = Input.GetAxis("Communicate");
        cancel = Input.GetAxis("Input Button Cancel"); // weird name demanded by event manager
        shieldOn = Input.GetAxis("ShieldOn");
        inventoryToggle = Input.GetAxis("Inventory");
        nextLevel = Input.GetAxis("NextLevel");



        // properties now accessed through reference to this script
	}
}
