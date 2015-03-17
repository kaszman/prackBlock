using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

public class GameControl : MonoBehaviour
{
		//variables
		public static GameControl control;
		public AudioSource cupidMusic;
		public AudioSource frogMusic;
		public AudioSource eyedMusic;
		public AudioSource marchMusic;

		//options variables
		private int blockSpeedPref;
		private int playerSpeedPref;
		private int ramAmount;
		private int highestUnlock;
		private SaveData data = new SaveData ();
		private bool paused;
		private bool pausedMenu;
		private AudioSource gameMusic;

		//runs on game start
		void Awake ()
		{
				gameMusic = cupidMusic;
				//do this if the game has never been saved
				if (!File.Exists (Application.persistentDataPath + "/GameData.bin")) {
						NewGame ();
				} else {
						Load ();
				}
				
				//singleton
				if (control == null) {
						DontDestroyOnLoad (gameObject);
						control = this;
				} else if (control != this) {
						Destroy (gameObject);
				}
				gameMusic.Play ();
				paused = false;
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
				Save ();
		}

		private void ApplyChanges (String variableName, float floatValue)
		{
				PlayerPrefs.SetFloat (variableName, floatValue);
				Save ();
		}

		private void ApplyChanges (String variableName, String stringValue)
		{
				PlayerPrefs.SetString (variableName, stringValue);
				Save ();
		}
	#endregion

	#region game data access

//	public void GameMusic
//	{
//
//	}

		/// <summary>
		/// Unlocks the level.
		/// </summary>
		/// <param name="level">Level.</param>
		public void UnlockLevel (int level)
		{
				Load ();
				if (level > HighestUnlock) {
						highestUnlock = level;
				}
				Save ();
		}

		/// <summary>
		/// Gets the highest unlock.
		/// </summary>
		/// <value>The highest unlock.</value>
		public int HighestUnlock {
				get { return highestUnlock;}
		}

		/// <summary>
		/// Gets or sets the game paused state.
		/// </summary>
		/// <value><c>true</c> if paused; otherwise, <c>false</c>.</value>
		public bool Paused {
				get { return paused; }
				set { paused = value; }
		}

		public bool PausedMenu {
				get { return pausedMenu; }
				set { pausedMenu = value; }
		}

		/// <summary>
		/// Checks if level is unlocked
		/// </summary>
		/// <returns><c>true</c>, if level unlocked was ised, <c>false</c> otherwise.</returns>
		/// <param name="level">Level.</param>
		private bool isLevelUnlocked (int level)
		{
				if (level <= GameControl.control.HighestUnlock) {
						return true;
				} else {
						return false;
				}
		}
	
	#endregion
	
	#region game data save/load/new
		/// <summary>
		/// game data save
		/// </summary>
		public void Save ()
		{
				GameControl.SetEnvironmentVariables ();
		
				Stream stream = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.Create);
		
				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Binder = new VersionDeserializationBinder ();
				SaveData data = new SaveData (PlayerPrefs.GetInt ("BlockSpeedPref"), PlayerPrefs.GetInt ("PlayerSpeedPref"), ramAmount, HighestUnlock);

				formatter.Serialize (stream, data);
		
				stream.Close ();
		}

		/// <summary>
		/// game data load
		/// </summary>
		public void Load ()
		{
				if (!File.Exists (Application.persistentDataPath + "/GameData.bin")) {
						//break;
				}
		
		
				GameControl.SetEnvironmentVariables ();
		
				Stream stream = null;
		
				try {
						stream = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.Open);
				} catch (FileNotFoundException e) {
						//	break;
				}
		
				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Binder = new VersionDeserializationBinder ();
				this.data = (SaveData)formatter.Deserialize (stream);
		
				stream.Close ();

				//change current data to imported save data
				BlockSpeedPref = data.blockSpeedPref;
				PlayerSpeedPref = data.playerSpeedPref;
				ramAmount = data.ramAmount;
				highestUnlock = data.highestUnlock;
		}

		/// <summary>
		/// Replaces the game data with new game data.
		/// </summary>
		public void NewGame ()
		{
				GameControl.SetEnvironmentVariables ();
		
				Stream stream = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.Create);
		
				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Binder = new VersionDeserializationBinder ();
				SaveData data = new SaveData (2, 3, ramAmount, 1);

				formatter.Serialize (stream, data);
		
				stream.Close ();
		}

	#endregion
		/* SetEnvironmentVariables required to avoid run-time code generation that will break iOS compatibility
 	 * Suggested by Nico de Poel:
	 * http://answers.unity3d.com/questions/30930/why-did-my-binaryserialzer-stop-working.html?sort=oldest
 	 */
		private static void SetEnvironmentVariables ()
		{
				Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
		}
}

//class to hold save data.
[Serializable]
class SaveData
{
		public int blockSpeedPref;
		public int playerSpeedPref;
		public int ramAmount;
		public int highestUnlock;

		public SaveData ()
		{
		}
		public SaveData (int bsp, int psp, int ra, int hu)
		{
				this.blockSpeedPref = bsp;
				this.playerSpeedPref = psp;
				this.ramAmount = ra;
				this.highestUnlock = hu;
		}

}
