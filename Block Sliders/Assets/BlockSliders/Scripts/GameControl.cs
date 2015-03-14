using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{
	//variables
	public static GameControl control;
	public AudioSource gameMusic;

	//options variables
	private int blockSpeedPref;
	private int playerSpeedPref;
	private int ramAmount;

	/// <summary>
	/// necessary. just do this. always. don't edit.
	/// if you really must know, it makes this a singleton
	/// </summary>
	void Awake()
	{
		//do this if the came has never been saved
		if (!File.Exists(Application.persistentDataPath + "/GameData.bin"))
		{
			PlayerPrefs.SetInt("PlayerSpeedPref", 1);
			PlayerPrefs.SetInt("BlockSpeedPref", 5);
		}

		if (control == null)
		{
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if(control != this)
		{
			Destroy(gameObject);
		}
		gameMusic.Play();
		gameMusic.loop = true;
	}

	// Use this for initialization
	void Start ()
	{
		loadData();
		GameControl.control.RamAmount = 5;
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


	#region player preference changes
	public int BlockSpeedPref
	{
		get { return blockSpeedPref; }
		set 
		{ 
			blockSpeedPref = value;
			ApplyChanges("BlockSpeedPref", value);
		}
	}

	public int PlayerSpeedPref
	{
		get { return playerSpeedPref; }
		set 
		{ 
			playerSpeedPref = value; 
			ApplyChanges("PlayerSpeedPref", value);
		}
	}

	public int RamAmount
	{
		get
		{ 
			loadData();
			return ramAmount; 
		}
		set
		{
			ramAmount = value;
			saveData();
		}
	}

	private void ApplyChanges(String variableName, int intValue)
	{
		PlayerPrefs.SetInt(variableName, intValue);
	}

	private void ApplyChanges(String variableName, float floatValue)
	{
		PlayerPrefs.SetFloat(variableName, floatValue);
	}

	private void ApplyChanges(String variableName, String stringValue)
	{
		PlayerPrefs.SetString(variableName, stringValue);
	}
	#endregion


	#region game data save
	/// <summary>
	/// game data save
	/// </summary>
	public void saveData()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/GameData.bin");

		//create save data holder and set its data
		SaveData data = new SaveData();
		data.blockSpeedPref = PlayerPrefs.GetInt("BlockSpeedPref");
		data.playerSpeedPref = PlayerPrefs.GetInt("PlayerSpeedPref");
		data.ramAmount = ramAmount;

		bf.Serialize(file, data);
		file.Close();
	}

	/// <summary>
	/// game data load
	/// </summary>
	public void loadData()
	{
		if(File.Exists(Application.persistentDataPath + "/GameData.bin"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/GameData.bin", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();

			//change current data to imported save data
			BlockSpeedPref = data.blockSpeedPref;
			PlayerSpeedPref = data.playerSpeedPref;
			ramAmount = data.ramAmount;
		}
	}

	#endregion
}

//class to hold save data.
[Serializable]
class SaveData
{
	public int blockSpeedPref;
	public int playerSpeedPref;
	public int ramAmount;
}