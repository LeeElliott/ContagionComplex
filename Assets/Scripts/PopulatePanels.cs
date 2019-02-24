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

            // Clear scientist nodes
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

            // String to pass to text element
            string outputString;

            // Add name to string
            outputString = allStaff[i].GetComponent<CharacterAI>().characterName += "\n";

            // Add age to string
            int charAge = allStaff[i].GetComponent<CharacterAI>().characterAge;
            outputString += charAge.ToString();
            outputString += "       ";

            // Add gender to string
            outputString += allStaff[i].GetComponent<CharacterAI>().characterGender += "\n";

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

        // Create GameObject instance
        GameObject newListNode;

        for (int i = 0; i < 9; i++)
        {
            // Create new instances of our prefab until we've created as many as we specified
            newListNode = (GameObject)Instantiate(node, transform);

            // String to pass to text element
            string outputString = (i + 1).ToString() + ". ";

            // TODO: Check if star has been earned


            // Set text element to contents of string
            //newListNode.transform.GetChild(1).GetComponent<Text>().text = outputString;
        }
    }
}
