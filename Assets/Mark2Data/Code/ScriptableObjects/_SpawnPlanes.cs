using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/PlaneSpawning")]
public class _SpawnPlanes : ScriptableObject
{
    public string _name, type, model, country, pilot, skill, count;
    public int unit;
    public int callSign1, callSign2;
    public int formationType;
    public List<float> spawnPositionsAtY, spawnPositionsAtZ, spawnPositionsAtX;
    public bool radio, hiddenOnMap, hiddenOnPlanner, hiddenOnRadar, playerChase;
    public string callSign_code;

    //Loadout
    public bool flares, chaffs, jammer, gun;
    public int flareCount, chaffChount, flaresBurstSize,ChaffsBurstSize, jammerMode, gunAmmo;
    public string s1, s2, s3, s4, s5l, s5, s5r, s6, s7, s8, s9;
    // public Vector2 aroundPoint;
    public _SpawnPlanes(_SpawnPlanes data)
    {
        name = data.name;
        type = data.type; model = data.model; country = data.country;
        pilot = data.pilot;
        skill = data.skill; count = data.count; unit = data.unit;
        callSign1 = data.callSign1; callSign2 = data.callSign2;
        formationType = data.formationType;
        callSign_code = data.callSign_code;
        spawnPositionsAtY = data.spawnPositionsAtY;
        spawnPositionsAtZ = data.spawnPositionsAtX;
        spawnPositionsAtX = data.spawnPositionsAtZ;
        radio = data.radio;
        hiddenOnRadar = data.hiddenOnRadar;
        hiddenOnMap = data.hiddenOnMap;
        hiddenOnPlanner = data.hiddenOnPlanner;
        playerChase = data.playerChase;
        // aroundPoint = data.aroundPoint;
    }
}


