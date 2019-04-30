using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatientScript : MonoBehaviour
{
    public Vector3 target;

    // Use this for initialization
    void Start ()
    {
        // Start moving instantly
        // Reference to navmeshagent attached to this object
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        // Set the navmeshagent's target destination
        agent.destination = target;
    }

    // Update is called once per frame
    void Update ()
    {
        // Reset scientist rotation to be upright
       //var angles = transform.rotation.eulerAngles;
       //angles.x = -90.0f;
       //transform.rotation = Quaternion.Euler(angles);
    }

    // Set the initial target destination
    public void SetTarget(Vector3 targetPoint)
    {
        target = targetPoint;
    }
}
