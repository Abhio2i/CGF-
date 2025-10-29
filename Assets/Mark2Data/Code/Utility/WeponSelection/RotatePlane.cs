using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlane : MonoBehaviour
{
    public GameObject F16;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        F16.transform.Rotate(speed * Time.deltaTime * Vector3.up);
    }
}
