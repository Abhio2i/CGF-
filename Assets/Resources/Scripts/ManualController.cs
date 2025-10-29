using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;

public class ManualController : MonoBehaviour
{
    public enum Maneuvers
    {
        None,
        Immelmann,
        Split,
        HighYoyo,
        LowYoyo,
        HardTurn,
        CanoapyRoll,
        AileronRoll,
        BarrelRoll,
        Dergree90
    }

    [Header("Joystick Controles")]
    [Header("Joystick_Inputs")]
    public float ver = 0;
    public float hor = 0;
    public float thro = 0;
    public bool brake = false;
    public float VerSmooth = 1;
    public float HorSmooth = 1;
    public float throttle = 1;
    public float throSmooth = 1;
    public float AngleOfAttack = 0f;
    public float AOASmooth = 1f;

    [Header("State")]
    public float CurrentBraking = 0;
    public bool GroundCollisionActivate = false;
    public bool AIControlling = false;
    public bool Formation = false;

    [Header("Aircraft Config")]
    public float MaxEnginePower = 40;

    [Header("GroundCollision")]
    public LayerMask layerMask;
    public bool GroundCollisionEnable = false;
    public float GroundCollisionActivationRange = 1000;
    public float GroundCollisionOffset = 100f;
    public float MaxGroundBrakePower = 30;
    public float MinSpeedFactor = 200;
    public float BrakeSmooth = 1f;
    public float SpeedFactor = 1f;
    public float AngleFactor = 1f;
    public float GroundHitDistance = 0f;

    [Header("AIControle")]
    public bool AIControleEnable = false;
    public bool AITargetFollow = false;
    public float MaxAiPowerBrakes = 10f;
    public float DefaultAiPowerBrakes = 0.2f;
    public Vector3 AiTarget = Vector3.zero;
    public float DefaultTargetFollowSmoothness = 0f;
    public float MaxTargetFollowSmoothness = 0f;
    public float TargetFollowSmoothness = 0f;
    public float DefaultSensitivity = 0.1f;
    public float CloseSensitivity = 2f;
    public float FormationSensitivity = 2f;
    public float sensitivity = 1f;

    [Header("Aircraft Alignment")]
    public bool AlignmentActivate = false;
    public bool TargetFollow = false;
    public float AlignmentRange = 0;
    public float AlignmentCenter = 0;
    public float TargetCenterDistance = 0f;
    public float TargetDistance = 0f;
    public float ForwardDistance = 0f;
    private float _lastForwardDistance = 0f;
    public float DistanceRate = 0f;
    public Vector3 TargetLocalPosition;
    public Vector3 TargetDefaultPosition;
    public bool Reset = false;

    [Header("Aircraft Maneuver")]
    public bool EnableManeuver = false;
    public Maneuvers maneuvers = Maneuvers.Immelmann;
    public float DefaultBankedTurnEffect = 0f;
    public float DefaultRollLevel = 0f;
    public float DefaultPitchLevel = 0f;
    public float DefaultTurnPitch = 0f;
    public float BankedTurnEffect = 0f;
    public float RollLevel = 0f;
    public float PitchLevel = 0f;
    public float TurnPitch = 0f;
    [Range(-180, 180)]
    public float TargetRoll = 0f;
    [Range(-180, 180)]
    public float TargetPitch = 0f;
    [Range(-180, 180)]
    public float TargetYaw = 0f;
    public float StoreYaw = 0;

    public float currentRoll = 0f;
    public float currentPitch = 0f;
    public float currentYaw = 0f;
    public bool start = false;
    public bool step1 = false;
    public bool step2 = false;
    public bool step3 = false;
    public bool step4 = false;
    public bool step5 = false;
    public bool step6 = false;
    public bool step7 = false;
    public bool end = false;


    [Header("Aircraft Information")]
    public float Speed = 1f;
    public float Altitude = 0f;
    public float Throttle = 0f;
    public float AOA = 0f;

