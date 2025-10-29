using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityInfoLog
{
    [SerializeField]
    public float Lat;
    [SerializeField]
    public float Long;
    [SerializeField]
    public float Altitude;
    [SerializeField]
    public float WaypointNumber;
    [SerializeField]
    public float WaypointDistance;
    [SerializeField]
    public float WaypointBearing;
    [SerializeField]
    public float TimeOfArrival;
    [SerializeField]
    public float Speed;
    [SerializeField]
    public float IndicatedSpeed;
    [SerializeField]
    public float TrueSpeed;
    [SerializeField]
    public float GroundSpeed;
    [SerializeField]
    public float Acceleration;
    [SerializeField]
    public float GForce;
    [SerializeField]
    public float PitchRate;
    [SerializeField]
    public float YawRate;
    [SerializeField]
    public float RollRate;
    [SerializeField]
    public float AirDensity;
    [SerializeField]
    public float EngineRPM;
    [SerializeField]
    public float Temperature;
    [SerializeField]
    public float EngineTemp;
    [SerializeField]
    public float Pressure;
    [SerializeField]
    public float VerticleVelocity;
    [SerializeField]
    public float Heading;
    [SerializeField]
    public float Pitch;
    [SerializeField]
    public float Roll;
    [SerializeField]
    public float AngleOfAttack;
    [SerializeField]
    public float Mach;
    [SerializeField]
    public float xVelocity;
    [SerializeField]
    public float yVelocity;
    [SerializeField]
    public float zVelocity;
    [SerializeField]
    public float xAccel;
    [SerializeField]
    public float yAccel;
    [SerializeField]
    public float zAccel;
    [SerializeField]
    public float RadarRange;
}

[Serializable]
public class WeaponLog
{
    [SerializeField]
    public int bullets;
    [SerializeField]
    public int BVR;
    [SerializeField]
    public int CCM;
    [SerializeField]
    public int ATS;
    [SerializeField]
    public int Bombs;
}
    public class Specification : MonoBehaviour
{
    public enum EntityType {
        None,
        Player,
        AI,
        Neutral,
        Ally,
        Enemy,
        Missile,
        Flare,
        Chaff,
        Tank,
        Ship,
        Human,
        Bullet
    }
    public float Temprature = 10f;
    public new string name = "object";
    public int size = 10;
    public int iff = 1111;
    public int M1 = 0000;
    public int M2 = 0000;
    public int M3 = 0000;

    public float health = 100;
    public string engine = "";
    public string exhaust = "";
    public EntityType entityType = EntityType.None;
    public WaypointsViewer waypointsViewer;
    public EntityInfoLog entityInfo;
    public Vector3 velocity = Vector3.one;//TestingPurpose
    public float currentVelo = 0;//TestingPurpose
    public float previosSpeed = 0;
    public float previosSpeedS = 0;
    public float deltaVelocity = 0;
    public float deltaTime = 0; 
    public float accel = 0;//TestingPurpose
    public float i, j, k;
    public WeaponLog weaponLog;
    public SilantroController silantro;
    public CombineUttam combineUttam;
    public NuteralPlaneController nuteralPlane;
    public ManualController manualController;
    public CarrierShipController carrierShipController;
    public GroundRadar groundRadar;
    public EW_Inputs ew_inputs;
    public CIT cit;
    public SilantroTurboFan turboFan;
    public Rigidbody rb;
    public Utility.LatLonAlt.LatLong latLong;
    private Vector3 previousVelocity;
    private void Start()
    {
        latLong = GameObject.FindObjectOfType<Utility.LatLonAlt.LatLong>();
        rb = gameObject.GetComponent<Rigidbody>();
        if( gameObject.TryGetComponent<SilantroController>(out SilantroController controllr) )
        {
            ew_inputs = GameObject.FindObjectOfType<EW_Inputs>();
            turboFan = GetComponentInChildren<SilantroTurboFan>();
            cit = gameObject.GetComponent<CIT>();
            silantro = controllr;
            entityType = EntityType.Player;
        }
        if (gameObject.TryGetComponent<ManualController>(out ManualController mControllr))
        {
            combineUttam = gameObject.GetComponent<CombineUttam>();
            manualController = mControllr;
            entityType = EntityType.AI;
        }
        if (gameObject.TryGetComponent<NuteralPlaneController>(out NuteralPlaneController neutral))
        {
            nuteralPlane = neutral;
            entityType = EntityType.Neutral;
        }
        if (gameObject.TryGetComponent<GroundRadar>(out GroundRadar gradar))
        {
            groundRadar = gradar;
            entityType = EntityType.Tank;
        }
        if (gameObject.TryGetComponent<CarrierShipController>(out CarrierShipController ship))
        {
            carrierShipController = ship;
            entityType = EntityType.Ship;
        }
        if (rb != null)
        {
            previousVelocity = rb.velocity;
        }
    }

