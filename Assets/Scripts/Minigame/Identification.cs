//-------------------------------
// Created by Lewis Whiteman
// 29/01/2019
//
// Stores a boolean whether there is a working scientist in the room
//-------------------------------
using UnityEngine;

public class Identification : MonoBehaviour
{

    public bool workingScientistInIdentification;

    /// <summary>
    /// Called every frame that a Rigidbody collides with this BoxCollider
    /// </summary>
    /// <param name="other">The Collider that is intersecting this trigger</param>
    private void OnTriggerStay(Collider other)
    {
        // Checks if the scientist is in the experimentation room
        if (other.tag == "Scientist" && !workingScientistInIdentification)
        {
            // Checks if the scientist is working
            if (other.GetComponent<CharacterAI>().behaviourState == CharacterAI.BehaviourState.working)
            {
                workingScientistInIdentification = true;
            }
        }
    }

    /// <summary>
    /// Called once on the frame that a Rigidbody stops colliding with this BoxCollider
    /// </summary>
    /// <param name="other">The Collider that is intersecting this trigger</param>
    private void OnTriggerExit(Collider other)
    {
        // Checks if the scientist is no longer in the experimentation room
        if (other.tag == "Scientist")
        {
            workingScientistInIdentification = false;
        }
    }
}
