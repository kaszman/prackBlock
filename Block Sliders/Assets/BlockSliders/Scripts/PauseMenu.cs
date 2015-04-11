using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
		public Canvas menuCanvas;
		public Canvas helpCanvas;
		public Canvas optionsCanvas;
		public Text levelNameDisplay;
		public bool paused;

		private Canvas displayCanvas;

		//options sliders
		public Slider blockspeedSlider;
		public Slider playerspeedSlider;
		public Slider fxvolumeSlider;

	
		void Start ()
		{
				displayCanvas = menuCanvas;
				levelNameDisplay.text = "Level" + GameControl.control.CurrentLevel.ToString ();
		}
	
		void Update ()
		{
				if (GameControl.control.PausedMenu && GameControl.control.Paused) {
						displayCanvas.enabled = true;
				} else {
						displayCanvas.enabled = false;
				}
				if (Application.isMobilePlatform) {
						displayCanvas.scaleFactor = 3f;
				}

				if (Input.GetKeyDown (KeyCode.Escape)) {
						GameControl.control.Paused = true;
						GameControl.control.PausedMenu = true;
				}
		}

	#region Methods to apply slider settings
//	public void changeBlockSpeed()
//	{
//	//	GameControl.control.BlockSpeedPref = (int)blockspeedSlider.value;
//	}
//	
//	public void changePlayerSpeed()
//	{
//	//	GameControl.control.PlayerSpeedPref = (int)playerspeedSlider.value;
//	}
	#endregion

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
						displayCanvas = menuCanvas;
				}
				if (canvas == 2) {
						blockspeedSlider.enabled = false;
						playerspeedSlider.enabled = false;
						displayCanvas = helpCanvas;
				}
				if (canvas == 3) {
						//if we change to the options menu, make sure sliders are at proper values
						blockspeedSlider.enabled = true;
						playerspeedSlider.enabled = true;
						blockspeedSlider.value = PlayerPrefs.GetInt ("BlockSpeedPref");
						playerspeedSlider.value = PlayerPrefs.GetInt ("PlayerSpeedPref");
						fxvolumeSlider.value = PlayerPrefs.GetInt ("FxVolumePref");
						displayCanvas = optionsCanvas;
				}
		}
	
}
