
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAutoPilot : MonoBehaviour
{
    public int speed = 250, direction, altitude=1500;
    public int currentDirection,currentSpeed,currentAltitude,minPitchAngle,maxPitchAngle;
    public float stablePitchAt200, roll,rollI,rollLerp, pitch,pitchI,pitchDamp, currentRollAngle, currentPitchAngle,targetRollAngle,targetPitchAngle;
    private float stablePitchAngle, pitchDistance, rollDistance,mPitch,mRoll,vel;
    private Vector3 flatForward,localFlatForward,flatRight,localFlatRight;
    private SilantroController controller;
    private Rigidbody rb;
    void Start()
    {
        controller = GetComponent<SilantroController>();
        //Invoke("Initialization", 2f);
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
       
        if (Input.GetKey(KeyCode.B)) { controller.turboFans[0].ToggleAfterburner(); }
        if (Input.GetKey(KeyCode.N)) { controller.turboFans[0].EngageAfterburner(); }
        if (Input.GetKey(KeyCode.M)) { controller.turboFans[0].DisEngageAfterburner(); }
        //    currentDirection = (int)transform.eulerAngles.y;
        //    currentAltitude = (int)transform.position.y;
        //    currentSpeed = (int)rb.velocity.magnitude;
        //    stablePitchAngle = stablePitchAt200 - (currentSpeed - 200)/50;
        //    #region Initialization
        //    //pitch = 0;
        //    roll = 0;
        //    #endregion
        //    //Pitch Angle of This Aircraft
        //    flatForward = transform.forward;
        //    flatForward.y = 0;
        //    flatForward.Normalize();
        //    localFlatForward = transform.InverseTransformDirection(flatForward);
        //    currentPitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * 60;

        //    //Rotation of This Aircraft
        //    flatRight = Vector3.Cross(Vector3.up, transform.forward);
        //    localFlatRight = transform.InverseTransformDirection(flatRight);
        //    currentRollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * 60;

        //    //Pitch
        //    PitchAngleCalculate();

        //    //Roll
        //    //RollAngleCalulate();

        //    //Throttle
        //    //ThrottleSetter();
        //    controller.m_input._throttleInput = 1;
        //}
        //void PitchAngleCalculate()
        //{
        //    if (currentPitchAngle > maxPitchAngle || currentPitchAngle < minPitchAngle){ PitchSetter(0);}
        //    else
        //    {
        //        pitchDistance = currentAltitude - altitude;
        //        targetPitchAngle = Mathf.Clamp(pitchDistance / 10, -60, 60);
        //        PitchSetter(targetPitchAngle);
        //    }
        //}
        //void PitchSetter(float targetAngle)
        //{
        //    //var pitchFactor = targetAngle - currentPitchAngle + stablePitchAngle;
        //    //pitch = Mathf.Clamp(pitchFactor * pitchI, -1, 1);
        //    //if (pitch < 0)
        //    //    pitch *= -pitch;
        //    //else
        //    //    pitch *= pitch;
        //    //mPitch = Mathf.SmoothDamp(mPitch, pitch, ref vel, pitchDamp);
        //    controller.m_input._rawPitchInput = pitch;
        //}
        //void RollAngleCalulate()
        //{
        //    RollSetter(targetRollAngle);
        //}
        //void RollSetter(float targetAngle)
        //{
        //    //rollRate = currentRollAngle - PreRoll;
        //    //PreRoll = currentRollAngle;
        //    //roll = Mathf.Clamp((targetAngle - rollRate * rollD) * rollI, -1, 1);
        //    if (roll < 0)
        //        roll *= -roll;
        //    else
        //        roll *= roll;
        //    if ((roll > 0 ? roll : -roll) < 0.1)
        //        roll = 0;
        //    mRoll = Mathf.Lerp(mRoll, roll, rollLerp);
        //    controller.m_input._rawRollInput = mRoll;
        //}
        //void ThrottleSetter()
        //{
        //    //pGain = error.z * P;
        //    //if (error.z > 0) { dGain = x - error.z; }
        //    //else { dGain = -x + error.z; }
        //    //dGain *= Time.fixedDeltaTime * 50 * (D / (errorPositive.z));
        //    //x = error.z;
        //    //if (pGain > 0) { throttleOffset = pGain - dGain; }
        //    //else { throttleOffset = pGain + dGain; }
        //    //throttle = Mathf.Clamp(throttleOffset, minThrottle, 1f);
        //    //controller.m_input._throttleInput = throttle;
    }
}
