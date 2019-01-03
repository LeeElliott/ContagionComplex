//-------------------------------
// Created by Lee Elliott
// 21/10/2018
//
// A script designed to hold all
// camera related functions.
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

            if(clickCount < 2)
            {
                clickCount++;

                // Double click times out in half a second
                Invoke("CancelClick", 0.5f);
            }

            if(clickCount > 1)
            {
                Zoom(mouseOrigin);
                clickCount = 0;
            }
           
            return;
        }

        if(!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
        // Using negative to simulate drag motion rather than following mouse
        Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);

        transform.Translate(move, Space.World);

        // Clamp used to limit distance camera can pan
        if(isZoomed)
        {
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -27.0f, 30.0f),
            Mathf.Clamp(transform.position.y, 2.0f, 25.0f),
            Mathf.Clamp(transform.position.z, -30.0f, -10.0f));
        }
        else
        {
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -16.0f, 21.0f),
            Mathf.Clamp(transform.position.y, 6.0f, 17.0f),
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
                transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 3, -10);
            }
        }
        // Zooms back out to preset location
        else
        {
            transform.position = new Vector3(5, 10, -30);
        }

        isZoomed = !isZoomed;
    }

    void CancelClick()
    {
        clickCount = 0;
    }
}
