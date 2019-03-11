//-------------------------------
// Created by Lee Elliott
// 11/03/20198
//
// A script designed to hold all
// settings control.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    // Set the volume to the level of the ambinet slider
    public void AmbientAltered()
    {

    }

    // Set the volume to the level of the ambinet slider
    public void SFXAltered()
    {

    }

    // Toggle floating dialogue
    public void DialogueToggled()
    {

    }

    // Exit options menu
    public void ExitOptions()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("SettingsMenu"));
    }
}
