using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour
{

		public void ChangeLevel (string toLevel)
		{
				if (isLevelUnlocked (toLevel)) {
						Application.LoadLevel (toLevel);
				}
		}

		public bool isLevelUnlocked (string level)
		{
//				GameControl.control.Load ();
//				LevelData[] temp = GameControl.control.GetLevelData ();
//				foreach (LevelData lvlData in temp) {
//						if (lvlData.name == level) {
//								return lvlData.lockSetting;
//						}
//				}
				return false;
		}
}
