using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class PlayerGadgetController : MonoBehaviour{

    #region grappleVars
    ConfigurableJoint myJoint;
    public float maxGrappleDist = 30f;
    public float winchSpeed = 0.5f;
    public float winchMomentumGain;
    public float hitRadius;
    private bool grappleOn = false;
    public LayerMask grappleMask;
    public LineRenderer myLR;
    private SoftJointLimit jointLimit = new SoftJointLimit();
    #endregion

    #region miscVars
    public Camera myCam;
    private Rigidbody myRB;
    #endregion

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
            #region Winch Stuff
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
                if(jointLimit.limit > maxGrappleDist) {
                    jointLimit.limit = maxGrappleDist;
                }
                myJoint.linearLimit = jointLimit;
            }
            #endregion
        }
        #region Grappling Hook Stuff
        if(Input.GetButtonDown("Start/Stop Grapple")) {
            RaycastHit hit;
            if(grappleOn == false) {
                if(Physics.Raycast(myCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f)), out hit, maxGrappleDist)) {
                    int layerTrash = 1 << hit.collider.gameObject.layer;
                    if((layerTrash & grappleMask.value) != 0) {
                        MakeGrappleHook(hit.point);
                    }
                } else if(Physics.SphereCast(myCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), hitRadius, out hit, maxGrappleDist)) {
                    int layerTrash = 1 << hit.collider.gameObject.layer;
                    if((layerTrash & grappleMask.value) != 0) {
                        MakeGrappleHook(hit.point);
                    }
                }
            } else {
                grappleOn = false;
                jointLimit.limit = Mathf.Infinity;
                myJoint.linearLimit = jointLimit;
                myLR.enabled = false;
            }
        }
        #endregion
    }

    void MakeGrappleHook(Vector3 point) {
        grappleOn = true;
        myJoint.connectedAnchor = point;
        jointLimit.limit = (this.transform.position - point).magnitude;
        myJoint.linearLimit = jointLimit;
        myLR.enabled = true;
        myLR.SetPosition(1, point);
    }
}