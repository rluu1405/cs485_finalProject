using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Test for drawing menu (clickable buttons)
public class menuscript : MonoBehaviour {

	// Use this for initialization

	public GameObject menu;
	bool menuOn;

	void Start () {
		menu.SetActive (false);
		menuOn = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(Input.GetKeyDown(KeyCode.I) && !menuOn)
		{
			menu.SetActive(true);
			menuOn = true;

		}
		else if(Input.GetKeyDown(KeyCode.I) && menuOn)
		{
			menu.SetActive(false);
			menuOn = false;
		}
	}
}
