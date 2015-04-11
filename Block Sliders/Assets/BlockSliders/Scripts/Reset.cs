using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour
{

		public string resetKey;
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (resetKey)) {
						Application.LoadLevel ("Lvl" + GameControl.control.CurrentLevel.ToString ());
				}
		}
}
