using Assets.Code.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utility.LatLonAlt;
using Weapon.FireMissile;
using WorldComposer;
using static CIT;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class ByteData
{
    [SerializeField]
    public string name;
    [SerializeField]
    public data_type data_Type;
    [SerializeField]
    public string unit;
    [SerializeField]
    public int size;
    [SerializeField]
    public int vInt;
    [SerializeField]
    public float vFloat;
    [SerializeField]
    public bool vbool;
    [SerializeField]
    public char vChar;

}

[Serializable]
public class Table
{
    [SerializeField]
    public TMP_InputField StartByte;
    [SerializeField]
    public TMP_Dropdown Data_Type;
    [SerializeField]
    public TMP_InputField Byte_Value;
    [SerializeField]
    public TMP_InputField Convert_Value;
    [SerializeField]
    public TMP_Dropdown Axis;
    [SerializeField]
    public Toggle Active;
}

public enum data_type
{
    Byte,
    Int,
    uInt,
    Float,
    Double,
    Decimal,
    Bool,
    Long,
    uLong,
    Char
}

public enum axis
{
    none,
    scn0,
    scn1,
    scn2,
    designate0,
    designate1,
    designate2,
    designate3,
    htpt0,
    htpt1,
    htpt2,
    htpt3,
    AttackMode,
    WeaponType,
    TimeToImpact0,
    TimeToImpact1,
    TimeToImpact2,
    TimeToImpact3,
    Trigger,
    Gear,
    Pause,
    RWR_Dispense,
    AircraftType,
    pitch,
    roll,
    yaw,
    throttle,
    brake,
    RangeInc,
    RangeDec,
    Azimuth,
    Bar,
    Elevation,
    AastNast,
    ChaffsRealase,
    FlareRealase,
    LockOn
}

[Serializable]
public class byteDataId
{
    [SerializeField]
    public int StartByte;
    [SerializeField]
    public data_type Data_Type;
    [SerializeField]
    public axis Axis;
    [SerializeField]
    public bool Active;
}

public class UDP_REC_Transmit : MonoBehaviour
{
    

    public int sendingPort = 5000; // Change this to the desired port
    public int recePort = 10000;
    public string sendingIp = "";
    public string reciveIp = "192.168.1.100";
    public bool RecUdpPacket = false;
    public float pitch = 0;
    public float roll = 0;
    public float yaw = 0;
    public float throttle = 0;
    public bool trigger = false;
    public bool gear = false;
    public bool brake = false;
    public bool pause = false;
    public bool Lock = false;
    public int WeaponType = 0;
    public bool rwr = false;
    public bool ChaffsRelease = false;
    public bool FlareReleasr = false;
    public int AttackMode = 1;
    public bool RangeInc = false;
    public bool RangeDec = false;
    public int Azimuth = 3;
    public int Bar = 3;
    public bool AAST = false;
    public bool Crm = false;

    [Header("UI")]
    public Toggle UdpEnable;
    public TMP_InputField SendPortFeild;
    public TMP_InputField SendIpFeild;
    public TMP_InputField RecPortFeild;
    public Toggle LogUpdate;
    public TextMeshProUGUI LogText;
    public Transform TableParent;
    public List<Table> Table = new List<Table>();
    public TMP_InputField Pitch_Sens;
    public TMP_InputField Roll_Sens;      
    public TMP_InputField Yaw_Sens;
    public TMP_InputField Throttle_Sens;
    public List<byteDataId> byteId = new List<byteDataId>();
    public bool SaveData = false;

    [Header("SendingData")]
    public TextAsset csvData;
    public List<ByteData> byteData = new List<ByteData>();
    public int byteCount = 0;

    //
    [Header("Status")]
    public List<Specification> allys = new List<Specification>();
    public List<Specification> enemy = new List<Specification>();
    public List<Specification> sams = new List<Specification>();
    public List<Specification> ship = new List<Specification>();

