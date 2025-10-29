using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.waypoint;
public static class Save_Waypoints 
{
    public static List<UnityEngine.Vector3> waypoints = new List<UnityEngine.Vector3>();
    public static List<GameObject> points;
    public static bool isLoop;
    public static void SetData()
    {
        //save waypoints
        
        waypoints.Clear();
        foreach(GameObject go in points)
        {
            waypoints.Add(go.transform.position);
        }
        Debug.Log("saved "+points.Count+ " "+ waypoints.Count);
    }
}

