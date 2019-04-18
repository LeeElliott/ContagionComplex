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
    public GameObject missionButton;
    public GameObject toggle;

    [SerializeField]
    private List<GameObject> buttonList;

    private void Start()
    {
        FillMissionList();
    }

    public void FillMissionList()
    {
        buttonList = new List<GameObject>();

        for (int i = 0; i < 99; i++)
        {
            GameObject button;

            // Create new instances of our prefab button
            button = (GameObject)Instantiate(missionButton, transform);

            // Change text of button
            button.GetComponentInChildren<Text>().text = "Mission " + (i + 1).ToString();

            buttonList.Add(button);

            // Add listener
            UnityEngine.UI.Button buttonRef = button.GetComponent<UnityEngine.UI.Button>();
            buttonRef.onClick.AddListener(MissionButtonClicked);

            // Set mission number stored in button
            buttonRef.GetComponent<MissionButtonScript>().missionNumber = (i + 1);

            // Show buttons are inactive
            if (i > 0)
            {
                Color colour = button.GetComponent<Image>().color;
                colour.a = 0.5f;

                button.GetComponent<Image>().color = colour;
            }
        }
    }

    public void MissionButtonClicked()
    {
        // Display info text and start button
        toggle.SetActive(true);

        // Set the text
    }

    public void StartClicked()
    {
        // Switch scene to main game
        SceneManager.LoadScene("MainGame");
    }

    public void OptionsClicked()
    {
        FillMissionList();

        // Switch to options scene
        //SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }
}
