/* I want to keep the FSM structure that I have here but not necessarily how it calculates movement
 *   -Jeff
 * 
 * Ideally, we want AI to avoid collisions, avoid obstacles, flock with teammates, priority-based arbitration
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FighterAi : BasicAi
{

    override protected void UpdateFightState() 
    {
        //Debug.Log("AI's target:  " + targetShip.name);

        if (targetShip != null)
        {
            // If target is facing you, evade
            if (IsFacing(targetShip.transform.forward, transform.position) && maneuverTimer > maneuverTimerBound)
            {
                aiShip.Shield(true);
                heading = Maneuver();
                //Debug.Log("At Flee(): " + heading);
                maneuverTimer = 0;
            }
            else // otherwise, seek and destroy
            {
                aiShip.Shield(false);
                heading = Seek();
                //Debug.Log("At Seek(): " + heading);
            }

            // Check for collisions
            if (maneuverTimer > maneuverTimerBound)
            {
                heading += ModifyForCollisionAvoidance();
                //Debug.Log("After ModifyCollisionAvoidance(): " + heading);
                maneuverTimer = 0;
            }



            maneuverTimer += Time.deltaTime;

            // Move to that point
            MoveTo(heading);

            // Update timer to prevent setting new maneuvers every frame

                
            // Fire if AI thinks it should
            //var range = (heading - transform.position).magnitude;
            //if (range < minimumRange)
            //    tendencyToFire = 1f;
            //else if (range < optimalRange)
            //    tendencyToFire = 0.9f;
            //else
            //    tendencyToFire = baseTendencyToFire;

            FireLasers();
        }
        else
            state = State.travelling;

    }

}
    