    public int count = 0;
    private byte[] recByte;
    private UdpClient udpClient;
    private System.Threading.Thread receiveThread;
    public bool isRunning = true;
    public bool isbound = false;
    [Header("Refrences")]
    public SilantroController controller;
    public MissileFire missileFire;
    public WeaponTracker weaponTracker;
    public BasicControle basicControle;
    public Assets.Code.Plane.Actuation actuation;
    public ExtrasIntegration extra;
    public Specification specification;
    public RadarScreenUpdate_new radarScreen;
    public EWRadar ewRadar;
    public EW_Inputs ew_inputs;
    public Utility.LatLonAlt.LatLong latLong;
    public WaypointsViewer waypointsViewer;
    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    // Start is called before the first frame update
    void Start()
    {
        string datStr = PlayerPrefs.GetString("DataID");
       if(datStr == "")
        {
            SaveData = true;
        }
        if (SaveData) {
            datStr = FeedBackRecorderAndPlayer.JsonHelper.ToJson<byteDataId>(byteId);
            PlayerPrefs.SetString("DataID", datStr);
            byteId = FeedBackRecorderAndPlayer.JsonHelper.FromJson<byteDataId>(datStr);
        }
        else
        {
            byteId = FeedBackRecorderAndPlayer.JsonHelper.FromJson<byteDataId>(datStr);
        }
        latLong = GameObject.FindObjectOfType<Utility.LatLonAlt.LatLong>();
        waypointsViewer = GameObject.FindObjectOfType<WaypointsViewer>();
        for (int i = 0; i < TableParent.childCount; i++)
        {

            TMP_InputField strtbyte = TableParent.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
            strtbyte.text = (i < byteId.Count ? byteId[i].StartByte : 0).ToString();
            TMP_Dropdown datatype = TableParent.GetChild(i).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>();
            datatype.value = i < byteId.Count ? (int)byteId[i].Data_Type : 0;

            TMP_Dropdown axiss = TableParent.GetChild(i).GetChild(4).GetChild(0).GetComponent<TMP_Dropdown>();
            axiss.options.Clear();
            List<string> names = new List<string>();
            Array enumValues = Enum.GetValues(typeof(axis));
            foreach (var enumValue in enumValues)
            {
                names.Add(enumValue.ToString());
            }
            axiss.AddOptions(names);
            axiss.value = i < byteId.Count ? (int)byteId[i].Axis : 0;

            Toggle actve = TableParent.GetChild(i).GetChild(5).GetChild(0).GetComponent<Toggle>();
            actve.isOn = i < byteId.Count ? byteId[i].Active : false;

            Table t = new Table();
            t.StartByte = strtbyte;
            t.Data_Type = datatype;
            t.Byte_Value = TableParent.GetChild(i).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();
            t.Convert_Value = TableParent.GetChild(i).GetChild(3).GetChild(0).GetComponent<TMP_InputField>();
            t.Axis = axiss;
            t.Active = actve;

            Table.Add(t);
            if (i >= byteId.Count)
            {
                byteId.Add(new byteDataId());
            }
        }

        string[] s = csvData.text.Split('\n');
        byteData = new List<ByteData>();
        byteCount = 0;
        int k = 0; 
        foreach(string str in s)
        {
            if (k < 2)
            {
                k++;
                continue;
            }
            k++;
            
            string[] sA = str.Split(",");
            Debug.Log(sA[0]);
            ByteData d = new ByteData();
            d.name = sA[1];
            d.unit = sA[2];
            //byteCount += (int)float.Parse(sA[5]);
            if (sA[4].ToLower().Contains("float"))
            {
                d.data_Type = data_type.Float;
                d.size = 4;
                byteCount += 4;
            }else
            if (sA[4].ToLower().Contains("bool"))
            {
                d.data_Type = data_type.Bool;
                d.size = 4;
                byteCount += 4;
            }
            else
            if (sA[4].ToLower().Contains("integer"))
            {
                d.data_Type = data_type.Int;
                d.size = 4;
                byteCount += 4;
            }
            else
            {
                byteCount += 0;
            }
            byteData.Add(d);
        }

        // Start listening for incoming UDP messages on a separate thread
        RecUpdatePort(RecPortFeild.text);
        SendUpdatePort(SendPortFeild.text);
        SendingUpdateIp(SendIpFeild.text);
        StartCoroutine(WaitForUdp());
        Invoke("entityFind",2f);
    }

    public void UpdateByteId()
    {
        int i = 0;
        foreach(Table t in Table)
        {
            byteId[i].StartByte = (int)float.Parse(t.StartByte.text);
            byteId[i].Data_Type = (data_type)t.Data_Type.value;
            byteId[i].Axis = (axis)t.Axis.value;
            byteId[i].Active = t.Active.isOn;
            i++;
        }
        string datStr = FeedBackRecorderAndPlayer.JsonHelper.ToJson<byteDataId>(byteId);
        PlayerPrefs.SetString("DataID", datStr);
    }


    public void entityFind()
    {
        foreach (GameObject ally in loadSystemAndSpawnPlanes. AllyPlanes)
        {
            allys.Add(ally.GetComponent<Specification>());
        }
        foreach (GameObject adver in loadSystemAndSpawnPlanes.AdversaryPlanes)
        {
            enemy.Add(adver.GetComponent<Specification>());
        }
        foreach (GameObject sam in loadSystemAndSpawnPlanes.Sams)
        {
            sams.Add(sam.GetComponentInChildren<Specification>());
        }
        foreach (GameObject warship in loadSystemAndSpawnPlanes.Warships)
        {
            ship.Add(warship.GetComponentInChildren<Specification>());
        }
    }

    public void ControlShift(bool i)
    {
        controller.inputType = i? SilantroController.InputType.UDP: SilantroController.InputType.Default;
    }