    private void Update()
    {
        if (latLong != null)
        {
            EntityInfoLog log = new EntityInfoLog();
            WeaponLog wLog = new WeaponLog();
            Vector2 latlong = latLong.worldToLatLong(new Vector2(transform.position.x, transform.position.z));
            log.Lat = latlong.x;
            log.Long = latlong.y;
            log.Altitude = transform.position.y * 3.281f; //feet
            log.Heading = transform.eulerAngles.y;
            if(entityType == EntityType.Neutral)
            { 
                log.Speed =nuteralPlane.speed/ 1.852f;//knots
            }

            if (rb != null && entityType == EntityType.Ship)
            {
                var spd = rb.velocity.x * 0.592535f * 3.281f;
                spd = spd < 0 ? -spd : spd;
                log.Speed = spd;
            }
            if (rb!=null)
            {
                log.xVelocity = rb.velocity.x * 3.281f;//m/s to feet/s
                log.yVelocity = rb.velocity.y * 3.281f;//m/s to feet/s
                log.zVelocity = rb.velocity.z * 3.281f;//m/s to feet/s
                                                       // Get the current velocity
                Vector3 currentVelocity = rb.velocity;
                i = Vector3.Dot(currentVelocity, transform.forward);
                j = Vector3.Dot(currentVelocity, transform.right);
                k = Vector3.Dot(currentVelocity, transform.up);
                // Calculate the change in velocity
                Vector3 deltaVelocity = currentVelocity - previousVelocity;
                // Calculate the acceleration (change in velocity divided by the elapsed time)
                Vector3 acceleration = deltaVelocity / Time.deltaTime;
                //accel = ((currentVelocity.magnitude - previousVelocity.magnitude)/*deltaVelocity.magnitude*/ / Time.fixedDeltaTime)* Oyedoyin.MathBase.toKnots;
                //deltaVelocity = (vut - previousVelocity).magnitude;
                accel = (/*(currentVelocity.magnitude - previousVelocity.magnitude)   */deltaVelocity / Time.fixedDeltaTime).magnitude * 3.281f;
                previosSpeed = previousVelocity.magnitude;

                log.xAccel = acceleration.x * 3.281f;//m/s to feet/s
                log.yAccel = acceleration.y * 3.281f;//m/s to feet/s
                log.zAccel = acceleration.z * 3.281f;//m/s to feet/s 
                
                //log.xAccel = (accel * transform.right).x * 3.281f;//m/s to feet/s
                //log.yAccel = (accel * transform.up).y * 3.281f;//m/s to feet/s
                //log.zAccel = (accel * transform.forward).z * 3.281f;//m/s to feet/s 
                // Store the current velocity for the next frame
                previousVelocity = currentVelocity;
            }

            if (entityType == EntityType.Tank && groundRadar)
            {
                log.RadarRange = groundRadar.radarRange;
            }

            if (entityType == EntityType.Player && silantro.core && ew_inputs)
            {
                velocity = silantro.core.aircraft.velocity* 3.281f;
                currentVelo = silantro.core.aircraft.velocity.magnitude;
                previosSpeedS = silantro.core.previousSpeed;
                
                deltaTime = Time.fixedDeltaTime;
                 

                log.WaypointNumber = waypointsViewer.currentWaypoint;
                log.WaypointDistance = waypointsViewer.WD*6076.115f; //Nauticle mile to Feet
                log.WaypointBearing = waypointsViewer.WB;
                log.TimeOfArrival = waypointsViewer.TOA;
                log.Speed = silantro.core.currentSpeed * 1.944f;//knots
                log.IndicatedSpeed = silantro.core.currentSpeed * 1.944f;//knots
                log.TrueSpeed = silantro.core.trueSpeed * 1.944f;//knots
                log.GroundSpeed = silantro.core.groundSpeed * 1.944f;//knots
                log.Acceleration = silantro.core.acceleration * 3.281f;
                log.GForce = silantro.core.gForce;
                log.PitchRate = silantro.core.pitchRate;
                log.RollRate = silantro.core.rollRate;
                log.AirDensity = silantro.core.airDensity;
                log.EngineRPM = turboFan.core.functionalRPM;
                log.Temperature = silantro.core.ambientTemperature;
                log.EngineTemp = silantro.core.ambientTemperature;
                log.Pressure = silantro.core.ambientPressure;
                log.VerticleVelocity = silantro.core.verticalSpeed * 3.281f;//m/s to feet/s
                log.AngleOfAttack = silantro.core.α;
                log.Pitch = ew_inputs.pitchAngle;
                log.Roll = ew_inputs.rollAngle;
                log.Mach = silantro.core.machSpeed;
            }
            if (entityType == EntityType.AI && manualController)
            {
                log.Speed = manualController.Speed * 0.54003f;
                //log.Altitude = manualController.Altitude;
                log.Pitch = manualController.currentPitch;
                log.Roll = manualController.currentRoll;
                int bvr = 0;
                int ccm = 0;
                int ats = 0;
                foreach(var navig in combineUttam.missileNavigs)
                {
                    if(navig.Value.type == missileNavigation.Type.ATS)
                    {
                        ats++;
                    }else
                    if (navig.Value.missileType == missileNavigation.RangeType.BVR)
                    {
                        bvr++;
                    }
                    else
                    if (navig.Value.missileType == missileNavigation.RangeType.CCM)
                    { 
                        ccm++;
                    }
                }
                wLog.BVR = bvr;
                wLog.CCM = ccm;
                wLog.ATS = ats;

            }
            entityInfo = log;
            weaponLog = wLog;
        }
    }
}
