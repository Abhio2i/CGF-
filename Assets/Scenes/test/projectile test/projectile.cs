using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public Transform Cannon;
    public Transform target;
    public float throwingVelocity = 10f;
    public float heightDifference = 0f;
    public Rigidbody bullet;
    private float gravity = 9.81f;
    public float yaw = 0;
    public float setYaw = 0;
    public Transform can;
    public Transform tg;

    public float initialSpeed = 10f; // Initial speed of the projectile
    public float targetDistance = 100f; // Horizontal distance to the target
    public float targetHeight = 10f; // Height of the target
    public float firingObjectHeight = 2f; // Height of the firing object above the ground
    public float Set_deg = 0;
    public float dMax = 0;
    public float b = 0;
    public float angle = 0;
    public float deg = 0;
    void Start()
    {
        if (target != null)
        {
            CalculateLaunchAngle();
        }
    }
    private void Update()
    {
        //CalculateLaunchAngle();
        soumyadipProjectile();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;
            bullet.velocity = bullet.transform.up * throwingVelocity;
        }

    }

    void soumyadipProjectile()
    {
        tg.position = new Vector3(0, targetHeight, 0);
        can.position = new Vector3(0, firingObjectHeight, -targetDistance);
        Cannon.eulerAngles = new Vector3(Set_deg, yaw, 0);

        //float h = targetHeight - firingObjectHeight;
        //h = h<0?-h:h;
        //float g = 9.81f;
        //float u = targetDistance * g;
        //float v = (initialSpeed * initialSpeed) + (2 * h * g);
        //float angle = Mathf.Atan(u/v);

        //// Convert the result from radians to degrees if needed
        //float resultInDegrees = angle * Mathf.Rad2Deg;
        //Cannon.eulerAngles = new Vector3(resultInDegrees, yaw, 0);

        float g = 9.81f;
        float v2 = initialSpeed * initialSpeed;
        dMax = v2 / g;
        b = (targetDistance * g) / v2;
        angle = Mathf.Asin(b)/2;
        deg = angle * Mathf.Rad2Deg;
        //Cannon.eulerAngles = new Vector3(deg, yaw, 0);
    }

    void CalculateLaunchAngle()
    {
        float g = 9.81f; // Acceleration due to gravity
        float vSquare = initialSpeed * initialSpeed;
        float xSquare = targetDistance * targetDistance;
        float y = targetHeight;
        float h = firingObjectHeight;
        float sqrtValue = vSquare * vSquare - g * (g * xSquare + 2 * (y + h) * vSquare);

        if (sqrtValue < 0)
        {
            Debug.LogError("No valid solution. The target is out of reach.");
            return;
        }

        float angle1 = Mathf.Atan2(vSquare + Mathf.Sqrt(sqrtValue), g * targetDistance);
        float angle2 = Mathf.Atan2(vSquare - Mathf.Sqrt(sqrtValue), g * targetDistance);


        // Convert angles from radians to degrees
        float angleInDegrees1 = Mathf.Rad2Deg * angle1;
        float angleInDegrees2 = Mathf.Rad2Deg * angle2;
        Cannon.eulerAngles = new Vector3(angleInDegrees1, yaw, 0);
        Debug.Log("Launch Angle 1: " + angleInDegrees1 + " degrees");
        Debug.Log("Launch Angle 2: " + angleInDegrees2 + " degrees");
    }
    //void CalculateLaunchAngle()
    //{
    //    Vector3 toTarget = target.position - Cannon.position;
    //    float horizontalDistance = new Vector3(toTarget.x, 0, toTarget.z).magnitude;
    //    float verticalDistance = toTarget.y; // Vertical difference between the target and the projectile
    //    heightDifference = verticalDistance;
    //    float launchAngle = CalculateLaunchAngle(throwingVelocity, horizontalDistance, verticalDistance);
    //    float yawAngle = CalculateYawAngle(toTarget);
        

    //    yaw = yawAngle;
    //    // Convert launch angle from radians to degrees
    //    float launchAngleDegrees = Mathf.Rad2Deg * launchAngle;
    //    Cannon.eulerAngles = new Vector3(launchAngleDegrees, yaw,0);
    //    Debug.Log("Launch Angle (degrees): " + launchAngleDegrees);
    //}

    float CalculateLaunchAngle(float v, float d, float h)
    {
        float g = gravity;
        float angle = Mathf.Atan((v * v + Mathf.Sqrt(v * v * v * v - g * (g * d * d + 2 * h * v * v))) / (g * d));
        return angle;
    }
    float CalculateYawAngle(Vector3 toTarget)
    {
        float yawAngle = Mathf.Atan2(toTarget.x, toTarget.z);
        return Mathf.Rad2Deg * yawAngle; // Convert to degrees
    }
    float CalculateHeightAngle(float v, float h)
    {
        float g = gravity;
        float angle = Mathf.Atan(v / Mathf.Sqrt(2 * h / g));
        return angle;
    }
    //float CalculateYawAngle(Vector3 toTarget)
    //{
    //    float yawAngle = Vector3.SignedAngle(Cannon.forward, toTarget, Vector3.up);
    //    return yawAngle;
    //}




}
