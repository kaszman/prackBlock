using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour
{

		public void ChangeLevel (int toLevel)
		{
				if (isLevelUnlocked (toLevel)) {
						if (toLevel == 0) {
								Application.LoadLevel ("Menu");
						} else {
								Application.LoadLevel ("Lvl" + toLevel.ToString ());
						}
				}
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
