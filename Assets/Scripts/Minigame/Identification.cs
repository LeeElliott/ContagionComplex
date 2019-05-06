//-------------------------------
// Created by Lewis Whiteman
// 29/01/2019
//
// Stores a boolean whether there is a working scientist in the room
//
// Edit 11/02/2019:
// - Added Identification minigame functionality
//-------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class Identification : MonoBehaviour
{
    public Canvas hudCanvas;
    public GameObject controller;
    public bool workingScientistInIdentification = false;
    public bool minigameReady = false;
    public bool minigameComplete = false;
    public bool minigameInProgress = false;
    public GameObject identify, success, failure;
    private float timeActive;
    private float buttonCooldown = 10.0f;

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
            CharacterAI.BehaviourState state = other.GetComponent<CharacterAI>().behaviourState;
            if (state == CharacterAI.BehaviourState.working)
            {
                workingScientistInIdentification = true;
				Debug.Log("ScientistWorking");
            }
        }
    }

    /// <summary>
    /// Use this for initialisation
    /// </summary>
    private void Start()
    {
        if (!controller.GetComponent<ControllerScript>().GetPaused())
        {
            // Listens for a click on the identify button and starts the minigame when clicked
            identify.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartMinigame);
        }
    }

    /// <summary>
    /// This runs every frame
    /// </summary>
    private void Update()
    {
        // If there is a working scientist in the room
        if(workingScientistInIdentification && (!success.activeSelf && !success.activeSelf))
        {
            // The total count of identification rooms that have their minigame ready
            int identificationCount = 0;

            // For every room with an identification component
            foreach (var i in GameObject.FindObjectsOfType<Identification>())
            {
                // If there is a working Scientist in the room
                if (i.minigameReady)
                {
                    identificationCount++;
                }
            }

            // If there are more patients in quarantine than ready experiments
            if (GameObject.FindObjectOfType<Quarantine>().patientCount > identificationCount)
            {
                if(!minigameInProgress)
                    ReadyMinigame();                
            }
        }
        else
        {
            identify.SetActive(false);
        }

        if((success.activeSelf || failure.activeSelf))
        {
            if (Time.time > timeActive + buttonCooldown)
            {
                success.SetActive(false);
                failure.SetActive(false);
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
            //workingScientistInIdentification = false;
			//Debug.Log("Scientist left");
        }
    }

    /// <summary>
    /// Toggles minigame ready state and displays the identify button
    /// </summary>
    private void ReadyMinigame()
    {
        minigameReady = true;
        identify.SetActive(true);
    }

    Camera mainCamera;
    /// <summary>
    /// Toggles minigame ready state and loads the minigame scene
    /// </summary>
    public void StartMinigame()
    {
        if (minigameReady)
        {
            minigameInProgress = true;
            minigameReady = false;
            identify.SetActive(false);
            mainCamera = Camera.main;

            // Pause scene in background
            controller.GetComponent<ControllerScript>().SetPaused(true);

            // Hide main HUD
            hudCanvas.gameObject.SetActive(false);

            SceneManager.LoadScene("SpotTheDifference", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("SpotTheDifference"));

            mainCamera.gameObject.SetActive(false);
        }
    }

    public void WinMinigame()
    {
        // Show main HUD
        hudCanvas.gameObject.SetActive(true);

        // Unpause main scene
        controller.GetComponent<ControllerScript>().SetPaused(false);

        minigameComplete = true;
        minigameInProgress = false;
        success.SetActive(true);
        timeActive = Time.time;
        mainCamera.gameObject.SetActive(true);
        Debug.Log("Win called!");
    }

    public void LoseMinigame()
    {
        // Show main HUD
        hudCanvas.gameObject.SetActive(true);

        // Unpause main scene
        controller.GetComponent<ControllerScript>().SetPaused(false);

        minigameInProgress = false;
        failure.SetActive(true);
        timeActive = Time.time;
        mainCamera.gameObject.SetActive(true);
    }
}
