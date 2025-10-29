using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Feed
{
    public class CameraManager : MonoBehaviour
    {

        public static GameObject selected;

        public static CinemachineVirtualCamera activeCineCamera,disableCineCamera;

        public List<CinemachineVirtualCamera> virtualCamera;

        //public List<Camera

        public int selectionDirection;
        
        public int index,previousPriority;
        
        public static int priority;

        public List<GameObject> objects;

        private void Awake()
        {
            activeCineCamera = virtualCamera[0];
            disableCineCamera = virtualCamera[1];
            objects = new List<GameObject>();
        }
        public void SwitchCamPosition(bool index)
        {
            if (index)
            {
                activeCineCamera = virtualCamera[1];
                disableCineCamera = virtualCamera[0];
            }
            else
            { 
                activeCineCamera = virtualCamera[0];
                disableCineCamera = virtualCamera[1];
            }
            ActiveVirtualCam();
        }
        public void ActiveVirtualCam()
        {
            if (selected == null) return;

            activeCineCamera.Priority = 1;
            disableCineCamera.Priority = 0;
            activeCineCamera.Follow = selected.transform;
            activeCineCamera.LookAt = selected.transform;
        }
        public void SelectTarget()
        {
            if (FreeRoamCam.positioning || FreeRoamCam.rotating) return;

            index = index + selectionDirection;

            if(index<0)
            {
                index = objects.Count - 1;
            }
            if(index>=objects.Count)
            {
                index = 0;
            }

            selected = objects[index];
            
            selectionDirection=0;

            ActiveVirtualCam();
        }
      
        
    }
}