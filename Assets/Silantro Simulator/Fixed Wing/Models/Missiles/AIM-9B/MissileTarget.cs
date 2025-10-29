using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTarget : MonoBehaviour
{
    [SerializeField]HEAT heatSource;

    [SerializeField] float speed = 400f;

    public void SetTarget(HEAT source)
    {
        heatSource = source;
    }

    private void FixedUpdate()
    {
        if (heatSource != null)
        {
            Vector3 targetDirection = heatSource.transform.position - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.position += transform.forward * speed * Time.fixedDeltaTime;
        }
    }
}
