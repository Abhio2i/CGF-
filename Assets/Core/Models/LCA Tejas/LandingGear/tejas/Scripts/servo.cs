using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class servo : MonoBehaviour
{
    
    public float MinAngle = 5f;
    public float MaxAngle = -15f;
    [Range(0f, -20f)]
    public float value = 0f;
    public float rate = 1f;
    public Vector3 direction = Vector3.zero;
    public Transform Axcel;
    private float current = 0f;

    public void set(float v)
    {
        //Axcel.localEulerAngles = new Vector3(v, 0f, 0f);
        value = v;
    }

    public void _set(float v)
    {
        Axcel.localEulerAngles = direction*v;
    }

    private void FixedUpdate()
    {
        current = Mathf.Lerp(current, value, Time.fixedDeltaTime * rate);
        Axcel.localEulerAngles = direction*current;
    }

    private void OnValidate()
    {
        _set(value);
    }


}
