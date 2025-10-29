using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerForWaypoints : MonoBehaviour
{
    public Transform player;
    public UnityEngine.Vector3 cameraOffset;
    public float smoothFactor = 0.5f;
    public bool NDCam;
    void Start()
    {
        if (NDCam) cameraOffset = new Vector3(0, 10000, 0);
        else
        {
            cameraOffset = transform.position - player.transform.position;
        }
    }
    void LateUpdate()
    {
        if(player == null) return;
        UnityEngine.Vector3 newPosition = player.transform.position + cameraOffset;
        transform.position = UnityEngine.Vector3.Slerp(transform.position, newPosition, smoothFactor);
    }
}
