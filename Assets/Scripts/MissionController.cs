//-------------------------------
// Created by Lee Elliott
// 21/01/2019
// 
// Edited by Lewis Hodgkin (Button sound triggers added)
// 07/05/2019
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
    int delay = 0;

    private void Start()
    {
        FillMissionList();
        GetComponent<GridLayoutGroup>().enabled = false;
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

    public void Update()
    {
        delay++;

        if (delay >= 2)
            GetComponent<GridLayoutGroup>().enabled = true;
    }

    public void MissionButtonClicked()
    {
		//play button click sound
		AkSoundEngine.PostEvent ("Play_Button_1_Forward", gameObject);
        // Display info text and start button
        toggle.SetActive(true);
     
        // Set the text
		
		
    }

    public void StartClicked()
    {
		//play button click sound
		AkSoundEngine.PostEvent ("Play_Button_1_Forward", gameObject);
        // Switch scene to main game
        SceneManager.LoadScene("MainGame");
		
    }

    public void OptionsClicked()
    {
        //FillMissionList();
		
		//play button click sound
         AkSoundEngine.PostEvent ("Play_Button_1_Forward", gameObject);


        // Switch to options scene
        //SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
    }
}
