using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Oyedoyin;
using Oyedoyin.Navigation;


//Display the final Dubins Paths
public class DubinsDebug : MonoBehaviour
{
    //Everything we need to add in the editor
    //Circles
    public Transform goalCircleLeft;
    public Transform goalCircleRight;
    public Transform startCircleLeft;
    public Transform startCircleRight;
    //Line renderers
    public LineRenderer lineRSR;
    public LineRenderer lineLSL;
    public LineRenderer lineRSL;
    public LineRenderer lineLSR;
    public LineRenderer lineRLR;
    public LineRenderer lineLRL;
    //The cars we generate paths to/from
    public Transform startCar;
    public Transform goalCar;

    //Objects
     public SilantroPath dataPath;
    public float turnRadius = 10f;
    public float stepDistance = 0.02f;


    void Update()
    {
        //To generate paths we need the position and rotation (heading) of the cars
        Vector3 basePosition = startCar.position;
        Vector3 goalPos = goalCar.position;
        //Heading is in radians
        float startHeading = startCar.eulerAngles.y * Mathf.Deg2Rad;
        float goalHeading = goalCar.eulerAngles.y * Mathf.Deg2Rad;

        dataPath = PathGenerator.GetPath(basePosition, startHeading, goalPos, goalHeading, turnRadius, stepDistance); 
        DisplayPath(dataPath, lineRLR);
    }






    //Display a path with a line renderer
    void DisplayPath(SilantroPath pathData, LineRenderer lineRenderer)
    {
        //Activate the line renderer
        lineRenderer.gameObject.SetActive(true);

        //The coordinates of the path
        List<Vector3> pathCoordinates = pathData.pathVectorPoints;

        //Display the final line
        lineRenderer.SetVertexCount(pathCoordinates.Count);

        for (int i = 0; i < pathCoordinates.Count; i++)
        {
            lineRenderer.SetPosition(i, pathCoordinates[i]);
        }
    }


    //Deactivate all line renderers in case a circle is not possible
    //Then we dont want to show the old circle
    void DeactivateLineRenderers()
    {
        lineLRL.gameObject.SetActive(false);
        lineRLR.gameObject.SetActive(false);
        lineLSL.gameObject.SetActive(false);
        lineRSR.gameObject.SetActive(false);
        lineLSR.gameObject.SetActive(false);
        lineRSL.gameObject.SetActive(false);
    }
}
