using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	public string resetKey;
	public string currentSceneName;
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(resetKey))
		{
			Application.LoadLevel (currentSceneName);
		}
	}
}
