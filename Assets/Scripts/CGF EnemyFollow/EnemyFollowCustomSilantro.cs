using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Newtonsoft.Json.Serialization;

public class EnemyFollowCustomSilantro : MonoBehaviour
{
    //Univerasal
    public Transform TargetObject;
    public Vector3 error;
    public float rollDistance,pitchDistance;
    private Vector3 errorPositive;
    public float SlowManueverRange = 150, follow_distance = 400,predictOffset = 1000,finalOffset;

    //For Pitch
    //public Vector3 pitchdirection;
    public float pitch, pitchLerp=1.5f, pitchP=0.00001f, pitchI=0.1f, pitchD=0,  pitchAngleLerp=0.07f; //pitchClamper = 1f maxPitch = 75,
    public float currentPitchAngle,  pitchFactor, pitchDelta, pitchReducer,  targetPitchAngle,pitchRate,prePitch, preY;

    //For Roll
    public float roll, rollLerp = 0.01f, rollP = 0.2f, rollI = 0.1f, rollD = 10, rollAngleLerp=0.07f;//maxRoll = 110, rollClamper = 1f, LerpRoll = 0.02f;
    public float currentRollAngle, targetRollAngle,  rollDelta, rollReducer, preX,  PreRoll,rollRate;

    //Throttle
    public float throttle, P = 0.4f, minThrottle = 0.2f, D = 60000;
    public float pGain, dGain, throttleOffset;

