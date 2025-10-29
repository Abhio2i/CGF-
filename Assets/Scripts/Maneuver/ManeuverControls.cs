using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Vazgriz.Plane;

// Manuever performing scripts 



public enum Maneuvers
{
    highspeedgunjink = 15, lowspeedgunjink =14,cobra=13, scissor=12, extension=11, barrelRoll=10,purePursuit = 9,lagPursuit=8,leadPursuit=7,highYoyo=6,lowYoyo=5,hardTurn=4,breakTurn=3,split_s=2,immelmann=1,none=0
}


public class ManeuverControls : MonoBehaviour
{
    
    public Maneuvers maneuver;


    // The object whose rotation we want to match.
    public Vector3 yoyo_target;
    public Vector3 hardTurn_target;
    public Vector3 breakTurn_Target;

    public List<Vector3> split_S_Target;
    public List<Vector3> immelmann_Target;

    // Angular speed in degrees per sec.
    public float speed;
    public bool stage1 =true,stage2=false, stage3=false;
    float factor=1;


    [SerializeField] Vector3 cobraAngle;
    [SerializeField] List<Vector3> scissorRoll;
    [SerializeField] List<Vector3> extensionRoll;
    [SerializeField] List<Vector2> scissorFactor;
    [SerializeField] List<Vector2> extensionFactor;

    [SerializeField] List<Vector3> barrelRoll;


    Vector3 targetAngle;

