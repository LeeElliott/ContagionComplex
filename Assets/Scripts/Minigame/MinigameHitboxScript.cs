//-------------------------------
// Created by Lewis Whiteman
// 29/01/2019
//
// Updates the MinigameController script when 
// Scientists and Patients enter and exit
// The BoxColliders attached to this GameObject
//-------------------------------
using UnityEngine;

public class MinigameHitboxScript : MonoBehaviour
{
    /// <summary>
    /// Called every frame that a Rigidbody collides with this BoxCollider
    /// </summary>
    /// <param name="other">The Collider that is intersecting this trigger</param>
    private void OnTriggerStay(Collider other)
    {
        // Checks if the patient is in quarantine
        if(other.tag == "Patient" && gameObject.name == "Quarantine")
        {
            GetComponentInParent<MinigameController>().patientInQuarantine = true;
        }

        // Checks if the scientist is in the experimentation room
        if (other.tag == "Scientist" && gameObject.name == "Experimentation")
        {
            // Checks if the scientist is working
            if (other.GetComponent<CharacterAI>().behaviourState == CharacterAI.BehaviourState.working)
            {
                GetComponentInParent<MinigameController>().workingScientistInExperimentation = true;
            }
        }
    }

    /// <summary>
    /// Called once on the frame that a Rigidbody stops colliding with this BoxCollider
    /// </summary>
    /// <param name="other">The Collider that is intersecting this trigger</param>
    private void OnTriggerExit(Collider other)
    {
        // Checks if the patient is no longer in quarantine
        if (other.tag == "Patient" && gameObject.name == "Quarantine")
        {
            GetComponentInParent<MinigameController>().patientInQuarantine = false;
        }

        // Checks if the scientist is no longer in the experimentation room
        if (other.tag == "Scientist" && gameObject.name == "Experimentation")
        {
            GetComponentInParent<MinigameController>().workingScientistInExperimentation = false;
        }
    }
}