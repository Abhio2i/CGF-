using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera camera;
    

    public float panSpeed = 160f;
    


    private float targetZoom;
    private Vector3 camPos;
    private float scrollData;
    private float zoomFactor;
    
    
    
    InputController controller;

    private void Awake()
    {
        targetZoom = camera.orthographicSize;
        camPos=camera.transform.position;

        zoomFactor = 0.1f;

        controller = new InputController();


        //controlls the camera movement

        controller.Camera.MoveRight.performed+= Context => camPos.x+=panSpeed*Time.deltaTime*100f;
        controller.Camera.MoveLeft.performed+= Context => camPos.x-=panSpeed*Time.deltaTime*100f;

        controller.Camera.MoveUp.performed += Context => camPos.z += panSpeed * Time.deltaTime*100f;
        controller.Camera.MoveDown.performed += Context => camPos.z -= panSpeed * Time.deltaTime*100f;

        //zoom in and zoom out
        controller.Camera.ZoomInOut.performed += Context => scrollData = Context.ReadValue<float>();
        controller.Camera.ZoomInOut.canceled += Context => scrollData = 0;

    }
    void Start()
    {
        
    }
    //zoom in zoom out function
    void Scroll()
    {
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 30f, 300f); //30 is the min zoom_in size and 300 is the max zoomOut size 
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, 0.4f);
    }
    void Move()
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position, camPos, 0.025f);
    }
    void Update()
    {
        Move();//to pan the map
        Scroll();//use to zoom in and zoom out
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
