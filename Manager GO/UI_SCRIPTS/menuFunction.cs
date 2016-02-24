using UnityEngine;
using System.Collections;

public class menuFunction : MonoBehaviour {

	public void startGame()
	{
		Application.LoadLevel("Coalition");
	}

	public void endGame()
	{
		Application.Quit ();
	}

}
