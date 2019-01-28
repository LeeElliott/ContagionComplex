//-------------------------------
// Created by Lee Elliott
// 21/01/2019
//
// A script designed to hold all
// functionality of the mission
// screen controller.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    // Update is called once per frame
    public void MissionOneClicked()
    {
        // Switch scene to main game
        SceneManager.LoadScene("MainGame");
    }
}
