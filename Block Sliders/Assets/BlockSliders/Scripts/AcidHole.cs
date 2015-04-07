using UnityEngine;
using System.Collections;

public class AcidHole : MonoBehaviour
{

		//parameters recieved from the game	
		public int sceneNumber;

		//private use variables
		private GameObject collisionObject;
		private string currentSceneName;


//		// Use this for initialization
		void Start ()
		{
				currentSceneName = "Lvl" + sceneNumber.ToString ();
		}
//	
//		// Update is called once per frame
//		void Update ()
//		{
//	
//		}

		/// <summary>
		/// Sets collisionObject to the offending collision object
		/// </summary>
		/// <param name="col">Col.</param>
		void OnCollisionEnter2D (Collision2D col)
		{			
				//if the object is a moveable block
				if (col.gameObject.tag == "Enemy") {	
						//destroy the offencing object
						Destroy (col.gameObject);

			
				}
		
		//if the object is a player
		else if (col.gameObject.tag == "Player") {
						//Reset the level
						Application.LoadLevel (currentSceneName);
				}
		}
}
