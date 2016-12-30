using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class GarbageHingeJoint : MonoBehaviour{

    public Vector3 hingeSpot = Vector3.zero;

    public float maxDist = 5.0f;

    private void Update() {
        Vector3 betweenVect = this.transform.position - hingeSpot;
        if(betweenVect.magnitude > maxDist) {
            betweenVect.Normalize();
            betweenVect *= maxDist;
            this.transform.position = hingeSpot + betweenVect;
        }
    }
}