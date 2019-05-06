//-------------------------------
// Created by Lewis Whiteman
// 29/01/2019
//
// Stores the number of patients currently in the Quarantine room
//-------------------------------
using UnityEngine;

public class Quarantine : MonoBehaviour
{
    // The number of patients currently in the room
    public uint patientCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        // If a patient enters the room
        if (other.tag == "Patient")
            patientCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        // If a patient leaves the room
       // if (other.tag == "Patient")
       //     patientCount--;
    }
}
