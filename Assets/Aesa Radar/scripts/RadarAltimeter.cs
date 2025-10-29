using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarAltimeter : MonoBehaviour
{
    public Transform plane;
    public Transform needle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(plane!= null && needle != null)
        {


            needle.rotation = Quaternion.RotateTowards(needle.rotation, Quaternion.Euler(0, 0, -(plane.position.y / 1000) * 270f), 50f * Time.deltaTime);
        }
    }
}
