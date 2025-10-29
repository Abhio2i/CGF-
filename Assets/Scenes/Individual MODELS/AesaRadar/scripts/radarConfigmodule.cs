using AirPlane.Radar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class loadScript
{
    [SerializeField]
    public float power = 2;
    [SerializeField]
    public float gain = 1;
    [SerializeField]
    public float frequency = 8;
    [SerializeField]
    public float minimalNoise = 1;
    [SerializeField]
    public float azimuth = 30;
    [SerializeField]
    public float bar = 30;
    [SerializeField]
    public float rcs = 2;
}

public class radarConfigmodule : MonoBehaviour
{
    public GameObject menu;
    public radarmodeltest modelTest;
    public loadScript loadscript;
    public UttamRadar AesaRadar;
    public RadarScreenUpdate_new radarScreen;
    public TextMeshProUGUI trmPower;
    public TextMeshProUGUI gain;
    public TextMeshProUGUI aperture;
    public TextMeshProUGUI freq;
    public TextMeshProUGUI minNoise;
    public string log;
    public TMP_InputField script;

    [Header("LOGCAT UI")]
    public TextMeshProUGUI logtext;
    public GameObject logpanel;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI minimalNoiseText;
    public TextMeshProUGUI gainText;
    public TextMeshProUGUI mainModeText;
    public TextMeshProUGUI subModeText;
    public TextMeshProUGUI azimuthText;
    public TextMeshProUGUI barText;
    public TextMeshProUGUI frequencyText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI rcsText;
    public TextMeshProUGUI citText;
    public TextMeshProUGUI jemText;
    public TextMeshProUGUI ptOnlyText;
    public Transform DetectedParent;
    public GameObject prefab;
    public List<Transform> detectedObjects = new List<Transform>();

    [Header("Mode UI")]
    public List<GameObject> ATAObject = new List<GameObject>();
    public List<GameObject> ATGObject = new List<GameObject>();
    public List<GameObject> ATSObject = new List<GameObject>();

    public GameObject jammer;
    public GameObject move;
    public GameObject iff;
    public GameObject altiude;
    public GameObject jamfreq;
    public TMP_Dropdown ATADropdown;
    public TMP_Dropdown ATGDropdown;
    public TMP_Dropdown ATSDropdown;
    public GameObject ATA;
    public GameObject ATG;
    public GameObject ATS;
    public GameObject Map;
    public Transform Plane;
    public Vector3 ATAPos;
    public Vector3 ATGPos;
    public Vector3 ATSPos;
    private void Start()
    {

        //script.text = JsonUtility.ToJson(loadscript);
        loadscript =   JsonUtility.FromJson<loadScript>(script.text);
        //if(AesaRadar != null) 
        //{
        //    AesaRadar.TRMPower = PlayerPrefs.GetFloat("trmpower");
        //    AesaRadar.G = PlayerPrefs.GetFloat("gain");
        //    AesaRadar.r = PlayerPrefs.GetFloat("aperture");
        //    AesaRadar.f = PlayerPrefs.GetFloat("freq");
        //    AesaRadar.NR = PlayerPrefs.GetFloat("minnoise");
        //}
        //else
        //{
        //    PlayerPrefs.SetFloat("trmpower", 2f);
        //    PlayerPrefs.SetFloat("gain", 1f);
        //    PlayerPrefs.SetFloat("aperture", 1f);
        //    PlayerPrefs.SetFloat("freq", 8f);
        //    PlayerPrefs.SetFloat("minnoise", 1f);
        //}
        //if(logtext != null)
        //{
        //    StartCoroutine(logAdd());
        //}
        for (int i = 0; i < 20; i++)
        {
            var obj = Instantiate(prefab);
            obj.transform.parent = DetectedParent;
            obj.transform.localPosition = new Vector3(0, i * -60f, 0);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
            detectedObjects.Add(obj.transform);
        }
    }

    //private void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.Escape))
    //    //{
    //    //    menu.SetActive(!menu.activeSelf);
    //    //}
    //}

