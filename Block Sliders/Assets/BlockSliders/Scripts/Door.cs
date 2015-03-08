using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	//parameters recieved from the game
	public string nextScene;
	public AudioSource playerCollide;
	public GameControl controlObject;

	//collision object
	private GameObject collisionObject;
	protected bool doorSoundStarted;
	private bool paused;

	void Start()
	{	
		doorSoundStarted = false;
		paused = false;
	}

	//update method only used for sound cause it's stupid
	void Update()
	{
		handleCollisionEvents();

		//control game speed
		if(paused)
		{ 
			Time.timeScale = 0;
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{



	}


	void handleCollisionEvents()
	{
		//if the object is a player, move to the next level and play sound
		if (collisionObject.tag == "Player")
		{
			if (!playerCollide.isPlaying)
			{
				if (doorSoundStarted == true)
				{
				  Application.LoadLevel(nextScene);
				}	
				playerCollide.Play();	
				paused = true;
			}			
			doorSoundStarted = true;

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
