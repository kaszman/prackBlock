using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScale : MonoBehaviour
{
		public Text drawText;
		private int counter;

		// Use this for initialization
		void Start ()
		{
				counter = GameControl.control.HighestUnlock;
		}
	
		// Update is called once per frame
		void Update ()
		{
				GameControl.control.Load ();
				drawText.text = GameControl.control.HighestUnlock.ToString ();

		}
	
		public void doTheStuff ()
		{
				GameControl.control.UnlockLevel (GameControl.control.HighestUnlock + 1);
				GameControl.control.Save ();
		}
}
