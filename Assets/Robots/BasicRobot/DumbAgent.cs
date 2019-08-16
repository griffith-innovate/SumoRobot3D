using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAgent : MonoBehaviour
{
    #region Public Interfaces
    public Collider Target { get; set; }[SerializeField]
    private Collider target;
    public OpticalSensor TargetFoundBy { get; set; }[SerializeField]
    private OpticalSensor targetFoundBy;
    public LineSensor[] LineFoundBy;[SerializeField]
    private LineSensor[] lineFoundBy;
    public bool IsAlive = true;[SerializeField]
    private bool isAlive;
    public string LineDirection;[SerializeField]
    private string lineDirection;
    public float AvgX;[SerializeField]
    private float avgX;
    public float AvgZ;[SerializeField]
    private float avgZ;
    #endregion

    #region Private Members
    private bool turning;
    private RobotController Controller;
    // Start is called before the first frame update
    void Start(){
        Controller = FindObjectOfType( typeof( RobotController ) ) as RobotController;
        Target = null;
        TargetFoundBy = null;
    }

    // Update is called once per frame
    void Update() {
        // Reset the robot if it has fallen off the edge
        if( transform.position.y < -1 ) {
            Controller.Reset();
        }

        // Only do something if it's alive
        if( IsAlive ){
            Scan();
            CheckLineSensors();
            if( Target == null ){
                Controller.SetSpeed(1.0f);
                Scout();
            } else {
                Aim();
                Controller.SetSpeed( 1.0f );
            }
        }
    }

    void CheckLineSensors(){
        Debug.Log("CheckLineSensors()");
        // Clear our line sensor data
        List<LineSensor> LineFoundBy = new List<LineSensor>();

        foreach( LineSensor sensor in Controller.LineSensors ){
            if( sensor.Hit ){
                LineFoundBy.Add( sensor );        
            }
        }
        this.LineFoundBy = LineFoundBy.ToArray();

        // What direction is the line?
        LineDirection = "Test";
        float sumX = 0.0f;
        float sumZ = 0.0f;
        // float avgX, avgZ;
        //foreach ( LineSensor sensor in LineFoundBy ) {
        for(int i = 0; i < this.LineFoundBy.Length; i++ ) {
            LineSensor sensor = this.LineFoundBy[i];
            Vector3 relativePoint = transform.InverseTransformPoint( sensor.transform.position );
            sumX += ( float )System.Math.Round( relativePoint.x, 3 );
            sumZ += ( float )System.Math.Round( relativePoint.z, 3 );
        }
        AvgX = sumX / LineFoundBy.Count;
        AvgZ = sumZ / LineFoundBy.Count;
        if( AvgZ > 0 ) {
            LineDirection = "North";
        } else if( AvgZ < 0 ) {
            LineDirection = "South";
        } else {
            LineDirection = "Middle";
        }
        if( AvgX > 0 ) {
            LineDirection += " East";
        } else if( AvgX < 0 ) {
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
        foreach( OpticalSensor sensor in Controller.OpticalSensors ){
            if( sensor.Hit ){
                Target = sensor.HitObject;
                TargetFoundBy = sensor;
                break;
            }
        }
    }
    void Scout(){
        // Check to see if our robot has hit a white line
        if( turning ){
            if( LineFoundBy.Length == 0 ){
                turning = false;
            }
        } else {
            if( LineFoundBy.Length > 0 ){
                turning = true;
                if( LineDirection.Contains( "East" ) ){
                    Controller.Turn( -30 );
                } else if( LineDirection.Contains( "West" ) ){
                    Controller.Turn( 30 );
                } else {
                    Controller.Turn( 130 );
                }
            }
        }
    }

    void Aim(){
        // What is the angle of the sensor that found it
        float angle = TargetFoundBy.Angle;
        Controller.Turn( angle );
    }
    #endregion
}
