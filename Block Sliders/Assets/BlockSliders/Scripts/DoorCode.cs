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
		private bool paused;

		void Start ()
		{	
				doorSoundStarted = false;
				paused = false;
		}

		//update method only used for sound cause it's stupid
		void Update ()
		{
				handleCollisionEvents ();

				//control game speed
				if (paused) { 
						Time.timeScale = 0;
				}
		}

		// Update is called once per frame
		void FixedUpdate ()
		{



		}


		void handleCollisionEvents ()
		{
				//if the object is a player, move to the next level and play sound
				if (collisionObject.tag == "Player") {			
						paused = true;
						if (!playerCollide.isPlaying) {
								if (doorSoundStarted == true) {
										GameControl.control.UnlockLevel (nextScene);
										Application.LoadLevel ("Lvl" + nextScene.ToString ());
										GameControl.control.Save ();
								}	
								playerCollide.Play ();	
						}			
						doorSoundStarted = true;

				}
		}
	
		/// <summary>
		/// Sets collisionObject to the offending collision object
		/// </summary>
		/// <param name="col">Col.</param>
		void OnCollisionEnter2D (Collision2D col)
		{			
				collisionObject = col.gameObject;
		}
}
