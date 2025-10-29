using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuteralPlaneController : MonoBehaviour
{
    public float speed = 10f;        // Speed of the plane
    public float turnSpeed = 3f;    // Turn speed of the plane
    public float yawSpeed = 2f;     // Yaw speed of the plane

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        rb.velocity = transform.forward * (speed / 3.6f);
        //// Get input from the user
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //// Calculate movement and rotation
        //Vector3 movement = transform.forward * speed;
        //Vector3 rotation = new Vector3(verticalInput*turnSpeed, horizontalInput * turnSpeed, 0);
        //float yawRotation = horizontalInput * yawSpeed;

        //// Apply forces to the Rigidbody
        //rb.AddForce(movement);
        //rb.AddTorque(rotation);
        //rb.AddRelativeTorque(0, yawRotation, 0);
    }
}
