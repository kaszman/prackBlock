using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{

	//sliders to control player and block speeds
	public UnityEngine.UI.Slider blockspeedSlider;
	public UnityEngine.UI.Slider playerspeedSlider;


	// Use this for initialization
	void Start ()
	{
		PlayerPrefs.SetInt("MobileSpeed", 5);
		PlayerPrefs.SetInt("PlayerSpeed", 1);
		PlayerPrefs.Save();
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
		PlayerPrefs.SetInt("MobileSpeed", (int)blockspeedSlider.value);
		PlayerPrefs.Save();
	}

	public void changePlayerSpeed()
	{
		PlayerPrefs.SetInt("PlayerSpeed", (int)playerspeedSlider.value);
		PlayerPrefs.Save();
	}
	#endregion
}
