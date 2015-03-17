using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderCode : MonoBehaviour
{
	
		//options sliders
		public UnityEngine.UI.Slider subjectSlider;
		public Text dataText;
		public string SettingName;

		void Update ()
		{
				dataText.text = ((int)subjectSlider.value - 1).ToString ();
		}


		// change a preference value
		public void ChangeValueBySlider ()
		{
				PlayerPrefs.SetInt (SettingName, (int)subjectSlider.value);
				GameControl.control.Save ();
		}
}
