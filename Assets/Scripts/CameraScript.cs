//-------------------------------
// Created by Lee Elliott
// 21/10/2018
//
// A script designed to hold all
// camera related functions.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    // Variables for panning
	// Position of cursor when mouse dragging starts
    private Vector3 mouseOrigin; 
	// Speed the camera moves at
    private float panSpeed = 0.5f;  

    // Variables for zooming
    public int clickCount = 0;
    public bool isZoomed = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get the left mouse button
        if(Input.GetMouseButtonDown(0))
        {
            // Store mouse position
            mouseOrigin = Input.mousePosition;

            // Less than a double click
            if(clickCount < 2)
            {
                // Increment click counter
                clickCount++;

                // Double click times out in half a second
                Invoke("CancelClick", 0.5f);
            }

            // Double click or more
            if(clickCount > 1)
            {
                // Toggle zoom level
                Zoom(mouseOrigin);

                // Reset click counter
                clickCount = 0;
            }
           
            return;
        }

        // If mouse left button not clicked ignore rest of function
        if(!Input.GetMouseButton(0)) return;

        // Convert cursor screen position to world space
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
        // Using negative to simulate drag motion rather than following mouse
        // Vector3 move = new Vector3(pos.x * -panSpeed, pos.y * -panSpeed, 0);
        Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);

        transform.Translate(move, Space.World);

        // Clamp used to limit distance camera can pan
        if(isZoomed)
        {
            // Clamp position to within these values
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -27.0f, 30.0f),
            Mathf.Clamp(transform.position.y, 2.0f, 22.0f),
            Mathf.Clamp(transform.position.z, -30.0f, -10.0f));
        }
        else
        {
            // Clamp position to within these values
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -16.0f, 21.0f),
            Mathf.Clamp(transform.position.y, 6.0f, 15.0f),
            Mathf.Clamp(transform.position.z, -30.0f, -10.0f));
        }

    }

    // Zooming in could use some work it is slightly inaccurate
    void Zoom(Vector3 pos)
    {
        // Zooms in relative to double tapped point
        if(!isZoomed)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100))
            {
                transform.position = new Vector3(hit.transform.position.x,
                    hit.transform.position.y + 3, -10);
            }
        }
        // Zooms back out to preset location
        else
        {
            // This can be changed
            transform.position = new Vector3(5, 10, -30);
        }

        // Reverse the value of boolean
        isZoomed = !isZoomed;
    }

    // Max time between clicks has exceeded reset counter
    void CancelClick()
    {
        clickCount = 0;
    }
}
