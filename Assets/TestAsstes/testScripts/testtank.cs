using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtank : MonoBehaviour
{
    public float speed = 1f;
    public float forward = 1f;
    public bool reversed = false;
    Vector3 b=Vector3.zero;
    public float t = 0f;
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        b=transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (speed > 0f)
        {
            t += Time.fixedDeltaTime;
            rb.velocity = forward * transform.forward * speed;
            //transform.localPosition += forward * transform.forward * speed*Time.fixedDeltaTime;
            if (t > 5f)
            {
                t = 0f;
                forward = -forward;
            }
        }
        
    }

    
}
