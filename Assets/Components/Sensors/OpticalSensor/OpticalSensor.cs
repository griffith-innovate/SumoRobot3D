using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticalSensor : MonoBehaviour
{
    #region Public Interfaces
    public float RayLength { get; set; }                                        // Assign length to the raycast
    [SerializeField]
    private float rayLength;
    public bool Hit { get; set; } = false;
    [SerializeField]
    private bool hit;
    // public Rigidbody hitObject;
    public Collider HitObject { get; set; }
    [SerializeField]
    private Collider hitObject;
    public float Angle { get; set; }
    [SerializeField]
    private float angle;
    public float Distance { get; set; }
    [SerializeField]
    private float distance;
    #endregion


    #region Private Members
    private RaycastHit vision;          // Used for detecting Raycast collision

    // Start is called before the first frame update
    void Start(){
        RayLength = 4.0f;
        Hit = false;
        Angle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update(){
        // Reset values
        Hit = false;
        HitObject = null;
        Distance = -1.0f;

        // Visualise the raycast in the scene
        Debug.DrawRay( transform.position, transform.forward * RayLength, Color.red, 0.5f );
        
        // Do something if the raycast hits a rigid body in the scene
        if ( Physics.Raycast( transform.position, transform.forward, out vision, RayLength ) ){
            Hit = true;
            HitObject = vision.collider;
            Distance = vision.distance;
        }
    }
    #endregion
}
