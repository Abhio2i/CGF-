using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EW.Flare;
using Weapon.MissileAction;
using System;
using Assets.Scripts.Feed;
using Assets.Code.Plane;
using TMPro;
using Vazgriz.Plane;
using UnityEngine.UI;
using AirPlane.Radar;

[Serializable]
public class  EWRadarLog
{
    [SerializeField]
    public float Radar_Range;
    [SerializeField]
    public float Flares;
    [SerializeField]
    public float Chaffs;
    [SerializeField]
    public float chaffBurstSize;
    [SerializeField]
    public float flareBurstSize;
    [SerializeField]
    public bool autoFlare;
    [SerializeField]
    public bool autoChaff;
    [SerializeField]
    public bool autoDircm;
    [SerializeField]
    public float AutoFlareActiveRange;
    [SerializeField]
    public float AutoChaffActiveRange;
    [SerializeField]
    public float Pt;
    [SerializeField]
    public float Gt;
    [SerializeField]
    public float f;
    [SerializeField]
    public float Gr;
    [SerializeField]
    public float Pr;
    [SerializeField]
    public bool MissileWarning;
    [SerializeField]
    public List<Vector3> detect = new List<Vector3>();
    [SerializeField]
    public List<string> info = new List<string>();

}
public class EWRadar : MonoBehaviour
{
    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    public MasterSpawn masterspawn;
    public MissionPlan missionplan;
    public LaserShoot DIRCM_Module;
    public GameObject flare, chaff;
    public bool Enemy = false,Player =false;
    public Toggle chaffToggle;
    public Toggle FlareToggle;
    public Toggle DircmToggle;
    public List<string> TargetsList = new List<string>()
    { "Player","Missile","EnemyMissile","EnemyPlane","Enemy","TestMissile","TestEnemy","Neutral"};
    
    [Header("To Observe")]
    public float radar_Range = 2000f, flareActivateRange = 750f, chaffActivateRange = 750f;
    public int maxFlares = 30, maxChaff = 60, burstChaffSize = 24, burstFlareSize = 24;
    public bool autoFlareFire = false, autoChaffFire = false;
    public List<GameObject> objectsDetected = new List<GameObject>();
    public Dictionary<GameObject,float> JamObjects = new Dictionary<GameObject, float>();

    [Header("For EW Test Only")]
    public bool testing = false;
    public TextMeshProUGUI flareText;
    public TextMeshProUGUI chaffText;
    public TextMeshProUGUI missileText;
    

    public static bool LeftRightEqualFlares = true;
    [NonSerialized] public GameObject LatestDetected;
    [NonSerialized] public bool QuickUpdate = false, ListUpdated = false;
    private int temp;
    private float closestMissile;
    private GameObject closestPosition;
    private SphereCollider RadarCollider;
    private List<Transform> FlarePos = new List<Transform>();
    private List<GameObject> FlaresPool = new List<GameObject>();
    private List<GameObject> ChaffsPool = new List<GameObject>();
    private PlaneData data;
    private Jammer jammer;
    public ManualController controller;
    public EWRadarLog ewRadarLog;

    [Header("TARGET RADAR ESTIMATED VALUES")]
    [Header("Power (watt)")]
    [Tooltip("Transmitted Output Power by Radar")]
    public float Pt = 1000f;
    [Header("Gain (dbm)")]
    [Tooltip("Transmitted Antenna Gain by Radar")]
    public float Gt = 2f;
    [Header("Frequency (MHz)")]
    [Tooltip("Transmitted Frequency in (MHz) by Radar")]
    public float f = 2f;
    [Header("EW RADAR CONFIG VALUES")]
    [Header("Recieve Gain (dB)")]
    [Tooltip("Receiving Antenna Gain")]
    public float Gr = 2f;
    [Header("ReceivePower (dbm) and Threshold")]
    [Range(-1f, -118.9f)]
    public double Pr = 1000f;
    //[Header("Set Distance (km)")]
    //[Tooltip("Distance in km from target")]
    //public float d = 2f;

