using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //move stuff
    Rigidbody myRB;
    public float moveSpeed = 1.0f;
    private float sphereRadius;

    //Look stuff
    public float mouseSensitivity;
    public Vector2 VerticalLookConstraints;
    float horizontalLookRotation;
    float verticalLookRotation;
    public Transform myCamera;

    // Use this for initialization
    void Start() {
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