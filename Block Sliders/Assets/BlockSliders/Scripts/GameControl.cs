using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
	public static AudioSource option1;
	public static AudioSource option2;
	public static AudioSource option3;
	public static AudioSource option4;
	private static float mobileSpeedTweak;


	static GameControl instance;
	
	public static GameControl Instance {
		get {
			if (instance == null) {
				instance = new GameControl ();
			}
			return instance;
		}
	}
	//speed tweak for mobile responsiveness
	public static float MobileSpeedTweak
	{
		get { return mobileSpeedTweak; }
		set { mobileSpeedTweak = value; }
	}

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);
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
