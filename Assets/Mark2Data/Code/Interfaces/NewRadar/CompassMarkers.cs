using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Enemy.waypoint;
//Navigational Display
public class CompassMarkers : MonoBehaviour
{
    [SerializeField] GameObject numericMarkers;
    [SerializeField] GameObject parentObject;
    [SerializeField] GameObject lineRendererObject;
    [SerializeField] GameObject test;


    [SerializeField] Image waypointMarker;
    [SerializeField] Camera cam;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform player;

    [SerializeField] RectTransform planeBeacon;
    [SerializeField] RectTransform canvas;
    [SerializeField] RectTransform HeadingIndicator;

    [SerializeField] Load_Waypoints load_Waypoints;

    RectTransform []textFields;
    List<GameObject> waypointIndicators=new List<GameObject>();
    List<RectTransform> position=new List<RectTransform> ();
    Vector3[] indPositions;
    float checkRadius,tempRadius;
    GameObject[] waypoints;


    private void Start()
    {
        textFields = new RectTransform[numericMarkers.transform.childCount];
        for (int i=0;i<numericMarkers.transform.childCount;i++)
        {
            textFields[i] = numericMarkers.transform.GetChild(i).GetComponent<RectTransform>();
        }
        Invoke(nameof(WaypointMarkers),20f);
        checkRadius=CheckInRangeWaypoint(checkRadius, HeadingIndicator);
    }
    void WaypointMarkers()//create waypoint marker
    {
        int index = 1;
        waypoints = GameObject.FindGameObjectsWithTag("waypoints");
        indPositions=new Vector3 [waypoints.Length+1];
        indPositions[0] = player.transform.position;
        foreach (GameObject vector3 in waypoints)
        {
            var markers = Instantiate(waypointMarker.gameObject);
            waypointIndicators.Add(markers.gameObject);
            position.Add(markers.GetComponent<RectTransform>());
            indPositions[index] = vector3.transform.position;
            index++;
            WorldTOCanvas(vector3.transform.position, markers.GetComponent<RectTransform>());
            markers.transform.SetParent(parentObject.transform, false);
        }
    }
    void ShowOnlyInRangeWaypoints() //to show in range waypoints only
    {
        foreach(GameObject pair in waypointIndicators)
        {
            tempRadius = CheckInRangeWaypoint(tempRadius, pair.GetComponent<RectTransform>());
            if (tempRadius <= checkRadius)
                pair.SetActive(true);
            else
                pair.SetActive(false);
        }
    }
    float CheckInRangeWaypoint(float mainRadius,RectTransform position1) //calculate dist of the waypoint from aircraft
    {
        Vector3 distBetween=position1.position-planeBeacon.position;
        mainRadius=Vector3.Magnitude(distBetween);
        return mainRadius;
    }
    void RotateMarkers() //rotation of markers wrt player
    {
        float z=player.transform.rotation.eulerAngles.y;
        numericMarkers.GetComponent<RectTransform>().localRotation=Quaternion.Euler(new Vector3(0,0,z));
        foreach(RectTransform transform in textFields)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, z));
        }
        Vector3 angles = cam.transform.rotation.eulerAngles;
        cam.transform.rotation = Quaternion.Euler(new Vector3(angles.x, z, angles.z));
        parentObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, z));
    }
    void WorldTOCanvas(Vector3 WorldObject,RectTransform UIElement)
    {
        Vector2 ViewportPosition = cam.WorldToViewportPoint(WorldObject);
        Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f)));

        UIElement.anchoredPosition = WorldObject_ScreenPosition;
    }

    void DrawPathToWaypoint()
    {
        load_Waypoints.LineRenderer(lineRenderer, indPositions.Length, 0, false, indPositions,0.1f);
    }
    private void FixedUpdate()
    {
        if ( waypoints==null) return;
        /*for(int i=0;i<waypoints.Length;i++)
        {
               WorldTOCanvas(waypoints[i].transform.position,position[i]);
        }*/
        RotateMarkers();
        ShowOnlyInRangeWaypoints();
       // DrawPathToWaypoint();
    }
    
}
