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
				if (drawText != null) {
						drawText.text = GameControl.control.HighestUnlock.ToString ();
				}

		}
	
		// Update is called once per frame
		void Update ()
		{

		}
	
		public void doTheStuff ()
		{
				GameControl.control.Load ();
				if (counter < GameControl.control.LevelCount) {
						counter++;
				}
				drawText.text = GameControl.control.HighestUnlock.ToString ();

				GameControl.control.UnlockLevel (counter);
				GameControl.control.Save ();
		}

		public void calibration ()
		{
				GameControl.control.Offset = Input.acceleration;
		}
}
