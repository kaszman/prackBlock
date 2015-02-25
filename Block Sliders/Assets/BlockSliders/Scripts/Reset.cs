using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	public string resetKey;
	public bool doReset;
	public GameObject playerObj = GameObject.Find("player");
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(resetKey))
		{
			Application.LoadLevel ("PrototypeScene");
		}
	}

	public bool DoReset
	{
		get { return doReset;}
		set { doReset = value;}
	}
}
