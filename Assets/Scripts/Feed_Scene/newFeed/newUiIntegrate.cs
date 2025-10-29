using AirPlane.Radar;
using Assets.Scripts.Feed;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class newUiIntegrate : MonoBehaviour
{

    public newUiIntegrate setupUI;
    public Emulator emulator;
    public SetEvents events;
    public Transform player;

    [Header("GlobeUI")]
    public RectTransform globex;
    public RectTransform globey;
    [Header("FuelGuage")]
    public Slider leftTk;
    public Slider centerTk;
    public Slider rightTk;
    public TextMeshProUGUI remianText;
    [Header("Weapons")]
    public List<GameObject> missile = new List<GameObject>();
    public List<GameObject> bombs = new List<GameObject>();
    [Header("RadarUI")]
    public Transform LeftLine;
    public Transform RightLine;
    public TextMeshProUGUI AzimuthText;
    public TextMeshProUGUI BarText;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI RA;
    public TextMeshProUGUI Maneuver;
    public List<TextMeshProUGUI> DistUnit = new List<TextMeshProUGUI>();
    public Toggle CIT;
    public Toggle Ptonly;
    public Toggle Jem;
    public Toggle Aj;
    public GameObject noise;
    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    public GameObject EnemyPrefab;
    public RectTransform Forground;

    [Header("RWRUI")]
    public List<GameObject> EnemyPrefabs2 = new List<GameObject>() ;
    public GameObject IconPrefab;
    public RectTransform RWRBG;
    public RectTransform EWDisplay;
    void Start()
    {
        if (Forground.childCount < 1)
        {
            for (var i = 0; i < 50; i++)
            {
                var obj = Instantiate(EnemyPrefab, Forground.transform);
                obj.transform.SetParent(Forground.transform);
                obj.SetActive(false);
                EnemyPrefabs.Add(obj.gameObject);

            }
        }
        else
        {
            for (var i = 0; i < Forground.childCount; i++)
            {
                EnemyPrefabs.Add(Forground.GetChild(i).gameObject);
            }
        }

        if (RWRBG.childCount < 1)
        {
            for (var i = 0; i < 50; i++)
            {
                var obj = Instantiate(IconPrefab, EWDisplay.transform);
                obj.transform.SetParent(EWDisplay.transform);
                obj.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
                obj.GetComponent<RectTransform>().transform.localScale = Vector3.one;
                obj.SetActive(false);
                EnemyPrefabs2.Add(obj.gameObject);

            }
        }
        else
        {
            for (var i = 0; i < Forground.childCount; i++)
            {
                EnemyPrefabs2.Add(RWRBG.GetChild(i).gameObject);
            }
        }
    }

    public void setup()
    {
        emulator = setupUI.emulator;
        player = transform;

        //GlobeUI
        globex = setupUI.globex;
        globey = setupUI.globey;

        //FuelGuage
        leftTk = setupUI.leftTk;
        centerTk = setupUI.centerTk;
        rightTk = setupUI.rightTk;
        remianText = setupUI.remianText;

        //Weapons
        foreach(var i in setupUI.missile)
        {
            missile.Add(i);
        }
        foreach (var i in setupUI.bombs)
        {
            bombs.Add(i);
        }

        //RadarUI
        LeftLine = setupUI.LeftLine;
        RightLine = setupUI.RightLine;
        AzimuthText = setupUI.AzimuthText;
        BarText = setupUI.BarText;
        distance = setupUI.distance;
        Maneuver = setupUI.Maneuver;
        RA = setupUI.RA;
        foreach (var i in setupUI.DistUnit)
        {
            DistUnit.Add(i);
        }
        CIT = setupUI.CIT;
        Ptonly = setupUI.Ptonly;
        Jem = setupUI.Jem;
        Aj = setupUI.Aj;
        noise = setupUI.noise;
        EnemyPrefab = setupUI.EnemyPrefab;
        Forground = setupUI.Forground;

        //RWRUI
        IconPrefab = setupUI.IconPrefab;
        RWRBG = setupUI.RWRBG;
        EWDisplay = setupUI.EWDisplay;
    }

    void Update()
    {
        if(events==null&& player != null && player.GetComponent<SetEvents>())
        {
            events=player.GetComponent<SetEvents>();
        }
    }
    private void FixedUpdate()
    {
        if (events == null) return;
       if (!events.IAmactive) return;

        float pitchAngle = UnityEngine.Vector3.Dot(transform.forward, UnityEngine.Vector3.up) * Mathf.Rad2Deg;
        float bankAngle = UnityEngine.Vector3.Dot(transform.right, UnityEngine.Vector3.up) * Mathf.Rad2Deg;
        Quaternion bankRotation = Quaternion.Euler(0f, 0f, -bankAngle);

        UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(0f, pitchAngle, 0f);
        if (globex)
        {
            globex.transform.localRotation = bankRotation;
        }
        if (globey)
        {
            globey.transform.rotation = Quaternion.Euler(-pitchAngle, 0, 0);
        }
        RA.text = transform.position.y.ToString("0") + "m";
        var s = PlayerPrefs.GetInt("CurrentFrame");
        if (events != null&&s<events.events.Count)
        {
            
            var split = events.events[s].Split("|*^*|");
            //MainPLane
            var data = split[0].Split("|**|");
            if (data.Length >= 2)
            {
                var stri = JsonUtility.FromJson<RadarLog>(data[0]);
                if (stri != null)
                {
                    var v = 0;
                    var part = (stri.RangeScale / 4) + (stri.RangeScale / 22);

                    foreach (var obj in DistUnit)
                    {
                        v++;
                        obj.text = (v * part).ToString("0") + "D";
                    }
                    //Forground.transform.localPosition = MainMode != Mode1.ATA ? forsave : Vector3.zero;
                    //Distance.text = uttamRadar.Range.ToString() + "D";
                    AzimuthText.text = (stri.Azimuth / 10).ToString("0") + "\r\nA";
                    BarText.text = (stri.Bar / 10).ToString("0") + "\r\nB";
                    RightLine.rotation = Quaternion.Euler(0f, 0f, -stri.Azimuth);
                    LeftLine.rotation = Quaternion.Euler(0f, 0f, stri.Azimuth);

                    CIT.isOn = stri.Cit;
                    Jem.isOn = stri.Jem;
                    Ptonly.isOn = stri.PtOnly;
                    Aj.isOn = stri.Aj;
                    noise.SetActive(stri.jemNoise);
                    noise.transform.rotation = Quaternion.Euler(0, 0, stri.jemAngle);
                    distance.text = stri.Range.ToString();
                    Forground.transform.localPosition = stri.Display;
                    Maneuver.text = "";
                    var i = 0;
                    for (var ict = 0; ict < EnemyPrefabs.Count; ict++)
                    {
                        var ob = EnemyPrefabs[ict];
                        if (i < stri.detect.Count)
                        {
                            ob.SetActive(true);
                            ob.transform.localPosition = stri.detect[i];
                            //var st2 = stri.icon[i].Split(",");
                            for (var b = 0; b < ob.transform.childCount; b++)
                            {
                                //ob.transform.GetChild(b).gameObject.SetActive((st2[b] == "1") ? true : false);
                            }
                        }
                        else
                        {
                            ob.SetActive(false);
                            for (var b = 0; b < ob.transform.childCount; b++)
                            {
                                ob.transform.GetChild(b).gameObject.SetActive(false);
                            }
                        }
                        i++;
                    }
                }
                var stri2 = JsonUtility.FromJson<RWRLog>(data[1]);
                if (stri2 != null)
                {
                    RWRBG.rotation = Quaternion.Euler(0, 0, stri2.rot);
                    var i = 0;
                    for (var ict = 0; ict < EnemyPrefabs2.Count; ict++)
                    {
                        var ob = EnemyPrefabs2[ict];
                        if (i < stri2.detect.Count)
                        {
                            ob.SetActive(true);
                            ob.transform.localPosition = stri2.detect[i];
                            var st2 = stri2.icon[i].Split(",");
                            for (var b = 0; b < ob.transform.childCount; b++)
                            {
                                ob.transform.GetChild(b).gameObject.SetActive((st2[b] == "1") ? true : false);
                            }
                        }
                        else
                        {
                            ob.SetActive(false);
                            for (var b = 0; b < ob.transform.childCount; b++)
                            {
                                ob.transform.GetChild(b).gameObject.SetActive(false);
                            }
                        }
                        i++;
                    }
                }
                var stri3 = JsonUtility.FromJson<ExtraPlaneLog>(data[2]);
                if (stri3 != null)
                {
                    remianText.text = stri3.remainFuel;
                    leftTk.value = stri3.leftTank;
                    rightTk.value = stri3.rightTank;
                    centerTk.value = stri3.centerTank;
                    var j = 0;
                    foreach (var i in stri3.missile)
                    {
                        missile[j].SetActive(i);
                        j++;
                    }
                    j = 0;
                    foreach (var i in stri3.bombs)
                    {
                        bombs[j].SetActive(i);
                        j++;
                    }


                }
            }
            ///AIPLanes
            if (split.Length > 1)
            {
                data = split[1].Split("|**|");
                if (data.Length >= 2)
                {
                    var stri = JsonUtility.FromJson<AIRadarLog>(data[0]);
                    if (stri != null)
                    {
                        var RangeScale = 66f;
                        var v = 0;
                        var part = (RangeScale / 4) + (RangeScale / 22);

                        foreach (var obj in DistUnit)
                        {
                            v++;
                            obj.text = (v * part).ToString("0") + "D";
                        }
                        //Forground.transform.localPosition = MainMode != Mode1.ATA ? forsave : Vector3.zero;
                        //Distance.text = uttamRadar.Range.ToString() + "D";
                        AzimuthText.text = (60 / 10).ToString("0") + "\r\nA";
                        BarText.text = (60 / 10).ToString("0") + "\r\nB";
                        RightLine.rotation = Quaternion.Euler(0f, 0f, -60);
                        LeftLine.rotation = Quaternion.Euler(0f, 0f, 60);
                       
                        CIT.isOn = true;
                        Jem.isOn = false;
                        Ptonly.isOn = true;
                        distance.text = "150";
                        noise.SetActive(false);
                        Maneuver.text = stri.Maneuvers.ToString();
                       var i = 0;
                        for (var ict = 0; ict < EnemyPrefabs.Count; ict++)
                        {
                            var ob = EnemyPrefabs[ict];
                            if (i < stri.detect.Count)
                            {
                                ob.SetActive(true);
                                var w = (Mathf.Tan(60 * Mathf.Deg2Rad) * RangeScale * 1000) * 2f;
                                Forground.transform.localPosition = new Vector3(0, -116.5f, 0);
                                var pos = transform.InverseTransformPoint(stri.detect[i]);
                                
                                var x = -(pos.x / (w / 2f)) * (Forground.rect.width / 2f);
                                var y = ((pos.z / (RangeScale * 1000)) * (Forground.rect.height / 2f)) - 30f;
                                var z = 0f;
                                y = y > (Forground.rect.height / 2f) ? (Forground.rect.height / 2f) : y;
                                ob.transform.localPosition = new Vector3(x,y,z);
                                ob.transform.GetChild(1).gameObject.SetActive(true);
                                ob.transform.GetChild(4).gameObject.SetActive(!stri.locks[i]);
                                ob.transform.GetChild(5).gameObject.SetActive(stri.locks[i]);
                            }
                            else
                            {
                                ob.SetActive(false);
                                for (var b = 0; b < ob.transform.childCount; b++)
                                {
                                    ob.transform.GetChild(b).gameObject.SetActive(false);
                                }
                            }
                            i++;
                        }
                    }
                   
                    var stri2 = JsonUtility.FromJson<AIEWLog>(data[1]);
                    if (stri2 != null)
                    {
                        RWRBG.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.y);
                        var i = 0;
                        for (var ict = 0; ict < EnemyPrefabs2.Count; ict++)
                        {
                            var ob = EnemyPrefabs2[ict];
                            if (i < stri2.detect.Count)
                            {
                                ob.SetActive(true);
                                var pos = transform.InverseTransformPoint(stri2.detect[i]);
                                var range = 20000;
                                var Distance = ((EWDisplay.rect.width / 2f) / (range)) * 0.75f;
                                ob.transform.localPosition = new Vector3(pos.x * Distance, pos.z * Distance, 0); ;
                                ob.transform.GetChild(1).gameObject.SetActive(true);
                                ob.transform.GetChild(2).gameObject.SetActive(true);
                                ob.transform.GetChild(4).gameObject.SetActive(true);
                                ob.transform.GetChild(5).gameObject.SetActive(true);
                            }
                            else
                            {
                                ob.SetActive(false);
                                for (var b = 0; b < ob.transform.childCount; b++)
                                {
                                    ob.transform.GetChild(b).gameObject.SetActive(false);
                                }
                            }
                            i++;
                        }
                    }
                    return;
                    var stri3 = JsonUtility.FromJson<ExtraPlaneLog>(data[2]);
                    if (stri3 != null)
                    {
                        remianText.text = stri3.remainFuel;
                        leftTk.value = stri3.leftTank;
                        rightTk.value = stri3.rightTank;
                        centerTk.value = stri3.centerTank;
                        var j = 0;
                        foreach (var i in stri3.missile)
                        {
                            missile[j].SetActive(i);
                            j++;
                        }
                        j = 0;
                        foreach (var i in stri3.bombs)
                        {
                            bombs[j].SetActive(i);
                            j++;
                        }


                    }
                }
            }

        }
    }

    public static bool TryParse(string vectorString, out Vector3 result)
    {
        result = Vector3.zero;

        // Remove any parentheses or whitespace from the string
        vectorString = vectorString.Trim('(', ')', '[', ']', '{', '}').Trim();

        // Split the string into three parts
        string[] parts = vectorString.Split(',');
        if (parts.Length != 3)
        {
            return false;
        }

        // Parse each part and store the result in a Vector3
        float x, y, z;
        if (!float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x)
            || !float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y)
            || !float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out z))
        {
            return false;
        }

        result = new Vector3(x, y, z);
        return true;
    }
}
