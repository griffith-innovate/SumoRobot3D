using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAgent : MonoBehaviour
{
    private RobotController Controller;
    public Collider Target;
    public OpticalSensor FoundBy;

    // Start is called before the first frame update
    void Start()
    {
        Controller = FindObjectOfType(typeof(RobotController)) as RobotController;
        Target = null;
        FoundBy = null;
    }

    // Update is called once per frame
    void Update()
    {
        Scan();
        if(Target == null){
            Controller.SetSpeed(0.0f);
            Scout();
        } else {
            Aim();
            Controller.SetSpeed(1.0f);
        }
    }

    void Scan(){
        // Assume we lost them
        Target = null;
        FoundBy = null;

        // Find out what it sees
        foreach(OpticalSensor sensor in Controller.OpticalSensors){
            if(sensor.Hit()){
                Target = sensor.HitObject();
                FoundBy = sensor;
                break;
            }
        }
    }
    void Scout(){
        // Turn around
        Controller.Turn(-10f);
    }

    void Aim(){
        // What is the angle of the sensor that found it
        float angle = FoundBy.Angle();
        Controller.Turn(angle);
    }
}
