using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
    #region Public Interfaces
    public float MotorTorque { get; set; } = 0.63f;                             // In Newton metres (Nm)
    [SerializeField]
    private float motorTorque;
    public float MotorMaxRPM { get; set; } = 7920.0f;                           // Get this from the motor data sheet      
    public float WheelDiameter { get; set; } = 0.055f;                          // Metres (m)
    public float WheelFrictionCoefficient { get; set; } = 0.6f;                 // The rolling coefficient between the wheel and the dohyo
    public float WheelOutputForce { get; set; }                                 // The force the wheel adds to the robot (N)
    [SerializeField]
    private float wheelOutputForce;
    public float WheelOutputSpeed { get; set; }                                 // The speed at which the robot will move forward (m/s)
    [SerializeField]
    private float wheelOutputSpeed;
    public float GearRatio { get; set; } = 0.12f;                               // 0, ... , 1
    [SerializeField]
    private float gearRatio;
    public float GearEfficiency { get; set; } = 0.8f;                           // 0, ..., 1
    [SerializeField]
    private float gearEfficiency;


    // Converts the relative speed to an absolute motor RPM and calculates the 
    // wheel output speed and force. 
    public void SetSpeed( float speed ) {
        float localMax = 1.0f;
        float localMin = -1.0f;

        // Constrain our speed value
        speed = Mathf.Clamp( speed, localMin, localMax );

        // Map MotorRPM to an absolute RPM value
        motorRPM = Mathf.Lerp( 0.0f, MotorMaxRPM, Mathf.InverseLerp( 0.0f, 1.0f, Mathf.Abs( speed ) ) );

        // Check if we're going forwards or backwards
        if ( speed < 0.0f ) {
            motorRPM *= -1;
        }

        // Calculate the gear output
        gearOutputTorque = ( MotorTorque / GearRatio ) * GearEfficiency;
        gearOutputRPM = ( motorRPM * GearRatio ) * GearEfficiency;

        // Calculate the wheel torque
        WheelOutputForce = gearOutputTorque / ( WheelDiameter / 2 );
        WheelOutputSpeed = ( Mathf.PI * WheelDiameter ) * ( gearOutputRPM / 60 );

        if ( this.name == "Wheel_FrontRight" ) {
            Debug.Log( string.Format( "MotorRPM: {0}", motorRPM ) );
        }
    }
    #endregion


    #region Private Members
    private float motorRPM;
    private float gearOutputRPM;                                              
    private float gearOutputTorque;
    // Start is called before the first frame update
    void Start() {
        motorRPM = 0.0f;
        GearRatio = Mathf.Clamp( GearRatio, 0, 1 );
        GearEfficiency = Mathf.Clamp( GearEfficiency, 0, 1 );
        MotorTorque = 0.63f;
        MotorMaxRPM = 7920.0f;
        GearRatio = 0.12f;
        GearEfficiency = 0.8f;
        WheelDiameter = 0.055f;
        WheelFrictionCoefficient = 0.6f;
    }

    // Update is called once per frame
    void Update() {
    }

    #endregion

}
