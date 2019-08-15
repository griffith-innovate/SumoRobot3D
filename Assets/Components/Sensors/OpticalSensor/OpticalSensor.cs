using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalSensor : MonoBehaviour
{
    private RaycastHit Vision;          // Used for detecting Raycast collision
    public float RayLength;             // Assign length to the raycast
    public bool Hit;                
    // public Rigidbody HitObject;
    public Collider HitObject;
    public string Name;
    public float Angle;
    public float Distance;


    // Start is called before the first frame update
    void Start()
    {
        RayLength = 4.0f;
        Hit = false;
        Angle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset values
        Hit = false;
        HitObject = null;
        Distance = -1.0f;

        // Visualise the raycast in the scene
        Debug.DrawRay(transform.position, transform.forward * RayLength, Color.red, 0.5f);
        
        // Do something if the raycast hits a rigid body in the scene
        if (Physics.Raycast(transform.position, transform.forward, out Vision, RayLength))
        {
            // Output the name of the object the raycast has collided with
            // Debug.Log(Vision.collider.name + ": " + Vision.distance);
            Hit = true;
            HitObject = Vision.collider;
            Distance = Vision.distance;
            // Debug.Log(HitObject);
        }
    }
}
