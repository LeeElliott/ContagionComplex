﻿//-------------------------------
// Created by Lee Elliott
// 01/03/2019
//
// A script designed to hold full
// functionality of the escorting
// scientists
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscortScript : MonoBehaviour
{
    public Vector3 target;

	// Use this for initialization
	void Start ()
    {
        // Start moving instantly upon spawning
        // Reference to navmeshagent attached to this object
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        // Set the navmeshagent's target destination
        agent.destination = target;
    }
	
	// Update is called once per frame
	void Update ()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Reset character rotation to be upright
        //var angles = transform.rotation.eulerAngles;
        //angles.x = -90.0f;
        //transform.rotation = Quaternion.Euler(angles);

        // Return to despawn point
        if (transform.position.x > (target.x - 1))
        {
            agent.destination = new Vector3(-90.0f, 0.0f, 0.0f);
        }

        // Destroy once out of view
        if (transform.position.x < -80.0f)
        {
            Destroy(gameObject);
        }
    }

    // Set the initial target destination
    public void SetTarget(Vector3 targetPoint)
    {
        target = targetPoint;
    }
}
