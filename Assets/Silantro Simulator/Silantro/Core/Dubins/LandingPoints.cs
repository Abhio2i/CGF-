using UnityEngine;
using Oyedoyin;
using Oyedoyin.Navigation;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LandingPoints : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public Transform point4;

    [Range(0, 1)] public float ApproachBreakLevel;
    [Range(0, 1)] public float LevelBreakLevel;
    [Range(0, 1)] public float DownwindLevel;
    [Range(0, 1)] public float BaseLegLevel;
    [Range(0, 1)] public float FinalLevel;

    public int ApproachBreakPoint;
    public int LevelBreakPoint;
    public int DownwindPoint;
    public int BaseLegPoint;
    public int FinalPoint;


    public Vector3 ApproachBreakVector;
    public Vector3 LevelBreakVector;
    public Vector3 DownwindVector;
    public Vector3 BaseLegVector;
    public Vector3 FinalVector;

    public float approachAltitude = 1500f;
    public float levelAltitude = 1000f;
    public float downwindAltitude = 600f;
    public float baseAltitude = 400f;
    public float finalAltitude = 400f;

    public List<Vector3> ABPoints;
    public List<Vector3> LBPoints;
    public List<Vector3> DWPoints;
    public List<Vector3> BLPoints;
    public List<Vector3> FNPoints;


    public float entryTurnRadius = 3500f;
    public float exitTurnRadius = 3500f;
    public float stepDistance = 50f;
    public List<Vector3> pathPoints;
    public List<Vector3> basePoints;
    public float totalDistance;


    public void DrawVectorPoint(Color color, Vector3 point, float pointerHeight, float pointRadius)
    {
        //Handles.color = color;
        Gizmos.color = color;
        //Handles.ArrowHandleCap(0, point, transform.rotation * Quaternion.LookRotation(Vector3.up), pointerHeight, EventType.Repaint); Gizmos.DrawSphere(point, pointRadius);
        Gizmos.color = Color.white;
    }






    private void OnDrawGizmos ()
    {
        pathPoints = new List<Vector3>();
        basePoints = new List<Vector3>();

        ABPoints = new List<Vector3>();
        DWPoints = new List<Vector3>();
        BLPoints = new List<Vector3>();
        FNPoints = new List<Vector3>();
        LBPoints = new List<Vector3>();

        basePoints = basePoints.Concat(PathGenerator.GetPath(point1.position, point1.eulerAngles.y * Mathf.Deg2Rad, point2.position, point2.eulerAngles.y * Mathf.Deg2Rad, entryTurnRadius, stepDistance).pathVectorPoints).ToList();
        basePoints = basePoints.Concat(PathGenerator.GetPath(point2.position, point2.eulerAngles.y * Mathf.Deg2Rad, point3.position, point3.eulerAngles.y * Mathf.Deg2Rad, exitTurnRadius, stepDistance).pathVectorPoints).ToList();
        basePoints = basePoints.Distinct().ToList();

        ApproachBreakPoint = (int)(ApproachBreakLevel * (basePoints.Count - 1));
        LevelBreakPoint = (int)(LevelBreakLevel * (basePoints.Count - 1));
        DownwindPoint = (int)(DownwindLevel * (basePoints.Count - 1));
        BaseLegPoint = (int)(BaseLegLevel * (basePoints.Count - 1));
        FinalPoint = (int)(FinalLevel * (basePoints.Count - 1));

        for (int a = 0; a < ApproachBreakPoint; a++) { ABPoints.Add(basePoints[a]); }
        for (int a = ApproachBreakPoint; a < LevelBreakPoint; a++) { LBPoints.Add(basePoints[a]); }
        for (int a = LevelBreakPoint; a < DownwindPoint; a++) { DWPoints.Add(basePoints[a]); }
        for (int a = DownwindPoint; a < BaseLegPoint; a++) { BLPoints.Add(basePoints[a]); }
        for (int a = BaseLegPoint; a < FinalPoint+1; a++) { FNPoints.Add(basePoints[a]); }

        MathBase.ConfigurePathHeight(ABPoints, transform.position.y * MathBase.toFt, approachAltitude);
        MathBase.ConfigurePathHeight(LBPoints, approachAltitude, levelAltitude);
        MathBase.ConfigurePathHeight(DWPoints, levelAltitude, downwindAltitude);
        MathBase.ConfigurePathHeight(BLPoints, downwindAltitude, baseAltitude);
        MathBase.ConfigurePathHeight(FNPoints, baseAltitude, finalAltitude);



        pathPoints = pathPoints.Concat(ABPoints).ToList();
        pathPoints = pathPoints.Concat(LBPoints).ToList();
        pathPoints = pathPoints.Concat(DWPoints).ToList();
        pathPoints = pathPoints.Concat(BLPoints).ToList();
        pathPoints = pathPoints.Concat(FNPoints).ToList();
        pathPoints = pathPoints.Distinct().ToList();


        ApproachBreakVector = pathPoints[ApproachBreakPoint];
        LevelBreakVector = pathPoints[LevelBreakPoint];
        DownwindVector = pathPoints[DownwindPoint];
        BaseLegVector = pathPoints[BaseLegPoint];
        FinalVector = pathPoints[FinalPoint];


        DrawVectorPoint(new Color(0, 1, 0), ApproachBreakVector, 50, 50);
        DrawVectorPoint(new Color(0.25f, 0.75f, 0), LevelBreakVector, 50, 50);
        DrawVectorPoint(new Color(0.50f, 0.50f, 0), DownwindVector, 50, 50);
        DrawVectorPoint(new Color(0.75f, 0.25f, 0), BaseLegVector, 50, 50);
        DrawVectorPoint(new Color(1, 0, 0), FinalVector, 50, 50);




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
