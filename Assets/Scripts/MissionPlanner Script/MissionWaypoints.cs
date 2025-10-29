using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;

public class MissionWaypoints : MonoBehaviour
{
    public Vector2 UpperLeftLatLong = new Vector2(15.712f,78.7203f);
    public Vector2 LowerRightLatLong = new Vector2(13.8425f, 80.6539f);
    public Vector2 CenterLatLong = new Vector2(14.7793f, 79.6871f);
    public Vector2 UpperLeftPos = new Vector2(-104065f, 104065f);
    public Vector2 LowerRightPos = new Vector2(104065f, -104065f);
    public Vector2 size = new Vector2(208130f, 208130f);

    public TextMeshProUGUI CordinateUI;
    public RectTransform MapUi;
    public RectTransform ScrollViewUI;
    public Camera MapCamera;
    public RectTransform startWaypoint;
    public RectTransform EndWaypoint;
    public RectTransform startWaypointUI;
    public RectTransform EndWaypointUI;
    public GameObject WaypointPrefabs;
    public GameObject WaypointUIPrefab;
    public List<Transform> Waypoints = new List<Transform>();
    public List<Transform> WaypointsUI = new List<Transform>();

    public bool UpdateUI= false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        // Convert the screen coordinates to local coordinates of the image
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MapUi, mousePosition, null, out Vector2 localMousePosition);
        localMousePosition.x = (localMousePosition.x + 500)/1000;
        localMousePosition.y = (localMousePosition.y + 500)/1000;
        Vector3 WorldPosition = MapCamera.ViewportToWorldPoint(localMousePosition);
        Vector2 latlong = worldToLatLong(new Vector2(WorldPosition.x, WorldPosition.z));
        CordinateUI.text = "Mouse X :" + localMousePosition.x.ToString("0.0000") + " ,Mouse Y :" + localMousePosition.y.ToString("0.0000") + " ," +
                               "Lat Distance :" + WorldPosition.x.ToString("000000") + " ,Long Distance :" + WorldPosition.z.ToString("000000")+" ," +
                               "Lat :" + latlong.x.ToString("0.00000") + " ,Long :" + latlong.y.ToString("0.00000");
        if (UpdateUI)
            UpdateWaypoints();
    }


    public Vector2 worldToLatLong( Vector2 world) 
    {
        float Latdis = UpperLeftLatLong.x-LowerRightLatLong.x;
        float Longdis = UpperLeftLatLong.y-LowerRightLatLong.y;
        float worldlatdis = UpperLeftPos.x -LowerRightPos.x;
        float worldLongdis = UpperLeftPos.y -LowerRightPos.y;
        float xdis = UpperLeftPos.x - world.x;
        float ydis = UpperLeftPos.y - world.y;
        float xpercent = xdis / worldlatdis;
        float ypercent = ydis / worldLongdis;
        return new Vector2(UpperLeftLatLong.x - (ypercent*Latdis), UpperLeftLatLong.y- (xpercent*Longdis));
    }

    public Vector2 LatLongToWorld(Vector2 latLong)
    {
        float Latdis = UpperLeftLatLong.x-LowerRightLatLong.x;
        float Longdis = UpperLeftLatLong.y-LowerRightLatLong.y;
        float worldlatdis = UpperLeftPos.x - LowerRightPos.x;
        float worldLongdis = UpperLeftPos.y - LowerRightPos.y;
        float xdis = UpperLeftLatLong.y - latLong.y;
        float ydis = UpperLeftLatLong.x - latLong.x;
        float xpercent = xdis / Longdis;
        float ypercent = ydis / Latdis;
        return new Vector2(UpperLeftPos.x - (xpercent * worldlatdis), UpperLeftPos.y - (ypercent * worldLongdis));
    }


    public Vector3 MapToWordCords(Camera MapCamera, RectTransform UI, RectTransform Target, float Width, float Height)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UI, Target.position, null, out Vector2 localMousePosition);
        //cord = s.TransformDirection(t.localPosition);
        //cord = new Vector3(localMousePosition.x, localMousePosition.y, 0);
        localMousePosition.x = (localMousePosition.x + (Width / 2)) / Width;
        localMousePosition.y = (localMousePosition.y + (Height / 2)) / Height;
        return MapCamera.ViewportToWorldPoint(localMousePosition);
    }
    public Vector3 MapToWordCords2(Camera MapCamera, RectTransform UI, Transform Target, float Width, float Height)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UI, Target.position, null, out Vector2 localMousePosition);
        //cord = s.TransformDirection(t.localPosition);
        //cord = new Vector3(localMousePosition.x, localMousePosition.y, 0);
        localMousePosition.x = (localMousePosition.x + (Width / 2)) / Width;
        localMousePosition.y = (localMousePosition.y + (Height / 2)) / Height;
        return MapCamera.ViewportToWorldPoint(localMousePosition);
    }

    public Vector3 MapToWordCords3(Camera MapCamera, RectTransform UI, Vector3 Target, float Width, float Height)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UI, Target, null, out Vector2 localMousePosition);
        //cord = s.TransformDirection(t.localPosition);
        //cord = new Vector3(localMousePosition.x, localMousePosition.y, 0);
        localMousePosition.x = (localMousePosition.x + (Width / 2)) / Width;
        localMousePosition.y = (localMousePosition.y + (Height / 2)) / Height;
        return MapCamera.ViewportToWorldPoint(localMousePosition);
    }

    public Vector3 WordCordsToMap(Camera MapCamera,Vector3 worlds,float Width, float Height)
    {
        Vector3 viewport = MapCamera.WorldToViewportPoint(worlds);
        //cord = s.TransformDirection(t.localPosition);
        //cord = new Vector3(localMousePosition.x, localMousePosition.y, 0);
        float x = (viewport.x*Width)-(Width/2f);
        float y = (viewport.y * Height) - (Height / 2f);
        return new Vector3(x,y,0);
    }


    public void CreateWaypoint()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            Vector2 mousePosition = Input.mousePosition;
            CreateWaypointFinal(mousePosition);
        }
    }

    public void CreateWaypointFinal(Vector2 position,bool local = false)
    {

         Vector2 mousePosition = position;
         if (Waypoints.Count == 0)
         {
             EndWaypoint.gameObject.SetActive(true);
             if(local)
                EndWaypoint.localPosition = mousePosition;
             else
             EndWaypoint.position = mousePosition;
             Waypoints.Add(EndWaypoint.transform);
             EndWaypointUI.gameObject.SetActive(true);
             WaypointsUI.Add(EndWaypointUI.transform);
         }
         else
         if (Waypoints.Count > 0)
         {
             GameObject obj = Instantiate(WaypointPrefabs);
             obj.transform.position = EndWaypoint.position;
             obj.transform.SetParent(MapUi);
             obj.SetActive(true);
             obj.transform.localScale = Vector3.one;
             Waypoints.Add(obj.transform);
             if (local)
                EndWaypoint.localPosition = mousePosition;
             else
                EndWaypoint.position = mousePosition;

             GameObject objUI = Instantiate(WaypointUIPrefab);
             objUI.transform.SetParent(startWaypointUI.parent);
             objUI.SetActive(true);
             objUI.transform.localScale = Vector3.one;
             WaypointsUI.Add(objUI.transform);

             EndWaypointUI.SetAsLastSibling();

         }
         UpdateWaypoints();
         UpdateScrolleViewHeight();

    }

    public void UpdateUIToggle(bool i)
    {
        UpdateUI = i;
    }
    public void UpdateScrolleViewHeight()
    {
        //Debug.Log(EndWaypointUI.localPosition.y);
        ScrollViewUI.sizeDelta = new Vector2(ScrollViewUI.sizeDelta.x,-EndWaypointUI.localPosition.y+(EndWaypointUI.sizeDelta.y/2)+150);
    }

    public void UpdateWaypoints()
    {
        Vector3 worlds = MapToWordCords(MapCamera, MapUi, startWaypoint.GetComponent<RectTransform>(), 1000, 1000);
        Vector2 latlongs = worldToLatLong(new Vector2(worlds.x, worlds.z));
        //Cordinate
        //startWaypoint.GetChild(1).GetChild(0).GetComponent<Text>().text = startWaypoint.localPosition.x + "," + startWaypoint.localPosition.y;
        startWaypoint.GetChild(1).GetChild(0).GetComponent<Text>().text = latlongs.x + "," + latlongs.y;

        //Latitude
        //startWaypointUI.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = startWaypoint.localPosition.x.ToString();
        startWaypointUI.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = latlongs.x.ToString();
        //Longitude
        //startWaypointUI.GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text = startWaypoint.localPosition.y.ToString();
        startWaypointUI.GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text = latlongs.y.ToString();

        //Planes

        if (Waypoints.Count > 0)
        {
            Vector3 Pos = Vector3.zero;
            Pos = Waypoints[0].localPosition;
            if(Waypoints.Count > 1)
            {
                Pos = Waypoints[1].localPosition;
            }
            Vector3 dirction = Pos - startWaypoint.localPosition;
            float ang = Vector3.SignedAngle(Vector3.up, dirction, Vector3.forward);
            RectTransform Plane = startWaypoint.GetChild(3).GetComponent<RectTransform>();
            Plane.localEulerAngles = new Vector3(0, 0, ang);
        }
        


        int i = 0;
        foreach (Transform waypoint in Waypoints)
        {
            if (i == 0)
            {
                Vector3 world = MapToWordCords(MapCamera, MapUi, waypoint.GetComponent<RectTransform>(), 1000, 1000);
                Vector2 latlong = worldToLatLong(new Vector2(world.x, world.z));
                //Number
                EndWaypoint.GetChild(0).GetComponent<Text>().text = (Waypoints.Count+1).ToString();
                //Cordinate
                //EndWaypoint.GetChild(1).GetChild(0).GetComponent<Text>().text = EndWaypoint.localPosition.x + "," + EndWaypoint.localPosition.y;
                EndWaypoint.GetChild(1).GetChild(0).GetComponent<Text>().text = latlong.x + "," + latlong.y;

                Vector3 pos = startWaypoint.localPosition;
                if(Waypoints.Count > 1)
                {
                    pos = Waypoints[Waypoints.Count-1].localPosition;
                }
                Vector3 direction = pos - waypoint.localPosition ;
                float angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
                RectTransform dir = waypoint.GetChild(2).GetComponent<RectTransform>();
                dir.localEulerAngles = new Vector3(0, 0, angle);
                dir.sizeDelta = new Vector2(dir.sizeDelta.x, direction.magnitude);

                RectTransform Planes = waypoint.GetChild(4).GetComponent<RectTransform>();
                Planes.localEulerAngles = new Vector3(0, 0, angle);
                //Latitude
                //EndWaypointUI.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = EndWaypoint.localPosition.x.ToString();
                EndWaypointUI.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = latlong.x.ToString();
                //Longitude
                //EndWaypointUI.GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text = EndWaypoint.localPosition.y.ToString();
                EndWaypointUI.GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text = latlong.y.ToString();

            }
            else
           if (i > 0)
            {

                Vector3 world =  MapToWordCords(MapCamera, MapUi, waypoint.GetComponent<RectTransform>(), 1000, 1000);
                Vector2 latlong = worldToLatLong(new Vector2(world.x,world.z));
                //Number
                waypoint.GetChild(0).GetComponent<Text>().text = (i+1).ToString();
                //Cordinate
                //waypoint.GetChild(1).GetChild(0).GetComponent<Text>().text = waypoint.localPosition.x + "," + waypoint.localPosition.y;
                waypoint.GetChild(1).GetChild(0).GetComponent<Text>().text = latlong.x + "," + latlong.y;


                //Direction
                Vector3 pos = Waypoints[i - 1].localPosition;
                if (i == 1)
                {
                    pos =startWaypoint.localPosition;
                }
                Vector3 direction = pos - waypoint.localPosition;
                float angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
                RectTransform dir = waypoint.GetChild(2).GetComponent<RectTransform>();
                dir.localEulerAngles = new Vector3 (0, 0, angle);
                dir.sizeDelta = new Vector2(dir.sizeDelta.x, direction.magnitude);
                //UI
                //Number
                WaypointsUI[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = "Waypoint " + (i+1);

                //Latitude
                //WaypointsUI[i].GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = waypoint.localPosition.x.ToString();
                WaypointsUI[i].GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = latlong.x.ToString();
                //Longitude
                //WaypointsUI[i].GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text = waypoint.localPosition.y.ToString();
                WaypointsUI[i].GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text = latlong.y.ToString();

            }
            i++;
        }
    }

    public void RemoveAllWaypoints()
    {
        while(Waypoints.Count > 1)
        {
            Destroy(Waypoints[1].gameObject);
            Waypoints.RemoveAt(1);
        }
        EndWaypoint.gameObject.SetActive(false);
        Waypoints.Clear();

        while (WaypointsUI.Count > 1)
        {
            Destroy(WaypointsUI[1].gameObject);
            WaypointsUI.RemoveAt(1);
        }
        EndWaypointUI.gameObject.SetActive(false);
        WaypointsUI.Clear();
    }

    public void RemoveWaypointByUI(Transform WaypointUI)
    {
        int i = 0;
        foreach (Transform waypoint in WaypointsUI)
        {
            if (waypoint == WaypointUI)
            {
                RemoveWaypointFinal(Waypoints[i].gameObject);
                break;
            }
            i++;
        }
    }
    public void UpdateWaypointByUI(Transform WaypointUI)
    {
        if(WaypointUI == startWaypointUI.transform)
        {
            Vector3 pos = startWaypoint.localPosition;
            //Latitude
            pos.x = float.Parse(WaypointUI.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text);
            //Longitude
            pos.y = float.Parse(WaypointUI.GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text);
            Vector2 world = LatLongToWorld(new Vector2(pos.x, pos.y));
            Vector3 viewport = WordCordsToMap(MapCamera, new Vector3(world.x, 0, world.y), 1000, 1000);

            startWaypoint.localPosition = viewport;
        }
        int i = 0;  
        foreach (Transform waypoint in WaypointsUI)
        {
            if (waypoint == WaypointUI)
            {
                
                Vector3 pos = Waypoints[i].localPosition;
                //Latitude
                pos.x = float.Parse(WaypointUI.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text);
                //Longitude
                pos.y = float.Parse(WaypointUI.GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<InputField>().text);
                Vector2 world = LatLongToWorld(new Vector2(pos.x,pos.y));
                Vector3 viewport =  WordCordsToMap(MapCamera,new Vector3(world.x,0,world.y),1000,1000);
                Waypoints[i].localPosition = viewport;

                break;
            }
            i++;
        }
        UpdateWaypoints();
    }

    public void RemoveWaypoint(GameObject obj)
    {
        if (Input.GetKey(KeyCode.LeftControl ) && Input.GetKey (KeyCode.LeftShift))
        {
            RemoveWaypointFinal(obj);
        }
    }


    public void RemoveWaypointFinal(GameObject obj)
    {

            int i = 0;
            foreach (Transform t in Waypoints)
            {
                if (t.gameObject == obj && t.gameObject == EndWaypoint.gameObject && Waypoints.Count == 1)
                {
                    Waypoints.Remove(t);
                    EndWaypoint.gameObject.SetActive(false);
                    //UI
                    WaypointsUI.Remove(EndWaypointUI);
                    EndWaypointUI.gameObject.SetActive(false);

                    UpdateWaypoints();
                    UpdateScrolleViewHeight();
                    i++;
                    break;
                }
                else
                if (t.gameObject == obj && t.gameObject == EndWaypoint.gameObject)
                {
                    int index = Waypoints.Count - 1;
                    EndWaypoint.position = Waypoints[index].position;
                    Transform o = Waypoints[index];
                    Waypoints.Remove(o);
                    Destroy(o.gameObject);
                    //UI
                    o = WaypointsUI[index];
                    WaypointsUI.Remove(o);
                    Destroy(o.gameObject);

                    UpdateWaypoints();
                    UpdateScrolleViewHeight();
                    i++;
                    break;
                }
                if (t.gameObject == obj)
                {
                    Waypoints.Remove(t);
                    Destroy(obj.gameObject);
                    //UI
                    Transform o = WaypointsUI[i];
                    WaypointsUI.Remove(o);
                    Destroy(o.gameObject);

                    UpdateWaypoints();
                    UpdateScrolleViewHeight();
                    i++;
                    break;
                }
                i++;
            }
    }
}
