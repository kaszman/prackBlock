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
				GameControl.control.Save ();
				GameControl.control.Load ();
		}

		//pause control
		void Update ()
		{
				if (GameControl.control.Paused == true) {
						Time.timeScale = .0000001f;
				} else {
						Time.timeScale = 1f;
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
				float[,] temp = GameControl.control.GetScoreData ();
				scoreBoard.text = temp [levelNumber, 1].ToString () + "\n" + temp [levelNumber, 1].ToString ()
						+ "\n" + temp [levelNumber, 1].ToString () + "\n" + temp [levelNumber, 1].ToString () 
						+ "\n" + temp [levelNumber, 1].ToString ();
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

		public float getTime ()
		{
				return levelTime;
		}

		public void endBehavior ()
		{
				finalTime = levelTime;
				finalTimeDisplay.text = finalTime.ToString ();
		}
}
