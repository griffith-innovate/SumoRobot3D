using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    public GameObject target = null; // Object to follow
    public Vector3 offset;

    // Start is called before the first frame update
    void Start() {
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update() {

        float newXPosition = target.transform.position.x - offset.x;
        float newZPosition = target.transform.position.z - offset.z;

        transform.position = new Vector3(newXPosition, transform.position.y, newXPosition);
    }
}
