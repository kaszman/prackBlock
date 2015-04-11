using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour
{

		public Transform lazTransform;
		public Transform thisTransform;
		public Vector3 offset;

		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
				thisTransform.position = new Vector3 (lazTransform.position.x + offset.x, lazTransform.position.y + offset.y, thisTransform.position.z);
		}
}
