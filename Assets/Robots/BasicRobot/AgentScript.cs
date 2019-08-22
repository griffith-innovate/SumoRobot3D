using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AgentScript : Agent {
    float speedRewardThreshold = 5.0f;
    RobotController robotController;
    // Rigidbody rBody;
    // Transform target;
    Rigidbody targetBody;

    Rigidbody m_Rigidbody;
    void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
        robotController = GetComponent<RobotController>();
        // target = transform.parent;
        // Debug.Log();
        targetBody = transform.parent.Find("target").GetComponent<Rigidbody>();
    }
    // public Transform targetTransform;
    // public Rigidbody targetRigidBody;
    public override void AgentReset() {
        robotController.AgentResetCheck();
    }

    public override void CollectObservations() {
        // targetTransform and Agent positions
        AddVectorObs(targetBody.transform.position.x);
        AddVectorObs(targetBody.transform.position.y);
        AddVectorObs(targetBody.transform.position.z);
        // Agent Location 
        AddVectorObs(m_Rigidbody.transform.position.x);
        AddVectorObs(m_Rigidbody.transform.position.y);
        AddVectorObs(m_Rigidbody.transform.position.z);
        // Agent velocity
        AddVectorObs(robotController.Speed);
        AddVectorObs(NormalizeAngle(m_Rigidbody.transform.eulerAngles[1]));
    }

    // private float lastDistFromCenter = 0;
    public override void AgentAction(float[] vectorAction, string textAction) {
        // Debug.Log("Updated");
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        float newSpeed = vectorAction[0];
        float newRotation = vectorAction[1];

        Debug.Log("SetSpeed: " + newSpeed + " SetRotation: " + newRotation);

        // m_Rigidbody.AddForce(controlSignal * Speed );
        robotController.SetSpeed(newSpeed);
        robotController.Turn(newRotation);



        //*********** Rewards **************/

        //Reward Fast Speed:
        // if (robotController.Speed ){

        // }
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



        // SetReward(robotController.Speed * 0.1f);
        // Target Fell off platform
        if (targetBody.transform.position.y < -1.0f || targetBody.transform.position.y > 2.0f) {
            SetReward(50.0f);
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

        // Debug.Log(m_Rigidbody.velocity.magnitude);

        // // if


        // // Should probably reward based on outwards magnitude, not any silly old magnitude.
        // if (distanceToTarget < 0.25f)
        // { //We are close to the target
        //     if(targetRigidBody.velocity.magnitude > speedRewardThreshold)
        //     { //We are hitting the cube fast
        //         // float distanceFromCenter = Vector3.Distance(targetRigidBody.transform.position, Vector3.zero);
        //         // float deltaDistanceFromCenter = distanceFromCenter - lastDistFromCenter;
        //         // lastDistFromCenter = distanceFromCenter;
        //         // SetReward(1.0f * deltaDistanceFromCenters);
        //         SetReward(1.0f);

        //     }else
        //     {   //We are hitting the cube slowly
        //         SetReward(-0.1f);
        //     }
        //     // Done();
        // }


        // // if the target fell off the platform
        // if (targetRigidBody.transform.position.y < 0)
        // {
        //     SetReward(20.0f);
        //     Done();
        // }
    }

    float NormalizeAngle(float value, float max = 360) {
        if (max == 0)
            return 0;
        else {
            return (value / max);
        }
    }
}

