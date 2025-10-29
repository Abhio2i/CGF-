using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaypointTracker : MonoBehaviour
{
    [SerializeField] string waypointTag;

    [SerializeField] public Vector3[] waypoints;
    [SerializeField] Vector3 currentVector;
    [SerializeField] Camera _camera;
    [SerializeField] Canvas canvas;
    [SerializeField] RawImage box;
    [SerializeField] RawImage arrow;
    [SerializeField] GameObject player;
    [SerializeField] GameObject _base;

    //[SerializeField] int desiredIndex;
    [SerializeField] int waypointIndex;
    private List<bool> visitedWaypoints;
    private void Start()
    {
        visitedWaypoints=new List<bool>();
        PlayerPrefs.SetInt("waypointDist", 0);
        PlayerPrefs.SetInt("waypointRemaining", 0);
        PlayerPrefs.SetInt("baseDist", 0); 


    }

    private void FixedUpdate()
    {
        if (waypointIndex < 0 || waypointIndex > waypoints.Length - 1)
            return;
        if (waypoints != null && waypoints.Length > 0)
        {

            PlayerPrefs.SetInt("waypointDist", (int)Vector3.Distance(player.transform.position, waypoints[waypointIndex]));
            PlayerPrefs.SetInt("waypointRemaining",waypoints.Length-visitedWaypoints.Count);
            PlayerPrefs.SetInt("baseDist", (int)Vector3.Distance(player.transform.position, _base.transform.position));

            if (Vector3.Distance(player.transform.position, waypoints[waypointIndex]) < 50f)
            {
                waypointIndex++;
                visitedWaypoints.Add(true);
            }
            if (waypointIndex >= waypoints.Length && Save_Waypoints.isLoop)
                waypointIndex = 0;
            if (waypointIndex >= waypoints.Length && !Save_Waypoints.isLoop)
            {
                //print("last destination");
                return;
            }
            Vector3 pos = _camera.WorldToScreenPoint(waypoints[waypointIndex]);
            currentVector = waypoints[waypointIndex];
            float x = _camera.pixelWidth;
            float y = _camera.pixelHeight;

            pos.z = Mathf.Clamp(pos.z, -1f, 1f);
            pos.x = Mathf.Clamp(pos.x, 0, x);
            pos.y = Mathf.Clamp(pos.y, 0, y);
            //print(pos + " " + x + " " + y);

           
            if (pos.x >= x)
            {
                arrow.gameObject.SetActive(true);
                box.gameObject.SetActive(false);
                arrow.transform.position = pos - new Vector3(10f, 0, 0);
                Vector3 angle = new Vector3(0, 0, 0);
                arrow.transform.rotation = Quaternion.Euler(angle);
                //print("a");
                return;
            }
            else if (pos.x <= 0)
            {
                arrow.gameObject.SetActive(true);
                box.gameObject.SetActive(false);
                arrow.transform.position = pos - new Vector3(-10f, 0, 0);
                Vector3 angle = new Vector3(0, 180f, 0);
                arrow.transform.rotation = Quaternion.Euler(angle);
                //print("b");
                return;
            }
            else if (pos.y >= y)
            {
                arrow.gameObject.SetActive(true);
                box.gameObject.SetActive(false);
                arrow.transform.position = pos - new Vector3(0, 10f, 0);
                Vector3 angle = new Vector3(0, 0f, 90f);
                arrow.transform.rotation = Quaternion.Euler(angle);
                //print("c");
                return;
            }
            else if (pos.y <= 0)
            {
                arrow.gameObject.SetActive(true);
                box.gameObject.SetActive(false);
                arrow.transform.position = pos - new Vector3(0, -10f, 0);
                Vector3 angle = new Vector3(0, 0, -90f);
                arrow.transform.rotation = Quaternion.Euler(angle);
                //print("d");
                return;
            }
            else
            {
                arrow.gameObject.SetActive(false);
                
                    box.gameObject.SetActive(isAhead(currentVector));
                
                box.transform.position = pos;
            }
        }
    }
    bool isAhead(Vector3 target)
    {
        Vector3 directionToTarget = player.transform.position - target;
        float angle = Vector3.Angle(player.transform.forward, directionToTarget);
        if (Mathf.Abs(angle) < 90)
        {
            return true;
        }
        else
            return false;
    }
}