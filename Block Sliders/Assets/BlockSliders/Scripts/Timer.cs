using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{

		private float timeRemaining;
		private bool ringing;
		private bool control;

		/// <summary>
		/// Initializes a new instance of the <see cref="Timer"/> class.
		/// </summary>
		/// <param name="lengthSeconds">Length seconds.</param>
		public Timer (float lengthSeconds)
		{
				timeRemaining = lengthSeconds;
				ringing = false;
				control = true;
		}

		public bool Ringing {
				get { return ringing;}
				set {
						ringing = value;
						control = false;
				}
		}

		public void AddTime (float lengthSeconds)
		{
				timeRemaining += lengthSeconds;
				control = true;
		}

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				Debug.ClearDeveloperConsole ();
				if (timeRemaining > 0 && control) {
						ringing = false;
						timeRemaining -= Time.deltaTime;
				} else if (timeRemaining <= 0 && control) {
						ringing = true;
				}
				Debug.Log (ringing.ToString () + "\n" + timeRemaining.ToString ("0.00"));
		}
}
