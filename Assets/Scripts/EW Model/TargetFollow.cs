using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EW.Flare;

public class TargetFollow : MonoBehaviour
{
    public Transform Target;
    public float TurnSpeed = 5f;
    public float Speed = 120f;
    public float AutoBlastRange = 5f;
    public float forwardFactor, upwardFactor;
    public GameObject Interpolator;

    private void Start()
    {
        if (Target == null)
        {
            Target = GameObject.Find("TestPlayer(Clone)").transform;
        }
    }
    void Update()
    {
        if (Target != null)
        {
            Vector3 targetDir = Target.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);  //Infront of the missile means <90
            var distance = Vector3.Distance(transform.position, Target.transform.position);

            if (transform.GetComponent<MissileHeatSystem>().Blinded && !Target.gameObject.name.Contains("Flare"))   //Deflacted
            {
                var Interpolatedpos = transform.position + Vector3.down*20 + transform.forward *80;
                var Rot = Quaternion.LookRotation(Interpolatedpos - transform.position);
                transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Rot, TurnSpeed * Time.deltaTime);
                transform.position += (transform.forward * Speed * Time.deltaTime);
            }
            else if (angle < 70)    //In Radar Range of Missile
            {
                if (distance > 4000)    //Very Far from Target
                {
                    var Interpolatedpos = Target.transform.position + (Target.transform.forward * Target.GetComponent<Rigidbody>().velocity.magnitude * forwardFactor * distance) + (Vector3.up * upwardFactor * distance);
                    //Interpolator.transform.position = Interpolatedpos;
                    var Rot = Quaternion.LookRotation(Interpolatedpos - transform.position);
                    transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Rot, TurnSpeed * Time.deltaTime);
                    transform.position += transform.forward * Speed * Time.deltaTime;
                }
                else       //Close to Target
                {
                    var Rot = Quaternion.LookRotation(Target.transform.position - transform.position);
                    transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Rot, TurnSpeed * Time.deltaTime);
                    transform.position += transform.forward * Speed * Time.deltaTime;
                }
            }   //No Target Visible
            else
                transform.position += transform.forward * Speed * Time.deltaTime;

            if (Vector3.Distance(Target.position, transform.position) < AutoBlastRange)
            {
                Target.gameObject.GetComponent<Rigidbody>().useGravity = true;
                transform.GetComponent<MissileHeatSystem>().destroyMissile();
            }
        }
        else    //No Target Assigned
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
    }
    public void FlaresActivated(Transform[] flares)
    {
        int i = Random.Range(0, flares.Length + 1);
        if(i == flares.Length)
        {
            return;
        }
        else
        {
            Target = flares[i];
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(50, 5, 50));
    }
    private void OnDestroy()
    {
        if (Target.gameObject.name.Contains("Flare")) ;
        GameObject.Find("TestCamera").transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Flares Destroyed " + gameObject.name.Substring(0, gameObject.name.Length - 7);
    }
}
