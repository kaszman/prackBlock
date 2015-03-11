using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakeImageFullScreen : MonoBehaviour {

	public Image image;
	// Use this for initialization
	void Start () {
		image.rectTransform.localScale = new Vector3(500, 500);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
