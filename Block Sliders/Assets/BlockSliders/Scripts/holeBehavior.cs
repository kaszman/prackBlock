using UnityEngine;
using System.Collections;

public class holeBehavior : MonoBehaviour {

	public object ownerObject;
	public SpriteRenderer ownerRenderer;	
	public Sprite filledHole;
	private GameObject collided;
	private bool filled;

	// Use this for initialization
	void Start ()
	{
		filled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
//what to do while the hole is empty
		if (filled == false)
		{
		//if the object is a moveable block
			if (collided.tag == "Enemy")
			{	
				//destroy the offencing object
				Destroy(collided);

				//set filled to true
				filled = true;
			}

			//if the object is a player, (restart? move back before hole?)
			else if (collided.tag == "Player")
			{
				//decide what to do
			}
		}

//what to do if the hole is filled
		else
		{
	//set the texture to the filled hole texture
			ownerRenderer.sprite = filledHole;
		}
	}

	void OnCollisionEnter2D(Collision2D col) 
	{			
		collided = new GameObject();
	}
}