    //Temprory Variables
    private Vector3 flatForward, localFlatForward, flatRight, localFlatRight, dif;
    private SilantroController controller;
    private float vel,x,tempX,tempY,deltaX,deltaX2,deltaY,mRoll,mPitch;
    private Vector2 safedist;
    private Transform ai;
    private Rigidbody rb,rb2;
    void Start()
    {
        controller = GetComponent<SilantroController>();
        //Invoke("Initialization", 2f);
        rb = GetComponent<Rigidbody>();
        var G = new GameObject("AI");
        G.transform.parent = this.transform;
        ai = G.transform;
        if (TargetObject.GetComponent<Rigidbody>()) { rb2 = TargetObject.GetComponent<Rigidbody>(); }
    }
    void Initialization()
    {
        //if (controller.m_startMode != StartMode.Hot || controller.m_hotMode != HotStartMode.AfterInitialization)
        //{
        //    controller.TurnOnEngines();
        //}
        //if (controller.m_wheels.brakeState == 0) { controller.m_input.ToggleBrakeState(); }
    }
    void FixedUpdate()
    {
        if(TargetObject == null) { return; }
        if (rb2!=null)
        {
            finalOffset = predictOffset * rb2.velocity.magnitude/250;
        }
        else { finalOffset = 0.0f; }
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
        error = TargetObject.position + (TargetObject.forward * finalOffset) - rb.position;
        ai.rotation = Quaternion.Euler(ai.rotation.x, ai.rotation.y, 0);
        rollDistance = (Quaternion.Inverse(ai.rotation) * error).x;
        ai.rotation = Quaternion.Euler(0, TargetObject.rotation.y, TargetObject.rotation.z);
        pitchDistance = TargetObject.position.y - transform.position.y;//(Quaternion.Inverse(ai.rotation) * error).x;
        ai.rotation = transform.rotation;
        error = Quaternion.Inverse(rb.rotation) * error;
        dif = TargetObject.position + (TargetObject.forward * finalOffset)  - transform.position;
        rollDistance = new Vector2(dif.x, dif.z).magnitude - (error.z>0?error.z:-error.z);
        rollDistance = rollDistance > 0 ? rollDistance : -rollDistance;
        pitchDistance = dif.y;
        error = new Vector3(error.x, error.y, error.z - follow_distance - finalOffset);       // Distance of follow position from it's local axis
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
        if (safedist.magnitude < SlowManueverRange || pitchDistance < SlowManueverRange)
        {
            targetPitchAngle = Mathf.Lerp(targetPitchAngle,-currentPitchAngle,pitchAngleLerp);
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
                else { deltaY = TargetObject.position.y - transform.position.y; }
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
        if (safedist.magnitude < SlowManueverRange || rollDistance <SlowManueverRange)
        {
            targetRollAngle = Mathf.Lerp(targetRollAngle, -currentRollAngle, rollAngleLerp);
        }
        else
        {
            if (error.x < 0) { targetRollAngle = 360 - (Mathf.Atan2(error.x, error.y) * Mathf.Rad2Deg * -1); }
            else { targetRollAngle = Mathf.Atan2(error.x, error.y) * Mathf.Rad2Deg; }
            if (targetRollAngle > 180) { targetRollAngle = targetRollAngle - 360; } //Rotation to make target on uperside of aircraft
            if (rollDistance<SlowManueverRange*4 && (transform.position.y - TargetObject.position.y) > SlowManueverRange)
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
        roll = Mathf.Clamp((targetAngle-rollRate*rollD) * rollI, -1, 1);
        if (roll < 0)
            roll *= -roll;
        else
            roll *= roll;
        if ((roll > 0 ? roll : -roll) < 0.1)
            roll = 0;
        mRoll = Mathf.Lerp(mRoll, roll, rollLerp);
        controller.input.rawRollInput = mRoll;

        //deltaX2 = (new Vector2(TargetObject.position.x, TargetObject.position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude;
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
        pGain = error.z * P;
        if (error.z > 0) { dGain = x - error.z; }
        else { dGain = -x + error.z; }
        dGain *= Time.fixedDeltaTime * 50 * (D / (errorPositive.z));
        x = error.z;
        if (pGain > 0) { throttleOffset = pGain - dGain; }
        else { throttleOffset = pGain + dGain; }
        throttle = Mathf.Clamp(throttleOffset, minThrottle, 1f);
        controller.input.rawThrottleInput = throttle;
    }
    #region Old
//    using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Rendering;
//using Newtonsoft.Json.Serialization;

//public class EnemyFollowCustomSilantro : MonoBehaviour
//{
//    //Univerasal
//    public Transform TargetObject;
//    public Vector3 error;
//    private Vector3 errorPositive;
//    public float SlowManueverRange = 150, follow_distance = 400, predictOffset = 1000, finalOffset;

//    //For Pitch
//    //public Vector3 pitchdirection;
//    public float pitch, pitchLerp = 1.5f, pitchP = 0.00001f, pitchI = 0.1f, pitchD = 0, pitchAngleLerp = 0.07f; //pitchClamper = 1f maxPitch = 75,
//    public float currentPitchAngle, pitchFactor, pitchDelta, pitchReducer, targetPitchAngle, pitchRate, prePitch, preY;

//    //For Roll
//    public float roll, rollLerp = 0.01f, rollP = 0.2f, rollI = 0.1f, rollD = 10, rollAngleLerp = 0.07f;//maxRoll = 110, rollClamper = 1f, LerpRoll = 0.02f;
//    public float currentRollAngle, targetRollAngle, rollDelta, rollReducer, preX, PreRoll, rollRate;

//    //Throttle
//    public float throttle, P = 0.4f, minThrottle = 0.2f, D = 60000;
//    public float pGain, dGain, throttleOffset;

//    //Temprory Variables
//    private Vector3 flatForward, localFlatForward, flatRight, localFlatRight;
//    private SilantroController controller;
//    private float vel, x, tempX, tempY, deltaX, deltaX2, deltaY;
//    private Vector2 safedist;
//    void Start()
//    {
//        controller = GetComponent<SilantroController>();
//        //Invoke("Initialization", 2f);
//    }
//    void Initialization()
//    {
//        //if (controller.m_startMode != StartMode.Hot || controller.m_hotMode != HotStartMode.AfterInitialization)
//        //{
//        //    controller.TurnOnEngines();
//        //}
//        //if (controller.m_wheels.brakeState == 0) { controller.m_input.ToggleBrakeState(); }
//    }
//    void FixedUpdate()
//    {
//        if (TargetObject == null) { return; }
//        if (TargetObject.GetComponent<Rigidbody>())
//        {
//            finalOffset = predictOffset * TargetObject.GetComponent<Rigidbody>().velocity.magnitude / 250;
//        }
//        else { finalOffset = 0.0f; }
//        #region Initialization
//        //if (!invoke)
//        //if(Leader.m_wowForce > 1)
//        //{
//        //    controller.TurnOnEngines();
//        //}
//        error = Vector3.zero;
//        pitch = 0;
//        roll = 0;
//        #endregion
//        error = TargetObject.position + (TargetObject.forward * finalOffset) - GetComponent<Rigidbody>().position;
//        error = Quaternion.Inverse(GetComponent<Rigidbody>().rotation) * error;
//        error = new Vector3(error.x, error.y, error.z - follow_distance - finalOffset);       // Distance of follow position from it's local axis
//        errorPositive = new Vector3(error.x > 0 ? error.x : -error.x, error.y > 0 ? error.y : -error.y, error.z > 0 ? error.z : -error.z);
//        safedist = error;   //Vector2

//        //Pitch Angle of This Aircraft
//        flatForward = transform.forward;
//        flatForward.y = 0;
//        flatForward.Normalize();
//        localFlatForward = transform.InverseTransformDirection(flatForward);
//        currentPitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * 60;

//        //Rotation of This Aircraft
//        flatRight = Vector3.Cross(Vector3.up, transform.forward);
//        localFlatRight = transform.InverseTransformDirection(flatRight);
//        currentRollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * 60;

//        //Pitch
//        PitchAngleCalculate();

//        //Roll
//        RollAngleCalulate();

//        //Throttle
//        ThrottleSetter();
//    }
//    void PitchAngleCalculate()
//    {
//        if (safedist.magnitude < SlowManueverRange && ((TargetObject.position.y - transform.position.y) < SlowManueverRange && (TargetObject.position.y - transform.position.y) > -SlowManueverRange))
//        {
//            targetPitchAngle = Mathf.Lerp(targetPitchAngle, -currentPitchAngle, pitchAngleLerp);
//        }
//        else
//        {
//            if (error.y < 0) { targetPitchAngle = 360 - (Mathf.Atan2(error.y, error.z) * Mathf.Rad2Deg * -1); }
//            else { targetPitchAngle = Mathf.Atan2(error.y, error.z) * Mathf.Rad2Deg; }
//            if (targetPitchAngle > 180) { targetPitchAngle = targetPitchAngle - 360; }
//            targetPitchAngle = -targetPitchAngle;
//            if (safedist.magnitude < SlowManueverRange * 2)
//            {
//                if ((currentRollAngle > 0 ? currentRollAngle : -currentRollAngle) > 45)
//                {
//                    deltaY = error.y;
//                }
//                else { deltaY = TargetObject.position.y - transform.position.y; }
//                deltaY *= deltaY;
//                targetPitchAngle = Mathf.Clamp(targetPitchAngle * deltaY * pitchP, targetPitchAngle > 0 ? 0 : targetPitchAngle, targetPitchAngle > 0 ? targetPitchAngle : 0);

//                if (error.y > 0) { pitchDelta = preY - error.y; }                       //PitchDelta is change in PitchDistance
//                else { pitchDelta = -preY + error.y; }
//                preY = error.y;

//                pitchReducer = pitchDelta * pitchD / (errorPositive.y);
//                pitchReducer = Mathf.Clamp(pitchReducer, -20, 20);
//                if ((errorPositive.y) < SlowManueverRange) { pitchReducer = 0; }

//                if (targetPitchAngle > 0) { targetPitchAngle = targetPitchAngle - pitchReducer; }
//                else { targetPitchAngle = targetPitchAngle + pitchReducer; }
//                //targetPitchAngle = Mathf.Clamp(targetPitchAngle, -maxPitch, maxPitch);
//            }
//        }
//        PitchSetter(targetPitchAngle);
//    }
//    void PitchSetter(float targetAngle)
//    {
//        pitch = Mathf.Clamp(targetAngle * pitchI, -1, 1);
//        if (pitch < 0)
//            pitch *= -pitch;
//        else
//            pitch *= pitch;
//        if ((pitch > 0 ? pitch : -pitch) < 0.01f)
//            pitch = 0;
//        controller.input.rawPitchInput = Mathf.SmoothDamp(controller.input.rawPitchInput, pitch, ref vel, pitchLerp);

//        //pitchRate = currentPitchAngle - prePitch;
//        //pitchRate = pitchRate >0 ? pitchRate:-pitchRate;
//        //pitch = Mathf.Clamp((targetAngle - currentPitchAngle)* pitchI,-pitchClamper,pitchClamper);
//        ////if((pitch<0?-pitch:pitch) < 0.1) { pitch = 0; }
//        //if(pitchRate <0.25)                                                                         //To Controll PitchRate
//        //controller.input.rawPitchInput = pitch;         //put Minus incase
//        //prePitch = currentPitchAngle;
//    }
//    void RollAngleCalulate()
//    {
//        if (safedist.magnitude < SlowManueverRange)
//        {
//            targetRollAngle = Mathf.Lerp(targetRollAngle, -currentRollAngle, rollAngleLerp);
//        }
//        else
//        {
//            if (error.x < 0) { targetRollAngle = 360 - (Mathf.Atan2(error.x, error.y) * Mathf.Rad2Deg * -1); }
//            else { targetRollAngle = Mathf.Atan2(error.x, error.y) * Mathf.Rad2Deg; }
//            if (targetRollAngle > 180) { targetRollAngle = targetRollAngle - 360; } //Rotation to make target on uperside of aircraft
//            if ((transform.position.y - TargetObject.position.y) > SlowManueverRange && safedist.magnitude < SlowManueverRange * 10)
//            {
//                //CodeRequired to make roll not upsidedown
//                if ((targetRollAngle + currentRollAngle) < 0) { targetRollAngle = targetRollAngle + 180; }
//                else { targetRollAngle = targetRollAngle - 180; }
//            }
//            else if (safedist.magnitude > SlowManueverRange * 2)
//            {
//            }
//            else
//            {
//                deltaX = (errorPositive.x);
//                deltaX /= SlowManueverRange;
//                deltaX *= deltaX;
//                targetRollAngle = Mathf.Clamp(targetRollAngle * deltaX * rollP, targetRollAngle > 0 ? 0 : targetRollAngle, targetRollAngle > 0 ? targetRollAngle : 0);   //to reduce rotation when closer

//                if (error.x > 0) { rollDelta = preX - error.x; }
//                else { rollDelta = -preX + error.x; }
//                rollDelta *= rollD / (errorPositive.x);
//                preX = error.x;                             //Change in RollDistance
//                rollReducer = rollDelta * rollD / (errorPositive.x);
//                rollReducer = Mathf.Clamp(rollReducer, -20, 20);
//                if ((errorPositive.x) < SlowManueverRange) { rollReducer = 0; }

//                //if (targetRollAngle > 0) { targetRollAngle = targetRollAngle - rollReducer; }
//                //else { targetRollAngle = targetRollAngle + rollReducer; }
//            }
//        }
//        RollSetter(targetRollAngle);
//    }
//    void RollSetter(float targetAngle)
//    {
//        rollRate = currentRollAngle - PreRoll;
//        PreRoll = currentRollAngle;
//        roll = Mathf.Clamp((targetAngle - rollRate * rollD) * rollI, -1, 1);
//        if (roll < 0)
//            roll *= -roll;
//        else
//            roll *= roll;
//        if ((roll > 0 ? roll : -roll) < 0.1)
//            roll = 0;
//        controller.input.rawRollInput = Mathf.Lerp(controller.input.rawRollInput, roll, rollLerp);


//        //deltaX2 = (new Vector2(TargetObject.position.x, TargetObject.position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude;
//        //deltaX2 -= errorPositive.z;     //Global Roll Required irrespective of current rotation
//        //if (deltaX2 > SlowManueverRange && (errorPositive.y)>SlowManueverRange || errorPositive.x<SlowManueverRange)
//        //{      //SlowManuever
//        //    if (currentRollAngle > 45) { roll = -1; }
//        //    else if(currentRollAngle < -45) { roll = 1; }
//        //}
//        //if (currentRollAngle > maxRoll) { roll = -1; }
//        //else if (currentRollAngle < -maxRoll) { roll = 1; }
//        //PreRoll = Mathf.Lerp(PreRoll, roll, LerpRoll);
//        //controller.input.rawRollInput = PreRoll;
//    }
//    void ThrottleSetter()
//    {
//        pGain = error.z * P;
//        if (error.z > 0) { dGain = x - error.z; }
//        else { dGain = -x + error.z; }
//        dGain *= Time.fixedDeltaTime * 50 * (D / (errorPositive.z));
//        x = error.z;
//        if (pGain > 0) { throttleOffset = pGain - dGain; }
//        else { throttleOffset = pGain + dGain; }
//        throttle = Mathf.Clamp(throttleOffset, minThrottle, 1f);
//        controller.input.rawThrottleInput = throttle;
//    }
//}

    #endregion
}
