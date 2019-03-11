//-------------------------------
// Created by Lewis Whiteman
// 29/01/2019
//
// Checks if the conditions for loading the minigame are met
// Loads the minigame if the conditions are met
// Controls the spawning of the experiment button
// Displays the success/failure messages accordingly
//
// Edit 11/02/2019:
// - Changed CheckConditions() to check for minigame completion
// Edit 11/03/2019: Lee Elliott
// - Including check to ensure that a mini game is not currently running
//-------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class Experimentation : MonoBehaviour
{
    public GameObject controller;
    public Canvas hudCanvas;

    // How many seconds before the success/failure buttons disappear
    public float cooldown = 10.0f;

    // The time when the Win() or Lose() functions have been called
    float checkTime = 0.0f;

    // The buttons that indicate the status of the minigame controller
    public GameObject
        // Shown when the player can start the minigame
        experiment,
        // Shown when the player completes the minigame successfully
        success,
        // Shown when the player fails the minigame
        failure;

    // If all of these == true then the minigame can start
    public bool 
        // Is there a patient in quarantine?
        patientInQuarantine = false,
        // Is there a working scientist in experimentation?
        workingScientistInExperimentation = false,
        // Has the identification stage been completed?
        identificationComplete = false;

    // Is the minigame already loaded?
    bool minigameLoaded = false;

    public bool displayingButton = false;
    // Room name displayed publically for easy room identification
    public string roomName;

    /// <summary>
    /// Use this for initialisation
    /// </summary>
    private void Start()
    {
        roomName = gameObject.name;

        if (!controller.GetComponent<ControllerScript>().GetPaused())
        {
            // Listens for a click on the identify button and starts the minigame when clicked
            experiment.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartMinigame);
        }
    }

    /// <summary>
    /// Checks if :
    /// - There are more ready identification rooms than experimentation rooms
    /// - There are more patients in quarantine than experimentation rooms
    /// </summary>
    /// <returns>True if both checks are true, else false</returns>
    bool CheckConditions()
    {
        // The amount of identification rooms that are ready
        int identificationCount = 0;

        // For every room with an identification component
        foreach(var i in GameObject.FindObjectsOfType<Identification>())
        {
            // If the identify minigame has been complete for this room
            if (i.minigameComplete)
            {
                identificationCount++;
            }
        }

        // The amount of experimentation rooms that are displaying the EXPERIMENT button
        int experimentationCount = 0;

        // For every room with a Experimentation
        foreach(var e in GameObject.FindObjectsOfType<Experimentation>())
        {
            // If that room is displaying a button
            if(e.displayingButton)
            {
                experimentationCount++;
            }
        }

        // If there are more ready identification rooms than experimentation rooms
        if(experimentationCount < identificationCount)
        {
            // If there are more patients in quarantine than ready experiments
            if(GameObject.FindObjectOfType<Quarantine>().patientCount > experimentationCount)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // If there is a working Scientist in this room and the minigame hasn't already been loaded
        if (workingScientistInExperimentation && !minigameLoaded)
        {
            // If the other conditions are met
            // TODO: Add a delay between each check to be more efficient
            if (CheckConditions())
            {
                // Enable the experiment button
                experiment.SetActive(true);
                displayingButton = true;   
				return;
            }
        }
        else
        {
            // Disable the experiment button
            experiment.SetActive(false);
            displayingButton = false;
			return;
        }

        // If either success or failure are displayed
        if (success.activeSelf || failure.activeSelf)
        {
            // If they have been showing for more than the cooldown time
            if(Time.time > checkTime + cooldown)
            {
                // Disable both of them
                success.SetActive(false);
                failure.SetActive(false);
				return;
            }
        }
    }

    /// <summary>
    /// Flags the minigame as loaded and loads the minigame scene
    /// </summary>
    void StartMinigame()
    {
        minigameLoaded = true;

        // Pause scene in background
        controller.GetComponent<ControllerScript>().SetPaused(true);

        // Hide main HUD
        hudCanvas.gameObject.SetActive(false);

        // Loads the scene in additive mode, loading it on top of the current scene
        SceneManager.LoadScene("Scenes/Mastermind", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Called when the player successfully completes the minigame
    /// </summary>
    public void Win()
    {
        // Show main HUD
        hudCanvas.gameObject.SetActive(true);

        // Unpause main scene
        controller.GetComponent<ControllerScript>().SetPaused(false);

        // Starts the cooldown timer
        checkTime = Time.time;

        // Displays the success button
        success.SetActive(true);

        // Snaps the camera to the canvas as the button is displayed
        Vector3 parentPosition = success.transform.parent.position;
        Camera.main.transform.position =
            new Vector3(parentPosition.x, parentPosition.y, Camera.main.transform.position.z);

        minigameLoaded = false;

        foreach(var i in GameObject.FindObjectsOfType<Identification>())
        {
            if(i.minigameComplete)
            {
                i.minigameComplete = false;
                return;
            }
        }

    }

    /// <summary>
    /// Called when the player fails the minigame
    /// </summary>
    public void Lose()
    {
        // Show main HUD
        hudCanvas.gameObject.SetActive(true);

        // Unpause main scene
        controller.GetComponent<ControllerScript>().SetPaused(false);

        // Starts the cooldown timer
        checkTime = Time.time;

        // Displays the failure button
        failure.SetActive(true);

        // Snaps the camera to the canvas as the button is displayed
        Vector3 parentPosition = success.transform.parent.position;
        Camera.main.transform.position =
            new Vector3(parentPosition.x, parentPosition.y, Camera.main.transform.position.z);
        minigameLoaded = false;

    }

    /// <summary>
    /// Called every frame that a Rigidbody collides with this BoxCollider
    /// </summary>
    /// <param name="other">The Collider that is intersecting this trigger</param>
    private void OnTriggerStay(Collider other)
    {
        // Checks if the scientist is in the experimentation room
        if (other.tag == "Scientist")
        {
            // Checks if the scientist is working
            if (other.GetComponent<CharacterAI>().behaviourState == CharacterAI.BehaviourState.working)
            {
                workingScientistInExperimentation = true;
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
            workingScientistInExperimentation = false;
        }
    }
}