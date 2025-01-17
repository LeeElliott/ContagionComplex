﻿//-------------------------------
// Created by Lee Elliott
// 18/10/2018
//
// Edited by Christopher Pohler (scientist spawning)
// 23/02/2019
//
// Edited by Lee Elliott (Patient delivery)
// 01/03/2019
//
// Edited by Lee Elliott (UI)
// 15/03/2019
//
// Edited by Lee Elliott (Voice)
// 22/04/2019
//
// A script designed to hold all
// functionality of the game
// controller.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ControllerScript : MonoBehaviour
{
    // Boolean used to pause player input
    private bool currentlyPaused = false;

    // Empty used as containers in editor
    private GameObject scientists;
    private GameObject delivery;

    // Store number of stars earned per discipline
    private int identificationStars = 0;
    private int experimentationStars = 0;
    private int productionStars = 0;

    // Timer for mission failure
    private float timerMax = 600.0f;
    private float missionTimer = 600.0f;

    // Used for spawning
    private bool canSpawn = true;
	private int totalScientists = 3;
	private int timeUntilSpawn = 0;
	private int randomSpawnTime = 0;
    private bool missionStart = true;
    public Transform scientist;
    public Transform escort;
    public Transform patient;

    // Currency value
    private int currency = 0;
    public Text currencyText;

    // Build button reference
    public GameObject buildButton;
    // Build panel reference
    public GameObject buildPanel;

    // Character panel reference
    public GameObject charPanel;
    // Character panel picture
    public Image profilePic;
    // Character panel text
    public Text nameText;
    public Text ageText;
    public Text genderText;
    // Character panel sliders
    public Slider identSlider;
    public Slider experSlider;
    public Slider prodSlider;

    // Storage of references to the profile images
    public Sprite portraitA;
    public Sprite portraitB;
    public Sprite portraitC;
    public Sprite portraitD;
    public Sprite portraitE;
    public Sprite portraitF;

    // Use this for initialization
    void Start()
    {
        // Hide build button until required
        buildButton.SetActive(true);

        // Hide build menu until needed
        buildPanel.SetActive(false);

        InitialiseGame();
    }

    // Update is called once per frame
    void Update()
    {
		if (canSpawn == true) 
		{
			// Variables used to spawn new scientists
			randomSpawnTime = Random.Range (1000, 10000);
			timeUntilSpawn++;
		}

        // If the mission is just starting
        if (missionStart)
        {
            // Initialise container object
            delivery = new GameObject();
            delivery.name = "Delivery";

            // Spawn patients and escorts
            SpawnEscort(-70.0f);
            SpawnPatient(-72.0f);
            SpawnPatient(-74.0f);
            SpawnPatient(-76.0f);
            SpawnPatient(-78.0f);
            SpawnEscort(-80.0f);

            // Set mission start to false to avoid additional spawning
            missionStart = false;
        }

		// Scientist spawning
		if (timeUntilSpawn >= randomSpawnTime) 
		{
			// Checks if able to spawn, then spawns a new random scientist
			if (canSpawn == true)
			{
				SpawnScientist();
				timeUntilSpawn = 0;
				totalScientists++;
			}
		}

		// Checks against total max scientists
		if (totalScientists == 5) 
		{
			canSpawn = false;
		}

        if (Input.GetMouseButtonDown(0))
        {
            if (charPanel.activeSelf)
            {
                charPanel.SetActive(false);
            }
        }

        UpdateCurrency();
        missionTimer -= Time.deltaTime;
    }

    private void InitialiseGame()
    {        
        // Spawn initial scientists
        scientists = new GameObject();
        scientists.name = "Scientists";

        // Three preset scientists spawned at game start
        SpawnScientist(2, 1, 5, "Jordan Henderson", 42, "Male", 4, 1, 19.0f);
        SpawnScientist(5, 3, 1, "Kate Green", 26, "Female", 0, 3, 17.0f);
        SpawnScientist(1, 5, -1, "Mary Curran", 34, "Female", 2, 4, 15.0f);

        // Hide character panel
        charPanel.SetActive(false);
    }

    // Used for random scientists
    private void SpawnScientist()
    {
        // Instantiate random scientist at spawn point
        Transform rand = Instantiate(scientist, new Vector3(-70.0f, 1.0f, 0.0f), Quaternion.identity);

        // Call function in character AI to set random stats and details
        rand.GetComponent<CharacterAI>().SetRandomStats();

        // Set the object's tag
        rand.tag = "Scientist";

        // Set parent to container for neatness
        rand.transform.SetParent(scientists.transform);

        // Set destination to entrance room
        NavMeshAgent agent = rand.GetComponent<NavMeshAgent>();

        // Hard coded for now should be changed
        agent.destination = new Vector3(-20.0f, 1.0f, 0.0f);
    }

    // Used for preset scientists
	private void SpawnScientist(int ident, int form, int prod, string name, int age, string gend, int image, int voice, float offset)
    {
        // Instantiate preset scientist in entrance room
        Transform temp = Instantiate(scientist, new Vector3(-offset, 1.0f, 0.0f),
            Quaternion.identity) as Transform;

        // Call function in character AI to set preset stats and details
        temp.GetComponent<CharacterAI>().SetUniqueStats(ident, form, prod, name, age, gend, image, voice);

        // Set the object's tag
        temp.tag = "Scientist";

        // Set parent to container for neatness
        temp.transform.SetParent(scientists.transform);
    }

    // Spawns escort for patient delivery
    private void SpawnEscort(float xPos)
    {
        // Set initial position
        Vector3 spawnPoint = new Vector3(xPos, 0.0f, -2.5f);

        // Instantiate object
        Transform spawn = Instantiate(escort);

        // Set object name and tag
        spawn.tag = "Escort";
        spawn.name = "Escort";

        // Set destination for navmeshagent
        NavMeshAgent agent = spawn.GetComponent<NavMeshAgent>();
        agent.nextPosition = spawnPoint;
        agent.GetComponent<EscortScript>().SetTarget(new Vector3(xPos + 105, 0.0f, -2.5f));

        // Set parent and ensure initial position and rotation correct
        spawn.transform.SetParent(delivery.transform);
        spawn.transform.SetPositionAndRotation(spawnPoint, new Quaternion(-90, 0, 0, 0));
    }

    // Spawns patient for patient delivery
    private void SpawnPatient(float xPos)
    {
        // Set initial position
        Vector3 spawnPoint = new Vector3(xPos, 0.0f, -2.5f);

        // Instantiate object
        Transform spawn = Instantiate(patient);

        // Set object name and tag
        spawn.tag = "Patient";
        spawn.name = "Patient";

        // Set destination for navmeshagent
        NavMeshAgent agent = spawn.GetComponent<NavMeshAgent>();
        agent.nextPosition = spawnPoint;
		agent.GetComponent<PatientScript>().SetTarget(new Vector3(xPos + 105.0f, 0.0f, -2.5f));

        // Set parent and ensure initial position and rotation correct
        spawn.transform.SetParent(delivery.transform);
        spawn.transform.SetPositionAndRotation(spawnPoint, new Quaternion(-90, 0, 0, 0));
    }

    // Fill the data into currency display
    private void UpdateCurrency()
    {
        currencyText.text = "£" + currency.ToString();
    }

    // Launch character card
    public void LaunchCharacterCard(string name, string age, string gender,
        int ident, int exper, int prod, int profile)
    {
        // Unhide panel
        charPanel.SetActive(true);

        // Set text
        nameText.text = name;
        ageText.text = age;
        genderText.text = gender;

        switch(profile)
        {
            case 0:
                profilePic.GetComponent<Image>().sprite = portraitA;
                break;
            case 1:
                profilePic.GetComponent<Image>().sprite = portraitB;
                break;
            case 2:
                profilePic.GetComponent<Image>().sprite = portraitC;
                break;
            case 3:
                profilePic.GetComponent<Image>().sprite = portraitD;
                break;
            case 4:
                profilePic.GetComponent<Image>().sprite = portraitE;
                break;
            case 5:
                profilePic.GetComponent<Image>().sprite = portraitF;
                break;
        }

        // Set sliders
        identSlider.value = ident;
        experSlider.value = exper;
        prodSlider.value = prod;
    }

    public void SetPaused(bool pause)
    {
        currentlyPaused = pause;
    }
    public bool GetPaused()
    {
        return currentlyPaused;
    }

    // Star count getters
    public int GetIStars()
    {
        return identificationStars;
    }
    public int GetEStars()
    {
        return experimentationStars;
    }
    public int GetPStars()
    {
        return productionStars;
    }
    public int GetTotalStars()
    {
        return (identificationStars + experimentationStars + productionStars);
    }
    public float GetTimer()
    {
        return missionTimer;
    }
    public float GetTimerMax()
    {
        return timerMax;
    }
}
