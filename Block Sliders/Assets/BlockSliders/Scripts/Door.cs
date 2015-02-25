using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	//parameters recieved from the game
	public string nextScene;

	//collision object
	private GameObject collisionObject;

	// Update is called once per frame
	void FixedUpdate ()
	{
		handleCollisionEvents();
	}


	void handleCollisionEvents()
	{
		//if the object is a player, (restart? move back before hole?)
		if (collisionObject.tag == "Player")
		{
			//LoadNextLevel
			Application.LoadLevel(nextScene);
		}
	}
	
	/// <summary>
	/// Sets collisionObject to the offending collision object
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionEnter2D(Collision2D col) 
	{			
		collisionObject = col.gameObject;
	}
}
