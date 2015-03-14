using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour
{	

		public void quitGame (bool AreYouSure)
		{
				if (AreYouSure == true) {
						UnityEngine.Resources.UnloadUnusedAssets ();
						System.GC.Collect ();
						System.GC.WaitForPendingFinalizers ();
						GameControl.control.Save ();
						Application.Quit ();
				}
		}
}
