using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMobility : MonoBehaviour
{
		public static PlayerMobility changer;
		public Animator anim;
		public GameObject owner;
		public CircleCollider2D circle;
		public float speed;
		public Vector2 velocity;
		public int tweak;

		public PlayerMobility ()
		{
		}

		void Start ()
		{
		}

		float Speed {
				set { speed = value;}
		}

		Vector2 Velocity {
				set {
						velocity = value;
				}
		}

		void FixedUpdate ()
		{
				velocity = Vector2.zero;
				if (Application.isMobilePlatform) {
						doMobileMovement ();
				} else {
						doKeyboardMovement ();
				}
		
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
				}
		
				//down
				if (Input.GetKey ("s")) {
						velocity.y -= 1;
				}
		
				//left
				if (Input.GetKey ("a")) {
						velocity.x -= 1;
				}
		
				//right
				if (Input.GetKey ("d")) {
						velocity.x += 1;
				}
				velocity.Normalize ();	
				velocity *= speed * PlayerPrefs.GetInt ("PlayerSpeedPref");
		}

		/// <summary>
		/// Does the mobile movement.
		/// </summary>
		private void doMobileMovement ()
		{
				if (GameControl.control.JoystickPref == 0) {
						Vector2 tempDest = Input.GetTouch (0).deltaPosition;
						velocity = tempDest;
						velocity.Normalize ();	
						velocity *= speed * PlayerPrefs.GetInt ("PlayerSpeedPref");
				} else {
						velocity = new Vector2 (CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical"));
						velocity *= speed * PlayerPrefs.GetInt ("PlayerSpeedPref");
				}
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
				if (velocity.x >= .9f) {
						anim.SetTrigger ("WalkingRight");
				} else {
						anim.ResetTrigger ("WalkingRight");
				}
				//walking left
				if (velocity.x <= -.9f) {
						anim.SetTrigger ("WalkingLeft");
				} else {
						anim.ResetTrigger ("WalkingLeft");
				}
				//walking up
				if (velocity.y >= .1f) {
						anim.SetTrigger ("WalkingUp");
				} else {
						anim.ResetTrigger ("WalkingUp");
				}
				//walking down
				if (velocity.y <= -.1f) {
						anim.SetTrigger ("WalkingDown");
				} else {
						anim.ResetTrigger ("WalkingDown");
				}

		}
}