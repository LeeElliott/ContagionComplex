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

    public void setOrder(int i)
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = i;
    }
}
