using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class CameraMovement : MonoBehaviour{

    public Transform moveTarget;

    Vector3 currentVel;

    // Update is called once per frame
    void Update() {
        const float smoothTime = 0.01f;
        Vector3 position = Vector3.SmoothDamp(this.transform.position, moveTarget.transform.position, ref currentVel, smoothTime);
        this.transform.position = position;
    }
}