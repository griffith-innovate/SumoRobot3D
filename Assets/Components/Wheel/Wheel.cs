using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    #region Member Variables
    public float MotorTorque;           // Nm
    public float MotorRPM;              
    public float MotorMaxRPM;

    public float GearRatio;             // 0, ... , 1
    public float GearEfficiency;        // 0, ..., 1
    public float GearOutputRPM;
    public float GearOuputTorque;

    public float WheelDiameter;         // metres
    public float WheelFrictionCoefficient; 
    public float WheelOutputTorque;
    public float WheelOuputForce;
    public float WheelOutputVelocity;

    public float xOffset;
    public float yOffset;

    #endregion
    // Start is called before the first frame update
    void Start()
    {   
        MotorTorque = 1.99f;
        MotorRPM = 0.0f;
        MotorMaxRPM = 8640.0f;
        GearEfficiency = 0.8f;
        WheelDiameter = 0.055f;
        WheelFrictionCoefficient = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Speed will be a value between 0 and 1
    void SetSpeed(float speed){
        float localMax = 1.0f;
        float localMin = -1.0f;

        // Constrain our speed value
        speed = Mathf.Clamp(speed, localMin, localMax);

        // Map MotorRPM to an absolute RPM value
        MotorRPM = (Mathf.Abs(speed) - localMax) * (MotorMaxRPM / localMax);

        // Check if we're going forwards or backwards
        if(speed < 0.0f){
            MotorRPM += -1;
        }

        // Calculate the gear output
        GearOuputTorque = (MotorTorque / GearRatio) * GearEfficiency;
        GearOutputRPM = (MotorRPM * GearRatio) * GearEfficiency;

        // Calculate the wheel torque
        WheelOutputTorque = GearOuputTorque / (WheelDiameter / 2);
        WheelOutputVelocity = (Mathf.PI * WheelDiameter) * (GearOutputRPM / 60);
    }

}