    public void SelectMode(int i)
    {
        ATA.SetActive(false);
        ATG.SetActive(false);
        ATS.SetActive(false);
        ATADropdown.transform.parent.gameObject.SetActive(false);
        ATGDropdown.transform.parent.gameObject.SetActive(false);
        ATSDropdown.transform.parent.gameObject.SetActive(false);
        modelTest.Aircrafts.Clear();
        modelTest.ClearObjects();

        jammer.SetActive(false);
        move.SetActive(false);
        iff.SetActive(false);
        altiude.SetActive(false);
        jamfreq.SetActive(false);
        if (i == 0)
        {
            //ATA
            radarScreen.MainMode = RadarScreenUpdate_new.Mode1.ATS;
            radarScreen.MainModeSelection();
            Map.SetActive(false);
            Plane.position = ATAPos;
            ATA.SetActive(true);
            foreach (GameObject go in ATAObject)
            {
                modelTest.Aircrafts.Add(go);
            }
            modelTest.aircraftDropdown = ATADropdown;
            ATADropdown.transform.parent.gameObject.SetActive(true);
            jammer.SetActive(true);
            iff.SetActive(true);
            altiude.SetActive(true);
        }
        else
        if (i == 1)
        {
            //ATG
            radarScreen.MainMode = RadarScreenUpdate_new.Mode1.ATA;
            radarScreen.MainModeSelection();
            Map.SetActive(true);
            Plane.position = ATGPos;
            ATG.SetActive(true);
            foreach (GameObject go in ATGObject)
            {
                modelTest.Aircrafts.Add(go);
            }
            modelTest.aircraftDropdown = ATGDropdown;
            ATGDropdown.transform.parent.gameObject.SetActive(true);
            move.SetActive(true);
        }
        else
        if(i == 2)
        {
            //ATS
            radarScreen.MainMode = RadarScreenUpdate_new.Mode1.ATG;
            radarScreen.MainModeSelection();
            Map.SetActive(true);
            Plane.position = ATSPos;
            ATS.SetActive(true);
            foreach (GameObject go in ATSObject)
            {
                modelTest.Aircrafts.Add(go);
            }
            modelTest.aircraftDropdown = ATSDropdown;
            ATSDropdown.transform.parent.gameObject.SetActive(true);
        }
    }


    public void submit()
    {
        if (AesaRadar != null)
        {
            loadscript = JsonUtility.FromJson<loadScript>(script.text);
            loadscript.azimuth = acct(loadscript.azimuth);
            loadscript.bar = acct(loadscript.bar);
            AesaRadar.TRMPower = loadscript.power*1.37f;
            
            radarScreen.gain = loadscript.gain - 2;
            radarScreen.GainSet(0);
            radarScreen.band = (RadarScreenUpdate_new.Bandwidth)((int)loadscript.frequency-7);
            radarScreen.BandwidthSet(0);
            AesaRadar.f = loadscript.frequency;
            //Debug.LogError("");
            AesaRadar.Azimuth = loadscript.azimuth*10f;
            AesaRadar.Bar = loadscript.bar*10f;
            AesaRadar.MinimumDetectSize = loadscript.rcs;
            AesaRadar.NR = loadscript.minimalNoise;
        }
    }

    public int acct(float i)
    {
        i = i > 6 ? i / 10 : i;
        i = i>6 ? 6: (i<1?1:i);
        return (int)i;
    }

    public void trmp(float i)
    {
        trmPower.text = i.ToString("0.00");
        PlayerPrefs.SetFloat("trmpower", i);
        loadscript.power = i;
    }
    public void Gain(float i)
    {
        gain.text = i.ToString("0.00");
        PlayerPrefs.SetFloat("gain", i);
        loadscript.gain = i;
    }
    public void Aperture(float i)
    {
        aperture.text = i.ToString("0.00");
        PlayerPrefs.SetFloat("aperture", i);
    }
    public void Frequency(float i)
    {
        freq.text = i.ToString("0");
        PlayerPrefs.SetFloat("freq", i);
        loadscript.frequency = i;
    }
    public void minnoise(float i)
    {
        minNoise.text = i.ToString("0.00");
        PlayerPrefs.SetFloat("minnoise", i);
        loadscript.minimalNoise = i;
    }

    public void azimuth(float i)
    {
        
        loadscript.azimuth = i;
    }

    public void bar(float i)
    {
        
        loadscript.bar = i;
    }

