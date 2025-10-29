using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dum : MonoBehaviour
{
    public RawImage alt;
    public RawImage speed;
    public GameObject player;
    public float var = 100f;
    Rigidbody playerObj;
    float playerSpeed;
    Vector3 position;
    Vector3 speedPosition;
    private void Start()
    {
        position = alt.GetComponent<RectTransform>().anchoredPosition;
        speedPosition=speed.GetComponent<RectTransform>().anchoredPosition;
        playerObj =player.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float offset = player.transform.position.y * 0.1f;
        alt.GetComponent<RectTransform>().anchoredPosition = new Vector3(position.x, position.y+offset, position.z);
        playerSpeed = playerObj.velocity.magnitude * 2f;
        speed.GetComponent<RectTransform>().anchoredPosition = new Vector3(speedPosition.x, speedPosition.y + playerSpeed, speedPosition.z);
    }

}
