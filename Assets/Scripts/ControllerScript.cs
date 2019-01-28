//-------------------------------
// Created by Lee Elliott
// 18/10/2018
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

    // Used for spawning
    private float spawnDelay;
    private bool canSpawn = false;
    private bool missionStart = true;
    public Transform scientist;
    public Transform escort;
    public Transform patient;

	// Use this for initialization
	void Start()
    {
        InitialiseGame();

        spawnDelay = Random.Range(20.0f, 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Scientist spawning
        if (canSpawn)
        {
            if (spawnDelay < 0.1f)
            {
                // Spawn random scientist
                SpawnScientist();
            }
            else
            {
                // Decrement timer
                spawnDelay -= Time.deltaTime;
            }
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
    }

    private void InitialiseGame()
    {        
        // Spawn initial scientists
        scientists = new GameObject();
        scientists.name = "Scientists";

        // Three preset scientists spawned at game start
        SpawnScientist(2, 1, 5, "Jordan Henderson", 42, "Male");
        SpawnScientist(5, 3, 1, "Kate Green", 26, "Female");
        SpawnScientist(1, 5, -1, "Mary Curran", 34, "Female");
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
    private void SpawnScientist(int ident, int form, int prod, string name, int age, string gender)
    {
        // Instantiate preset scientist in entrance room
        Transform temp = Instantiate(scientist, new Vector3(-20.0f, 1.0f, 0.0f),
            Quaternion.identity) as Transform;

        // Call function in character AI to set preset stats and details
        temp.GetComponent<CharacterAI>().SetUniqueStats(ident, form, prod, name, age, gender);

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
}
