using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region moveVars
    private Rigidbody myRB;
    public float moveSpeed = 1.0f;
    private float sphereRadius;
    #endregion

    #region lookVars
    public float mouseSensitivity;
    public Vector2 VerticalLookConstraints;
    public float beginningHorizontalLookRotation;
    private float horizontalLookRotation;
    private float verticalLookRotation;
    public Transform myCamera;
    #endregion

    // Use this for initialization
    void Start() {
        horizontalLookRotation = beginningHorizontalLookRotation;
        myRB = this.GetComponent<Rigidbody>();
        if(myCamera == null) {
            myCamera = FindObjectOfType<Camera>().transform;
        }
        if(myCamera == null) {
            Debug.LogError("There is no camera!");
        }
        sphereRadius = this.transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update() {
        SetLookRotations();
        #region movementBasedOnInput
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(input.x > 0) {
            myRB.AddForceAtPosition(myCamera.transform.right * moveSpeed * Time.deltaTime, (-myCamera.transform.right + this.transform.position) * sphereRadius, ForceMode.Acceleration);
        }
        if(input.x < 0) {
            myRB.AddForceAtPosition(-myCamera.transform.right * moveSpeed * Time.deltaTime, (-myCamera.transform.right + this.transform.position) * sphereRadius, ForceMode.Acceleration);
        }
        if(input.y > 0) {
            myRB.AddForceAtPosition(myCamera.transform.forward * moveSpeed * Time.deltaTime, (-myCamera.transform.forward + this.transform.position) * sphereRadius, ForceMode.Acceleration);
        }
        if(input.y < 0) {
            myRB.AddForceAtPosition(-myCamera.transform.forward * moveSpeed * Time.deltaTime, (myCamera.transform.forward + this.transform.position) * sphereRadius, ForceMode.Acceleration);
        }
        #endregion
    }

    void SetLookRotations() {
        //Debug.Log(Input.GetAxis("Mouse X") * mouseSensitivity);
        horizontalLookRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation,
            VerticalLookConstraints.x,
            VerticalLookConstraints.y);
        myCamera.rotation = Quaternion.Euler((Vector3.left * verticalLookRotation) + (Vector3.up * horizontalLookRotation));
    }
}