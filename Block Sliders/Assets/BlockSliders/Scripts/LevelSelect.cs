using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	private string toLevel;


	public LevelSelect(string toLevel)
	{
		this.toLevel = toLevel;
	}


	void FixedUpdate()
	{
			ChangeLevel(toLevel);
	}


	public void ChangeLevel(string toLevel)
	{
		Application.LoadLevel(toLevel);
	}
}
