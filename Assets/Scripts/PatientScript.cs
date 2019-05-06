//-------------------------------
// Created by Lee Elliott
// 01/03/2019
//
// Edited by Lee Elliott (audio triggering)
// 02/05/2019
//
// A script designed to hold full
// functionality of the escorted
// patients
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatientScript : MonoBehaviour
{
    public Vector3 target;
    private int infection = 0;

    // Use this for initialization
    void Start ()
    {
        // Start moving instantly
        // Reference to navmeshagent attached to this object
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        // Set the navmeshagent's target destination
        agent.destination = target;
    }

    // Update is called once per frame
    void Update ()
    {
        // If zoomed in
        if (Camera.main.GetComponent<CameraScript>().isZoomed && GetComponent<MeshRenderer>().IsVisibleFrom(Camera.main))
        {
            // Generate random number
            int x = Random.Range(0, 999);

            if (x == 123)
            {
                switch(infection)
                {
                    case 0:
                        AkSoundEngine.PostEvent("Play_Bird_FX", gameObject);                        
                        break;
                    case 1:
                        AkSoundEngine.PostEvent("Play_Cotards_FX", gameObject);
                        break;
                    case 2:
                        AkSoundEngine.PostEvent("Play_Cow_FX", gameObject);
                        break;
                }
            }
        }
    }

    // Set the initial target destination
    public void SetTarget(Vector3 targetPoint)
    {
        target = targetPoint;
    }
}
