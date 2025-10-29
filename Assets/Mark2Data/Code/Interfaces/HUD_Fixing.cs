using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class HUD_Fixing : MonoBehaviour
{
    public Transform Nose;
    public Transform Hud;
    public Camera camera;
    public float Smooth = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float width;
    private void Update()
    {
        //HudUpadte();
    }

    private void LateUpdate()
    {
        //HudUpadte();
    }

    void FixedUpdate()
    {
        HudUpadte();
    }
    public void HudUpadte()
    {
        Vector3 _screen = camera.ViewportToScreenPoint(new Vector3 (1,1,0));
        Vector3 point = camera.WorldToScreenPoint(Nose.position);
        point.z = 0;
        point.x = point.x - (_screen.x/ 2);
        point.y = point.y - (_screen.y / 2);
        //Debug.Log(_screen);
        //float dist = Vector3.Distance(Hud.localPosition,point);
        Vector3 pos = Vector3.Lerp(Hud.localPosition, point, Time.fixedDeltaTime * Smooth);
        if (Vector3.Distance(Vector3.zero, pos) < 10)
        {
            pos = Vector3.zero;
        }
        Hud.localPosition = pos;
        
    }

   
}
