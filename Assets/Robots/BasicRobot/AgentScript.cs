using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AgentScript : Agent {
    float speedRewardThreshold = 5.0f; 
    RobotController robotController;
    // Rigidbody rBody;
    // Transform target;
    public Rigidbody targetBody;
    public int count =1;
    public float SavedSpeed = 0.0f;
    Rigidbody m_Rigidbody;
    private Vector3 startLocation;
    private Vector3 startTargetLocation;
    private bool collided = false;
    private bool once = false;
    public void OnCollisionEnter(Collision collision){
            if (collision.gameObject.tag == "target"){
                collided = true;
            }
    }
    void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        robotController = GetComponent<RobotController>(); 
        m_Rigidbody.transform.Rotate(0.0f, 0.0f, 0.0f);
        startLocation = transform.localPosition;

        // targetBody = transform.parent.Find("target").GetComponent<Rigidbody>();
        startTargetLocation = targetBody.transform.localPosition;
        targetBody.transform.Rotate(0.0f, 0.0f, 0.0f);

        // targetBody.transform.localPosition = ;
        // Vector2 circle = Random.insideUnitCircle * 0.6f;
        // targetBody.transform.localPosition = new Vector3(circle[0], 0.5f, circle[1]);
    }
    // public Transform targetTransform;
    // public Rigidbody targetRigidBody;
    public override void AgentReset() 
    {
        // robotController.AgentResetCheck();
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.velocity = Vector3.zero;
        transform.localPosition = startLocation;
        transform.Rotate(0.0f, 0.0f, 0.0f);
        // transform.rotation = Quaternion.Euler(90f, -90f, 0f);

        targetBody.angularVelocity = Vector3.zero;
        targetBody.velocity = Vector3.zero;
        targetBody.transform.localPosition = startTargetLocation;
        targetBody.transform.Rotate(0.0f, 0.0f, 0.0f);
        // Vector2 circle = Random.insideUnitCircle * 0.6f;
        // transform.localPosition = new Vector3(circle[0], 0.09f, circle[1]);

        // circle = Random.insideUnitCircle * 0.6f;
        // targetBody.transform.localPosition = new Vector3(circle[0], 0.09f, circle[1]);
    }

    public override void CollectObservations()
    {
        // targetTransform and Agent positions
        //5 optical sensors.
        foreach (OpticalSensor Sensor in robotController.OpticalSensors)
        {
            float hit = -1;
            if(Sensor.Hit){
                hit = Sensor.Distance;
            }
            AddVectorObs(hit);
        }
        //6 Line sensors.
        foreach (var Sensor in robotController.LineSensors)
        {
            float hit = -1;
            if(Sensor.Hit){
                hit = 1;
            }
            AddVectorObs(hit);
        }
        // Agent velocity
        AddVectorObs(robotController.Speed);
        // AddVectorObs(NormalizeAngle(m_Rigidbody.transform.eulerAngles[1]));        
    }

    // private float lastDistFromCenter = 0;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Debug.Log("Updated");
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        float newSpeed = vectorAction[0];
        float newRotation = vectorAction[1];

        Debug.Log("SetSpeed: "+ newSpeed + " SetRotation: "+newRotation);
        // Debug.Log("velocity: "+ m_Rigidbody.velocity.magnitude);
        // m_Rigidbody.AddForce(controlSignal * Speed );
        robotController.SetSpeed(newSpeed);
        robotController.Turn(newRotation);



        // controlSignal.x = vectorAction[0];
        // controlSignal.z = vectorAction[1];

        // m_Rigidbody.AddForce(controlSignal * 1000);
       
       
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
        Debug.Log("reward: "+reward);
        
        
        string sensorOutput="SensorOutput: ";
        foreach (OpticalSensor Sensor in robotController.OpticalSensors)
        {
            float hit = -1;
            if(Sensor.Hit){
                // if (Sensor.name)
                hit = Sensor.Distance;
                reward += (1-hit)*(1-hit);        
            }
            // AddVectorObs(hit);
            sensorOutput = sensorOutput += ","+ hit;
        }

         foreach (var Sensor in robotController.LineSensors)
        {
            float hit = -1;
            if(Sensor.Hit){
                // if (Sensor.name)
                hit = -0.5f;
                reward += hit;
            }
            // AddVectorObs(hit);
            sensorOutput = sensorOutput += ","+ hit;
        }

        AddReward(reward);
        Debug.Log(sensorOutput);

        if(collided && !once ){
            AddReward(50);
            collided = false;
            once = true;
        }
        if (transform.position.y < 0.0f) {
            AddReward(-50);
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
    private void complicatedRewardScheme(){
        var damping =2;
        var lookPos = targetBody.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        Debug.Log("rotation: "+rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        // at the moment only reward forward movement.
        // if(m_Rigidbody.velocity.magnitude > 0){
        //     if (m_Rigidbody.velocity.magnitude >= SavedSpeed ){
        //         count ++;
        //         AddReward(m_Rigidbody.velocity.magnitude * count);
        //     }else{
        //         count = 1;
        //         AddReward(m_Rigidbody.velocity.magnitude * count);
        //     }
        // }else{
        //     if (m_Rigidbody.velocity.magnitude <= SavedSpeed ){
        //         count ++;
        //         AddReward(m_Rigidbody.velocity.magnitude * count * -0.4f);
        //     }else{
        //         count = 1;
        //         AddReward(m_Rigidbody.velocity.magnitude * count * -0.4f);
        //     }
        // }
        // if(robotController.Speed > 0){
        //     if (robotController.Speed >= SavedSpeed ){
        //         count ++;
        //         AddReward(robotController.Speed * count);
        //     }else{
        //         count = 1;
        //         AddReward(robotController.Speed * count);
        //     }
        // }else{
        //     if (robotController.Speed <= SavedSpeed ){
        //         count ++;
        //         AddReward(robotController.Speed * count * -0.4f);
        //     }else{
        //         count = 1;
        //         AddReward(robotController.Speed * count * -0.4f);
        //     }
        // }
        // if( robotController.Speed > 0 ){
        //     // Debug.Log("positiveSpeed");
        //     AddReward(robotController.Speed);
        //     // SetReward(Mathf.Abs(robotController.DistanceMoved * 0.05f));
        // }


        
        if( robotController.Speed > 0 ){
            SetReward(Mathf.Abs(robotController.Speed * 0.1f));
            // SetReward(Mathf.Abs(robotController.DistanceMoved * 0.05f));
        }
        
        // Float Reward = 1 - (distance/distance_max)* 0.4;
        
                
        // SetReward(robotController.Speed * 0.1f);
        // Target Fell off platform
        if (targetBody.transform.position.y < -1.0f || targetBody.transform.position.y > 2.0f)
        {
            SetReward(100.0f);
            Done();
        }
        // Itself Fell off platform
        if (m_Rigidbody.transform.position.y < -1.0f || m_Rigidbody.transform.position.y > 2.0f)
        {
            SetReward(-50.0f);
            Done();
        }

        float distanceToTarget = Vector3.Distance(targetBody.transform.position, transform.position);
        Debug.Log(distanceToTarget);
        // Reached target
        if (distanceToTarget < 0.40f)
        {
            SetReward(0.2f);
            // Done();
        }
                //Reward Fast Speed:
        // if (robotController.Speed ){
            
        // }
      
        
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
}

