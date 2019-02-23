//-------------------------------
// Created by Lee Elliott
// 22/01/2019
//
// A script designed to control
// the functionality of the
// spawned objects in the mini game.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    // Bool storing if this a difference
    public bool difference = false;
    private bool found = false;

    // Use this for initialization
	void Start()
    {
        // Hide marker by default
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OnMouseDown()
    {
        // Get reference to controller object
        var controller = GameObject.Find("IdentController");

        if (controller.GetComponent<IdentificationController>().transitionStatus == 0)
        {
            if (difference == true && found == false)
            {
                // Unhide marker
                transform.GetChild(1).gameObject.SetActive(true);

                // Increase found differences counter
                controller.GetComponent<IdentificationController>().foundDifferences++;

                // Initiate first transition
                controller.GetComponent<IdentificationController>().transitionStatus = 1;

                // Ensure double clicks not possible
                found = true;
            }
        }
    }
    bool removedCollider = false;
    public void UpdateCollider()
    {
        if(!removedCollider && !difference)
        {
            Destroy(GetComponent<CircleCollider2D>());
            removedCollider = true;
        }
    }
    public void setOrder(int i)
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = i;
    }
}
