using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Newtonsoft.Json.Serialization;
using System.Net.NetworkInformation;

public class WaypointsFollow : MonoBehaviour
{
    //Univerasal
    public List<Vector3> Waypoint;
    public bool test, contourFlying;
    public Vector3 error;
    public float rollDistance, pitchDistance;
    private Vector3 errorPositive;
    public float contourFlyingRange = 250, SlowManueverRange = 250, follow_distance = 500, Offset = 1000;

    //For Pitch
    //public Vector3 pitchdirection;
    public float pitch, pitchLerp = 1.5f, pitchP = 0.00001f, pitchI = 0.1f, pitchD = 0, pitchAngleLerp = 0.07f; //pitchClamper = 1f maxPitch = 75,
    public float currentPitchAngle, pitchFactor, pitchDelta, pitchReducer, targetPitchAngle, pitchRate, prePitch, preY;

    //For Roll
    public float roll, rollLerp = 0.01f, rollP = 0.2f, rollI = 0.1f, rollD = 10, rollAngleLerp = 0.07f;//maxRoll = 110, rollClamper = 1f, LerpRoll = 0.02f;
    public float currentRollAngle, targetRollAngle, rollDelta, rollReducer, preX, PreRoll, rollRate;

    //Throttle
    public float fixedSpeed = 250;
    //private float currentRequiredSpeed=0;
    //public float throttle, P = 0.4f, minThrottle = 0.2f, D = 60000;
    //public float pGain, dGain, throttleOffset;

