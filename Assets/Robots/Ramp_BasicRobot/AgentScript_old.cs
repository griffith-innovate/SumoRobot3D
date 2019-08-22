using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AgentScript_old : Agent
{
    float speedRewardThreshold = 5.0f;

    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }
    // public Transform targetTransform;
    public Rigidbody targetRigidBody;
    public override void AgentReset()
    {
        // Debug.Log(targetTransform);
        if (this.transform.position.y < 0)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3( 0, 0.5f, 0);
        }


        targetRigidBody.angularVelocity = Vector3.zero;
        targetRigidBody.velocity = Vector3.zero;
        // Move the target to a new spot
        //generate new position: 
        Vector3 newPos = new Vector3(0f,0.5f,0f);
        // Vector3 newPos = new Vector3(Random.value * 1.22f - 0.64f,0.5f,Random.value * 1.22f - 0.64f);
        // while (Vector3.Distance(this.transform.position,newPos) < 0.30f){
        //     newPos = new Vector3(Random.value * 1.22f - 0.64f,0.5f,Random.value * 1.22f - 0.64f);
        // }
        targetRigidBody.transform.position = newPos;
    }

    public override void CollectObservations()
    {
        // targetTransform and Agent positions
        AddVectorObs(targetRigidBody.transform.position);
        AddVectorObs(this.transform.position);

        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }

    public float speed = 0;
    private float lastDistFromCenter = 0;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Debug.Log("Updated");
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];

        rBody.AddForce(controlSignal * speed );

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                targetRigidBody.transform.position);

        // Debug.Log(distanceToTarget);
        // Reached target
        // if (distanceToTarget < 0.40f)
        // {
        //     SetReward(0.2f);
        //     // Done();
        // }s
        
        Debug.Log(rBody.velocity.magnitude);

        // Should probably reward based on outwards magnitude, not any silly old magnitude.
        if (distanceToTarget < 0.25f)
        { //We are close to the target
            if(targetRigidBody.velocity.magnitude > speedRewardThreshold)
            { //We are hitting the cube fast
                // float distanceFromCenter = Vector3.Distance(targetRigidBody.transform.position, Vector3.zero);
                // float deltaDistanceFromCenter = distanceFromCenter - lastDistFromCenter;
                // lastDistFromCenter = distanceFromCenter;
                // SetReward(1.0f * deltaDistanceFromCenters);
                SetReward(1.0f);

            }else
            {   //We are hitting the cube slowly
                SetReward(-0.1f);
            }
            // Done();
        }

        // Fell off platform
        if (this.transform.position.y < 0)
        {
            SetReward(-10.0f);
            Done();
        }
        // if the target fell off the platform
        if (targetRigidBody.transform.position.y < 0)
        {
            SetReward(20.0f);
            Done();
        }
    }
}
