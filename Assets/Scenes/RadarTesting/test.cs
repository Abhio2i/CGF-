using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vazgriz.Plane;

public class test : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var d = cam.WorldToViewportPoint(target.position);
        Debug.Log(d);
    }
}
