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

public class ControllerScript : MonoBehaviour
{
    // Empty used as container
    private GameObject scientists;

    // Used for spawning
    private float spawnDelay;
    private bool canSpawn = false;
    public Transform scientist;

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
}
