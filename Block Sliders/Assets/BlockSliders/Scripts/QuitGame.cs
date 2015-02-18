using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {	

	public void quitGame(bool AreYouSure)
	{
		if (AreYouSure == true)
		{
			Application.Quit();
		}
	}





	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape))
	   {
			quitGame(true);
		}
	}


}
