using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    #region Member Variables
    Rigidbody m_Rigidbody;
    public float m_Speed;
    public float m_Acceleration;
    public float m_Brake;
    public float MaxSpeed;
    public float MinSpeed;
    public float MaxTurn;
    public float MinTurn;
    public float Weight;                    // Newtons
    public float MagneticDownforce;         // Newtons
    public Wheel[] Wheels;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Speed = 0.0f;
        m_Acceleration = 0.1f;
        m_Brake = 0.5f;

        // What are the maximum velocity values?
        // +'ive for forwards, -'tive for backwards
        MaxSpeed = 1.0f;        
        MinSpeed = 0.0f;

        // +'tive for Right. -'tive for Left.
        MinTurn = -3.0f;
        MaxTurn = 3.0f;

        // Set motors
    }

    // Update is called once per frame
    void Update()
    {
        checkInputs();
        // Move object
        m_Rigidbody.velocity = transform.forward * m_Speed;
    }

    // Set a relative speed. Valid input range is -1, ... , +1. This will 
    // be mapped to the minimum and maximum speeds. 
    public void setSpeed(float speed){
        float localMin = -1.0f;
        float localMax = 1.0f;

        // Constrain the inputs to our limits
        if(speed > localMax){
            speed = localMax;
        }
        if(speed < localMin){
            speed = localMin;
        }

        // Map the input to a speed
        float m_speed = (speed - localMax) * (MaxSpeed - MinSpeed) / (localMax - localMin) + MinSpeed;
    }

    void checkInputs()
    {
        // Check for acceleration
        if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // m_Speed += m_Acceleration;
            setSpeed(1);
        }

        // Check for deceleration/braking
        if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            // m_Speed -= m_Brake;
            setSpeed(-1);
        }

        // Check for right turn
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0.0f, 0.5f, 0.0f);
        }

        // Check for left turn
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0.0f, -0.5f, 0.0f);
        }

    }

    public bool checkWheelSlip(){
        return false;
    }

}
