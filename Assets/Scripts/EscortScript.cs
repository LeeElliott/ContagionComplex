using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscortScript : MonoBehaviour
{
    public Vector3 target;

	// Use this for initialization
	void Start ()
    {
        // Start moving instantly
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target;
    }
	
	// Update is called once per frame
	void Update ()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        // Reset scientist rotation to be upright
        var angles = transform.rotation.eulerAngles;
        angles.x = -90.0f;
        transform.rotation = Quaternion.Euler(angles);

        // Return to despawn point
        if (transform.position.x > 38.0f)
        {
            agent.destination = new Vector3(-90.0f, 0.0f, 0.0f);
        }

        // Destroy once out of view
        if (transform.position.x < -80.0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 targetPoint)
    {
        target = targetPoint;
    }
}
