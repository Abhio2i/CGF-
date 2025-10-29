using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopControl : MonoBehaviour
{
    public bool isLeft;
    public bool isRight;

    public bool isTurningLeft;
    public bool isTurningRight;
    //public SilantroInput input;
    public Rigidbody player;

    public Transform playerRotations;

    public float speed,mySpeed;


    float rotationDirection;


    private void FixedUpdate()
    {
       rotationDirection = PlayerPrefs.GetFloat("RotationAngle");
        Quaternion playerAngles = playerRotations.rotation;
        speed = player.velocity.magnitude;
        if(rotationDirection>0)
        {
            if(isLeft)
            {
                mySpeed = 1.3f * speed;
            }
            else if(isRight)
            {
                mySpeed = 0.7f * speed;
            }
        }
        else if (rotationDirection < 0)
        {
            if (isLeft)
            {
                mySpeed = 0.7f * speed;
            }
            else if (isRight)
            {
                mySpeed = 1.3f * speed;
            }
        }
        else
        {
            mySpeed = speed;
        }

        GetComponent<Rigidbody>().AddForce(transform.forward * mySpeed);
    }
}
