//-------------------------------
// Created by Lee Elliott
// 15/11/2018
//
// A script designed to hold all
// functionality of the splash
// screen controller.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashController : MonoBehaviour {

    public float timer = 10.0f;
    public Text splashText;

	// Update is called once per frame
	void Update()
    {
        if(timer <= 0.0f)
        {
            // Switch scene to menu
            SceneManager.LoadScene("MainMenu");
        }
        /*else if(timer <= 5.0f)
        {
            splashText.text = "St Andrews\nUniversity";
            
        }*/

        if(timer >= 0.0f)
        {
            timer -= Time.deltaTime;
        }
	}
}
