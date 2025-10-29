using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BalanceSystem : MonoBehaviour
{
    [SerializeField] public List<Transform> extraWeight;
    [SerializeField] float val_X, val_Y, val_Z;
    public float sumX, sumY, sumZ, totalMass;
    public Vector3 CentreOfMass, rotAngle;
    public bool doIt;
    public Vector3 _comAngles;
    void Start()
    {
        StartCoroutine(timer());
    }

    // Update is called once per frame
    public Vector3 angles;
    Vector3 rotationFactor;
    public bool isThrottle;
    public bool inputsActive;
    void FixedUpdate()
    {
        //if (inputsActive || isThrottle)                    //uncomment this if you want the balance system to work when there is no inputs from user
          //  return;
        if (inputsActive)                                   //use this if you want the balance system to work when there is only throttle applied ,this will give manual control to user
            return;
        
        sumX = sumY = sumZ = totalMass = 0;

        if (extraWeight.Count > 0)
        {
            if (!doIt)
            {
                //tilt plane due to COM

                TiltingPlane();
                Calculate(extraWeight);
            }
            else
            {
                //auto balance function

                BalancingPlane();
            }
        }
        else
        {
            angles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(angles), 0.25f * Time.deltaTime);
    }
    void BalancingPlane()
    {
        angles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        CentreOfMass = Vector3.Lerp(CentreOfMass, Vector3.zero, 0.15f * Time.deltaTime);
        GetComponent<Rigidbody>().centerOfMass = CentreOfMass;
    }
    void TiltingPlane()
    {
        rotationFactor = CentreOfMass - transform.position;
        float rotX = Mathf.Clamp(rotationFactor.x * 10f, -10f, 10f);
        float rotY = Mathf.Clamp(rotationFactor.y * 10f, -10f, 10f);
        float rotZ = Mathf.Clamp(rotationFactor.z * 10f, -10f, 10f);
        _comAngles = new Vector3(rotZ, -rotY, -rotX);
        angles = transform.rotation.eulerAngles + _comAngles;
    }
    IEnumerator timer()
    {
        while (true)
        {
            doIt = false;
            yield return new WaitForSeconds(3f);
            doIt = true;
            yield return new WaitForSeconds(11f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(CentreOfMass, 1f);
    }
    //Calculate Centre of mass
    public void Calculate(List<Transform> obj)
    {
        foreach (Transform tr in obj)
        {
            sumX = sumX + (tr.position.x * tr.GetComponent<Rigidbody>().mass);
            sumY = sumY + (tr.position.y * tr.GetComponent<Rigidbody>().mass);
            sumZ = sumZ + (tr.position.z * tr.GetComponent<Rigidbody>().mass);
            totalMass += tr.GetComponent<Rigidbody>().mass;
        }
        val_X = sumX / totalMass;
        val_Y = sumY / totalMass;
        val_Z = sumZ / totalMass;
        CentreOfMass = new Vector3(val_X, val_Y, val_Z);
        GetComponent<Rigidbody>().centerOfMass = CentreOfMass;  
    }
}
