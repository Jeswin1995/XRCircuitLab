using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceConstrainer : MonoBehaviour
{
    public float forceThreshold = 10f;  // Threshold for detecting force
    public List<GameObject> objectsToMonitor; // List of objects to monitor
    public float lerpSpeed = 5f;        // Speed of the interpolation

    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    private bool thresholdBroken = false;

    void Start()
    {
        // Store the initial positions of all the objects
        foreach (var obj in objectsToMonitor)
        {
            if (obj != null)
            {
                initialPositions[obj] = obj.transform.position;
            }
        }
    }

    void Update()
    {
        bool stopMovement = false;

        // Check force and torque for each child rigidbody
        foreach (GameObject obj in objectsToMonitor)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null && (rb.velocity.magnitude > forceThreshold || rb.angularVelocity.magnitude > forceThreshold))
            {
                stopMovement = true;
                break;  // If any rigidbody exceeds the threshold, stop the movement of all objects
            }
        }

        // If threshold is broken, adjust positions to maintain initial distances
        if (stopMovement && !thresholdBroken)
        {
            thresholdBroken = true;

            // Calculate the initial distances between the objects and store them
            Dictionary<GameObject, Dictionary<GameObject, float>> initialDistances = new Dictionary<GameObject, Dictionary<GameObject, float>>();

            foreach (GameObject obj1 in objectsToMonitor)
            {
                foreach (GameObject obj2 in objectsToMonitor)
                {
                    if (obj1 != obj2)
                    {
                        if (!initialDistances.ContainsKey(obj1))
                            initialDistances[obj1] = new Dictionary<GameObject, float>();

                        initialDistances[obj1][obj2] = Vector3.Distance(initialPositions[obj1], initialPositions[obj2]);
                    }
                }
            }

            // Adjust positions to maintain the initial distances
            foreach (GameObject obj1 in objectsToMonitor)
            {
                foreach (GameObject obj2 in objectsToMonitor)
                {
                    if (obj1 != obj2)
                    {
                        Vector3 direction = obj2.transform.position - obj1.transform.position;
                        float currentDistance = direction.magnitude;
                        float targetDistance = initialDistances[obj1][obj2];

                        // Move object 2 to maintain the initial distance from object 1
                        Vector3 targetPosition = obj1.transform.position + direction.normalized * targetDistance;

                        // Smoothly move the object to the target position using Lerp
                        obj2.transform.position = Vector3.Lerp(obj2.transform.position, targetPosition, lerpSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }
}
