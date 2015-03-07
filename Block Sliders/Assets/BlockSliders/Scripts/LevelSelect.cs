using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public UnityEngine.UI.Button button;
	public enum select {Forward = 0, Backward = 1, Select = 2};
	public select selection;
	private string toLevel;


	public LevelSelect(select selection, string toLevel)
	{
		this.selection = selection;
		this.toLevel = toLevel;
	}


	void FixedUpdate()
	{
			ChangeLevel(toLevel);
	}

	//do what the selection says



	public void ChangeLevel(string toLevel)
	{
		if (selection == select.Select)
		{
		Application.LoadLevel(toLevel);
		}
		//if (selection
	}
}
