using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public UnityEngine.UI.Button button;
	public enum sel {Forward = 0, Backward = 1, levelSelect = 2};
	public sel action = sel.Forward;
	public int ApplicableLevelDest = 0;
	
	// Update is called once per frame
	void Update ()
	{

	}

	//do what the selection says

	public void DoAction()
	{
	//go forward
		if (action == 0)
		{
			//advance scene one from current
			Application.LoadLevel("PrototypeScene");
		}

		else if (action == sel.Backward)
		{
			//decriment current scene by one
		}
		else if (action == sel.levelSelect)
		{
			Application.LoadLevel("PrototypeScene");
		}
		else
		{			
			//should never happen. default to main menu for testing?
		}
	}
}
