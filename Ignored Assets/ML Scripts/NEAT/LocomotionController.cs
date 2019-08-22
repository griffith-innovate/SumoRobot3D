/*
* LocomotionController is responsible for each individual Smart Unit
*/

using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;

public class LocomotionController : UnitController {
    private Rigidbody rBody;
    private bool IsRunning;
    private IBlackBox box;
    private bool toggleTargetTracking = true;//this is set through the TargetController component on the Target GameObject

    public float fitness;
    public GameObject target;
    public float force = 5f;

    //control the values of our velocity toward the target
    public float toVelocity = 2.5f;
    public float maxVelocity = 15.0f;
    public float maxForce = 40.0f;
    public float gain = 5f;
    public ISignalArray inputArr;
    public ISignalArray outputArr;

    // Use this for initialization
    void Start() {
        rBody = GetComponent<Rigidbody>();

        //we get this value each time a new bobber is instantiated
        target = GameObject.Find("Target");

        //find out if we want to gravitate toward the target or not
        toggleTargetTracking = target.GetComponent<TargetController>().isTracked;
    }

    // FixedUpdate called every fixed framerate frame, if the MonoBehaviour is enabled.
    //FixedUpdate should be used instead of Update when dealing with Rigidbody.
    void FixedUpdate() {
        //verify we are running
        if (IsRunning) {
            //determin our heading
            Vector3 heading = target.transform.position - transform.position;

            //get our distace from our target
            float distance = heading.magnitude;


            //if we are interested in moving in direction of the target we can turn this on
            if (toggleTargetTracking) {
                // calc a target vel proportional to distance (clamped to maxVel)
                Vector3 tgtVelocity = Vector3.ClampMagnitude(toVelocity * heading, maxVelocity);

                // calculate the velocity error
                Vector3 error = tgtVelocity - rBody.velocity;

                // calc a force proportional to the error (clamped to maxForce)
                Vector3 lateralForce = Vector3.ClampMagnitude(gain * error, maxForce);

                rBody.AddForce(lateralForce);
            }

            // initialize input and output signal arrays
            inputArr = box.InputSignalArray;
            outputArr = box.OutputSignalArray;

            //use vertical position as input
            //this is so the Neural Net (NN) knows where the cube is in vertical space
            //it can use this value to judge when a thrust should be triggered
            inputArr[0] = transform.position.y;

            //apply force as long as the cube is within a certain distance from a given value
            //the given value is decided on by the Neural Net (NN) based on input values
            if (transform.position.y < (float)outputArr[0]) {
                //inform the NN that a force is being applied to the cube
                inputArr[1] = 1;

                //allow the NN to decide on how much force is going to be applied with the value from `outputArr[1]`
                rBody.AddForce(Vector3.up * (float)outputArr[1] * force, ForceMode.Impulse);

                //allow the NN to decide on which direction angular velocity is going to be 
                //applied with the values from `outputArr[2]` through `outputArr[4]`
                rBody.angularVelocity = new Vector3((float)outputArr[2], (float)outputArr[3], (float)outputArr[4]);

            } else {
                //inform the NN that zero force is being applied to the cube
                inputArr[1] = 0;
            }

            //send the fitness of each cube to the NN so it always has a gauge of how its doing at all times
            //not just at the end of a training session
            inputArr[2] = fitness;
            box.Activate();

            //send a fraction with distance in the denominator
            //this is so we have a value that increases while our distance gets smaller
            //(eg. if distance is 100, our fraction is 1/100)
            //(eg. if distance is 1, our fraction is 1/1 or just a value of 1)

            if (distance > 0) {//cannot divide by zero and cannot be closer to something than 0
                AddFitness(Mathf.Abs(1 / distance));
            }
        }
    }

    public override void Activate(IBlackBox box) {
        this.box = box;
        this.IsRunning = true;
    }

    public override float GetFitness() {
        var fit = fitness;//cache the fitness value
        fitness = 0;//reset fitness value each time we start a new training cycle

        if (fit < 0)
            fit = 0;

        return fit;

    }

    public override void Stop() {
        this.IsRunning = false;
    }

    void AddFitness(float fit) {
        //increment our fitness score on every frame by the fit value
        fitness += fit;
    }
}