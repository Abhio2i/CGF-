using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnPlanesData
{
    public enum StartMode{
        Cold,
        Hot
    }
    [Header("Position")]
    public float Latitude;
    public float Longitude;
    public float Altitude;
    public float Bearing;
    public StartMode startMode;
    public float speed;

    [Header("Weapons")]
    public int Chaffs;
    public int Flares;
    public int Bullets;
    public List<string> Hardpoints;

    [Header("Radar")]
    public float Power;
    public float Gain;
    public float Frequency;
    public float MinimalNoise;
    public float RCS;

    [Header("Electronic Warfare")]
    public float EWPower;
    public float EWGain;
    public float RecieveGain;
    public float Threshold;
    public bool AutoChaffs;
    public bool AutoFlares;
    public bool DIRCM;
    public bool Jammer;



    [Header("OLD")]
    public string name, type, model, country, pilot, skill, count, callSignCode;
    public int formationType, unit, callSign1, callSign2;
    public Vector3 spawnPosition;
    public bool radio, hiddenOnRadar, hiddenOnMap, hiddenOnPlanner, playerChase;


    //Loadout
    public bool flares, chaffs, jammer, gun;
    public int flareCount, chaffCount,flareBurst=24,chaffBurst=24, jammerMode, gunAmmo;
    public string s1, s2, s3, s4, s5L, s5, s5R, s6, s7, s8, s9;
    // public Vector2 aroundPoint; 

    public SpawnPlanesData(SpawnPlanesData data)
    {
        name = data.name;
        type = data.type; model = data.model; country = data.country;
        pilot = data.pilot;
        skill = data.skill; count = data.count; unit = data.unit;
        callSign1 = data.callSign1; callSign2 = data.callSign2;
        formationType = data.formationType; callSignCode = data.callSignCode;
        spawnPosition = data.spawnPosition;
        radio = data.radio;
        hiddenOnRadar = data.hiddenOnRadar;
        hiddenOnMap = data.hiddenOnMap;
        hiddenOnPlanner = data.hiddenOnPlanner;
        playerChase = data.playerChase;
        flareBurst = 24; chaffBurst = 24;
        //   aroundPoint = data.aroundPoint;
    }
    public void resetme()
    {
        s1=s2=s3=s4=s5L=s5=s5R=s6=s7=s8=s9="";
    }
}