    public void udpStartStop(bool i)
    {
        if (i)
        {//start udp
            StartReceive();
        }
        else
        {//close udp
            StopReceive();
        }
        
    }
    private void FixedUpdate()
    {
        if (isbound)
        {
            //encode();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(isbound)
        {

            encode();
            foreach (Table t in Table)
            {
                if (t.Active.isOn)
                {
                    string s = Decode(recByte, int.Parse(t.StartByte.text), (data_type)t.Data_Type.value);
                    t.Convert_Value.text = s;

                    if(t.Axis.value == (int)axis.pitch)
                    {
                        pitch = float.Parse(s);
                    }else
                    if (t.Axis.value == (int)axis.roll)
                    {
                        roll = float.Parse(s);
                    }else
                    if (t.Axis.value == (int)axis.yaw)
                    {
                        yaw = float.Parse(s);
                    }else
                    if (t.Axis.value == (int)axis.throttle)
                    {
                        throttle = float.Parse(s);
                    }else
                    if (t.Axis.value == (int)axis.brake)
                    {
                        brake = float.Parse(s)>0.5;
                    }else
                    if (t.Axis.value == (int)axis.Trigger)
                    {
                        trigger = float.Parse(s) > 0.5;
                    }else
                    if (t.Axis.value == (int)axis.Gear)
                    {
                        gear = float.Parse(s) > 0.5;
                    }else
                    if (t.Axis.value == (int)axis.Pause)
                    {
                        pause = float.Parse(s) > 0.5;
                    }else
                    if (t.Axis.value == (int)axis.WeaponType)
                    {
                        WeaponType = (int)float.Parse(s);
                    }
                    else
                    if (t.Axis.value == (int)axis.RWR_Dispense)
                    {
                        rwr = float.Parse(s) > 0.5;
                    }
                    else
                    if (t.Axis.value == (int)axis.ChaffsRealase)
                    {
                        ChaffsRelease = float.Parse(s) > 0.5;
                    }
                    else
                    if (t.Axis.value == (int)axis.FlareRealase)
                    {
                        FlareReleasr = float.Parse(s) > 0.5;
                    }
                    else
                    if (t.Axis.value == (int)axis.LockOn)
                    {
                        Lock = float.Parse(s) > 0.5;
                    }
                    else
                    if (t.Axis.value == (int)axis.AttackMode)
                    {
                        AttackMode = (int)float.Parse(s);
                    }
                    else
                    if (t.Axis.value == (int)axis.RangeInc)
                    {
                        RangeInc = float.Parse(s) > 0.5;
                    }
                    else
                    if (t.Axis.value == (int)axis.RangeDec)
                    {
                        RangeDec = float.Parse(s) > 0.5;
                    }
                    else
                    if (t.Axis.value == (int)axis.Azimuth)
                    {
                        bool b = float.Parse(s) > 0.5;
                        if (b)
                        { 
                            Azimuth += 3;
                            Azimuth = Azimuth > 6 ? 0 : Azimuth;
                        }
                    }
                    else
                    if (t.Axis.value == (int)axis.Bar)
                    {
                        Bar = (int)float.Parse(s);
                    }
                    else
                    if (t.Axis.value == (int)axis.AastNast)
                    {
                        AAST = float.Parse(s) > 0.5;
                    }
                    /*
                    else
                    if (t.Axis.value == (int)axis.RangeDec)
                    {
                        Crm = float.Parse(s) > 0.5;
                    }
                    */
                }
            }
        }

        if(controller.inputType == SilantroController.InputType.UDP)
        {
            controller.input.rawPitchInput = pitch * float.Parse(Pitch_Sens.text);
            controller.input.rawRollInput = roll * float.Parse(Roll_Sens.text);
            controller.input.rawYawInput = yaw * float.Parse(Yaw_Sens.text);
            controller.input.rawThrottleInput = throttle * float.Parse(Throttle_Sens.text);
            weaponTracker.WeaponType = WeaponType;
            if (trigger && WeaponType == 0)
            {
                weaponTracker.gun.FireGun();
            }
            else
            if (trigger && WeaponType == 3)
            {
                missileFire.mDrop();
            }
            else
            {
                missileFire.mFire();
            }

            if (pause)
            {
                if (!basicControle.pause)
                {
                    if (!basicControle.MenuPanel.activeSelf)
                    {
                        basicControle.pause = !basicControle.pause;
                        basicControle.PauseGame(basicControle.pause);
                    }
                }
            }
            else
            {
                if (basicControle.pause)
                {
                    if (!basicControle.MenuPanel.activeSelf)
                    {
                        basicControle.pause = !basicControle.pause;
                        basicControle.PauseGame(basicControle.pause);
                    }
                }
            }

            if (gear)
            {
                actuation.CheckAnimation();
            }

            if (rwr && (ChaffsRelease||FlareReleasr))
            {
                ewRadar.SpawnFlare();
                ewRadar.SpawnChaff();
            }

            if(AttackMode != ((int)radarScreen.uttamRadar.MainMode+1))
            {
                if (AttackMode > 0)
                {
                    radarScreen.MainModeSelection();
                }
            }

            if (RangeInc)
            {
                radarScreen.RangeScaleSet(1);
            }
            if (RangeDec)
            {
                radarScreen.RangeScaleSet(0);
            }
            if (!radarScreen.acm)
            {
                radarScreen.azimuthSet((float)Azimuth);
                radarScreen.barSet((float)Bar);
            }

            if (Crm)
            {
                if (!radarScreen.uttamRadar.CRM)
                {
                    radarScreen.submode("crm");
                }
            }
            else
            {
                if (radarScreen.uttamRadar.CRM)
                {
                    radarScreen.submode("crm");
                }
            }
            radarScreen.AastSet(AAST);

            if (Lock)
            {
                if (weaponTracker.radar.SelectedTarget == null)
                {
                    radarScreen.manualSelectTarget(1);
                    weaponTracker.radar.ManualLockTarget();
                    weaponTracker.AutoMissileSelect();
                }

            }
            else
            {
                if (weaponTracker.radar.SelectedTarget != null)
                {
                    weaponTracker.radar.ManualBreakLockTarget();
                }
            }
        }

    }

    public void encode()
    {
        byte[] result = new byte[byteCount];
        string stringdata = "";
        byte hexValue = 0x09;

        // Convert byte to ASCII character
        char asciiChar = (char)hexValue;
        string join = asciiChar+""; 
        int i = 0;
        bool loop = false;
        bool sloop = false;
        bool eloop = false;
        bool Wloop = false;
        bool t1loop = false;
        bool t2loop = false;
        int count = 0;
        //string decmal = "0.000000";
        for (int k =0;k<byteData.Count;k++)
        {
            string strdata = "";
            ByteData data = byteData[k];
            if (data.name == "Roll_D")//it is use as id
            {
                // Original float value
                float originalValue = FeedBackRecorderAndPlayer.currentFrame;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "delta-sec")
            {
                // Original float value
                float originalValue = Time.deltaTime;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "pressure-alt-ft")
            {
                // Original float value
                float originalValue = specification.entityInfo.Altitude;//feet
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Radio_altitude-alt-ft")
            {
                // Original float value
                float originalValue = specification.entityInfo.Altitude;//feet
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "heading_deg")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Heading;//deg
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Mach_number")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Mach;//mach
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "CAS")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Speed;//knots
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "AoA")
            {
                // Original bool value
                float originalValue = specification.entityInfo.AngleOfAttack;//deg
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "VSI")
            {
                // Original bool value
                float originalValue = specification.entityInfo.VerticleVelocity;//fpsquare
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Aircraft_Acceleration")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Acceleration;//fpsquare
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Ve_velocity_east")
            {
                // Original bool value
                float originalValue = specification.entityInfo.xVelocity;//fps
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Vn_velocity_north")
            {
                // Original bool value
                float originalValue = specification.entityInfo.yVelocity;//fps
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Vz_velocity_down")
            {
                // Original bool value
                float originalValue = specification.entityInfo.zVelocity;//fps
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "acceleration_x")
            {
                // Original bool value
                float originalValue = specification.entityInfo.xAccel;//fpsquare
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "acceleration_y")
            {
                // Original bool value
                float originalValue = specification.entityInfo.yAccel;//fpsquare
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "acceleration_z")
            {
                // Original bool value
                float originalValue = specification.entityInfo.zAccel;//fpsquare
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Theta_D")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Pitch;//deg
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Phi_D")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Roll;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Psi_D")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Heading;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "HTPT_ENABLE")
            {
                // Original bool value
                bool originalValue = weaponTracker.radar.SelectedTarget;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += (originalValue ? 1 : 0).ToString("0.000000") + join;
            }
            else
            if (data.name == "UNDESIGNATE")
            {
                // Original bool value
                bool originalValue = !weaponTracker.radar.SelectedTarget;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += (originalValue ? 1 : 0).ToString("0.000000") + join;
            }
            else
            if (data.name == "Lattitude_D")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Lat;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "Longitude_D")
            {
                // Original bool value
                float originalValue = specification.entityInfo.Long;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "RANGE_INCREASE")
            {
                // Original bool value
                bool originalValue = RangeInc;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += (originalValue ? 1 : 0).ToString("0.000000") + join;
            }
            else
            if (data.name == "RANGE_DECREASE")
            {
                // Original bool value
                bool originalValue = RangeDec;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += (originalValue ? 1 : 0).ToString("0.000000") + join;
            }
            if (data.name == "GROUND_SPEED")
            {
                // Original bool value
                float originalValue = specification.entityInfo.GroundSpeed;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "T10_TX_ON_OFF")
            {
                // Original bool value
                bool originalValue = radarScreen.radarlog.TxSil;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += (originalValue ?1:0).ToString("0.000000") + join;
            }
            else
            if (data.name == "engineN2")
            {
                // Original bool value
                float originalValue = specification.entityInfo.EngineRPM;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "InterrogationOn")
            {
                // Original bool value
                bool originalValue = specification.cit.CitModes != CITModes.None;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += (originalValue ? 1 : 0).ToString("0.000000") + join;
            }
            else
            if (data.name == "CCM_MOdesSelect")
            {
                // Original bool value
                bool originalValue = weaponTracker.WeaponType == 2;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += (originalValue ? 1 : 0).ToString("0.000000") + join;
            }
            else
            if (data.name == "FPL Wpt id")
            {
                // Original bool value
                int originalValue = (int)specification.entityInfo.WaypointNumber;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0") + join;
            }
            else
            if (data.name == "FPL Wpt distance")
            {
                // Original bool value
                float originalValue = specification.entityInfo.WaypointDistance;//feet
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }
            else
            if (data.name == "FPL Wpt Bearing")
            {
                // Original bool value
                float originalValue = specification.entityInfo.WaypointBearing;
                // Convert float to byte array
                byte[] byteArray = BitConverter.GetBytes(originalValue);
                Array.Copy(byteArray, 0, result, i, byteArray.Length);
                strdata += originalValue.ToString("0.000000") + join;
            }



            //Allys
            if (k >= 54 && k <= 116)
            {
                loop = true;
                
                if (count < allys.Count && allys[count].gameObject.activeSelf)
                {
                    data = byteData[k];
                    // Original float value 
                    float originalValue = allys[count].entityInfo.Altitude;//feet
                    // Convert float to byte array
                    byte[] byteArray = BitConverter.GetBytes(originalValue);
                    Array.Copy(byteArray, 0, result, i, byteArray.Length);
                    strdata += originalValue.ToString("0.000000") + join;
                    i += data.size;
                    k++;//55

                    data = byteData[k];
                    // Original float value
                    float originalValue2 = allys[count].entityInfo.xVelocity;//fps
                    // Convert float to byte array
                    byte[] byteArray2 = BitConverter.GetBytes(originalValue2);
                    Array.Copy(byteArray2, 0, result, i, byteArray2.Length);
                    strdata += originalValue2.ToString("0.000000") + join;
                    i += data.size;
                    k++;//56
                     
                    data = byteData[k];
                    // Original float value
                    float originalValue3 = allys[count].entityInfo.yVelocity;//fps
                    // Convert float to byte array
                    byte[] byteArray3 = BitConverter.GetBytes(originalValue3);
                    Array.Copy(byteArray3, 0, result, i, byteArray3.Length);
                    strdata += originalValue3.ToString("0.000000") + join;
                    i += data.size;
                    k++;//57

                    data = byteData[k];
                    // Original float value
                    float originalValue4 = allys[count].entityInfo.zVelocity;//fps
                    // Convert float to byte array
                    byte[] byteArray4 = BitConverter.GetBytes(originalValue4);
                    Array.Copy(byteArray4, 0, result, i, byteArray4.Length);
                    strdata += originalValue4.ToString("0.000000") + join;
                    i += data.size;
                    k++;//58

                    data = byteData[k];
                    // Original float value
                    float originalValue5 = allys[count].entityInfo.Heading;//deg
                    // Convert float to byte array
                    byte[] byteArray5 = BitConverter.GetBytes(originalValue5);
                    Array.Copy(byteArray5, 0, result, i, byteArray5.Length);
                    strdata += originalValue5.ToString("0.000000") + join;
                    i += data.size;
                    k++;//59

                    data = byteData[k];
                    // Original float value
                    float originalValue6 = allys[count].entityInfo.Lat;
                    // Convert float to byte array
                    byte[] byteArray6 = BitConverter.GetBytes(originalValue6);
                    Array.Copy(byteArray6, 0, result, i, byteArray6.Length);
                    strdata += originalValue6.ToString("0.000000") + join;
                    i += data.size;
                    k++;//60

                    data = byteData[k];
                    // Original float value
                    float originalValue7 = allys[count].entityInfo.Long;
                    // Convert float to byte array
                    byte[] byteArray7 = BitConverter.GetBytes(originalValue7);
                    Array.Copy(byteArray7, 0, result, i, byteArray7.Length);
                    strdata += originalValue7.ToString("0.000000") + join;

                }
                count++;
            }
            else
            {
                if (loop) 
                {
                    loop = false;
                    count = 0;
                }
            }

            //waypoint
            if (k >= 141 && k <= 161)
            {
                Wloop = true;

                if (count < waypointsViewer.Waypoints.Count)
                {
                    Vector3 latlong = waypointsViewer.LatlongWaypoints[count];

                    data = byteData[k]; 
                    // Original float value
                    float originalValue1 = latlong.x;//Lat
                    // Convert float to byte array
                    byte[] byteArray1 = BitConverter.GetBytes(originalValue1);
                    Array.Copy(byteArray1, 0, result, i, byteArray1.Length);
                    strdata += originalValue1.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value 
                    float originalValue2 = latlong.y;//Long
                    // Convert float to byte array
                    byte[] byteArray2 = BitConverter.GetBytes(originalValue2);
                    Array.Copy(byteArray2, 0, result, i, byteArray2.Length);
                    strdata += originalValue2.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue6 = latlong.z;//Elevation metre
                    // Convert float to byte array
                    byte[] byteArray6 = BitConverter.GetBytes(originalValue6);
                    Array.Copy(byteArray6, 0, result, i, byteArray6.Length);
                    strdata += originalValue6.ToString("0.000000") + join;




                }
                count++;
            }
            else
            {
                if (Wloop)
                {
                    Wloop = false;
                    count = 0;
                }
            }
            
            /*
            //Sams
            if (k >= 117 && k <= 192)
            {
                sloop = true;

                if (count < sams.Count && sams[count].gameObject.activeSelf)
                {
                    data = byteData[k];
                    // Original float value
                    float originalValue1 = sams[count].entityInfo.Lat;
                    // Convert float to byte array
                    byte[] byteArray1 = BitConverter.GetBytes(originalValue1);
                    Array.Copy(byteArray1, 0, result, i, byteArray1.Length);
                    strdata += originalValue1.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value 
                    float originalValue2 = sams[count].entityInfo.Long;
                    // Convert float to byte array
                    byte[] byteArray2 = BitConverter.GetBytes(originalValue2);
                    Array.Copy(byteArray2, 0, result, i, byteArray2.Length);
                    strdata += originalValue2.ToString("0.000000") + join;
                    i += data.size;
                    k++;


                    data = byteData[k];
                    // Original float value
                    float originalValue3 = sams[count].entityInfo.Altitude;
                    // Convert float to byte array
                    byte[] byteArray3 = BitConverter.GetBytes(originalValue3);
                    Array.Copy(byteArray3, 0, result, i, byteArray3.Length);
                    strdata += originalValue3.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    //data = byteData[k];
                    //// Original float value
                    //float originalValue4 = sams[count].entityInfo.yVelocity;
                    //// Convert float to byte array
                    //byte[] byteArray4 = BitConverter.GetBytes(originalValue4);
                    //Array.Copy(byteArray4, 0, result, i, byteArray4.Length);
                    //strdata += originalValue4.ToString("0.000000") + join;
                    //i += data.size;
                    //k++;

                    //data = byteData[k];
                    //// Original float value
                    //float originalValue5 = sams[count].entityInfo.Speed;
                    //// Convert float to byte array
                    //byte[] byteArray5 = BitConverter.GetBytes(originalValue5);
                    //Array.Copy(byteArray5, 0, result, i, byteArray5.Length);
                    //strdata += originalValue5.ToString("0.000000") + join;
                    //i += data.size;
                    //k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue6 = sams[count].entityInfo.RadarRange;
                    // Convert float to byte array
                    byte[] byteArray6 = BitConverter.GetBytes(originalValue6);
                    Array.Copy(byteArray6, 0, result, i, byteArray6.Length);
                    strdata += originalValue6.ToString("0.000000") + join;




                }
                count++;
            }
            else
            {
                if (sloop)
                {
                    sloop = false;
                    count = 0;
                }
            }
            */
            
            //Enemys
            if (k >= 169 && k <= 192)
            {
                eloop = true;

                if (count < enemy.Count && enemy[count].gameObject.activeSelf)
                {
                    data = byteData[k];
                    // Original float value
                    float originalValue1 = enemy[count].entityInfo.Lat;
                    // Convert float to byte array
                    byte[] byteArray1 = BitConverter.GetBytes(originalValue1);
                    Array.Copy(byteArray1, 0, result, i, byteArray1.Length);
                    strdata += originalValue1.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value 
                    float originalValue2 = enemy[count].entityInfo.Long;
                    // Convert float to byte array
                    byte[] byteArray2 = BitConverter.GetBytes(originalValue2);
                    Array.Copy(byteArray2, 0, result, i, byteArray2.Length);
                    strdata += originalValue2.ToString("0.000000") + join;
                    i += data.size;
                    k++;


                    data = byteData[k];
                    // Original float value
                    float originalValue3 = enemy[count].entityInfo.Altitude;
                    // Convert float to byte array
                    byte[] byteArray3 = BitConverter.GetBytes(originalValue3);
                    Array.Copy(byteArray3, 0, result, i, byteArray3.Length);
                    strdata += originalValue3.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue4 = enemy[count].entityInfo.Speed;
                    // Convert float to byte array
                    byte[] byteArray4 = BitConverter.GetBytes(originalValue4);
                    Array.Copy(byteArray4, 0, result, i, byteArray4.Length);
                    strdata += originalValue4.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue5 = enemy[count].entityInfo.yVelocity;
                    // Convert float to byte array
                    byte[] byteArray5 = BitConverter.GetBytes(originalValue5);
                    Array.Copy(byteArray5, 0, result, i, byteArray5.Length);
                    strdata += originalValue5.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue6 = enemy[count].entityInfo.Heading;
                    // Convert float to byte array
                    byte[] byteArray6 = BitConverter.GetBytes(originalValue6);
                    Array.Copy(byteArray6, 0, result, i, byteArray6.Length);
                    strdata += originalValue6.ToString("0.000000") + join;


                    

                }
                count++;
            }
            else
            {
                if (eloop)
                {
                    eloop = false;
                    count = 0;
                }
            }
            

            //Ships
            if (k >= 292 && k <= 315)
            {
                eloop = true;

                if (count < ship.Count && ship[count].gameObject.activeSelf)
                {
                    data = byteData[k];
                    // Original float value
                    float originalValue1 = ship[count].entityInfo.Lat;
                    // Convert float to byte array
                    byte[] byteArray1 = BitConverter.GetBytes(originalValue1);
                    Array.Copy(byteArray1, 0, result, i, byteArray1.Length);
                    strdata += originalValue1.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value 
                    float originalValue2 = ship[count].entityInfo.Long;
                    // Convert float to byte array
                    byte[] byteArray2 = BitConverter.GetBytes(originalValue2);
                    Array.Copy(byteArray2, 0, result, i, byteArray2.Length);
                    strdata += originalValue2.ToString("0.000000") + join;
                    i += data.size;
                    k++;


                    data = byteData[k];
                    // Original float value
                    float originalValue3 = ship[count].entityInfo.Altitude;
                    // Convert float to byte array
                    byte[] byteArray3 = BitConverter.GetBytes(originalValue3);
                    Array.Copy(byteArray3, 0, result, i, byteArray3.Length);
                    strdata += originalValue3.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    var spd = ship[count].entityInfo.xVelocity*0.592535f;
                    spd = spd < 0 ? -spd : spd;
                    data = byteData[k];
                    // Original float value
                    float originalValue4 = spd;
                    // Convert float to byte array
                    byte[] byteArray4 = BitConverter.GetBytes(originalValue4);
                    Array.Copy(byteArray4, 0, result, i, byteArray4.Length);
                    strdata += originalValue4.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue5 = ship[count].entityInfo.yVelocity;
                    // Convert float to byte array
                    byte[] byteArray5 = BitConverter.GetBytes(originalValue5);
                    Array.Copy(byteArray5, 0, result, i, byteArray5.Length);
                    strdata += originalValue5.ToString("0.000000") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue6 = ship[count].entityInfo.Heading;
                    // Convert float to byte array
                    byte[] byteArray6 = BitConverter.GetBytes(originalValue6);
                    Array.Copy(byteArray6, 0, result, i, byteArray6.Length);
                    strdata += originalValue6.ToString("0.000000") + join;




                }
                count++;
            }
            else
            {
                if (eloop)
                {
                    eloop = false;
                    count = 0;
                }
            }
            /*
            //TARGET SET 1
            if (k >= 169 && k <= 192)
            {
                t1loop = true;

                if (count < radarScreen.radarlog.detect.Count)
                {
                    data = byteData[k];
                    Vector2 latlong = latLong.worldToLatLong(new Vector2(radarScreen.radarlog.Pos[count].x, radarScreen.radarlog.Pos[count].z));

                    // Original float value
                    float originalValue = latlong.x;//latitude
                    // Convert float to byte array
                    byte[] byteArray = BitConverter.GetBytes(originalValue);
                    Array.Copy(byteArray, 0, result, i, byteArray.Length);
                    strdata += originalValue.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue2 = latlong.y;//longitude
                    // Convert float to byte array
                    byte[] byteArray2 = BitConverter.GetBytes(originalValue2);
                    Array.Copy(byteArray2, 0, result, i, byteArray2.Length);
                    strdata += originalValue2.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue3 = radarScreen.radarlog.Info[count].y;//altitude
                    // Convert float to byte array
                    byte[] byteArray3 = BitConverter.GetBytes(originalValue3);
                    Array.Copy(byteArray3, 0, result, i, byteArray3.Length);
                    strdata += originalValue3.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue4 = 0;//climbrate
                    // Convert float to byte array
                    byte[] byteArray4 = BitConverter.GetBytes(originalValue4);
                    Array.Copy(byteArray4, 0, result, i, byteArray4.Length);
                    strdata += originalValue4.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue5 = radarScreen.radarlog.Info[count].z;//speed
                    // Convert float to byte array
                    byte[] byteArray5 = BitConverter.GetBytes(originalValue5);
                    Array.Copy(byteArray5, 0, result, i, byteArray5.Length);
                    strdata += originalValue5.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue6 = radarScreen.radarlog.Info[count].w;//bearing
                    // Convert float to byte array
                    byte[] byteArray6 = BitConverter.GetBytes(originalValue6);
                    Array.Copy(byteArray6, 0, result, i, byteArray6.Length);
                    strdata += originalValue6.ToString("0.00") + join;


                }
                count++;
            }
            else
            {
                if (t1loop)
                {
                    t1loop = false;
                    count = 0;
                }
            }

            //TARGET SET 1
            if (k >= 207 && k <= 230)
            {
                if (!t2loop)
                {
                    count = 4;
                }
                t2loop = true;

                if (count < radarScreen.radarlog.detect.Count)
                {
                    data = byteData[k];
                    Vector2 latlong = latLong.worldToLatLong(new Vector2(radarScreen.radarlog.Pos[count].x, radarScreen.radarlog.Pos[count].z));

                    // Original float value
                    float originalValue = latlong.x;//latitude
                    // Convert float to byte array
                    byte[] byteArray = BitConverter.GetBytes(originalValue);
                    Array.Copy(byteArray, 0, result, i, byteArray.Length);
                    strdata += originalValue.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue2 = latlong.y;//longitude
                    // Convert float to byte array
                    byte[] byteArray2 = BitConverter.GetBytes(originalValue2);
                    Array.Copy(byteArray2, 0, result, i, byteArray2.Length);
                    strdata += originalValue2.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue3 = radarScreen.radarlog.Info[count].y;//altitude
                    // Convert float to byte array
                    byte[] byteArray3 = BitConverter.GetBytes(originalValue3);
                    Array.Copy(byteArray3, 0, result, i, byteArray3.Length);
                    strdata += originalValue3.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue4 = 0;//climbrate
                    // Convert float to byte array
                    byte[] byteArray4 = BitConverter.GetBytes(originalValue4);
                    Array.Copy(byteArray4, 0, result, i, byteArray4.Length);
                    strdata += originalValue4.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue5 = radarScreen.radarlog.Info[count].z;//speed
                    // Convert float to byte array
                    byte[] byteArray5 = BitConverter.GetBytes(originalValue5);
                    Array.Copy(byteArray5, 0, result, i, byteArray5.Length);
                    strdata += originalValue5.ToString("0.00") + join;
                    i += data.size;
                    k++;

                    data = byteData[k];
                    // Original float value
                    float originalValue6 = radarScreen.radarlog.Info[count].w;//bearing
                    // Convert float to byte array
                    byte[] byteArray6 = BitConverter.GetBytes(originalValue6);
                    Array.Copy(byteArray6, 0, result, i, byteArray6.Length);
                    strdata += originalValue6.ToString("0.00") + join;


                }
                count++;
            }
            else
            {
                if (t2loop)
                {
                    t2loop = false;
                    count = 0;
                }
            }
            */

            if (strdata.Length == 0)
            {
                strdata += "0" + join;
            }
            else
            {
                strdata = strdata.Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0");
            }
            stringdata += strdata; 

            i += data.size;
        }

        //string s = "120.55 434 02";
        //byte[] dat = BitConverter.GetBytes((float)120.55);
        byte[] byteArra = Encoding.ASCII.GetBytes(stringdata);
        //string asciiString = Encoding.ASCII.GetString(dat);
        Debug.Log(stringdata);
        SendByteMessage(byteArra);
    }

    void OnDestroy()
    {
        // Signal the receive thread to stop
        isRunning = false;
        // Close the UDP client
        if (udpClient != null)
        {
            isbound = false;
            udpClient.Close();
        }
        // Wait for the receive thread to finish
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join();
        }
    }
    public void SendUpdatePort(string port)
    {
        float p = float.Parse(port);
        p = Mathf.Clamp(p, 1000, 10000);
        sendingPort = (int)p;
    }

