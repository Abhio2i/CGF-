using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/SpawnMaster")]
[Serializable]
public class MasterSpawn : ScriptableObject
{
    [SerializeField]
    public List<SpawnPlanesData> ally_spawnPlanes;
    //[SerializeField]
    //public List<AIPlaneData> ai_spawnPlanes;
    [SerializeField]
    public List<SpawnPlanesData> neutral_spawnPlanes;
    [SerializeField]
    public List<SpawnPlanesData> adversary_spawnPlanes;
    [SerializeField]
    public List<SpawnPlanesData> sams_spawnPlanes;
}