using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
		//text fileds
		public Text helpText;

		//variavles to hold canvas'
		public Canvas mainCanvas;
		public Canvas helpCanvas;
		public Canvas optionsCanvas;
		public Canvas levelcanvas;
		public Canvas playcanvas;
		public Canvas creditscanvas;
		public Button resumeButton;
		public Button levelSelectButton;

		//options sliders
		public Slider blockspeedSlider;
		public Slider playerspeedSlider;
		public Slider fxvolumeSlider;
		public Slider musicvolumeSlider;
	
		//private variables
		private Canvas displayCanvas;

		// Use this for initialization
		void Awake ()
		{
				displayCanvas = mainCanvas;
				displayCanvas.enabled = true;
		}

		void Start ()
		{
				if (Application.isMobilePlatform) {
						helpText.text = "Touch the screen with two fingers to activate ramming" +
								"\nRamming needs a running start!" +
								"\nUse the joystick or swipe the screen to move the ram" +
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
				if (GameControl.control.HighestUnlock <= 1) {
						GameControl.control.NewGame ();
				}
		}

		// Update is called once per frame
		void Update ()
		{
				if (GameControl.control.HighestUnlock == 1) {
						resumeButton.interactable = false;
						levelSelectButton.interactable = false;
				} else {
						resumeButton.interactable = true;
						levelSelectButton.interactable = true;
				}
		}

		public void changeCanvas (int canvas)
		{
				displayCanvas.enabled = false;
				if (canvas == 0) {
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						fxvolumeSlider.enabled = false;
						musicvolumeSlider.enabled = false;
						displayCanvas = mainCanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 1) {
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						fxvolumeSlider.enabled = false;
						musicvolumeSlider.enabled = false;
						displayCanvas = helpCanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 2) {
						displayCanvas.enabled = false;

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
						displayCanvas.enabled = true;
				}
				if (canvas == 3) {
						displayCanvas.enabled = false;
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						fxvolumeSlider.enabled = false;
						musicvolumeSlider.enabled = false;
						displayCanvas = levelcanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 4) {
						displayCanvas.enabled = false;
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						fxvolumeSlider.enabled = false;
						musicvolumeSlider.enabled = false;
						displayCanvas = playcanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 5) {
						displayCanvas.enabled = false;
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						fxvolumeSlider.enabled = false;
						musicvolumeSlider.enabled = false;
						displayCanvas = creditscanvas;
						displayCanvas.enabled = true;
				}
		}
}
