using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private RaycastHit vision;          // Used for detecting Raycast collision
    public float rayLength;             // Assign length to the raycast
    public bool hit;
    public Rigidbody hitObject;


    // Start is called before the first frame update
    void Start()
    {
        rayLength = 4.0f;
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Visualise the raycast in the scene
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red, 0.5f);

        // Do something if the raycast hits a rigid body in the scene
        if (Physics.Raycast(transform.position, transform.forward, out vision, rayLength))
        {
            // Output the name of the object the raycast has collided with
            Debug.Log(vision.collider.name + ": " + vision.distance);
        }
    }
}
