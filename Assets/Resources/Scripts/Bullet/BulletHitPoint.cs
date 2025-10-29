using Unity.VisualScripting;
using UnityEngine;

public class BulletHitPoint : MonoBehaviour
{
    public Transform plane1; // The firing plane
    public Transform plane2; // The target plane
    public float bulletSpeed = 800f; // Speed of the bullet relative to the firing plane
    public float plane1v = 800f;
    public float plane2v = 800f;
    public Vector3 plane1Velocity = new Vector3(200, 0, 0); // Velocity of the firing plane
    public Vector3 plane2Velocity = new Vector3(150, -50, 0); // Velocity of the target plane

    public Transform target;

    void Start()
    {
        Vector3 hitPoint = CalculateHitPoint();
        Debug.Log("Hit Point: " + hitPoint);
    }

    public void Update()
    {
        plane1Velocity = plane1.forward * plane1v;
        plane2Velocity = plane2.forward * plane2v;
        Vector3 hitPoint = CalculateHitPoint();
        target.position = hitPoint;
    }

    Vector3 CalculateHitPoint()
    {
        Vector3 P1_0 = plane1.position;
        Vector3 P2_0 = plane2.position;

        Vector3 V1 = plane1Velocity;
        Vector3 V2 = plane2Velocity;

        Vector3 R_0 = P2_0 - P1_0;
        Vector3 V_r = V2 - V1;

        float a = Vector3.Dot(V_r, V_r) - bulletSpeed * bulletSpeed;
        float b = 2 * Vector3.Dot(R_0, V_r);
        float c = Vector3.Dot(R_0, R_0);

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            Debug.LogError("No solution exists for the given parameters.");
            return Vector3.zero;
        }

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float t1 = (-b + sqrtDiscriminant) / (2 * a);
        float t2 = (-b - sqrtDiscriminant) / (2 * a);

        float t = Mathf.Max(t1, t2);

        if (t < 0)
        {
            Debug.LogError("The intercept time is negative. Planes will not meet in the future.");
            return Vector3.zero;
        }

        Vector3 hitPoint = P2_0 + V2 * t;

        return hitPoint;
    }
}