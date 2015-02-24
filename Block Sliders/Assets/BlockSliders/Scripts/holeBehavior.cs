using UnityEngine;
using System.Collections;

public class holeBehavior : MonoBehaviour {

	public SpriteRenderer ownerRenderer;	
	public Sprite filledHoleSprite;

	private GameObject coco;
	private bool filled = false;

	// Use this for initialization
	//void Start ()
	//{
	//	filled = false;
	//}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
//what to do while the hole is empty
		if (filled == false)
		{
			handleCollisionEvents();
		}

//what to do if the hole is filled
		else
		{
	//set the texture to the filled hole texture
			ownerRenderer.sprite = filledHoleSprite;
		}
	}

	void OnCollisionEnter2D(Collision2D col) 
	{			
		coco = new GameObject();
		coco = col.gameObject;
	}

	void handleCollisionEvents()
	{
		//if the object is a moveable block
		if (coco.tag == "Enemy")
		{	
			//destroy the offencing object
			Destroy(coco);
			
			//set filled to true
			filled = true;
		}
		
		//if the object is a player, (restart? move back before hole?)
		else if (coco.tag == "Player")
		{
			//decide what to do
		}
	}
}
