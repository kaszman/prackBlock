using UnityEngine;
using System.Collections;

public class AcidHole : MonoBehaviour
{
		//private use variables
		private GameObject collisionObject;

//		// Use this for initialization
//		void Start ()
//		{
//	
//		}
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
						Application.LoadLevel ("Lvl" + GameControl.control.CurrentLevel.ToString ());
				}
		}
}
