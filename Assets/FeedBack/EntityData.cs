using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class EntityData 
{
    [SerializeField]
    public List<Vector3> position;
    [SerializeField]
    public List<Vector3> rotation;
    [SerializeField]
    public List<bool> active;
    [SerializeField]
    public TextMeshProUGUI Text;
    [SerializeField]
    public EWRadar eWRadar;
    [SerializeField]
    public CombineUttam aIRadar;
    [SerializeField]
    public Specification specification;
    [SerializeField]
    public Specification.EntityType specificationType;
    [SerializeField]
    public List<string> value;
    [SerializeField]
    public List<RadarLog> radarLog;
    [SerializeField]
    public List<AIRadarLog> aiRadarLog;
    [SerializeField]
    public List<EWRadarLog> ewRadarLog;
    [SerializeField]
    public List<EntityInfoLog> entityInfoLog;
    [SerializeField]
    public List<WeaponLog> weaponLog;

    public EntityData(){ 
        position = new List<Vector3>();
        rotation = new List<Vector3>();
        active = new List<bool>();
        Text = null;

        eWRadar = null;
        value = new List<string>();
        radarLog = new List<RadarLog>();
        aiRadarLog = new List<AIRadarLog>();
        ewRadarLog = new List<EWRadarLog>();
        entityInfoLog = new List<EntityInfoLog>();
        weaponLog = new List<WeaponLog>();
        specificationType = Specification.EntityType.None;
}

}

[Serializable]
public class EventData
{
    [SerializeField]
    public List<string> events;

    public EventData()
    {
        events = new List<string>();
    }

}