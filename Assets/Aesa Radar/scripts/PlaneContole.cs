using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneContole : MonoBehaviour
{

    public float rotationSpeed = 5f;
    public float Speed = 400f;
    public float distancethrottle = 2000f;
    public float maxSpeed = 1200f;
    public float throttle = 1;
    public Vector3 offset = new Vector3(50f, 0, 0);
    public Rigidbody rb;
    public Vazgriz.Plane.AIController aiController;
    public Vazgriz.Plane.Plane plane;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("OverrideControle", 1f);
        Invoke("GivenControle", 3f);
    }

    public void OverrideControle()
    {
        plane.enabled = false;
        Debug.Log("Override");
        //rb.isKinematic = true;
    }
    public void GivenControle()
    {
        //plane.enabled = true;
        //rb.isKinematic = false;
        Debug.Log("Override");
    }

    private void FixedUpdate()
    {
        float dis = Vector3.Distance(aiController.targetPosition ,transform.position);
        if(dis < distancethrottle)
        {
            throttle = dis / distancethrottle;
        }
        else
        {
            throttle = 0.1f;
        }
        
        rb.velocity = transform.forward * ((Speed+((maxSpeed-Speed)*throttle)) / 3.6f);

        // Get the direction to the target
        Vector3 directionToTarget = (aiController.targetPosition+offset) - transform.position;

        // Calculate the rotation to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly interpolate towards the target rotation
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

}
