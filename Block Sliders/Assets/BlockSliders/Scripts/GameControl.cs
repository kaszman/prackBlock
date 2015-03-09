using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{

	//sliders to control player and block speeds
	public UnityEngine.UI.Slider blockspeedSlider;
	public UnityEngine.UI.Slider playerspeedSlider;


	// Use this for initialization
	void Start ()
	{
		loadOptions();
		//what to do with the screen on mobile devices
		if (Application.isMobilePlatform)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{


	}

	/// <summary>
	/// Saves the game data.
	/// </summary>
	public void saveOptions()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "options.bin");
		SaveData data = new SaveData();
		data.blockSpeed = PlayerPrefs.GetInt("BlockSpeed");
		data.playerSpeed = PlayerPrefs.GetInt("PlayerSpeed");

		bf.Serialize(file, data);
		file.Close();
	}

	/// <summary>
	/// Loads the game data.
	/// </summary>
	public void loadOptions()
	{
		if(File.Exists(Application.persistentDataPath))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "options.bin", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();

			PlayerPrefs.SetInt("BlockSpeed", data.blockSpeed);
			PlayerPrefs.SetInt("PlayerSpeed", data.playerSpeed);
			blockspeedSlider.value = PlayerPrefs.GetInt("BlockSpeed");
			playerspeedSlider.value = PlayerPrefs.GetInt("PlayerSpeed");
		}
	}

	#region Methods to apply slider settings
	public void changeBlockSpeed()
	{
		PlayerPrefs.SetInt("BlockSpeed", (int)blockspeedSlider.value);
	}

	public void changePlayerSpeed()
	{
		PlayerPrefs.SetInt("PlayerSpeed", (int)playerspeedSlider.value);
	}
	#endregion
}

//class to hold save data.
[Serializable]
class SaveData
{
	public int blockSpeed;
	public int playerSpeed;
}