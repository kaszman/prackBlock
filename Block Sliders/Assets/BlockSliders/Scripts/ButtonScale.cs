using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScale : MonoBehaviour
{
		public Text drawText;

		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				drawText.text = GameControl.control.HighestUnlock.ToString ();

		}
	
		public void doTheStuff ()
		{
				GameControl.control.UnlockLevel (GameControl.control.HighestUnlock + 1);
		}
}
