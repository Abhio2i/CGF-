using Assets.Scripts.Feed;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility.LatLonAlt;
using ColorUtility = UnityEngine.ColorUtility;

[Serializable]
public class RadarLog
{
    public Vector3 Display;
    public RadarScreenUpdate_new.Mode1 MainMode;
    public RadarScreenUpdate_new.Mode2 SubMode;
    public RadarScreenUpdate_new.ACMModes ACMModes;
    public RadarScreenUpdate_new.Bandwidth bandwidth;
    public float gain;
    public float Azimuth;
    public float RangeScale;
    public float Bar;
    public bool TWS;
    public bool ACM;
    public bool CRM;
    public bool PtOnly;
    public bool AAST;
    public bool TxSil;
    public bool PassiveMode;
    public bool ABDutyCycle;
    public bool HILo;
    public bool BScope;
    public bool SAR;
    public bool ISAR;
    public bool RBM;
    public bool GACM;
    public bool Cit;
    public bool WA;
    public bool Jem;
    public float contrast;

    public int TRM;
    public float TRMPOWER;
    public float G;
    public float r;
    public float f;
    public float T;
    public float N;
    public float NR;
    public float Range;
    public int iff;
    public float Elevation;
    public float MinimumDetectSize;
    public float VelocityThreshold;
    public float pulseTime;
    public float peactimeWartimeAccuracy;
    public float frequency;
    public float RMax;
    public float RMin2;
    public float RMax2;



    public bool Aj;
    public bool Lpi;
    public bool jemNoise;
    public float jemAngle;
    //Distance Altitude Speed Bearing
    public List<Vector4> Info=new List<Vector4>();
    public List<Vector3> Pos = new List<Vector3>();
    public List<Vector3> detect = new List<Vector3>();
    public List<Vector3> Data = new List<Vector3>();
}

[Serializable]
public class Sprits
{
    [SerializeField]
    public GameObject sprite;
    [SerializeField]
    public GameObject ally;
    [SerializeField]
    public GameObject enemy;
     [SerializeField]
    public GameObject dir;
     [SerializeField]
    public GameObject Select;
     [SerializeField]
    public GameObject HighThreat;
     [SerializeField]
    public GameObject Lock;
     [SerializeField]
    public GameObject HardLock;
     [SerializeField]
    public TextMeshProUGUI Distance;
     [SerializeField]
    public TextMeshProUGUI Altitude;
     [SerializeField]
    public TextMeshProUGUI Speed;
     [SerializeField]
    public TextMeshProUGUI Heading;
     [SerializeField]
    public TextMeshProUGUI name;
    [SerializeField]
    public TextMeshProUGUI Id;

}


[Serializable]
public class WaypointSprite
{
    [SerializeField]
    public Transform point;
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public RectTransform direction;
}

