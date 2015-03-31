using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class QuitGame : MonoBehaviour
{	

		public void quitGame (bool AreYouSure)
		{
				if (AreYouSure == true) {				
						GameControl.control.Save ();
						UnityEngine.Resources.UnloadUnusedAssets ();
						System.GC.Collect ();
						System.GC.WaitForPendingFinalizers ();
						Application.Quit ();
				}
		}
}
