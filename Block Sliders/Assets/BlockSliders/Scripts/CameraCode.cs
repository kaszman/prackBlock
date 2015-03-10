using UnityEngine;
using System.Collections;

public class CameraCode : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0) + offset;
	}
}
