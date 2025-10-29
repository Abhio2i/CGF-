using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAnotherCam : MonoBehaviour
{
    [Header("Main Cameras")]
    public Camera CameraFront;
    public Camera CameraTop;
    public Camera CameraRight;
    public Camera CameraLeft;
    
    [Header("Follow Cameras")]
    public Camera FollowCameraFront;
    public Camera FollowCameraTop;
    public Camera FollowCameraRight;
    public Camera FollowCameraLeft;

    // Follow all cameras to actual cameras.
    void FixedUpdate()
    {
        FollowCameraFront.transform.SetPositionAndRotation(CameraFront.transform.position, CameraFront.transform.rotation); 
        FollowCameraTop.transform.SetPositionAndRotation(CameraTop.transform.position,CameraTop.transform.rotation);
        FollowCameraLeft.transform.SetPositionAndRotation(CameraLeft.transform.position, CameraLeft.transform.rotation);
        FollowCameraRight.transform.SetPositionAndRotation(CameraRight.transform.position, CameraRight.transform.rotation);
        
    }
}
