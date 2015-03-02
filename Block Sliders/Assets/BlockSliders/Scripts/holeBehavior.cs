using UnityEngine;
using System.Collections;

public class holeBehavior : MonoBehaviour {

	//parameters recieved from the game
	public GameObject ownerObject;
	public Camera controlObj;
	public SpriteRenderer ownerRenderer;	
	public Sprite filledHoleSprite;	
	public string currentSceneName;

	//private use variables
	private GameObject collisionObject;
	private bool filled = false;

	// Use this for initialization
	//void Start ()
	//{
	//	filled = false;
	//}
	
	// Update is called once per frame
	void Update ()
	{
//what to do while the hole is empty
		if (!filled)
		{
			handleCollisionEvents();
		}

//what to do if the hole is filled
		if (filled)
		{
			//make hole walkable
			ownerObject.collider2D.enabled = false;
		}
	}
	
	/// <summary>
	/// Determine what to do when the hole is collided with
	/// </summary>
	void handleCollisionEvents()
	{
		//if the object is a moveable block
		if (collisionObject.tag == "Enemy")
		{	
			//destroy the offencing object
			Destroy(collisionObject);

			//set filled to true
			filled = true;

			//change the sprite to a filled hole
			ownerRenderer.sprite = filledHoleSprite;



		}
		
		//if the object is a player, (restart? move back before hole?)
		else if (collisionObject.tag == "Player")
		{
			//Reset the level
			Application.LoadLevel (currentSceneName);
		}
	}

	/// <summary>
	/// Sets collisionObject to the offending collision object
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionEnter2D(Collision2D col) 
	{			
		//collisionObject = new GameObject();
		collisionObject = col.gameObject;
	}
}
