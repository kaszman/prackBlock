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

		protected bool ramming = false;

		private LevelSelect selector;
		private float timerRam;
		private float levelTime;
		private float finalTime;
		private int levelNumber;

		void Start ()
		{
				GameControl.control.Paused = false;
				GameControl.control.PausedMenu = false;
				GameControl.control.RamAmount = 5;
				if (Application.isMobilePlatform) {
						ramDisplay.canvas.scaleFactor = 3f;
				}
				levelNumber = GameControl.control.CurrentLevel;
		}

		//pause control
		void Update ()
		{
				if (GameControl.control.Paused == true) {
						Time.timeScale = .0000001f;
				} else {
						Time.timeScale = 1f;
				}

				//update scoreboard only when it is showing
				if (scoreBoard.enabled) {
						float[,] temp = GameControl.control.GetScoreData ();
						scoreBoard.text = temp [levelNumber - 1, 0].ToString ("000.0") + "\n" + temp [levelNumber - 1, 1].ToString ("000.0")
								+ "\n" + temp [levelNumber - 1, 2].ToString ("000.0") + "\n" + temp [levelNumber - 1, 3].ToString ("000.0") 
								+ "\n" + temp [levelNumber - 1, 4].ToString ("000.0");
				}
		}
		void FixedUpdate ()
		{		
				timerRam -= Time.deltaTime;
				levelTime += Time.deltaTime;

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
				timerDisplay.text = levelTime.ToString ("000.0");
				ramDisplay.text = "Rams: " + GameControl.control.RamAmount;
		}

		void OnCollisionEnter2D (Collision2D col)
		{						

				if (ramming) {
						if (col.gameObject.tag == tagCanCollide) {
								GameControl.control.PlayBreak ();
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
				float adjustedTime = levelTime - GameControl.control.RamAmount;
		
				if (adjustedTime < 0) {
						adjustedTime = 0;
				}
				finalTime = levelTime;
				finalTimeDisplay.text = finalTime.ToString ("000.0") + " - " + GameControl.control.RamAmount.ToString () + " rams = " + adjustedTime.ToString ("000.0");
				GameControl.control.addLeaderboardTime (adjustedTime, levelNumber);
		}
}
