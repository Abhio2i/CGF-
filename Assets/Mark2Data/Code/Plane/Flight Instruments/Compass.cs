using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public GameObject compass,player,globe;
    public TextMeshProUGUI compassHeading;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //compass.transform.rotation = Quaternion.Euler(0, 0, player.transform.eulerAngles.y);
        if (globe != null)
        {
            globe.transform.localEulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
        }
        if(compassHeading != null)
        {
            compassHeading.text = player.transform.eulerAngles.y.ToString("000");
        }
    }
}
