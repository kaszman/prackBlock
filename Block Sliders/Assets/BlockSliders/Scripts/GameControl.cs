using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
	public Camera mainCamera;
	//sliders to control player and block speeds
	public UnityEngine.UI.Slider blockspeedSlider;
	public UnityEngine.UI.Slider playerspeedSlider;


	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (mainCamera);
		PlayerPrefs.SetInt("BlockSpeed", 5);
		PlayerPrefs.SetInt("PlayerSpeed", 1);
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

	#region Methods to apply slider settings
	public void changeBlockSpeed()
	{
		PlayerPrefs.SetInt("BlockSpeed", (int)blockspeedSlider.value);
		PlayerPrefs.Save();
	}

	public void changePlayerSpeed()
	{
		PlayerPrefs.SetInt("PlayerSpeed", (int)playerspeedSlider.value);
		PlayerPrefs.Save();
	}
	#endregion
}
