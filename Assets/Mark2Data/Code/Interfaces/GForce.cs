using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GForce : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] GameObject player;
    Rigidbody rb;
    float timeStep;
    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        timeStep = 2f;
    }
    private void FixedUpdate()
    {
        if (rb == null) return;
        // Angular velocity is in radians per second.
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector3 localAngularVel = transform.InverseTransformDirection(rb.angularVelocity);

        // Local pitch velocity (X) is positive when pitching down.

        // Radius of turn = velocity / angular velocity
        float radius = (Mathf.Approximately(localAngularVel.x, 0.0f)) ? float.MaxValue : localVelocity.z / localAngularVel.x;

        // The radius of the turn will be negative when in a pitching down turn.

        // Force is mass * radius * angular velocity^2
        float verticalForce = (Mathf.Approximately(radius, 0.0f)) ? 0.0f : (localVelocity.z * localVelocity.z) / radius;

        // Express in G (Always relative to Earth G)
        float verticalG = verticalForce / -9.81f;

        // Add the planet's gravity in. When the up is facing directly up, then the full
        // force of gravity will be felt in the vertical.
        verticalG += transform.up.y * (Physics.gravity.y / -9.81f);

        force = Mathf.Round(verticalG);
        PlayerPrefs.SetFloat("GForce",force);
    }
}
