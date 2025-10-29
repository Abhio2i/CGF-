using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle : MonoBehaviour
{
    public Transform Target;
    public Transform dummy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dir = Target.position-transform.position;
        var ok = transform.InverseTransformDirection(Target.localEulerAngles);
        dummy.localEulerAngles = ok;
    }
}
