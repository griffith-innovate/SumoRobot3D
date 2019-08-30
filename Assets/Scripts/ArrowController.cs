using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ArrowController : Agent
{

    private Vector3 startLocation;
    private Vector3 startRotation;
    private Vector3 startTargetLocation;
    float speedRewardThreshold = 5.0f;
    // Rigidbody rBody;
    // Transform target;
    public Rigidbody targetBody;
    public int count =1;
    public float SavedSpeed = 0.0f;
    Rigidbody m_Rigidbody;
    public bool collided = false;
    public void OnCollisionEnter(Collision collision){
            if (collision.gameObject.tag == "target"){
                collided = true;
            }
    }


    void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        // robotController = GetComponent<RobotController>(); 
        // target = transform.parent;
        // Debug.Log();
         startLocation = transform.position;
        targetBody = transform.parent.Find("target").GetComponent<Rigidbody>();
        startTargetLocation = targetBody.position;
        m_Rigidbody.transform.Rotate(0.0f, Random.value * 180, 0.0f);

        // targetBody.transform.localPosition = ;
        Vector2 circle = Random.insideUnitCircle * 0.6f;
        targetBody.transform.localPosition = new Vector3(circle[0], 0.5f, circle[1]);
    }
    public override void AgentReset()
    {
          // Debug.Log(targetTransform);
            // If the Agent fell, zero its momentum
            Debug.Log("reset Bot");
            // m_Rigidbody.angularVelocity = Vector3.zero;
            // m_Rigidbody.velocity = Vector3.zero;
            // m_Rigidbody.transform.position = startLocation;
            // m_Rigidbody.transform.eulerAngles = startRotation;
            Vector2 circle = Random.insideUnitCircle * 0.6f;
            targetBody.transform.localPosition = new Vector3(circle[0], 0.5f, circle[1]);
            // targetBody.transform.localPosition = new Vector3(Random.Range(-0.7f, 0.7f), 0, Random.Range(-0.7f, 0.7f));
    }

    public override void CollectObservations()
    {
        Debug.Log("targetLoc: "+targetBody.transform.localPosition +"myLoc: "+transform.localPosition+"myAngle: "+transform.eulerAngles[1]);
        AddVectorObs(targetBody.transform.localPosition);
        // AddVectorObs(transform.localPosition);
        // AddVectorObs(m_Rigidbody.velocity.x);
        // AddVectorObs(m_Rigidbody.velocity.z);
        AddVectorObs(transform.eulerAngles[1]);
        // AddVectorObs(NormalizeAngle(m_Rigidbody.transform.eulerAngles[1]));        
    }

    // private float lastDistFromCenter = 0;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Debug.Log("Updated");
        // Actions, size = 2
        // Vector3 controlSignal = Vector3.zero;
        // float newSpeed = vectorAction[0];
        // float newRotation = vectorAction[1];
        // Debug.Log("SetSpeed: "+ newSpeed + " SetRotation: "+newRotation);
        // controlSignal.x = vectorAction[0];
        // controlSignal.z = vectorAction[1];
        // m_Rigidbody.AddForce(controlSignal * 1000);
        Debug.Log(vectorAction[0]);
        float turnVal = vectorAction[0] * 360;
        Debug.Log("output: "+turnVal);

        m_Rigidbody.transform.Rotate(new Vector3(0,transform.eulerAngles[1]+turnVal,0),Space.World);
        CheckInputs();


        //*********** Rewards **************/
        simpleRewardScheme();
        // Debug.Log(GetStepCount());
        // if (GetStepCount() >=10){
        //     Done();
        // }
    }
     float NormalizeAngle(float value, float max =360){
        if (max == 0)  
            return 0; 
        else{
            return  (value / max);
        }
    }
    private void simpleRewardScheme(){
        if (this.transform.position.y < 0.0f || targetBody.position.y < 0.0f ) {
            Done();
        }
        SetReward(0);
        // at the moment only reward forward movement.
        float dot = Vector3.Dot(transform.up, (targetBody.position - transform.position).normalized);

        float reward = dot;
        reward = Mathf.Max(reward,0);
        reward = reward * reward;
        AddReward(reward);
        Debug.Log("reward: "+reward);
        if(dot > 0.8f) { 
            Debug.Log("Quite facing");
        }

        //reward for staying alive
        // float dist = Vector3.Distance(m_Rigidbody.transform.position, robotController.startLocation);
        // AddReward(dist*dist);

        // Itself Fell off platform
        if (m_Rigidbody.transform.position.y < 0 || m_Rigidbody.transform.position.y > 2.0f)
        {
            // SetReward(-50.0f);
            Debug.Log("Fallen, Done");
            Done();
        }
    }

     // These controls are for testing the robot movements
    void CheckInputs() {
            // Check for right turn
            if (Input.GetKey("w")) {
                // m_Rigidbody.transform.Rotate(0.5f, 0.0f, 0.0f);
                transform.LookAt(targetBody.transform,transform.forward);
                Debug.Log("Rotate up");
            }

            if (Input.GetKey("d")) {
                // m_Rigidbody.transform.Rotate(0.5f, 0.0f, 0.0f);
                m_Rigidbody.transform.Rotate(new Vector3(0,-5,0),Space.World);
                Debug.Log("Rotate right");
            }

            // Check for left turn
            if (Input.GetKey("a")) {
                // m_Rigidbody.transform.Rotate(-0.5f, 0.0f, 0.0f);
                m_Rigidbody.transform.Rotate(new Vector3(0,5,0),Space.World);
                Debug.Log("Rotate left");
            }
    }
}

