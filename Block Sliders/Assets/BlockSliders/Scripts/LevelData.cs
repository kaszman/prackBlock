using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class LevelData
{
		public string name;
		public bool lockSetting;
		
		public LevelData (string name)
		{
				this.name = name;
				lockSetting = false;
		}
				
		public void changeLockValue (bool lockValue)
		{
				lockSetting = lockValue;
				GameControl.control.Save ();
		}
}


