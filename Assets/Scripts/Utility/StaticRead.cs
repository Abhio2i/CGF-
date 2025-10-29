using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRead : MonoBehaviour
{
    public int value = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        value = StaticTest.frame;
    }
}
