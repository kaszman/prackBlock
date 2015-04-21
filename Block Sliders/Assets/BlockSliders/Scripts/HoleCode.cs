using UnityEngine;
using System.Collections;

public class HoleCode : MonoBehaviour
{

		//parameters recieved from the game
		public GameObject ownerObject;
		public SpriteRenderer ownerRenderer;	
		public Sprite filledHoleSprite;	
		private string currentSceneName;

		//private use variables
		private GameObject collisionObject;
		private bool filled = false;

		// Use this for initialization
		void Start ()
		{
				currentSceneName = "Lvl" + GameControl.control.CurrentLevel.ToString ();
		}

		/// <summary>
		/// Handle collision event
		/// </summary>
		/// <param name="col">Col.</param>
		void OnCollisionEnter2D (Collision2D col)
		{			
				//if the object is a moveable block
				if (col.gameObject.tag == "Enemy") {	

						GameControl.control.PlayLockIn ();

						//destroy the offencing object
						Destroy (col.gameObject);
			
						//set filled to true
						filled = true;
			
						//change the sprite to a filled hole
						ownerRenderer.sprite = filledHoleSprite;
			
						if (filled) {
								//make hole walkable
								ownerObject.GetComponent<Collider2D> ().enabled = false;
						}
			
				}
		
		//if the object is a player, restart level
		else if (col.gameObject.tag == "Player") {
						GameControl.control.PlayFall ();
						//Reset the level
						Application.LoadLevel (currentSceneName);
				}
		}
}
