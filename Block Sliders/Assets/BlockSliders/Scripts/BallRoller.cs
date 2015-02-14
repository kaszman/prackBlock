using UnityEngine;
using System.Collections;

public class BallRoller : MonoBehaviour {

	private Vector3 acceleration;
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		acceleration = Vector3.zero;

		//up
		if (Input.GetKey("w"))
		{
			acceleration.x += 1;
		}
		
		//down
		if (Input.GetKey("s"))
		{
			acceleration.x -= 1;
		}
		
		//left
		if (Input.GetKey("a"))
		{
			acceleration.z += 1;
		}

		//right
		if (Input.GetKey("d"))
		{
			acceleration.z -= 1;
		}
		
		acceleration.Normalize();
		//acceleration *= speed;
		rigidbody.AddTorque(acceleration);
	}
}
