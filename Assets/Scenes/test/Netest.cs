using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Netest : MonoBehaviour
{
    float speed = 100f;
    float factor = 1;


    [SerializeField]Vector3 cobraAngle;
    [SerializeField] List<Vector3> scissorRoll;
    [SerializeField] List<Vector2> scissorFactor;

    [SerializeField] List<Vector3> barrelRoll;

    Vector3 targetAngle;

    public bool create;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward*factor ;
        if(create)
        {
            StartCoroutine(Roll());
            create = false;
        }
    }
    bool isRotation=true;
    IEnumerator Cobra()
    {
        targetAngle = transform.rotation.eulerAngles + cobraAngle;
        Vector3 initialAngle = transform.rotation.eulerAngles;
        isRotation = true;
        while (isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(targetAngle),60f*Time.fixedDeltaTime);
            factor = 0.01f;
            yield return null;
            if (transform.rotation == Quaternion.Euler(targetAngle))
            {
                isRotation = false;
            }
        }
        yield return new WaitForSeconds(0.3f);
        targetAngle = transform.rotation.eulerAngles - cobraAngle;
        while(!isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(initialAngle), 120f * Time.fixedDeltaTime);
            factor = 0.01f;
            yield return null;
            if (transform.rotation == Quaternion.Euler(initialAngle))
            {
                isRotation = true;
                StopCoroutine(Cobra());
            }
        }
    }
    int index;
    IEnumerator ScissorRoll()
    {
        Vector3 initialAngle = transform.rotation.eulerAngles;
        isRotation = true;
        while (isRotation)
        {
            targetAngle = initialAngle + scissorRoll[index];
            float rotationAngle = scissorFactor[index].x;
            float duration = scissorFactor[index].y;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetAngle), rotationAngle * Time.fixedDeltaTime);
            factor = 1.2f;
            yield return null;
            if (transform.rotation == Quaternion.Euler(targetAngle))
            {
                yield return new WaitForSeconds(duration);
                index++;
            }
            if(index==scissorRoll.Count)
            {
                factor = 1f;
                isRotation = false;
                index = 0;
                StopCoroutine(ScissorRoll());
            }
        }
    }
    public float rollSpeed = 100f;
    public float rollDuration = 1f;
    public Vector3 rollDirection = Vector3.right;
    bool isRolling;
    IEnumerator Roll()
    {
        isRolling = true;

        float t = 0f;
        while (t < rollDuration)
        {
            float rollAngle = rollSpeed * Time.deltaTime;
            Vector3 torque = rollDirection * rollAngle;
            GetComponent<Rigidbody>().AddTorque(torque,ForceMode.Acceleration);
            factor = 1.9f;
            t += Time.deltaTime;
            Debug.Log(t);
            yield return null;
        }
        Debug.Log("CLOSE");
        isRolling = false;
    }
}
    