    [Header("ResolveDistance (km)")]
    [Tooltip("Distance in km from target")]
    [ReadOnlyWhenPlaying]
    public float D = 2f;
    [ReadOnlyWhenPlaying]
    public float PT = 2f;
    //[Header("ResolveReceivePower (dbm)")]
    //[ReadOnlyWhenPlaying]
    //public double PR = 1000f;
    [Header("Formula Range = exp( PT + GT - 32.4 - 20log(f) + Gr - Pr / 20 )")]
    [ReadOnlyWhenPlaying]
    public string formula = "exp( PT + GT - 32.4 - 20log(f) + Gr - Pr / 20 )";

    [Header("external update")]
    public bool AutoUpdate = false;


    void Start()
    {
        data = GetComponentInParent<PlaneData>();       
        controller = transform.root.GetComponent<ManualController>();
        jammer = GetComponent<Jammer>();
        for (int i = 0; i < 4; i++)
        {
            FlarePos.Add(transform.GetChild(1).GetChild(i));
        }
        //RadarCollider = gameObject.AddComponent<SphereCollider>();
        //RadarCollider.isTrigger = true;
        if (Player&&!testing && missionplan!=null)
        {

            maxFlares = missionplan.ally_spawnPlanes[0].Flares;
            maxChaff = missionplan.ally_spawnPlanes[0].Chaffs;
            burstChaffSize = 4;
            burstFlareSize = 4;
            autoChaffFire = missionplan.ally_spawnPlanes[0].AutoChaffs;
            autoFlareFire = missionplan.ally_spawnPlanes[0].AutoFlares;
            DIRCM_Module.active = missionplan.ally_spawnPlanes[0].DIRCM; 
            chaffToggle.isOn = autoChaffFire;
            FlareToggle.isOn = autoFlareFire;
            DircmToggle.isOn = DIRCM_Module.active;
        }
        if (jammer.JammerActive)
        {
            StartCoroutine(JammerSwitch());
        }
        //DIRCM_Module.Range = radar_Range/1.2f;
        //DIRCM_Module.EffectRange = DIRCM_Module.Range / 1.2f;
        SetRangeRWR();
        PoolFlaresAndChaffs();
        StartCoroutine("ClearList");    //To Refresh the list automatically
    }

    public float getRange()
    {
        PT = 10 * Mathf.Log(Pt / 0.001f);//watt to dBm
        //PR = PT + Gt - 32.4 - (20 * Mathf.Log(f)) - (20 * Mathf.Log(d)) + Gr;
        D = Mathf.Exp((float)(PT + Gt - 32.4 - (20 * Mathf.Log(f)) + Gr - Pr) / 20f);
        return D;
    }



