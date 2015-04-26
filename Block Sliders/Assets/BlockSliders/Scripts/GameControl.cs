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
		public AudioSource marchMusic;

		//sound effect
		public AudioSource BlockSlideFX;
		public AudioSource DoorSoundFX;
		public AudioSource PewFX;
		public AudioSource FallFX;
		public AudioSource BreakFX;
		public AudioSource LockInFX;
		public AudioSource SplashFX;

		//options variables
		public int blockSpeedPref;
		public int playerSpeedPref;
		public int fxVolumePref;
		public int musicVolumePref;
		public int ramAmount;
		public int joystickPref;

		//save variables
		public int highestUnlock;
		public bool paused;
		public bool pausedMenu;
		private AudioSource gameMusic;
		public float[,] scoreData = new float[100, 5];
		private int fn = 100;
		private Vector3 offSet;

		//runs on game start
		void Awake ()
		{
				if (!File.Exists (Application.persistentDataPath + "/GameData.bin")) {
						NewGame ();
				}
				offSet = Vector3.zero;
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
						Screen.sleepTimeout = 999999;
				}
				Load ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				ControlMusic ();
				SetVolumes ();

				if (paused) {
						PlayBlockSlide (false);
						System.GC.Collect (System.GC.MaxGeneration, GCCollectionMode.Forced);
				}
				if (highestUnlock > fn) {
						highestUnlock = fn;
				}
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
		
				for (int i = 0; i < fn; i++) {
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

		/// <summary>
		/// Controls the music.
		/// </summary>
		protected void ControlMusic ()
		{
				if (!gameMusic.isPlaying) {
						int whatToPlay = UnityEngine.Random.Range (1, 4);
						switch (whatToPlay) {
						case 1:
								gameMusic = cupidMusic;
								break;
						case 2:
								gameMusic = frogMusic;
								break;
						case 3:
								gameMusic = marchMusic;
								break;
						}
						gameMusic.Play ();
				}

		}

		protected void SetVolumes ()
		{
				gameMusic.volume = (float)musicVolumePref / 10;

				BlockSlideFX.volume = (float)FxVolumePref / 50;
				DoorSoundFX.volume = (float)FxVolumePref / 10;
				PewFX.volume = (float)FxVolumePref / 10;
				FallFX.volume = (float)FxVolumePref / 10;
				BreakFX.volume = (float)FxVolumePref / 10;
				LockInFX.volume = (float)FxVolumePref / 10;
				SplashFX.volume = (float)FxVolumePref / 10;
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

		public int FxVolumePref {
				get { return fxVolumePref; }
				set { 
						fxVolumePref = value; 
						ApplyChanges ("FxVolumePref", value);
				}
		}

		public int MusicVolumePref {
				get { return musicVolumePref;}
				set {
						musicVolumePref = value;
						ApplyChanges ("MusicVolumePref", value);
				}
		}
		
		public int JoystickPref {
				get { return joystickPref; }
				set {
						joystickPref = value;
						ApplyChanges ("JoystickPref", value);
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

		public int CurrentLevel {
				get { return Application.loadedLevel - 1;}
		}

		/// <summary>
		/// Gets the level count.
		/// </summary>
		/// <value>The level count.</value>
		public int LevelCount {
				get { return fn;}
		}

		public int CurrentCount {
				get { return Application.levelCount - 2;}
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

		public Vector3 Offset {
				get { return offSet;}
				set { offSet = value;}
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

	#region Play Audio FX
		//methods to play sound effects
		public void PlayBlockSlide (bool playing)
		{
				if (BlockSlideFX != null) {
						if (!BlockSlideFX.isPlaying && playing) {
								BlockSlideFX.volume = (float)FxVolumePref / 50;
								BlockSlideFX.Play ();
								BlockSlideFX.loop = true;
						}

						if (!playing) {
								BlockSlideFX.Stop ();
								BlockSlideFX.loop = false;
						}
				}

		}

		public void PlayDoorSound ()
		{
				if (DoorSoundFX != null) {
						if (!DoorSoundFX.isPlaying) {
								DoorSoundFX.volume = (float)FxVolumePref / 10;
								DoorSoundFX.Play ();
						}
				}
		}

		public void PlayPew ()
		{
				if (PewFX != null) {
						if (!PewFX.isPlaying) {
								PewFX.volume = (float)FxVolumePref / 10;
								PewFX.Play ();
						}
				}
		}

		public void PlayFall ()
		{
				if (FallFX != null) {
						if (!FallFX.isPlaying) {
								FallFX.volume = (float)FxVolumePref / 10;
								FallFX.Play ();
						}
				}
		}

		public void PlayBreak ()
		{
				if (BreakFX != null) {
						if (!BreakFX.isPlaying) {
								BreakFX.volume = (float)FxVolumePref / 10;
								BreakFX.Play ();
						}
				}
		}

		public void PlayLockIn ()
		{
				if (LockInFX != null) {
						if (!LockInFX.isPlaying) {
								LockInFX.volume = (float)FxVolumePref / 10;
								LockInFX.Play ();
						}
				}
		}

		public void PlaySplash ()
		{
				if (SplashFX != null) {
						if (!SplashFX.isPlaying) {
								SplashFX.volume = (float)FxVolumePref / 10;
								SplashFX.Play ();
						}
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
		
				Stream stream = File.Open (Application.persistentDataPath + "/GameData.bin", FileMode.OpenOrCreate);

				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Binder = new VersionDeserializationBinder ();
				SaveData data = new SaveData (PlayerPrefs.GetInt ("BlockSpeedPref"), PlayerPrefs.GetInt ("PlayerSpeedPref"), PlayerPrefs.GetInt ("FxVolumePref"), PlayerPrefs.GetInt ("MusicVolumePref"), PlayerPrefs.GetInt ("JoystickPref"), ramAmount, highestUnlock, scoreData);


				formatter.Serialize (stream, data);
		
				stream.Close ();

//				var sr = File.CreateText (Application.persistentDataPath + "/Debug.txt");
//				sr.Write ("Block speed:\t" + PlayerPrefs.GetInt ("BlockSpeedPref").ToString () + "\n");
//				sr.Write ("FX Volume:\t" + PlayerPrefs.GetInt ("FxVolumePref").ToString () + "\n");
//				sr.Write ("Player speed:\t" + PlayerPrefs.GetInt ("PlayerSpeedPref").ToString () + "\n");
//				sr.Write ("Ram Amount:\t" + ramAmount.ToString () + "\n");
//				sr.Write ("Highest Unlock:\t" + highestUnlock.ToString () + "\n");
//
//				sr.Close ();
		}

		/// <summary>
		/// game data load
		/// </summary>
		public void Load ()
		{
				if (File.Exists (Application.persistentDataPath + "/GameData.bin")) {
		
		
						GameControl.SetEnvironmentVariables ();

						Stream stream = File.OpenRead (Application.persistentDataPath + "/GameData.bin");
		
						BinaryFormatter formatter = new BinaryFormatter ();
						formatter.Binder = new VersionDeserializationBinder ();
						SaveData data = (SaveData)formatter.Deserialize (stream);
		
						stream.Close ();

						//change current data to imported save data
						BlockSpeedPref = data.blockSpeedPref;
						PlayerSpeedPref = data.playerSpeedPref;
						FxVolumePref = data.fxVolumePref;
						MusicVolumePref = data.musicVolumePref;
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
								scoreData [i, j] = (j + i + 2 * 10) + (UnityEngine.Random.Range (1.1f, 11.1f) / 10);
						}
				}

				SaveData data = new SaveData (3, 3, 9, 9, 1, ramAmount, 1, scoreData);

				formatter.Serialize (stream, data);
		
				stream.Close ();

				BlockSpeedPref = data.blockSpeedPref;
				PlayerSpeedPref = data.playerSpeedPref;
				FxVolumePref = data.fxVolumePref;
				ramAmount = data.ramAmount;
				highestUnlock = data.highestUnlock;
				scoreData = data.scoreData;
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
		public int fxVolumePref;
		public int musicVolumePref;
		public int joystickPref;
		public int ramAmount;
		public int highestUnlock;
		public float[,] scoreData;

		public SaveData ()
		{
		}
		public SaveData (int bsp, int psp, int fxv, int mv, int jp, int ra, int hu, float[,] ld)
		{
				blockSpeedPref = bsp;
				playerSpeedPref = psp;
				fxVolumePref = fxv;
				musicVolumePref = mv;
				joystickPref = jp;
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
