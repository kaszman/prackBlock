using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	Object coco;

	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.E))
		{
			Destroy(coco);
		}
	}


void OnCollisionEnter2D(Collision2D col) {
	if (col.gameObject.tag == "Enemy")
		coco = col.gameObject;
}
}
