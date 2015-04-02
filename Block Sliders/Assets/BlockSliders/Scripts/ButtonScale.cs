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
				drawText.text = GameControl.control.HighestUnlock.ToString ();

		}
	
		public void doTheStuff ()
		{
				GameControl.control.Load ();
				if (counter < 18) {
						counter++;
				}
				GameControl.control.UnlockLevel (counter);
				GameControl.control.Save ();
		}
}
