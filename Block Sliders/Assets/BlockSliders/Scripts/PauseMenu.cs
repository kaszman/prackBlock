using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	public Canvas menuCanvas;
	public Canvas helpCanvas;
	public Canvas optionsCanvas;
	public string currentLevel;
	
	//code generated ui
	private Rect windowRect;
	private bool paused = false;
	private bool isHelpShown = false;
	private int rectSize = 200;
	private float rectRemaining;
	private int padding = 1;
	private int numOfBtn = 4;
	private Canvas displayCanvas;
	
	void Start()
	{
		displayCanvas = menuCanvas;
		//set up code gen menu
		windowRect = new Rect(0, 0, rectSize, rectSize);
		Vector2 centerPos = new Vector2(displayCanvas.pixelRect.center.x, displayCanvas.pixelRect.center.y * 1.5f);
		windowRect.center = centerPos;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = true;
		}

		if (Application.isMobilePlatform)
		{
			windowRect.height = Screen.height / 2;
			windowRect.width = Screen.width / 2;
			windowRect.center = new Vector2(Screen.width/2, Screen.height/2);
			displayCanvas.scaleFactor = 3f;
		}
		
		rectRemaining = windowRect.height - 35;

		//control game speed
		if(paused)
		{ 
			Time.timeScale = 0;
			displayCanvas.enabled = true;
		}
		else
		{
			Time.timeScale = 1;
			displayCanvas.enabled = false;
		}
	}

	public void pauseState(bool pausedOrNot)
	{
		paused = pausedOrNot;
	}

	/// <summary>
	/// Changes the canvas to display.
	/// </summary>
	/// <param name="canvas">Canvas number.</param>
	public void changeCanvas(int canvas)
	{			
		displayCanvas.enabled = false;
		if (canvas == 1)
		{
			displayCanvas = menuCanvas;
		}
		if (canvas == 2)
		{
			displayCanvas = helpCanvas;
		}
		if (canvas == 3)
		{
			displayCanvas = optionsCanvas;
		}
	}

//	private void OnGUI()
//	{
//		GUI.skin.button.fontSize = (int)windowRect.width/10;
//
//		if (paused)
//		{
//			windowRect = GUI.Window(0, windowRect, windowFun, "Pause Menu");
//		}
//	}
//
//	//functions 
//	private void windowFun(int id)
//	{
//		//creates a new button then does button action. Also sets button size
//		if (GUILayout.Button("Resume", GUILayout.Height(rectRemaining/numOfBtn - padding)))
//		{
//			isHelpShown = false;
//			//myCanvis.enabled = isHelpShown;
//			paused = false;
//		}
//		if (GUILayout.Button("Help", GUILayout.Height(rectRemaining/numOfBtn - padding)))
//		{
//			isHelpShown = !isHelpShown;
//			//myCanvis.enabled = isHelpShown;
//		}
//		if (GUILayout.Button("Restart Level", GUILayout.Height(rectRemaining/numOfBtn - padding)))
//		{
//			Application.LoadLevel(currentLevel);
//		}
//		if (GUILayout.Button("Exit", GUILayout.Height(rectRemaining/numOfBtn - padding)))
//		{
//			Application.LoadLevel("Menu");
//		}
//	}
}
