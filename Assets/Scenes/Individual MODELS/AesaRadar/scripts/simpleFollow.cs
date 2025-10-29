using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleFollow : MonoBehaviour
{
    public Transform Target;
    public float TurnSpeed = 5f;
    public float Speed = 5f;
    public float AutoBlastRange = 5f;

    public float timer = 2f;
    public bool fire = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(time());
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.forward * 100f;
        if (Target != null && fire)
        {
            pos = Target.position;

        }



        if (fire && Vector3.Distance(pos, transform.position) > 0f)
        {
            var targetRotation = Quaternion.LookRotation(pos - transform.position);

            // Smoothly rotate towards the target point.
            //transform.rotation = Quaternion.LerpUnclamped(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);

            //transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
            transform.position += transform.forward * Speed * Time.deltaTime;

            if (Vector3.Distance(pos, transform.position) < AutoBlastRange && transform.GetComponent<MissileHeatSystem>() != null)
            {
                transform.GetComponent<MissileHeatSystem>().Temprature = transform.GetComponent<MissileHeatSystem>().MaxTemprature + 1;
            }
        }

    }

    IEnumerator time()
    {
        yield return new WaitForSeconds(timer);
        fire = true;
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(100, 100, 100));
    }
}
