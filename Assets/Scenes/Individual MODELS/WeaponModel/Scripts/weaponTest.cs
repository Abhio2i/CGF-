using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponTest : MonoBehaviour
{
    [Header("Velocity of Missile km/hr")]
    public float Vmissile = 100f;
    [Header("Missile burn Time in sec")]
    public float Tburn = 5f;
    [Header("Velocity of Enemy km/hr")]
    public float Venemy = 100f;
    [Header("Velocity of Shooter km/hr")]
    public float Vyou = 100f;
    [Header("Angle between Target and Shooter km/hr")]
    public float angle = 30f;

    [Header("------------------ ----------")]
    [Header("Range")]
    [Header("success")]
    [Header("success")]
    [Header("success")]
    [Header("success")]
    [Header("success")]
    public float Rmax = 0f;
    
     SilantroGun gun;
    // Start is called before the first frame update
    void Start()
    {
        //gun.InitializeGun();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Mouse0))
        {
            //gun.FireGun();
        }
        
        getRange();
    }


    public void getRange()
    {
        Rmax = ((Vmissile * (Tburn/3600)) / 2) * ((Venemy + Vyou) / Venemy)* Mathf.Cos(angle*(Mathf.PI/180));
    }
}