    //Temprory Variables
    public Vector3 currentWay;
    private Vector3 flatForward, localFlatForward, flatRight, localFlatRight, dif;
    private SilantroController controller;
    private float vel, x, deltaX, deltaY, mRoll, mPitch;
    private Vector2 safedist;
    private Transform ai;
    private Rigidbody rb;
    public int index=0;
    void Start()
    {
        controller = GetComponent<SilantroController>();
        //Invoke("Initialization", 2f);
        rb = GetComponent<Rigidbody>();
        var G = new GameObject("AI");
        G.transform.parent = this.transform;
        ai = G.transform;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 w in Waypoint)
        {
            Gizmos.DrawSphere(w, 350);
        }
    }
    private void OnEnable()
    {
        if (!test)
        {
            Waypoint = Save_Waypoints.waypoints;
            if (Waypoint.Count < 1) { this.enabled = false; }
        }
        GetComponent<SilantroController>().inputType = SilantroController.InputType.AI;
        FirstWay();
    }
    private void OnDisable()
    {
        GetComponent<SilantroController>().inputType = SilantroController.InputType.Default;
    }
    void FixedUpdate()
    {
        if (Waypoint == null) { return; }
        #region Initialization
        //if (!invoke)
        //if(Leader.m_wowForce > 1)
        //{
        //    controller.TurnOnEngines();
        //}
        error = Vector3.zero;
        pitch = 0;
        roll = 0;
        #endregion
        currentWay = Waypoint[index];
        if (contourFlying)
        {
            currentWay.y = 0 - SlowManueverRange + contourFlyingRange + 50;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.down * 5, Vector3.down, out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    currentWay.y = transform.position.y - hit.distance - SlowManueverRange + contourFlyingRange + 50;
                }
            }
        }
        WayPointSelector();
        error = currentWay - rb.position;
        ai.rotation = Quaternion.Euler(ai.eulerAngles.x, ai.eulerAngles.y, 0);
        rollDistance = (Quaternion.Inverse(ai.rotation) * error).x;
        //ai.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
        //pitchDistance = currentWay.y - transform.position.y;//(Quaternion.Inverse(ai.rotation) * error).x;
        //pitchDistance = pitchDistance > 0 ? pitchDistance : -pitchDistance;
        ai.rotation = transform.rotation;
        error = Quaternion.Inverse(rb.rotation) * error;
        if (error.z < 0) { FirstWay(); }
        dif = currentWay - transform.position;
        rollDistance = new Vector2(dif.x, dif.z).magnitude - (error.z > 0 ? error.z : -error.z);
        rollDistance = rollDistance > 0 ? rollDistance : -rollDistance;
        pitchDistance = dif.y > 0 ? dif.y : -dif.y;
        error = new Vector3(error.x, error.y, error.z - follow_distance);       // Distance of follow position from it's local axis
        errorPositive = new Vector3(error.x > 0 ? error.x : -error.x, error.y > 0 ? error.y : -error.y, error.z > 0 ? error.z : -error.z);
        safedist = error;   //Vector2

        //Pitch Angle of This Aircraft
        flatForward = transform.forward;
        flatForward.y = 0;
        flatForward.Normalize();
        localFlatForward = transform.InverseTransformDirection(flatForward);
        currentPitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * 60;

        //Rotation of This Aircraft
        flatRight = Vector3.Cross(Vector3.up, transform.forward);
        localFlatRight = transform.InverseTransformDirection(flatRight);
        currentRollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * 60;

        //Pitch
        PitchAngleCalculate();

        //Roll
        RollAngleCalulate();

        //Throttle
        ThrottleSetter();
    }
    void PitchAngleCalculate()
    {
        if (safedist.magnitude < SlowManueverRange || (pitchDistance < SlowManueverRange && (currentRollAngle > 0 ? currentRollAngle : -currentRollAngle) < 30))
        {
            targetPitchAngle = Mathf.Lerp(targetPitchAngle, -currentPitchAngle, pitchAngleLerp);
        }
        else
        {
            if (error.y < 0) { targetPitchAngle = 360 - (Mathf.Atan2(error.y, error.z) * Mathf.Rad2Deg * -1); }
            else { targetPitchAngle = Mathf.Atan2(error.y, error.z) * Mathf.Rad2Deg; }
            if (targetPitchAngle > 180) { targetPitchAngle = targetPitchAngle - 360; }
            targetPitchAngle = -targetPitchAngle;
            if (safedist.magnitude < SlowManueverRange * 2)
            {
                if ((currentRollAngle > 0 ? currentRollAngle : -currentRollAngle) > 45)
                {
                    deltaY = error.y;
                }
                else { deltaY = currentWay.y - transform.position.y; }
                deltaY *= deltaY;
                targetPitchAngle = Mathf.Clamp(targetPitchAngle * deltaY * pitchP, targetPitchAngle > 0 ? 0 : targetPitchAngle, targetPitchAngle > 0 ? targetPitchAngle : 0);

                if (error.y > 0) { pitchDelta = preY - error.y; }                       //PitchDelta is change in PitchDistance
                else { pitchDelta = -preY + error.y; }
                preY = error.y;

                pitchReducer = pitchDelta * pitchD / (errorPositive.y);
                pitchReducer = Mathf.Clamp(pitchReducer, -20, 20);
                if ((errorPositive.y) < SlowManueverRange) { pitchReducer = 0; }

                if (targetPitchAngle > 0) { targetPitchAngle = targetPitchAngle - pitchReducer; }
                else { targetPitchAngle = targetPitchAngle + pitchReducer; }
                //targetPitchAngle = Mathf.Clamp(targetPitchAngle, -maxPitch, maxPitch);
            }
        }
        PitchSetter(targetPitchAngle);
    }
    void PitchSetter(float targetAngle)
    {
        pitch = Mathf.Clamp(targetAngle * pitchI, -1, 1);
        if (pitch < 0)
            pitch *= -pitch;
        else
            pitch *= pitch;
        if ((pitch > 0 ? pitch : -pitch) < 0.01f)
            pitch = 0;
        mPitch = Mathf.SmoothDamp(mPitch, pitch, ref vel, pitchLerp);
        controller.input.rawPitchInput = -mPitch;

        //pitchRate = currentPitchAngle - prePitch;
        //pitchRate = pitchRate >0 ? pitchRate:-pitchRate;
        //pitch = Mathf.Clamp((targetAngle - currentPitchAngle)* pitchI,-pitchClamper,pitchClamper);
        ////if((pitch<0?-pitch:pitch) < 0.1) { pitch = 0; }
        //if(pitchRate <0.25)                                                                         //To Controll PitchRate
        //controller.input.rawPitchInput = pitch;         //put Minus incase
        //prePitch = currentPitchAngle;
    }
    void RollAngleCalulate()
    {
        if (safedist.magnitude < SlowManueverRange || rollDistance < SlowManueverRange)
        {
            targetRollAngle = Mathf.Lerp(targetRollAngle, -currentRollAngle, rollAngleLerp);
        }
        else
        {
            if (error.x < 0) { targetRollAngle = 360 - (Mathf.Atan2(error.x, error.y) * Mathf.Rad2Deg * -1); }
            else { targetRollAngle = Mathf.Atan2(error.x, error.y) * Mathf.Rad2Deg; }
            if (targetRollAngle > 180) { targetRollAngle = targetRollAngle - 360; } //Rotation to make target on uperside of aircraft
            if (rollDistance < SlowManueverRange * 4 && (transform.position.y - currentWay.y) > SlowManueverRange)
            {
                //CodeRequired to make roll not upsidedown
                if ((targetRollAngle + currentRollAngle) < 0) { targetRollAngle = targetRollAngle + 180; }
                else { targetRollAngle = targetRollAngle - 180; }
            }
            else if (rollDistance > SlowManueverRange * 2)
            {
            }
            else
            {
                deltaX = (errorPositive.x);
                deltaX /= SlowManueverRange;
                deltaX *= deltaX;
                targetRollAngle = Mathf.Clamp(targetRollAngle * deltaX * rollP, targetRollAngle > 0 ? 0 : targetRollAngle, targetRollAngle > 0 ? targetRollAngle : 0);   //to reduce rotation when closer

                if (error.x > 0) { rollDelta = preX - error.x; }
                else { rollDelta = -preX + error.x; }
                rollDelta *= rollD / (errorPositive.x);
                preX = error.x;                             //Change in RollDistance
                rollReducer = rollDelta * rollD / (errorPositive.x);
                rollReducer = Mathf.Clamp(rollReducer, -20, 20);
                if ((errorPositive.x) < SlowManueverRange) { rollReducer = 0; }

                //if (targetRollAngle > 0) { targetRollAngle = targetRollAngle - rollReducer; }
                //else { targetRollAngle = targetRollAngle + rollReducer; }
            }
        }
        RollSetter(targetRollAngle);
    }
    void RollSetter(float targetAngle)
    {
        rollRate = currentRollAngle - PreRoll;
        PreRoll = currentRollAngle;
        roll = Mathf.Clamp((targetAngle - rollRate * rollD) * rollI, -1, 1);
        if (roll < 0)
            roll *= -roll;
        else
            roll *= roll;
        if ((roll > 0 ? roll : -roll) < 0.1)
            roll = 0;
        mRoll = Mathf.Lerp(mRoll, roll, rollLerp);
        controller.input.rawRollInput = mRoll;

        //deltaX2 = (new Vector2(Waypoint[index].position.x, Waypoint[index].position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude;
        //deltaX2 -= errorPositive.z;     //Global Roll Required irrespective of current rotation
        //if (deltaX2 > SlowManueverRange && (errorPositive.y)>SlowManueverRange || errorPositive.x<SlowManueverRange)
        //{      //SlowManuever
        //    if (currentRollAngle > 45) { roll = -1; }
        //    else if(currentRollAngle < -45) { roll = 1; }
        //}
        //if (currentRollAngle > maxRoll) { roll = -1; }
        //else if (currentRollAngle < -maxRoll) { roll = 1; }
        //PreRoll = Mathf.Lerp(PreRoll, roll, LerpRoll);
        //controller.input.rawRollInput = PreRoll;
    }
    void ThrottleSetter()
    {
        //pGain = error.z * P;
        //if (error.z > 0) { dGain = x - error.z; }
        //else { dGain = -x + error.z; }
        //dGain *= Time.fixedDeltaTime * 50 * (D / (errorPositive.z));
        //x = error.z;
        //if (pGain > 0) { throttleOffset = pGain - dGain; }
        //else { throttleOffset = pGain + dGain; }
        //throttle = Mathf.Clamp(throttleOffset, minThrottle, 1f);
        //controller.m_input._throttleInput = throttle;
        if(rb.velocity.magnitude < fixedSpeed)
        controller.input.rawThrottleInput = 1;
        else { controller.input.rawThrottleInput = 0; }
    }
    void WayPointSelector()
    {
        if (Vector3.Distance(transform.position, Waypoint[index]) < Offset)
        {
            index++;
            if (index >= Waypoint.Count)
            {
                if (Save_Waypoints.isLoop) { index = 0; }
                else { this.enabled = false; }

            }
        }
    }
    void FirstWay()
    {
        int j = 0, k = 0;
        for (int i = 1; i < Waypoint.Count; i++)
        {
            if (Vector3.Distance(Waypoint[i], transform.position) < Vector3.Distance(Waypoint[j], transform.position))
            {
                j = i;
            }
        }
        if (j == 0) { k = Waypoint.Count-1; }
        for (int i = 0; i < Waypoint.Count; i++)
        {
            if (i == j || i == k) { continue; }
            if (Vector3.Distance(Waypoint[i], transform.position) < Vector3.Distance(Waypoint[k], transform.position))
            {
                k = i;
            }
        }
        if (Vector3.Distance(Waypoint[k], transform.position) < Vector3.Distance(Waypoint[j], transform.position))
            index = k;
        else
            index = j;
    }
}
