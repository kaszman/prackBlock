using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
		public SpriteRenderer player;
		public string tagCanCollide;
		public int timerLengthSec;
		public Text ramDisplay;
		public Text timerDisplay;
		public Text finalTimeDisplay;
		public Text scoreBoard;
		public int levelNumber;

		protected bool ramming = false;

		private GameObject coco;
		private LevelSelect selector;
		private float timerRam;
		private float levelTime;
		private float finalTime;

		void Start ()
		{
				GameControl.control.Paused = false;
				GameControl.control.PausedMenu = false;
				GameControl.control.RamAmount = 5;
				if (Application.isMobilePlatform) {
						ramDisplay.canvas.scaleFactor = 3f;
				}
				//GameControl.control.Load ();
		}

		//pause control
		void Update ()
		{
				if (GameControl.control.Paused == true) {
						Time.timeScale = .0000001f;
				} else {
						Time.timeScale = 1f;
				}
//				if (GameControl.control.Paused && !GameControl.control.PausedMenu) {
//						endBehavior ();
//				}
				//GameControl.control.Load ();
				float[,] temp = GameControl.control.GetScoreData ();
				scoreBoard.text = temp [levelNumber - 1, 0].ToString ("000.0") + "\n" + temp [levelNumber - 1, 1].ToString ("000.0")
						+ "\n" + temp [levelNumber - 1, 2].ToString ("000.0") + "\n" + temp [levelNumber - 1, 3].ToString ("000.0") 
						+ "\n" + temp [levelNumber - 1, 4].ToString ("000.0");
		}
		void FixedUpdate ()
		{		
				//start ramming
				if ((Input.GetKey (KeyCode.E) || Input.touchCount == 2) && timerRam <= 0 && GameControl.control.RamAmount > 0) {
						ramming = true;
						timerRam = timerLengthSec;
				}

				timerRam -= Time.deltaTime;
				levelTime += Time.deltaTime;

				if (timerRam <= 0) {
						ramming = false;
						player.color = Color.white;
				}

				player.color = Color.white;

				if (ramming == true) {
						player.color = Color.blue;
				}
				if (GameControl.control.displayTimer == true) {
						timerDisplay.enabled = true;
				} else {
						timerDisplay.enabled = false;

				}

				timerDisplay.text = levelTime.ToString ("000.0");
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
				if (col.gameObject.tag == "Door") {
						endBehavior ();
				}
		}

		public float getTime ()
		{
				return levelTime;
		}

		public void endBehavior ()
		{
				finalTime = levelTime;
				finalTimeDisplay.text = finalTime.ToString ("000.0");
				GameControl.control.addLeaderboardTime (finalTime, levelNumber);
		}
}
