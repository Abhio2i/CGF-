using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Waypoints : MonoBehaviour
{
    int maxPoints;
    int count;

    [SerializeField] private Camera _camera;

    public GameObject pin;

    public List<GameObject> points;
    [SerializeField] float altitude;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        maxPoints = 10;
        count = 0;
        altitude = 500f;
    }

    // Update is called once per frame
    void Update()
    {
        SetUpWayPoints();
    }
    //sets up waypoints
    void SetUpWayPoints()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (count < maxPoints)
            {
                UnityEngine.Vector3 newPos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                newPos = new UnityEngine.Vector3(newPos.x, altitude, newPos.z);
                GameObject waypins = Instantiate(pin, newPos, Quaternion.identity);
                points.Add(waypins);
                count++;
            }
        }
    }
}
