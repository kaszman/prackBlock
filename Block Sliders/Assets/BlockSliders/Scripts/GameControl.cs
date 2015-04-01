﻿using UnityEngine;
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
		public int highestUnlock;
		public bool paused;
		public bool pausedMenu;
		private AudioSource gameMusic;
		public float[,] scoreData = new float[16, 5];
		private int fn = 16;

		//runs on game start
		void Awake ()
		{
//				//do this if the game has never been saved
//				if (File.Exists (Application.persistentDataPath + "/GameData.bin")) {
//						NewGame ();
//				} else {
//				Load ();
//				}
				
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
				//what to do with the screen on mobile devices
				if (Application.isMobilePlatform) {
						Screen.orientation = ScreenOrientation.LandscapeLeft;
						Screen.sleepTimeout = SleepTimeout.NeverSleep;
				}
				//Load ();
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

				//CONTAINS A BUG
				//populates all scores for that level with highest score

				//load the level's time score data into 5 floats
				float a = scoreData [(LevelID - 1), 0];
				float b = scoreData [(LevelID - 1), 1];
				float c = scoreData [(LevelID - 1), 2];
				float d = scoreData [(LevelID - 1), 3];
				float e = scoreData [(LevelID - 1), 4];

				//sort the floats
				float[] sortArray = new float[6]{a, b, c, d, e, finalTime};

				float temp = 0;
		
				for (int i = 0; i < sortArray.Length; i++) {
						for (int sort = 0; sort < sortArray.Length - 1; sort++) {
								if (sortArray [sort] > sortArray [sort + 1]) {
										temp = sortArray [sort + 1];
										sortArray [sort + 1] = sortArray [sort];
										sortArray [sort] = temp;
								}       
						}   

				}
				//Array.Sort (sortArray, (x, y) => x.CompareTo (y));
				//Array.Sort (sortArray);

				//put the floats back into the score data
				scoreData [(LevelID - 1), 0] = sortArray [0];
				scoreData [(LevelID - 1), 1] = sortArray [1];
				scoreData [(LevelID - 1), 2] = sortArray [2];
				scoreData [(LevelID - 1), 3] = sortArray [3];
				scoreData [(LevelID - 1), 4] = sortArray [4];

				//save
				Save ();
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
						Save ();
				}
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
		
				Stream stream = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.Open);

				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Binder = new VersionDeserializationBinder ();
				SaveData data = new SaveData (PlayerPrefs.GetInt ("BlockSpeedPref"), PlayerPrefs.GetInt ("PlayerSpeedPref"), ramAmount, highestUnlock, scoreData);


				formatter.Serialize (stream, data);
		
				stream.Close ();

				var sr = File.CreateText (Application.persistentDataPath + "/Debug.txt");
				sr.Write ("Block speed:\t" + PlayerPrefs.GetInt ("BlockSpeedPref").ToString () + "\n");
				sr.Write ("Player speed:\t" + PlayerPrefs.GetInt ("PlayerSpeedPref").ToString () + "\n");
				sr.Write ("Ram Amount:\t" + ramAmount.ToString () + "\n");
				sr.Write ("Highest Unlock:\t" + highestUnlock.ToString () + "\n");

				sr.Close ();
		}

		/// <summary>
		/// game data load
		/// </summary>
		public void Load ()
		{
				if (File.Exists (Application.persistentDataPath + "/GameData.bin")) {
		
		
						GameControl.SetEnvironmentVariables ();
		
//						Stream stream = null;
//		
//						try {
//								stream = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.Open);
//						} catch (FileNotFoundException e) {
						Stream stream = File.OpenRead (Application.persistentDataPath + "/GameData.bin");
						//	}
		
						BinaryFormatter formatter = new BinaryFormatter ();
						formatter.Binder = new VersionDeserializationBinder ();
						SaveData data = (SaveData)formatter.Deserialize (stream);
		
						stream.Close ();

						//change current data to imported save data
						BlockSpeedPref = data.blockSpeedPref;
						PlayerSpeedPref = data.playerSpeedPref;
						ramAmount = data.ramAmount;
						highestUnlock = data.highestUnlock;
						scoreData = data.scoreData;
				}
		}

		/// <summary>
		/// Replaces the game data with new game data.
		/// </summary>
		public void NewGame ()
		{
				GameControl.SetEnvironmentVariables ();
		
				Stream stream = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.OpenOrCreate);
		
				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Binder = new VersionDeserializationBinder ();

				for (int i = 0; i < fn; i++) {
						for (int j = 0; j < 5; j++) {
								string temp = i.ToString () + " " + j.ToString ();
								scoreData [i, j] = j + 1 * 10;
						}
				}

				SaveData data = new SaveData (2, 3, ramAmount, 1, scoreData);

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
				blockSpeedPref = bsp;
				playerSpeedPref = psp;
				ramAmount = ra;
				highestUnlock = hu;
				scoreData = ld;
		}
}

#region sealed class
public sealed class VersionDeserializationBinder : SerializationBinder
{ 
		public override Type BindToType (string assemblyName, string typeName)
		{ 
				if (!string.IsNullOrEmpty (assemblyName) && !string.IsNullOrEmpty (typeName)) { 
						Type typeToDeserialize = null; 
			
						assemblyName = Assembly.GetExecutingAssembly ().FullName; 
			
						// The following line of code returns the type. 
						typeToDeserialize = Type.GetType (String.Format ("{0}, {1}", typeName, assemblyName)); 
			
						return typeToDeserialize; 
				} 
		
				return null; 
		} 
}
#endregion
