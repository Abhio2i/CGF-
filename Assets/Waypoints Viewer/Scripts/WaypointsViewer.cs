using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
//using static UnityEditor.PlayerSettings;

public class WaypointsViewer : MonoBehaviour
{
    public FeedBackRecorderAndPlayer feed;
    public MissionPlan missionPlan;
    public Utility.LatLonAlt.LatLong latLong;
    public WaypointCircuit circuit;
    public RectTransform MapUI;
    public Camera waypointsCamera;
    public int Size = 20000;
    public int Sensitivity = 2;
    public int min = 1000;
    public int max = 100000;
    public bool Zoom = false;
    public Transform CameraTransform;
    public Transform MainPlane;
    public Specification specification;
    public Vector3 Offset = new Vector3(0, 6000, 0);
    [Header("Waypoints")]
    public List<Vector3> LatlongWaypoints = new List<Vector3>();
    public List<Vector3> Waypoints = new List<Vector3>();
    public List<Transform> WaypointsUi = new List<Transform>();
    public List<RectTransform> WaypointsDirectionUI = new List<RectTransform>();

    [Header("Allys")]
    public List<Transform> Allys = new List<Transform>();
    public List<Transform> AllysUi = new List<Transform>();

    [Header("Adversary")]
    public List<Transform> Adversary = new List<Transform>();
    public List<Transform> AdversaryUi = new List<Transform>();

    [Header("Sams")]
    public List<Transform> Sams = new List<Transform>();
    public List<Transform> SamsUi = new List<Transform>();
    public List<RectTransform> SamsRangeUI = new List<RectTransform>();
    public List<RectTransform> SamsDistanceUI = new List<RectTransform>();

    [Header("Neutral")]
    public List<Transform> Neutrals = new List<Transform>();
    public List<Transform> NeutralUi = new List<Transform>();

    [Header("Warship")]
    public List<Transform> Warships = new List<Transform>();
    public List<Transform> WarshipUi = new List<Transform>();

    [Header("Missile")]
    public List<Transform> Missiles = new List<Transform>();
    public List<Transform> MissilesUi = new List<Transform>();

    [Header("Prefabs")]
    public GameObject AllyPrefab;
    public GameObject AdversaryPrefab;
    public GameObject NeutralPrefab;
    public GameObject SamsPrefab;
    public GameObject WarshipPrefab;
    public GameObject WaypointPrefab;
    public GameObject ArrowPrefab;
    public GameObject MissilePrefab;

    [Header("Waypoint Info")]
    public float WD;
    public float WB;
    public int TOA;
    public int currentWaypoint = 0;
    public TextMeshProUGUI WaypintNumber;
    public TextMeshProUGUI WaypintDistance;
    public TextMeshProUGUI ArrrivalTime;
    public TextMeshProUGUI WaypointBearing;

    // Start is called before the first frame update
    void Start()
    {
        feed = GameObject.FindObjectOfType<FeedBackRecorderAndPlayer>();
        int i = 0;
        for ( i = circuit.transform.childCount-1; i>-1;i--)
        {
            Destroy(circuit.transform.GetChild(i).gameObject);
        }

        i = 0;
        foreach (Cordinates cordinate in missionPlan.waypoints)
        {
            Vector3 cord = cordinate.position;
            Vector2 latlon = latLong.worldToLatLong(new Vector2(cord.x, cord.z));
            LatlongWaypoints.Add(new Vector3(latlon.x, latlon.y,cord.y));
            Waypoints.Add(cord);
            GameObject waypoint = Instantiate(WaypointPrefab);
            waypoint.transform.SetParent(MapUI);
            waypoint.transform.localScale = Vector3.one;
            waypoint.SetActive(true);
            waypoint.transform.localPosition = worldToMap(cord);
            waypoint.transform.GetChild(1).GetComponent<Text>().text = i.ToString();
            WaypointsUi.Add(waypoint.transform);
            WaypointsDirectionUI.Add(waypoint.transform.GetChild(0).GetComponent<RectTransform>());

            if (i > 0)
            {
                GameObject obj = new GameObject("MyEmptyObject" + (missionPlan.waypoints.Count-i));
                obj.transform.SetParent(circuit.transform);
                obj.transform.localPosition = new Vector3( cord.x, 1000, cord.z);
                //obj.transform.SetAsFirstSibling();
            }
            i++;

        }
        
        circuit.ManualUpdate();

        if (Waypoints.Count > 2)
        {
            Vector3 end = Waypoints[1];
            Waypoints.RemoveAt(1);
            Waypoints.Add(end);
        }
        Invoke("InitMissilePoints", 2f);
    }

