using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CameraControlEW : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI TOPViewRange;
    public void topviewZoomIN()
    {
        var i = 0;
        int low = 1000, max = 60000;
        if (i > 0 && cam.orthographicSize < max)
        {
            cam.orthographicSize += 1000f;
        }
        else
        if (i < 1 && cam.orthographicSize > low)
        {
            cam.orthographicSize -= 1000f;
        }
        //cam.transform.localPosition = new Vector3(0, 0, cam.orthographicSize - 100f);
        //var pos = cam.transform.position;
        //pos.y = 90000;
        //var z = cam.orthographicSize * 2f;
        //TOPViewRange.text = z.ToString() + "m *" + z.ToString() + "m";
    }
    public void topviewZoomOUT()
    {
        var i = 1;
        int low = 1000, max = 60000;
        if (i > 0 && cam.orthographicSize < max)
        {
            cam.orthographicSize += 1000f;
        }
        else
        if (i < 1 && cam.orthographicSize > low)
        {
            cam.orthographicSize -= 1000f;
        }
        //cam.transform.localPosition = new Vector3(0, 0, cam.orthographicSize - 100f);
        //var pos = cam.transform.position;
        //pos.y = 90000;
        //cam.transform.position = pos;
        var z = cam.orthographicSize * 2f;
        TOPViewRange.text = z.ToString() + "m *" + z.ToString() + "m";
    }
}
