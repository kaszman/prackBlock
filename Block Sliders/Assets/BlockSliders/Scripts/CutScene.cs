using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutScene : MonoBehaviour
{
		//canvas to display
		public Canvas outside;
		public Canvas cave;
		public Canvas inside;

		//dialog boxes
		public Text momText;
		public Text laz1;
		public Text laz2;
		public Text laz3;

		//change scene
		private bool sceneOne;
		private bool sceneTwo;
		private bool sceneThree;

		// Use this for initialization
		void Start ()
		{
				sceneOne = true;
				sceneTwo = false;
				sceneThree = false;
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				outside.enabled = sceneOne;
				cave.enabled = sceneTwo;
				inside.enabled = sceneThree;
				doCutscene ();
		}

		public void changeCanvas (int destination)
		{
				if (destination == 1) {
						sceneOne = true;
						sceneTwo = false;
						sceneThree = false;
				}
				if (destination == 2) {
						sceneOne = false;
						sceneTwo = true;
						sceneThree = false;
				}
				if (destination == 3) {
						sceneOne = false;
						sceneTwo = false;
						sceneThree = true;
				}
		}

		public void doCutscene ()
		{
				//if on level one
				//display mom text for 5 seconds

				//display laz level 1 text for 5 seconds


				//if on level 2
				//display laz level 2 text


				//if on level 3
				//display human text story

				//display laz noise
		}

		public void Skip ()
		{
				Application.LoadLevel (1);
		}
}
