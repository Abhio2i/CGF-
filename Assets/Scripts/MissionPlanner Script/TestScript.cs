using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class TestScript : MonoBehaviour
{
    public RectTransform s;
    public RectTransform t;
    public Vector3 cord = Vector3.zero;
    public Vector3 World = Vector3.zero;
    public Camera MapCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cord = t.eulerAngles;
        World = t.localEulerAngles;
    }

    public Vector3 MapToWordCords(Camera MapCamera, RectTransform UI,RectTransform Target ,float Width,float Height)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(s, t.position, null, out Vector2 localMousePosition);
        //cord = s.TransformDirection(t.localPosition);
        cord = new Vector3(localMousePosition.x, localMousePosition.y, 0);
        localMousePosition.x = (localMousePosition.x + (Width/2)) / Width;
        localMousePosition.y = (localMousePosition.y + (Height/2)) / Height;
        return MapCamera.ViewportToWorldPoint(localMousePosition);
    }
}
