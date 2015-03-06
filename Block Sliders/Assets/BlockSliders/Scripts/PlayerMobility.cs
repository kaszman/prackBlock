using UnityEngine;
using System.Collections;

public class PlayerMobility : MonoBehaviour {

	public Animator anim;
	public GameObject owner;
	public PolygonCollider2D polygon;
	public CircleCollider2D circle;
	public float speed;
	private Vector2 velocity;

	void Start()
	{
		polygon.enabled = true;
	}

	void FixedUpdate()
	{
		velocity = Vector2.zero;

		if (Application.isMobilePlatform)
		{
			doMobileMovement();
		}
		else
		{
			doKeyboardMovement();
		}
		
		velocity.Normalize();
		velocity *= speed;
		rigidbody2D.AddForce(velocity);
	}

	#region Movement methods
	/// <summary>
	/// Does the keyboard movement script.
	/// </summary>
	private void doKeyboardMovement()
	{
		//up
		if (Input.GetKey("w"))
		{
			velocity.y += 1;
			anim.SetTrigger("WalkingUp");
			circle.enabled = false;
			polygon.enabled = true;
		}
		
		//down
		if (Input.GetKey("s"))
		{
			velocity.y -= 1;
			anim.SetTrigger("WalkingDown");
			circle.enabled = false;
			polygon.enabled = true;
		}
		
		//left
		if (Input.GetKey("a"))
		{
			velocity.x -= 1;
			anim.SetTrigger("WalkingLeft");
			polygon.enabled = false;
			circle.enabled = true;
		}
		
		//right
		if (Input.GetKey("d"))
		{
			velocity.x += 1;
			anim.SetTrigger("WalkingRight");
			polygon.enabled = false;
			circle.enabled = true;
		}
	}

	private void doMobileMovement()
	{
		Vector2 tempDest = Input.GetTouch(0).deltaPosition;
		velocity = tempDest;
	}

	#endregion
}