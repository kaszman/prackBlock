using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
		public Canvas menuCanvas;
		public Canvas helpCanvas;
		public Text helpText;
		public Canvas optionsCanvas;
		public Canvas joystickCanvas;
		public Text levelNameDisplay;
		public bool paused;

		private Canvas displayCanvas;

		//options sliders
		public Slider blockspeedSlider;
		public Slider playerspeedSlider;
		public Slider fxvolumeSlider;
		public Slider musicvolumeSlider;
	
		void Start ()
		{
				//	joystickCanvas.enabled = true;
				displayCanvas = menuCanvas;
				if (Application.isMobilePlatform) {
						helpText.text = "Touch the screen with two fingers to activate ramming" +
								"\nRamming needs a running start!" +
								"\nSwipe the screen to move the ram" +
								"\nTilt the device to move the blocks " +
								"\nHole traps can be plugged by blocks" +
								"\nAcid traps can not be plugged";
				} else {
						helpText.text = "Press E to activate ramming" +
								"\nRamming needs a running start!" +
								"\nUse WASD to move the ram" +
								"\nUse the ARROW keys to move the blocks " +
								"\nHole traps can be plugged by blocks" +
								"\nAcid traps can not be plugged";
				}
				levelNameDisplay.text = "Level " + GameControl.control.CurrentLevel.ToString ();
		}
	
		void Update ()
		{
				if (GameControl.control.JoystickPref == 1) {
						//		joystickCanvas.enabled = true;
				} else if (GameControl.control.JoystickPref == 0) {
						//		joystickCanvas.enabled = false;
				}
		
				if (GameControl.control.PausedMenu && GameControl.control.Paused) {
						displayCanvas.enabled = true;
				} else {
						displayCanvas.enabled = false;
				}

				if (Application.isMobilePlatform) {
						displayCanvas.scaleFactor = 3f;
				}

				if (Input.GetKeyDown (KeyCode.Escape) && !GameControl.control.Paused) {
						PauseEngage ();
				}
		}

		/// <summary>
		/// Changes the pause state
		/// </summary>
		/// <param name="pausedOrNot">If set to <c>true</c> paused or not.</param>
		public void pauseState (bool pausedOrNot)
		{
				GameControl.control.Paused = pausedOrNot;
				GameControl.control.PausedMenu = pausedOrNot;
		}

		/// <summary>
		/// Changes the canvas to display.
		/// </summary>
		/// <param name="canvas">Canvas number.</param>
		public void changeCanvas (int canvas)
		{			
				displayCanvas.enabled = false;
				if (canvas == 1) {
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						fxvolumeSlider.enabled = false;
						musicvolumeSlider.enabled = false;
						displayCanvas = menuCanvas;
				}
				if (canvas == 2) {
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						fxvolumeSlider.enabled = false;
						musicvolumeSlider.enabled = false;
						displayCanvas = helpCanvas;
				}
				if (canvas == 3) {
						//if we change to the options menu, make sure sliders are at proper values
						blockspeedSlider.enabled = true;
						playerspeedSlider.enabled = true;
						fxvolumeSlider.enabled = true;
						musicvolumeSlider.enabled = true;
						blockspeedSlider.value = PlayerPrefs.GetInt ("BlockSpeedPref");
						playerspeedSlider.value = PlayerPrefs.GetInt ("PlayerSpeedPref");
						fxvolumeSlider.value = PlayerPrefs.GetInt ("FxVolumePref");
						musicvolumeSlider.value = PlayerPrefs.GetInt ("MusicVolumePref");
						displayCanvas = optionsCanvas;
				}
		}

		public void calibration ()
		{
				GameControl.control.Offset = Input.acceleration;
		}

		public void PauseEngage ()
		{
				GameControl.control.Paused = true;
				GameControl.control.PausedMenu = true;
		}
	
}