    [Header("UI Information")]
    public bool UiEnable;
    public Text Speed_Text;
    public Text Speed2_Text;
    public Text Altitude_Text;
    public Text throttle_Text;
    
   
    [Header("Refrences")]
    public AeroplaneController aeroplaneController;
    public AeroplaneAiControl aeroplaneAiControl;
    public Transform followTarget;
    public Transform DummyTarget;
    /*
    [Header("config")]
    public bool CollisionControl = false;
    public AeroplaneController aeroplaneController;
    public AeroplaneAiControl aeroplaneAiControl;
    public bool Manual = true;
    public bool AutoThrottle = false;
    public float DefaultBrake = 0.2f;
    public float MaxBrake = 0.2f;
    public float MaxPower = 0;
    public float DistanceOffset = 0;
    public float RollSensitivity = 0.2f;
    public float PitchSensitivity = 0.2f;
    public float smoothV = 1;
    public float smoothH = 1;
    public float angle = 0;
    public float brake = 0;
    public Text Speed;
    public Text Altitude;
    public Text throttle;
    */
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aeroplaneController = GetComponent<AeroplaneController>();
        aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
        followTarget = aeroplaneAiControl.m_Target;
        TargetDefaultPosition = followTarget.localPosition;
    }

    public Maneuvers GetRandomEnumValue()
    {
        Array values = Enum.GetValues(typeof(Maneuvers));
        System.Random random = new System.Random();
        Maneuvers randomValue = (Maneuvers)values.GetValue(random.Next(values.Length));
        if(randomValue == Maneuvers.None)
        {
            randomValue = Maneuvers.HardTurn;
        }
        return randomValue;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        #region Joystick Controls
        ver = Input.GetAxis("Vertical");
        hor = Input.GetAxis("Horizontal");
        thro = (Input.GetAxis("Throttle") + 1) * 0.5f;
        brake = Input.GetKey(KeyCode.Space);
        #endregion

        #region Aircraft Information
        AOA = Vector3.Angle(transform.forward, -Vector3.up);
        Altitude =  (transform.position.y * 3.281f);
        Speed = (rb.velocity.magnitude * 3.6f);
        #endregion

        #region Default State
        AIControlling = AIControleEnable;
        GroundCollisionActivate = false;
        sensitivity = DefaultSensitivity;
        #endregion

        #region Ground Collison Avoidence
        if (GroundCollisionEnable)
        {
            float offset = 0;
            RaycastHit hit;
            if(Physics.Raycast(transform.position,transform.forward, out hit,500, layerMask))
            {
                //Debug.Log(hit.transform.name);
                offset = hit.point.y;
            }

            GroundHitDistance = (rb.velocity.magnitude * 3.6f) * SpeedFactor * AngleFactor * (1 - (AOA / 90f));


            if ( ((transform.position.y * 3.281f) - offset) - GroundHitDistance < GroundCollisionOffset &&  Speed > MinSpeedFactor)
            {
                GroundCollisionActivate = true;
                hor = 0;
                ver = -1;
                thro = 0;
                brake = true;

               aeroplaneController.m_AutoRollLevel = 0.2f;
                var brakes = ((GroundHitDistance / GroundCollisionActivationRange) * MaxGroundBrakePower) + 3;
                CurrentBraking = Mathf.Lerp(CurrentBraking, brakes, Time.deltaTime * BrakeSmooth);
                AIControlling = false;
            }

        }
        #endregion

        #region Aircraft Alignment
        if (AIControlling)
        {

            AiTarget = Vector3.Lerp(AiTarget, aeroplaneAiControl.m_Target.position, Time.deltaTime * TargetFollowSmoothness);
            //DummyTarget.position = AiTarget;

            TargetDistance = Vector3.Distance(AiTarget, transform.position);
            TargetCenterDistance = (TargetDistance - AlignmentCenter);
            TargetLocalPosition = aeroplaneAiControl.m_Target.InverseTransformPoint(transform.position);
            ForwardDistance = TargetLocalPosition.z;
            DistanceRate = ForwardDistance - _lastForwardDistance;
            _lastForwardDistance = ForwardDistance;

            //Default Throttle and Brakes                                 
            thro = 1f;
            brake = false;
            AlignmentActivate = false;
            TargetFollowSmoothness = DefaultTargetFollowSmoothness;
            //Braking and Throttle Apply when AlignmentRange Reach
            if (Reset == false)
            {
                //followTarget.localPosition = TargetDefaultPosition;
                if (TargetDistance < AlignmentRange)
                {

                    AlignmentActivate = true;
                    if (ForwardDistance < 0 || true)
                    {
                        if (Formation)
                        {
                            sensitivity = FormationSensitivity;
                        }
                        else
                        {
                            sensitivity = CloseSensitivity;
                        }
                        
                        //thro = (TargetDistance / AlignmentRange)-1*DistanceRate;
                        thro = (TargetDistance - AlignmentCenter) / AlignmentCenter * -DistanceRate;
                        thro = thro < 0 ? 0 : Mathf.Exp(thro);
                        thro = Mathf.Clamp(thro, 0, 2);
                        if (TargetFollow)
                        {
                            brake = false;
                        }
                        else
                        {
                            brake = true;
                        }

                        CurrentBraking = Mathf.Lerp(CurrentBraking, (1 - thro) * MaxAiPowerBrakes, Time.deltaTime * BrakeSmooth);
                        if (ForwardDistance > 0)
                        {
                            thro = 0;
                            CurrentBraking = MaxAiPowerBrakes;
                        }
                        TargetFollowSmoothness = MaxTargetFollowSmoothness;
                    }
                    else
                    {
                        Reset = true;
                        if (TargetFollow)
                        {
                            //followTarget.localPosition = new Vector3(TargetDefaultPosition.x, TargetDefaultPosition.y, TargetDefaultPosition.z + 1000);
                        }
                        else
                        {
                            //followTarget.localPosition = new Vector3(TargetDefaultPosition.x, TargetDefaultPosition.y, TargetDefaultPosition.z - 1000);
                        }
                    }

                }
                else
                {
                    CurrentBraking = Mathf.Lerp(CurrentBraking, DefaultAiPowerBrakes, Time.deltaTime * BrakeSmooth);

                }
            }
            else
            {

                if (TargetDistance < AlignmentCenter || TargetDistance > 1500)
                {
                    //followTarget.localPosition = TargetDefaultPosition;
                    Reset = false;
                }
            }


            aeroplaneAiControl.throttleInput = throttle;

        }
        throttle = Mathf.Lerp(throttle, thro, Time.deltaTime * throSmooth);
        AngleOfAttack = Mathf.Lerp(AngleOfAttack, AOA, Time.deltaTime * AOASmooth);
        float aoa = ((1 - (AngleOfAttack / 180)) * 2f);
        aoa = aoa > 1 ? Mathf.Pow(aoa, 0.5f) : Mathf.Pow(aoa, 5);
        #endregion

        #region Maneuver
        
        AIControlling = !start;
        if (start)
        {
            aeroplaneController.m_BankedTurnEffect = 0;
            aeroplaneController.m_AutoTurnPitch = 0;
            aeroplaneController.m_AutoPitchLevel = 0;
            aeroplaneController.m_AutoRollLevel = 0;
        }

        currentPitch = ConvertAngle(transform.localEulerAngles.x);
        currentRoll = ConvertAngle(transform.localEulerAngles.z);
        currentYaw = transform.eulerAngles.y;
        float p = currentPitch < 0 ? -currentPitch : currentPitch;
        float tp = TargetPitch < 0 ? -TargetPitch : TargetPitch;

        float r = currentRoll < 0 ? -currentRoll : currentRoll;
        float cp = r > 90 ?(currentPitch<0? (-90- (90+currentPitch)): 90 +(90-currentPitch)) : currentPitch;
        currentPitch = cp;
        if (EnableManeuver)
        {
            thro = 1;
            bool disableRoll = false;
            bool disablePitch = false;
            bool overideContorls = false;

            if (maneuvers == Maneuvers.Immelmann)
            {
                
                if (start)
                {
                    disableRoll = true;
                    if (step1 == false)
                    {
                        TargetPitch = -175;
                        TargetRoll = 0;
                        if (currentPitch > -176 && currentPitch < -174)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        TargetRoll = 0;
                        TargetPitch = 0;
                        if (currentRoll < 1 && currentRoll > -1)
                        {
                            step2 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                    }
                }
            }
            else
            if(maneuvers == Maneuvers.Split)
            {
                
                if (start)
                {
                    disableRoll = false;
                    if (step1 == false)
                    {
                        TargetPitch = 0;
                        TargetRoll = -178;
                        if(currentRoll < -90)
                        {
                            TargetPitch = 178;
                        }
                        if (currentRoll < -177 && currentRoll > -178)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        disableRoll = true;
                        float rl = 178f;
                        if(currentPitch < 90)
                        {
                            rl = 0;
                        }
                        TargetRoll = currentRoll < 0 ? -rl : rl;
                        TargetPitch = 0;

                        if (currentPitch < 1 && currentPitch > -1)
                        {
                            step2 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                        disableRoll = false;
                    }
                }
            }
            else
            if (maneuvers == Maneuvers.CanoapyRoll)
            {
                
                if (start)
                {
                    overideContorls = true;
                    if (step1 == false)
                    {
                        ver = -1;
                        hor = 1;

                        TargetPitch = 0;
                        TargetRoll = 0;

                        if (currentRoll < -160 && currentRoll > -178)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        ver = -1;
                        hor = 1;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentRoll < 1 && currentRoll > -1)
                        {
                            step2 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                        disableRoll = false;
                        overideContorls = false;
                    }
                }
            }
            else
            if (maneuvers == Maneuvers.AileronRoll)
            {

                if (start)
                {
                    overideContorls = true;
                    if (step1 == false)
                    {
                        ver = 0;
                        hor = 1;

                        TargetPitch = 0;
                        TargetRoll = 0;

                        if (currentRoll < -160 && currentRoll > -178)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        ver = 0;
                        hor = 1;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentRoll < 7 && currentRoll > -7)
                        {
                            step2 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                        disableRoll = false;
                        overideContorls = false;
                    }
                }
            }
            else
            if (maneuvers == Maneuvers.BarrelRoll)
            {

                if (start)
                {
                    overideContorls = true;
                    if (step1 == false)
                    {
                        ver = -0.7f;
                        hor = 0.5f;

                        TargetPitch = 0;
                        TargetRoll = 0;

                        if (currentRoll < -80 && currentRoll > -90)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        ver = -1;
                        hor = -0.5f;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentPitch < -70f && currentPitch > -80f)
                        {
                            step2 = true;
                        }
                    }
                    else
                    if (step2 && step3 == false)
                    {
                        ver = -1;
                        hor = -0.02f;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentPitch < -170f && currentPitch > -180f)
                        {
                            step3 = true;
                        }
                    }
                    else
                    if (step3 && step4 == false)
                    {
                        ver = -1f;
                        hor = -0.1f;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentPitch < 90f && currentPitch > 80f)
                        {
                            step4 = true;
                        }
                    }
                    else
                    if (step4 && step5 == false)
                    {
                        ver = -0.4f;
                        hor = -0.3f;

                        TargetRoll = 0;
                        TargetPitch = 0;

                        if ( (currentRoll < 4f && currentRoll > -4f))
                        {
                            step5 = true;
                        }
                    }
                    else
                    if (step5 && step6 == false)
                    {
                        ver = -1f;
                        hor = -0.04f;

                        TargetRoll = 0;
                        TargetPitch = 0;

                        if ( (currentPitch < 4f && currentPitch > -4f))
                        {
                            step6 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                        disableRoll = false;
                        overideContorls = false;
                    }
                }
            }
            else
            if (maneuvers == Maneuvers.HighYoyo)
            {

                if (start)
                {
                    overideContorls = true;
                    if (step1 == false)
                    {
                        ver = -0.5f;
                        hor = -0.1f; 

                        TargetPitch = 0;
                        TargetRoll = 0;

                        if (currentRoll < 180 && currentRoll > 175)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        ver = -0.7f;
                        hor = 0.15f;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentRoll < 110 && currentRoll > 90)
                        {
                            step2 = true;
                        }
                    }
                    else
                    if (step2 && step3 == false)
                    {
                        ver = -1f;
                        hor = -0.1f;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentPitch < 50 && currentPitch > 40)
                        {
                            step3 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                        disableRoll = false;
                        overideContorls = false;
                    }
                }
            }
            else
            if (maneuvers == Maneuvers.LowYoyo)
            {

                if (start)
                {
                    overideContorls = true;
                    if (step1 == false)
                    {
                        ver = -0.1f;
                        hor = 0.3f;

                        TargetPitch = 0;
                        TargetRoll = 0;

                        if (currentRoll < -110 && currentRoll > -120)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        ver = -1f;
                        hor = 0f;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentPitch < 5f && currentPitch > -5)
                        {
                            step2 = true;
                        }
                    }
                    else
                    if (step2 && step3 == false)
                    {
                        ver = -1f;
                        hor = 0.01f;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        if (currentPitch < -20 && currentPitch > -30)
                        {
                            step3 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                        disableRoll = false;
                        overideContorls = false;
                    }
                }
            }
            else
            if (maneuvers == Maneuvers.HardTurn)
            {

                if (start)
                {
                    disableRoll = false;
                    if (step1 == false)
                    {
                        TargetPitch = 0;
                        TargetRoll = 90;

                        if (currentRoll >88 && currentRoll < 92)
                        {
                            step1 = true;
                            StoreYaw = currentYaw;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        overideContorls = true;
                        ver = -1;
                        hor = 0;
                        TargetRoll = 0;
                        TargetPitch = 0;

                        float yaw = currentYaw - StoreYaw;
                        yaw = yaw<0?-yaw:yaw;
                        if (yaw > 90f)
                        {
                            TargetRoll = 0;
                            TargetPitch = 0;
                            step2 = true;
                        }
                    }
                    else
                    {
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                    }
                }
            }
            else
            if (maneuvers == Maneuvers.Dergree90)
            {
                if (start)
                {
                    disableRoll = true;
                    if (step1 == false)
                    {
                        overideContorls = true;
                        ver = -1;
                        hor = 0;
                        TargetPitch = 0;
                        TargetRoll = 0;
                        thro = 0;
                        brake = true;
                        aeroplaneController.m_PitchEffect = 1f;
                        CurrentBraking = 3;                        
                        if (currentPitch > -90 && currentPitch < -80)
                        {
                            step1 = true;
                        }
                    }
                    else
                    if (step1 && step2 == false)
                    {
                        overideContorls = true;
                        ver = 1;
                        hor = 0;
                        TargetRoll = 0;
                        TargetPitch = 0;
                        aeroplaneController.m_PitchEffect = 0.7f;
                        if (currentPitch > -5f && currentPitch < 5f)
                        {
                            TargetRoll = 0;
                            TargetPitch = 0;
                            step2 = true;
                        }
                    }
                    else
                    {
                        aeroplaneController.m_PitchEffect = 0.35f;
                        ///end
                        start = step1 = step2 = step3 = step4 = step5 = step6 = step7 = end = false;
                    }
                }
            }

                //hor = currentRoll > TargetRoll ? 1 : -1;
                if (overideContorls == false)
            {
                hor = (currentRoll - TargetRoll) / 180;
                float h = Mathf.Pow(hor < 0 ? -hor : hor, 0.2f);
                hor = hor < 0 ? -h : h;
                ///pitch -> disable roll for certain angle
                hor = ((p > 60) || tp > 80) && disableRoll ? 0 : hor;

                ver = -(currentPitch - TargetPitch) / 180;
                float v = Mathf.Pow(ver < 0 ? -ver : ver, 0.2f);
                ver = ver < 0 ? -v : v;
            }
        }
        #endregion 

        #region send Input to AerplaneController and AeroAiController

        aeroplaneAiControl.Activate = AIControlling;


        if (AIControlling)
        {
            aeroplaneController.m_MaxEnginePower = throttle * MaxEnginePower;
            aeroplaneAiControl.m_brake = brake;
            aeroplaneController.m_AirBrakesEffect = CurrentBraking;
            if (Formation)
            {
                aeroplaneAiControl.m_RollSensitivity = sensitivity;
                aeroplaneAiControl.m_PitchSensitivity = sensitivity;
            }
            else
            {
                aeroplaneAiControl.m_RollSensitivity = sensitivity;
                aeroplaneAiControl.m_PitchSensitivity = sensitivity / 2f;
            }

        }
        else
        {
            aeroplaneController.m_AirBrakesEffect = CurrentBraking;

            aeroplaneController.m_MaxEnginePower = thro * MaxEnginePower * (1 - (Altitude / 50000)) * aoa;
            aeroplaneController.Move(hor * HorSmooth, ver * VerSmooth, 0, thro, brake);
        }


        #endregion


        if (UiEnable)
        {
            float kn = (Speed * 0.539957f);
            kn = kn > 667f ? kn / 667 : kn;
            Speed_Text.text = kn.ToString("0.0") +( kn>667f?"Mach":"kn");
            if (Speed2_Text != null)
            Speed2_Text.text = Speed.ToString("0") + "km/hr";
            Altitude_Text.text = Altitude.ToString("0") + "ft";
            throttle_Text.text = (thro * 100f).ToString("0")+"%";
        }

        /*
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        t = (Input.GetAxis("Throttle") + 1) * 0.5f;
        b = Input.GetKey(KeyCode.Space);
        if ((transform.position.y * 3.281f) - output < CollisionOffset)
        {
            CollisionControl = true;
            h = 0;
            v = -1;
            t = 0;
            b = true;
            aeroplaneController.m_AutoRollLevel = 0.2f;
            var brakes = ((output / 1000) * 30) + 3;
            brake = Mathf.Lerp(brake, brakes, Time.deltaTime * BrakeSmooth);
            aeroplaneAiControl.Activate = false;
        }
        else
        {
            CollisionControl = false;
            if (Manual)
            {
                brake = Mathf.Lerp(brake, 3, Time.deltaTime * BrakeSmooth);
            } 
            aeroplaneController.m_AutoRollLevel = 0f;

            aeroplaneAiControl.Activate = !Manual;
        }
        if (!Manual) {
            if (!CollisionControl)
            {
                t = 1f;
                RollSensitivity = 0.2f;
                PitchSensitivity = 0.2f;
                bool brakes = false;
                if (AutoThrottle)
                {
                    var d = Vector3.Distance(transform.position, aeroplaneAiControl.m_Target.position);
                    if (d<DistanceOffset)
                    {
                        t = d / DistanceOffset;
                        RollSensitivity = (1 - t) * 2;
                        RollSensitivity = (1 - t) * 2;
                        brakes = true;
                        brake = Mathf.Lerp(brake, (1 - t) * MaxBrake, Time.deltaTime * BrakeSmooth);
                    }
                    else
                    {
                        brake = Mathf.Lerp(brake, DefaultBrake, Time.deltaTime * BrakeSmooth);
                    }
                   
                }
                aeroplaneAiControl.throttleInput = t;
                aeroplaneController.m_MaxEnginePower = MaxPower;
                //aeroplaneAiControl.m_RollSensitivity = RollSensitivity;
                //aeroplaneAiControl.m_PitchSensitivity = PitchSensitivity;
                aeroplaneAiControl.m_brake = brakes;
                aeroplaneController.m_AirBrakesEffect = brake;
            }
        }
        else
        {
            aeroplaneController.m_AirBrakesEffect = brake;
            aeroplaneController.m_MaxEnginePower = t * MaxPower;
        }


        if (/*CollisionControl ||* Manual)
        {
            aeroplaneController.Move(h * smoothH, v * smoothV, 0, t, b);
        }

        if (Ui)
        {
            Speed.text = (rb.velocity.magnitude * 3.6f).ToString("0") + "km/hr";
            Altitude.text = (transform.position.y * 3.281f).ToString("0") + "ft";
            throttle.text = (t * 100f).ToString("0");
        }
        angle = Vector3.Angle(transform.forward,-Vector3.up);
        output = (rb.velocity.magnitude * 3.6f) * SpeedFactor * AngleFactor * (1-(angle/90));
        */
    }

    // Method to convert 0-360 degree angle to -180 to 180 degree angle
    public float ConvertAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
