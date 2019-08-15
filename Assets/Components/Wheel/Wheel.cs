using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    #region Member Variables
    public float MotorTorque = 0.63f;           // Nm
    private float motorRPM;              
    public float MotorMaxRPM = 7920.0f;

    public float GearRatio = 0.12f;             // 0, ... , 1
    public float GearEfficiency = 0.8f;        // 0, ..., 1
    private float gearOutputRPM;
    private float gearOutputTorque;

    public float WheelDiameter = 0.055f;         // metres
    public float WheelFrictionCoefficient = 0.6f; 
    //private float wheelOutputTorque;
    private float wheelOutputForce;
    private float wheelOutputSpeed;

    #endregion
    // Start is called before the first frame update
    void Start()
    {   
        motorRPM = 0.0f;
        GearRatio = Mathf.Clamp(GearRatio, 0, 1);
        GearEfficiency = Mathf.Clamp(GearEfficiency, 0, 1);
        MotorTorque = 0.63f;
        MotorMaxRPM = 7920.0f;
        GearRatio = 0.12f;
        GearEfficiency = 0.8f;
        WheelDiameter = 0.055f;
        WheelFrictionCoefficient = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Speed will be a value between 0 and 1
    public void SetSpeed(float speed){
        float localMax = 1.0f;
        float localMin = -1.0f;

        // Constrain our speed value
        speed = Mathf.Clamp(speed, localMin, localMax);

        // Map MotorRPM to an absolute RPM value
        // MotorRPM = (Mathf.Abs(speed) - localMax) * (MotorMaxRPM / localMax);
        motorRPM = Mathf.Lerp(0.0f, MotorMaxRPM, Mathf.InverseLerp(0.0f, 1.0f, Mathf.Abs(speed)));

        // Check if we're going forwards or backwards
        if(speed < 0.0f){
            motorRPM *= -1;
        }

        // Calculate the gear output
        gearOutputTorque = (MotorTorque / GearRatio) * GearEfficiency;
        gearOutputRPM = (motorRPM * GearRatio) * GearEfficiency;

        // Calculate the wheel torque
        wheelOutputForce = gearOutputTorque / (WheelDiameter / 2);
        wheelOutputSpeed = (Mathf.PI * WheelDiameter) * (gearOutputRPM / 60);

        if (this.name == "Wheel_FrontRight") {
            Debug.Log(string.Format("MotorRPM: {0}", motorRPM));
        }
    }
    public float WheelOutputForce(){
        return wheelOutputForce;
    }
    public float WheelOutputSpeed() {
        return wheelOutputSpeed;
    }
}