    public void logCapture()
    {
        Time.timeScale = 0;
        string log = radarScreen.getLog();
        RadarLog radarLog = JsonUtility.FromJson<RadarLog>(log);
        powerText.text = AesaRadar.Pt.ToString("0.0")+"w";
        minimalNoiseText.text = AesaRadar.NR.ToString()+"db";
        gainText.text = AesaRadar.G.ToString()+"db";
        mainModeText.text = radarLog.MainMode.ToString();
        subModeText.text = radarLog.SubMode.ToString();
        azimuthText.text = radarLog.Azimuth.ToString()+"deg";
        barText.text = radarLog.Bar.ToString() + "deg";
        frequencyText.text = radarLog.frequency.ToString() + "ghz";
        rangeText.text = radarLog.Range.ToString() + "km";
        rcsText.text = radarLog.MinimumDetectSize.ToString() + "m2";

        citText.text = radarLog.Cit.ToString();
        jemText.text = radarLog.Jem.ToString();
        ptOnlyText.text = radarLog.PtOnly.ToString();

        
        foreach(var item in detectedObjects)
        {
            item.gameObject.SetActive(false);
        }
        var i = 0;
        foreach(var item in AesaRadar.DetectedObjects)
        {
            var spec = item.GetComponent<Specification>();
            var obj = detectedObjects[i];
            obj.gameObject.SetActive(true);
            var no = obj.GetChild(1).GetComponent<TextMeshProUGUI>();
            no.text = i.ToString()+".";
            var name = obj.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            name.text = "Unknown";
            if (radarLog.Jem)
            {
                string names = "Unknown";
                string t = item.tag.ToLower();
                
                if (t.Contains("player")||t.Contains("plane"))
                {
                    //names = "Plane";
                    names = item.name.Split("_")[0].ToString();
                }
                else
                if (t.Contains("base"))
                {
                    names = "Base";
                }else
                if (t.Contains("tank"))
                {
                    names = "Ground Vehicle";
                }
                else
                if (t.Contains("ship"))
                {
                    names = "Ship";
                }
                
                name.text = names;
                    //name.text = item.name.Split("_")[0].ToString();
            }
            var iff = obj.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
            iff.text = "";
            if (radarLog.Cit&&spec!=null)
            {
                iff.text = spec.iff.ToString();
            }
            var pirorty = obj.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
            pirorty.text = "Casual";
            if (AesaRadar.LockTargetList.Contains(item))
            {
                pirorty.text = "HIGH";
            }
            var type = obj.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
            type.text = "ENEMY";
            if ( radarLog.Cit && spec != null && spec.iff == AesaRadar.iff)
            {
                type.text = "ALLY";
            }
            var loc = obj.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>();
            loc.text = "False";
            if (AesaRadar.HardLockTargetObject == item)
            {
                loc.text = "True";
            }
            var engine = obj.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>();
            engine.transform.parent.gameObject.SetActive(false);
            engine.text = "";
            var burner = obj.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>();
            burner.transform.parent.gameObject.SetActive(false);
            burner.text = "";
            if (radarLog.Jem && spec != null)
            {
                engine.transform.parent.gameObject.SetActive(true);
                burner.transform.parent.gameObject.SetActive(true);
                engine.text = spec.engine;
                burner.text = spec.exhaust;
            }
            var distance = obj.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>();
            distance.text = (Vector3.Distance(AesaRadar.transform.root.position, item.transform.position) / 1000).ToString("0.0") + "km"; 

            var size = obj.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>();
            size.text = "null";
            if(spec != null)
            {
                size.text = spec.size.ToString()+"m2";
            }

            i++;
        }

        logtext.text = log;
    }

    public void contiune()
    {
        Time.timeScale = 1;
    }

    IEnumerator  logAdd()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(logAdd());
        
        foreach(var o in AesaRadar.DetectedObjects)
        {
            var n = (o.tag.ToLower().Contains("plane")||o.tag.ToLower().Contains("player")|| o.tag.ToLower().Contains("ally")) ? "AirCraft" : "object";
            n += "  Mode: " + AesaRadar.SubMode.ToString();
            n += " " + (AesaRadar.Cit ? (o.name.ToLower().Contains("ally")?"Ally":"Enemy") : "unknown");
            n += "  Distance: " + (Vector3.Distance(AesaRadar.transform.root.position,o.transform.position)).ToString("0.00")+"meter";
            //float speed = 0;
            //if (o.GetComponent<Rigidbody>() != null)
            //{
            //    speed = o.GetComponent<Rigidbody>().velocity.magnitude;
            //}
            //n += "  Speed: " + speed.ToString("0.00") + "m/s";
            n += "  Angle: " + Vector3.Angle(o.transform.position - AesaRadar.transform.position, AesaRadar.transform.forward).ToString("0.0") + "°deg";
            n += "  locked: " + (AesaRadar.HardLockTargetObject == o ? "true" : "false");
            n += "  Cit: " + AesaRadar.Cit.ToString();
            n += "  JEM: " + AesaRadar.JEM.ToString();
            n += "  Name: " + (AesaRadar.JEM?(o.name.ToLower().Contains("tejas")?"Tejas":"F16"):"unknown");
            log += DateTime.Now +" : "+n+ "\n";
        }
        logtext.text = log;
    }

    public void logpanelvision(bool i)
    {
        logpanel.SetActive(i);
    }
}
    