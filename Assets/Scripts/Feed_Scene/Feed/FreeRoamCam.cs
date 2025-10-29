using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Feed
{
    public class FreeRoamCam : MonoBehaviour
    {
        [SerializeField] Camera freeRoamCam;

        [SerializeField] float panSpeed;

        [SerializeField] float rotateSpeed;

        Inputs inputActions;

        public static float x_axis,y_axis,z_axis;
        public static bool rotating,positioning;
        public bool SatelliteCam;
        public GameObject GodCanvas;
        private static Camera cam;

        Vector3 refVelocity=Vector3.zero;

        public GameObject look;

        public void EnableCamera(bool isactive)
        {
            if (SatelliteCam)
            {
                GodCanvas.SetActive(isactive);
            }
            else
            {
                freeRoamCam.gameObject.SetActive(isactive);
                LookAtThisObject();
            }
        }
        void LookAtThisObject()
        {
            look = CameraManager.selected;
            Vector3 newPos = CameraManager.selected.GetComponent<SetEntityPositionAndRotation>().currentPos;
            print(CameraManager.activeCineCamera.transform.position);
            freeRoamCam.transform.position = CameraManager.activeCineCamera.transform.position;
            freeRoamCam.transform.rotation = CameraManager.activeCineCamera.transform.rotation;
        }
        private void FixedUpdate()
        {
            if (freeRoamCam!=null)
            {
                if(positioning)
                {
                    ChangeCameraPosition();
                }
                else if(rotating)
                {
                    RotateCamera();
                }    
            }
        }
        void ChangeCameraPosition()
        {
            float temp = panSpeed * Time.fixedDeltaTime;

            Vector3 targetCamPosition = freeRoamCam.transform.localPosition + new Vector3(x_axis, y_axis, z_axis) * temp;

            freeRoamCam.transform.localPosition = targetCamPosition;//Vector3.SmoothDamp(freeRoamCam.transform.localPosition
                //targetCamPosition, ref refVelocity, 3f);

        }
        void RotateCamera()
        {
            float temp =rotateSpeed * Time.fixedDeltaTime;
            freeRoamCam.transform.Rotate(y_axis*temp, x_axis*temp, z_axis*temp);
            //var rotateVector3=freeRoamCam.transform.rotation.eulerAngles + new Vector3(y_axis,x_axis, z_axis) * temp;

            //freeRoamCam.transform.rotation=Quaternion.Slerp(freeRoamCam.transform.rotation
            //    ,Quaternion.Euler(rotateVector3),0.5f*Time.fixedDeltaTime);
        }

    }
}