    public void InitMissilePoints()
    {
        foreach (Transform m in FeedBackRecorderAndPlayer.Missiles)
        {
            Missiles.Add(m);
        }
        //Allys
        foreach (Transform mi in Missiles)
        {
            Vector3 cord = mi.position;
            GameObject missileIcon = Instantiate(MissilePrefab);
            missileIcon.transform.SetParent(MapUI);
            missileIcon.transform.localScale = Vector3.one;
            missileIcon.SetActive(true);
            missileIcon.transform.localPosition = worldToMap(cord);
            MissilesUi.Add(missileIcon.transform);

        }
    }


        public void InitPlanesPoints()
    {
        //Allys
        foreach (Transform ally in Allys)
        {
            Vector3 cord = ally.position;
            GameObject AllyIcon = Instantiate(AllyPrefab);
            AllyIcon.transform.SetParent(MapUI);
            AllyIcon.transform.localScale = Vector3.one;
            AllyIcon.SetActive(true);
            AllyIcon.transform.GetChild(0).GetComponent<Text>().text = Allys.IndexOf(ally).ToString();
            AllyIcon.transform.localPosition = worldToMap(cord);
            AllysUi.Add(AllyIcon.transform);
            
        }

        //Adversary
        foreach (Transform adversary in Adversary)
        {
            Vector3 cord = adversary.position;
            GameObject adversaryIcon = Instantiate(AdversaryPrefab);
            adversaryIcon.transform.SetParent(MapUI);
            adversaryIcon.transform.localScale = Vector3.one;
            adversaryIcon.SetActive(true);
            adversaryIcon.transform.GetChild(0).GetComponent<Text>().text = Adversary.IndexOf(adversary).ToString();
            adversaryIcon.transform.localPosition = worldToMap(cord);
            AdversaryUi.Add(adversaryIcon.transform);

        }

        //Sams
        foreach (Transform Sam in Sams)
        {
            Vector3 cord = Sam.position;
            GameObject SamIcon = Instantiate(SamsPrefab);
            SamIcon.transform.SetParent(MapUI);
            SamIcon.transform.localScale = Vector3.one;
            SamIcon.SetActive(true);
            SamIcon.transform.localPosition = worldToMap(cord);
            SamIcon.transform.GetChild(2).GetComponent<Text>().text = Sams.IndexOf(Sam).ToString();
            SamsUi.Add(SamIcon.transform);
            SamsRangeUI.Add(SamIcon.transform.GetChild(0).GetComponent<RectTransform>());
            SamsDistanceUI.Add(SamIcon.transform.GetChild(1).GetComponent<RectTransform>());
            SamIcon.transform.GetChild(1).GetComponent<RectTransform>().SetParent(MapUI);

        }

        //Neutral
        foreach (Transform Neutral in Neutrals)
        {
            Vector3 cord = Neutral.position;
            GameObject NeutralIcon = Instantiate(NeutralPrefab);
            NeutralIcon.transform.SetParent(MapUI);
            NeutralIcon.transform.localScale = Vector3.one;
            NeutralIcon.SetActive(true);
            NeutralIcon.transform.GetChild(0).GetComponent<Text>().text = Neutrals.IndexOf(Neutral).ToString();
            NeutralIcon.transform.localPosition = worldToMap(cord);
            NeutralUi.Add(NeutralIcon.transform);

        }

        //Warship
        foreach (Transform Warship in Warships)
        {
            Vector3 cord = Warship.position;
            GameObject WarshipIcon = Instantiate(WarshipPrefab);
            WarshipIcon.transform.SetParent(MapUI);
            WarshipIcon.transform.localScale = Vector3.one;
            WarshipIcon.SetActive(true);
            WarshipIcon.transform.GetChild(0).GetComponent<Text>().text = Warships.IndexOf(Warship).ToString();
            WarshipIcon.transform.localPosition = worldToMap(cord);
            WarshipUi.Add(WarshipIcon.transform);

        }
    }

