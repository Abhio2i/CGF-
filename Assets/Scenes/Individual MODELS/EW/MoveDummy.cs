using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDummy : MonoBehaviour
{
    public float speed = 1.0f;
    public Rigidbody rb;
    public bool move = false;
    public bool controle = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            
            transform.position += transform.forward*speed*Time.deltaTime;
        }
        if (controle)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, 30 * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, -30 * Time.deltaTime, 0);
            }

            transform.position += new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")) * Time.deltaTime * 30 ;
        }
    }
}
