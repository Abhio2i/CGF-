using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public enum update
    {
        FixedUpdate,
        Update
    }
    public update UpdateType = update.Update;
    public Transform target;
    public Vector3 TargetPosition;
    public Vector3 TargetRotation;
    public float SmoothPostion;
    public float SmoothRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UpdateType == update.Update)
        {
            manualUpdate(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (UpdateType == update.FixedUpdate)
        {
            manualUpdate(Time.fixedDeltaTime);
        }
    }

    public void manualUpdate(float delta)
    { 
        TargetPosition = Vector3.Lerp(TargetPosition,target.position, delta * SmoothPostion);
        transform.position = TargetPosition;


        TargetRotation = Vector3.Lerp(TargetRotation, target.eulerAngles, delta * SmoothRotation);
        transform.eulerAngles = TargetRotation;

        Vector3 direction = target.position - transform.position;

        // Create the rotation we need to be in to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target point  
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, delta * SmoothRotation);


    }
}
