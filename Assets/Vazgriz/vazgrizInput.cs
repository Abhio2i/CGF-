using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class vazgrizInput : MonoBehaviour
{
    //public Vazgriz.Plane.Plane plane;
    public UnityStandardAssets.Vehicles.Aeroplane.AeroplaneController plane;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Joystick.all.Count > 0)
        {
            Vector2 temp = Joystick.all[0].stick.ReadUnprocessedValue();
            if (temp.x < 0)
                temp.x = (1 + temp.x);
            else
                temp.x = -1 * (1 - temp.x);

            if (temp.y < 0)
                temp.y = 1 + temp.y;
            else
                temp.y = -1 * (1 - temp.y);
            //_rawRollInput = temp.x;
            //_rawPitchInput = -temp.y;
            //plane.SetThrottleInput(1);
            //plane.SetControlInput(new Vector3(temp.y*10, 0, -temp.x));
            plane.Move(-temp.x, temp.y, 0, 1, false);
            Debug.Log(temp);
        }
        
    }
}
