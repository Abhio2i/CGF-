using UnityEngine;
using Data.Plane;
using EW.Flare;
using InputActions;
using System.Collections.Generic;
using System.Collections;
public class script : MonoBehaviour
{
    public GameObject mainCAM;
    public GameObject refer;
    public Vector3 Offset;
    public InputActionManager2_script InputsData;
    float throttleInput;
    Vector3 controlInput;
    public Rigidbody rb;
    public float Throttle;
    public GameObject Plane;
    public float thrustSpeed =1;
    [SerializeField]
    float maxThrust;
    public Vector3 LocalVelocity;
    public Vector3 Velocity;
    public Vector3 LocalAngularVelocity;
    public float AngleOfAttack;
    public float AngleOfAttackYaw;
    public Vector3 LocalGForce;
    public Vector3 lastVelocity;
    [SerializeField]
    float gLimit;
    [SerializeField]
    float gLimitPitch;
    public Vector3 EffectiveInput { get; private set; }
    public Vector3 inputvec3;
    public GameObject rudder;
    public GameObject aileronLeft;
    public GameObject aileronRight;


    #region Lift
    [Header("Lift")]
    [SerializeField]
    float liftParameter;
    [SerializeField]
    AnimationCurve liftAOACurve;
    [SerializeField]
    float inducedDragParameter;
    [SerializeField]
    AnimationCurve inducedDragCurve;
    [SerializeField]
    float liftStabilityTorque;
    [SerializeField]
    float rudderParameter;
    [SerializeField]
    AnimationCurve rudderAOACurve;
    [SerializeField]
    AnimationCurve rudderInducedDragCurve;
    [SerializeField]
    float rudderStabilityTorque;
    /*
    bool flapsDeployed;
    [SerializeField]
    float flapsLiftPower;
    [SerializeField]
    float flapsAOABias;
    [SerializeField]
    float flapsDrag;
    [SerializeField]
    float flapsRetractSpeed;*/
    #endregion

    #region Drag
    [Header("Drag")]
    [SerializeField]
    AnimationCurve dragForward;
    [SerializeField]
    AnimationCurve dragBack;
    [SerializeField]
    AnimationCurve dragLeft;
    [SerializeField]
    AnimationCurve dragRight;
    [SerializeField]
    AnimationCurve dragTop;
    [SerializeField]
    AnimationCurve dragBottom;
    [SerializeField]
    Vector3 angularDrag;
    [SerializeField]
    float airbrakeDrag;
    private bool AirbrakeDeployed;
    #endregion

    #region Steering
    [Header("Steering")]
    [SerializeField]
    Vector3 turnSpeed; //in deg per sec (generic scaling parameter to multiply with steering parameter which is limiting)
    [SerializeField]
    Vector3 turnAcceleration;//in deg per sec (generic scaling parameter to multiply with steering parameter which is limiting)
    [SerializeField]
    AnimationCurve steeringCurve;
    #endregion


    /* 
     
     
     [SerializeField]
     float throttleSpeed;
     
     [SerializeField]
     float initialSpeed;*/


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //keep in small area
        Vector3 newpos;
        newpos.z = (Plane.transform.position.z ) % 500;
        newpos.y = (Plane.transform.position.y ) % 500;
        newpos.x = (Plane.transform.position.x ) % 500;
        Plane.transform.position = newpos;




