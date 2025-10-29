using AirPlane.Radar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Aeroplane;

public class Brain : MonoBehaviour
{
    public float Distance = 0;
    public float Angle = 0;
    public Transform DummyTarget;
    public Vector3 TargetOffset = Vector3.zero;
    public Transform FormationTargetPlane;
    public Transform WaypointTarget;
    public CombineUttam combineUttam;
    public AeroplaneAiControl aeroplaneAiControl;
    public ManualController manualController;
    public WaypointProgressTracker waypointProgressTracker;
    public SilantroGun silantroGun;
    public Transform barrelA;
    public Transform barrelB;
    public bool fire = false;
    public bool Formation = false;
    public bool WaypointFollow = false;
    public bool KnowMissed = false;
    public bool TargetMissed = false;
    public bool Return = false;
    public Vector3 LastPosition = Vector3.zero;
    public Vector3 ReturnPosition = Vector3.zero;
 
    // Start is called before the first frame update
    void Start()
    {
        if (Formation)
        {
            //manualController.MaxEnginePower += 20;
            aeroplaneAiControl.m_Target = FormationTargetPlane;
            waypointProgressTracker.enabled = false;
            manualController.TargetFollow = false;
        }
        DummyTarget.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            ///
            if (combineUttam.Target != null)
            {
                float angle = 10;
                barrelA.LookAt(combineUttam.Target.position);
                float x = barrelA.localEulerAngles.x;
                x = x > 180 ? (x - 360) : x;
                x = x > angle ? angle : (x < -angle ? -angle : x);
                float y = barrelA.localEulerAngles.y;
                y = y > 180 ? (y - 360) : y;
                y = y > angle ? angle : (y < -angle ? -angle : y);
                barrelA.localEulerAngles = new Vector3(x, y, 0f);
                barrelB.LookAt(combineUttam.Target.position);
                x = barrelB.localEulerAngles.x;
                x = x > 180 ? (x - 360) : x;
                x = x > angle ? angle : (x < -angle ? -angle : x);
                y = barrelB.localEulerAngles.y;
                y = y > 180 ? (y - 360) : y;
                y = y > angle ? angle : (y < -angle ? -angle : y);
                barrelB.localEulerAngles = new Vector3(x, y, 0f);
            }
            else
            {
                barrelA.localEulerAngles = Vector3.zero;
                barrelB.localEulerAngles = Vector3.zero;
            }
            //silantroGun.Fire();    
        }

        if (combineUttam.Target == null )
        {
            fire = false;
            if (!KnowMissed && !WaypointFollow)
            {
                KnowMissed=true;
                TargetMissed=true;
                Return = false;
                LastPosition = FormationTargetPlane.position;

                float Distance = Vector3.Distance(LastPosition, transform.position);
                if (Distance > 200f)
                {
                    DummyTarget.eulerAngles = new Vector3(0, FormationTargetPlane.eulerAngles.y, 0);
                    TargetOffset = FormationTargetPlane.forward * 1000;
                    DummyTarget.position = FormationTargetPlane.position + TargetOffset;
                }
                else
                {
                    DummyTarget.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    TargetOffset = DummyTarget.forward * 1000;
                    DummyTarget.position = transform.position + TargetOffset;
                }

                
                
                ReturnPosition = DummyTarget.position;
            }
        }
        else
        {
            WaypointFollow = false;
            KnowMissed =false;
            TargetMissed = false;
            Return = false;
            FormationTargetPlane = combineUttam.Target;
            waypointProgressTracker.enabled = false;
            aeroplaneAiControl.m_Target = DummyTarget;
            manualController.TargetFollow = true;
            DummyTarget.position = FormationTargetPlane.position + TargetOffset;
            DummyTarget.eulerAngles = new Vector3(0, FormationTargetPlane.eulerAngles.y, 0);
            TargetOffset = DummyTarget.right * -18 + FormationTargetPlane.forward * 500f;

            var distance = Vector3.Distance(combineUttam.Target.position, transform.position);
            var lockAngle = Vector3.Angle(combineUttam.Target.position - transform.position, transform.forward);
            if (lockAngle < 10f && distance < 3000)
            {
                fire = true;
            }
            else
            {
                fire = false;
            }


        }


        manualController.Formation = WaypointFollow;
        if (!WaypointFollow)
        {
            FormationTargetPlane = combineUttam.Target;
        }

        if (TargetMissed && !WaypointFollow)
        {
            if (!Return)
            {
                DummyTarget.position = ReturnPosition;
                float Distance = Vector3.Distance(DummyTarget.position, transform.position);
                if(Distance <100f)
                {
                    Return = true;
                }
            }
            else
            {
                DummyTarget.position = LastPosition;
                float Distance = Vector3.Distance(DummyTarget.position, transform.position);
                if (Distance < 100f)
                {
                    Return = false;
                }
            }
        }

        if (DummyTarget.position.y < 400)
        {
            DummyTarget.position = new Vector3(DummyTarget.position.x, 400, DummyTarget.position.z);
        }

        /*if (manualController.Reset)
        {
            TargetOffset = DummyTarget.forward * 1000;
        }
        else
        {
            TargetOffset = DummyTarget.right * -18;
        }*/
        //DummyTarget.rotation = FormationTargetPlane.rotation;

        /*
        if(combineUttam.Target != null)
        {
            Distance = Vector3.Distance(combineUttam.Target.position,transform.position);
            Angle = Vector3.Angle(combineUttam.Target.position - combineUttam.Radar.transform.position, transform.forward);
            if (Distance < 10000 )
            {
                aeroplaneAiControl.m_Target = combineUttam.Target;
                manualController.enabled = true;
            }
            else
            {
                
                if(Formation && FormationTargetPlane!=null && FormationTargetPlane.gameObject.activeSelf)
                {
                    aeroplaneAiControl.m_Target = FormationTargetPlane;
                    manualController.enabled = true;
                }
                else
                {
                    aeroplaneAiControl.m_Target = WaypointTarget;
                    aeroplaneAiControl.Activate = true;
                    aeroplaneAiControl.throttleInput = 0.5f;
                    aeroplaneAiControl.m_brake = false;
                    manualController.enabled = false;
                }
                
            }
        }
        else
        {
            Distance = -1;
            Angle = -1;
            if (Formation && FormationTargetPlane != null && FormationTargetPlane.gameObject.activeSelf)
            {
                aeroplaneAiControl.m_Target = FormationTargetPlane;
                manualController.enabled = true;
            }
            else
            {
                aeroplaneAiControl.m_Target = WaypointTarget;
                aeroplaneAiControl.Activate = true;
                aeroplaneAiControl.throttleInput = 0.5f;
                aeroplaneAiControl.m_brake = false;
                manualController.enabled = false;
            }

        }
        */
    }
}
