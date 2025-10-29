using UnityEngine;
using Oyedoyin;
using Oyedoyin.Navigation;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestPath : MonoBehaviour
{
    

    public Transform point1;
    public Transform point2;
    public Transform point3;
    public Transform point4;
    public Transform point5;
    public Transform point6;
    public Transform point7;
    public Transform point8;
 
    public float turnRadiusA = 10f, turnRadiusB = 1000, turnRadiusC = 1000, turnRadiusD = 1000;
    public float stepDistance = 0.02f;
    public List<Vector3> pathPoints;
    public float totalDistance;

    private void OnDrawGizmos()
    {
        pathPoints = new List<Vector3>();


        Gizmos.DrawSphere(point1.position, 100);
        Gizmos.DrawSphere(point2.position, 100);
        Gizmos.DrawSphere(point3.position, 100);
        Gizmos.DrawSphere(point4.position, 100);
        Gizmos.DrawSphere(point5.position, 100);
        Gizmos.DrawSphere(point6.position, 100);
        Gizmos.DrawSphere(point7.position, 100);
        Gizmos.DrawSphere(point8.position, 100);
        

        pathPoints = pathPoints.Concat(PathGenerator.GenerateWaypointConnection(point1, point2, point3, turnRadiusA, stepDistance)).ToList();
        pathPoints = pathPoints.Concat(PathGenerator.GenerateWaypointConnection(point3, point4, point5, turnRadiusB, stepDistance)).ToList();
        pathPoints = pathPoints.Concat(PathGenerator.GenerateWaypointConnection(point5, point6, point7, turnRadiusC, stepDistance)).ToList();
        pathPoints = pathPoints.Concat(PathGenerator.GenerateWaypointConnection(point7, point8, point1, turnRadiusD, stepDistance)).ToList();

        Vector3 headPoint = pathPoints[0];
        //Handles.color = Color.green; Handles.ArrowHandleCap(0, headPoint, transform.rotation * Quaternion.LookRotation(Vector3.up), 30, EventType.Repaint);

        totalDistance = 0f;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            if (pathPoints[i] != null)
            {
                Vector3 newPoint = pathPoints[i];
                Vector3 lastPoint = pathPoints[i + 1];
                Gizmos.DrawLine(newPoint, lastPoint);
                Gizmos.DrawSphere(lastPoint, .5f);
                float currentDistance = Vector3.Distance(newPoint, lastPoint);
                totalDistance += currentDistance;
            }
        }
    }
}
