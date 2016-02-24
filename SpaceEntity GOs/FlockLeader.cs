using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockLeader : MonoBehaviour {

	// managing flock
	public int MAX_FLOCK_SIZE = 6;
	public float MIN_SEPARATION = 160f; //take into account the size of the ships instead!
	public List<Ship> flock = new List<Ship> ();
    float leaderWeight = 0; //change leaderweight to be equal to how many ships are in the flock
	Ship leader;
	Vector3 averagePosition = new Vector3();
	Vector3 averageRotation = new Vector3();



	void Awake() {
		leader = GetComponent<Ship> ();
	}

	void Start() {
		flock.Add (leader);
        leaderWeight = 1;
	}

	void Update() {
		averagePosition = Vector3.zero;
		//averageRotation = Vector3.zero;
		foreach (var x in flock) {
            if (x != null)
                averagePosition += x.transform.position;
            else
            {
                flock.Remove(x);
                break;
            }
			//averageRotation += x.transform.rotation;
		}
		averagePosition = new Vector3(averagePosition.x/flock.Count, averagePosition.y/flock.Count, averagePosition.z/flock.Count);
		//averageRotation = new Vector3(averageRotation.x/followers.Count, averageRotation.y/followers.Count, averageRotation.z/followers.Count);
	}


	// Thanks to Red3d flocking theory
	public Vector3 GetHeading(BasicAi instance) {
        //Debug.Log(instance.GetInstanceID() + " at: " + instance.transform.position + ", Avg Pos: " + averagePosition);

        //ensure minimal separation
		foreach (var x in flock) {
			Vector3 vectorToFlockmate = x.transform.position - instance.transform.position;
			if (x.gameObject.GetInstanceID() != instance.gameObject.GetInstanceID() && vectorToFlockmate.magnitude < MIN_SEPARATION) {
				//Debug.Log(instance.gameObject.GetInstanceID() + " at " + instance.transform.position + " compared to " + x.GetInstanceID() + " at " + x.transform.position + "Too close with " + vectorToFlockmate.magnitude + "/" + MIN_SEPARATION + " separation");
                return (- vectorToFlockmate);
			}
		}

        Vector3 weightedPosition = leader.transform.position * leaderWeight + averagePosition; // weight leader position more/less heavily  
        return new Vector3(weightedPosition.x / (leaderWeight + 1), weightedPosition.y / (leaderWeight + 1), weightedPosition.z / (leaderWeight + 1)); ;
	}




}//end class
