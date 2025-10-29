using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEwSuit : MonoBehaviour
{
    public GameObject preview;
    public GameObject[] spawnObjects;
    private GameObject currentlySelected;
    public Transform mainPlane;
    public float offset = 5;

    public void Start()
    {
        currentlySelected = spawnObjects[0];
    }
    public void DropDownSpawn(int i)
    {
        currentlySelected = spawnObjects[i];
    }
    public void SpawnNow()
    {
        var obj = Instantiate(currentlySelected);
        obj.transform.position = mainPlane.position + mainPlane.forward * Camera.main.orthographicSize / offset + obj.transform.up * -5f; ;
        GetComponent<Selector>().filterObj.Add(obj);
        //Destroy(obj, 180);
    }
    public void closePreview()
    {
        preview.SetActive(false);
    }
}
