﻿//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;

public class SceneSelect : MonoBehaviour
{

		//ui elements
		public Text levelNameText;
		public Button prevButton;
		public Button nextButton;
		public Button back;
		public Button Select;
		public Image levelPicture;
		public int numOfLevels;
		public Sprite[] levelImages = new Sprite[100];

		//int to select the level
		private int selectionNumber = 1;


		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (selectionNumber > levelImages.Length) {
						selectionNumber = levelImages.Length;
				}
				levelNameText.text = "Level " + selectionNumber.ToString ();
				levelPicture.sprite = levelImages [selectionNumber - 1];
		}

		//increment level selector, limited to unlocked levels
		public void nextLevel ()
		{
				if (selectionNumber + 1 <= GameControl.control.HighestUnlock) {
						selectionNumber++;
				}
		}

		//decriment level selector, not below 1
		public void backLevel ()
		{
				if (selectionNumber - 1 >= 1) {
						selectionNumber--;
				}
		}

		//load the selected level
		public void SelectLevel ()
		{
				Application.LoadLevel ("Lvl" + selectionNumber.ToString ());
		}

		//set the level picture to the highest unlock
		public void adjustLevel ()
		{
				selectionNumber = GameControl.control.HighestUnlock;
		}
}
