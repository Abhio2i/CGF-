using Cinemachine;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Feed;
using System.Collections.Generic;

namespace Assets.Scripts.Feed_Scene.newFeed
{
    public class CameraBehaviourForMissiles : MonoBehaviour
    {
        public List<GameObject> missiles = new List<GameObject>();
        public CameraManager cameraManager;

        private CinemachineVirtualCamera camera;
        private GameObject missile;
        Inputs inputActions;

        public bool iAmActive;

        int count = -1;

        private void Awake()
        {
            inputActions=new Inputs();

            inputActions.Camera.Missile_Trigger.performed += _ =>
            {
                CycleMissile();
            };
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }
        private void FixedUpdate()
        {
            camera = CameraManager.activeCineCamera;
        }
        public void CycleMissile()
        {
            if (missiles.Count <= 0) return;
            
            count++;
            
            if(count==missiles.Count)
            {
                count = -1;
                cameraManager.SelectTarget();
                return;
            }
            missile = missiles[count];
            
            AssignCameraToTarget();
        }
        void AssignCameraToTarget()
        {
            if(missile!=null && missile.GetComponent<SetEntityPositionAndRotation>().iAmActive)
            {
                camera.LookAt = missile.transform;
                camera.Follow = missile.transform;
            }
            else
            {
                count = -1;
            }
        }
    }
}