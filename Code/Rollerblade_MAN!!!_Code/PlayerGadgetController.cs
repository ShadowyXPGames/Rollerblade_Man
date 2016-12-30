using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class PlayerGadgetController : MonoBehaviour{

    //GrappleStuff
    ConfigurableJoint myJoint;
    public float maxGrappleDist = 30f;
    public float winchSpeed = 0.5f;
    public float winchMomentumGain;
    public bool grappleOn = false;
    public LayerMask grappleMask;
    public LineRenderer myLR;
    private SoftJointLimit jointLimit = new SoftJointLimit();

    public Camera myCam;
    private Rigidbody myRB;

    private void Start() {
        myJoint = this.GetComponent<ConfigurableJoint>();
        if(myLR == null) {
            Debug.LogWarning("No linerenderer set, attempting to find on one the gameobject");
            this.GetComponent<LineRenderer>();
        }
        if(myCam == null) {
            FindObjectOfType<Camera>();
        }
        myRB = this.GetComponent<Rigidbody>();
        myLR.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        DoGrappleStuff();
    }

    void DoGrappleStuff() {
        if(grappleOn) {
            myLR.SetPosition(0, myLR.transform.position);
            if(Input.GetAxisRaw("Pullwinch/Push") > 0) {
                jointLimit.limit -= winchSpeed * Time.deltaTime;
                if(jointLimit.limit < .5) {
                    jointLimit.limit = 0.5f;
                }
                myJoint.linearLimit = jointLimit;
                Vector3 thingy = myJoint.connectedAnchor - this.transform.position;
                thingy.Normalize();
                myRB.AddForceAtPosition(thingy * winchMomentumGain * Time.deltaTime, thingy * (-this.transform.localScale.x / 5), ForceMode.Acceleration);
            } else if(Input.GetAxisRaw("Pullwinch/Push") < 0) {
                jointLimit.limit += winchSpeed * Time.deltaTime;
                myJoint.linearLimit = jointLimit;
            }
        }
        if(Input.GetButtonDown("Start/Stop Grapple")) {
            RaycastHit hit;
            if(grappleOn == false) {
                if(Physics.Raycast(myCam.ScreenPointToRay(new Vector3(myCam.pixelWidth / 2, myCam.pixelHeight / 2, 0)), out hit, maxGrappleDist)) {
                    int layerTrash = 1 << hit.collider.gameObject.layer;
                    if((layerTrash & grappleMask.value) != 0) {
                        grappleOn = true;
                        myJoint.connectedAnchor = hit.point;
                        jointLimit.limit = (this.transform.position - hit.point).magnitude;
                        myJoint.linearLimit = jointLimit;
                        myLR.enabled = true;
                        myLR.SetPosition(1, hit.point);
                    }
                }
            } else {
                grappleOn = false;
                jointLimit.limit = Mathf.Infinity;
                myJoint.linearLimit = jointLimit;
                myLR.enabled = false;
            }
        }
    }
}