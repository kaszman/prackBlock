using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorCode : MonoBehaviour
{

		//parameters recieved from the game
		public int nextScene;
		public Canvas successCanv;

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
				if (doorSoundStarted && GameControl.control.paused == true) {
						successCanv.enabled = true;
				} else {
						successCanv.enabled = false;
				}

		}

		/// <summary>
		/// Handle collision events
		/// </summary>
		/// <param name="col">Col.</param>
		void OnCollisionEnter2D (Collision2D col)
		{			
				//if the object is a player, move to the next level and play sound
				if (col.gameObject.tag == "Player") {	
						GameControl.control.PlayDoorSound ();
						doorSoundStarted = true;
						GameControl.control.UnlockLevel (nextScene);
						GameControl.control.Save ();
						GameControl.control.Paused = true;
				}
		}
}
