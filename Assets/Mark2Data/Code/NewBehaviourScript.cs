using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public MasterSpawn spawn;
    public List<_SpawnPlanes> ally_spawnPlanes;
    public List<_SpawnPlanes> adversary_spawnPlanes;
    public List<_SpawnPlanes> neutral_spawnPlanes;

    [SerializeField] List<string> names;
    [SerializeField] List<string> count;
    private void Awake()
    {
        names.Clear();
        count.Clear();
    }
    private void Start()
    {
        
    }
}
