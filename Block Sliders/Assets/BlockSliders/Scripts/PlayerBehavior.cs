using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
		public SpriteRenderer player;
		public string tagCanCollide;
		public int timerLengthSec;
		public Text ramDisplay;

		protected bool ramming = false;

		private GameObject coco;
		private LevelSelect selector;
		private float timerRam;

		void Start ()
		{
				GameControl.control.Paused = false;
				GameControl.control.PausedMenu = false;
				GameControl.control.RamAmount = 5;
				if (Application.isMobilePlatform) {
						ramDisplay.canvas.scaleFactor = 3f;
				}
				GameControl.control.Save ();
				GameControl.control.Load ();
		}

		//pause control
		void Update ()
		{
				if (GameControl.control.Paused == true) {
						Time.timeScale = .00001f;
				} else {
						Time.timeScale = 1f;
				}
		}
		void FixedUpdate ()
		{		
				timerRam -= Time.deltaTime;

				if (timerRam <= 0) {
						ramming = false;
						player.color = Color.white;
				}

				player.color = Color.white;

				//start ramming
				if ((Input.GetKeyDown (KeyCode.E) || Input.touchCount == 2) && timerRam <= 0 && GameControl.control.RamAmount > 0) {
						ramming = true;
						timerRam = timerLengthSec;
				}

				if (ramming == true) {
						player.color = Color.blue;
				}

				ramDisplay.text = "Rams: " + GameControl.control.RamAmount;
		}

		void OnCollisionEnter2D (Collision2D col)
		{			
				if (ramming) {
						if (col.gameObject.tag == tagCanCollide) {
								Destroy (col.gameObject);
								GameControl.control.RamAmount -= 1;
								ramming = false;
								timerRam = 0;
						}
				}
		}
}
