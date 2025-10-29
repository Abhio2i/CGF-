using Assets.Code.UI;
using Assets.Scripts.Feed;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Flight_Instruments;
using UnityEngine;

[Serializable]
public class ExtraAIPlaneLog
{
    public List<bool> missile = new List<bool>();
    public List<bool> bombs = new List<bool>();
    public float leftTank;
    public float centerTank;
    public float rightTank;
    public string remainFuel;
    public Maneuvers maneuver;
}

public class LogRegisterAI : MonoBehaviour
{

    public CombineUttam radar;
    public CombineEW rwr;
    public ManeuverControls ManeuverControls;
    //public FuelGauge fuelGauge;
    public PlaneData planeData;
    //public WeaponTracker weapon;
    

    // Update is called once per frame
    void Update()
    {
       // ExtraPlaneLog extraPlaneLog = new ExtraPlaneLog();
        //if (fuelGauge.leftTk != null)
        //{
        //    extraPlaneLog.leftTank = fuelGauge.leftTk.value;
        //    extraPlaneLog.centerTank = fuelGauge.centerTk.value;
        //    extraPlaneLog.rightTank = fuelGauge.rightTk.value;
        //    extraPlaneLog.remainFuel = fuelGauge.RemainText.text;
        //}
        //foreach(var b in weapon.HardPointsMissileUI)
        //{
        //    extraPlaneLog.missile.Add(b.activeSelf);
        //}
        //extraPlaneLog.missile.Add(weapon.WarningText.activeSelf);
        //foreach (var b in weapon.HardPointBombUI)
        //{
        //    extraPlaneLog.bombs.Add(b.activeSelf);
        //}
        //extraPlaneLog.bombs.Add(weapon.WarningTextBomb.activeSelf);

        //planeData.SetMessage(radar.Log+"|**|"+rwr.Log+"|**|"+JsonUtility.ToJson(extraPlaneLog));

        planeData.SetMessage("|*^*|"+radar.Log + "|**|" + rwr.Log);
    }
}
