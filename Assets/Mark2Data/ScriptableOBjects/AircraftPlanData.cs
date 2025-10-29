using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AircraftPlanData
{
    public enum StartMode
    {
        Cold,
        Hot
    }
    [Header("Position")]
    public float Latitude;
    public float Longitude;
    public float Altitude;
    public float Bearing;
    public float Heading;
    public StartMode startMode;
    public float speed;

    [Header("Weapons")]
    public int Chaffs;
    public int Flares;
    public int Bullets;
    public List<string> Hardpoints;
    public List<MissileInfo> missileInfo;

    [Header("Radar")]
    public float Power;
    public float Gain;
    public float Frequency;
    public float Temp;
    public float NoiseFactor;
    public float MinimalNoise;
    public float TRCS;

    [Header("Electronic Warfare")]
    public float EWPower;
    public float EWGain;
    public float EWFrequency;
    public float RecieveGain;
    public float Threshold;
    public bool AutoChaffs;
    public bool AutoFlares;
    public bool DIRCM;
    public bool Jammer;

    [Header("JAMMER Configuration")]
    public float TransmittedPower;
    public float RadarReceiverGain;
    public float RadarFrequency;
    public float TargetCrossSection;
    public float TargetDistance;
    public float JammerTransPower;
    public float JammerReceiverGain;
    public float JammerTransFreq;
    public float TargetReceiJamSignalGain;

    [Header("OLD")]
    public string name, type, model, country, pilot, skill, count, callSignCode;
    public int formationType, unit, callSign1, callSign2;
    public Vector3 spawnPosition;
    public bool radio, hiddenOnRadar, hiddenOnMap, hiddenOnPlanner, playerChase;


    //Loadout
    public bool flares, chaffs, jammer, gun;
    public int flareCount, chaffCount, flareBurst = 24, chaffBurst = 24, jammerMode, gunAmmo;
    public string s1, s2, s3, s4, s5L, s5, s5R, s6, s7, s8, s9;

    public AircraftPlanData(AircraftPlanData data=null)
    {
        if(data != null)
        {
            // Copy Position data
            Latitude = data.Latitude;
            Longitude = data.Longitude;
            Altitude = data.Altitude;
            Bearing = data.Bearing;
            Heading = data.Heading;
            startMode = data.startMode;
            speed = data.speed;

            // Copy Weapons data
            Chaffs = data.Chaffs;
            Flares = data.Flares;
            Bullets = data.Bullets;
            Hardpoints = new List<string>(data.Hardpoints);
            missileInfo = new List<MissileInfo>();
            foreach(MissileInfo info  in data.missileInfo)
            {
                MissileInfo inf = new MissileInfo(info);
                missileInfo.Add(inf);
            }

            // Copy Radar data
            Power = data.Power;
            Gain = data.Gain;
            Frequency = data.Frequency;
            Temp = data.Temp;
            NoiseFactor = data.NoiseFactor;
            MinimalNoise = data.MinimalNoise;
            TRCS = data.TRCS;

            // Copy Electronic Warfare data
            EWPower = data.EWPower;
            EWGain = data.EWGain;
            EWFrequency = data.EWFrequency;
            RecieveGain = data.RecieveGain;
            Threshold = data.Threshold;
            AutoChaffs = data.AutoChaffs;
            AutoFlares = data.AutoFlares;
            DIRCM = data.DIRCM;
            Jammer = data.Jammer;

            //JAMMER Configuration
            TransmittedPower = data.TransmittedPower;
            RadarReceiverGain = data.RadarReceiverGain;
            RadarFrequency = data.RadarFrequency;
            TargetCrossSection = data.TargetCrossSection;
            TargetDistance = data.TargetDistance;
            JammerTransPower = data.JammerTransPower;
            JammerReceiverGain = data.JammerReceiverGain;
            JammerTransFreq = data.JammerTransFreq;
            TargetReceiJamSignalGain = data.TargetReceiJamSignalGain;

            // Copy OLD data
            name = data.name;
            type = data.type;
            model = data.model;
            country = data.country;
            pilot = data.pilot;
            skill = data.skill;
            count = data.count;
            callSignCode = data.callSignCode;
            formationType = data.formationType;
            unit = data.unit;
            callSign1 = data.callSign1;
            callSign2 = data.callSign2;
            spawnPosition = data.spawnPosition;
            radio = data.radio;
            hiddenOnRadar = data.hiddenOnRadar;
            hiddenOnMap = data.hiddenOnMap;
            hiddenOnPlanner = data.hiddenOnPlanner;
            playerChase = data.playerChase;

            // Copy Loadout data
            flares = data.flares;
            chaffs = data.chaffs;
            jammer = data.jammer;
            gun = data.gun;
            flareCount = data.flareCount;
            chaffCount = data.chaffCount;
            flareBurst = data.flareBurst;
            chaffBurst = data.chaffBurst;
            jammerMode = data.jammerMode;
            gunAmmo = data.gunAmmo;
            s1 = data.s1;
            s2 = data.s2;
            s3 = data.s3;
            s4 = data.s4;
            s5L = data.s5L;
            s5 = data.s5;
            s5R = data.s5R;
            s6 = data.s6;
            s7 = data.s7;
            s8 = data.s8;
            s9 = data.s9;
        }
    }
}
