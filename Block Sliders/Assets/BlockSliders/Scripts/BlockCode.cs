using UnityEngine;
using System.Collections;

public class BlockCode: MonoBehaviour
{

		public float speed;
		private Vector2 velocity;

		void Start ()
		{
				gameObject.transform.Rotate (0, Random.Range (-1, 2) * 180, Random.Range (0, 5) * 90);
		}
	
		void FixedUpdate ()
		{	
				velocity = Vector2.zero;

				//get input from screen tilt
				if (Application.isMobilePlatform) {
						doMobileMovement ();
				} else {
						doKeyboardMovement ();
				}

				velocity *= speed * PlayerPrefs.GetInt ("BlockSpeedPref");
				GetComponent<Rigidbody2D> ().AddForce (velocity);
		}
		
		#region Movement methods
		/// <summary>
		/// Does the keyboard movement script.
		/// </summary>
		private void doKeyboardMovement ()
		{
				//left (add in sprite motion)
				if (Input.GetKey (KeyCode.LeftArrow)) {
						velocity.x -= 1;
				}
		
				//right (add in sprite motion)
				if (Input.GetKey (KeyCode.RightArrow)) {
						velocity.x += 1;
				}
		
				//up (add in sprite motion)
				if (Input.GetKey (KeyCode.UpArrow)) {
						velocity.y += 1;
				}
		
				//down (add in sprite motion)
				if (Input.GetKey (KeyCode.DownArrow)) {
						velocity.y -= 1;
				}
				if (velocity.magnitude > .3f) {
						GameControl.control.PlayBlockSlide (true);
				} else {
						GameControl.control.PlayBlockSlide (false);
				}
				velocity.Normalize ();
		}
		
		private void doMobileMovement ()
		{

				velocity = Input.acceleration;	
				if (velocity.magnitude > .3f) {
						GameControl.control.PlayBlockSlide (true);
				} else {
						GameControl.control.PlayBlockSlide (false);
				}
				velocity.Normalize ();
		}

		void OnDestroy ()
		{
				GameControl.control.PlayBlockSlide (false);
		}
		#endregion
}
