using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceFunctions : MonoBehaviour, Follow
{
    private float planeSpeed;
    // private float percentage;
    private float rotateAngleZ;
    private IEnumerator routine;
    private float duration;
    

    public float speed;
    public GameObject planePosition;
    public UnityEngine.Vector3 targetPosition;
    public float rotateSpeed;

    //move object towards target
    public void Move(UnityEngine.Vector3 targetPosition)
    {

        UnityEngine.Vector3 distbetween = planePosition.transform.position - targetPosition;
        if (distbetween.magnitude < 100f && distbetween.magnitude >= 40f)
        {
            //timeToComplete += Time.deltaTime;
            //if (percentage >= 0)
            //    percentage = 1 - (timeToComplete / stopDuration);
            //planeSpeed = speed * percentage;
            planeSpeed = speed / 2f;
         
            

        }
        else
        {
            if (distbetween.magnitude < 40f)
            {
                planeSpeed = 0.5f;
               // planePosition.GetComponent<Rigidbody>().isKinematic = true;
                
            }
            else
            {
                planeSpeed = speed;
                //planePosition.GetComponent<Rigidbody>().isKinematic = false;
            }
            // percentage = 0f;
        }
        planePosition.GetComponent<Rigidbody>().velocity = planePosition.transform.forward * planeSpeed;
    }
    //rotate plane towards target
    public void Rotate(UnityEngine.Vector3 targetPosition)
    {
        var targetRotation = Quaternion.LookRotation(targetPosition - planePosition.transform.position, UnityEngine.Vector3.up);
        Quaternion rotateAngles = Quaternion.Slerp(planePosition.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        rotateAngleZ = rotateAngles.eulerAngles.z;
        if (rotateAngleZ > 180)
        {
            rotateAngleZ -= 360;
        }
        UnityEngine.Vector3 newAngles = new UnityEngine.Vector3(rotateAngles.eulerAngles.x, rotateAngles.eulerAngles.y, rotateAngleZ);
        planePosition.transform.rotation = Quaternion.Euler(newAngles);
    }


    public void CoroutineSyntax()
    {
        duration += 0.2f;
        if (duration >= 1f)
        {
            Rotate(targetPosition);
            duration = 0f;
        }
        //Rotate(targetPosition);
        Move(targetPosition);
    }
}
