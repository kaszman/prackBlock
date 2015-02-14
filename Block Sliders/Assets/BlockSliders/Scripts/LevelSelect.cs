using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public enum sel {Forward = 0, Backward = 1, levelSelect = 2};
	public sel action;
	public int ApplicableLevelDest = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	//if there is a collision with a player
		DoSelection();
	}

	//do what the selection says

	private void DoSelection()
	{
	//go forward
		if (action == sel.Forward)
		{
			//advance scene one from current
			Application.LoadLevel(Application.loadedLevel + 1);
		}

		else if (action == sel.Backward)
		{
			//decriment current scene by one
		}
		else if (action == sel.levelSelect)
		{
			Application.LoadLevel(sel.levelSelect);
		}
		else
		{			
			//should never happen. default to main menu for testing?
		}
	}
}
