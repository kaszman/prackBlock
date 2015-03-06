using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x, 0, 0) + offset;
	}
}
