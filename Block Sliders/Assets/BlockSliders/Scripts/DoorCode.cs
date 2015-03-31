using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorCode : MonoBehaviour
{

		//parameters recieved from the game
		public int nextScene;
		public AudioSource playerCollide;
		public Canvas successCanv;

		//collision object
		private GameObject collisionObject;
		protected bool doorSoundStarted;

		void Start ()
		{	
				doorSoundStarted = false;
		}

		void FixedUpdate ()
		{

		}

		//update method only used for sound cause it's stupid
		void Update ()
		{
				if (doorSoundStarted && GameControl.control.paused == true) {
						//loadingSlider.value = loadingSlider.value + .05f;
						successCanv.enabled = true;
				} else {
						//loadingSlider.value = 0;
						successCanv.enabled = false;
				}
				if (!playerCollide.isPlaying) {
						if (doorSoundStarted == true) {	
								//Time.timeScale = 1f;
								//GameControl.control.Paused = false;
//								if (nextScene == 0) {
//										Application.LoadLevel ("Menu");
//								}
//				else {
//										Application.LoadLevel ("Lvl" + nextScene.ToString ());
//								}
						}	
						//what to do when the player hits the door
						//playerCollide.Play ();			
						//GameControl.control.Paused = true;
				}
		}



		void handleCollisionEvents ()
		{
//				//if the object is a player, move to the next level and play sound
//				if (collisionObject.tag == "Player") {
//						if (!playerCollide.isPlaying) {
//								if (doorSoundStarted == true) {
//										GameControl.control.UnlockLevel (nextScene);
//										if (nextScene == 0) {
//												Application.LoadLevel ("Menu");
//										} else {
//												Application.LoadLevel ("Lvl" + nextScene.ToString ());
//										}
//								}	
//								//what to do when the player hits the door
//								playerCollide.Play ();			
//								GameControl.control.Save ();
//								GameControl.control.Paused = true;
//						}			
//						doorSoundStarted = true;
//				}
		}
	
		/// <summary>
		/// Sets collisionObject to the offending collision object
		/// </summary>
		/// <param name="col">Col.</param>
		void OnCollisionEnter2D (Collision2D col)
		{			
				//if the object is a player, move to the next level and play sound
				if (col.gameObject.tag == "Player") {
//						if (!playerCollide.isPlaying) {
//								if (doorSoundStarted == true) {
//										//Time.timeScale = 1f;
//										GameControl.control.UnlockLevel (nextScene);
//										//	GameControl.control.Paused = false;
//										LevelSelect.select.ChangeLevel (nextScene);
//								}	
//								//what to do when the player hits the door
//								playerCollide.Play ();			
//								GameControl.control.Save ();
//								//GameControl.control.Paused = true;
//						}		
						playerCollide.Play ();
						doorSoundStarted = true;
						GameControl.control.UnlockLevel (nextScene);
						GameControl.control.Save ();
						GameControl.control.Paused = true;
				}
		}
}
