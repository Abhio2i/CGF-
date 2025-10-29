using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOfRadar : MonoBehaviour
{
    public Transform Radar;
    public Vector3 RadarRotation;
    public Vector3 PlaneRotation;

    private void Update()
    {
        RadarRotation = Radar.eulerAngles;
        PlaneRotation = transform.eulerAngles;
    }
}
