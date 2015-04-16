using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutScene : MonoBehaviour
{
		//next buttons
		public Button next1;
		public Button next2;
		public Button next3;

		//canvas to display
		public Canvas outside;
		public Canvas cave;
		public Canvas inside;

		//dialog boxes
		public Image momText1;
		public Image momText2;
		public Image laz1;
		public Image laz2;
		public Image laz3;
		public Image human;

		//change scene
		private bool sceneOne;
		private bool sceneTwo;
		private bool sceneThree;

		//timer
		private float timerTime;

		// Use this for initialization
		void Start ()
		{
				timerTime = 0;
				sceneOne = true;
				sceneTwo = false;
				sceneThree = false;
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				timerTime += Time.deltaTime;
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
				timerTime = 0;
		}

		public void doCutscene ()
		{
				//if on level one
				if (sceneOne) {
						//display mom text for 5 seconds
						if (timerTime <= 5) {
								momText1.enabled = true;
								momText1.GetComponentInChildren<Text> ().enabled = true;

								laz1.enabled = false;
								laz1.GetComponentInChildren<Text> ().enabled = false;
						} 				
			//display laz level 1 text
				else {
								laz1.enabled = true;
								laz1.GetComponentInChildren<Text> ().enabled = true;

								momText1.enabled = false;
								momText1.GetComponentInChildren<Text> ().enabled = false;
								next1.interactable = true;
						}
				}


				//if on level 2
				//display laz level 2 text
				if (sceneTwo) {
						//display laz text for 5 seconds, then switch to mom
						if (timerTime <= 5) {
								momText2.enabled = false;
								momText2.GetComponentInChildren<Text> ().enabled = false;
				
								laz2.enabled = true;
								laz2.GetComponentInChildren<Text> ().enabled = true;
						} else {
								laz2.enabled = false;
								laz2.GetComponentInChildren<Text> ().enabled = false;
				
								momText2.enabled = true;
								momText2.GetComponentInChildren<Text> ().enabled = true;
								next2.interactable = true;
						}
				}

				//if on level 3
				if (sceneThree) {
						//display laz confused text
						if (timerTime <= 5) {
								human.enabled = false;
								human.GetComponentInChildren<Text> ().enabled = false;
				
								laz3.enabled = true;
								laz3.GetComponentInChildren<Text> ().enabled = true;
						}
						//display human text story
						//update the message every 5 seconds
						if (timerTime > 5 && timerTime < 12) {
								human.enabled = true;
								human.GetComponentInChildren<Text> ().enabled = true;

								//change what to say
								human.GetComponentInChildren<Text> ().text = "Hey there... ram... so bad news. You've wandered into a human intelligence testing arena.";

								laz3.enabled = false;
								laz3.GetComponentInChildren<Text> ().enabled = false;
						}
						if (timerTime > 12 && timerTime < 19) {
								human.enabled = true;
								human.GetComponentInChildren<Text> ().enabled = true;
				
								//change what to say
								human.GetComponentInChildren<Text> ().text = "But I do have some good news! Nick, the only person to ever successfully complete the test, is right here!";
				
								laz3.enabled = false;
								laz3.GetComponentInChildren<Text> ().enabled = false;
						}
						if (timerTime > 19 && timerTime < 26) {
								human.enabled = true;
								human.GetComponentInChildren<Text> ().enabled = true;
				
								//change what to say
								human.GetComponentInChildren<Text> ().text = ".... okay so it appears that he left. BETTER news! It looks like it'll be me guiding you through the test!";
				
								laz3.enabled = false;
								laz3.GetComponentInChildren<Text> ().enabled = false;
						}
						if (timerTime > 26 && timerTime < 33) {
								human.enabled = true;
								human.GetComponentInChildren<Text> ().enabled = true;
				
								//change what to say
								human.GetComponentInChildren<Text> ().text = "If we work together, I think we can beat this. Maybe even do better than some of the previous test takers!";
				
								laz3.enabled = false;
								laz3.GetComponentInChildren<Text> ().enabled = false;
						}
						if (timerTime > 33) {
								next3.interactable = true;
						}
				}
		}

		public void Skip ()
		{
				Application.LoadLevel (1);
		}
}
