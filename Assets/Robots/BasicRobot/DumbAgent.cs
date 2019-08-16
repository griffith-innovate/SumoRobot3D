using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAgent : MonoBehaviour
{
    private RobotController Controller;
    public Collider Target;
    public OpticalSensor TargetFoundBy;
    public List<LineSensor> LineFoundBy;
    public bool isAlive = true;
    public string LineDirection;
    public float avgX;
    public float avgZ;
    public List<float> xValues;
    public List<float> zValues;
    private bool turning;

    // Start is called before the first frame update
    void Start()
    {
        Controller = FindObjectOfType(typeof(RobotController)) as RobotController;
        Target = null;
        TargetFoundBy = null;
        LineFoundBy = new List<LineSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only do something if it's alive
        if(isAlive){
            Scan();
            CheckLineSensors();
            if(Target == null){
                Controller.SetSpeed(1.0f);
                Scout();
            } else {
                Aim();
                Controller.SetSpeed(1.0f);
            }
        }
    }

    void CheckLineSensors(){
        Debug.Log("CheckLineSensors()");
        // Clear our line sensor data
        LineFoundBy.Clear();

        foreach(LineSensor sensor in Controller.LineSensors){
            if(sensor.Hit()){
                LineFoundBy.Add(sensor);        
            }
        }

        // What direction is the line?
        LineDirection = "Test";
        float sumX = 0.0f;
        float sumZ = 0.0f;
        // float avgX, avgZ;
        foreach(LineSensor sensor in LineFoundBy) {
            Vector3 relativePoint = transform.InverseTransformPoint(sensor.transform.position);
            sumX += (float)System.Math.Round(relativePoint.x, 3);
            sumZ += (float)System.Math.Round(relativePoint.z, 3);
            xValues.Add(relativePoint.x);
            zValues.Add(relativePoint.z);
        }
        avgX = sumX / LineFoundBy.Count;
        avgZ = sumZ / LineFoundBy.Count;
        if(avgZ > 0) {
            LineDirection = "North";
        } else if(avgZ < 0) {
            LineDirection = "South";
        } else {
            LineDirection = "Middle";
        }
        if(avgX > 0) {
            LineDirection += " East";
        } else if(avgX < 0) {
            LineDirection += " West";
        } else {
            LineDirection += " Center";
        }
    }

    void Scan(){
        // Assume we lost them
        Target = null;
        TargetFoundBy = null;

        // Find out what it sees
        foreach(OpticalSensor sensor in Controller.OpticalSensors){
            if(sensor.Hit()){
                Target = sensor.HitObject();
                TargetFoundBy = sensor;
                break;
            }
        }
    }
    void Scout(){
        // Check to see if our robot has hit a white line
        if(turning){
            if(LineFoundBy.Count == 0){
                turning = false;
            }
        } else {
            if(LineFoundBy.Count > 0){
                turning = true;
                if(LineDirection.Contains("East")){
                    Controller.Turn(-30);
                } else if(LineDirection.Contains("West")){
                    Controller.Turn(30);
                } else {
                    Controller.Turn(130);
                }
            }
        }
        // Turn around
            // Controller.Turn(-10f);


    }

    void Aim(){
        // What is the angle of the sensor that found it
        float angle = TargetFoundBy.Angle();
        Controller.Turn(angle);
    }
}
