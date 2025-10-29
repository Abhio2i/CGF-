using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cordinates
{
    public float Latitude;
    public float Longitude;
    public Vector3 position;

    public Cordinates(Cordinates data = null)
    {
        if (data != null)
        {
            // Copy Position data
            Latitude = data.Latitude;
            Longitude = data.Longitude;
            position = data.position;
        }
    }
}
