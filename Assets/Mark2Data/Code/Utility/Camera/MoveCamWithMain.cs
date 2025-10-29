using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamWithMain : MonoBehaviour
{
    public Camera target;
    public Camera follow;

    private void Update()
    {
        target.transform.position = follow.transform.position;
        target.transform.rotation = follow.transform.rotation;
    }
}
