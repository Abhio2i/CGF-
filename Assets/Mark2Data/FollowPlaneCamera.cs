using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlaneCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform camera;

    Vector3 offset;
    private void Start()
    {
        offset=target.position-camera.position;
    }
    private void FixedUpdate()
    {
        Vector3 tempPos = target.position + offset;
        camera.position=new Vector3(tempPos.x,camera.position.y,tempPos.z);
    }
}
