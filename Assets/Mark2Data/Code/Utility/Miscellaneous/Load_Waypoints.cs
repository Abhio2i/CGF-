using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_Waypoints : MonoBehaviour
{
    public List<UnityEngine.Vector3> waypoints;
    public bool isLineloop;
    public GameObject points;

    private LineRenderer lineRenderer;
    [SerializeField] GameObject gameManager;
    UnityEngine.Vector3[] myWaypoints;
    int count = 0;
    void Awake()
    {
        waypoints=Save_Waypoints.waypoints;
        isLineloop = Save_Waypoints.isLoop;
        lineRenderer=GetComponent<LineRenderer>();
        myWaypoints = new UnityEngine.Vector3[waypoints.Count];
        
        for(int i=0;i<waypoints.Count;i++)
        {
            myWaypoints[i]=waypoints[i];
            float _posy = myWaypoints[i].y;
            if(_posy < 0)
            {
                myWaypoints[i].y = 200;
            }
            Instantiate(points, myWaypoints[i], Quaternion.identity);
            count++;
        }
        gameManager.GetComponent<HUD_2>().waypointList = myWaypoints;
        //draw lines
        LineRenderer(lineRenderer,count,0,isLineloop,myWaypoints,30f);
        if (waypoints.Count > 0)
        {
            gameManager.GetComponent<WaypointTracker>().enabled = true;
            gameManager.GetComponent<WaypointTracker>().waypoints = myWaypoints;
        }
    }
    
    public void LineRenderer(LineRenderer lineRenderer,int count,int temp,bool loop,Vector3[] waypointPosition,float width)
    {
        lineRenderer.positionCount = (int)width;
        lineRenderer.startWidth = 30f;
        if (loop)
        {
            lineRenderer.loop = true;
        }
        while (temp < count)
        {
            SetLineData(temp, waypointPosition[temp],lineRenderer);
            temp++;
        }
    }
    private void SetLineData(int _count, UnityEngine.Vector3 _pos,LineRenderer lineRenderer)
    {
        lineRenderer.SetPosition(_count,_pos);
    }
    
}
