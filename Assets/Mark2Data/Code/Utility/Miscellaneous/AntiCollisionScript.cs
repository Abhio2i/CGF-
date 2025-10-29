using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCollisionScript : MonoBehaviour
{
    #region collision checking elements
    [Header("References")]
    float rayLength = 200f;
    int numberOfRays = 12;//to control the sensitivity adjust the number of rays // choose even number of rays.
    float angle = 90f;
    RaycastHit hitTerrain;
    //RaycastHit hitGround;
    public float rightRotation;
    public float leftRotation;
    //public float rotateDuration;
    public bool collisionInFront;//checks collision in front
    public bool collisionInBelow;//checks collision below
    #endregion 

    public GameObject plane;

    InterfaceFunctions function;

    public void Start()
    {
        function=new InterfaceFunctions();
        function.planePosition = plane;
        collisionInFront = false;
        collisionInFront = false;
    }

    // float timeToComplete;
    public bool sign = false;
    public void CheckCollisionsInRange()
    {
        LayerMask ground = LayerMask.GetMask("Ground");
        rightRotation = leftRotation =0;
        // timeToComplete= 0;
        for (int i = 0; i < numberOfRays; i++)
        {
            //Debug.Log(i);
            var rotation = plane.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / (float)numberOfRays) * angle * 2 - angle, plane.transform.up);
            var direction = rotation * rotationMod * (UnityEngine.Vector3.forward * rayLength);
            var ray = new Ray(plane.transform.position, direction);
            Debug.DrawRay(plane.transform.position, direction, Color.green);
            if (Physics.Raycast(plane.transform.position,direction,out hitTerrain,200f,ground))
            {

                
                
                plane.GetComponent<Rigidbody>().velocity = plane.transform.forward * 10f;

                if (hitTerrain.collider.tag == "Ground" )
                {
                  
                    Debug.DrawRay(plane.transform.position, direction, Color.red);
                    Gizmos.color = Color.red;
                    if (i < numberOfRays / 2)
                    {
                        rightRotation++;
                        //collisionInFront = true;
                       
                    }
                    else
                    {
                        leftRotation++;
                        //collisionInFront = true;
                      
                    }
                    collisionInFront = true;
                }
            }
            else
            {
                collisionInFront = false;
            }
            sign=collisionInFront & (sign | true);
            //else if ((i == numberOfRays - 1) && !Physics.Raycast(ray, out hitTerrain, 200))
            //{
            //    collisionInFront = false;
            //}
        }
        if (rightRotation >= 3)
        {

            UnityEngine.Vector3 targetRotation = plane.transform.rotation.eulerAngles + new UnityEngine.Vector3(0, 180f, 0);
                    Debug.Log(plane.transform.rotation.eulerAngles + " "+targetRotation);
                    Debug.DrawRay(plane.transform.position, plane.transform.forward * 200f, Color.cyan);
            plane.transform.rotation = Quaternion.Euler(plane.transform.rotation.eulerAngles + new UnityEngine.Vector3(0, 270f, 0) * 0.15f * Time.deltaTime);
            Debug.Log("right " + plane.transform.rotation);
        }


        if (leftRotation >= 3)
        {

            UnityEngine.Vector3 targetRotation = plane.transform.rotation.eulerAngles - new UnityEngine.Vector3(0, 180f, 0);
            Debug.Log(plane.transform.rotation.eulerAngles + " " + targetRotation);
            Debug.DrawRay(plane.transform.position, plane.transform.forward * 150f, Color.cyan);
            plane.transform.rotation = Quaternion.Euler(plane.transform.rotation.eulerAngles - new UnityEngine.Vector3(0, 270f, 0) * 0.3f * Time.deltaTime);
        }
        



    }
    public void CheckCollisionWithGround()
    {

        if (Physics.Raycast(plane.transform.position, -1 * plane.transform.up, out hitTerrain, 100))
        {
            plane.GetComponent<Rigidbody>().velocity = plane.transform.forward * 30f;
            if (hitTerrain.collider.tag == "Ground")
            {
                collisionInBelow = true;
                UnityEngine.Vector3 newAngle = plane.transform.rotation.eulerAngles + new UnityEngine.Vector3(-60f, 0, 0);
                plane.transform.rotation = Quaternion.Euler(plane.transform.rotation.eulerAngles + new UnityEngine.Vector3(-60f, 0, 0) * 0.3f * Time.deltaTime);
                //function.Rotate(newAngle);
            }
        }
        else
        {
            collisionInBelow = false;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            Gizmos.color = Color.green;
            var rotation = plane.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / (float)numberOfRays) * angle * 2 - angle, plane.transform.up);
            var direction = rotation * rotationMod * (UnityEngine.Vector3.forward * rayLength);
            Gizmos.DrawRay(plane.transform.position, direction);
        }
        Gizmos.DrawRay(plane.transform.position, -100 * plane.transform.up);

    }
}
