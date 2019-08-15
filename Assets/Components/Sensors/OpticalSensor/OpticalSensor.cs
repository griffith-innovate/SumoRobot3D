using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalSensor : MonoBehaviour
{
    private RaycastHit vision;          // Used for detecting Raycast collision
    private float rayLength;             // Assign length to the raycast
    private bool hit = false;                
    // public Rigidbody hitObject;
    private Collider hitObject;
    private float angle;
    private float distance;

    public float Angle(){
        return angle;
    }
    public float Distance(){
        return distance;
    }
    public Collider HitObject(){
        return hitObject;
    }
    public bool Hit(){
        return hit;
    }
    // Start is called before the first frame update
    void Start()
    {
        rayLength = 4.0f;
        hit = false;
        angle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset values
        hit = false;
        hitObject = null;
        distance = -1.0f;

        // Visualise the raycast in the scene
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red, 0.5f);
        
        // Do something if the raycast hits a rigid body in the scene
        if (Physics.Raycast(transform.position, transform.forward, out vision, rayLength))
        {
            // Output the name of the object the raycast has collided with
            // Debug.Log(vision.collider.name + ": " + vision.distance);
            hit = true;
            hitObject = vision.collider;
            distance = vision.distance;
            // Debug.Log(hitObject);
        }
    }
}
