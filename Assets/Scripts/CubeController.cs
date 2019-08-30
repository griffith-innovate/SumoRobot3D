using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CubeController : Agent
{

    private Vector3 startLocation;
    private Vector3 startTargetLocation;
    float speedRewardThreshold = 5.0f;
    // Rigidbody rBody;
    // Transform target;
    public Rigidbody targetBody;
    public int count =1;
    public float SavedSpeed = 0.0f;
    Rigidbody m_Rigidbody;
    private bool collided = false;
    private bool once = false;
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
        // targetBody = transform.parent.Find("target").GetComponent<Rigidbody>();
        startTargetLocation = targetBody.position;
        m_Rigidbody.transform.Rotate(0.0f, Random.value * 180, 0.0f);

        // targetBody.transform.localPosition = ;
        Vector2 circle = Random.insideUnitCircle * 0.6f;
        targetBody.transform.localPosition = new Vector3(circle[0], 0.5f, circle[1]);
    }
    public override void AgentReset()
    {
        // If the Agent fell, zero its momentum
        Debug.Log("reset Bot");


        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.velocity = Vector3.zero;
        transform.position = startLocation;
        targetBody.angularVelocity = Vector3.zero;
        targetBody.velocity = Vector3.zero;

        Vector2 circle = Random.insideUnitCircle * 0.6f;
        transform.localPosition = new Vector3(circle[0], 0.09f, circle[1]);

        circle = Random.insideUnitCircle * 0.6f;
        targetBody.transform.localPosition = new Vector3(circle[0], 0.09f, circle[1]);
    }

    public override void CollectObservations()
    {
        AddVectorObs(targetBody.transform.localPosition);
        AddVectorObs(transform.localPosition);
        AddVectorObs(m_Rigidbody.velocity.x);
        AddVectorObs(m_Rigidbody.velocity.z);
        // AddVectorObs(m_Rigidbody.transform.eulerAngles);
        // AddVectorObs(NormalizeAngle(m_Rigidbody.transform.eulerAngles[1]));        
    }

    // private float lastDistFromCenter = 0;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        Debug.Log(controlSignal);
        m_Rigidbody.AddForce(controlSignal * 100 );
        
        CheckInputs();
        //*********** Rewards **************/
        simpleRewardScheme();
    }
     float NormalizeAngle(float value, float max =360){
        if (max == 0)  
            return 0; 
        else{
            return  (value / max);
        }
    }
    private void simpleRewardScheme(){
        SetReward(0);
        float reward = 1 - Vector3.Distance(transform.position, targetBody.position);
        reward = reward * reward;
        // Debug.Log("reward: "+reward);
        AddReward(reward);

        if(collided && !once ){
            AddReward(20);
            collided = false;
            once = true;
        }
        if (transform.position.y < 0.0f) {
            AddReward(-20);
            Debug.Log("self fallen");
            Done();
        }
        if (targetBody.position.y < 0.0f ) {
            AddReward(50);
            AddReward(1000 - GetStepCount());
            Debug.Log("target fallen");
            Done();
        }
    }

     // These controls are for testing the robot movements
    void CheckInputs() {
            // Check for right turn
            if (Input.GetKey("w")) {
                // m_Rigidbody.transform.Rotate(0.5f, 0.0f, 0.0f);
                Debug.Log("Rotate up");
            }

            if (Input.GetKey("d")) {
                // m_Rigidbody.transform.Rotate(0.5f, 0.0f, 0.0f);
                Debug.Log("Rotate right");
            }

            // Check for left turn
            if (Input.GetKey("a")) {
                // m_Rigidbody.transform.Rotate(-0.5f, 0.0f, 0.0f);
                Debug.Log("Rotate left");
            }
    }
}