public class RadarScreenUpdate_new : MonoBehaviour
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

    public enum Bandwidth
    {
        BW1,
        BW2,
        BW3,
        BW4,
        BW5
    }

    public enum ACMModes
    {
        VS,
        BS,
        SA,
        FS,
        HUD,
        HMDS
    }

    //INPUT Parameters
    public Mode1 MainMode = Mode1.ATA;
    public Mode2 SubMode = Mode2.TWS;
    public Bandwidth band = Bandwidth.BW1;
    public ACMModes ACMMode = ACMModes.VS;
    public float gain = 3f;
    public float Azimuth = 55f;
    public float RangeScale = 20f;
    //--------------
    private float a = 55f;
    private float b = 55f;
    public Transform ScanBar;
    public Transform ScanAzimuth;
    public Transform LeftLine;
    public Transform RightLine;
    public Transform NoiseLine;
    public bool scanReverseAzimuth = false;
    public bool scanReverseBar = false;
    public bool PTOnly = false;
    public bool AAST = false;
    public bool TxSil = false;
    public bool PassiveMode = false;
    public bool ABDutyCycle = false;
    public bool HILo = true;
    public bool BScope = false;
    public bool SAR = false;
    public bool ISAR = false;
    public bool RBM = false;
    public bool Cit = false;
    public bool WA = false;
    public bool jem = false;
    public float contrast = 1.1f;
    [Header("HUD DLZ UI")]
    public Transform TargetDLZLocation;
    public Transform Rmin2;
    public Transform Rmax2;
    public Transform effective;

    

    [Header("RadarPanel DLZ UI")]
    public RectTransform RP_Range;
    public Transform RP_Rmin2;
    public Transform RP_Rmax2;
    public Transform RP_Rmin;


    public TextMeshProUGUI HudMainModeText;
    public TextMeshProUGUI HudSubModeText;
    public TextMeshProUGUI LockRange;
    public TextMeshProUGUI Mod1Text;
    public TextMeshProUGUI TWSText;
    public TextMeshProUGUI AASTText;
    public TextMeshProUGUI CRMText;
    public TextMeshProUGUI ACMText;
    public TextMeshProUGUI RBMText;
    public TextMeshProUGUI HRMText;
    public TextMeshProUGUI RAText;
    public List<TextMeshProUGUI> DistUnit;
    public TextMeshProUGUI AzimuthText;
    public TextMeshProUGUI BarText;
    public TextMeshProUGUI ElevationText;
    public RectTransform BandwidthNobe;
    public InputField BandwidthInputField;
    public RectTransform ACMModeNobe;
    public RectTransform GainNobe;
    public Toggle TxSilToggle;
    public Toggle SARToggle;
    public Toggle RBMToggle;
    public Toggle ISARToggle;
    public Toggle HRMToggle;
    public GameObject MapView;
    public RectTransform Cursor;
    public GameObject BombDropLoc;
    public TextMeshProUGUI DropDistance;
    public Vector3 BombDropPosition;
    public GameObject LandRejectToggle;
    public GameObject PPIPanel;
    public GameObject BScopePanel;
    public GameObject BombDrop;
    public GameObject topographicMap;
    public GameObject acmVSModePanel;
    public GameObject acmBSModePanel;
    public GameObject acmHUDModePanel;
    public GameObject acmHMDSModePanel;
    public GameObject acmSAModePanel;
    public Transform HMDSObject;
    public UttamRadar uttamRadar;
    public PlaneData planeData;
    public ApplyStream rbmStream;
    public Transform DummyTarget;
    public TextMeshProUGUI Distance;
    public TextMeshProUGUI VisibleDistance;
    public GameObject WeatherMap;
    public GameObject weatherRadar;

    [Header("Range Signature")]
    public TextMeshProUGUI RSscale;
    public TextMeshProUGUI RSheading;
    public TextMeshProUGUI RSError;
    public TextMeshProUGUI RSLength;
    public TextMeshProUGUI RSpostion;

    [Header("CUED Search Init")]
    public bool cued = false;
    public TMP_InputField MinAlt;
    public TMP_InputField MaxAlt;

    public TMP_InputField MinSpeed;
    public TMP_InputField MaxSpeed;

    public TMP_InputField MinRange;
    public TMP_InputField MaxRange;

    [Header("Slewable")]
    [Range(-1,1)]
    public float xsle = 0;
    [Range(-1, 1)]
    public float ysle = 0;
    public int sleInpX = 0;
    public int sleInpY = 0;
    public float SleSensitivity = 0.1f;
    public Transform SlewHudObject;
    public Transform SlewSmallHudObject;


    public Vector3 ForgroundOffset = new Vector3(0, -116.5f, 0);
    public float ForgroundOffset2 = 30f;
    public GameObject Forground;
    public GameObject Forground2;
    public GameObject EnemyPrefab;
    public GameObject WaypointPrefab;
    public RectTransform Display;
    public Transform Plane;
    public List<Sprits> EnemyPrefabs;
    public List<WaypointSprite> Waypoints;
    private List<Sprits> EnemyPrefabs2;
    public List<RectTransform> RSBar;
    private Rigidbody rb;
    public bool tws, hpt, acm, rbm, hrm, agr, aj,crm,lpi;
    private int zoom=0, xzPos=0, yzPos = 0;
    private float hRange;
    private Vector3 forsave;
    public string Log;
    public RadarLog radarlog = new RadarLog();
    PlayerControle PlayerInput;
    public MissionPlan missionPlan;

    private void OnEnable()
    {
        PlayerInput.Enable();
    }

    private void OnDisable()
    {
        PlayerInput.Disable();
    }

    public void Awake()
    {
        PlayerInput = new PlayerControle();
    }

    void Start()
    {

        PlayerInput.Radar.RadarON.performed += ctx => { TxSilSet(true); TxSilToggle.isOn = true; };
        PlayerInput.Radar.RadarOFF.performed += ctx => { TxSilSet(false); TxSilToggle.isOn = false; };

        PlayerInput.Radar.ACMMODEON.performed += ctx => { if(!acm)submode("acm"); };
        PlayerInput.Radar.ACMMODEOFF.performed += ctx => { if (acm) submode("acm"); };
        PlayerInput.Radar.RangeInc.performed += ctx => { RangeScaleSet(0); };
        PlayerInput.Radar.RangeDec.performed += ctx => { RangeScaleSet(1); };

        PlayerInput.Radar.AAST.performed += ctx => { AastSet(0); };
        PlayerInput.Radar.NAST.performed += ctx => { AastSet(1); };

        PlayerInput.Radar.ACMMODEUP.performed += ctx => { ACMModeSet(1); };
        PlayerInput.Radar.ACMMODEDOWN.performed += ctx => { ACMModeSet(0); };

        PlayerInput.Radar.SlewUP.performed += ctx => { sleInpY = 1; }; 
        PlayerInput.Radar.SlewUP.canceled += ctx => { sleInpY = 0; };

        PlayerInput.Radar.SlewDOWN.performed += ctx => { sleInpY = -1; };
        PlayerInput.Radar.SlewDOWN.canceled += ctx => { sleInpY = 0; };

        PlayerInput.Radar.SlewRIGHT.performed += ctx => { sleInpX = 1; };
        PlayerInput.Radar.SlewRIGHT.canceled += ctx => { sleInpX = 0; };

        PlayerInput.Radar.SlewLEFT.performed += ctx => { sleInpX = -1; };
        PlayerInput.Radar.SlewLEFT.canceled += ctx => { sleInpX = 0; };
         
        PlayerInput.Radar.SlewCenter.performed += ctx => { xsle = 0; ysle = 0; }; 

        tws = hpt = acm = rbm = hrm = aj = lpi = false;
        rb = Plane.root.GetComponent<Rigidbody>();
        //Make Ready For Update Screen
        DummyTarget = uttamRadar.transform.GetChild(3).transform;
        forsave = Forground.transform.localPosition;


        Waypoints = new List<WaypointSprite>();

        foreach (Cordinates cordinate in missionPlan.waypoints)
        { 
            WaypointSprite sprite = new WaypointSprite();
            Vector3 cord = cordinate.position;
            GameObject waypoint = Instantiate(WaypointPrefab);
            waypoint.transform.SetParent(Forground.transform);
            waypoint.transform.localScale = Vector3.one;
            waypoint.SetActive(true);
            waypoint.transform.localPosition = Vector3.zero;
            sprite.point = waypoint.transform;
            sprite.position = cord;
            sprite.direction = waypoint.transform.GetChild(0).GetComponent<RectTransform>();
            Waypoints.Add(sprite);
        }

        if (Waypoints.Count > 2)
        {
            WaypointSprite sprite = Waypoints[1];
            Waypoints.RemoveAt(1);
            Waypoints.Add(sprite);
        }

        EnemyPrefabs = new List<Sprits>();
        EnemyPrefabs2 = new List<Sprits>();
        

        for (var i = 0; i < 50; i++)
        {
            var obj = Instantiate(EnemyPrefab, Forground.transform);
            obj.transform.SetParent(Forground.transform);
            obj.SetActive(false);
            Transform t = obj.transform;
            Sprits sprits = new Sprits();
            sprits.sprite = obj;
            sprits.ally = t.GetChild(0).gameObject;
            sprits.enemy = t.GetChild(1).gameObject;
            sprits.dir = t.GetChild(2).gameObject;
            sprits.Select = t.GetChild(3).gameObject;
            sprits.HighThreat = t.GetChild(4).gameObject;
            sprits.Lock = t.GetChild(5).gameObject;
            sprits.HardLock = t.GetChild(6).gameObject;
            sprits.Distance = t.GetChild(7).GetComponent<TextMeshProUGUI>();
            sprits.Altitude = t.GetChild(8).GetComponent<TextMeshProUGUI>();
            sprits.Speed = t.GetChild(9).GetComponent<TextMeshProUGUI>();
            sprits.Heading = t.GetChild(10).GetComponent<TextMeshProUGUI>();
            sprits.name = t.GetChild(11).GetComponent<TextMeshProUGUI>();

            EnemyPrefabs.Add(sprits);

            // Create a new entry for the OnPointerClick event
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;

            // Add a callback function to the event
            entry.callback.AddListener((data) => { manualSelectTarget((PointerEventData)data); });

            // Add the entry to the EventTrigger
            obj.GetComponent<EventTrigger>().triggers.Add(entry);



            obj = Instantiate(EnemyPrefab, Forground2.transform);
            obj.transform.SetParent(Forground2.transform);
            obj.SetActive(false);
            t = obj.transform;
            sprits = new Sprits();
            sprits.sprite = obj;
            sprits.ally = t.GetChild(0).gameObject;
            sprits.enemy = t.GetChild(1).gameObject;
            sprits.dir = t.GetChild(2).gameObject;
            sprits.Select = t.GetChild(3).gameObject;
            sprits.HighThreat = t.GetChild(4).gameObject;
            sprits.Lock = t.GetChild(5).gameObject;
            sprits.HardLock = t.GetChild(6).gameObject;
            sprits.Distance = t.GetChild(7).GetComponent<TextMeshProUGUI>();
            sprits.Altitude = t.GetChild(8).GetComponent<TextMeshProUGUI>();
            sprits.Speed = t.GetChild(9).GetComponent<TextMeshProUGUI>();
            sprits.Heading = t.GetChild(10).GetComponent<TextMeshProUGUI>();
            sprits.name = t.GetChild(11).GetComponent<TextMeshProUGUI>();
            EnemyPrefabs2.Add(sprits);
        }

       
        submode("tws");
    }

    public void manualSelectTarget(int i)
    {
        if (i > 0 && uttamRadar.HardLockTargetObject!=null)
        {
            uttamRadar.SelectedTarget = uttamRadar.HardLockTargetObject;
            if(uttamRadar.SelectedTarget.TryGetComponent<Specification>(out Specification spec))
            {
                uttamRadar.SelectedTargetSpecification = spec;
            }
        }
    }

    // Callback function for the OnPointerClick event
    public void manualSelectTarget(PointerEventData eventData)
    {
        int i = 0;

        foreach (Sprits p in EnemyPrefabs)
        {
            if(p.sprite == eventData.pointerClick)
            {
                break;
            }
            i++;
        }

        if (i>=0)
        {
            GameObject obj = uttamRadar.DetectedObjects[i];
            if(obj != null)
            {
                Debug.Log("Pointer Clicked on: " + eventData.pointerClick.name);
                Debug.Log("object" + obj.name);
                uttamRadar.SelectedTarget = obj;
                if (uttamRadar.SelectedTarget.TryGetComponent<Specification>(out Specification spec))
                {
                    uttamRadar.SelectedTargetSpecification = spec;
                }
            }

        }
       
        // Add your code for pointer click actions here
    }

    public void cursorPosition(GameObject obj)
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        // Raycast into the canvas
        GraphicRaycaster gr = obj. GetComponent<GraphicRaycaster>();
        //Debug.Log(gr);
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerData, results);

        if (results.Count > 0)
        {
            // Get the local position of the click on the UI element
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(results[0].gameObject.GetComponent<RectTransform>(), pointerData.position, null, out localPos);
            Cursor.localPosition = localPos;
            uttamRadar.xPos = -localPos.y/(Display.sizeDelta.x/2f);
            uttamRadar.yPos = localPos.x / (Display.sizeDelta.x / 2f);
            //Debug.Log("Clicked position in UI element's local space: " + localPos);
        }
    }
    



    private void Update()
    {
        //scan Azimuth Animation
        if (ScanAzimuth != null)
        {
            a = uttamRadar.Azimuth;
            var ut = ScanAzimuth.localEulerAngles;
            //Debug.Log(ut.z);
            if (ut.z >= uttamRadar.Azimuth - 1&& ut.z<180f)
            {
                scanReverseAzimuth = true;
            }
            if (ut.z <= (360- uttamRadar.Azimuth + 1)&&ut.z>180f)
            {
                scanReverseAzimuth = false;
            }
            a = scanReverseAzimuth ? a * -1f : a;
            ScanAzimuth.rotation = Quaternion.RotateTowards(ScanAzimuth.rotation, Quaternion.Euler(0f, 0f, a), 100f * Time.fixedDeltaTime);

        }

        //scan bar Animation
        if (ScanBar != null)
        {
            b = uttamRadar.Bar*2.5f;
            var ut = ScanBar.localPosition;
            //Debug.Log(ut.z);
            if (ut.y >= (uttamRadar.Bar*2.5f) - 1 && ut.y > 0f)
            {
                scanReverseBar = true;
            }
            if (ut.y <= -(uttamRadar.Bar * 2.5f) + 1&& ut.y < 0f)
            {
                scanReverseBar = false;
            }
            b = scanReverseBar ? b * -1f : b;
            var d = scanReverseBar ? -1f : 1f;
            //ScanBar.localPosition = Vector3.LerpUnclamped(ScanBar.localPosition,new Vector3(-30f,b,0),2.5f*Time.fixedDeltaTime);
            ScanBar.localPosition += new Vector3(0, d*50f, 0 )* Time.fixedDeltaTime;
        }
        var v = 0;
        var part = (RangeScale / 4) + (RangeScale / 22);
        uttamRadar.VisualRange = RangeScale;

        foreach (var obj in DistUnit)
        {
            v++;
            obj.text = (v * part).ToString("0") + "";
        }
        //Forground.transform.localPosition = MainMode != Mode1.ATA ? forsave : Vector3.zero;
        Distance.text = uttamRadar.Range.ToString() + "";
        VisibleDistance.text = RangeScale.ToString();
        AzimuthText.text = (uttamRadar.Azimuth / 10).ToString("0") + "\r\nA";
        BarText.text = (uttamRadar.Bar / 10).ToString("0") + "\r\nB";
        ElevationText.text = (uttamRadar.Elevation).ToString("0")+ "\r\nE";
        RightLine.rotation = Quaternion.Euler(0f, 0f, -uttamRadar.Azimuth);
        LeftLine.rotation = Quaternion.Euler(0f, 0f, uttamRadar.Azimuth);
        //RAText.text = (uttamRadar.Altitude*3.281f).ToString("0") + "ft";
        RAText.text = planeData.specification.entityInfo.Altitude.ToString("0") + "ft";
        AASTText.text = AAST ? "AAST" : "NAST";
        HudMainModeText.text = uttamRadar.MainMode.ToString();
        if (acm)
        {
            HudSubModeText.text = ACMMode.ToString();
        }
        else
        {
            HudSubModeText.text = uttamRadar.SubMode.ToString();
        }
        //DLZ CODE
        if (LockRange)
        {
            hRange = (uttamRadar.MissileMaxRange+0)/ 1.852f;
            hRange = (!float.IsNaN(hRange)&& hRange!=Mathf.Infinity) ? hRange : (uttamRadar.DefaultHighThreatRange / 1.852f);
            LockRange.text = hRange.ToString("0");

            Rmin2.localPosition = new Vector3(9.999889f, ((uttamRadar.rmin2 / 100) * 200) - 100, 0);
            Rmax2.localPosition = new Vector3(9.999889f, ((uttamRadar.rmax2 / 100) * 200) - 100, 0);
            effective.localPosition = new Vector3(20.07086f, ((uttamRadar.rmin2 / 100) * 200) - 100, 0);
            effective.localScale = new Vector3(1, (((uttamRadar.rmax2 - uttamRadar.rmin2) / 100) * 200), 1);
             
            if (uttamRadar.HardLockTargetObject!=null&& uttamRadar.missileSelect)
            {
                
                float d = Vector3.Distance(uttamRadar.lockTargetProcess.transform.position, uttamRadar.transform.root.position) / uttamRadar.mile;
                
                TargetDLZLocation.localPosition = new Vector3(-19.2f, ((d / hRange) * 200) - 100, 0);
                TargetDLZLocation.gameObject.SetActive(true);
                RP_Range.gameObject.SetActive(true);
                RP_Range.sizeDelta = new Vector2(1,(hRange/RangeScale)*Display.rect.height);
                RP_Rmax2.localPosition = new Vector3(0, (uttamRadar.rmax2/100)*RP_Range.sizeDelta.y, 0);
                RP_Rmin2.localPosition = new Vector3(0, (uttamRadar.rmin2 / 100) * RP_Range.sizeDelta.y, 0);
                TargetDLZLocation.parent.gameObject.SetActive(true);
            }
            else
            {
                TargetDLZLocation.gameObject.SetActive(false);
                TargetDLZLocation.parent.gameObject.SetActive(false);
                RP_Range.gameObject.SetActive(false);
            }
        }
        //bomb drop predict code
        float speed = 0;
        var vl = Plane.root.GetComponent<MoveDummy>();
        if (!vl)
        {
            speed = Vector3.Dot(rb.velocity,Plane.root.forward);
        }
        else
        {
            speed = vl.speed;
        }
        var t = Mathf.Sqrt(2 * Plane.root.position.y / 9.81f);
        var xdis = speed * t;
        var pos = Plane.root.position;
        pos += Plane.root.forward * xdis;
        pos.y = 72.0f;
        BombDropPosition = pos;
        //BombDrop.transform.position = pos;
        pos = uttamRadar.MapCamera.WorldToViewportPoint(pos);
        var x = (Display.rect.width * pos.x) - (Display.rect.width / 2);
        var y = (Display.rect.width * pos.y) - (Display.rect.width / 2);
        var z = Mathf.Clamp(pos.z, -1f, 1f);
        if(uttamRadar.HardLockTargetObject != null)
        {

            DropDistance.text = Vector3.Distance(BombDropPosition, uttamRadar.HardLockTargetObject.transform.position).ToString("0");
            BombDropLoc.transform.localPosition = new Vector3(x, y, z);
            BombDropLoc.SetActive(true);
        }else
        {
            BombDropLoc.SetActive(false);
        }

        //Radar Map Zoom
        if (zoom < 0)
        {
            if (uttamRadar.zoom > 1)
            {
                uttamRadar.zoom = uttamRadar.zoom - (1f * Time.deltaTime);
            }
            uttamRadar.zoom = uttamRadar.zoom < 1 ? 1:uttamRadar.zoom;
        }
        else
        if (zoom > 0)
        {
            if (uttamRadar.zoom < 10)
            {
                uttamRadar.zoom = uttamRadar.zoom + (1f * Time.deltaTime);
            }
            uttamRadar.zoom = uttamRadar.zoom > 10 ? 10 : uttamRadar.zoom;
        }

        if (xzPos < 0)
        {
            if (uttamRadar.xPos > -1)
            {
                uttamRadar.xPos = uttamRadar.xPos - ((1f - (uttamRadar.zoom / 11f)) * Time.deltaTime);
            }
            uttamRadar.xPos = uttamRadar.xPos < -1 ? -1 : uttamRadar.xPos;
        }
        else
        if (xzPos > 0)
        {
            if (uttamRadar.xPos < 1)
            {
                uttamRadar.xPos = uttamRadar.xPos + ((1f - (uttamRadar.zoom / 11f)) * Time.deltaTime);
            }
            uttamRadar.xPos = uttamRadar.xPos > 1 ? 1 : uttamRadar.xPos;
        }

        if (yzPos < 0)
        {
            if (uttamRadar.yPos > -1)
            {
                uttamRadar.yPos = uttamRadar.yPos - ((1f - (uttamRadar.zoom / 11f)) * Time.deltaTime);
            }
            uttamRadar.yPos = uttamRadar.yPos < -1 ? -1 : uttamRadar.yPos;
        }
        else
        if (yzPos > 0)
        {
            if (uttamRadar.yPos < 1)
            {
                uttamRadar.yPos = uttamRadar.yPos + ((1f - (uttamRadar.zoom / 11f)) * Time.deltaTime);
            }
            uttamRadar.yPos = uttamRadar.yPos > 1 ? 1 : uttamRadar.yPos;
        }

        if(SubMode == Mode2.ACM && ACMMode == ACMModes.HMDS)
        {
            float xrot = HMDSObject.localEulerAngles.x;
            xrot = (xrot > 0 && xrot < 180) ? -xrot : (360 - xrot);
            xrot = xrot == 360 ? 0 : xrot;
            xrot = xrot>55?55:(xrot<-55?xrot:xrot);
            float yrot = HMDSObject.localEulerAngles.y;
            yrot = (yrot > 0 && yrot < 180) ? -yrot : (360 - yrot);
            yrot = yrot == 360 ? 0 : yrot;
            yrot = yrot > 60 ? 60 : (yrot < -60 ? yrot : yrot);
            uttamRadar.Elevation = xrot;
            uttamRadar.AzimuthElev = yrot;

        }else
         if (SubMode == Mode2.ACM && ACMMode == ACMModes.SA)
        {
            xsle += sleInpX * SleSensitivity * Time.fixedDeltaTime;
            xsle = (xsle < 0 && xsle < -1) ? -1 : ((xsle > 0 && xsle > 1)?1:xsle);
            ysle += sleInpY * SleSensitivity * Time.fixedDeltaTime;
            ysle = (ysle < 0 && ysle < -1) ? -1 : ((ysle > 0 && ysle > 1) ? 1 : ysle);

            SlewHudObject.localPosition = new Vector3(xsle * 124f, ysle * 118f, 0);
            SlewSmallHudObject.localPosition = new Vector3(xsle*22f, ysle*10f, 0);
            uttamRadar.Elevation = -ysle * 22f;
            uttamRadar.AzimuthElev = xsle * 25f;
        }

        //PassiveMode
        if (PassiveMode && (MainMode != Mode1.ATA || SubMode != Mode2.TWS))
            MainModeSelection();

        RadarLog log = new RadarLog();
        ScreenUpdate(log);
        waypointsUpdate();
        if (planeData != null)
        {
            //string n = "/:/UIMOdE";
            //n+= "MainMode:" + uttamRadar.MainMode.ToString()+"||";
            //n+="SubMode:" + uttamRadar.SubMode.ToString()+"||";
            //n += "Azimuth:" + uttamRadar.Azimuth.ToString() + "||";
            //n += "Bar:" + uttamRadar.Bar.ToString() + "||";
            //n += "RangeScale:" +RangeScale.ToString() + "||";
            //n += "Cit:" + uttamRadar.Cit.ToString() + "||";
            //n += "JEM:" + uttamRadar.JEM.ToString() + "||";
            //n += "PTonly:" + PTOnly.ToString() + "||";
            //n += "TxSil:" + uttamRadar.TxSil.ToString() + "||";
            //n += "WA:" + WA.ToString();
            log.MainMode = (Mode1)uttamRadar.MainMode;
            log.SubMode = (Mode2)uttamRadar.SubMode;
            log.ACMModes = (ACMModes)ACMMode;
            log.bandwidth = (Bandwidth)band;
            log.gain = gain;
            log.Azimuth = uttamRadar.Azimuth;
            log.Bar = uttamRadar.Bar;
            log.TWS = tws;
            log.ACM = acm;
            log.CRM = crm;
            log.PtOnly = PTOnly;
            log.AAST = AAST;
            log.TxSil = uttamRadar.TxSil;
            log.PassiveMode = uttamRadar.PassiveMode;
            log.ABDutyCycle = uttamRadar.ABDutyCycle;
            log.HILo = HILo;
            log.BScope = BScope;
            log.SAR = uttamRadar.SAR;
            log.ISAR = ISAR;
            log.RBM = RBM;
            log.GACM = uttamRadar.GACM;
            log.Cit = uttamRadar.Cit;
            log.WA = WA;
            log.Jem = uttamRadar.JEM;
            log.contrast = contrast;
            log.TRM = uttamRadar.TRM;
            log.TRMPOWER = uttamRadar.TRMPower;
            log.G =uttamRadar.G;
            log.r =uttamRadar.r;
            log.f =uttamRadar.f;
            log.T =uttamRadar.T;
            log.N =uttamRadar.N;
            log.NR =uttamRadar.NR;
            log.Range =uttamRadar.Range;
            log.iff =uttamRadar.iff;
            log.Elevation =uttamRadar.Elevation;
            log.MinimumDetectSize =uttamRadar.MinimumDetectSize;
            log.VelocityThreshold =uttamRadar.VelocityThreshold;
            log.pulseTime =uttamRadar.pulseTime;
            log.peactimeWartimeAccuracy =uttamRadar.peactimeWartimeAccuracy;
            log.frequency =uttamRadar.frequency;
            log.RMax = hRange;
            log.RMax2 = uttamRadar.rmax2;
            log.RMin2 = uttamRadar.rmin2;

            log.Display = Display.transform.localPosition;
            log.RangeScale = RangeScale;    

            log.WA = WA;
            log.Aj = aj;
            log.Lpi = lpi;
            log.jemNoise = NoiseLine.gameObject.activeSelf;
            log.jemAngle = uttamRadar.NoiseAngle;
            radarlog = log;
            Log = JsonUtility.ToJson(log);
            //planeData.SetMessage(JsonUtility.ToJson(log));
        }
        NoiseUpdate();
        RSBarUpdate();
    }

    public void RSBarUpdate()
    {
        if(Mode1.ATS == MainMode)
        {
            if(uttamRadar.SelectedTargetSpecification != null && uttamRadar.SelectedTarget != null)
            {
                uttamRadar.SelectedTargetSpecification.carrierShipController.TargetAngle = uttamRadar.transform.root.eulerAngles.y;
                int s = 0;
                float sum = 0;
                foreach(float i in uttamRadar.SelectedTargetSpecification.carrierShipController.data)
                {
                    RSBar[s].sizeDelta = new Vector2(2, Mathf.Clamp(i*80f,0f, 80f));
                    sum += i * 10f;
                    s++;
                }
                RSscale.text = "T"+(RSBar.Count * 10f).ToString("0");
                RSheading.text = "A"+uttamRadar.SelectedTargetSpecification.entityInfo.Heading.ToString();
                RSLength.text = sum.ToString("0") + "m";
                RSError.text = "AE"+((RSBar.Count * 10f)-sum).ToString("000");
                RSpostion.text = uttamRadar.SelectedTargetSpecification.entityInfo.Lat.ToString("0.00000") + "," + uttamRadar.SelectedTargetSpecification.entityInfo.Long.ToString("0.00000");
            }
            else
            {
                foreach(RectTransform r in RSBar)
                { 
                    r.sizeDelta = new Vector2(2, 2f);
                }
                RSscale.text = "";
                RSError.text = "";
                RSheading.text = "";
                RSLength.text = "";
            }
        }
    } 

   void NoiseUpdate()
    {
        if (uttamRadar.NoiseAngle == 0) { NoiseLine.gameObject.SetActive(false); }
        else { NoiseLine.gameObject.SetActive(true); }
        NoiseLine.rotation = Quaternion.Euler(0, 0, uttamRadar.NoiseAngle);
    }

    public string getLog()
    {
        RadarLog log = new RadarLog();

            //string n = "/:/UIMOdE";
            //n+= "MainMode:" + uttamRadar.MainMode.ToString()+"||";
            //n+="SubMode:" + uttamRadar.SubMode.ToString()+"||";
            //n += "Azimuth:" + uttamRadar.Azimuth.ToString() + "||";
            //n += "Bar:" + uttamRadar.Bar.ToString() + "||";
            //n += "RangeScale:" +RangeScale.ToString() + "||";
            //n += "Cit:" + uttamRadar.Cit.ToString() + "||";
            //n += "JEM:" + uttamRadar.JEM.ToString() + "||";
            //n += "PTonly:" + PTOnly.ToString() + "||";
            //n += "TxSil:" + uttamRadar.TxSil.ToString() + "||";
            //n += "WA:" + WA.ToString();
            log.MainMode = (Mode1)uttamRadar.MainMode;
            log.SubMode = (Mode2)uttamRadar.SubMode;
            log.Azimuth = uttamRadar.Azimuth;
            log.Bar = uttamRadar.Bar;
            log.frequency = uttamRadar.f;
            log.Range = uttamRadar.Range;
            log.Display = Display.transform.localPosition;
            log.RangeScale = RangeScale;
            log.Cit = uttamRadar.Cit;
            log.Jem = uttamRadar.JEM;
            log.PtOnly = PTOnly;
            log.TxSil = uttamRadar.TxSil;
            log.WA = WA;
            log.Aj = aj;
            log.Lpi = lpi;
            log.jemNoise = NoiseLine.gameObject.activeSelf;
            log.jemAngle = uttamRadar.NoiseAngle;
            Log = JsonUtility.ToJson(log);
        return Log;

    }

    public void waypointsUpdate()
    {
        var i = 0;
        Transform last = null;
        foreach (WaypointSprite sprite in Waypoints)
        {
            var ob = sprite.point;
            DummyTarget.position = sprite.position;
            var x = 0f;
            var y = 0f;
            var z = 0f;

            var w = ((RangeScale * uttamRadar.mile) / Mathf.Tan((90 - uttamRadar.Azimuth) * Mathf.Deg2Rad));
            Display.transform.localPosition = ForgroundOffset;
            x = (DummyTarget.localPosition.x / w) * (Display.rect.height / (Mathf.Tan((90 - uttamRadar.Azimuth) * Mathf.Deg2Rad)));
            y = ((DummyTarget.localPosition.z / (RangeScale * uttamRadar.mile)) * (Display.rect.height /* 2f*/)) - ForgroundOffset2;
            z = 0f;


            ob.localPosition = new Vector3(x, y, z);

            if (i > 0)
            {
                if (last != null)
                {

                    Vector3 direction = last.localPosition - ob.localPosition;
                    float angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);

                    RectTransform dir = sprite.direction;
                    dir.localEulerAngles = new Vector3(0, 0, angle);
                    dir.sizeDelta = new Vector2(dir.sizeDelta.x, direction.magnitude);
                }
            }
            last = ob;
            i++;
        }
    }
    //Method for update all radar modes on screen
    public void  ScreenUpdate(RadarLog log)
    {
        if (MainMode == Mode1.ATG)
        {
            if (!uttamRadar.MapCamera.gameObject.activeSelf || !uttamRadar.MapCamera.enabled)
                return;
        }

        //var str = "UIDetect";
        var i = 0;
        for(var ict =0;ict<EnemyPrefabs.Count;ict++)
        {
            var ob = EnemyPrefabs[ict];
            var ob2 = EnemyPrefabs2[ict];
            if (i < uttamRadar.DetectedObjects.Count)
            {
                var detect = uttamRadar.DetectedObjects[i];
                if (detect != null)
                {
                    
                    ob.sprite.SetActive(true);
                    ob2.sprite.SetActive(true);
                    //ob.transform.localRotation = Quaternion.Euler(0, 0, -RadarParentRot.z);
                    var obj = uttamRadar.DetectedObjects[i];

                    DummyTarget.position = obj.transform.position;


                    var x = 0f;
                    var y = 0f;
                    var z = 0f;

                    if (SubMode == Mode2.TWS || SubMode == Mode2.HPT || SubMode == Mode2.ACM)
                    {


                        var w = ((RangeScale * uttamRadar.mile)/Mathf.Tan((90-uttamRadar.Azimuth) * Mathf.Deg2Rad));
                        Display.transform.localPosition = ForgroundOffset;
                        x = (DummyTarget.localPosition.x / w) * (Display.rect.height/(Mathf.Tan((90-uttamRadar.Azimuth)*Mathf.Deg2Rad)));
                        y = ((DummyTarget.localPosition.z / (RangeScale * uttamRadar.mile)) * (Display.rect.height /* 2f*/)) - ForgroundOffset2;
                        z = 0f;
                        //y = y > (Display.rect.height / 2f) ? (Display.rect.height / 2f) : y;
                        if (BScope)
                        {
                            Vector3 targetDir = new Vector3(x, y + 300, 0) - Vector3.zero;
                            float angle = Vector3.Angle(targetDir, transform.up);
                            angle = x < 0 ? -angle : angle;
                            angle = angle / uttamRadar.Azimuth;
                            x = angle * (Display.rect.width / 2f);
                            //Debug.Log(angle);
                        }

                        if (uttamRadar.HardLockTargetObject == obj)
                        {
                            Vector3 targetDir = new Vector3(x, y + 300, 0) - Vector3.zero;
                            float angle = Vector3.Angle(targetDir, transform.up);
                            angle = x < 0 ? angle : -angle;
                            RP_Range.transform.localEulerAngles = new Vector3(0, 0, angle);
                        }
                    }
                    else if (SubMode == Mode2.HRM || SubMode == Mode2.GMTI || SubMode == Mode2.AGR || SubMode == Mode2.RBM)
                    {
                        Display.transform.localPosition = new Vector3(0, 0, 0);
                        /*
                        var AZ = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;
                        var BR = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;

                        Debug.Log(DummyTarget.localPosition.z);
                        Debug.Log(AZ);

                        x = (DummyTarget.localPosition.x / (AZ / 0.92f)) * (Display.rect.width / 2f);
                        y = (DummyTarget.localPosition.y / (BR / 0.92f)) * (Display.rect.height / 2f);
                        z = 0f;
                        */
                        Vector3 pos = uttamRadar.MapCamera.WorldToViewportPoint(obj.transform.position);
                        x = (Display.rect.width * pos.x) - (Display.rect.width / 2);
                        y = (Display.rect.width * pos.y) - (Display.rect.width / 2);
                        z = Mathf.Clamp(pos.z, -1f, 1f);
                        //Debug.Log(pos);
                        //transform.localPosition = new Vector3(400*(pos.x/ Screen.width), 400 * ( pos.y/ Screen.height),pos.z);

                    }
                    else if (SubMode == Mode2.STWS || SubMode == Mode2.STCT)
                    {
                        Display.transform.localPosition = new Vector3(0, 0, 0);
                        /*
                        var AZ = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;
                        var BR = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;

                        //Debug.Log(DummyTarget.localPosition.z);
                        //Debug.Log(AZ);

                        x = (DummyTarget.localPosition.x / (AZ / 0.92f)) * (Display.rect.width / 2f);
                        y = (DummyTarget.localPosition.y / (BR / 0.92f)) * (Display.rect.height / 2f);
                        z = 0f;
                        */
                        Vector3 pos = uttamRadar.MapCamera.WorldToScreenPoint(obj.transform.position);
                        x = (Display.rect.width * (pos.x / uttamRadar.MapImage.width)) - (Display.rect.width / 2);
                        y = (Display.rect.width * (pos.y / uttamRadar.MapImage.height)) - (Display.rect.width / 2);
                        z = Mathf.Clamp(pos.z, -1f, 1f);
                    }
                    else
                    {
                        ob2.sprite.SetActive(false);
                        ob.sprite.SetActive(false);
                    }


                    
                    ob.sprite.transform.localPosition = new Vector3(x, y, z);
                   
                    //str += ";";
                    
                     if (crm||true)
                    {
                        /*
                        //Display.transform.localPosition = new Vector3(0, 0, 0);//it will create issue
                        //var AZ = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;
                        //var BR = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;

                        ////Debug.Log(DummyTarget.localPosition.z);
                        ////Debug.Log(AZ);

                        //x = (DummyTarget.localPosition.x / (AZ / 0.92f)) * (Display.rect.width / 2f);
                        //y = (DummyTarget.localPosition.y / (BR / 0.92f)) * (Display.rect.height / 2f);
                        //z = 0f;
                        */
                        Vector3 pos = uttamRadar.MapCamera.WorldToScreenPoint(obj.transform.position);
                        x = (Display.rect.width * (pos.x / uttamRadar.MapImage.width)) - (Display.rect.width / 2);
                        y = (Display.rect.width * (pos.y / uttamRadar.MapImage.height)) - (Display.rect.width / 2);
                        z = Mathf.Clamp(pos.z, -1f, 1f);
                    }


                    ob2.sprite.transform.localPosition = new Vector3(x, y, z);
                    if (uttamRadar.HardLockTargetObject!=obj && PTOnly/*&&(SubMode==Mode2.HPT||SubMode==Mode2.ACM)*/)
                    {
                        ob.sprite.SetActive(false);
                        ob2.sprite.SetActive(false);
                    }


                    if (ob.sprite.activeSelf)
                    {
                        //Distance Altitude Speed Bearing
                        Vector4 info = Vector4.zero;
                        float speed = 0;
                        if (uttamRadar.FoundedObjectsRB.ContainsKey(obj))
                            speed = Mathf.Round(uttamRadar.FoundedObjectsRB[obj].velocity.magnitude * 1.944f);//knots
                        else
                        if (obj.GetComponent<MoveDummy>())
                            speed = Mathf.Round(obj.GetComponent<MoveDummy>().speed * 1.944f);

                        float altitude = (obj.transform.position.y * 3.281f) / 1000;//feet
                        float bearing = uttamRadar.transform.root.eulerAngles.y - obj.transform.eulerAngles.y;//degree
                        float Distance = (Vector3.Distance(obj.transform.position, uttamRadar.transform.position) / 1852);//Nauticle Mile
                        info.x = Distance;
                        info.y = altitude;
                        info.z = speed;
                        info.w = bearing;
                        Vector3 data = Vector3.zero;
                        data.x = obj.tag.Contains("Player") ? 1 : 0;
                        data.y = obj == uttamRadar.HardLockTargetObject? 1 : 0;
                        data.z = obj == uttamRadar.SelectedTarget? 1 : 0;

                        SetIcon(ob, obj,info);
                        SetIcon(ob2, obj, info);
                        log.Info.Add(info);
                        log.Pos.Add(detect.transform.position);
                        log.Data.Add(data);
                        log.detect.Add(ob.sprite.transform.localPosition);
                    }
                    //log.info.Add(str);

                    DummyTarget.localPosition = Vector3.zero;

                }

            }
            else
            {
                ob.sprite.SetActive(false);
                ob2.sprite.SetActive(false);
                for (var b = 0; b < ob.sprite.transform.childCount; b++)
                {
                    ob.sprite.transform.GetChild(b).gameObject.SetActive(false);
                    ob2.sprite.transform.GetChild(b).gameObject.SetActive(false);
                }
            }
            i++;
        }

       
    }





    //update detected objects icon
    /*square = track object
     * traingle = missile
     * diamond = lock
     * cross = hard lock ready for fire
     * dot = object
     */
    public void SetIcon(Sprits sprits, GameObject enemy , Vector4 info)
    {
        ///Check Enemy Or Player
        if (enemy.transform.tag.ToLower().Contains("player"))
        {
            sprits.ally.SetActive(true);
            sprits.enemy.SetActive(false);
        }
        else
        {
            sprits.ally.SetActive(false);
            sprits.enemy.SetActive(true);
            /*
            //if (/*(enemy.transform.tag.ToLower().Contains("player") || enemy.transform.tag.ToLower().Contains("ally")) &&Cit)
            {
                
                var spec = enemy.GetComponent<Specification>();
                if (spec!=null&&spec.iff == uttamRadar.iff)
                {
                    ping.transform.GetChild(0).gameObject.SetActive(true);
                    ping.transform.GetChild(1).gameObject.SetActive(false);
                }
                

            }
            */
         }

        //Target Bearing
        //AAST / NAST
        var a = uttamRadar.transform.root.eulerAngles.y;
        var b = enemy.transform.eulerAngles.y;
        var Angle = info.w; //uttamRadar.transform.root.eulerAngles.y - enemy.transform.eulerAngles.y;
        if (AAST)
        {
            sprits.dir.SetActive(true);
            sprits.dir.transform.eulerAngles = new Vector3(0, 0, Angle);
        }
        else
        {
            var ang = ((a - b) < 0 ? -(a - b) : (a - b));
            if (ang > 160 && ang < 200)
            {
                sprits.dir.SetActive(true);
                sprits.dir.transform.eulerAngles = new Vector3(0, 0, Angle);
            }
            else
            {
                sprits.sprite.SetActive(false);

            }

        }

        //Traget is Selected or not
        if (uttamRadar.SelectedTarget == enemy)
        {
            sprits.Select.SetActive(true); 
        }
        else
        {
             sprits.Select.SetActive(false);
        }


        //Target is High Threat or not 
        if (uttamRadar.LockTargetList.Contains(enemy))
        {
            sprits.HighThreat.SetActive(true);
        }
        else
        {
            sprits.HighThreat.SetActive(false);
        }

        //Target is lock or not
        if (uttamRadar.tempLockTarget && enemy == uttamRadar.lockTargetProcess)
        {
            sprits.HighThreat.SetActive(false);
            sprits.Lock.SetActive(true);
        }
        else
        {
            sprits.Lock.SetActive(false);
        }

        //Target is HardLock
        if (uttamRadar.TargetLock && enemy == uttamRadar.HardLockTargetObject)
        {
            sprits.Lock.SetActive(false);
            sprits.HardLock.SetActive(true);
        }
        else
        {
            sprits.HardLock.SetActive(false);
        }


        //Taregt Distance
        sprits.Distance.gameObject.SetActive(true);
        sprits.Distance.text = info.x.ToString("0.0");
        //Taregt Altitude
        sprits.Altitude.gameObject.SetActive(true);
        sprits.Altitude.text = info.y.ToString("0.0");
        //Taregt Speed
        sprits.Speed.gameObject.SetActive(true);
        sprits.Speed.text = info.z.ToString("0.0");
        //Taregt Heading
        sprits.Heading.gameObject.SetActive(true);
        sprits.Heading.text = info.w.ToString("0.0");

        //Target Name
        if (enemy == uttamRadar.SelectedTarget ) {
            sprits.name.gameObject.SetActive(true);
            if (enemy.name.ToLower().Contains("tejas"))
            {
                sprits.name.text = uttamRadar.JEM ? "TJ" : "";
            }
            else
            if (enemy.name.ToLower().Contains("f16"))
            {
                sprits.name.text = uttamRadar.JEM ? "F16" : "";
            }
            else
            if (enemy.name.ToLower().Contains("f 18"))
            {
                sprits.name.text = uttamRadar.JEM ? "F18" : "";
            }
            else
            if (enemy.name.ToLower().Contains("sukhoi"))
            {
                sprits.name.text = uttamRadar.JEM ? "S30" : "";
            }
            else
            {
                sprits.name.text = uttamRadar.JEM ? "" : "";
            }
        }
        else
        {
            sprits.name.gameObject.SetActive(false);
        }
    }



    //Method for update mode through UI Button
    public void MainModeSelection()
    {
        acmVSModePanel.SetActive(false);
        acmBSModePanel.SetActive(false);
        MapView.SetActive(false);
        topographicMap.SetActive(false);
        if ((int)MainMode == 2)
        {
            MainMode = Mode1.ATA;
        }
        else
        {
            MainMode = (Mode1)((int)MainMode) + 1;
        }
        Mod1Text.text = MainMode.ToString();



        if (MainMode == Mode1.ATA)
        {
            SubMode = Mode2.TWS;
            uttamRadar.Elevation = 0f;
        }
        else if (MainMode == Mode1.ATG)
        {
            SubMode = Mode2.GMTI;
            RBMToggle.isOn = true;
            MapView.SetActive(true);
            //topographicMap.SetActive(true);
            uttamRadar.Elevation = 60f;
        }
        else if (MainMode == Mode1.ATS)
        {
            SubMode = Mode2.STWS;
            MapView.SetActive(true);
            uttamRadar.Elevation = 60f;
        }

        //Mod2Text.text = SubMode.ToString();
        uttamRadar.MainMode = (UttamRadar.Mode1)((int)MainMode);
        uttamRadar.SubMode = (UttamRadar.Mode2)((int)SubMode);
        uttamRadar.DetectedObjects.Clear();
        uttamRadar.FoundedObjectsRB.Clear();
        uttamRadar.FoundedObjects.Clear();
        
        tws = hpt = acm = rbm = hrm = false;
        submode("tws");
        //if (planeData != null)
        //{
        //    //planeData.SetMessage("MainMode:"+uttamRadar.MainMode.ToString());
        //    //planeData.SetMessage("SubMode:" + uttamRadar.SubMode.ToString());
        //}
    }

    //Method for update mode through UI Button
    public void SubModeSelection()
    {
        acmVSModePanel.SetActive(false);
        acmBSModePanel.SetActive(false);
        MapView.SetActive(false);
        topographicMap.SetActive(false);
        if (MainMode == Mode1.ATA)
        {
            if ((int)SubMode == 2)//5
            {
                SubMode = Mode2.TWS;
            }
            else
            {
                SubMode = (Mode2)((int)SubMode) + 1;
            }

        }
        else if (MainMode == Mode1.ATG)
        {
            if ((int)SubMode == 8)
            {
                SubMode = Mode2.HRM;
                MapView.SetActive(true);
                //topographicMap.SetActive(true);
            }
            else
            {
                SubMode = (Mode2)((int)SubMode) + 1;
            }

            if (SubMode == Mode2.HRM)
            {
                //topographicMap.SetActive(true);
                MapView.SetActive(true);
            }
            else if (SubMode == Mode2.GMTI)
            {
                MapView.SetActive(true);
                //topographicMap.SetActive(true);
            }
            else if (SubMode == Mode2.RBM)
            {
                MapView.SetActive(true);
                topographicMap.SetActive(true);
            }
        }
        else if (MainMode == Mode1.ATS)
        {
            if ((int)SubMode == 12)
            {
                SubMode = Mode2.STWS;
                MapView.SetActive(true);
            }
            else
            {
                SubMode = (Mode2)((int)SubMode) + 1;
            }


            if (SubMode == Mode2.STCT)
            {
                MapView.SetActive(true);
            }
            else if (SubMode == Mode2.ISAR)
            {
                MapView.SetActive(true);
            }
        }

        //Mod2Text.text = SubMode.ToString();
        uttamRadar.SubMode = (UttamRadar.Mode2)((int)SubMode);
    }

    public void submode(String type)
    {
        uttamRadar.zoom = 1;
        uttamRadar.xPos = 0;
        uttamRadar.yPos = 0;
        acmVSModePanel.SetActive(false);
        acmBSModePanel.SetActive(false);
        acmSAModePanel.SetActive(false);
        acmHUDModePanel.SetActive(false);
        acmHMDSModePanel.SetActive(false);
        uttamRadar.Elevation = 0;
        uttamRadar.AzimuthElev = 0;
        xsle = 0;
        ysle = 0;
        uttamRadar.HighThreatRange = uttamRadar.DefaultHighThreatRange;
        RBM = false;
        switch (type)
        {
            case "tws":
                tws = !tws;
                //hpt = !tws;
                rbm = false;
                hrm = false;

                break;
            case "agr":
                agr = !agr;
                break;
            case "crm":
                crm = !crm;
                break;
            case "acm":
                acm = !acm;
                if(acm&&MainMode==Mode1.ATA) { ACMModeSet(-1); }
                else
                {
                    uttamRadar.Azimuth = 30;
                    uttamRadar.Bar = 30;
                }
                break;
            case "rbm":
                if (MainMode == Mode1.ATG || MainMode == Mode1.ATS)
                {
                    rbm = !rbm;
                    hrm = rbm ? false : hrm;
                }
                else
                {
                    rbm = false;
                }
                break;
            case "hrm":
                if (MainMode == Mode1.ATG || MainMode == Mode1.ATS)
                {
                    hrm = !hrm;
                    rbm = rbm ? false : rbm;
                }
                else
                {
                    hrm = false;
                }
                break;
        }

        if (MainMode == Mode1.ATA)
        {
            if (tws || hpt)
            {
                if (hpt)
                {
                    if (acm)
                    {
                        SubMode = Mode2.ACM;
                    }
                    else
                    {
                        SubMode = Mode2.HPT;
                    }
                }
                else
                {
                    if (acm)
                    {
                        SubMode = Mode2.ACM;
                    }
                    else
                    {
                        SubMode = Mode2.TWS;
                    }
                }
            }

        }
        else if (MainMode == Mode1.ATG)
        {
            if (hrm)
            {
                SubMode = Mode2.HRM;
                acm = false;
                tws = true;
                agr = false;
            }
            else
            if (rbm)
            {
                SubMode = Mode2.RBM;
                tws = true;
                acm = false;
                agr = false;
            }
            else
            if (tws)
            {
                SubMode = Mode2.GMTI;
                acm = false;
                hrm = false;
                agr=false;
            }
            else
            {
                SubMode = Mode2.AGR;
                acm = false;
            }

        }
        else if (MainMode == Mode1.ATS)
        {
            hrm = false;
            acm = false;
            if (rbm)
            {
                SubMode = Mode2.ISAR;
            }
            else
            if (tws)
            {
                SubMode = Mode2.STWS;
            }
            else
            {
                SubMode = Mode2.STCT;
            }


        }

        uttamRadar.SubMode = (UttamRadar.Mode2)((int)SubMode);
        TWSText.text = tws ? "TWS" : (MainMode == Mode1.ATA ? "TWS" : "TWS");
        TWSText.text = MainMode == Mode1.ATS && !tws ? "STCT" : TWSText.text;
        TWSText.text = MainMode == Mode1.ATG ?( tws ? "GMTI" : "AGR") :TWSText.text;
        //TWSText.color = tws ? Color.green : (MainMode == Mode1.ATG ? new Color(0, 0.4f, 0) : Color.green);
        CRMText.color = crm ? Color.green : new Color(0, 0.4f, 0);
        ACMText.color = acm ? Color.green : new Color(0, 0.4f, 0);
        RBMText.text = MainMode == Mode1.ATS ? "ISAR" : "RBM";
        RBMText.color = rbm ? Color.green : new Color(0, 0.4f, 0);
        HRMText.color = hrm ? Color.green : new Color(0, 0.4f, 0);



        var m = SubMode == Mode2.ISAR || SubMode == Mode2.STWS || SubMode == Mode2.STCT || SubMode == Mode2.GMTI || SubMode == Mode2.HRM|| SubMode == Mode2.RBM|| SubMode == Mode2.AGR;
        MapView.SetActive(m);
        LandRejectToggle.SetActive(m&&MainMode == Mode1.ATS);
        LandRejectSet(false);
        RBM = SubMode == Mode2.RBM;
        //topographicMap.SetActive(SubMode == Mode2.RBM);
        //rbmStream.rbm = SubMode== Mode2.RBM;

        uttamRadar.CRM = crm;
        //if (planeData != null)
        //{
        //    planeData.SetMessage("SubMode:" + uttamRadar.SubMode.ToString());
        //}

        uttamRadar.mesh.updateRadar();
        uttamRadar.mesh2.updateRadar();
        VelocityThresholdSet(HILo);
    }
    public void azimuthSet(int i)
    {
        if (i > 0)
        {
            if (uttamRadar.Azimuth < uttamRadar.MaxAzimuth / 2f)
            {
                uttamRadar.Azimuth += 10f;
            }
        }
        else
        {
            if (uttamRadar.Azimuth > -(uttamRadar.MaxAzimuth / 2f))
            {
                uttamRadar.Azimuth -= 10f;
            }
        }
        if (uttamRadar.Azimuth < 10)
        {
            uttamRadar.Azimuth = 10;
        }
        //if (planeData != null)
        //{
        //    planeData.SetMessage("Azimuth:" + uttamRadar.Azimuth.ToString());
        //}
    }

    
    public void azimuthSet(float i)
    {
        uttamRadar.Azimuth = Mathf.Clamp(Mathf.Round(i) * 10, 10, uttamRadar.MaxAzimuth / 2f);
        
        //if (planeData != null)
        //{
        //    planeData.SetMessage("Azimuth:" + uttamRadar.Azimuth.ToString());
        //}
    }

    public void ISARSet(bool i)
    {
        uttamRadar.SarPostProcess.enabled = !i;
        uttamRadar.IsarPostProcess.enabled = i;
        uttamRadar.RbmPostProcess.enabled = false;
        ISAR = i;
        if (i)
        {
            // Get the layer indices by name
            //int groundLayer = LayerMask.NameToLayer("Ground");
            int entityLayer = LayerMask.NameToLayer("Entity");
            int FlamneLayer = LayerMask.NameToLayer("Flame");
            // Set the culling layers for the camera using bitwise OR
            int layerMask = 1 << entityLayer | 1 << FlamneLayer;
            uttamRadar.MapCamera.cullingMask = layerMask;
            Color color;
            if (ColorUtility.TryParseHtmlString("#0006A9", out color))
            {
                // Set the background color of the camera
                uttamRadar.MapCamera.backgroundColor = color;

               // Debug.Log("Camera background color changed to: " + hexColor);
            }
        }
        else
        {
            int groundLayer = LayerMask.NameToLayer("Ground");
            int entityLayer = LayerMask.NameToLayer("Entity");
            int FlamneLayer = LayerMask.NameToLayer("Flame");
            // Set the culling layers for the camera using bitwise OR
            int layerMask = 1 << groundLayer | 1 << entityLayer | 1 << FlamneLayer;
            uttamRadar.MapCamera.cullingMask = layerMask;
            Color color;
            if (ColorUtility.TryParseHtmlString("#000000", out color))
            {
                // Set the background color of the camera
                uttamRadar.MapCamera.backgroundColor = color;

                // Debug.Log("Camera background color changed to: " + hexColor);
            }
        }
    }

    public void barSet(int i)
    {
        if (i > 0)
        {
            if (uttamRadar.Bar < uttamRadar.MaxBar / 2f)
            {
                uttamRadar.Bar += 10f;
            }
        }
        else
        {
            if (uttamRadar.Bar > -(uttamRadar.MaxBar / 2f))
            {
                uttamRadar.Bar -= 10f;
            }
        }
        if (uttamRadar.Bar < 10)
        {
            uttamRadar.Bar = 10;
        }
        //if (planeData != null)
        //{
        //    planeData.SetMessage("Bar:" + uttamRadar.Bar.ToString());
        //}
    }

    public void barSet(float i)
    {
        uttamRadar.Bar = Mathf.Clamp(Mathf.Round(i) * 10, 10, uttamRadar.MaxBar / 2f);

        //if (planeData != null)
        //{
        //    planeData.SetMessage("Azimuth:" + uttamRadar.Azimuth.ToString());
        //}
    }

    public void ElevationSet(int i)
    {
        if (i > 0)
        {
            if (uttamRadar.Elevation < uttamRadar.MaxElevation)
            {
                uttamRadar.Elevation += 2f;
            }
        }
        else
        {
            if (uttamRadar.Elevation > 0)
            {
                uttamRadar.Elevation -= 2f;
            }
        }

        //if (planeData != null)
        //{
        //    planeData.SetMessage("Bar:" + uttamRadar.Bar.ToString());
        //}
    }


    public void RangeScaleSet(int i)
    {
        if (i==0)
        {
            if (RangeScale>10)
            {
                RangeScale = RangeScale/2;
            }
        }
        else
        {
            //if ((RangeScale + 5) < uttamRadar.Range)
            if (RangeScale < 80)
            {
                RangeScale = RangeScale*2;
            }
        }


        //if (planeData != null)
        //{
        //    planeData.SetMessage("RangeScale:" +RangeScale.ToString());
        //}
    }


    public void BandwidthSet(int i)
    {
        if (i == 0)
        {
            if ((int)band - 1 >= 0)
            {
                band = (Bandwidth)((int)band) - 1;
            }
        }
        else
        {
            if ((int)band + 1 < 5)
            {
                band = (Bandwidth)((int)band) + 1;
            }
        }
        BandwidthNobe.rotation = Quaternion.Euler(0, 0, ((int)band)* -58.45f);
        
        if ((int)band == 0)
        {
            BandwidthInputField.gameObject.SetActive(true);
            float f = float.Parse(BandwidthInputField.text);
            f = f < 8 ? 8 : (f > 12 ? 12 : f);
            Debug.Log(f);
            BandwidthInputField.text = f.ToString();
            uttamRadar.f = f;
        }
        else
        {
            BandwidthInputField.gameObject.SetActive(false);
            //BandwidthInputField.text = "8.00";
            uttamRadar.f = (int)band + 8;
        }
        
        //uttamRadar.f = (int)band+8;

    }

    public void ACMModeSet(int i)
    {
        
        if (i >= 0)
        {
            if (i == 0)
            {
                if ((int)ACMMode - 1 >= 0)
                {
                    ACMMode = (ACMModes)((int)ACMMode) - 1;
                }
            }
            else
            {
                if ((int)ACMMode + 1 < 6)
                {
                    ACMMode = (ACMModes)((int)ACMMode) + 1;
                }
            }
        }
        ACMModeNobe.rotation = Quaternion.Euler(0, 0, ((int)ACMMode) * -45.45f);
        acmVSModePanel.SetActive(false);
        acmBSModePanel.SetActive(false);
        acmSAModePanel.SetActive(false);
        acmHUDModePanel.SetActive(false);
        acmHMDSModePanel.SetActive(false);
        uttamRadar.Elevation = 0;
        uttamRadar.AzimuthElev = 0;
        xsle = 0;
        ysle = 0;
        if (!acm) return;
        switch ((int)ACMMode)
        {
            
            case 0:
                //vertical Scan
                uttamRadar.Azimuth = 5;
                uttamRadar.Bar = 30;
                acmVSModePanel.SetActive(true);
                uttamRadar.HighThreatRange = 20;
                RangeScale = 20;
                break;
            case 1:
                //Borsight Scan
                uttamRadar.Azimuth = 5;
                uttamRadar.Bar = 5;
                acmBSModePanel.SetActive(true);
                uttamRadar.HighThreatRange = 55;
                RangeScale = 55;
                break;
            case 2:
                //Slewable Scan
                uttamRadar.Azimuth = 10;
                uttamRadar.Bar = 10;
                acmSAModePanel.SetActive(true);
                uttamRadar.HighThreatRange = 30;
                RangeScale = 30;
                break;
            case 3:
                //Full Scan
                uttamRadar.Azimuth = 60;
                uttamRadar.Bar = 60;
                uttamRadar.HighThreatRange = 55;
                RangeScale = 55;
                break;
            case 4:
                //HUD Scan
                uttamRadar.Azimuth = 25;
                uttamRadar.Bar = 25;
                acmHUDModePanel.SetActive(true);
                uttamRadar.HighThreatRange = 20;
                RangeScale = 20;
                break;
            case 5:
                //HMDS Scan
                uttamRadar.Azimuth = 10;
                uttamRadar.Bar = 10;
                acmHMDSModePanel.SetActive(true);
                uttamRadar.HighThreatRange = 45;
                RangeScale = 45;
                break;


        }

        //uttamRadar.f = (int)band+8;

    }

    public void RstCursor()
    {
        uttamRadar.xPos = 0;
        uttamRadar.yPos = 0;
        uttamRadar.zoom = 1;
    }
    public void ZoomSet(int i)
    {
        zoom = i;
    }

    public void ZoomXPosSet(int i)
    {
        xzPos = i;
    }

    public void ZoomYPosSet(int i)
    {
        yzPos = i;
    }

    public void peactimeWartime(bool i)
    {
        uttamRadar.peactimeWartimeAccuracy = i ? 1 : 0.5f;
        uttamRadar.TRMPower = i ? uttamRadar.TRMPower*2f : uttamRadar.TRMPower/2f;
    }

    public void VelocityThresholdSet(bool i)
    {
        int high = MainMode == Mode1.ATA ? 500 : 50;
        HILo = i;
        uttamRadar.VelocityThreshold = i ? 0 : high;
    }

    public void GainSet(int i)
    {
        if (i == 0)
        {
            if (gain-1 >= -2)
            {
                gain = gain - 1;
            }
        }
        else
        {
            if (gain+1 < 3)
            {
                gain = gain + 1;
            }
        }
        GainNobe.rotation = Quaternion.Euler(0, 0, (gain) * -58.45f);
        uttamRadar.G = gain+3;
    }



    public void ptonlySet(bool i)
    {
        PTOnly = i;
        uttamRadar.PtOnly = i;
        //if (planeData != null)
        //{
        //    planeData.SetMessage("PTOnly:" + (PTOnly?"True":"False"));
        //}
    }
    public void AastSet(bool i)
    {
        AAST = !i;
        
    }

    public void AastSet(int i)
    {
        AAST = i==0;

    }

    public void DutyCycleSet(bool i)
    {
        ABDutyCycle = i;
        uttamRadar.ABDutyCycle = i;
        //if (planeData != null)
        //{
        //    planeData.SetMessage("TxSil:" + (TxSil ? "True" : "False"));
        //}
    }

    public void BScopeSet(bool i)
    {
        BScope = i;
        BScopePanel.SetActive(i);
        PPIPanel.SetActive(!i);
    }

    public void TxSilSet(bool i)
    {
        TxSil = i;
        uttamRadar.TxSil = i;
        //if (planeData != null)
        //{
        //    planeData.SetMessage("TxSil:" + (TxSil ? "True" : "False"));
        //}
    }
    public void CitSet(bool i)
    {
        Cit = i;
        uttamRadar.Cit = i;
        //if (planeData != null)
        //{
        //    planeData.SetMessage("Cit:" + (Cit ? "True" : "False"));
        //}
    }

    public void applypower(float i)
    {
        uttamRadar.applyPower = i/3f;
        
    }
    public void WASet(bool i)
    {
        WA = i;
        weatherRadar.SetActive(i);
        WeatherMap.SetActive(i);
        //Forground.SetActive(!i);
        //if (planeData != null)
        //{
        //    planeData.SetMessage("WA:" + (WA ? "True" : "False"));
        //}
    }

    public void HRMSet(bool i)
    {
        if (i)
        {
            submode("hrm");
        }
        else
        {
            submode("rbm");
            if (MainMode == Mode1.ATG)
                RBMToggle.isOn = true;
        }
        
        //if (planeData != null)
        //{
        //    planeData.SetMessage("JEM:" + (jem? "True" : "False"));
        //}
    }

    public void SARSet(bool i)
    {
        SAR = i;
        uttamRadar.SAR = i;
        RBMToggle.isOn=true;
    }

    public void RBMSet(bool i)
    {
        
        if (i)
        {
            if (MainMode == Mode1.ATG)
                HRMToggle.isOn = false;
            //submode("rbm");
        }
        else
        {
            //submode("hrm");
            if(MainMode==Mode1.ATG)
            HRMToggle.isOn = true;
        }
        return;
        if (SubMode == Mode2.RBM)
        {
            uttamRadar.SarPostProcess.enabled = !i;
            uttamRadar.IsarPostProcess.enabled = false;
            uttamRadar.RbmPostProcess.enabled = i;
            if (i)
            {
                // Get the layer indices by name
                int groundLayer = LayerMask.NameToLayer("Ground");
                //int entityLayer = LayerMask.NameToLayer("Entity");
                //int FlamneLayer = LayerMask.NameToLayer("Flame");
                // Set the culling layers for the camera using bitwise OR
                int layerMask = 1 << groundLayer;// | 1 << FlamneLayer;
                uttamRadar.MapCamera.cullingMask = layerMask;
                Color color;
                if (ColorUtility.TryParseHtmlString("#000000", out color))
                {
                    // Set the background color of the camera
                    uttamRadar.MapCamera.backgroundColor = color;

                    // Debug.Log("Camera background color changed to: " + hexColor);
                }
            }
            
        }
        else
        {
            uttamRadar.SarPostProcess.enabled = !i;
            uttamRadar.IsarPostProcess.enabled = false;
            uttamRadar.RbmPostProcess.enabled = i;
            int groundLayer = LayerMask.NameToLayer("Ground");
            int entityLayer = LayerMask.NameToLayer("Entity");
            int FlamneLayer = LayerMask.NameToLayer("Flame");
            // Set the culling layers for the camera using bitwise OR
            int layerMask = 1 << groundLayer | 1 << entityLayer | 1 << FlamneLayer;
            uttamRadar.MapCamera.cullingMask = layerMask;
            Color color;
            if (ColorUtility.TryParseHtmlString("#000000", out color))
            {
                // Set the background color of the camera
                uttamRadar.MapCamera.backgroundColor = color;

                // Debug.Log("Camera background color changed to: " + hexColor);
            }
        }



        //if (planeData != null)
        //{
        //    planeData.SetMessage("JEM:" + (jem? "True" : "False"));
        //}
    }

    public void LandRejectSet(bool i)
    {

        if (MainMode == Mode1.ATS && !ISAR)
        {
            if (i)
            {
                // Get the layer indices by name
                //int groundLayer = LayerMask.NameToLayer("Ground");
                int entityLayer = LayerMask.NameToLayer("Entity");
                int FlamneLayer = LayerMask.NameToLayer("Flame");
                // Set the culling layers for the camera using bitwise OR
                int layerMask = 1 << entityLayer | 1 << FlamneLayer;
                uttamRadar.MapCamera.cullingMask = layerMask;
                Color color;
                if (ColorUtility.TryParseHtmlString("#000000", out color))
                {
                    // Set the background color of the camera
                    uttamRadar.MapCamera.backgroundColor = color;

                    // Debug.Log("Camera background color changed to: " + hexColor);
                }
            }
            else
            {
                int groundLayer = LayerMask.NameToLayer("Ground");
                int entityLayer = LayerMask.NameToLayer("Entity");
                int FlamneLayer = LayerMask.NameToLayer("Flame");
                // Set the culling layers for the camera using bitwise OR
                int layerMask = 1 << groundLayer | 1 << entityLayer | 1 << FlamneLayer;
                uttamRadar.MapCamera.cullingMask = layerMask;
                Color color;
                if (ColorUtility.TryParseHtmlString("#000000", out color))
                {
                    // Set the background color of the camera
                    uttamRadar.MapCamera.backgroundColor = color;

                    // Debug.Log("Camera background color changed to: " + hexColor);
                }
            }
        }
        else
        {
            int groundLayer = LayerMask.NameToLayer("Ground");
            int entityLayer = LayerMask.NameToLayer("Entity");
            int FlamneLayer = LayerMask.NameToLayer("Flame");
            // Set the culling layers for the camera using bitwise OR
            int layerMask = 1 << groundLayer | 1 << entityLayer | 1 << FlamneLayer;
            uttamRadar.MapCamera.cullingMask = layerMask;
            Color color;
            if (ColorUtility.TryParseHtmlString("#000000", out color))
            {
                // Set the background color of the camera
                uttamRadar.MapCamera.backgroundColor = color;

                // Debug.Log("Camera background color changed to: " + hexColor);
            }
        }



    }

    public void JEMSet(bool i)
    {
        jem = i;
        uttamRadar.JEM = i;
        //if (planeData != null)
        //{
        //    planeData.SetMessage("JEM:" + (jem? "True" : "False"));
        //}
    }

    public void AJSet(bool i)
    {
        aj = i;
        uttamRadar.AJ = i;
        if (i)
        {
            StartCoroutine(freqChangeRapid(0));
            GainSet(1);
            GainSet(1);
            GainSet(1);
            GainSet(1);
            GainSet(1);
        }
        else
        {
            GainSet(0);
            GainSet(0);
            GainSet(0);
            GainSet(0);
            GainSet(0);
        }
        //if (planeData != null)
        //{
        //    planeData.SetMessage("JEM:" + (jem? "True" : "False"));
        //}
    }

    public void LPISet(bool i)
    {
        lpi = i;
        
        if (i)
        {
            StartCoroutine(freqChangeRapid2(0));
            GainSet(0);
            GainSet(0);
            GainSet(0);
            GainSet(0);
            GainSet(0);
        }
        else
        {
            GainSet(1);
            GainSet(1);
            GainSet(1);
            GainSet(1);
            GainSet(1);
        }
        //if (planeData != null)
        //{
        //    planeData.SetMessage("JEM:" + (jem? "True" : "False"));
        //}
    }

    IEnumerator freqChangeRapid2(int i)
    {
        yield return new WaitForSeconds(0.3f);
        if (uttamRadar.SelectedTarget != null)
        {
            BandwidthSet(i);
            if (uttamRadar.f == 12)
            {
                i = 0;
            }
            if (uttamRadar.f == 8)
            {
                i = 1;
            }
        }
        if (lpi)
        {
            StartCoroutine(freqChangeRapid2(i));
        } 
    }

    IEnumerator freqChangeRapid(int i)
    {
        yield return new WaitForSeconds(1);
        BandwidthSet(i);
        if(uttamRadar.f == 12)
        {
            i = 0;
        }
        if (uttamRadar.f == 8)
        {
            i = 1;
        }
        if (aj)
        {
            StartCoroutine(freqChangeRapid(i));
        }
    }



    public void PassiveModeSet(bool i)
    {
        PassiveMode = i;
        uttamRadar.PassiveMode = i;
        uttamRadar.mesh.updateRadar();
        uttamRadar.mesh2.updateRadar();
        //if (planeData != null)
        //{
        //    planeData.SetMessage("JEM:" + (jem? "True" : "False"));
        //}
    }


    public void CuedSet(bool i)
    {
        cued = i;
        uttamRadar.cued = i;
        MaxAlt.text = minMax(float.Parse(MaxAlt.text), 100, 50000).ToString();
        MaxSpeed.text = minMax(float.Parse(MaxSpeed.text), 53, 7000).ToString();
        MaxRange.text = minMax(float.Parse(MaxRange.text), 1, 70).ToString();

        MinAlt.text = minMax(float.Parse(MinAlt.text), 100, float.Parse(MaxAlt.text)).ToString();
        MinSpeed.text = minMax(float.Parse(MinSpeed.text), 53, float.Parse(MaxSpeed.text)).ToString();
        MinRange.text = minMax(float.Parse(MinRange.text), 1, float.Parse(MaxRange.text)).ToString();

        uttamRadar.MinAltitude = float.Parse(MinAlt.text);
        uttamRadar.MaxAltitude = float.Parse(MaxAlt.text);
        uttamRadar.MinSpeed = float.Parse(MinSpeed.text);
        uttamRadar.MaxSpeed = float.Parse(MaxSpeed.text);
        uttamRadar.MinRange = float.Parse(MinRange.text);
        uttamRadar.MaxRange = float.Parse(MaxRange.text);
        uttamRadar.mesh.updateRadar();
        uttamRadar.mesh2.updateRadar();
    }

    public float minMax(float v, float min, float max)
    {
        v = v<min ? min : v;
        v = v>max ? max : v;
        return v;
    }


    public void MapContrast(float i)
    {
        if (i < -45)
        {
            i = 5;
        }
        uttamRadar.colorGrading.postExposure.value = i/10f;
        uttamRadar.colorGrading.contrast.value = i;
        contrast = i;
        uttamRadar.contrast = contrast;
        uttamRadar.WeatherMode.Contrast = MapToRange(i, -50, 100, 0, 1);
    }

    float MapToRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
    }



    public void FreezeUnfreeze(bool i)
    {
        uttamRadar.MapCamera.gameObject.SetActive(!i);
    }

    public void selectNextTarget()
    {
        uttamRadar.SelectNextTarget();
    }
    public void clear()
    {
        uttamRadar.clear();
    }
}
