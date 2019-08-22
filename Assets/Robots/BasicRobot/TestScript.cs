using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    #region Member Variables
    Rigidbody mRigid;
    RobotController robotController;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
    robotController = GetComponent<RobotController>();    
    mRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(mRigid.transform.position.x);
    }

    float NormalizeAngle(float value, float max =360){
        if (max == 0)  
            return 0; 
        else{
            return  (value / max);
        }
    }
}


