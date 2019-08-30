using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Ryoma_AgentScript : Agent {
    float speedRewardThreshold = 5.0f;
    RobotController robotController;
    Rigidbody targetBody;
    public int count = 1;
    public float SavedSpeed = 0.0f;
    Rigidbody m_Rigidbody;
    void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
        robotController = GetComponent<RobotController>();
        targetBody = transform.parent.Find("target").GetComponent<Rigidbody>();
    }
    public override void AgentReset() {
        robotController.AgentResetCheck();
    }

    public override void CollectObservations() {
        // Agent Location 
        int counter = 0;
        
        //5 optical sensors.
        foreach (OpticalSensor Sensor in robotController.OpticalSensors) {
            float hit = -1;
            if (Sensor.Hit) {
                hit = 1;
            }
            AddVectorObs(hit);
            counter++;
        }
        //6 Line sensors.
        foreach (var Sensor in robotController.LineSensors) {
            float distance = -1;
            if (Sensor.Hit) {
                distance = Sensor.Distance;
            }
            AddVectorObs(distance);
            counter++;
        }

        AddVectorObs(robotController.Speed);
        AddVectorObs(NormalizeAngle(m_Rigidbody.transform.eulerAngles[1]));
        counter++;
        counter++;
    }

    // private float lastDistFromCenter = 0;
    public override void AgentAction(float[] vectorAction, string textAction) {
        Vector3 controlSignal = Vector3.zero;
        float newSpeed = vectorAction[0];
        float newRotation = vectorAction[1];

        Debug.Log("SetSpeed: " + newSpeed + " SetRotation: " + newRotation);



        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];

        m_Rigidbody.AddForce(controlSignal * 1000);


        //*********** Rewards **************/
        simpleRewardScheme();

    }

    float NormalizeAngle(float value, float max = 360) {
        if (max == 0)
            return 0;
        else {
            return (value / max);
        }
    }
    private void simpleRewardScheme() {
        SetReward(0);
        // at the moment only reward forward movement.
        if (m_Rigidbody.velocity.magnitude > 0) {
            if (m_Rigidbody.velocity.magnitude >= SavedSpeed) {
                count++;
                AddReward(m_Rigidbody.velocity.magnitude * count);
            } else {
                count = 1;
                AddReward(m_Rigidbody.velocity.magnitude * count);
            }
        } else {
            if (m_Rigidbody.velocity.magnitude <= SavedSpeed) {
                count++;
                AddReward(m_Rigidbody.velocity.magnitude * count * -0.4f);
            } else {
                count = 1;
                AddReward(m_Rigidbody.velocity.magnitude * count * -0.4f);
            }
        }
        //reward for staying alive
        float dist = Vector3.Distance(m_Rigidbody.transform.position, robotController.startLocation);
        AddReward(dist * dist);


        // Itself Fell off platform
        if (m_Rigidbody.transform.position.y < 0 || m_Rigidbody.transform.position.y > 2.0f) {
            // SetReward(-50.0f);
            Debug.Log("Fallen, Done");

            Done();
        }
    }
    private void complicatedRewardScheme() {
        var damping = 2;
        var lookPos = targetBody.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        Debug.Log("rotation: " + rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);



        if (robotController.Speed > 0) {
            SetReward(Mathf.Abs(robotController.Speed * 0.1f));
            // SetReward(Mathf.Abs(robotController.DistanceMoved * 0.05f));
        }

        // Target Fell off platform
        if (targetBody.transform.position.y < -1.0f || targetBody.transform.position.y > 2.0f) {
            SetReward(100.0f);
            Done();
        }
        // Itself Fell off platform
        if (m_Rigidbody.transform.position.y < -1.0f || m_Rigidbody.transform.position.y > 2.0f) {
            SetReward(-50.0f);
            Done();
        }

        float distanceToTarget = Vector3.Distance(targetBody.transform.position, transform.position);
        Debug.Log(distanceToTarget);
        // Reached target
        if (distanceToTarget < 0.40f) {
            SetReward(0.2f);
            // Done();
        }
    }
}

