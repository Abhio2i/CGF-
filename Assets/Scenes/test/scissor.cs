using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class scissor : MonoBehaviour
{
    //public float forward = 10f;
    //public float rotspeed = 10f;
    //public float angle = 30f;
    //public float direction = 1f;
    //public float ang = 0;
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    var s = direction * angle * rotspeed * Time.deltaTime;
    //    s = s < 0 ? -s : s;
    //    ang += s;

    //    if (ang > angle)
    //    {
    //        ang = 0;
    //        direction *= -1;
    //    }
    //    transform.position += transform.forward * forward*Time.deltaTime;
    //    transform.Rotate(transform.up, direction* angle * rotspeed * Time.deltaTime,Space.Self);

    //}

    private void Start()
    {
        StartCoroutine(_BarrellRoll2());
    }


    public float forward = 80f;
    public float rotspeed = 0.3f;
    public float angle1 = 80f;
    public float angle2 = 170f;
    public float angle3 = 80f;
    public float ang = 0;
    public bool stage1,stage2,stage3;
    IEnumerator _BarrellRoll2()
    {

        stage1 = true;
        stage2 = false;
        stage3 = false;
        Vector3 initangle = transform.localEulerAngles;
        float duration = 0;
        GetComponent<Rigidbody>().isKinematic = true;
        while (stage1)
        {
            float s =  angle1 * rotspeed * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angle1)
            {
                ang = 0;
                stage1 = false;
                stage2 = true;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.up,  angle1 * rotspeed * Time.deltaTime,Space.World);
            yield return null;
        }
        ang = 0;
       
        while (stage2)
        {
            float s = angle2/2f * rotspeed/2f * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angle2)
            {
                ang = 0;
                stage2=false;
                stage3 = true;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.right-transform.forward/2f,  -angle2/2f * rotspeed/2f * Time.deltaTime, Space.World);
            duration += Time.deltaTime;
            yield return null;
        }

        ang = 0;
        while (stage3)
        {
            float s =  angle3 * rotspeed*2f * Time.deltaTime;
            s = s < 0 ? -s : s;
            ang += s;

            if (ang > angle3)
            {
                ang = 0;
                stage3 = false;
            }
            transform.position += transform.forward * forward * Time.deltaTime;
            transform.Rotate(transform.up,  angle3 * -rotspeed*2f * Time.deltaTime, Space.World);
            duration += Time.deltaTime;
            yield return null;
        }

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = transform.forward * 190f;
        ang = 0;
        StopCoroutine(_BarrellRoll2());
    }
}
