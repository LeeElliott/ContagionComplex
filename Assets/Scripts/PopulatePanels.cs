//-------------------------------
// Created by Lee Elliott
// 23/02/2019
//
// A script designed fill the scientist list
// and mission data panel.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulatePanels : MonoBehaviour
{
    // Game objects to be spawned
    public GameObject node;
    public GameObject panel;

    // References to all profile images
    public Sprite profileA;
    public Sprite profileB;
    public Sprite profileC;
    public Sprite profileD;
    public Sprite profileE;
    public Sprite profileF;

    // Reference to controller object
    public GameObject controller;

    // References to sliders
    public Slider progressSlider;
    public Slider timerSlider;

    // CSV script reference
    private LoadFromCSV scriptA;

    // Use this for initialization
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StaffClicked()
    {
        if (panel.activeSelf)
        {
            // Hide panel
            panel.SetActive(false);

            // Clear scientist nodes
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            PopulateStaff();
        }
    }

    public void MissionClicked()
    {
        if (panel.activeSelf)
        {
            // Hide panel
            panel.SetActive(false);

            // Clear mission nodes
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            PopulateMission();
        }
    }

    public void BuildClicked()
    {
        // Reusing node and panel for neatness
        // Hide the button
        node.SetActive(false);

        // Show the build menu
        panel.SetActive(true);
    }


    private void PopulateStaff()
    {
        // Activate panel
        panel.SetActive(true);

        // Create GameObject instance
        GameObject newListNode;

        // Make a list of ALL scientists
        GameObject[] allStaff = GameObject.FindGameObjectsWithTag("Scientist");

        // Calculate number of list nodes
        int numberToCreate = allStaff.Length;

        for (int i = 0; i < numberToCreate; i++)
        {
            // Create new instances of our prefab until we've created as many as we specified
            newListNode = (GameObject)Instantiate(node, transform);

            // Apply profile image
            int profile = allStaff[i].GetComponent<CharacterAI>().profileImage;

            // HACK: Get the image component of the profile picture
            Image image = newListNode.transform.GetChild(0).GetComponent<Image>();
   

            switch (profile)
            {
                case 0:
                    // Set the image component to correct sprite
                    image.sprite = profileA;
                    break;
                case 1:
                    // Set the image component to correct sprite
                    image.sprite = profileB;
                    break;
                case 2:
                    // Set the image component to correct sprite
                    image.sprite = profileC;
                    break;
                case 3:
                    // Set the image component to correct sprite
                    image.sprite = profileD;
                    break;
                case 4:
                    // Set the image component to correct sprite
                    image.sprite = profileE;
                    break;
                case 5:
                    // Set the image component to correct sprite
                    image.sprite = profileF;
                    break;
            }

            // String to pass to text element
            string outputString;

            // Add name to string
            outputString = allStaff[i].GetComponent<CharacterAI>().characterName + "\n";

            // Add age to string
            int charAge = allStaff[i].GetComponent<CharacterAI>().characterAge;
            outputString += charAge.ToString();
            outputString += "       ";

            // Add gender to string
            outputString += allStaff[i].GetComponent<CharacterAI>().characterGender + "\n";

            // Add Identification stat
            int ident = allStaff[i].GetComponent<CharacterAI>().GetStat(1);
            outputString += "I : ";
            outputString += ident.ToString();
            outputString += "\n";

            // Add Experimentation stat
            int exper = allStaff[i].GetComponent<CharacterAI>().GetStat(2);
            outputString += "E : ";
            outputString += exper.ToString();
            outputString += "\n";

            // Add Production stat
            int prod = allStaff[i].GetComponent<CharacterAI>().GetStat(3);
            outputString += "P : ";
            outputString += prod.ToString();
            outputString += "\n";

            // TODO: Space left for location
            outputString += "Location";

            // Set text element to string
            newListNode.transform.GetChild(1).GetComponent<Text>().text = outputString;
        }
    }

    private void PopulateMission()
    {
        // Activate panel
        panel.SetActive(true);

        // Sliders update in separate code file

        // Create GameObject instance
        GameObject newListNode;

        // Loop to fill data sections
        for (int i = 0; i < 9; i++)
        {
            // Create new instances of our prefab until we've created as many as we specified
            newListNode = (GameObject)Instantiate(node, transform);

            // String to pass to text element
            string outputString = (i + 1).ToString() + ". ";

            // Check if star has been earned
            // Set script
            scriptA = GetComponent<LoadFromCSV>();

            switch (i)
            {
                case 0:
                    if(controller.GetComponent<ControllerScript>().GetIStars() > 0)
                    {
                        outputString = scriptA.LoadData(1, 1);
                    }
                    break;
                case 1:
                    if (controller.GetComponent<ControllerScript>().GetIStars() > 1)
                    {
                        outputString = scriptA.LoadData(2, 1);
                    }
                    break;
                case 2:
                    if (controller.GetComponent<ControllerScript>().GetIStars() > 2)
                    {
                        outputString = scriptA.LoadData(3, 1);
                    }
                    break;
                case 3:
                    if (controller.GetComponent<ControllerScript>().GetEStars() > 0)
                    {
                        outputString = scriptA.LoadData(4, 1);
                    }
                    break;
                case 4:
                    if (controller.GetComponent<ControllerScript>().GetEStars() > 1)
                    {
                        outputString = scriptA.LoadData(5, 1);
                    }
                    break;
                case 5:
                    if (controller.GetComponent<ControllerScript>().GetEStars() > 2)
                    {
                        outputString = scriptA.LoadData(6, 1);
                    }
                    break;
                case 6:
                    if (controller.GetComponent<ControllerScript>().GetPStars() > 0)
                    {
                        outputString = scriptA.LoadData(7, 1);
                    }
                    break;
                case 7:
                    if (controller.GetComponent<ControllerScript>().GetPStars() > 1)
                    {
                        outputString = scriptA.LoadData(8, 1);
                    }
                    break;
                case 8:
                    if (controller.GetComponent<ControllerScript>().GetPStars() > 2)
                    {
                        outputString = scriptA.LoadData(9, 1);
                    }
                    break;
            }

            // Set text element to contents of string
            newListNode.transform.GetChild(1).GetComponent<Text>().text = outputString;
        }
    }
}
