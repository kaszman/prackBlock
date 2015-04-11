using UnityEngine;
using System.Collections;

public class PlayerMobility : MonoBehaviour
{

		public Animator anim;
		public GameObject owner;
		public PolygonCollider2D polygon;
		public CircleCollider2D circle;
		public float speed;
		private Vector2 velocity;

		void Start ()
		{
				polygon.enabled = true;
		}

		float Speed {
				set { speed = value;}
		}

		void FixedUpdate ()
		{
				velocity = Vector2.zero;

				if (Application.isMobilePlatform) {
						doMobileMovement ();
				} else {
						doKeyboardMovement ();
				}
		
				velocity.Normalize ();	
				velocity *= speed * PlayerPrefs.GetInt ("PlayerSpeedPref");
				GetComponent<Rigidbody2D> ().AddForce (velocity);		
				doAnimation ();
		}

	#region Movement methods
		/// <summary>
		/// Does the keyboard movement.
		/// </summary>
		private void doKeyboardMovement ()
		{
				//up
				if (Input.GetKey ("w")) {
						velocity.y += 1;
						//anim.SetTrigger("WalkingUp");
						circle.enabled = false;
						polygon.enabled = true;
				}
		
				//down
				if (Input.GetKey ("s")) {
						velocity.y -= 1;
						//anim.SetTrigger("WalkingDown");
						circle.enabled = false;
						polygon.enabled = true;
				}
		
				//left
				if (Input.GetKey ("a")) {
						velocity.x -= 1;
						//anim.SetTrigger("WalkingLeft");
						polygon.enabled = false;
						circle.enabled = true;
				}
		
				//right
				if (Input.GetKey ("d")) {
						velocity.x += 1;
						//anim.SetTrigger("WalkingRight");
						polygon.enabled = false;
						circle.enabled = true;
				}
		}

		/// <summary>
		/// Does the mobile movement.
		/// </summary>
		private void doMobileMovement ()
		{
				Vector2 tempDest = Input.GetTouch (0).deltaPosition;
				velocity = tempDest;
				velocity *= PlayerPrefs.GetInt ("PlayerSpeedPref");
		}
	#endregion

		/// <summary>
		/// Does proper animation based on movement direction.
		/// </summary>
		private void doAnimation ()
		{
				if (velocity.magnitude <= .05f) {
						anim.ResetTrigger ("WalkingUp");
						anim.ResetTrigger ("WalkingDown");
						anim.ResetTrigger ("WalkingLeft");
						anim.ResetTrigger ("WalkingRight");
				}
				//walking right
				if (velocity.x >= .9f) {// || velocity.normalized.x >= .7f)
						anim.SetTrigger ("WalkingRight");
				} else {
						anim.ResetTrigger ("WalkingRight");
				}
				//walking left
				if (velocity.x <= -.9f) {// || velocity.normalized.x <= -.7f)
						anim.SetTrigger ("WalkingLeft");
				} else {
						anim.ResetTrigger ("WalkingLeft");
				}
				//walking up
				if (velocity.y >= .1f) {// || velocity.normalized.y >= .2f)
						anim.SetTrigger ("WalkingUp");
				} else {
						anim.ResetTrigger ("WalkingUp");
				}
				//walking down
				if (velocity.y <= -.1f) {// || velocity.normalized.y >= -.2f)
						anim.SetTrigger ("WalkingDown");
				} else {
						anim.ResetTrigger ("WalkingDown");
				}

		}
}