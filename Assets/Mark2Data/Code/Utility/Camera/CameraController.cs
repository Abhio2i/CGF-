using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy.camera.CameraController
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        public LineRenderer line;
        public bool waypoints;
        public Vector3 AllyPos, EnemyPos, WaypoinPos;
        public float orthosize = 3000, waysize = 21360,enemysize= 11000;

        public int panSpeed = 160;
        public int maxZoom = 8000;
        public int minZoom = 500;
        public int zoomSpeed = 100;

        private float targetZoom;
        private UnityEngine.Vector3 camPos;
        private float scrollData;
        private float zoomFactor;

        InputController controller;

        private void Awake()
        {
            if(_camera)
                targetZoom = _camera.orthographicSize;
            if (_camera)
                camPos = _camera.transform.position;

            zoomFactor = 0.1f;

            controller = new InputController();
            //controlls the camera movement

            controller.Camera.MoveRight.performed += Context => camPos.x += panSpeed * Time.deltaTime * 100f;
            controller.Camera.MoveLeft.performed += Context => camPos.x -= panSpeed * Time.deltaTime * 100f;

            controller.Camera.MoveUp.performed += Context => camPos.z += panSpeed * Time.deltaTime * 100f;
            controller.Camera.MoveDown.performed += Context => camPos.z -= panSpeed * Time.deltaTime * 100f;

            //zoom in and zoom out
            controller.Camera.ZoomInOut.performed += Context => scrollData = Context.ReadValue<float>();
            controller.Camera.ZoomInOut.canceled += Context => scrollData = 0;

        }

        //zoom in zoom out function
        void Scroll()
        {
            targetZoom -= scrollData * zoomFactor;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom); //30 is the min zoom_in size and 300 is the max zoomOut size 
            //camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, 0.4f);
            if (_camera)
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, zoomSpeed);
            if (line != null)
                line.widthMultiplier = (int)_camera.orthographicSize / 100;
        }
        void Move()
        {
            if(_camera)
            _camera.transform.position = UnityEngine.Vector3.Lerp(_camera.transform.position, camPos, 0.025f);
        }
        void Update()
        {
            Move();//to pan the map
            Scroll();//use to zoom in and zoom out
            //SwitchPos();
        }
        private void OnEnable()
        {
            controller.Enable();
        }

        private void OnDisable()
        {
            controller.Disable();
        }
        public void SwitchPos(int i)
        {
            if (i == 5)
            {
                camPos = WaypoinPos;
                targetZoom = waysize;
            }
            else if (i == 0)
            {
                camPos = AllyPos;
                targetZoom = orthosize;
            }
            else if (i == 1 || i==2)
            {
                camPos = EnemyPos;
                targetZoom = enemysize;
            }
        }
    }
}