using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialScript : MonoBehaviour
{
		public Text tutorialText;
		public int level;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				writeTutorial ();
		}

	#region level-specific text handling
		/// <summary>
		/// Writes the tutorial based on the level.
		/// </summary>
		public void writeTutorial ()
		{
				switch (level) {
				case 1:
						if (Application.isMobilePlatform) {
								tutorialText.text = "Swipe the screen to move";
						} else {
								tutorialText.text = "Use WASD to move";
						}
						break;				
				case 2:
						if (Application.isMobilePlatform) {
								tutorialText.text = "Tilt your phone to move the blocks";
						} else {
								tutorialText.text = "Use the arrow keys to move the blocks";
						}
						break;		
				case 3:
						tutorialText.text = "Fill in the hole with the block to pass over safely";
						break;				
				case 4:
						tutorialText.text = "Use what you've learned to navigate this maze";
						break;				
				case 5:
						tutorialText.text = "Keep in mind that you may need to use blocks from different places";
						break;			
				case 6:
						if (Application.isMobilePlatform) {
								tutorialText.text = "You may need to ram some blocks, touch the screen with two fingers to initiate ramming";
						} else {
								tutorialText.text = "You may need to ram some blocks, press E to initiate ramming";
						}
						break;
				}
		}

	#endregion
}