    public Vector3 worldToMap(Vector3 worldPosition)
    {
        Vector3 cord =  waypointsCamera.WorldToViewportPoint(worldPosition);
        cord.z = 0;
        cord.x = (cord.x * MapUI.sizeDelta.x) - (MapUI.sizeDelta.x / 2);
        cord.y = (cord.y * MapUI.sizeDelta.y) - (MapUI.sizeDelta.y / 2);
        return cord;
    }

    public void IsZoom(bool isZoom)
    {
        Zoom = isZoom;
        Debug.Log("Zoom Allow " + isZoom);
    }
    // Update is called once per frame
    void Update()
    {

        //Waypoint Info
        if (Waypoints.Count > 0)
        {
            Vector3 p1 = Waypoints[currentWaypoint];
            Vector3 p2 = MainPlane.position;
            p1.y = p2.y = 0;

            float Distance = Vector3.Distance(p1, p2)/ 1852f;
            // Calculate the direction from p1 to p2
            Vector3 direction = p1 - p2;

            // Project the direction and the MainPlane forward direction onto the horizontal plane
            Vector3 directionHorizontal = new Vector3(direction.x, 0, direction.z);
            Vector3 planeForwardHorizontal = new Vector3(MainPlane.forward.x, 0, MainPlane.forward.z);

            // Normalize the vectors to avoid issues with non-unit vectors
            directionHorizontal.Normalize();
            planeForwardHorizontal.Normalize();

            // Calculate the angle between the projected vectors
            float bearing = Vector3.Angle(directionHorizontal, planeForwardHorizontal);

            // Optionally, determine the sign of the angle
            float sign = Mathf.Sign(Vector3.Cross(directionHorizontal, planeForwardHorizontal).y);
            bearing *= sign;
            // Convert the bearing to a 0-360 range
            if (bearing < 0)
            {
                bearing += 360;
            }

            if (Distance * 1852 <1000)
            {
                WaypointsUi[currentWaypoint].GetChild(2).gameObject.SetActive(false);
                currentWaypoint++;
                currentWaypoint = currentWaypoint >= Waypoints.Count ? 0 : currentWaypoint;
                WaypointsUi[currentWaypoint].GetChild(2).gameObject.SetActive(true);
            }

            WB = bearing;
            WaypintDistance.text = Distance.ToString("0")+"NM";
            WaypintNumber.text = "W "+currentWaypoint;
            WaypointBearing.text = bearing.ToString("0")+ "°";
            // Calculate the time in hours
            double timeInHours = Distance / specification.entityInfo.IndicatedSpeed;

            // Convert the time to minutes
            double timeInMinutes = timeInHours * 60;

            // Get the whole number of minutes
            int minutes = (int)timeInMinutes;

            // Calculate the remaining seconds
            double fractionalMinutes = timeInMinutes - minutes;
            int seconds = (int)(fractionalMinutes * 60);
            WD = Distance;
            TOA = (int)timeInHours;
            if (!FeedBackRecorderAndPlayer.isPlaying)
            {
                ArrrivalTime.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }
            else
            {
                ArrrivalTime.text = "";
            }

        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(scrollInput != 0 && Zoom)
        {
            Size -= (int)(scrollInput * Sensitivity);
            Size = Mathf.Clamp(Size, min, max);
            waypointsCamera.orthographicSize = Size;
        }
        //follow Main Plane
        Vector3 pos = MainPlane.position;
        pos.y = 0;
        pos += Offset;
        CameraTransform.position = pos;
        CameraTransform.localEulerAngles = new Vector3(90, MainPlane.eulerAngles.y, 0);
        


        int i = 0;
        //Update Position
        foreach(Transform t in WaypointsUi)
        {
            t.localPosition = worldToMap(Waypoints[i]);
            i++;
        }

        i = 0;
        //Update direction
        foreach (Transform t in WaypointsUi)
        {
            if (i < Waypoints.Count - 1)
            {
                Transform nxt = WaypointsUi[i + 1];
                Vector3 direction = nxt.localPosition - t.localPosition;
                float angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
                RectTransform dir = WaypointsDirectionUI[i];
                dir.localEulerAngles = new Vector3(0, 0, angle);
                dir.sizeDelta = new Vector2(dir.sizeDelta.x, direction.magnitude);
            }
            i++;
        }

        //allys
        i = 0;
        foreach(Transform allyIcon in AllysUi)
        {
            if (Allys[i] != null&& Allys[i].gameObject.activeSelf)
            {
                allyIcon.gameObject.SetActive(true);
                allyIcon.localPosition = worldToMap(Allys[i].position);
                allyIcon.localEulerAngles = new Vector3(0, 0, MainPlane.eulerAngles.y - Allys[i].eulerAngles.y);
            }
            else
            {
                allyIcon.gameObject.SetActive(false);
            }
            i++;
        }

        //adversary
        i = 0;
        foreach (Transform adversaryIcon in AdversaryUi)
        {
            if (Adversary[i] != null && Adversary[i].gameObject.activeSelf)
            {
                adversaryIcon.gameObject.SetActive(true);
                adversaryIcon.localPosition = worldToMap(Adversary[i].position);
                adversaryIcon.localEulerAngles = new Vector3(0, 0, MainPlane.eulerAngles.y - Adversary[i].eulerAngles.y);
            }
            else
            {
                adversaryIcon.gameObject.SetActive(false);
            }
            i++;
        }

        //Sams
        i = 0;
        foreach (Transform SamIcon in SamsUi)
        {
            if (Sams[i] != null && Sams[i].gameObject.activeSelf)
            {
                SamIcon.gameObject.SetActive(true);
                SamIcon.localPosition = worldToMap(Sams[i].position);
                
                SamsDistanceUI[i].localPosition = worldToMap(Sams[i].position+new Vector3(3000,0,0));
                float distace = Vector3.Distance(SamIcon.localPosition, SamsDistanceUI[i].localPosition)*2f;
                SamsRangeUI[i].sizeDelta = new Vector2 (distace, distace);
                //SamIcon.localEulerAngles = new Vector3(0, 0, MainPlane.eulerAngles.y - Sams[i].eulerAngles.y);
            }
            else
            {
                SamIcon.gameObject.SetActive(false);
            }
            i++;
        }

        //Warship
        i = 0;
        foreach (Transform WarshipIcon in WarshipUi)
        {
            if (Warships[i] != null && Warships[i].gameObject.activeSelf)
            {
                WarshipIcon.gameObject.SetActive(true);
                WarshipIcon.localPosition = worldToMap(Warships[i].position);
                //SamIcon.localEulerAngles = new Vector3(0, 0, MainPlane.eulerAngles.y - Sams[i].eulerAngles.y);
            }
            else
            {
                WarshipIcon.gameObject.SetActive(false);
            }
            i++;
        }

        //Neutral
        i = 0;
        foreach (Transform NeutralIcon in NeutralUi)
        {
            if (Neutrals[i] != null && Neutrals[i].gameObject.activeSelf)
            {
                NeutralIcon.gameObject.SetActive(true);
                NeutralIcon.localPosition = worldToMap(Neutrals[i].position);
                NeutralIcon.localEulerAngles = new Vector3(0, 0, MainPlane.eulerAngles.y - Neutrals[i].eulerAngles.y);
            }
            else
            {
                NeutralIcon.gameObject.SetActive(false);
            }
            i++;
        }


        //Missile
        i = 0;
        foreach (Transform missileIcon in MissilesUi)
        {
            if (Missiles[i] != null && Missiles[i].gameObject.activeSelf)
            {
                missileIcon.gameObject.SetActive(true);
                missileIcon.localPosition = worldToMap(Missiles[i].position);
                missileIcon.localEulerAngles = new Vector3(0, 0, MainPlane.eulerAngles.y - Missiles[i].eulerAngles.y);
            }
            else
            {
                missileIcon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