    public void SendingUpdateIp(string ip)
    {
        sendingIp = ip;
    }

    public void RecUpdatePort(string port)
    {
        float p = float.Parse(port);
        p = Mathf.Clamp(p, 1000, 10000);
        recePort = (int)p;
    }

    void StartReceive()
    {
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        // Create a local endpoint to bind the UdpClient to
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, recePort);

        // Bind the UdpClient to the local endpoint
        udpClient.Client.Bind(localEndPoint);
        isbound = true;
        if (receiveThread == null)
        {
            // Start a new thread to listen for incoming UDP messages
            receiveThread = new System.Threading.Thread(new System.Threading.ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
    }

    public string Decode(byte[] byteArray,int start_byte,data_type type,bool value = true)
    {
        if (byteArray == null)
        {
            return "0";
        }


        if(type == data_type.Byte)
        {
            byte singleByte = byteArray[start_byte];
            string singleByteString = singleByte.ToString("X2");
            return singleByteString;
        }else
        if(type == data_type.Int)
        {
            int vale = BitConverter.ToInt32(byteArray, start_byte);
            return vale.ToString();
        }else
        if(type == data_type.uInt)
        {
            uint vale = BitConverter.ToUInt32(byteArray, start_byte);
            return vale.ToString();
        }
        else
        if (type == data_type.Float)
        {
            float vale = BitConverter.ToSingle(byteArray, start_byte);
            return vale.ToString();
        }
        else
        if (type == data_type.Double)
        {
            double vale = BitConverter.ToDouble(byteArray, start_byte);
            return vale.ToString();
        }
        else
        if (type == data_type.Decimal)
        {
            //decimal vale = BitConverter.decima(byteArray, start_byte);
            return "0";
        }
        else
        if (type == data_type.Bool)
        {
            bool vale = BitConverter.ToBoolean(byteArray, start_byte);
            return vale.ToString();
        }
        else
        if (type == data_type.Char)
        {
            char vale = BitConverter.ToChar(byteArray, start_byte);
            return vale.ToString();
        }
        return "0";
    } 

    void StopReceive()
    {
        isbound = false;
        udpClient.Client.Close();
    }

    void ReceiveData()
    {
        while (isRunning)
        {

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, recePort);

            if (isbound)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);

                    recByte = data;
                    RecUdpPacket = true;
                    // Assuming 'recByte' is your byte array
                    string message = Encoding.UTF8.GetString(recByte);
                    string packetData = BitConverter.ToString(data);

                    //Debug.Log("Received message from " + remoteEndPoint.Address + " : len " + data.Length);
                    Debug.Log("Received message from " + remoteEndPoint.Address + " : len " + message);
                }
                catch (SocketException e)
                {
                    Debug.LogError($"Socket error: {e}");
                                    }
                catch (Exception e)
                {
                    Debug.LogError($"Unexpected error: {e}");
                    
                }
            }
        }
    }

    void SendByteMessage(byte[] data)
    {
        IPAddress specificIPAddress = IPAddress.Parse(sendingIp);
        // Send the UDP message to the broadcast address on the specified port
        udpClient.Send(data, data.Length, new IPEndPoint(specificIPAddress, sendingPort));
    }
     
    IEnumerator WaitForUdp()
    {
        while (!RecUdpPacket)
        {
            yield return null;
        }
        RecUdpPacket = false;

        if (recByte != null)
        {
            if (LogUpdate.isOn)
            {
                string message = Encoding.UTF8.GetString(recByte);
                string packetData = BitConverter.ToString(recByte);
                LogText.text = count.ToString("0000000") +" : " +recByte.Length +"\n" + packetData;
                count++;
            }

            //Debug.Log(recByte.Length);
            // Process the received message (you can modify this part based on your application)
            if (recByte.Length == 180)
            {
                Debug.Log("Sending message : len " + recByte.Length);
            }

            if (recByte.Length == 108)
            {
                Debug.Log("Received message from : len " + recByte.Length);
                //Decode(recByte);
            }
        }
        StartCoroutine(WaitForUdp());
    }


}
