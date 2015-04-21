using UnityEngine;
using System.Collections;

public class AcidHole : MonoBehaviour
{
		//private use variables
		private GameObject collisionObject;
		private string currentSceneName;


		// Use this for initialization
		void Start ()
		{
				currentSceneName = "Lvl" + GameControl.control.CurrentLevel.ToString ();
		}

		void OnCollisionEnter2D (Collision2D col)
		{			
				GameControl.control.PlaySplash ();

				//if the object is a moveable block
				if (col.gameObject.tag == "Enemy") {	
						//destroy the offencing object
						Destroy (col.gameObject);

			
				}
		//if the object is a player
				else if (col.gameObject.tag == "Player") {
						GameControl.control.PlayFall ();
						//Reset the level
						Application.LoadLevel (currentSceneName);
				}
		}
}
