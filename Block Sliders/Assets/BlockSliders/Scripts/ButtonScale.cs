using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScale : MonoBehaviour
{
		public Text drawText;
		public int counter;

		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		public void doTheStuff ()
		{
				counter++;
				//GameControl.control.UnlockLevel (counter);
				drawText.text = counter.ToString ();//GameControl.control.highestUnlock ();
		}
}
