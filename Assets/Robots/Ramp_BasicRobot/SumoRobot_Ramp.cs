using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoRobot_Ramp : MonoBehaviour
{
    #region Member Variables
    Rigidbody m_Rigidbody;
    public float m_Speed;
    public float m_Acceleration;
    public float m_Brake;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Speed = 0.0f;
        m_Acceleration = 0.1f;
        m_Brake = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        checkInputs();
        // Move object
        m_Rigidbody.velocity = transform.forward * m_Speed;
    }
    void checkInputs()
    {
        // Check for acceleration
        if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_Speed += m_Acceleration;
        }

        // Check for deceleration/braking
        if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_Speed -= m_Brake;
        }

        // Check for right turn
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            // Debug.Log("Turn right");
            // Vector3 NewTransform = new Vector3(0, 1, 0);
            // transform.Rotate(NewTransform * Time.deltaTime * m_Speed, Space.World);
            // transform.rotation = new Quaternion(0, 1, 0, 0);
            transform.Rotate(0.0f, 0.5f, 0.0f);
        }

        // Check for left turn
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0.0f, -0.5f, 0.0f);
            // Debug.Log("Turn left");
            // transform.rotation = new Quaternion(0, -1, 0, 0);
            // Vector3 NewTransform = new Vector3(0, -1, 0);
            // transform.Rotate(NewTransform * Time.deltaTime * m_Speed, Space.World);
        }

    }
}
