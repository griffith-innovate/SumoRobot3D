using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSensor : MonoBehaviour
{
    public float Range = 0.1f;
    private RaycastHit vision;          // Used for detecting Raycast collision
    public bool hit = false;                
    // public Rigidbody hitObject;
    public Collider hitObject;
    private float angle;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
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
        Debug.DrawRay(transform.position, transform.forward * Range, Color.yellow, 0.5f);
        
        // Do something if the raycast hits a rigid body in the scene
        if (Physics.Raycast(transform.position, transform.forward, out vision, Range))
        {
            // Output the name of the object the raycast has collided with
            // Debug.Log(vision.collider.name + ": " + vision.distance);
            // hit = true;
            hitObject = vision.collider;
            distance = vision.distance;
            if(hitObject.name == "WhiteLine"){
                hit = true;
            }
            // Debug.Log(hitObject);
        }
        
    }
    
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
}
