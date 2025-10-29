using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Enemy.waypoint
{
    public class WayPointmanager : MonoBehaviour
    {

        
        #region variables and inputs
        [SerializeField] Camera _camera;
        [SerializeField] GameObject pin;
        public TMP_InputField altitudeInput;
        [SerializeField] TMP_InputField latitudeInput;
        [SerializeField] TMP_InputField longitudeInput;
        [SerializeField] MapLatLong mapData;
        [HideInInspector] public bool add;
        [HideInInspector] public bool remove;
        public bool edit;
        [HideInInspector] public int count;
        [HideInInspector] public int totalWaypointsPresent;
        
        [SerializeField]  Toggle isClosedCircuit;

        public Color myColor = Color.red;
        public List<GameObject> myWaypoints;

        private LineRenderer lineRenderer;
        private Vector3[] waypoint;
        private float lat;
        private float lon;
        #endregion
        private void Start()
        {
            mapData = GetComponent<MapLatLong>();
            //myColor = pin.GetComponent<Renderer>().sharedMaterial.color;
            myWaypoints = new List<GameObject>();
            lineRenderer=GetComponent<LineRenderer>();
        }
        private void Update()
        {
            lat=_camera.ScreenToWorldPoint(Input.mousePosition).z;
            lon=_camera.ScreenToWorldPoint(Input.mousePosition).x;
            Vector2 data = mapData.CalculateLatLong(lat, lon);
            longitudeInput.text= data.y.ToString();
            latitudeInput.text= data.x.ToString();
            totalWaypointsPresent = myWaypoints.Count;
           
            if(myWaypoints!=null)
            //if(myWaypoints.Count > 0)
            {
                //draw waypoints on map
                waypoint = new Vector3[myWaypoints.Count];
                for (int i=0;i<myWaypoints.Count;i++)
                {
                    waypoint[i]=myWaypoints[i].transform.position;
                }
                DrawLines(waypoint, 30f, 30f);
            }

           

            if (count == 1 && add)
            {
                AddingWaypointsFunction();
            }
            if (count == 1 && remove)
            {
                RemovingWaypointsFunctions();
            }
            if (count == 1 && edit)
            {
                EditingWaypointsFunction();
            }
        }
        #region ADD Waypoints
        public void AddWaypoint() //used to set the bool and update the count whenever the "add" button is pressed
        {
            add = true;
            count++;
        }
        //add waypoint
        void AddingWaypointsFunction()
        {
            AddWaypoint waypoint = gameObject.AddComponent<AddWaypoint>();
            waypoint._camera = _camera;
            waypoint.pin = pin;
            waypoint.altitude = altitudeInput;
            waypoint.count = totalWaypointsPresent;
            add = false;
            waypoint.enabled = true;
        }
        #endregion
        //remove waypoint
        #region REMOVE Waypoints
        public void RemoveWaypoints()
        {
            remove = true;
            count++;
        }
        void RemovingWaypointsFunctions()
        {
            RemoveWaypoint waypoint = gameObject.AddComponent<RemoveWaypoint>();
            waypoint.camera = _camera;
            waypoint.enabled = true;
            remove = false;

        }
        #endregion
        //edit waypoint
        #region EDIT Waypoints
        public void EditWaypoints()
        {
            edit = true;
            count++;
        }
        void EditingWaypointsFunction()
        {
            EditWaypoint waypoint = gameObject.AddComponent<EditWaypoint>();
            waypoint.altitude = altitudeInput;
            waypoint._camera = _camera;
            waypoint.enabled = true;
            edit = false;
        }
        #endregion

        //reset
        public void Reset()
        {
            count = 0;
            altitudeInput.text = "";
            Destroy(gameObject.GetComponent<AddWaypoint>());
            Destroy(gameObject.GetComponent<RemoveWaypoint>());
            Destroy(gameObject.GetComponent<EditWaypoint>());

            if (myWaypoints.Count > 0)
            {
                foreach (GameObject point in myWaypoints)
                {
                    point.GetComponent<Renderer>().material.color = myColor;
                }
            }
        }

     
        //draw lines
        private void DrawLines(UnityEngine.Vector3[] vertexPositions, float startWidth, float endWidth)
        {
            //lineRenderer.startWidth = startWidth;
            //lineRenderer.endWidth = endWidth;
            lineRenderer.loop = isClosedCircuit.isOn;
            lineRenderer.positionCount = vertexPositions.Length;
            lineRenderer.SetPositions(vertexPositions);
        }
        
        public void SaveWaypoints()
        {
            Save_Waypoints.points = myWaypoints;
            Save_Waypoints.isLoop = isClosedCircuit.isOn;
            Save_Waypoints.SetData();
        }
    }
}