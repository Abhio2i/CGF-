using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.waypoint;
using UnityEngine.InputSystem;

public class RemoveWaypoint : MonoBehaviour
{
    private GameObject selectedObject;
    private InputController controller;
    private WayPointmanager waypointmanager;

    [HideInInspector] public new Camera camera;
    void Start()
    {
        waypointmanager = gameObject.GetComponent<WayPointmanager>();
    }
    private void Awake()
    {
        controller = new InputController();
        controller.CommonActions.MouseClick.performed += ctx => selectObject();
    }

    // Update is called once per frame
    void Update()
    {
        RemoveSelectedWaypoint();
    }
    void selectObject()
    {
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("waypoints"))
            {
                selectedObject = hit.collider.gameObject;
            }
        }
    }
    void RemoveSelectedWaypoint()
    {   //remove waypoints

        if (selectedObject != null)
        {
            Destroy(selectedObject);
            waypointmanager.myWaypoints.Remove(selectedObject);
        }
    }
    private void OnEnable()
    {
        controller.Enable();
    }
    private void OnDisable()
    {
        controller.Disable();
    }
}
