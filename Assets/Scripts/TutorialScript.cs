using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialScript : MonoBehaviour {

	public Text TextBox = null;
	public GameObject NextButton;
	public Button Buttooon;
	public int counter = 0;

	public void changeText()
	{ 	
		counter++;
		if (counter == 1) 
		{
			TextBox.text = "It is your job to help us find cures for these viruses! You will use the lab equipment here to continue the research.";
		} 
		else if (counter == 2)
		{
			TextBox.text = "We hope that you will be able to create a cure for these viruses. Good luck!";
		}

	}

}