    void RotateFunction(float rotateSpeed, Vector3 target, float speed)
    {
        var step = rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(target), step);
    }
    Vector3 temp;

    void Stable()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));   
    }
    public float direction;
    public void Perform()
    {
        GetComponent<Rigidbody>().velocity = 190 * transform.forward * factor;
        if (maneuver == Maneuvers.breakTurn)
            factor = 0.1f;
        else
            factor = 1f;
        
        switch(maneuver)
        {
            case Maneuvers.highYoyo:
                if (initial)
                {
                    Stable();
                    temp = transform.rotation.eulerAngles + yoyo_target;
                    temp.y *= direction;
                    initial = false;
                }
                RotateFunction(10, temp, speed);
                CompareStage(temp);
                break;
            case Maneuvers.lowYoyo:
                {
                    if (initial)
                    {
                        Stable();
                        temp = transform.rotation.eulerAngles + yoyo_target;
                        temp.y *= direction;
                        initial = false;
                    }
                    RotateFunction(50, temp, speed);
                    CompareStage(temp);
                }
                break;
            case Maneuvers.split_s:
                StageRotation(split_S_Target[0], split_S_Target[1], 280, 10);
                break;
            case Maneuvers.breakTurn:
                {
                    if (initial)
                    {
                        Stable();
                        temp = transform.rotation.eulerAngles + breakTurn_Target;
                        initial = false;
                    }
                    RotateFunction(280, temp, speed);
                    CompareStage(temp);
                }
                break;
            case Maneuvers.hardTurn:
                {
                    if (initial)
                    {
                        Stable();
                        temp = transform.rotation.eulerAngles + hardTurn_target;
                        initial = false;
                    }
                    RotateFunction(80, temp, speed);
                    CompareStage(temp);
                }
                break;
            case Maneuvers.immelmann:
                {
                    StageRotation(immelmann_Target[0], immelmann_Target[1], 10, 280);
                }
                break;
            case Maneuvers.cobra:
                {
                    if (initial)
                    {
                        StartCoroutine(Cobra());
                        initial = false;
                    }
                }
                break;
            case Maneuvers.scissor:
                {
                    if (initial)
                    {
                        StartCoroutine(_ScissorRoll2());
                        initial = false;
                    }
                }
                break;
            case Maneuvers.extension:
                {
                    if (initial)
                    {
                        StartCoroutine(Extension(extensionRoll, extensionFactor));
                        initial = false;
                    }
                }
                break;
             case Maneuvers.barrelRoll:
                {
                    if (initial)
                    {
                        StartCoroutine(_BarrellRoll2());
                        initial = false;
                    }
                }
                break;
            case Maneuvers.none:
                {
                    stage1 = initial=true;
                    stage2 = false;
                }
                break;
            default:
                stage1 = initial = true;
                stage2 = false;
                GetComponent<DicisonTree>().currentManeuver = Maneuvers.none;
                StopAllCoroutines();
                break;

        }
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    void CompareStage(Vector3 target)
    {
        if (transform.rotation == Quaternion.Euler(target))
        {
            GetComponent<DicisonTree>().currentManeuver = Maneuvers.none;
            maneuver = Maneuvers.none;
            initial = true;
            stage1 = true; stage2 = false;
            // StartCoroutine(GetComponent<DicisonTree>().ManeuverIntervals());
        }
            //maneuver=_Maneuvers.none;
    }
    public bool initial=true;
    Vector3 temp1, temp2;
    void StageRotation(Vector3 firstStage,Vector3 secondStage,float firstStageRate,float secondStageRate)
    {
        if(initial)
        {
            temp1 = firstStage+transform.rotation.eulerAngles;
            temp2 = secondStage+transform.rotation.eulerAngles;
            initial = false;
        }
        if (stage1)
        {
            RotateFunction(firstStageRate, temp1, speed);

            if (transform.rotation == Quaternion.Euler(temp1))
            {
                stage1 = false; stage2 = true; //initial=true;
            }
        }
        else if (stage2)
        {
            RotateFunction(secondStageRate, temp2, speed);
            CompareStage(temp2);
        }
    }
    
        int index;
    Vector3 Split_s_fun()
    {
        if (index >= split_S_Target.Count)
        {
            Vector3 target = split_S_Target[index];
            if (transform.rotation.eulerAngles == target)
                index++;
            return target;
        }
        return Vector3.zero;
    }
    /*
    void RotateFunction(float rotateSpeed,Vector3 target,float speed)
    {
        // The step size is equal to speed times frame time.
        var step = rotateSpeed * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(target), step);
    }*/


    bool isRotation = true;
    IEnumerator Cobra()
    {
        targetAngle = transform.rotation.eulerAngles + cobraAngle;
        Vector3 initialAngle = transform.rotation.eulerAngles;
        Quaternion startRot = transform.rotation;
        isRotation = true;
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY| RigidbodyConstraints.FreezePositionX;
        factor = 0.0f;
        GetComponent<Rigidbody>().isKinematic = true;
        float duration = 0;
        bool rot = true;
        while (isRotation)
        {
            if (rot)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetAngle), 10f * Time.fixedDeltaTime);
            }
            

            Vector3 forwardDirection = startRot * Vector3.forward;
            transform.position += forwardDirection * 1f * 10f * Time.fixedDeltaTime;
            yield return null;

            if (transform.rotation == Quaternion.Euler(targetAngle))
            {
                duration += Time.deltaTime;
                if(duration > 1f)
                {
                    isRotation = false;
                }
            }
        }

        //yield return new WaitForSeconds(0.5f);
        targetAngle = transform.rotation.eulerAngles - cobraAngle;
        while (!isRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(initialAngle), 15f * Time.fixedDeltaTime);
            factor = 0.01f;
            Vector3 forwardDirection = startRot * Vector3.forward;
            transform.position += forwardDirection * 1f * 10f * Time.fixedDeltaTime;
            yield return null;
            if (transform.rotation == Quaternion.Euler(initialAngle))
            {
                isRotation = true;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().velocity = transform.forward * 100f;
                factor = 1f;
                StopCoroutine(Cobra());
                initial = true;
                maneuver = Maneuvers.none;
            }
        }
    }

   
    //[Range(-1, 1)]
    //[SerializeField] float rollInput = 1f;
    //float rollAngle = 50f;
    //Vector3 angle;
    //IEnumerator _ScissorRoll()
    //{

    //    Vector3 initialAngle = transform.rotation.eulerAngles;
    //    float _angle = rollAngle;
    //    float duration = 0;

    //    float temp = 8f;
    //    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    //    Quaternion reset = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -rollInput * rollAngle));
    //    while (transform.rotation != reset)
    //    {
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, reset, 120 * Time.fixedDeltaTime);
    //        yield return null;
    //    }

    //    while (duration < 5)
    //    {
    //        GetComponent<Rigidbody>().freezeRotation = true;
    //        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    //        angle = initialAngle + ((Vector3.up) * rollInput * rollAngle);
    //        Quaternion roll = Quaternion.Euler(angle);
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, roll, temp * Time.fixedDeltaTime);
    //        if (transform.rotation == roll)
    //        {
    //            rollInput *= -1;
    //            temp += 8f;
    //            temp = Mathf.Clamp(temp, 5, _angle);
    //            reset = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -rollInput * rollAngle));
    //            while (transform.rotation != reset)
    //            {
    //                transform.rotation = Quaternion.RotateTowards(transform.rotation, reset, 120 * Time.fixedDeltaTime);
    //                yield return null;
    //            }
    //            yield return new WaitForSeconds(0.1f);

    //        }
    //        duration += Time.deltaTime;
    //        yield return null;
    //        if (duration > 5)
    //        {
    //            maneuver = Maneuvers.none;
    //            initial = true;
    //            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    //            StopCoroutine(_ScissorRoll());
    //        }
    //    }
    //}


    public float forward = 80f;
    public float rotspeed = 0.3f;
    public float angle1 = 80f;
    public float angle2 = 170f;
    public float direction2 = 1f;
    public float scissortime = 5f;
    public float ang = 0;
    IEnumerator _ScissorRoll2()
    {

        stage1 = true;
        stage2 = false;
        float duration = 0;
        GetComponent<Rigidbody>().isKinematic = true;
        while (stage1)
        {
            float s = direction2 * angle1 * rotspeed * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angle1)
            {
                ang = 0;
                direction2 *= -1;
                stage1 = false;
                stage2 = true;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.up, direction2 * angle1 * rotspeed * Time.deltaTime, Space.World);
            duration += Time.deltaTime;
            if (duration > scissortime)
            {
                stage1 = false;
                stage2 = false;
            }
            yield return null;
        }
        ang = 0;
        while (stage2)
        {
            float s = direction2 * angle2 * rotspeed * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angle2)
            {
                ang = 0;
                direction2 *= -1;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.up, direction2 * angle2 * rotspeed * Time.deltaTime, Space.World);
            duration +=Time.deltaTime;
            if(duration > scissortime)
            {
                stage2 = false;
            }
            yield return null;
        }
        initial = true;
        maneuver = Maneuvers.none;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 190f;
        direction2 = 1f;
        ang = 0;
        StopCoroutine(_ScissorRoll2());
    }
        IEnumerator Extension(List<Vector3> maneuverRoll, List<Vector2> maneuverFactor)
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
                maneuver = Maneuvers.none;
                initial = true;
                StopCoroutine(Extension(maneuverRoll, maneuverFactor));
            }

        }
    }
    //public float rollSpeed = 30f;
    //public float rollDuration = 1f;
    //public Vector3 rollDirection = Vector3.right;
    //bool isRolling;
    //IEnumerator Roll()
    //{
    //    isRolling = true;
    //    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    //    float t = 0f;
    //    while (t < rollDuration)
    //    {
    //        //float rollAngle = rollSpeed * Time.deltaTime;
    //        Vector3 torque = rollDirection * 0.1f;
    //        GetComponent<Rigidbody>().AddTorque(transform.InverseTransformDirection(torque), ForceMode.Acceleration);
    //        factor = 1.9f;
    //        t += Time.deltaTime;
    //        yield return null;
    //    }
    //    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    //    initial = true;
    //    maneuver = Maneuvers.none;
    //   // GetComponent<DicisonTree>().isManeuvering = true;
    //    isRolling = false;
    //}


    public float rotspeedB = 0.3f;
    public float angleB1 = 80f;
    public float angleB2 = 170f;
    public float angleB3 = 80f;

    IEnumerator _BarrellRoll2()
    {

        stage1 = true;
        stage2 = false;
        stage3 = false;
        ang = 0;
        Vector3 initangle = transform.localEulerAngles;
        float duration = 0;
        GetComponent<Rigidbody>().isKinematic = true;
        while (stage1)
        {
            float s = angleB1 * rotspeedB * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angleB1)
            {
                ang = 0;
                stage1 = false;
                stage2 = true;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.up, angleB1 * rotspeedB * Time.deltaTime, Space.World);
            yield return null;
        }
        ang = 0;

        while (stage2)
        {
            float s = angleB2 / 2f * rotspeedB / 2f * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angleB2)
            {
                ang = 0;
                stage2 = false;
                stage3 = true;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.right - transform.forward / 2f, -angleB2 / 2f * rotspeedB / 2f * Time.deltaTime, Space.World);
            yield return null;
        }

        ang = 0;
        while (stage3)
        {
            float s = angleB3 * rotspeedB * 2f * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angleB3)
            {
                ang = 0;
                stage3 = false;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.up, angleB3 * -rotspeedB * 2f * Time.deltaTime, Space.World);
            duration += Time.deltaTime;
            yield return null;
        }
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        initial = true;
        maneuver = Maneuvers.none;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 190f;
        ang = 0;
        StopCoroutine(_BarrellRoll2());
    }
}
