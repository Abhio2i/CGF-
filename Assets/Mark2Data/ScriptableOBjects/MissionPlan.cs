using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MissionPlan")]
[Serializable]


public class MissionPlan : ScriptableObject
{
    [SerializeField]
    public MissionInfo missionInfo;
    [SerializeField]
    public List<AircraftPlanData> ally_spawnPlanes;
    [SerializeField]
    public List<AircraftPlanData> neutral_spawnPlanes;
    [SerializeField]
    public List<AircraftPlanData> adversary_spawnPlanes;
    [SerializeField]
    public List<AircraftPlanData> sams_spawnPlanes;
    [SerializeField]
    public List<AircraftPlanData> Warship_spawnPlanes;
    [SerializeField]
    public List<Cordinates> waypoints;
    [SerializeField]
    public WeatherData WeatherData;

}


