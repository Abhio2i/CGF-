using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Spawning;
public class RotateAndPreviewSelection : MonoBehaviour
{
    [SerializeField]GameObject model;
    [SerializeField] GameObject current;
   

    [SerializeField] Camera myCamera;
    [SerializeField] GameObject spawn_Manager; //put the spawnManger gameObject here
    [SerializeField] float dist;
    [SerializeField] float speed;
    GameObject previous;

    SpawnManager spawnManager;
    private void Awake()
    {
        dist = 3f;
        speed = 10f;
        current = new GameObject();
        model = new GameObject();
        if (spawnManager == null)
        {
            spawnManager = spawn_Manager.GetComponent<SpawnManager>();
            previous = spawnManager.selectedPlane;
        }
    }
    private void Update()
    {
        //spawn model on preview Window
        if (spawnManager.selectedPlane!=null)
        {
            current = spawnManager.selectedPlane;
            if (previous!=current)
            {
                    Destroy(model);
                model = Instantiate(current, myCamera.transform.position + new UnityEngine.Vector3(0, 0, dist), Quaternion.identity);
                    model.layer = LayerMask.NameToLayer("onGUI");

                previous = current;
            }
        }
        model.transform.Rotate(UnityEngine.Vector3.up * speed * Time.deltaTime);
    }
}
