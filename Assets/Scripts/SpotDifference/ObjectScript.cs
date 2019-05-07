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
		var failClick = GameObject.Find ("GameBack");

        if (controller.GetComponent<IdentificationController>().transitionStatus == 0)
        {
			if (difference == false)
			{
				AkSoundEngine.PostEvent ("Play_Missed_Click", gameObject);
			}
				
			if (difference == true && found == false) {
				// Call Wwise sound engine to play correct sound
				AkSoundEngine.PostEvent ("Play_Success_Click", gameObject);

				// Unhide marker
				transform.GetChild (1).gameObject.SetActive (true);

				// Increase found differences counter
				controller.GetComponent<IdentificationController> ().foundDifferences++;

				// Initiate first transition
				controller.GetComponent<IdentificationController> ().transitionStatus = 1;

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
