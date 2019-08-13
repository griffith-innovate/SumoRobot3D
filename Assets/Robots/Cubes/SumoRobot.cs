using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoRobot : MonoBehaviour
{
    #region Member Variables
    private float lastTime;

    // Motor and gearing
    public float MotorTorque;
    public float MotorRPM;
    public float GearRatio;
    public float GearEfficiency;

    // Chassis
    public float Width;
    public float Depth;

    // These variables are for positioning the wheels. Assume that the 
    // robot is facing North with the front at positive Y. 
    public float WheelRadius;
    public float FrontWheelAxisY;
    public float RearWheelAxisY;
    public float LeftWheelAxisX;
    public float RightWheelAxisX;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        this.lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
