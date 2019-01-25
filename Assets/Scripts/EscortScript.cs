using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscortScript : MonoBehaviour
{
    public Vector3 target = new Vector3(105.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start ()
    {
        // Start moving instantly
        target += transform.position;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Reset scientist rotation to be upright
        var angles = transform.rotation.eulerAngles;
        angles.x = -90.0f;
        transform.rotation = Quaternion.Euler(angles);

        // Return to despawn point
        if (transform.position.x > 38.0f)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = new Vector3(-80.0f, 0.0f, 0.0f);
        }
    }
}
