using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
		public static LevelSelect select;

		/// <summary>
		/// Changes the level to selected level if it is unlocked.
		/// </summary>
		/// <param name="toLevel">To level.</param>
		public void ChangeLevel (int toLevel)
		{
				System.GC.Collect ();
				if (isLevelUnlocked (toLevel)) {
						if (toLevel == 0) {
								Application.LoadLevel ("Menu");
						} else {
								Application.LoadLevel ("Lvl" + toLevel.ToString ());
						}
				}
		}

		/// <summary>
		/// Resumes the game based on farthest unlock
		/// </summary>
		public void ResumeGame ()
		{
				//GameControl.control.Load ();
				ChangeLevel (GameControl.control.HighestUnlock);
		}

		/// <summary>
		/// Starts a new game;
		/// </summary>
		public void NewGame ()
		{
				System.GC.Collect ();
				GameControl.control.NewGame ();
				GameControl.control.Save ();
				GameControl.control.UnlockLevel (1);
				Application.LoadLevel ("Lvl1");
		}

		private bool isLevelUnlocked (int level)
		{
				if (level <= GameControl.control.HighestUnlock) {
						return true;
				} else {
						return false;
				}
		}


}