using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Enemy.waypoint;
using UnityEngine.InputSystem;

public class AddWaypoint : MonoBehaviour
{
    private InputController controller;
    private float speed;
    private int setAltitude;

    [HideInInspector] public Camera _camera;
    [HideInInspector] public TMP_InputField altitude;
    // [HideInInspector] 
    [HideInInspector] public int count;
    [HideInInspector] public GameObject pin;
    [HideInInspector] public List<GameObject> points;
    [HideInInspector] public bool isClicked;
    private GameObject waypins;
    WayPointmanager waypointmanager;

    private void Start()
    {
        waypointmanager = GetComponent<WayPointmanager>();
    }
    private void Awake()
    {
        controller = new InputController();
        points = new List<GameObject>();
        controller.CommonActions.MouseClick.performed += ctx => SetUpWayPoints();
    }
    private void Update()
    {

    }
    private void OnEnable()
    {
        controller.Enable();
    }
    private void OnDisable()
    {
        controller.Disable();
    }
    void SetUpWayPoints()
    {
        //adding waypoints
        if (!IsMyMouseOverUI())
        {
            if (altitude.text == "")
            {
                setAltitude = 600;
            }
            else
            {
                setAltitude = int.Parse(altitude.text);
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    waypins = Instantiate(pin, hit.point, Quaternion.identity);
                    int temp;
                    if(!int.TryParse(waypointmanager.altitudeInput.text,out temp)) { temp = (int) waypins.transform.position.y + 100; }
                    waypins.transform.position = new UnityEngine.Vector3(waypins.transform.position.x, temp, waypins.transform.position.z);
                    waypins.transform.localScale = new UnityEngine.Vector3(50, 50, 50);
                }
            }

            waypointmanager.myWaypoints.Add(waypins);
            count++;
            gameObject.GetComponent<WayPointmanager>().add = false;
            gameObject.GetComponent<WayPointmanager>().count = 0;
            isClicked = true;

        }
    }
    bool IsMyMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
