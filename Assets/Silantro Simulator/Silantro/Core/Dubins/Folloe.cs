using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oyedoyin.Navigation;
using Oyedoyin;

public class Folloe : MonoBehaviour
{
    public int currentWayPointIndex = 1;
    public SilantroPath generatedPath;


    public bool isCircular;
    public float angle;
    public TestPath path;
    public float heading;

    public void Update()
    {
        //generatedPath = path.dataPath;


        if (generatedPath != null && generatedPath.pathVectorPoints.Count > 1)
        {
            Vector3 wp2 = GetWaypointPos(currentWayPointIndex);
            Vector3 wp1 = GetWaypointPos(currentWayPointIndex - 1);

            Vector3 wpVec = (wp2 - wp1).normalized;

            //The angle between the front vector of the vehicle and the vector between the waypoints
            angle = Vector3.Angle(this.transform.forward, wpVec);

            Vector3 frontAxle = transform.position;

            if (CalculateProgress(frontAxle, wp1, wp2) > 1f)
            {
                currentWayPointIndex += 1;

                //Clamp when we have reached the last waypoint so we start all over again
                if (currentWayPointIndex > generatedPath.pathVectorPoints.Count - 1 && isCircular)
                {
                    currentWayPointIndex = 0;
                }
                //The path in not circular so we should stop following it
                else if (currentWayPointIndex > generatedPath.pathVectorPoints.Count - 1)
                {
                    //wayPoints = null;

                    //Stop the car when we have reached the end of the path
                    //carScript.StopCar();
                }

                //Debug.Log(currentWayPointIndex);
            }
        }
    }



    public Vector3 GetWaypointPos(int index)
    {
        int waypointIndex = MathBase.ClampListIndex(index, generatedPath.pathVectorPoints.Count);
        Vector3 waypointPos = generatedPath.pathVectorPoints[waypointIndex];
        return waypointPos;
    }



    float CalculateProgress(Vector3 carPos, Vector3 wp_from, Vector3 wp_to)
    {
        float Rx = carPos.x - wp_from.x;
        float Rz = carPos.z - wp_from.z;

        float deltaX = wp_to.x - wp_from.x;
        float deltaZ = wp_to.z - wp_from.z;

        //If progress is > 1 then the car has passed the waypoint
        float progress = ((Rx * deltaX) + (Rz * deltaZ)) / ((deltaX * deltaX) + (deltaZ * deltaZ));

        //Debug.Log(progress);

        return progress;
    }
}
