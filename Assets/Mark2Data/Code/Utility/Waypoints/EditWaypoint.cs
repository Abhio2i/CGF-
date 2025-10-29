using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.waypoint;
using TMPro;
using UnityEngine.InputSystem;

public class EditWaypoint : MonoBehaviour
{
    private int myAltitude;
    private InputController controller;

    [HideInInspector] public TMP_InputField altitude;
    [HideInInspector] public Camera _camera;
    [HideInInspector] public GameObject selectedObject;

    private void Awake()
    {
        controller = new InputController();
        controller.CommonActions.MouseClick.performed += ctx => selectObject();
    }
    void selectObject()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("waypoints"))
            {
                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<Renderer>().sharedMaterial.color = Color.cyan;
            }
        }
    }
    void Edit()
    {
        if (selectedObject != null)
        {

            if (altitude.text == "")
            {
                myAltitude = (int)selectedObject.transform.position.y;
            }
            else
            {
                myAltitude = int.Parse(altitude.text);
            }
            selectedObject.transform.position = new UnityEngine.Vector3(selectedObject.transform.position.x, myAltitude, selectedObject.transform.position.z);
        }
    }
    private void Update()
    {
        Edit();
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
