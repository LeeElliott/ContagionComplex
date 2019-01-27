﻿//-------------------------------
// Created by Lewis Whiteman
// 21/01/2019
//
// The main controller inside the mastermind minigame.
// Controls:
// - The generation of new chemical chains
// - Updating the chain based on user input
// - Verification of user input
// - The minigame timer
// - Updating the success table based on previous attempts
//-------------------------------
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MastermindController : MonoBehaviour
{
    // Has the player won the minigame?
    public bool hasWon = false;
    
    // The string that will be populated with the solution
    public string solution = "";
    public string input = "";

    // The current length of the solution
    int chainLength = 4;

    // The index of the anchor the current element is linked to
    int anchorIndex = 0;

    // The number of elements that are either:
    // - Correct
    // - Correct color but incorrect position
    // - Incorrect
    [SerializeField]
    int
        correct = 0,
        incorrectPosition = 0,
        incorrect = 0;

    // Table layout is:
    // O | ? | X
    // 2 | 1 | 1
    // 3 | 0 | 1
    // 4 | 0 | 0 <-- win condition

    // O = Correct color and position
    // ? = Correct color wrong position
    // X = Wrong color and position
    public Text successTableText;

    // Contains the pressable button prefabs
    public GameObject[] buttons;
    
    // Contains the anchor points for each element
    public GameObject[] anchors;

    [SerializeField]
    // The anchors that contain the solution positions
    public List<GameObject> anchorsInPlay;

    // The prefab of the element visual
    // TODO: Make sure this still works once element asset is created
    public GameObject elementPrefab;

    // Visual indicator of how much time is left
    // TODO: Make this a UI element instead of a GameObject
    public GameObject timerBar;

    // The initial scale of the timer bar
    Vector3 timerBarInitialScale;

    // The length of the timer in seconds
    public float timerLength = 60.0f;

    // The amount that the timer GameObject scales down by each frame
    float timerScaleDelta;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {

        GenerateChain(chainLength);
        anchorIndex = 0;

        // HACK: Manually setting button colors until the assets are ready
        buttons[0].GetComponent<MeshRenderer>().material.color = Color.red;
        buttons[1].GetComponent<MeshRenderer>().material.color = Color.green;
        buttons[2].GetComponent<MeshRenderer>().material.color = Color.blue;
        buttons[3].GetComponent<MeshRenderer>().material.color = Color.yellow;
        timerBar.GetComponent<MeshRenderer>().material.color = Color.green;

        // Stores the initial world scale of the timer bar
        timerBarInitialScale = timerBar.transform.lossyScale;


        // Instantiate an element prefab for every anchor
        foreach (GameObject g in anchors)
        {
            GameObject instantiated = Instantiate(elementPrefab, g.transform);
        }

        anchorsInPlay = new List<GameObject>();

        // Splits the anchors into adjacent pairs and randomly picks one anchor
        // In each pair to be used for the solution
        // NOTE: This will only work if there are an even amount of anchors
        for (int i = 0; i < anchors.Length; i+=2)
        {
            int indexOffset = Random.Range(0, 2);
            anchorsInPlay.Add(anchors[i + indexOffset]);
        }

        // Sets the default color of the anchors based on whether they are in play
        foreach (GameObject g in anchors)
        {
            if (anchorsInPlay.Contains(g))
            {
                g.GetComponentInChildren<MeshRenderer>().material.color = Color.grey;
            }
            else
            {
                g.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            }
        }
    }

    /// <summary>
    /// Randomly generates a chain of elements and adds them to the solution
    /// </summary>
    /// <param name="length">The length of the chain to be generated</param>
    void GenerateChain(int length)
    {

        // Resets solution and success on creation of a new chain
        solution = "";
        correct = 0;

        // The labels for each button
        // Extend this if more buttons are added
        string possibleChoices = "ABCD";

        // Populates the solution one element at a time
        for (int i = 0; i < length; i++)
        {
            // Adds a single random element to the end of the current solution
            solution += possibleChoices[Random.Range(0, possibleChoices.Length)];
        }
    }

    /// <summary>
    /// Spawns the visuals of the element when it is selected by the player
    /// </summary>
    /// <param name="element">The element to be spawned</param>
    public void SpawnElement(Color elementColor)
    {

        // Changes the color of the current element to the color of the button pressed
        anchorsInPlay[anchorIndex]
            .GetComponentInChildren<MeshRenderer>().material.color = elementColor;

        // Moves the index to the next anchor
        anchorIndex++;


        // TODO: Make the following if statement its own function
        // This function should be called when the player presses a submit button        
        // If the player has made enough guesses to make a chain
        if (input.Length == chainLength)
        {
            // Loops through each element that the user input
            for (int i = 0; i < chainLength; i++)
            {
                // If the solution contains the current element anywhere in the sequence
                if (solution.Contains(input[i].ToString()))
                {
                    string occurenceLocations = "";
                    // If the element at the current position matches the solution it is correct
                    if (solution[i] == input[i])
                    {
                        correct++;
                    }
                    else
                    {
                        // Used for checking if the value of incorrectPosition has changed
                        int oldIncorrectPosCount = incorrectPosition;

                        // For each element in the solution
                        for (int j = 0; j < solution.Length; j++)
                        {
                            // If the element j in the solution matches the current input element
                            if (solution[j] == input[i]
                                // And the input element corresponding to solution[j] isn't correct
                                && input[j] != solution[j])
                            {
                                // And solution[j] hasn't been flagged by a previous element
                                if (!occurenceLocations.Contains(j.ToString()))
                                {
                                    // Flag the current index (j)
                                    occurenceLocations += j.ToString();
                                    incorrectPosition++;
                                    break;
                                }
                            }
                        }

                        // If the incorrect position hasn't changed, the element must be incorrect
                        // In this case the solution contains the color of the current element
                        // But either the player correctly guessed the matching element in the
                        // Solution already, or this condition has been met with a previous element
                        if (incorrectPosition == oldIncorrectPosCount)
                        {
                            incorrect++;
                        }
                    }
                }
                // If the solution does not contain the element then it is incorrect
                else
                {
                    incorrect++;
                }
            }

            // Updates the success table according to the previous guess
            SetTableText();

            // If the player has guessed every element correctly
            if (correct == chainLength)
            {
                // Reset input and call the win condition
                input = "";
                Win();
                anchorIndex = 0;
                ResetColors();

                // TODO: Add correct choice sound
            }
            else
            {
                // Reset the minigame state
                input = "";

                // TODO: Add ResetTableValues function instead of manually resetting each variable
                correct = 0;
                incorrectPosition = 0;
                incorrect = 0;
                anchorIndex = 0;
                ResetColors();

                // TODO: Add incorrect choice sound
            }
        }

        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Gets the scale of the timer bar
        Vector3 scale = timerBar.transform.localScale;

        timerScaleDelta = timerBarInitialScale.x * (Time.deltaTime / timerLength);

        // If the x scale drops below zero, 30 seconds have passed
        // TODO: Add a way to change the length of the timer
        if (scale.x > 0.0f)
        {
            // Store deltaTime in a local variable for consistency
            float delta = Time.deltaTime;
            Vector3 position = timerBar.transform.position;

            // Shrinks the timer bar and moves it along half the distance
            // to accommodate for the centre pivot
            scale.x -= timerScaleDelta;
            position.x -= timerScaleDelta / 2.0f;

            // Sets the modified positions
            timerBar.transform.position = position;
            timerBar.transform.localScale = scale;
        }
        // The scale has dropped below zero and the player has lost the minigame
        else
        {
            Lose();
        }
    }

    void SetTableText()
    {

        // HACK: This scrolls assuming a constant number of lines
        // TODO: Allow for a variable number of lines when scrolling
        int newLines = 0;
        int trimStart = 0, trimCount = 0;
        for (int i = 0; i < successTableText.text.Length; i++)
        {
            
            if (successTableText.text[i] == '\n')
            {
                switch(newLines)
                {
                    case 0: trimStart = i;
                        break;
                    case 1: trimCount = i - trimStart;
                        break;
                }
                
                newLines++;
            }
        }
        if (newLines > 3)
        {
            successTableText.text = successTableText.text.Remove(trimStart,trimCount);
        }


        // Adds a new line to the table
        successTableText.text += '\n';
        
        // Adds the value of correct elements ending with a space for formatting
        successTableText.text += correct.ToString() + ' ';

        // Adds the value of incorrectly positioned elements ending with a space for formatting
        successTableText.text += incorrectPosition.ToString() + ' ';

        // Adds the value of incorrect elements ending with a space for formatting
        successTableText.text += incorrect.ToString();
    }

    void ResetColors()
    {
        // Resets the color of each element
        foreach (GameObject g in anchors)
        {
            if(anchorsInPlay.Contains(g))
            {
                g.GetComponentInChildren<MeshRenderer>().material.color = Color.grey;
            }
            else
            {
                g.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            }
        }
        
    }

    /// <summary>
    /// Called when the player wins the minigame
    /// </summary>
    void Win()
    {
        hasWon = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Mastermind"));

        // TODO: Do stuff...
    }

    /// <summary>
    /// Called when the player loses the minigame
    /// </summary>
    void Lose()
    {
        hasWon = false;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Mastermind"));

        // TODO: Do stuff...

    }
}