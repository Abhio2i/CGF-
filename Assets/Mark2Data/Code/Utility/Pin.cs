using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.waypoint;

public class Pin : MonoBehaviour
{
    Color mycolor;
    WayPointmanager wayPointmanager;
    GameObject gameManager;
    GameObject wayPoint_manager;
    bool editIsActive;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManger");
        wayPoint_manager=gameManager.transform.GetChild(4).gameObject; //adjust the child number according to the index of "Waypoint"
        mycolor = gameObject.GetComponent<Renderer>().material.color;
        wayPointmanager=wayPoint_manager.GetComponent<WayPointmanager>();
    }
    private void Update()
    {
        if (wayPoint_manager.GetComponent<EditWaypoint>() != null)
            editIsActive = true;
    }
    private void OnMouseOver()
    {
        if(!editIsActive)
            gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    void OnMouseExit()
    {
        if(!editIsActive)
            gameObject.GetComponent<Renderer>().material.color = mycolor;
    }
}
