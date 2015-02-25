using UnityEngine;
using System.Collections;

public class PlayerMobility : MonoBehaviour {

	public Animator anim;
	public GameObject owner;
	public float speed;
	private Vector2 velocity;
	
	void FixedUpdate()
	{
		velocity = Vector2.zero;
		
		//up
		if (Input.GetKey("w"))
		{
			velocity.y += 1;
			anim.SetTrigger("WalkingUp");
		}
		
		//down
		if (Input.GetKey("s"))
		{
			velocity.y -= 1;
			anim.SetTrigger("WalkingDown");
		}
		
		//left
		if (Input.GetKey("a"))
		{
			velocity.x -= 1;
			anim.SetTrigger("WalkingSide");
		}
		
		//right
		if (Input.GetKey("d"))
		{
			velocity.x += 1;
			anim.SetTrigger("WalkingSide");
		}
		
		velocity.Normalize();
		velocity *= speed;
		rigidbody2D.AddForce(velocity);
	}
	
}