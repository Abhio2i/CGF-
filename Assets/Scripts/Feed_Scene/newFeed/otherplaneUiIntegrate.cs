using Assets.Scripts.Feed;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class otherplaneUiIntegrate : MonoBehaviour
{

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
    public List<GameObject> missile;
    public List<GameObject> bombs;
    [Header("RadarUI")]
    public Transform LeftLine;
    public Transform RightLine;
    public TextMeshProUGUI AzimuthText;
    public TextMeshProUGUI BarText;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI RA;
    public List<TextMeshProUGUI> DistUnit;
    public Toggle CIT;
    public Toggle Ptonly;
    public Toggle Jem;
    public List<GameObject> EnemyPrefabs;
    public GameObject EnemyPrefab, Forground;

    [Header("RWRUI")]
    public List<GameObject> EnemyPrefabs2;
    public GameObject IconPrefab;
    public RectTransform RWRBG;
    void Start()
    {
        for (var i = 0; i < 50; i++)
        {
            var obj = Instantiate(EnemyPrefab, Forground.transform);
            obj.transform.SetParent(Forground.transform);
            obj.SetActive(false);
            EnemyPrefabs.Add(obj.gameObject);

        }
        for (var i = 0; i < 50; i++)
        {
            var obj = Instantiate(IconPrefab, RWRBG.transform);
            obj.transform.SetParent(RWRBG.transform);
            obj.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
            obj.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            obj.SetActive(false);
            EnemyPrefabs2.Add(obj.gameObject);

        }
    }


    void Update()
    {
        if (events == null && player != null && player.GetComponent<SetEvents>())
        {
            events = player.GetComponent<SetEvents>();
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
        if (events != null && s < events.events.Count)
        {
            var data = events.events[s].Split("|**|");
            if (data.Length < 2) return;
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
                distance.text = stri.Range.ToString();
                Forground.transform.localPosition = stri.Display;
                var i = 0;
                for (var ict = 0; ict < EnemyPrefabs.Count; ict++)
                {
                    var ob = EnemyPrefabs[ict];
                    if (i < stri.Info.Count)
                    {
                        ob.SetActive(true);
                        ob.transform.localPosition = stri.Info[i];
                        //var st2 = stri.Info[i].Split(",");
                        for (var b = 0; b < ob.transform.childCount; b++)
                        {
                           // ob.transform.GetChild(b).gameObject.SetActive((st2[b] == "1") ? true : false);
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
            if (stri != null)
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
