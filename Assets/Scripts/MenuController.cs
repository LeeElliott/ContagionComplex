//-------------------------------
// Created by Lee Elliott
// 15/11/2018
//
// A script designed to hold all
// functionality of the menu
// controller.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Store text object for "flash"
    public Text messageText;

    private bool increasing = false;

	public void MissionsClicked()
    {
        // Load mission menu
        SceneManager.LoadScene("MissionsMenu");
    }

    private void Update()
    {
        // Store current alpha value of text
        float alphaValue = messageText.color.a;

        // Check if increasing or decreasing
        if (!increasing)
        {
            // If less than 0 stop decreasing and start increasing
            if (alphaValue > 0)
            {
                // Decrease alpha
                alphaValue -= 0.05f;

                // Set text colour to include new alpha
                messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, alphaValue);
            }
            else
            {
                // Switch direction of alpha change
                increasing = true;
            }
        }
        // If more than 1 stop increasing and start decreasing
        else
        {
            if (alphaValue < 1)
            {
                // Increase alpha
                alphaValue += 0.02f;

                // Set text colour to include new alpha
                messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, alphaValue);
            }
            else
            {
                // Switch direction of alpha change
                increasing = false;
            }
        }
    }
}
