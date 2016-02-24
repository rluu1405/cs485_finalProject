using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class text : targetFollow {

	GameObject tf;
	string dist;
	float x;
	public Text distance;
	// Update is called once per frame

    GameObject player;

	void Start()
	{
		tf = GameObject.Find ("followTarget");
		distance = GetComponent<Text> (); 
		distance.text = "";
        player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update () {
	
		if(tf.GetComponent<targetFollow>().target != null)
		{
			x = Mathf.RoundToInt(Mathf.Abs(tf.GetComponent<targetFollow>().target.position.magnitude - player.transform.position.magnitude));
			dist = x.ToString();
		    //Debug.Log (dist);
			distance.text = dist;

			if(!tf.GetComponent<targetFollow>().target.renderer.isVisible)
			{
				distance.text = "";
			}

		}
		else
		{
			distance.text = "";
		}
	}
}
