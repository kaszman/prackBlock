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
		public int blockSpeedPref;
		public int playerSpeedPref;
		public int ramAmount;

		//save variables
		public int highestUnlock = 1;
		public SaveData data = new SaveData ();
		public bool paused;
		public bool pausedMenu;
		private AudioSource gameMusic;
		public float[,] scoreData = new float[12, 5];

		//runs on game start
		void Awake ()
		{
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
				paused = false;
				gameMusic = cupidMusic;
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
				Save ();

				int hold = Application.levelCount - 1;
		
//				float[,] temp = new float[hold, 5];
				for (int i = 0; i < 12; i++) {
						for (int j = 0; j < 5; j++) {
								string temp = i.ToString () + " " + j.ToString ();
								Debug.Log (temp);
								scoreData [i, j] = i;
						}
				}
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				ControlMusic ();
		}

	#region LeaderBoard methods

		/// <summary>
		/// A method to sort the leaderboard and insert the given time in the propper place.
		/// </summary>
		/// <param name="finalTime">Final time.</param>
		/// <param name="LevelID">Level.</param>
		public void addLeaderboardTime (float finalTime, int LevelID)
		{
				//float[] levelData = 1;
		}

	#endregion

	#region Music Method
		protected void ControlMusic ()
		{
				if (!gameMusic.isPlaying) {
						int whatToPlay = UnityEngine.Random.Range (1, 5);
						switch (whatToPlay) {
						case 1:
								gameMusic = cupidMusic;
								break;
						case 2:
								gameMusic = frogMusic;
								break;
						case 3:
								gameMusic = eyedMusic;
								break;
						case 4:
								gameMusic = marchMusic;
								break;
						}
						gameMusic.Play ();
				}

		}


	#endregion

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

		public float[,] GetScoreData ()
		{
				return scoreData;
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
				SaveData data = new SaveData (PlayerPrefs.GetInt ("BlockSpeedPref"), PlayerPrefs.GetInt ("PlayerSpeedPref"), ramAmount, highestUnlock, scoreData);

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
				scoreData = data.scoreData;
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
				int hold = Application.levelCount - 2;

				float[,] temp = new float[hold, 5];
				for (int i = 0; i < hold; i++) {
						for (int j = 0; j < 4; j++) {
								temp [i, j] = j;
						}
				}

				SaveData data = new SaveData (2, 3, ramAmount, 1, temp);

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
public class SaveData
{
		public int blockSpeedPref;
		public int playerSpeedPref;
		public int ramAmount;
		public int highestUnlock;
		public float[,] scoreData;

		public SaveData ()
		{
		}
		public SaveData (int bsp, int psp, int ra, int hu, float[,] ld)
		{
				this.blockSpeedPref = bsp;
				this.playerSpeedPref = psp;
				this.ramAmount = ra;
				this.highestUnlock = hu;
				scoreData = ld;
		}
}
