using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Spawning;
public class RotateObject : MonoBehaviour
{
    public GameObject model;
    GameObject current;
    SpawnManager spawnManager;
    public Camera _camera;
    bool isCreated;
    private void Start()
    {
        spawnManager=GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        //current = new GameObject();
    }
    private void Update()
    {
       
        if (model != null)
        {
            model.transform.Rotate(0, 3f * Time.deltaTime, 0, Space.Self);
        }
    }
}
