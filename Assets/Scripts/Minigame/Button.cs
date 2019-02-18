//-------------------------------
// Created by Lewis Whiteman
// 22/01/2019
//
// Controls the function of the buttons in the Mastermind minigame
// Sends the appropriate choice alongside providing visual feedback
// that the button has been pressed
//-------------------------------
using UnityEngine;

public class Button : MonoBehaviour
{
    // The color of the gameobject
    Material material;
    /// <summary>
    /// Activated when the player clicks with a mouse or touches the screen
    /// over the current gameobject
    /// </summary>
    private void OnMouseDown()
    {

        // Gets the current color of the button
        material = GetComponent<MeshRenderer>().material;

        // Finds the mastermind controller and adds the last character of the name to the input
        // HACK: Consider finding a way to consistently name the buttons instead of grabbing names
        char buttonName = gameObject.name[gameObject.name.Length - 1];
        GameObject.FindObjectOfType<MastermindController>().input += buttonName;


        // Spawns the element corresponding to the color of this button
        GameObject.FindObjectOfType<MastermindController>().SpawnElement(material);

        // Darkens the button's material color to give visual feedback of a press
        //GetComponent<MeshRenderer>().material.color = material.color - new Color(0.2f, 0.2f, 0.2f);

    }

    /// <summary>
    /// Detects the release of the mouse button or a screen touch
    /// </summary>
    private void OnMouseUp()
    {

        // Resets the button's color back to its original color
        //GetComponent<MeshRenderer>().material.color = material.color;

    }
}