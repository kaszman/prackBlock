using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public UnityEngine.UI.Button button;
	public enum select {Forward = 0, Backward = 1, Select = 2};
	public select selection;

	//do what the selection says

	public void DoAction(string toLevel)
	{
		if (selection == select.Select)
		{
		Application.LoadLevel(toLevel);
		}
	}
}
