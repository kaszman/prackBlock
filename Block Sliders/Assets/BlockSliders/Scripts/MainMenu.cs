using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

		//variavles to hold canvas'
		public Canvas mainCanvas;
		public Canvas helpCanvas;
		public Canvas optionsCanvas;
		public Canvas levelcanvas;
		public Canvas playcanvas;
		public Canvas creditscanvas;
		public Canvas leaderboardcanvas;

		//options sliders
		public Slider blockspeedSlider;
		public Slider playerspeedSlider;
	
		//private variables
		private Canvas displayCanvas;

		// Use this for initialization
		void Awake ()
		{
				displayCanvas = mainCanvas;
				displayCanvas.enabled = true;
		}
	
		// Update is called once per frame
		void Update ()
		{
//				if (Application.isLoadingLevel) {
//						displayCanvas = mainCanvas;
//				}
		}

		public void changeCanvas (int canvas)
		{
				displayCanvas.enabled = false;
				if (canvas == 0) {
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						displayCanvas = mainCanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 1) {
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						displayCanvas = helpCanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 2) {
						displayCanvas.enabled = false;

						//if we change to the options menu, make sure sliders are at proper values
						blockspeedSlider.enabled = true;
						playerspeedSlider.enabled = true;
						blockspeedSlider.value = PlayerPrefs.GetInt ("BlockSpeedPref");
						playerspeedSlider.value = PlayerPrefs.GetInt ("PlayerSpeedPref");
						displayCanvas = optionsCanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 3) {
						displayCanvas.enabled = false;

						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						displayCanvas = levelcanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 4) {
						displayCanvas.enabled = false;
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						displayCanvas = playcanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 5) {
						displayCanvas.enabled = false;

						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						displayCanvas = creditscanvas;
						displayCanvas.enabled = true;
				}
				if (canvas == 6) {
						displayCanvas.enabled = false;

						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						displayCanvas = leaderboardcanvas;
						displayCanvas.enabled = true;
				}
		}
}
