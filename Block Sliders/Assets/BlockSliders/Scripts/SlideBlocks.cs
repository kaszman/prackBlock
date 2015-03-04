using UnityEngine;
using System.Collections;

public class SlideBlocks : MonoBehaviour {

	public float speed;
	private Vector2 velocity;
	
	void FixedUpdate()
	{	
		velocity = Vector2.zero;

		//left (add in sprite motion)
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			velocity.x -= 1;
		}
		
		//right (add in sprite motion)
		if (Input.GetKey(KeyCode.RightArrow))
		{
			velocity.x += 1;
		}
		
		//up (add in sprite motion)
		if (Input.GetKey(KeyCode.UpArrow))
		{
			velocity.y += 1;
		}
		
		//down (add in sprite motion)
		if (Input.GetKey(KeyCode.DownArrow))
		{
			velocity.y -= 1;
		}

		velocity.Normalize();

		//get input from screen tilt
		//if (Application.isMobilePlatform)
		//{
			velocity = Input.acceleration.normalized;
		//}		

		velocity *= speed;
		rigidbody2D.AddForce(velocity);
		
		
	}
}
