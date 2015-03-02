using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	public Canvas myCanvis;
	
	//public GUISkin mySkin;
	private Rect windowRect;
	private bool paused = false;
	
	void Start()
	{
		windowRect = new Rect(0, 0, 350, 450);
	}
	
	void Update()
	{
		windowRect.center = myCanvis.pixelRect.center;
		if (Input.GetKeyDown(KeyCode.Q))
		{
			paused = !paused;
		}
		
		if(paused){ Time.timeScale = 0; }
		else{ Time.timeScale = 1; }
	}
	
	private void OnGUI()
	{
		if (paused)
		{
			windowRect = GUI.Window(0, windowRect, windowFun, "Pause Menu");
		}
	}

	//functions 
	private void windowFun(int id)
	{
		if (GUILayout.Button("Resume"))
		{
			paused = false;
		}
		if (GUILayout.Button("Options"))
		{
			
		}
		if (GUILayout.Button("Exit"))
		{
			Application.LoadLevel("Menu");
		}
	}
}
