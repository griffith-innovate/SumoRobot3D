
/*
================================================================================
RobotController

The RobotController is essentially an emulation fo the motor and sensor 
controller. The class is responsible for creating and maintaining the interfaces
for the Neural Network (NN) to control the simulated robot. Robot parameters,
such as weight etc, are also set here. The public interfaces are:
- Speed
- Turn
- OpticalSensors
- LineSensors
- Wheels
================================================================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RobotController : MonoBehaviour {
    #region Public Interfaces
    public float Speed { get; set; }
    public float Velocity { get; set; }                  // Velocity of the robot in m/s
    public float Weight { get; set; }                    // Newtons
    public float MagneticDownforce { get; set; }         // Newtons
    public Wheel[] Wheels { get; set; }
    public OpticalSensor[] OpticalSensors { get; set; }
    public LineSensor[] LineSensors { get; set; }
    public float Force { get; set; }
    // public MotorController MC;
    public bool PControled_1;
    public bool PControled_2;

    public bool SetStartLocation { get; set; }

    public float DistanceMoved;
    public Rigidbody rigidbodyComponent;
    // Start is called before the first frame update
    void Start() {
        Debug.Log(gameObject.name);
        Debug.Log(PControled_1);
        Debug.Log(PControled_2);

        rigidbodyComponent = GetComponent<Rigidbody>();
        Weight = 3.0f;
        MagneticDownforce = 1000;
        Speed = 0.0f;

        // update the reset location of the robot, if not checked, robot will default to zero/zero
        if (SetStartLocation) {
            Debug.Log("updating start position");
            startLocation = rigidbodyComponent.transform.position;
            startRotation = rigidbodyComponent.transform.eulerAngles;
            previousLocation = startLocation;
        }

        // Get the wheels
        Wheels = GetComponentsInChildren<Wheel>();
        OpticalSensors = GetComponentsInChildren<OpticalSensor>();
        LineSensors = GetComponentsInChildren<LineSensor>();

        // Convert our downforce to a mass (kg)
        rigidbodyComponent.mass = Weight + (MagneticDownforce / 9.81f);
    }

    // Update is called once per frame 
    void Update() {
        AgentResetCheck();
        CheckInputs();
        // Move object
        rigidbodyComponent.transform.Translate(Vector3.forward * Velocity * Time.deltaTime);
        DistanceMoved = Vector3.Distance(previousLocation, rigidbodyComponent.transform.position);
        //Reset();
        // rigidbodyComponent.transform.forward 
    }

    // Set a relative speed. Valid input range is -1, ... , +1. This will be 
    // mapped to the minimum and maximum speeds. 
    public void SetSpeed(float speed) {
        // Debug.Log("SetSpeed(" + speed + ")");
        float localMin = -1.0f;
        float localMax = 1.0f;

        Speed = Mathf.Clamp(speed, localMin, localMax);

        // Increase the RPM for the wheels
        float velocitySum = 0.0f;
        float forceSum = 0.0f;
        foreach (Wheel wheel in Wheels) {
            wheel.SetSpeed(speed);
            velocitySum += wheel.WheelOutputSpeed;
            forceSum += wheel.WheelOutputForce;
        }
        Velocity = velocitySum / Wheels.Length;
    }

    // Turns or rotates the robot along the Y-axis
    public void Turn(float angle) {
        transform.Rotate(0.0f, angle, 0.0f);
    }

    // Reset the robot to its original state
    public void Reset() {
        Debug.Log("reset Bot");

        // Set velocity of the robot to zero
        rigidbodyComponent.angularVelocity = Vector3.zero;
        rigidbodyComponent.velocity = Vector3.zero;

        // Stop the robot
        SetSpeed(0);

        // Reset the position of the robot
        rigidbodyComponent.transform.position = startLocation;
        rigidbodyComponent.transform.eulerAngles = startRotation;
    }

    public void AgentResetCheck() {
        // Debug.Log(targetTransform);
        if (this.transform.position.y < -2.0f) {
            // If the Agent fell, zero its momentum
            Debug.Log("reset Bot");
            rigidbodyComponent.angularVelocity = Vector3.zero;
            rigidbodyComponent.velocity = Vector3.zero;
            Speed = 0;
            SetSpeed(Speed);
            rigidbodyComponent.transform.position = startLocation;
            rigidbodyComponent.transform.eulerAngles = startRotation;
        }
    }

    #endregion
    #region Private Members
    private Vector3 startLocation;
    private Vector3 startRotation;
    private Vector3 previousLocation;

    // These controls are for testing the robot movements
    void CheckInputs() {
        if (PControled_1 == true) {
            // Check for acceleration
            if (Input.GetKeyDown("w")) {
                // Speed += m_Acceleration;
                //SetSpeed(1);
                SetSpeed(Speed + 0.1f);
            }

            // Check for deceleration/braking
            if (Input.GetKeyDown("s")) {
                // Speed -= m_Brake;
                SetSpeed(Speed - 0.1f);
            }

            // Check for right turn
            if (Input.GetKey("d")) {
                rigidbodyComponent.transform.Rotate(0.0f, 0.5f, 0.0f);
            }

            // Check for left turn
            if (Input.GetKey("a")) {
                rigidbodyComponent.transform.Rotate(0.0f, -0.5f, 0.0f);
            }
        } else if (PControled_2 == true) {
            // Check for acceleration
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                // Speed += m_Acceleration;
                //SetSpeed(1);
                SetSpeed(Speed + 0.1f);
            }

            // Check for deceleration/braking
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                // Speed -= m_Brake;
                SetSpeed(Speed - 0.1f);
            }

            // Check for right turn
            if (Input.GetKey(KeyCode.RightArrow)) {
                rigidbodyComponent.transform.Rotate(0.0f, 0.5f, 0.0f);
            }

            // Check for left turn
            if (Input.GetKey(KeyCode.LeftArrow)) {
                rigidbodyComponent.transform.Rotate(0.0f, -0.5f, 0.0f);
            }
        }
    }



    #endregion

    // Set the speeds of the wheels so that we can have a differential turn
    // in a direction with a given radius
    // public void Turn(int direction, float radius){
    //     // R = W(V_L + V_R) / 2(V_L - VR)
    //     // Where R = Radius of turn
    //     // W = Distance between Left and Right wheels

    //     // V_L, V_R = Velocity of Left and Right wheels respectively
    //     // These can be calculated using:
    //     // V_L = V ( 1 + W/(2R) )
    //     // V_R = V ( 1 - W/(2R) )
    // }

}