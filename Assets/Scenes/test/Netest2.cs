using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum maneuver
{
    none,cobra,scissor,extension,barrelRoll
}
public class Netest2 : MonoBehaviour
{
    float speed = 100f;
    float factor = 1;


    [SerializeField]Vector3 cobraAngle;
    [SerializeField] List<Vector3> scissorRoll;
    [SerializeField] List<Vector3> extensionRoll;
    [SerializeField] List<Vector2> scissorFactor;
    [SerializeField] List<Vector2> extensionFactor;

    [SerializeField] List<Vector3> barrelRoll;

    [SerializeField] maneuver maneuver;

    Vector3 targetAngle;

    public bool create=false;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward*factor ;
        if(create)
        {
            //StartCoroutine(ScissorRoll(scissorRoll,scissorFactor));
            if (maneuver == maneuver.cobra)
            {
                StartCoroutine(Cobra());
            }
            if (maneuver == maneuver.scissor)
            {
                StartCoroutine(_ScissorRoll());
            }
            if(maneuver==maneuver.extension)
            {
                StartCoroutine(Extension(extensionRoll, extensionFactor));
            }
            if(maneuver==maneuver.barrelRoll)
            {
                StartCoroutine(Roll());
            }
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(targetAngle),100f*Time.fixedDeltaTime);
            factor = 0.0f;
            GetComponent<Rigidbody>().isKinematic = true;
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(initialAngle), 240f * Time.fixedDeltaTime);
            factor = 0.01f;
            yield return null;
            if (transform.rotation == Quaternion.Euler(initialAngle))
            {
                isRotation = true;
                GetComponent<Rigidbody>().isKinematic = false;
                factor = 1f;
                StopCoroutine(Cobra());
                maneuver = maneuver.none;
            }
        }
    }
    int index;
    [Range(-1,1)]
    [SerializeField]float rollInput=1f;
    float rollAngle = 50f;
    Vector3 angle;
    IEnumerator _ScissorRoll()
    {
        Vector3 initialAngle = transform.rotation.eulerAngles;
        float _angle = rollAngle;
        float duration = 0;
        float temp = 8f;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Quaternion reset = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -rollInput * rollAngle));
        while (transform.rotation != reset)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, reset, 120 * Time.fixedDeltaTime);
            yield return null;
        }
        
        while (duration < 5)
        {
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            angle = initialAngle + ((Vector3.up) * rollInput * rollAngle);
            Quaternion roll = Quaternion.Euler(angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, roll, temp * Time.fixedDeltaTime) ;
            if(transform.rotation==roll)
            {
                rollInput *= -1;
                temp += 8f;
                temp = Mathf.Clamp(temp, 5 ,_angle);
                reset = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -rollInput * rollAngle));
                while (transform.rotation!=reset)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, reset, 120 * Time.fixedDeltaTime);
                    yield return null;
                }
                yield return new WaitForSeconds(0.1f);
               
            }
            duration += Time.deltaTime;
            yield return null;
            if(duration>5)
            {
                maneuver = maneuver.none;
                StopCoroutine(_ScissorRoll());
            }
        }
    }
    IEnumerator Extension(List<Vector3>maneuverRoll,List<Vector2>maneuverFactor)
    {
        Vector3 initialAngle = transform.rotation.eulerAngles;
        isRotation = true;
        factor = 1;
        while (isRotation)
        {
            targetAngle = initialAngle + maneuverRoll[0];
            float rotationAngle = maneuverFactor[0].x;
            float duration = maneuverFactor[0].y;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetAngle), rotationAngle * Time.fixedDeltaTime);
            yield return null;
            if (transform.rotation == Quaternion.Euler(targetAngle))
            {
                factor = 1.5f;
                yield return new WaitForSeconds(duration);
                maneuver = maneuver.none;
                StopCoroutine(Extension(maneuverRoll, maneuverFactor));
            }
            
        }
    }
    public float rollSpeed = 30f;
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
            Vector3 torque = rollDirection * 0.1f;
            GetComponent<Rigidbody>().AddTorque(torque,ForceMode.Acceleration);
            factor = 1.9f;
            t += Time.deltaTime;
            yield return null;
        }
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        maneuver = maneuver.none;
        isRolling = false;
    }

   
}
    

