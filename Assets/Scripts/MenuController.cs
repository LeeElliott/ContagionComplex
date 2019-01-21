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
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void MissionsClicked()
    {
        SceneManager.LoadScene("MissionsMenu");
    }

    public void SettingsClicked()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void ExitClicked()
    {
        // Doesn't work in editor
        Application.Quit();
    }
}
