// This is essentially the motor controller simulator

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    #region Member Variables
    Rigidbody m_Rigidbody;
    public float m_Speed;
    public float Velocity;                  // Velocity of the robot in m/s
    public float Weight;                    // Newtons
    public float MagneticDownforce;         // Newtons
    public Wheel[] Wheels;
    public OpticalSensor[] OpticalSensors;
    public LineSensor[] LineSensors;
    public float Force;
    // public MotorController MC;
    public bool PControled_1 = false;
    public bool PControled_2 = false;

    public bool setStartLocation;
    private Vector3 startLocation;
    private Vector3 startRotation;
    
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);
        Debug.Log(PControled_1);
        Debug.Log(PControled_2);
        m_Rigidbody = GetComponent<Rigidbody>();
        // m_Rigidbody = gameObject.rigidbody;
        Weight = 3.0f;
        MagneticDownforce = 1000;
        m_Speed = 0.0f;

        // update the reset location of the robot, if not checked, robot will default to zero/zero
        if (setStartLocation){
            Debug.Log("updating start position");
            startLocation = m_Rigidbody.transform.position;
            startRotation = m_Rigidbody.transform.eulerAngles;
        }


        // Get the wheels
        Wheels = GetComponentsInChildren<Wheel>();
        OpticalSensors = GetComponentsInChildren<OpticalSensor>();
        LineSensors = GetComponentsInChildren<LineSensor>();
        
        // Convert our downforce to a mass (kg)
        m_Rigidbody.mass = Weight + (MagneticDownforce / 9.81f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        // Move object
        // m_Rigidbody.velocity = transform.forward * (Velocity + Time.deltaTime);
        m_Rigidbody.transform.Translate(Vector3.forward * Velocity * Time.deltaTime);
        AgentReset();
        // m_Rigidbody.transform.forward 
    }

    // Set a relative speed. Valid input range is -1, ... , +1. This will 
    // be mapped to the minimum and maximum speeds. 
    public void SetSpeed(float speed){
        // Debug.Log("SetSpeed(" + speed + ")");
        float localMin = -1.0f;
        float localMax = 1.0f;

        m_Speed = Mathf.Clamp(speed, localMin, localMax);

        // Increase the RPM for the wheels
        float velocitySum = 0.0f;
        float forceSum = 0.0f;
        foreach(Wheel wheel in Wheels){
            wheel.SetSpeed(speed);
            velocitySum += wheel.WheelOutputSpeed();
            forceSum += wheel.WheelOutputForce();
        }
        Velocity = velocitySum / Wheels.Length;
    }

    public void Turn(float angle){
        transform.Rotate(0.0f, angle, 0.0f);
    }

    void CheckInputs()
    {
        if (PControled_1 == true){
            // Check for acceleration
            if (Input.GetKeyDown("w") )
            {
                // m_Speed += m_Acceleration;
                //SetSpeed(1);
                SetSpeed(m_Speed + 0.1f);
            }

            // Check for deceleration/braking
            if (Input.GetKeyDown("s") )
            {
                // m_Speed -= m_Brake;
                SetSpeed(m_Speed - 0.1f);
            }

            // Check for right turn
            if (Input.GetKey("d") )
            {
                m_Rigidbody.transform.Rotate(0.0f, 0.5f, 0.0f);
            }

            // Check for left turn
            if (Input.GetKey("a") )
            {
                m_Rigidbody.transform.Rotate(0.0f, -0.5f, 0.0f);
            }
        } else if (PControled_2 == true){
            // Check for acceleration
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // m_Speed += m_Acceleration;
                //SetSpeed(1);
                SetSpeed(m_Speed + 0.1f);
            }

            // Check for deceleration/braking
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // m_Speed -= m_Brake;
                SetSpeed(m_Speed - 0.1f);
            }

            // Check for right turn
            if (Input.GetKey(KeyCode.RightArrow))
            {
                m_Rigidbody.transform.Rotate(0.0f, 0.5f, 0.0f);
            }

            // Check for left turn
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                m_Rigidbody.transform.Rotate(0.0f, -0.5f, 0.0f);
            }
        }
    }

    public bool CheckWheelSlip(){
        return false;
    }


    public void AgentReset()
    {
        // Debug.Log(targetTransform);
        if (this.transform.position.y < 0)
        {
            // If the Agent fell, zero its momentum
            Debug.Log("reset Bot");
            m_Rigidbody.angularVelocity = Vector3.zero;
            m_Rigidbody.velocity = Vector3.zero;
            m_Speed =0;
            SetSpeed(m_Speed);
            // m_Acceleration = 0;
            m_Rigidbody.transform.position = startLocation;
            m_Rigidbody.transform.eulerAngles = startRotation;
        }


        // targetRigidBody.angularVelocity = Vector3.zero;
        // targetRigidBody.velocity = Vector3.zero;
        // Move the target to a new spot
        //generate new position: 
        // Vector3 newPos = new Vector3(0f,0.5f,0f);
        // Vector3 newPos = new Vector3(Random.value * 1.22f - 0.64f,0.5f,Random.value * 1.22f - 0.64f);
        // while (Vector3.Distance(this.transform.position,newPos) < 0.30f){
        //     newPos = new Vector3(Random.value * 1.22f - 0.64f,0.5f,Random.value * 1.22f - 0.64f);
        // }
        // targetRigidBody.transform.position = startLocation;
    }

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
