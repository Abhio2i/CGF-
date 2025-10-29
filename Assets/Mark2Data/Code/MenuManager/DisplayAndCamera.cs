using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAndCamera : MonoBehaviour
{

    int dis = 0;
    // Start is called before the first frame update
    void Start()
    {
         dis = Display.displays.Length;
        print(dis);
    }
}
