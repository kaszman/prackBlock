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
//		private LevelData[] thisLevelData;


		//runs on game start
		void Awake ()
		{
				Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
				//do this if the game has never been saved
				if (!File.Exists (Application.persistentDataPath + "/GameData.bin")) {
						Save ();
						//		NewGame ();
				}
				
				//singleton
				if (control == null) {
						DontDestroyOnLoad (gameObject);
						control = this;
				} else if (control != this) {
						Destroy (gameObject);
				}
				gameMusic.Play ();
		}


		// Use this for initialization
		void Start ()
		{
				Load ();
				GameControl.control.RamAmount = 5;
				//what to do with the screen on mobile devices
				if (Application.isMobilePlatform) {
						Screen.orientation = ScreenOrientation.LandscapeLeft;
						Screen.sleepTimeout = SleepTimeout.NeverSleep;
				}
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{

		}


	#region player preference changes
		public int BlockSpeedPref {
				get { return blockSpeedPref; }
				set { 
						blockSpeedPref = value;
						ApplyChanges ("BlockSpeedPref", value);
				}
		}

		public int PlayerSpeedPref {
				get { return playerSpeedPref; }
				set { 
						playerSpeedPref = value; 
						ApplyChanges ("PlayerSpeedPref", value);
				}
		}

		public int RamAmount {
				get { 
						Load ();
						return ramAmount; 
				}
				set {
						ramAmount = value;
						Save ();
				}
		}

		private void ApplyChanges (String variableName, int intValue)
		{
				PlayerPrefs.SetInt (variableName, intValue);
		}

		private void ApplyChanges (String variableName, float floatValue)
		{
				PlayerPrefs.SetFloat (variableName, floatValue);
		}

		private void ApplyChanges (String variableName, String stringValue)
		{
				PlayerPrefs.SetString (variableName, stringValue);
		}
	#endregion

	#region game data change

//		public void UnlockLevel (int level)
//		{
//				Load ();
//				thisLevelData [level].lockSetting = false;
//				Save ();
//		}

	#endregion

	#region game data read

//		public LevelData[] GetLevelData ()
//		{
//				return thisLevelData;
//		}
//
//		public string highestUnlock ()
//		{
//				int highestIndex = 0;
//				foreach (LevelData data in thisLevelData) {
//						if (data.lockSetting == true) {
//								highestIndex++;
//						}
//				}
//				return thisLevelData [highestIndex].name;
//		}

	#endregion


	#region game data save
		/// <summary>
		/// game data save
		/// </summary>
		public void Save ()
		{
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Create (Application.persistentDataPath + "/GameData.bin");

				//create save data holder and set its data
				SaveData data = new SaveData ();
				data.blockSpeedPref = PlayerPrefs.GetInt ("BlockSpeedPref");
				data.playerSpeedPref = PlayerPrefs.GetInt ("PlayerSpeedPref");
				data.ramAmount = ramAmount;
//				data.data = thisLevelData;

				bf.Serialize (file, data);
				file.Close ();
		}

		/// <summary>
		/// game data load
		/// </summary>
		public void Load ()
		{
				if (File.Exists (Application.persistentDataPath + "/GameData.bin")) {
						BinaryFormatter bf = new BinaryFormatter ();
						FileStream file = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.Open);
						SaveData data = (SaveData)bf.Deserialize (file);
						file.Close ();

						//change current data to imported save data
						BlockSpeedPref = data.blockSpeedPref;
						PlayerSpeedPref = data.playerSpeedPref;
						ramAmount = data.ramAmount;
//						thisLevelData = data.GetLevelData;
				}
		}

		/// <summary>
		/// Replaces the game data with new game data.
		/// </summary>
		public void NewGame ()
		{
				//path and formatter
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Create (Application.persistentDataPath + "/GameData.bin");
		
				//save data object
				SaveData data = new SaveData ();

				//set save data to default values
				data.blockSpeedPref = 5;
				data.playerSpeedPref = 1;
				data.ramAmount = ramAmount;
//				data.NewLevelData ();

				//save 
				bf.Serialize (file, data);
				file.Close ();
		}

	#endregion
}

//class to hold save data.
[Serializable]
class SaveData : MonoBehaviour
{
		public int blockSpeedPref;
		public int playerSpeedPref;
		public int ramAmount;
		//public LevelData[] data;


//		public LevelData[] GetLevelData {
//				get { return data; }
//		}

//		public void SetLevelData (LevelData[] currentLevelData)
//		{
//				int index = 0;
//				foreach (LevelData lvlData in data) {
//						lvlData.name = currentLevelData [index].name;
//						lvlData.changeLockValue (currentLevelData [index].lockSetting);
//				}
//
//		}

//		public void NewLevelData ()
//		{
//				data = new LevelData[100];
//				for (int i = 0; i > data.Length; i++) {
//						string level = "Lvl" + i.ToString ();
//						data [i] = new LevelData (level);
//				}
//		}

}
