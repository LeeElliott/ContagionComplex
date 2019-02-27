//-------------------------------
// Created by Lee Elliott
// 18/10/2018
//
// Edited by Christopher Pohler (scientist spawning)
// 23/02/2019
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
    // Empty used as containers in editor
    private GameObject scientists;
    private GameObject delivery;

    // Store number of stars earned per discipline
    private int identificationStars = 3;
    private int experimentationStars = 3;
    private int productionStars = 3;

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

    // Character panel reference
    public GameObject charPanel;

	// Use this for initialization
	void Start()
    {
        // Hide build button until required
        buildButton.SetActive(false);

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

        UpdateCurrency();
    }

    private void InitialiseGame()
    {        
        // Spawn initial scientists
        scientists = new GameObject();
        scientists.name = "Scientists";

        // Three preset scientists spawned at game start
        SpawnScientist(2, 1, 5, "Jordan Henderson", 42, "Male", 4);
        SpawnScientist(5, 3, 1, "Kate Green", 26, "Female", 0);
        SpawnScientist(1, 5, -1, "Mary Curran", 34, "Female", 2);

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
    private void SpawnScientist(int ident, int form, int prod, string name, int age, string gender, int image)
    {
        // Instantiate preset scientist in entrance room
        Transform temp = Instantiate(scientist, new Vector3(-20.0f, 1.0f, 0.0f),
            Quaternion.identity) as Transform;

        // Call function in character AI to set preset stats and details
        temp.GetComponent<CharacterAI>().SetUniqueStats(ident, form, prod, name, age, gender, image);

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
        agent.GetComponent<EscortScript>().SetTarget(new Vector3(xPos + 120, 0.0f, -2.5f));

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
        agent.GetComponent<PatientScript>().SetTarget(new Vector3(xPos + 120, 0.0f, -2.5f));

        // Set parent and ensure initial position and rotation correct
        spawn.transform.SetParent(delivery.transform);
        spawn.transform.SetPositionAndRotation(spawnPoint, new Quaternion(-90, 0, 0, 0));
    }

    // Fill the data into currency display
    private void UpdateCurrency()
    {
        currencyText.text = "£" + currency.ToString();
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
}
