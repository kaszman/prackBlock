using UnityEngine;
using System.Collections;

public class DoorCode : MonoBehaviour
{

		//parameters recieved from the game
		public int nextScene;
		public AudioSource playerCollide;

		//collision object
		private GameObject collisionObject;
		protected bool doorSoundStarted;

		void Start ()
		{	
				doorSoundStarted = false;
		}

		//update method only used for sound cause it's stupid
		void Update ()
		{
				if (!playerCollide.isPlaying) {
						if (doorSoundStarted == true) {					
								GameControl.control.Save ();

								//Time.timeScale = 1f;
								GameControl.control.UnlockLevel (nextScene);
								GameControl.control.Paused = false;
								if (nextScene == 0) {
										Application.LoadLevel ("Menu");
								} else {
										Application.LoadLevel ("Lvl" + nextScene.ToString ());
								}
						}	
						//what to do when the player hits the door
						//playerCollide.Play ();			
						//GameControl.control.Paused = true;
				}
		}

		// Update is called once per frame
		void FixedUpdate ()
		{



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
						GameControl.control.Paused = true;
				}
		}
}
