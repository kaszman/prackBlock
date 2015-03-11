using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	//variavles to hold canvas'
	public Canvas mainCanvas;
	public Canvas helpCanvas;
	public Canvas optionsCanvas;
	public Canvas levelcanvas;
	public Canvas playcanvas;
	public Canvas creditscanvas;
	public Canvas leaderboardcanvas;


	private Canvas displayCanvas;

	//options sliders
	public Slider blockspeedSlider;
	public Slider playerspeedSlider;

	// Use this for initialization
	void Start () {
		displayCanvas = mainCanvas;
		displayCanvas.enabled = true;
		if (Application.isMobilePlatform)
		{
			displayCanvas.scaleFactor = 3f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeCanvas(string canvas)
	{			
		displayCanvas.enabled = false;
		if (canvas == "main")
		{
			blockspeedSlider.enabled = false;
			playerspeedSlider.enabled = false;
			displayCanvas = mainCanvas;
			displayCanvas.enabled = true;
		}
		if (canvas == "help")
		{
			blockspeedSlider.enabled = false;
			playerspeedSlider.enabled = false;
			displayCanvas = helpCanvas;
			displayCanvas.enabled = true;
		}
		if (canvas == "options")
		{
			//if we change to the options menu, make sure sliders are at proper values
			blockspeedSlider.enabled = true;
			playerspeedSlider.enabled = true;
			blockspeedSlider.value = PlayerPrefs.GetInt("BlockSpeedPref");
			playerspeedSlider.value = PlayerPrefs.GetInt("PlayerSpeedPref");
			displayCanvas = optionsCanvas;
			displayCanvas.enabled = true;
		}
		if (canvas == "level")
		{
			blockspeedSlider.enabled = false;
			playerspeedSlider.enabled = false;
			displayCanvas = levelcanvas;
			displayCanvas.enabled = true;
		}
		if (canvas == "play")
		{
			blockspeedSlider.enabled = false;
			playerspeedSlider.enabled = false;
			displayCanvas = playcanvas;
			displayCanvas.enabled = true;
		}
		if (canvas == "credits")
		{
			blockspeedSlider.enabled = false;
			playerspeedSlider.enabled = false;
			displayCanvas = creditscanvas;
			displayCanvas.enabled = true;
		}
		if (canvas == "leader")
		{
			blockspeedSlider.enabled = false;
			playerspeedSlider.enabled = false;
			displayCanvas = leaderboardcanvas;
			displayCanvas.enabled = true;
		}
	}
}
