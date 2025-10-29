using Assets.Code.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SocialPlatforms;

public class UttamRadar : MonoBehaviour
{
    public enum Mode1
    {
        ATA,
        ATG,
        ATS
    };
    public enum Mode2
    {
        TWS,
        HPT,
        ACM,
        AJ,
        RA,
        WEATHER,
        HRM,
        GMTI,
        RBM,
        AGR,
        STWS,
        ISAR,
        STCT,
        RS
    };


    [Header("Transmitter Reciever Module")]
    public int TRM = 763;
    [Header("TRM Power (watt per TRM)")]
    public float TRMPower = 10f;
    [Header("Antenna Gain in db")]
    public float G = 1f;//antenna Gain
    [Header("Target Radar Cross Section")]
    public float r = 1f;//Radar radius
    [Header("Frequency in GHz")]
    public float f = 4f;//frequency
    [Header("Temperature in kelvin")]
    public float T = 290f;//kelvin
    [Header("Noise factor")]
    public float N = 1f;//Noise factor
    [Header("MinimumSignal to noise factor")]
    public float NR = 1f;//MinimumSignal to noise factor
    [Header("Output Range in Km")]
    public float Range = 10f;

    public float bandwidth;
    public float lamda;
    public double Pmin;
    public float gamma;
    public float Pt;
    //CIT code
    public int iff = 1112;

    //INPUT Parameters
    public Mode1 MainMode = Mode1.ATA;
    public Mode2 SubMode = Mode2.TWS;

    public float MaxAzimuth = 60f;
    public float Azimuth = 30f;


    public float MaxBar = 30f;
    public float Bar = 0f;

    public float MaxAzimuthElev = 30f;
    public float AzimuthElev = 0f;

    public float MaxElevation = 30f;
    public float Elevation = 0f;

    public float MinimumDetectSize = 3f;
    public float PhysicalMoveSpeed = 2f;
    public float DefaultHighThreatRange = 18f;
    public float HighThreatRange = 2f;
    public float MinThreatRange = 2f;
    public float VelocityThreshold = 500f;
    public float mile = 10000f;
    public float VisualRange = 0f;
    //pulse frequency
    public float frequency = 2f;
    public float pulseTime = 0.1f;
    public float peactimeWartimeAccuracy = 1f;
    public float applyPower = 1f;
    public int LockTime = 5;
    public bool CRM = false;
    public bool JEM = false;
    public bool SAR = false;
    public bool ABDutyCycle = false;
    public bool TxSil = false;
    public bool PassiveMode = false;
    public bool Cit = false;
    public bool AJ = false;
    public bool PtOnly = false;
    public float contrast =1f;

    [Header("Gun ")]
    public bool GACM = true;
    public Transform barrelA;
    public Transform barrelB;
    public bool hitActive = false;
    public Vector3 hitPoint = Vector3.zero;


    private float frame = 0f;
    public ColorGrading colorGrading;
    public meshcreate mesh;
    public meshcreate mesh2;
    public PostProcessVolume postProcessVolume;
    public List<string> ignoreObjects = new List<string>();
    public List<string> targetTagsAir = new List<string>();
    public List<string> targetTagsGround = new List<string>();
    public List<string> targetTagsSea = new List<string>();
    //OUTPUT Parameters
    public float Altitude = 0.5f;
    public Dictionary<GameObject,Vector3>FoundedObjects = new Dictionary<GameObject, Vector3>();
    public Dictionary<GameObject, Rigidbody> FoundedObjectsRB = new Dictionary<GameObject, Rigidbody>();
    public List<GameObject> DetectedObjects;
    public List<GameObject> SightObjects;
    private float jamTime = 0.5f;

    


    public GameObject SelectedTarget;
    public Specification SelectedTargetSpecification;
    public bool TargetLock = false;
    public bool tempLockTarget = false;
    public GameObject lockTargetProcess = null;
    public Vector3 HardLockTargetPosition = Vector3.zero;
    public GameObject HardLockTargetObject = null;

    public List<GameObject> LockTargetList = null;
    public Dictionary<GameObject, Vector3> LockTargetListLate = new Dictionary<GameObject, Vector3>();
    [NonSerialized] public float NoiseAngle;
    private Specification specs;

    [Header("CUED SEARCH INITIATION")]
    public bool cued = false;
    public float MinAltitude = 1000f;
    public float MaxAltitude = 2000f;
    public float MinSpeed = 200f;
    public float MaxSpeed = 800f;
    public float MinRange = 1f;
    public float MaxRange = 55f;

    [Header("DLZ (Dynamic launch zone)")]
    [Header("Velocity of Missile km/hr")]
    public float Vmissile = 100f;
    [Header("Range of Missile km/hr")]
    public float VRange = 100f;
    [Header("Missile burn Time in sec")]
    public float Tburn = 5f;
    [Header("Velocity of Enemy km/hr")]
    public float Venemy = 100f;
    [Header("Velocity of Shooter km/hr")]
    public float Vyou = 100f;
    [Header("Angle between Target and Shooter km/hr")]
    public float angle = 30f;
    public bool activeDLZ = false;
    public bool testDlz = false; 
    public float rmin2 = 20f;
    public float rmax2 = 80f;
    public float MissileMaxRange = 0;
    public bool missileSelect = false;
    public Rigidbody targetvelocity;
    public GameObject targetDlz;
    public WeaponTracker weaponTracker;

    [Header("Map Controls")]
    public Camera MapCamera;
    public Camera RBMCamera;
    public Camera WeatherCamera;
    public Camera TAMapCamera;
    public RenderTexture MapImage;
    public Transform CamraObject;
    public PostProcessVolume SarPostProcess;
    public PostProcessVolume IsarPostProcess;
    public PostProcessVolume RbmPostProcess;
    public WeatherMode WeatherMode;
    public int SarResolution = 80;
    public bool SarScan = false;
    public bool updateMap = false;
    [Range(-1f, 1f)]
    public float xPos = 0f;
    [Range(-1f, 1f)]
    public float yPos = 0f;
    [Range(1, 10f)]
    public float zoom = 0f;


