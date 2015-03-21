using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
		public Slider loadingBar;
		public float loadingTime;
		// Use this for initialization
		void Start ()
		{
				loadingTime = -5f;
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				if (loadingTime >= 110) {
						Application.LoadLevel ("Menu");
				}

				loadingTime += Random.Range (0.001f, 0.9f);
				loadingBar.value = loadingTime;
		}
}
