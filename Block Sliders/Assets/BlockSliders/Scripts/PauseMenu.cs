﻿using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	public Canvas myCanvis;
	public string currentLevel;
	
	//public GUISkin mySkin;
	private Rect windowRect;
	private bool paused = false;
	private bool isHelpShown = false;
	private int rectSize = 200;
	private float rectRemaining;
	private int padding = 1;
	private int numOfBtn = 4;
	
	void Start()
	{
		windowRect = new Rect(0, 0, rectSize, rectSize);

		//set the menu rectangle center
		Vector2 centerPos = new Vector2(myCanvis.pixelRect.center.x, myCanvis.pixelRect.center.y * 1.5f);
		windowRect.center = centerPos;
	}
	
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			isHelpShown = false;
		}

		if (Application.isMobilePlatform)
		{
			windowRect.height = Screen.height / 2;
			windowRect.width = Screen.width / 2;
			windowRect.center = new Vector2(Screen.width/2, Screen.height/2);
		}

		
		rectRemaining = windowRect.height - 35;

		myCanvis.enabled = isHelpShown;

		//control game speed
		if(paused){ Time.timeScale = 0; }
		else{ Time.timeScale = 1; }
	}
	
	private void OnGUI()
	{
		GUI.skin.button.fontSize = (int)windowRect.width/10;

		if (paused)
		{
			windowRect = GUI.Window(0, windowRect, windowFun, "Pause Menu");
		}
	}

	//functions 
	private void windowFun(int id)
	{
		//creates a new button then does button action. Also sets button size
		if (GUILayout.Button("Resume", GUILayout.Height(rectRemaining/numOfBtn - padding)))
		{
			isHelpShown = false;
			//myCanvis.enabled = isHelpShown;
			paused = false;
		}
		if (GUILayout.Button("Help", GUILayout.Height(rectRemaining/numOfBtn - padding)))
		{
			isHelpShown = !isHelpShown;
			//myCanvis.enabled = isHelpShown;
		}
		if (GUILayout.Button("Restart Level", GUILayout.Height(rectRemaining/numOfBtn - padding)))
		{
			Application.LoadLevel(currentLevel);
		}
		if (GUILayout.Button("Exit", GUILayout.Height(rectRemaining/numOfBtn - padding)))
		{
			Application.LoadLevel("Menu");
		}
	}
}