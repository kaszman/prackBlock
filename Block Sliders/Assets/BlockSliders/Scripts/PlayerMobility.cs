using UnityEngine;
using System.Collections;

public class PlayerMobility : MonoBehaviour {
	
	public float speed;
	private Vector2 velocity;
	
	void FixedUpdate()
	{
		velocity = Vector2.zero;
		
		//up
		if (Input.GetKey("w"))
		{
			velocity.y += 1;
		}
		
		//down
		if (Input.GetKey("s"))
		{
			velocity.y -= 1;
		}
		
		//left
		if (Input.GetKey("a"))
		{
			velocity.x -= 1;
		}
		
		//right
		if (Input.GetKey("d"))
		{
			velocity.x += 1;
		}
		
		velocity.Normalize();
		velocity *= speed;
		rigidbody2D.AddForce(velocity);
	}
	
}