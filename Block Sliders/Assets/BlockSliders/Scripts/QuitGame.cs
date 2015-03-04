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
}
