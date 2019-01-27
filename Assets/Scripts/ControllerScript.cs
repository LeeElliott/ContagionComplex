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
    // Empty used as container in editor
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

        if (missionStart)
        {
            delivery = new GameObject();
            delivery.name = "Delivery";

            // Spawn patients
            SpawnEscort(-70.0f);
            SpawnPatient(-72.0f);
            SpawnPatient(-74.0f);
            SpawnPatient(-76.0f);
            SpawnPatient(-78.0f);
            SpawnEscort(-80.0f);

            missionStart = false;
        }
    }

    private void InitialiseGame()
    {        
        // Spawn initial scientists
        scientists = new GameObject();
        scientists.name = "Scientists";

        SpawnScientist(2, 1, 5, "Jordan Henderson", 42, "Male");
        SpawnScientist(5, 3, 1, "Kate Green", 26, "Female");
        SpawnScientist(1, 5, -1, "Mary Curran", 34, "Female");
    }

    private void SpawnScientist()
    {
        Transform rand = Instantiate(scientist, new Vector3(-30.0f, 1.0f, 0.0f), Quaternion.identity);
        rand.GetComponent<CharacterAI>().SetRandomStats();
        rand.tag = "Scientist";
        rand.transform.SetParent(scientists.transform);
        rand.transform.SetPositionAndRotation(transform.position, new Quaternion(-90, 0, 0, 0));
    }

    private void SpawnScientist(int ident, int form, int prod, string name, int age, string gender)
    {
        Transform temp = Instantiate(scientist, new Vector3(-20.0f, 1.0f, 0.0f), Quaternion.identity) as Transform;
        temp.GetComponent<CharacterAI>().SetUniqueStats(ident, form, prod, name, age, gender);
        temp.tag = "Scientist";
        temp.transform.SetParent(scientists.transform);
    }

    private void SpawnEscort(float xPos)
    {
        Vector3 spawnPoint = new Vector3(xPos, 0.0f, -2.5f);

        Transform spawn = Instantiate(escort, spawnPoint, Quaternion.identity);
        spawn.tag = "Escort";
        spawn.name = "Escort";

        NavMeshAgent agent = spawn.GetComponent<NavMeshAgent>();
        agent.nextPosition = spawnPoint;
        agent.GetComponent<EscortScript>().SetTarget(new Vector3(xPos + 120, 0.0f, -2.5f));

        spawn.transform.SetParent(delivery.transform);
        spawn.transform.SetPositionAndRotation(spawnPoint, new Quaternion(-90, 0, 0, 0));
    }

    private void SpawnPatient(float xPos)
    {
        Vector3 spawnPoint = new Vector3(xPos, 0.0f, -2.5f);

        Transform spawn = Instantiate(patient, spawnPoint, Quaternion.identity);
        spawn.tag = "Patient";
        spawn.name = "Patient";

        NavMeshAgent agent = spawn.GetComponent<NavMeshAgent>();
        agent.nextPosition = spawnPoint;
        agent.GetComponent<PatientScript>().SetTarget(new Vector3(xPos + 120, 0.0f, -2.5f));

        spawn.transform.SetParent(delivery.transform);
        spawn.transform.SetPositionAndRotation(spawnPoint, new Quaternion(-90, 0, 0, 0));
    }
}
