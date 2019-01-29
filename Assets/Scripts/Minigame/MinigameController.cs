//-------------------------------
// Created by Lewis Whiteman
// 29/01/2019
//
// Checks if the conditions for loading the minigame are met
// Loads the minigame if the conditions are met
// Controls the spawning of the experiment button
// Displays the success/failure messages accordingly
//-------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameController : MonoBehaviour
{
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

    /// <summary>
    /// Use this for initialisation
    /// </summary>
    private void Start()
    {
        // Listens for a click on the experiment button and starts the minigame when clicked
        experiment.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartMinigame);        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // If all conditions are met to start the minigame
        if(patientInQuarantine && workingScientistInExperimentation && identificationComplete)
        {
            // If the minigame hasn't already been loaded
            if (!minigameLoaded)
            {
                // Enable the experiment button
                experiment.SetActive(true);
            }
            else
            {
                // Disable the experiment button
                experiment.SetActive(false);
            }
        }

        // If either success or failure are displayed
        if(success.activeSelf || failure.activeSelf)
        {
            // If they have been showing for more than the cooldown time
            if(Time.time > checkTime + cooldown)
            {
                // Disable both of them
                success.SetActive(false);
                failure.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Flags the minigame as loaded and loads the minigame scene
    /// </summary>
    void StartMinigame()
    {
        minigameLoaded = true;

        // Loads the scene in additive mode, loading it on top of the current scene
        SceneManager.LoadScene("Scenes/Mastermind", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Called when the player successfully completes the minigame
    /// </summary>
    public void Win()
    {
        // Starts the cooldown timer
        checkTime = Time.time;

        // Displays the success button
        success.SetActive(true);

        // Snaps the camera to the canvas as the button is displayed
        Vector3 parentPosition = success.transform.parent.position;
        Camera.main.transform.position =
            new Vector3(parentPosition.x, parentPosition.y, Camera.main.transform.position.z);
    }

    /// <summary>
    /// Called when the player fails the minigame
    /// </summary>
    public void Lose()
    {
        // Starts the cooldown timer
        checkTime = Time.time;

        // Displays the failure button
        failure.SetActive(true);

        // Snaps the camera to the canvas as the button is displayed
        Vector3 parentPosition = success.transform.parent.position;
        Camera.main.transform.position =
            new Vector3(parentPosition.x, parentPosition.y, Camera.main.transform.position.z);
    }
}