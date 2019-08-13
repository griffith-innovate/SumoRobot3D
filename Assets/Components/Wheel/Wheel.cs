using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Power
{
    public float RPM;
    public float Torque;
    public float Radius;
}

public class Wheel : MonoBehaviour
{
    #region Member variables
    // Motor and gearing
    public float MotorTorque = 0.63f;        // in Nm
    public float MotorRPM = 0.0f;
    public float MaxMotorRPM = 7920.0f;
    public float GearRatio = 0.8f;
    public float GearEfficiency = 0.8f;

    // These variables are for positioning the wheels. Assume that the 
    // robot is facing North with the front at positive Y. 
    public float WheelRadius = 0.055f / 2.0f;               // 55mm in metres
    public float Acceleration = 0.1f;
    public float Deceleration = 0.5f;

    public float Velocity;
    public float Force;

    public float x;
    public float y;

    #endregion

    public void accelerate()
    {
        Power GearOutput, WheelOutput;

        // Increase the Motor RPM
        MotorRPM += Acceleration;

        // Calculate power of gears and wheels
        GearOutput = calculateGearOutput();
        WheelOutput = calculateWheelOutput(GearOutput);

        // Calculate the velocity and force
        Velocity = (Mathf.PI * WheelOutput.Radius * 2) * (WheelOutput.RPM / 60);
        Force = WheelOutput.Torque / WheelOutput.Radius;
    }
    public void brake()
    {
        Power GearOutput, WheelOutput;

        // Decreate motor RPM
        MotorRPM -= Deceleration;

        // Calculate power of gears and wheels
        GearOutput = calculateGearOutput();
        WheelOutput = calculateWheelOutput(GearOutput);

        // Calculate the velocity and force
        Velocity = (Mathf.PI * WheelOutput.Radius * 2) * (WheelOutput.RPM / 60);
        Force = WheelOutput.Torque / WheelOutput.Radius;
    }

    private Power calculateWheelOutput(Power GearOutput)
    {
        Power WheelOutput = new Power();
        WheelOutput.Torque = GearOutput.Torque / WheelRadius;
        WheelOutput.RPM = GearOutput.RPM;
        WheelOutput.Radius = this.WheelRadius;
        return WheelOutput;
    }

    private Power calculateGearOutput()
    {
        Power Result = new Power();
        Result.Torque = MotorTorque / GearRatio;
        Result.RPM = MotorRPM * GearRatio;
        return Result;
    }
}