    void PoolFlaresAndChaffs() 
    {
        var feedbck = GameObject.FindObjectOfType<FeedBackRecorderAndPlayer>();
        if (maxChaff > 0)
        {
            for (int i = 0; i < burstChaffSize * 2; i++)
            {
                var obj = Instantiate(chaff, transform);
                obj.SetActive(false);
                obj.name = "Chaff" + transform.parent.name;
                ChaffsPool.Add(obj);
                feedbck.ExtraEntity.Add(obj.transform);
                //FeedBackRecorderAndPlayer.Allys.Add(obj.transform);
            }
        }
        if (maxFlares > 0)
        {
            for (int i = 0; i < burstFlareSize * 2; i++)
            {
                var obj = Instantiate(flare, transform);
                obj.SetActive(false);
                obj.name = "Flare" + transform.parent.name;
                FlaresPool.Add(obj);
                feedbck.ExtraEntity.Add(obj.transform);
                //FeedBackRecorderAndPlayer.Allys.Add(obj.transform);
            }

        }
    }
    public void SetRangeRWR()
    {
        if(RadarCollider == null)
        {
            RadarCollider = gameObject.AddComponent<SphereCollider>();
            RadarCollider.isTrigger = true;
        }
        
        RadarCollider.radius = radar_Range;
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the for Collider Size
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radar_Range);
    }
    void OnTriggerStay(Collider other)
    {
        if (TargetsList.Contains(other.gameObject.tag))
        {
            if (!objectsDetected.Contains(other.gameObject))
            {
                bool AllowAdd = false;
                if (other.GetComponent<CombineUttam>())
                {
                    if (!other.GetComponentInChildren<AIRadar>().DetectedObjects.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                    AllowAdd = true;
                }
                else if (other.GetComponent<missileNavigation>())
                {
                    if (!other.GetComponent<missileNavigation>().objt.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                    AllowAdd = true;
                }
                else if (other.GetComponentInChildren<UttamRadar>())
                {
                    if (!other.GetComponentInChildren<UttamRadar>().DetectedObjects.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                    AllowAdd = true;
                }
                else if (other.GetComponent<GroundRadar>())
                {
                    if (!other.GetComponent<GroundRadar>().DetectedObjects.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                    AllowAdd = true;
                }
                else
                {
                    return;
                }
                if(AllowAdd)
                {
                    ListUpdated = true;
                    LatestDetected = other.gameObject;
                    objectsDetected.Add(other.gameObject);      //Adding detected object to the list
                }
                
            }
            else
            {
                if (other.GetComponent<CombineUttam>())
                {
                    if (other.GetComponent<CombineUttam>().Radar.DetectedObjects.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                }
                else if (other.GetComponent<missileNavigation>())
                {
                    if (other.GetComponent<missileNavigation>().objt.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                }
                else if (other.GetComponentInChildren<UttamRadar>())
                {
                    if (other.GetComponentInChildren<UttamRadar>().DetectedObjects.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                }
                else if (other.GetComponent<GroundRadar>())
                {
                    if (other.GetComponent<GroundRadar>().DetectedObjects.Contains(transform.parent.gameObject))
                    {
                        return;
                    }
                }
                objectsDetected.Remove(other.gameObject);       //Removing objects out of sight
                QuickUpdate = true;     //To refresh icons on the radar
                ListUpdated = true;
            }
        }
    }
    private void Update()
    {
        CounterMeasures();
        EWRadarLog Log = new EWRadarLog();
        Log.Radar_Range = radar_Range;
        Log.Flares = maxFlares;
        Log.Chaffs = maxChaff;
        Log.chaffBurstSize = burstChaffSize;
        Log.flareBurstSize = burstFlareSize;
        Log.autoFlare = autoFlareFire;
        Log.autoChaff = autoChaffFire;
        Log.autoDircm = DIRCM_Module.active;
        Log.AutoFlareActiveRange = flareActivateRange;
        Log.AutoChaffActiveRange = chaffActivateRange;
        Log.Pt = Pt;
        Log.Gt = Gt;
        Log.f = f;
        Log.Gr = Gr;
        Log.Pr = (float)Pr;
        foreach (var obj in objectsDetected)
        {
            if (obj.tag.ToLower().Contains("missile"))
            {
                Log.MissileWarning = true;
            }
            Log.detect.Add(obj.transform.position);
            String info = "";
            info = obj.tag.Contains("Enemy") ? "E" : "";
            info = obj.tag.Contains("Player") ? "A" : "U";
            info = obj.tag.Contains("Missile") ? "M" : "";
            Log.info.Add(info);
        }
        ewRadarLog = Log;
    }
    void OnTriggerExit(Collider other)
    {
        if (objectsDetected.Contains(other.gameObject))
        {
            objectsDetected.Remove(other.gameObject);       //Removing objects out of sight
            QuickUpdate = true;     //To refresh icons on the radar
            ListUpdated = true;
        }
    }
    private void CounterMeasures()
    {
        temp = 0;
        closestMissile = 1000000;
        closestPosition = null;
        foreach (var hit in objectsDetected)
        {
            if (hit == null)
                continue;
            if (hit.tag.Contains("Missile") && Vector3.Angle(transform.position - hit.transform.position, hit.transform.forward) < 30)
                // chaff activation script
                if (autoChaffFire && Vector3.Distance(transform.position, hit.transform.position) < chaffActivateRange && hit.GetComponent<missileNavigation>().NavigationType == missileNavigation.mode.ActiveRadar)    //chaffs
                {
                    if(controller!=null && controller.start == false)
                    {
                        controller.maneuvers =  controller.GetRandomEnumValue();
                        string By = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(transform.root.gameObject);
                        FeedBackRecorderAndPlayer.AddEvent(By + " Perform " + controller.maneuvers.ToString());
                        controller.start = true;
                    }
                    //if ((Vector3.Distance(hit.transform.position + (hit.transform.forward * Vector3.Distance(hit.transform.position, transform.position)), transform.position) < 25))
                    autoChaffFire = false;
                    Invoke(nameof(ChaffOn), 5f);
                    SpawnChaff();
                }
                //flare activation script
                else if (autoFlareFire && Vector3.Distance(transform.position, hit.transform.position) < flareActivateRange && hit.GetComponent<missileNavigation>().NavigationType == missileNavigation.mode.Infrared) //&& Vector3.Distance(transform.parent.forward, hit.transform.forward) < 1.1f)
                {
                    if (controller != null && controller.start == false)
                    {
                        controller.maneuvers = controller.GetRandomEnumValue();
                        string By = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(transform.root.gameObject);
                        FeedBackRecorderAndPlayer.AddEvent(By + " Perform " + controller.maneuvers.ToString());
                        controller.start = true;
                    }
                    autoFlareFire = false;
                    Invoke(nameof(FlareOn), 5f);
                    SpawnFlare();
                }
                //DIRCM
                if (closestMissile > Vector3.Distance(transform.position, hit.transform.position))
                {
                    closestMissile = Vector3.Distance(transform.position, hit.transform.position);
                    closestPosition = hit;
                }
        }
        if (closestPosition != null) //no target
        {
            if(closestPosition.GetComponent<missileNavigation>()!=null&&closestPosition.GetComponent<missileNavigation>().NavigationType == missileNavigation.mode.Infrared)
            DIRCM_Module.TargetPosition = closestPosition;
        }
    }
    IEnumerator ClearList()
    {        //Refresh the list
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < objectsDetected.Count; i++)
        {
            if (objectsDetected[i] == null || !objectsDetected[i].activeSelf)
            {
                objectsDetected.Remove(objectsDetected[i]);
                QuickUpdate = true;
                ListUpdated = true;
            }
        }
        StartCoroutine("ClearList");
    }
    public void SpawnFlare()
    {
        StartCoroutine(DelayedFlaresShoot());
        flareActivated();
        if(data)
        data.SetMessage("Spawn Flares");
    }

    public void flareBurstSize(int i)
    {
        int[] n = {1,4,8,15,24,512};

        burstFlareSize = n[i];
    }

    public void chaffBurstSize(int i)
    {
        int[] n = { 1, 4, 8, 15, 24, 512 };

        burstChaffSize = n[i];
    }

    IEnumerator DelayedFlaresShoot()
    {
        if (maxFlares > 0)
        {
            string By = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(transform.root.gameObject);
            FeedBackRecorderAndPlayer.AddEvent(By + " Released Flares");
            int i = 0; int j = 0;
            if (!FlaresPool[0].activeSelf) { i = 0; } 
            else if (!FlaresPool[burstFlareSize].activeSelf) { i = burstFlareSize; }
            else {
                for (int k = 0; k < burstFlareSize; k++)
                {
                    maxFlares--;
                    if (maxFlares < 0) { break; }
                    var _flare = Instantiate(flare,transform);
                    _flare.name = "Flare" + transform.parent.name;
                    _flare.transform.SetPositionAndRotation(FlarePos[k % FlarePos.Count].position, transform.rotation);
                    if(_flare.gameObject.GetComponent<FlareActive>()==null)
                        _flare.gameObject.AddComponent<FlareActive>();
                    _flare.transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.1f);
                    if (++j % 4 == 0) { yield return new WaitForSeconds(0.2f); }
                }
            }
            for (int k = 0; k < burstFlareSize;k++,i++)
            {
                maxFlares--;
                if(maxFlares < 0) { break; }
                FlaresPool[i].SetActive(true);
                FlaresPool[i].transform.SetPositionAndRotation(FlarePos[i%FlarePos.Count].position, transform.rotation);
                if (FlaresPool[i].gameObject.GetComponent<FlareActive>() == null)
                    FlaresPool[i].gameObject.AddComponent<FlareActive>(); 
                FlaresPool[i].transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.1f);
                if (++j % 4 == 0){yield return new WaitForSeconds(0.2f);}
            }
        }
    }

    public void SpawnChaff()
    {
        StartCoroutine(DelayedChaffShoot());
        chaffActivated();
        if(data!=null)
        data.SetMessage("Spawned Chaffs");
    }

    IEnumerator DelayedChaffShoot()
    {
        if (maxChaff > 0)
        {
            string By = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(transform.root.gameObject);
            FeedBackRecorderAndPlayer.AddEvent(By + " Released Chaffs");
            int i = 0; int j = 0;
            if (!ChaffsPool[0].activeSelf) { i = 0; }
            else if (!ChaffsPool[burstFlareSize].activeSelf) { i = burstChaffSize; }
            else
            {
                for (int k = 0; k < burstChaffSize; k++)
                {
                    maxChaff--;
                    if (maxChaff < 0) { break; }
                    var _chaff = Instantiate(chaff, transform);
                    _chaff.name = "Chaff" + transform.parent.name;
                    _chaff.transform.SetPositionAndRotation(FlarePos[k % FlarePos.Count].position, transform.rotation);
                    if (_chaff.gameObject.GetComponent<FlareActive>() == null)
                        _chaff.gameObject.AddComponent<FlareActive>();
                    ChaffsPool[i].gameObject.GetComponent<Rigidbody>().velocity = transform.root.GetComponent<Rigidbody>().velocity.magnitude * ChaffsPool[i].transform.forward;
                    _chaff.transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.1f);
                    if (++j % 4 == 0) { yield return new WaitForSeconds(0.2f); }
                    
                }
            }
            for (int k = 0; k < burstChaffSize; k++, i++)
            {
                maxChaff--;
                if (maxChaff < 0) { break; }
                ChaffsPool[i].SetActive(true);
                ChaffsPool[i].transform.SetPositionAndRotation(FlarePos[i % FlarePos.Count].position, transform.rotation);
                if (ChaffsPool[i].gameObject.GetComponent<FlareActive>() == null)
                    ChaffsPool[i].gameObject.AddComponent<FlareActive>();
                ChaffsPool[i].gameObject.GetComponent<Rigidbody>().velocity = transform.root.GetComponent<Rigidbody>().velocity.magnitude* ChaffsPool[i].transform.forward;
                ChaffsPool[i].transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.1f);
                if (++j % 4 == 0) { yield return new WaitForSeconds(0.2f); }
                //Debug.Log("work");
            }
        }
    }

    void FlareOn()
    {
        autoFlareFire = true;
    }
    void ChaffOn()
    {
        autoChaffFire = true;
    }

    IEnumerator JammerSwitch()
    {
        yield return new WaitForSeconds(5);
        bool noEffect = false;
        foreach (GameObject obj in objectsDetected)
        {
            if (!obj.tag.Contains("Missile")) noEffect = true;
        }
        if (noEffect)
        {
            jammer.jamFrequency = (Jammer.JamFrequency)(((int)jammer.jamFrequency + 1) % 5);
        }
    }
    #region EW_Radar_Test
    public void hittedMissile()
    {
        missileText.text = "Missile Hit";
        Invoke(nameof(missileTextDisappear),3f);
    }
    void missileTextDisappear()
    {
        missileText.text = "";
    }
    void flareActivated()
    {
        if (!testing) { return; }
        flareText.text = "Flares Activated";
        StopCoroutine(FlareTextDisappear());
        StartCoroutine(FlareTextDisappear());
    }
    IEnumerator ChaffTextDisappear()
    {
        yield return new WaitForSeconds(2f);
        chaffText.text = "";
    }
    void chaffActivated()
    {
        if (!testing) { return; }
        chaffText.text = "Chaffs Activated";
        StopCoroutine(ChaffTextDisappear());
        StartCoroutine(ChaffTextDisappear());
    }
    IEnumerator FlareTextDisappear()
    {
        yield return new WaitForSeconds(2f);
        flareText.text = "";
    }
    #endregion
}