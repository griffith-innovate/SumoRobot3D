using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoRobot : MonoBehaviour
{
    #region Member Variables
    public float speed;                  // Current speed in m/s
    public float acceleration;           // Acceleration in m/s^2
    public float maxSpeed = 1.5f;        // Maximum speed in m/s
    public float downForce = 110.0f;     // Downforce in newtons: (mass * grav) + magnetic_force
    public float lastTime;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        this.lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.time - this.lastTime;
        this.speed += acceleration * deltaTime;
        this.transform.position.x += this.speed;
    }
}
