using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Formation : MonoBehaviour 
{

    public GameObject FlockPoint;
    public Vector3 FollowOffset;
    Vector3 FollowDistance;
    List<BasicAi> Followers;

    //public enum FormationType // want in basic ai
    //{
    //    Square,
    //    Circle,
    //    Triangle,
    //}
    //public FormationType F = FormationType.Square;

    // "there is more work to be done" - Every politician ever
    public Vector3 Formate()
    {
        //Followers.Add(FollowerShip);
        //foreach(var Ship in Followers)
        //{
        //    switch(F)
        //    {
        //        case FormationType.Square:
        //            {
                        
        //            }
        //            break;
        //        case FormationType.Circle:
        //            {

        //            }
        //            break;
        //        case FormationType.Triangle:
        //            {

        //            }
        //            break;           

        //    }
        //}
        FollowDistance = FlockPoint.transform.position + FollowOffset;
        Debug.Log("Returning: " + FollowDistance + "BTW this: " + this.name);
        return FollowDistance;
    }
    
}
