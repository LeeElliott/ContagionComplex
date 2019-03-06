//-------------------------------
// Created by Lee Elliott
// 04/03/2019
//
// A script designed update the
// mission menu sliders.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdater : MonoBehaviour
{
    // Object references
    public GameObject controller;
    public Slider timerSlider;
    public Slider progressSlider;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (timerSlider.IsActive())
        {
            // Set value of timer slider as a percentage
            float timer = controller.GetComponent<ControllerScript>().GetTimer();
            timer *= 100;
            timer /= controller.GetComponent<ControllerScript>().GetTimerMax();

            timerSlider.value = timer;
        }

        if (progressSlider.IsActive())
        {
            // Set value of progress slider as a percentage
            int progress = controller.GetComponent<ControllerScript>().GetTotalStars();
            progress *= 100;
            progress /= 9;

            progressSlider.value = progress;
        }
	}
}
