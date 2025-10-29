using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//script info
//displays player speed,and altitude

public class PFD : MonoBehaviour
{
    public RawImage alt;
    public RawImage speed;
    public TMP_Text _speed, _alt;
    public GameObject player;
    public float var = 100f;
    Rigidbody playerObj;
    float playerSpeed;
    Vector3 position;
    Vector3 speedPosition;
    private void Start()
    {
        position = alt.GetComponent<RectTransform>().anchoredPosition;
        playerObj = player.GetComponent<Rigidbody>();
        speedPosition = speed.GetComponent<RectTransform>().anchoredPosition;
    }
    private void Update()
    {
        playerSpeed = playerObj.velocity.magnitude * 2f;
        float offset = player.transform.position.y * 0.1f;

        alt.GetComponent<RectTransform>().anchoredPosition = new Vector3(position.x, position.y + offset, position.z);
        speed.GetComponent<RectTransform>().anchoredPosition = new Vector3(speedPosition.x, speedPosition.y + playerSpeed, speedPosition.z);
       
        int temp1 = (int)player.transform.position.y;
        int temp2 = (int)playerObj.velocity.magnitude;


        _speed.text = temp2.ToString();
        _alt.text = temp1.ToString();
    }

}
