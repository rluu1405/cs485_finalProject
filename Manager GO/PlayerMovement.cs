using UnityEngine;
using System.Collections;
using System.Text;

public class PlayerMovement : MonoBehaviour
{


    // not the same as keypress, press button once and this bool is true until
    //  the ship has come to a stop
    //private bool isBraking = false; 
    public float targetVelocity = 0f;
    private bool isControlledByMouse = true;  // move up to InputManager


    #region References to Attached GO Components
    private InputManager input;
    #endregion


    #region References to External GO Components
    private Player player;
    #endregion




    // Use this for references
    void Awake ()
    {
        input = GetComponent<InputManager>();
        player = GetComponent<Player>();
    }


    // Updates each frame
    void Update()
    {
        if (HasAcquiredReferences()) // If PlayerInput is not found, find it
        {   
            ApplyRotation();
            UpdateTargetVelocity();
            player.currentShip.accelerate_until(targetVelocity);
        }
    }


    // DEBUG GUI -- COMMENT THIS OUT ON RELEASE
    void OnGUI()
    {
        if (HasAcquiredReferences() && player.currentShip)
        {
            /*Used to display values for easy debugging.*/
            StringBuilder output = new StringBuilder();
            output.Append("\n\n\n\nThrottle:\t\t");
            output.Append(targetVelocity);

            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), output.ToString());
        }
    }




    void UpdateTargetVelocity()
    {
        if (input.cutEngine > 0)
        {
            targetVelocity = 0f;
        }
        else if (input.maxEngine > 0)
        {
            targetVelocity = player.currentShip.MAX_SPEED;
        }
        else if (targetVelocity < player.currentShip.MAX_SPEED && input.throttle > 0)
        {
            targetVelocity += 0.5f;
            player.currentShip.EngineSoundSource.pitch += 0.01f;
        }
        else if (targetVelocity > player.currentShip.MAX_NEGATIVE_SPEED && input.throttle < 0)
        {
            targetVelocity -= 0.5f;
            player.currentShip.EngineSoundSource.pitch -= 0.01f;
        }
    }

    void ApplyRotation()
    {
        if (isControlledByMouse)
            ComputePitchYawByMouse();
        else
            ComputePitchYawByKeyboard();

        // roll always controlled by keyboard
        if (input.roll < 0)
        {
            player.currentShip.transform.Rotate(Vector3.forward * player.currentShip.ROLL_FACTOR * Time.deltaTime);
        }
        else if (input.roll > 0)
        {
            player.currentShip.transform.Rotate(Vector3.back * player.currentShip.ROLL_FACTOR * Time.deltaTime);
        }
        //end handling of roll.
    }

    void ComputePitchYawByMouse()
    {
        player.currentShip.transform.Rotate(-input.mouseVertical * Time.deltaTime, input.mouseHorizontal * Time.deltaTime, 0);
    }

    void ComputePitchYawByKeyboard()
    {
        if (input.yaw > 0)
        {
            player.currentShip.transform.Rotate(Vector3.up * player.currentShip.YAW_FACTOR * Time.deltaTime);
        }
        //end handling of yaw
        if (input.pitch < 0)
        {
            player.currentShip.transform.Rotate(Vector3.right * player.currentShip.PITCH_FACTOR * Time.deltaTime);
        }
        else if (input.pitch > 0)
        {
            player.currentShip.transform.Rotate(Vector3.left * player.currentShip.PITCH_FACTOR * Time.deltaTime);
        }
    }

    public void EnableMouseControl(bool isEnabled)
    {
        isControlledByMouse = isEnabled;
    }

    bool HasAcquiredReferences()
    {
        if (input == null) // If PlayerInput is not found, find it
        {
            input = GetComponent<InputManager>();
            return false;
        }
        else if (player == null) // If InputMangaer not found, find it
        {
            player = GetComponent<Player>();
            return false;
        }
        else if (player.currentShip == null)
        {
            // wait
            return false;
        }
        else
            return true;
    }
}
