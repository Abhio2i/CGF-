using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using newSaveSystem;
using System;

namespace Utility.Cameras
{

    public class CameraControler : MonoBehaviour
    {
        //Note:- Front GameObjet original pos.y = 1.8f;
        PlayerControle cameraSwitch;
        public GameObject TopDownCamera;
        public GameObject TargetF;
        public GameObject camTransformF;
        public GameObject TargetR;
        public GameObject camTransformR;
        public GameObject TargetL;
        public GameObject camTransformL;
        public UnityEngine.Vector3 Offset = new UnityEngine.Vector3(0, 0, 0);
        public float SmoothTime = 0.3f;
        public bool isfrontCam = false;
        public Vector3 frontOffset;
        public Vector3 cockpitOffset = new Vector3(0, 1, 18.2f);
        public Vector2 Axis = Vector2.zero;
        public bool NoseControl = true;
        public Vector3 Rotation = Vector3.zero;
        public CinemachineVirtualCamera front;
        public CinemachineVirtualCamera sideLeft;
        public CinemachineVirtualCamera sideRight;

        public GameObject cam1_VideoCapture;
        public GameObject cam2_VideoCapture;
        public GameObject cam3_VideoCapture;
        public GameObject cam4_VideoCapture;

        [SerializeField] GameObject _hudCanvas;
        [SerializeField] Transform Nose;

        public int count = 1;
        private CinemachineBrain frontCam;

        private void OnEnable()
        {
            cameraSwitch.Enable();
           // cam1_VideoCapture.SetActive(false);
        }
        private void OnDisable()
        {
            cameraSwitch.Disable();
        }
        int x;
        private void Awake()
        {
            cameraSwitch = new PlayerControle();
            //x = PlayerPrefs.GetInt("cockpit");
            x = SavingSystem.cockpitView;
           
            //print(x);
        }
        private void Start()
        {
            if (x!=1)
            {
                TargetF.transform.localPosition = frontOffset;
            }
            cameraSwitch.Camera.Switch.started += camera => SwitchCamera();
            cameraSwitch.Camera.TopDown.started += camera => TopDown();
            cameraSwitch.Camera.CockpitView.started += camera => CockpitView();
            cameraSwitch.Camera.NoseDown.performed += (ctx) => {

                Axis.y = ctx.ReadValue<float>();
            };
            cameraSwitch.Camera.NoseDown.canceled += (ctx) => {

                Axis.y = ctx.ReadValue<float>();
            };
            cameraSwitch.Camera.NoseRight.performed += (ctx) => {

                Axis.x = ctx.ReadValue<float>();
            };
            cameraSwitch.Camera.NoseRight.canceled += (ctx) => {

                Axis.x = ctx.ReadValue<float>();
            };

            cameraSwitch.Camera.NoseRight1.performed += (ctx) => {

                Axis.x = ctx.ReadValue<float>();
            };
            cameraSwitch.Camera.NoseDown1.performed += (ctx) => {

                Axis.y = ctx.ReadValue<float>();
            };

            cameraSwitch.Camera.NoseRight1.canceled += (ctx) => {

                Axis.x = ctx.ReadValue<float>();
            };
            cameraSwitch.Camera.NoseDown1.canceled += (ctx) => {

                Axis.y = ctx.ReadValue<float>();
            };
        }

        private void FixedUpdate()
        {

            //Rotation = Vector3.Lerp(Rotation, new Vector3(-Axis.y * 55, Axis.x * 60, 0), Time.fixedDeltaTime * 3f);
            Rotation = Vector3.Lerp(Rotation, new Vector3(-Axis.y * 60, Axis.x * 160, 0), Time.fixedDeltaTime * 3f);
            TargetF.transform.localEulerAngles = Rotation;
            Nose.localEulerAngles = new Vector3(-Rotation.x,-Rotation.y,0);
        }

        void CockpitView()
        {
            if(TargetF.transform.localPosition == frontOffset)
            {
                TargetF.transform.localPosition = cockpitOffset;
            }
            else { TargetF.transform.localPosition = frontOffset; }
        }

        private void LateUpdate()
        {
            #region frontFollow
            if (!TargetF||!TargetL||TargetR||camTransformF||camTransformL||camTransformR) return;
            // update position
            UnityEngine.Vector3 targetPositionf = TargetF.transform.position + Offset;
            camTransformF.transform.position = targetPositionf;

            // update rotation
            camTransformF.transform.LookAt(TargetF.transform);
            camTransformF.transform.eulerAngles = TargetF.transform.eulerAngles;
            #endregion
            #region RightFollow
            // update position
            UnityEngine.Vector3 targetPositionR = TargetR.transform.position + Offset;
            camTransformR.transform.position = targetPositionR;

            // update rotation
            camTransformR.transform.LookAt(TargetR.transform);
            camTransformR.transform.eulerAngles = TargetR.transform.eulerAngles;
            #endregion
            #region LeftFollow
            // update position
            UnityEngine.Vector3 targetPositionL = TargetL.transform.position + Offset;
            camTransformL.transform.position = targetPositionL;

            // update rotation
            camTransformL.transform.LookAt(TargetL.transform);
            camTransformL.transform.eulerAngles = TargetL.transform.eulerAngles;
            #endregion
        }
        private void TopDown()
        {
            TopDownCamera.SetActive(!TopDownCamera.activeSelf);
            cam4_VideoCapture.SetActive(!TopDownCamera.activeSelf);
        }
        private void SwitchCamera()
        {
            if (!front | !sideLeft | !sideRight) { return; }
            if (count % 3 == 0)
            {
                front.m_Priority = 3;
                //cam1_VideoCapture.SetActive(true);
                //cam2_VideoCapture.SetActive(false);
                //cam3_VideoCapture.SetActive(false);
                sideLeft.m_Priority = 2;
                sideRight.m_Priority = 1;
                _hudCanvas.SetActive(true);
            }
            else if (count % 3 == 1)
            {
                sideLeft.m_Priority = 3;
                front.m_Priority = 2;
                sideRight.m_Priority = 1;
                //cam1_VideoCapture.SetActive(false);
                //cam2_VideoCapture.SetActive(true);
                //cam3_VideoCapture.SetActive(false);
                _hudCanvas.SetActive(false);
            }
            else if (count % 3 == 2)
            {
                sideRight.m_Priority = 3;
                sideLeft.m_Priority = 2;
                front.m_Priority = 1;
                //cam1_VideoCapture.SetActive(false);
                //cam2_VideoCapture.SetActive(false);
                //cam3_VideoCapture.SetActive(true);
                _hudCanvas.SetActive(false);
            }
            count++;

        }
    }
}