        controlInput.x = InputsData.uMove + InputsData.dMove;
        controlInput.y = -(InputsData.yawleft - InputsData.yawright);
        controlInput.z = -(InputsData.rMove + InputsData.lMove);
        //print(controlInput);
        throttleInput = Mathf.Clamp(InputsData.thrust, -1f, 1f);
        mainCAM.transform.position = refer.transform.position+Offset;
        Vector3 axis;
        float angle;
        refer.transform.rotation.ToAngleAxis(out angle, out axis);
        /*mainCAM.transform.RotateAround(refer.transform.position,axis,angle*Mathf.Deg2Rad);*/
        //mainCAM.transform.rotation = refer.transform.rotation;
    }
    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        CalculateState(dt);
        UpdateLift();
        UpdateDrag();
        UpdateSteering(dt);
        UpdateThrust();
        UpdateThrottle(dt);
        UpdateAirbrake();

    }
    void UpdateDrag() // eq. of drag : D = Cd * A * .5 * rho * V^2
    {
        var lv = LocalVelocity;

        float airbrakeDrag = AirbrakeDeployed ? this.airbrakeDrag : 0;

        //calculate coefficient of drag depending on direction on velocity
        var coefficient = Scale6(
        lv.normalized,
        dragRight.Evaluate(Mathf.Abs(lv.x)), dragLeft.Evaluate(Mathf.Abs(lv.x)), //animation curves need to be tuned according to IRL data
        dragTop.Evaluate(Mathf.Abs(lv.y)), dragBottom.Evaluate(Mathf.Abs(lv.y)),
        dragForward.Evaluate(Mathf.Abs(lv.z))+ airbrakeDrag ,
        dragBack.Evaluate(Mathf.Abs(lv.z))
        );

        var drag = coefficient.magnitude * lv.sqrMagnitude * -lv.normalized;// set A = 2 (changes in Cd proportionally) and for now rho = 1(constant)
        
        rb.AddRelativeForce(drag);
    }
    void UpdateAirbrake()
    {
        AirbrakeDeployed = InputsData.Airbrake;
    }
    void UpdateThrust()
    {
        rb.AddRelativeForce(Throttle * maxThrust * Plane.transform.forward);
    }
    void UpdateThrottle(float dt)
    {
        float target = 0;

        if (throttleInput > 0) target = throttleInput;
        if (throttleInput < 0) target = 0;
        Throttle = MoveTo(Throttle, target, thrustSpeed, dt);
    }
    public static float MoveTo(float value, float target, float speed, float deltaTime)
    //cant instantly attain 'target' from current 'value' so increase by given 'speed*dt'
    {
        float min = 0;
        float max = 1;
        float diff = target - value;
        float delta = Mathf.Clamp(diff, -speed * deltaTime, speed * deltaTime);
        return Mathf.Clamp(value + delta, min, max);
    }
    Vector3 Scale6(Vector3 vec, float r, float l, float u, float d, float f, float b)
    {
        Vector3 result = Vector3.zero;
        //vec.x>0?result.x=r*vec.x: result.x = l * vec.x;
        if (vec.x > 0) { result.x = r * vec.x; }
        if (vec.x <= 0) { result.x = l * vec.x; }
        if (vec.y > 0) { result.y = u * vec.y; }
        if (vec.y <= 0) { result.y = d * vec.y; }
        if (vec.z > 0) { result.z = f * vec.z; }
        if (vec.z <= 0) { result.z = b * vec.z; }

        return result;
    }
    void CalculateState(float dt)
    {
        var invRotation = Quaternion.Inverse(rb.rotation);
        Velocity = rb.velocity;
        LocalVelocity = invRotation * Velocity;  //transform world velocity into local space
        LocalAngularVelocity = invRotation * rb.angularVelocity;  //transform into local space
        LocalGForce = CalculateGForce(LocalAngularVelocity, LocalVelocity);
        //print(LocalGForce);
        //print(AirbrakeDeployed);
        
        CalculateAngleOfAttack();
    }
    void CalculateAngleOfAttack()
    {
        AngleOfAttack = Mathf.Atan2(-LocalVelocity.y, LocalVelocity.z);
        AngleOfAttackYaw = Mathf.Atan2(LocalVelocity.x, LocalVelocity.z);
    }

    float CalculateSteeringError(float dt, float angularVelocity,
            float targetVelocity, float angularAcceleration)//actual change in vel. limited by possible acceleration
    {
        var error = targetVelocity - angularVelocity;
        var d_omega = angularAcceleration * dt;
        return Mathf.Clamp(error, -d_omega, d_omega);
    }
    void UpdateSteering(float dt)
    {
        var speed = Mathf.Max(0, LocalVelocity.z);//Steer only when moving forward
        var steeringParameter = steeringCurve.Evaluate(speed); //limits how fast the plame can turn

        var gForceScaling = CalculateGLimiter(controlInput, turnSpeed * Mathf.Deg2Rad * steeringParameter);

        var targetAV = Vector3.Scale(controlInput, turnSpeed * steeringParameter * gForceScaling);
        var av = LocalAngularVelocity * Mathf.Rad2Deg;

        var correction = new Vector3(
            CalculateSteeringError(dt, av.x, targetAV.x, turnAcceleration.x * steeringParameter),
            CalculateSteeringError(dt, av.y, targetAV.y, turnAcceleration.y * steeringParameter),
            CalculateSteeringError(dt, av.z, targetAV.z, turnAcceleration.z * steeringParameter)
        );

        rb.AddRelativeTorque(correction * Mathf.Deg2Rad, ForceMode.VelocityChange);    //ignore rigidbody mass(=1)

        if (correction.y > 0) { rudderRotateClockWise(correction.y); } //rudder rotation for yaw (not relevent for physics only for acurate look)
        if (correction.y < 0) { rudderRotateCounterClockWise(correction.y); }

        if (correction.z > 0) { aileronsForRollCounterClockwise(correction.z); }//flaps rotation for roll (not relevent for physics only for acurate look)
        if (correction.z < 0) { aileronsForRollClockwise(correction.z); }

        var correctionInput = new Vector3(
            Mathf.Clamp((targetAV.x - av.x) / turnAcceleration.x, -1, 1),
            Mathf.Clamp((targetAV.y - av.y) / turnAcceleration.y, -1, 1),
            Mathf.Clamp((targetAV.z - av.z) / turnAcceleration.z, -1, 1)
        );

        var effectiveInput = (correctionInput + controlInput) * gForceScaling;

        EffectiveInput = new Vector3(
            Mathf.Clamp(effectiveInput.x, -1, 1),
            Mathf.Clamp(effectiveInput.y, -1, 1),
            Mathf.Clamp(effectiveInput.z, -1, 1)
        );
    }

    Vector3 CalculateGForce(Vector3 angularVelocity, Vector3 velocity)
    {
        //estiamte G Force from angular velocity and velocity
        //G = AngularVelocity cross Velocity
        return Vector3.Cross(angularVelocity, velocity);
    }

    Vector3 CalculateGForceLimit(Vector3 input)
    {
        return Scale6(input,
            gLimit, gLimitPitch,    //pitch down, pitch up
            gLimit, gLimit,         //yaw
            gLimit, gLimit          //roll
        );                          //is this scalling right?
    }

    float CalculateGLimiter(Vector3 controlInput, Vector3 maxAngularVelocity)//Added for the pilot (could be turned off for AI)
    {
        //if the player gives input with magnitude less than 1, scale up their input so that magnitude == 1
        var maxInput = controlInput.normalized;
        var limit = CalculateGForceLimit(maxInput);
        var maxGForce = CalculateGForce(Vector3.Scale(maxInput, maxAngularVelocity), LocalVelocity);
        if (maxGForce.magnitude > limit.magnitude)
        {
            //example:
            //maxGForce = 16G, limit = 8G
            //so this is 8 / 16 or 0.5
            return limit.magnitude / maxGForce.magnitude;
        }
        return 1;
    }

    Vector3 CalculateLift(float angleOfAttack, Vector3 rightAxis, float liftParameter,
           AnimationCurve aoaCurve, AnimationCurve inducedDragCurve, out float liftForceMag)//returns lift and induced drag
    {
        var liftVelocity = Vector3.ProjectOnPlane(LocalVelocity, rightAxis);    //project velocity onto YZ plane
                                                                                //rightAxis is the unit vector normal to the plane of change

        //L = Cl * A * .5 * rho * V^2
        //coefficient varies with AOA
        var liftCoefficient = aoaCurve.Evaluate(angleOfAttack * Mathf.Rad2Deg);
        liftForceMag = liftVelocity.sqrMagnitude * liftCoefficient * liftParameter;

        //lift is perpendicular to velocity
        var liftDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);
        var lift = liftDirection * liftForceMag;

        //induced drag coeff varies with square of lift coefficient (all other const.s are eaten by inducedDragParameter) 
        var liftInducedDragCoeff = liftCoefficient * liftCoefficient;
        var dragDirection = -liftVelocity.normalized;
        var inducedDrag = dragDirection * liftVelocity.sqrMagnitude * liftInducedDragCoeff * this.inducedDragParameter * inducedDragCurve.Evaluate(Mathf.Max(0, LocalVelocity.z));//animation curves need to be tuned according to IRL data

        return lift + inducedDrag;
    }

    void UpdateLift() 
    {
        if (LocalVelocity.sqrMagnitude < 1f) return;

        /*float flapsLiftPower = FlapsDeployed ? this.flapsLiftPower : 0;
        float flapsAOABias = FlapsDeployed ? this.flapsAOABias : 0;
        Vector3 liftForce = CalculateLift(AngleOfAttack + (flapsAOABias * Mathf.Deg2Rad), Plane.transform.right,
        liftParameter + flapsLiftPower, liftAOACurve, inducedDragCurve, out float liftForceMagnitude);*/
        
        Vector3 liftForce = CalculateLift(AngleOfAttack , Plane.transform.right,
            liftParameter , liftAOACurve, inducedDragCurve, out float liftForceMagnitude);

        Vector3 yawForce = CalculateLift(AngleOfAttackYaw, Plane.transform.up, rudderParameter,
            rudderAOACurve, rudderInducedDragCurve, out float rudderLiftForceMagnitude);

        rb.AddRelativeForce(liftForce);
        rb.AddRelativeForce(yawForce);
    }

    void rudderRotateClockWise(float planerot)
    {
        //rudder.transform.localRotation = Quaternion.Euler(0,Mathf.Clamp(planerot*Mathf.Rad2Deg,-1f,1f),0);
        rudder.transform.localRotation = Quaternion.Euler(0, planerot , 0);
    }

    void rudderRotateCounterClockWise(float planerot)
    {
        //rudder.transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(planerot * Mathf.Rad2Deg, -1f, 1f), 0);
        rudder.transform.localRotation = Quaternion.Euler(0, planerot , 0);
    }
    void aileronsForRollCounterClockwise(float planerot)
    {
        aileronLeft.transform.localRotation = Quaternion.Euler(planerot , 0, 0);
        aileronRight.transform.localRotation = Quaternion.Euler(-planerot , 0, 0);
    }

    void aileronsForRollClockwise(float planerot)
    {
        aileronLeft.transform.localRotation = Quaternion.Euler(planerot , 0, 0);
        aileronRight.transform.localRotation = Quaternion.Euler(-planerot , 0, 0);
    }
}
