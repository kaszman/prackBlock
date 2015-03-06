using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	//what to do with the screen on mobile devices
	if (Application.isMobilePlatform)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}

	}
}