    private Rigidbody rb;
    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        specs = transform.root.GetComponent<Specification>();
        //Filter Detected object List
        StartCoroutine(Filter());
        postProcessVolume = GetComponentInChildren<PostProcessVolume>();
        if (postProcessVolume!=null&& postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            // Modify the contrast intensity
            colorGrading.contrast.value = contrast;
        }
    }

    public void ManualLockTarget()
    {
        if (PassiveMode) return;

        if (SelectedTarget != null)
        {
            GameObject obj = SelectedTarget;
            TargetLock = true;
            lockTargetProcess = obj;
            HardLockTargetObject = obj;
            HardLockTargetPosition = obj.transform.position;
        }
    }


    public void ManualBreakLockTarget()
    {
        //SelectedTarget = null;
        TargetLock = false;
        lockTargetProcess = null;
        HardLockTargetObject = null;
        HardLockTargetPosition = Vector3.zero;

    }
    IEnumerator MapUpdate()
    {
        yield return new WaitForSeconds(SubMode==Mode2.ISAR?0.1f:0.7f);
        MapCamera.enabled = true;
        yield return new WaitForSeconds(0.1f);
        MapCamera.enabled = false;
        if (SarScan)
        {
            StartCoroutine(MapUpdate());
        }
    }

    private void FixedUpdate()
    {
        MapScreenUpdate();
    }


    private void LateUpdate()
    {
        MapScreenUpdate();
    }
    public void MapScreenUpdate()
    {
        float fov = (Azimuth * 2f) / zoom;
        MapCamera.fieldOfView = fov;
        if (SelectedTarget != null && (SAR || SubMode == Mode2.ISAR))
        {
            if (SarScan == false)
            {
                SarScan = true;
                SarResolution = 20;
                zoom = 70;
                StartCoroutine(MapUpdate());
            }
            // Step 1: Store the current local Euler angles
            Vector3 currentEulerAngles = MapCamera.transform.localEulerAngles;

            // Step 2: Make the object look at the target
            MapCamera.transform.LookAt(SelectedTarget.transform);

            // Step 3: Retrieve the new local Euler angles after LookAt
            Vector3 newEulerAngles = MapCamera.transform.localEulerAngles;

            // Step 4: Restore the original Z rotation
            newEulerAngles.z = currentEulerAngles.z;

            // Step 5: Apply the constrained Euler angles back to the transform
            //MapCamera.transform.localEulerAngles = newEulerAngles;

            //MapCamera.transform.LookAt(SelectedTarget.transform); 
            Vector3 eularAgle = newEulerAngles;
            eularAgle.x = newEulerAngles.x > 180 ? -(360 - newEulerAngles.x) : newEulerAngles.x;
            eularAgle.y = newEulerAngles.y > 180 ? -(360 - newEulerAngles.y) : newEulerAngles.y;
            float maxY = (Azimuth - (fov / 2));
            float maxX = (Bar - (fov / 2));
            //xPos = Mathf.Clamp(eularAgle.x, -maxX, maxX);
            //yPos = eularAgle.x;
            MapCamera.transform.localEulerAngles = new Vector3(Mathf.Clamp(eularAgle.x, -maxX, maxX), Mathf.Clamp(eularAgle.y, -maxY, maxY), 0);

        }
        else
        {
            MapCamera.enabled = true;
            zoom = 1;
            SarScan = false;
            SarResolution = 80;
            MapCamera.transform.localEulerAngles = new Vector3(xPos * (Azimuth - (fov / 2f)), yPos * (Azimuth - (fov / 2f)), 0);
        }
    }
    Vector3 CalculateHitPoint(Transform plane1,Transform plane2, Vector3 plane1Velocity,Vector3 plane2Velocity, float bulletSpeed)
    {
        Vector3 P1_0 = plane1.position;
        Vector3 P2_0 = plane2.position;

        Vector3 V1 = plane1Velocity;
        Vector3 V2 = plane2Velocity;

        Vector3 R_0 = P2_0 - P1_0;
        Vector3 V_r = V2 - V1;

        float a = Vector3.Dot(V_r, V_r) - bulletSpeed * bulletSpeed;
        float b = 2 * Vector3.Dot(R_0, V_r);
        float c = Vector3.Dot(R_0, R_0);

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            Debug.LogError("No solution exists for the given parameters.");
            return Vector3.zero;
        }

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float t1 = (-b + sqrtDiscriminant) / (2 * a);
        float t2 = (-b - sqrtDiscriminant) / (2 * a);

        float t = Mathf.Max(t1, t2);

        if (t < 0)
        {
            Debug.LogError("The intercept time is negative. Planes will not meet in the future.");
            return Vector3.zero;
        }

        Vector3 hitPoint = P2_0 + V2 * t;

        return hitPoint;
    }
    void Update()
    {
        //Radar calculation
         bandwidth = f * Mathf.Pow(10f, 9f);
         lamda = (3 * Mathf.Pow(10, 8)) / bandwidth;
         Pmin = (1.38 * Mathf.Pow(10, -23) * T * bandwidth * N * NR);//minimun detectable signal
         gamma = Mathf.PI * r * r;////Target Radar Cross Section/ radar aperture
         Pt = TRMPower * TRM * applyPower;//total consume power
        Range = Mathf.Sqrt(Mathf.Sqrt((float)((Pt * Mathf.Pow(G, 2) * Mathf.Pow(lamda, 2) * Mathf.Pow(gamma, 2)) / (Mathf.Pow(4f * Mathf.PI, 3) * Pmin))));
        Range = Mathf.Round(Range / (mile / 1000));

        //var bandwidth = f * Mathf.Pow(10f, 9f);
        //var lamda = (3 * Mathf.Pow(10, 8)) / bandwidth;
        //var Pmin = (1.38 * Mathf.Pow(10, -23) * T * bandwidth * N * NR);//minimun detectable signal
        //var gamma = Mathf.PI * r * r;////Target Radar Cross Section/ radar aperture
        //var Pt = TRMPower * TRM * applyPower;//total consume power
        //Range = Mathf.Sqrt(Mathf.Sqrt((float)((Pt * Mathf.Pow(G, 2) * Mathf.Pow(lamda, 2) * Mathf.Pow(gamma, 2)) / (Mathf.Pow(4f * Mathf.PI, 3) * Pmin))));
        //Range = Mathf.Round(Range);
        if (TxSil)
        {
            Range = 0.1f;
        }
        if (activeDLZ && HardLockTargetObject!=null&&missileSelect)
        {
            if (HardLockTargetObject != targetDlz)
            {
                targetDlz = HardLockTargetObject;
                targetvelocity = targetDlz.GetComponent<Rigidbody>();
            }

            Venemy =  targetvelocity.velocity.magnitude * 3.6f;
            
            if (testDlz&&transform.root.GetComponent<MoveDummy>())
            {
                Vyou = transform.root.GetComponent<MoveDummy>().speed * 3.6f;
            }
            else
            {
                Vyou = rb.velocity.magnitude * 3.6f;
            }

            angle = transform.root.eulerAngles.x;
            //DLZ equation
            MissileMaxRange = ((Vmissile * (Tburn / 3600)) / 2) * ((Venemy + Vyou) / Venemy) * Mathf.Cos(angle * (Mathf.PI / 180));
        }
        else
        {
            /*
            if (missileSelect)
            {
                HighThreatRange = VRange;
            }
            else
            {
                HighThreatRange = DefaultHighThreatRange;
            }
            */

        }

        Altitude = transform.position.y;
        iff = specs.iff;
        Quaternion radarRot = transform.localRotation;
        Quaternion CameraRot = CamraObject.localRotation;
        //update radar position
        if (true)//(Elevation <= MaxElevation && Elevation >= -MaxElevation)
        {
            float elv = Elevation;
            float azi = AzimuthElev;
            elv = (elv < 0 && elv < -MaxElevation) ? -MaxElevation : elv;
            elv = (elv > 0 && elv > MaxElevation) ? MaxElevation : elv;

            azi = (azi < 0 && azi < -MaxAzimuthElev) ? -MaxAzimuthElev : azi;
            azi = (azi > 0 && azi > MaxAzimuthElev) ? MaxAzimuthElev : azi;

            radarRot = Quaternion.RotateTowards(radarRot, Quaternion.Euler(elv, azi, 0f), PhysicalMoveSpeed * Time.fixedDeltaTime);
            if(MainMode == Mode1.ATA && CRM)
            {
                if (CameraRot != Quaternion.Euler(elv + MaxElevation, azi + MaxAzimuthElev, 0f))
                {
                    CameraRot = Quaternion.Euler(elv + MaxElevation, azi + MaxAzimuthElev, 0f);
                }
            }
            else
            {
                if(CameraRot != Quaternion.identity)
                {
                    CameraRot = Quaternion.identity;
                }

            }
        
        }

        transform.localRotation = radarRot;
        CamraObject.localRotation = CameraRot;


        pulseTime = ABDutyCycle ? 0.01f : 0.15f;

        if (frame >= frequency)
        {
            frame = 0f;
            mesh.Range = Range * mile;
            mesh.azimuth = Azimuth;
            mesh.bar = Bar;

            mesh2.Range = Range * mile;
            mesh2.azimuth = Azimuth;
            mesh2.bar = Bar;
            //RadarCollider.localScale = new Vector3(Azimuth,Bar, Range*mile);
            //MapCamera.sensorSize = new Vector2(Azimuth,Bar);
            //MapCamera.fieldOfView = Azimuth * 2f;

            MapCamera.farClipPlane = Range * mile;
            RBMCamera.fieldOfView = Azimuth * 2f;
            float range = Mathf.Clamp((VisualRange*mile) /2f,0,50000);
            WeatherCamera.orthographicSize = range;
            WeatherCamera.transform.localPosition = new Vector3(0, WeatherCamera.farClipPlane / 2, range);
            TAMapCamera.orthographicSize = range;
            TAMapCamera.transform.localPosition = new Vector3(0, TAMapCamera.farClipPlane / 2, range);
        }
        else
        {
            frame++;
            //transform.localScale = Vector3.one;
        }
        ///
        if (GACM&&HardLockTargetObject!=null)
        {
            hitActive = true;
            Transform plane1 = transform.root; 
            Transform plane2 = HardLockTargetObject.transform;
            Vector3 plane1Velocity = rb.velocity;
            Vector3 plane2Velocity = FoundedObjectsRB[HardLockTargetObject].velocity;
            float bulletSpeed = rb.velocity.magnitude + 1200;
            hitPoint = CalculateHitPoint(plane1, plane2, plane1Velocity, plane2Velocity, bulletSpeed);
            barrelA.LookAt(hitPoint);


            float angle = 0f;
            float x = barrelA.localEulerAngles.x;
            x = x > 180 ? (x - 360) : x;
            x = x > angle ? angle : (x < -angle ? -angle : x);
            float y = barrelA.localEulerAngles.y;
            y = y > 180 ? (y - 360) : y;
            y = y > angle ? angle : (y < -angle ? -angle : y);
            barrelA.localEulerAngles = new Vector3(x, y, 0f);

            barrelB.LookAt(hitPoint);
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
            hitActive = false;
            barrelA.localEulerAngles = Vector3.zero;
            barrelB.localEulerAngles = Vector3.zero;
        }


        //Activate Mode Function According Selected Mode
        if (SubMode == Mode2.HPT)
        {
            //HPT(DetectedObjects);
            AACM();
        }
        else if (SubMode == Mode2.ACM)
        {
            AACM(true);
        }/*
        else if (SubMode == Mode2.AGR)
        {
            if (MapImage.width != 300)
            {
                MapImage.Release();
                MapImage.width = 300;
                MapImage.height = 300;
                MapImage.Create();
            }
            GMTI(true);
        }*/
        else if (SubMode == Mode2.GMTI||SubMode == Mode2.RBM || SubMode == Mode2.AGR || SubMode == Mode2.HRM)
        {
            if (SarScan)
            {
                if (MapCamera.enabled)
                {
                    SarResolution += 2;
                }
                SarResolution = SarResolution > 400 ? 400 : SarResolution;
            }

            //RBM/GMTI
            int resolution = 80;
            //HRM+GMTI on RBM
            resolution = SubMode == Mode2.HRM?150:resolution;
            //SAR
            resolution = SarScan?SarResolution:resolution;

            if (MapImage.width != resolution)
            {
                if (MapCamera.enabled)
                {
                    MapImage.Release();
                    MapImage.width = resolution;
                    MapImage.height = resolution;
                    MapImage.Create();
                    MapCamera.Render();
                }
            }
            
           
            GMTI(SubMode != Mode2.GMTI);
        }/*
        else if (SubMode == Mode2.HRM)
        {
            if (MapImage.width != 900)
            {
                MapImage.Release();
                MapImage.width = 900;
                MapImage.height = 900;
                MapImage.Create();
            }
        }*/
        else if (SubMode == Mode2.STWS)
        {
            if (MapImage.width != 300)
            {
                MapImage.Release();
                MapImage.width = 300;
                MapImage.height = 300;
                MapImage.Create();
            }
        }
        else if (SubMode == Mode2.STCT)
        {
            if (MapImage.width != 300)
            {
                MapImage.Release();
                MapImage.width = 300;
                MapImage.height = 300;
                MapImage.Create();
            }
            STCT();
        }
        else if (SubMode == Mode2.ISAR)
        {
            if (MapImage.width != 800)
            {
                MapImage.Release();
                MapImage.width = 800;
                MapImage.height = 800;
                MapImage.Create();
            }
        }
        else
        {
            tempLockTarget = false;
            if (TargetLock)
            {
                if(HardLockTargetObject != null)
                {
                    HardLockTargetPosition = HardLockTargetObject.transform.position;
                }
                else
                {
                    TargetLock = false;
                    HardLockTargetPosition = Vector3.zero;
                    HardLockTargetObject = null;
                    lockTargetProcess = null;
                }
            }
            //TargetLock = false;
            //HardLockTargetPosition = Vector3.zero;
            //HardLockTargetObject = null;
            //lockTargetProcess = null;

            LockTargetList.Clear();
            LockTargetListLate.Clear();

            if (MapImage.width != 200)
            {
                MapImage.Release();
                MapImage.width = 200;
                MapImage.height = 200;
                MapImage.Create();
            }
        }
        jamTime = Mathf.Lerp(jamTime, 0, Time.fixedDeltaTime);
        if (jamTime < 0.05f)
        {
            NoiseAngle = 0;
            SightObjects.Clear();
        }
    }

    public void SelectNextTarget()
    {
        if(lockTargetProcess != null)
        {
            int i = LockTargetList.IndexOf(lockTargetProcess);
           // Debug.Log(i);
            i++;
            if (i < LockTargetList.Count)
            {
                TargetLock = true;
                lockTargetProcess = LockTargetList[i];
                HardLockTargetObject = LockTargetList[i];
                HardLockTargetPosition = HardLockTargetObject.transform.position;
            }
            else
            {
                i = 0;
                TargetLock = true;
                lockTargetProcess = LockTargetList[i];
                //Debug.Log(i);
                HardLockTargetObject = LockTargetList[i];
                HardLockTargetPosition = HardLockTargetObject.transform.position;
            }
        }
    }

    //Objects Detected By Radar Collider
    public void Trigger(Collider other)
    {


        //Debug.Log(FoundedObjectsRB.Count);
        if (!FoundedObjects.ContainsKey(other.gameObject))
        {
            if (MainMode == Mode1.ATA && targetTagsAir.Contains(other.gameObject.tag))
            {
                if (!SightObjects.Contains(other.gameObject))
                {
                    FoundedObjects.Add(other.gameObject,other.transform.position);
                    if(!FoundedObjectsRB.ContainsKey(other.gameObject))
                    FoundedObjectsRB.Add(other.gameObject, other.GetComponent<Rigidbody>());    
                }
                    
                    //DetectedObjects.Add(other.gameObject);
            }

            if ((MainMode == Mode1.ATG||CRM) && targetTagsGround.Contains(other.gameObject.tag))
            {
                if (!SightObjects.Contains(other.gameObject))
                {
                    FoundedObjects.Add(other.gameObject, other.transform.position);
                    FoundedObjectsRB.Add(other.gameObject, other.GetComponent<Rigidbody>());
                }
                //DetectedObjects.Add(other.gameObject);
            }
            
            if ((MainMode == Mode1.ATS || CRM) && targetTagsSea.Contains(other.gameObject.tag))
            {
                
                if (!SightObjects.Contains(other.gameObject))
                {
                    FoundedObjects.Add(other.gameObject, other.transform.position);
                    FoundedObjectsRB.Add(other.gameObject, other.GetComponent<Rigidbody>());
                }
                //DetectedObjects.Add(other.gameObject);
            }


        }
        //if (!SightObjects.Contains(other.gameObject) && targetTags.Contains(other.gameObject.tag))
        //{
        //    SightObjects.Add(other.gameObject);

        //}

        Dictionary<GameObject, Vector3> list = new Dictionary<GameObject, Vector3>(FoundedObjects);
        foreach (var obj in list.Keys)
        {
            if (obj == null || obj.transform.root.gameObject.activeSelf == false)
            {
                FoundedObjectsRB.Remove(obj);
                FoundedObjects.Remove(obj);
                //DetectedObjects.Remove(obj);

                if (DetectedObjects.Contains(obj))
                {
                    DetectedObjects.Remove(obj);
                }
            }
            else
            {
                //Specification specification = obj.GetComponent<Specification>();
                //if(specification != null)
                //{
                //    if (specification.size < MinimumDetectSize)
                //    {
                //        DetectedObjects.Remove(obj);
                //    }
                //}
            }
        }

        //Passive mode for survilliance
        if (PassiveMode)
            PassiveFilter();

        //Cued Search Init
        if (cued)
            CuedFilter();

        

        //for (int i = 0; i < DetectedObjects.Count; i++)
        //{
        //    if (!SightObjects.Contains(DetectedObjects[i]))
        //    {
        //        DetectedObjects.Remove(DetectedObjects[i]);
        //    }
        //}


    }

    public void PassiveFilter()
    {
        Dictionary<GameObject, Vector3> list = new Dictionary<GameObject, Vector3>(FoundedObjects);
        foreach (var obj in list.Keys)
        {
            if (obj != null && obj.activeSelf == true)
            {
                AIRadar radar = obj.GetComponentInChildren<AIRadar>();
                if (radar && radar.DetectedObjects.Contains(transform.root.gameObject))
                {
                    
                }
                else
                {

                    FoundedObjectsRB.Remove(obj);
                    FoundedObjects.Remove(obj);
                    //DetectedObjects.Remove(obj);

                    if (DetectedObjects.Contains(obj))
                    {
                        DetectedObjects.Remove(obj);
                    }
                }
            }
        }
    }

    public void CuedFilter()
    {
        Dictionary<GameObject, Vector3> list = new Dictionary<GameObject, Vector3>(FoundedObjects);
        foreach (var obj in list.Keys)
        {
            if (obj != null && obj.activeSelf == true)
            {
                float altitude = obj.transform.position.y* 3.281f;//In ft
                float Speed = FoundedObjectsRB[obj].velocity.magnitude*1.944f;//In knot miles
                float Range = Vector3.Distance(obj.transform.position,transform.position)/1852f;//In nautical miles
                if (altitude >= MinAltitude && altitude < MaxAltitude &&
                    Speed >= MinSpeed && Speed < MaxSpeed &&
                    Range >= MinRange && Range < MaxRange)
                {

                }
                else
                {

                    FoundedObjectsRB.Remove(obj);
                    FoundedObjects.Remove(obj);
                    //DetectedObjects.Remove(obj);

                    if (DetectedObjects.Contains(obj))
                    {
                        DetectedObjects.Remove(obj);
                    }
                }
            } 
        }
    }

    //Objects Remove After Disapper
    public void TriggerExit(Collider other)
    {
        if (FoundedObjects.ContainsKey(other.gameObject))
        {
            FoundedObjectsRB.Remove(other.gameObject);
            FoundedObjects.Remove(other.gameObject);
            
            //DetectedObjects.Remove(other.gameObject);
            //Debug.Log(other.name + " exit");
            if (DetectedObjects.Contains(other.gameObject))
            {
                DetectedObjects.Remove(other.gameObject);
            }
        }
        //if (SightObjects.Contains(other.gameObject))
        //{
        //    SightObjects.Remove(other.gameObject);
        //    //Debug.Log(other.name + " exit");
        //}
    }



    IEnumerator Filter()
    {
        yield return new WaitForSeconds(0.1f);
        Dictionary<GameObject, Vector3> list = new Dictionary<GameObject, Vector3>(FoundedObjects);
        foreach(var obj in list.Keys)
        {
            if (obj == null || obj.transform.root.gameObject.activeSelf == false)
            {
                FoundedObjectsRB.Remove(obj);
                FoundedObjects.Remove(obj);
                
                //DetectedObjects.Remove(obj);
                if (DetectedObjects.Contains(obj))
                {
                    DetectedObjects.Remove(obj);
                }
            }
        }

        List<GameObject> lst = new List<GameObject>(DetectedObjects);
        foreach(var obj in lst)
        {
            if (!FoundedObjects.ContainsKey(obj))
            {
                DetectedObjects.Remove(obj);
            }
        }

        //Passive mode for survilliance
        if (PassiveMode)
            PassiveFilter();

        //Cued Search Init
        if (cued)
            CuedFilter();
        //for (int i = 0; i < list.Count; i++)
        //{
        //    var obj = list[i];
        //    if (obj == null || obj.activeSelf == false)
        //    {
        //        FoundedObjects.Remove(obj);
        //        //DetectedObjects.Remove(obj);
        //        //if (DetectedObjects.Contains(obj))
        //        //{
        //        //    DetectedObjects.Remove(obj);
        //        //}
        //    }
        //}

        //for (int i = 0; i < DetectedObjects.Count; i++)
        //{
        //    if (!SightObjects.Contains(DetectedObjects[i]))
        //    {
        //        DetectedObjects.Remove(DetectedObjects[i]);
        //    }
        //}

        Dictionary<GameObject, Vector3> lists = new Dictionary<GameObject, Vector3>(FoundedObjects);
        foreach (var obj in lists)
        {
            //float objectSpeed = ((Vector3.Distance(obj.Value,obj.Key.transform.position)/0.1f)*10f)*3.6f;
            float objectSpeed = FoundedObjectsRB[obj.Key].velocity.magnitude*3.6f;
            float distancepre = (Vector3.Distance(obj.Value, transform.root.position));
            FoundedObjects[obj.Key]=obj.Key.transform.position;
            float distance = Vector3.Distance(obj.Key.transform.position, transform.root.position);
            float speed = (distancepre-distance);
            speed = speed < 0 ? -objectSpeed / 3.6f : objectSpeed; 
            float threshold = (((Range*mile)-distance)* pulseTime* peactimeWartimeAccuracy) *(1+(speed/500f));
            //Debug.Log(distancepre + " ," + distance + " ," + speed + " ," + threshold );
            //Debug.Log(objectSpeed);
            if (!DetectedObjects.Contains(obj.Key))
            {
                if (threshold > 2000f) {
                    if (objectSpeed >= VelocityThreshold)
                    {
                        DetectedObjects.Add(obj.Key);
                    }
                }
                
            }
            else
            {
                if (!FeedBackRecorderAndPlayer.isPlaying || true)
                {
                    if (threshold < 2000f)
                    {
                        DetectedObjects.Remove(obj.Key);
                    }

                    if (objectSpeed < VelocityThreshold)
                    {
                        DetectedObjects.Remove(obj.Key);
                    }
                }
            }
            
        }

        if(HardLockTargetObject != null&&!DetectedObjects.Contains(HardLockTargetObject))
        {
            TargetLock = false;
            lockTargetProcess = null;
            HardLockTargetObject = null;
            HardLockTargetPosition = Vector3.zero;
        }

        if(SelectedTarget != null && !DetectedObjects.Contains(SelectedTarget))
        {
            SelectedTarget = null;
        }

        StartCoroutine(Filter());
    }


    //Function for Auto-Acquisition/Air Combat maneuvering
    /*
     * filter and track object basis on
     * Speed, Direction, Distance
     * 
     */

    public void clear()
    {
        FoundedObjects.Clear();
        FoundedObjectsRB.Clear();
        DetectedObjects.Clear();
        SightObjects.Clear();
        //TargetLock = false;
        tempLockTarget = false;
        //lockTargetProcess = null;
        //HardLockTargetPosition = Vector3.zero;
        //HardLockTargetObject = null;

        LockTargetList.Clear();
        LockTargetListLate.Clear();
    }

    public bool CitCheck(GameObject obj)
    {
        
        if (Cit)
        {
            if (obj != null)
            {
                Specification specs = obj.GetComponent<Specification>();
                if (specs != null)
                {
                    return specs.iff == iff;
                }
            }

        }
        return false;
    }

    public void AACM(bool lok = false)
    {
        for (int i = 0; i < DetectedObjects.Count; i++)
        {
            

            if (DetectedObjects[i]==null||ignoreObjects.Contains(DetectedObjects[i].tag)|| CitCheck(DetectedObjects[i])) { continue; }
            if (i >= 6 || DetectedObjects[i] == null) { break; }
            var obj = DetectedObjects[i];
            var d = Vector3.Distance(obj.transform.position, transform.position);
            var directionDistance = Vector3.Distance(obj.transform.position + (obj.transform.forward * d), transform.position);
            var speed = 0f;
            if (LockTargetListLate.ContainsKey(obj))
            {
                speed = (LockTargetListLate[obj] - obj.transform.position).magnitude / Time.deltaTime;
                LockTargetListLate[obj] = obj.transform.position;
            }

            //Debug.Log(speed);
            if ((d < (HighThreatRange * mile) || directionDistance < Azimuth / 10) &&d> MinThreatRange*mile /*&& speed >5f*/ && !LockTargetList.Contains(obj))
            {
                LockTargetList.Add(obj);
                LockTargetListLate.Add(obj, obj.transform.position);
            }
            else if ((d > (HighThreatRange * mile) || d < MinThreatRange * mile) && LockTargetList.Contains(obj))
            {
                LockTargetList.Remove(obj);
                LockTargetListLate.Remove(obj);
            }
        }
        for (int i = 0; i < LockTargetList.Count; i++)
        {
            var obj = LockTargetList[i];
            if (obj == null)
            {
                LockTargetList.Remove(obj);
                LockTargetListLate.Remove(obj);
            }
            else
            {
                /* customize condition */
                bool ignore = (ignoreObjects.Contains(obj.tag) && CitCheck(DetectedObjects[i])) || obj.tag.ToLower().Contains("missile");


                if (obj.activeSelf == false || !DetectedObjects.Contains(obj) || ignore)
                {
                    LockTargetList.Remove(obj);
                    LockTargetListLate.Remove(obj);
                }
            }
            
        }
        if (lok)
        {
            HPT(LockTargetList);
        }



    }

    //Function  High priority Target
    /*
     * filter and hard lock Object based on high priorty
     * mainly (Closest object and type (missile , plane))
     * 
     */
    public void HPT(List<GameObject> DetectedObjects)
    {

        if (tempLockTarget)
        {

            if (lockTargetProcess)
            {
                var lockAngle = Vector3.Angle(lockTargetProcess.transform.position - transform.position, transform.forward);
                if (!DetectedObjects.Contains(lockTargetProcess))// || lockAngle > 15f)
                {
                    TargetLock = false;
                    HardLockTargetPosition = Vector3.zero;
                    HardLockTargetObject = null;
                    lockTargetProcess = null;
                    tempLockTarget = false;
                }
                else
                {
                    return;
                }
            }
            else
            {
                TargetLock = false;
                HardLockTargetPosition = Vector3.zero;
                HardLockTargetObject = null;
                lockTargetProcess = null;
                tempLockTarget = false;
            }


        }

        var max = 0f;
        GameObject loc = null;
        foreach (var obj in DetectedObjects)
        {
            if (obj == null || obj.tag.ToLower().Contains("missile")) continue;
            var d = Vector3.Distance(obj.transform.position, transform.position);
            var lockAngle = Vector3.Angle(obj.transform.position - transform.position, transform.forward);
            bool ignor = (ignoreObjects.Contains(obj.tag) && CitCheck(obj));

            if ((d < max || max == 0f) && !ignor)// && lockAngle < 15f)
            {
                max = d;
                loc = obj;
            }
        }
        
        if (loc != null )
        {
            bool ignore = (ignoreObjects.Contains(loc.tag) && CitCheck(loc));
            if (!ignore)
            {
                tempLockTarget = true;
                //HardLockTargetPosition = loc.transform.position;
                lockTargetProcess = loc;
                StartCoroutine(lockProcess(loc));
            }
            else
            {
                tempLockTarget = false;
            }

        }
        else
        {
            tempLockTarget = false;
        }

    }

    IEnumerator lockProcess(GameObject obj)
    {
        //Debug.Log("process");
        int i = LockTime;
        bool lok = true;
        while (i > 0)
        {
            yield return new WaitForSeconds(1f);
            if (!tempLockTarget || obj != lockTargetProcess)
            {
                lok = false;
                i = 0;
                break;
            }
            //if(Vector3.Angle(obj.transform.position - transform.position, transform.forward)<15)
            i--;
        }
        if (lok)
        {
            TargetLock = true;
            HardLockTargetPosition = lockTargetProcess.transform.position;
            HardLockTargetObject = lockTargetProcess;
            if(weaponTracker != null)
            {
                //weaponTracker.AutoMissileSelect();
            }
        }

    }



    //Function for Ground Moving Target Tracing/Indication
    /*
     * filter and track object basis on
     * type "Tank , Car , Airbase"
     * 
     */
    public void GMTI(bool val=false)
    {
        if (!val)
        {
            TargetLock = false;
            HardLockTargetObject = null;
            LockTargetList.Clear();
            LockTargetListLate.Clear();
            return;
        }

        for (int i = 0; i < DetectedObjects.Count; i++)
        {

            if (LockTargetList.Count>6|| DetectedObjects[i] == null) { break; }
            var obj = DetectedObjects[i];
            var d = Vector3.Distance(obj.transform.position, transform.position);
            var directionDistance = Vector3.Distance(obj.transform.position + (obj.transform.forward * d), transform.position);
            var speed = 0f;
            if (LockTargetListLate.ContainsKey(obj))
            {
                speed = (LockTargetListLate[obj] - obj.transform.position).magnitude / Time.deltaTime;
                LockTargetListLate[obj] = obj.transform.position;
            }

       
            //Debug.Log(speed);
            if (/*(directionDistance < Azimuth / 10) &&*/ !LockTargetList.Contains(obj) && (obj.tag == "EnemyTank" || obj.tag == "EnemyAirBase" || obj.tag == "EnemyBase" || obj.tag == "EnemyRadar"))
            {
                LockTargetList.Add(obj);
                LockTargetListLate.Add(obj, obj.transform.position);
            }
            //else if (LockTargetList.Contains(obj))
            //{
            //    LockTargetList.Remove(obj);
            //    LockTargetListLate.Remove(obj);
            //    Debug.Log("exit");
            //}
        }
        for (int i = 0; i < LockTargetList.Count; i++)
        {
            var obj = LockTargetList[i];
            if (obj == null || obj.activeSelf == false || !DetectedObjects.Contains(obj) || ignoreObjects.Contains(obj.tag))
            {
                LockTargetList.Remove(obj);
                LockTargetListLate.Remove(obj);
            }
        }

        if(val)
        GHPT(LockTargetList);


    }

    //High Priorty for Ground Similar as Air to Air
    public void GHPT(List<GameObject> DetectedObjects)
    {
        var max = 30000f;
        if (TargetLock)
        {
            if (!DetectedObjects.Contains(HardLockTargetObject) || HardLockTargetObject == null)
            {
                tempLockTarget = false;
                lockTargetProcess = null;
                TargetLock = false;
                HardLockTargetPosition = Vector3.zero;
                HardLockTargetObject = null;
                return;
            }

            var h = transform.position.y;
            var g = 9.8f;
            var plane = transform.parent.parent.GetComponent<Rigidbody>();
            float localForwardVelocity = Vector3.Dot(plane.velocity, transform.parent.parent.forward);
            var range = localForwardVelocity * Mathf.Sqrt((2 * h) / g);

            var p = new Vector2(HardLockTargetObject.transform.position.x, HardLockTargetObject.transform.position.z);
            //var t = new Vector2(transform.position.x, transform.position.z);
            var t = transform.parent.parent.position + (transform.parent.parent.forward * range);
            t = new Vector2(t.x, t.z);
            var d = Vector2.Distance(p, t);
            if (!DetectedObjects.Contains(HardLockTargetObject) || d > max||HardLockTargetObject==null)
            {
                tempLockTarget = false;
                lockTargetProcess = null;
                TargetLock = false;
                HardLockTargetPosition = Vector3.zero;
                HardLockTargetObject = null;
            }
            else
            {
                return;
            }
        }


        GameObject loc = null;
        var i = 0;
        foreach (var obj in DetectedObjects)
        {
            var h = transform.position.y;
            var g = 9.8f;
            var plane = transform.parent.parent.GetComponent<Rigidbody>();
            float localForwardVelocity = Vector3.Dot(plane.velocity, transform.parent.parent.forward);
            var range = localForwardVelocity * Mathf.Sqrt((2 * h) / g);

            var p = new Vector2(obj.transform.position.x, obj.transform.position.z);
            //var t = new Vector2(transform.position.x, transform.position.z);
            var t = transform.parent.parent.position + (transform.parent.parent.forward * range);
            t = new Vector2(t.x, t.z);
            var d = Vector2.Distance(p, t);
            //Debug.Log(d + "," + range + "," + t.y);
            if ((d < max /*|| max == 0f*/))
            {
                max = d;
                loc = obj;
            }
            i++;
        }
        if (loc != null && !ignoreObjects.Contains(loc.tag))
        {

            //TargetLock = true;
            //HardLockTargetPosition = loc.transform.position;
            //HardLockTargetObject = loc;
            tempLockTarget = true;
            lockTargetProcess = loc;
            StartCoroutine(lockProcess(loc));

        }
        else
        {
            TargetLock = false;
        }

        //return new List<GameObject>();
    }

    //Function for Ground Moving Target Tracing/Indication
    /*
     * filter and track object basis on
     * type "Ship , Stone , warship"
     * 
     */
    public void STCT()
    {
        for (int i = 0; i < DetectedObjects.Count; i++)
        {
            if (i >= 6) { break; }
            var obj = DetectedObjects[i];
            var d = Vector3.Distance(obj.transform.position, transform.position);
            var directionDistance = Vector3.Distance(obj.transform.position + (obj.transform.forward * d), transform.position);
            var speed = 0f;
            if (LockTargetListLate.ContainsKey(obj))
            {
                speed = (LockTargetListLate[obj] - obj.transform.position).magnitude / Time.deltaTime;
                LockTargetListLate[obj] = obj.transform.position;
            }
            //Debug.Log(obj.tag);
            //Debug.Log(speed);
            if (!LockTargetList.Contains(obj) && (obj.tag == "EnemyShip" || obj.tag == "EnemyWarship"))
            {
                LockTargetList.Add(obj);
                LockTargetListLate.Add(obj, obj.transform.position);
            }
            //else if (LockTargetList.Contains(obj))
            //{
            //    LockTargetList.Remove(obj);
            //    LockTargetListLate.Remove(obj);
            //}
        }
        for (int i = 0; i < LockTargetList.Count; i++)
        {
            var obj = LockTargetList[i];
            if (obj == null || obj.activeSelf == false || !DetectedObjects.Contains(obj) || ignoreObjects.Contains(obj.tag))
            {
                LockTargetList.Remove(obj);
                LockTargetListLate.Remove(obj);
            }
        }

        SHPT(LockTargetList);


    }

    //High Priorty for Sea Similar as Air to Air
    public void SHPT(List<GameObject> DetectedObjects)
    {

        if (TargetLock)
        {
            if (!DetectedObjects.Contains(HardLockTargetObject) ||  HardLockTargetObject == null)
            {
                tempLockTarget = false;
                lockTargetProcess = null;
                TargetLock = false;
                HardLockTargetPosition = Vector3.zero;
                HardLockTargetObject = null;
                return;
            }
            var h = transform.position.y;
            var g = 9.8f;
            var plane = transform.parent.parent.GetComponent<Rigidbody>();
            float localForwardVelocity = Vector3.Dot(plane.velocity, transform.parent.parent.forward);
            var range = localForwardVelocity * Mathf.Sqrt((2 * h) / g);

            var p = new Vector2(HardLockTargetObject.transform.position.x, HardLockTargetObject.transform.position.z);
            //var t = new Vector2(transform.position.x, transform.position.z);
            var t = transform.parent.parent.position + (transform.parent.parent.forward * range);
            t = new Vector2(t.x, t.z);
            var d = Vector2.Distance(p, t);
            if (!DetectedObjects.Contains(HardLockTargetObject) || d > 10000f|| HardLockTargetObject == null)
            {
                tempLockTarget = false;
                lockTargetProcess = null;
                TargetLock = false;
                HardLockTargetPosition = Vector3.zero;
                HardLockTargetObject = null;

            }
            else
            {
                return;
            }
        }

        var max = 0f;
        GameObject loc = null;
        var i = 0;
        foreach (var obj in DetectedObjects)
        {
            //var d = Vector3.Distance(obj.transform.position, transform.position);
            var size = obj.transform.lossyScale.x * obj.transform.lossyScale.y * obj.transform.lossyScale.z;
            var h = transform.position.y;
            var g = 9.8f;
            var plane = transform.parent.parent.GetComponent<Rigidbody>();
            float localForwardVelocity = Vector3.Dot(plane.velocity, transform.parent.parent.forward);
            var range = localForwardVelocity * Mathf.Sqrt((2 * h) / g);

            var p = new Vector2(obj.transform.position.x, obj.transform.position.z);
            //var t = new Vector2(transform.position.x, transform.position.z);
            var t = transform.parent.parent.position + (transform.parent.parent.forward * range);
            t = new Vector2(t.x, t.z);
            var d = Vector2.Distance(p, t);
            //Debug.Log(d + "," + range + "," + t.y);
            if (obj.tag == "EnemyShip" /*&& size > max*/ && d < 10000f)
            {
                max = size;
                loc = obj;
            }
            i++;
        }
        if (loc != null && !ignoreObjects.Contains(loc.tag))
        {
            //TargetLock = true;
            //HardLockTargetPosition = loc.transform.position;
            //HardLockTargetObject = loc;
            //StartCoroutine(lockProcess(loc));

            tempLockTarget = true;
            lockTargetProcess = loc;
            StartCoroutine(lockProcess(loc));
        }
        else
        {
            TargetLock = false;
        }

        //return new List<GameObject>();
    }
    public void JamSignal(Jammer.JamMode mode, Jammer.JamFrequency frequency, Jammer.JamPower power, GameObject jammer)
    {
        if(mode == Jammer.JamMode.Barring)
        {
            if((int)power > G)
            {
                DetectedObjects.Clear();
            }

            return;
        }

        if (((int)frequency + 8 == f||mode == Jammer.JamMode.DRFMjamming) && (int)power > G&&!AJ)//JamValid or Not
            if (((int)frequency + 8 == f || mode == Jammer.JamMode.DRFMjamming) && (int)power > G)//JamValid or Not
            {
                Vector3 error = (Quaternion.Inverse(transform.rotation) * (jammer.transform.position - transform.position));
                NoiseAngle = -(Vector2.SignedAngle(new Vector2(error.x, error.z), Vector2.one) + 50);
                var jamAngle = Vector3.SignedAngle(jammer.transform.position - transform.position, transform.forward, transform.up);
            foreach (GameObject aircraft in DetectedObjects)
            {
                var anglediff = Vector3.SignedAngle(aircraft.transform.position - transform.position, transform.forward, transform.up) - jamAngle;
                if ((anglediff > 0 ? anglediff : -anglediff) < 3)
                {
                    SightObjects.Add(aircraft);
                    if (!SightObjects.Contains(aircraft))
                       StartCoroutine(JamIT(aircraft));
                }
             }
            int count = SightObjects.Count;
            for (int i =0;i<count;i++)
            {
                    if (i < SightObjects.Count)
                    {
                        var aircraft = SightObjects[i];
                        if (aircraft == null) continue;
                        var anglediff = Vector3.SignedAngle(aircraft.transform.position - transform.position, transform.forward, transform.up) - jamAngle;
                        if ((anglediff > 0 ? anglediff : -anglediff) < 3)
                        {

                        }
                        else
                        {
                            SightObjects.Remove(aircraft);
                        }
                        DetectedObjects.Remove(aircraft);
                    }
            }
            jamTime = 1;
            }
            else { SightObjects.Clear(); }
    }
    IEnumerator JamIT(GameObject aircraft)
    {
        yield return new WaitForSeconds(0.1f);
        SightObjects.Add(aircraft);
    }
}