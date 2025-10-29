using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class EWloadScript
{
    [SerializeField]
    public float power = 2;
    [SerializeField]
    public float gain = 1;
    [SerializeField]
    public float recieveGain = 5;
    [SerializeField]
    public float threshold = -118.9f;
    [SerializeField]
    public bool autoChaff = false;
    [SerializeField]
    public float chaffActive = 3;
    [SerializeField]
    public bool autoFlare = false;
    [SerializeField]
    public float flareActive = 3;
    [SerializeField]
    public bool dircm = false;
    [SerializeField]
    public float dircmActive = 3;
    [SerializeField]
    public float deflectionTime = 3;
    [SerializeField]
    public float jammerFreq = 5;
}


public class EWConfig : MonoBehaviour
{
    public EWRadar ewRadar;
    public RWR rwr;
    public Selector selector;
    public EWequation equation;
    public EWloadScript loadscript;
    public TextMeshProUGUI ewRange, flareRange, chaffRange, laserRange, deflectTime;
    public GameObject[] dircm;
    public GameObject flare, chaff;
    public GameObject ewSphere;
    public TMP_InputField script;
    public TextMeshProUGUI Distance;
    Vector3 pos = Vector3.zero;
    Quaternion  rot = Quaternion.identity;

    [Header("Log Cat UI")]
    public Transform DetectedParent;
    public GameObject prefab;
    public TextMeshProUGUI logtext;
    public GameObject logpanel;

    public TextMeshProUGUI powerText;
    public TextMeshProUGUI RecieveGainText;
    public TextMeshProUGUI gainText;
    public TextMeshProUGUI ThresholdText;
    public TextMeshProUGUI AutoChaffText;
    public TextMeshProUGUI AutoFlareText;
    public TextMeshProUGUI DircmText;
    public TextMeshProUGUI ChaffActiveRangeText;
    public TextMeshProUGUI FlareActiveRangeText;
    public TextMeshProUGUI DircmActiveRangeText;
    public TextMeshProUGUI deflectionText;
    private List<Transform> detectedObjects = new List<Transform>();
    private void Start()
    {
        pos = selector.mainPlane.position;
        rot = selector.mainPlane.rotation;
        loadscript = JsonUtility.FromJson<EWloadScript>(script.text);
        //script.text = JsonUtility.ToJson(loadscript);

        PlayerPrefs.SetFloat("ewRange",30);
         PlayerPrefs.SetInt("autoChaff",0);
         PlayerPrefs.SetInt("autoFlare",0);
         PlayerPrefs.SetFloat("chaffRange",0.7f);
         PlayerPrefs.SetFloat("flareRange",0.7f);
         PlayerPrefs.SetInt("Dircm",0);
         PlayerPrefs.SetFloat("laserRange",1);
         PlayerPrefs.SetFloat("deflectTime",1);
        ewSphere.transform.localScale = new Vector3(ewRadar.radar_Range*2,1, ewRadar.radar_Range * 2);

        for(int i = 0; i < 20; i++)
        {
            var obj = Instantiate(prefab);
            obj.transform.parent = DetectedParent;
            obj.transform.localPosition = new Vector3(0,i*-60f,0);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
            detectedObjects.Add(obj.transform);
        }
    }

    public void logCapture()
    {
        Time.timeScale = 0;
        //string log = radarScreen.getLog();
        //RadarLog radarLog = JsonUtility.FromJson<RadarLog>(log);
        powerText.text = loadscript.power.ToString("")+"w";
        RecieveGainText.text = loadscript.recieveGain.ToString("") + "db";
        gainText.text = loadscript.gain.ToString("") + "db";
        ThresholdText.text = loadscript.threshold.ToString("") + "db";
        AutoChaffText.text = ewRadar.autoChaffFire.ToString();
        AutoFlareText.text = ewRadar.autoFlareFire.ToString();
        DircmText.text = ewRadar.DIRCM_Module.active.ToString();
        ChaffActiveRangeText.text = ewRadar.chaffActivateRange.ToString("")+"m";
        FlareActiveRangeText.text = ewRadar.flareActivateRange.ToString("") + "m";
        DircmActiveRangeText.text = ewRadar.DIRCM_Module.effectRange.ToString("") + "m";
        deflectionText.text = ewRadar.DIRCM_Module.idleDeflectionTime.ToString("") + "s";

    
        foreach (var item in detectedObjects)
        {
            item.gameObject.SetActive(false);
        }
        var i = 0;
        foreach (var item in ewRadar.objectsDetected)
        {
            var spec = item.GetComponent<Specification>();
            var obj = detectedObjects[i];
            obj.gameObject.SetActive(true);
            var no = obj.GetChild(1).GetComponent<TextMeshProUGUI>();
            no.text = i.ToString() + ".";
            var name = obj.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            name.text = item.name;
            var type = obj.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
            type.text = "Aircraft";
            if(item.name.ToLower().Contains("missile"))
            {
                type.text = "Missile";
            }
            var fre = obj.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
            fre.text = "null";
            var r = item.GetComponentInChildren<AIRadar>();
            if (r)
            {
                fre.text = r.f.ToString()+"ghz";
            }

            var distance = obj.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>();
            distance.text =( Vector3.Distance(ewRadar.transform.root.position,item.transform.position)/1000).ToString("0.0")+"km";
            
            var angle = obj.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>();
            angle.text = (180-Vector3.Angle(ewRadar.transform.root.position - item.transform.position, ewRadar.transform.root.forward)).ToString("0.0") + " deg";

            var size = obj.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>();
            size.text = "";
            if (spec != null)
            {
                size.text = spec.size.ToString()+"m2";
            }

            var pulseRate = obj.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>();
            pulseRate.text = UnityEngine.Random.Range(2,5).ToString("0.0")+"Hz";

            var speed = obj.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>();
            speed.text = "";
            var mo = item.GetComponent<MoveDummy>();
            if (mo != null)
            {
                speed.text = mo.speed.ToString() + "m/s";
            }


            i++;
        }
        
        //logtext.text = log;
    }

    public void contiune()
    {
        Time.timeScale = 1;
    }
    public void clear()
    {
        ewRadar.enabled = false;
        rwr.enabled = false;
        foreach (GameObject go in selector.filterObj)
        {
            if (go == null) continue;
            Destroy(go);
        }
        selector.filterObj.Clear();
        selector.mainPlane.GetComponent<MoveDummy>().controle = false;
        selector.mainPlane.position = pos;
        selector.mainPlane.rotation = rot;
    }

    public void submit()
    {
        if (ewRadar != null)
        {
            loadscript = JsonUtility.FromJson<EWloadScript>(script.text);
            equation.Pt = loadscript.power;
            equation.Gt = loadscript.gain;
            equation.Gr = loadscript.recieveGain;
            equation.Pr = loadscript.threshold;
            ewRadar.radar_Range = equation.getRange()*1000;
            ewRadar.autoChaffFire = loadscript.autoChaff;
            ewRadar.autoFlareFire = loadscript.autoFlare;
            ewRadar.chaffActivateRange = loadscript.chaffActive*1000f;
            ewRadar.flareActivateRange = loadscript.flareActive * 1000f;
            ewRadar.DIRCM_Module.active = loadscript.dircm;
            ewRadar.DIRCM_Module.effectRange = loadscript.dircmActive*1000;
            ewRadar.DIRCM_Module.idleDeflectionTime = loadscript.deflectionTime;
            Distance.text = equation.getRange().ToString("0.0")+"km";
            ewRadar.SetRangeRWR();
            rwr.SetConfigRWR();
            ewSphere.transform.localScale = new Vector3(ewRadar.radar_Range * 2, 1, ewRadar.radar_Range * 2);

        }
    }
    public void SetEWConfig()
    {
        if (ewRadar != null&&false)
        {
            //ewRadar.radar_Range = PlayerPrefs.GetFloat("ewRange") * 1000;
            ewRadar.autoChaffFire = PlayerPrefs.GetInt("autoChaff") == 0 ? false : true;
            ewRadar.autoFlareFire = PlayerPrefs.GetInt("autoFlare") == 0 ? false : true;
            ewRadar.chaffActivateRange = PlayerPrefs.GetFloat("chaffRange")*1000;
            ewRadar.flareActivateRange = PlayerPrefs.GetFloat("flareRange")*1000;
            ewRadar.DIRCM_Module.active = PlayerPrefs.GetInt("Dircm") == 0 ? false : true;
            ewRadar.DIRCM_Module.effectRange = PlayerPrefs.GetFloat("laserRange")*1000;
            ewRadar.DIRCM_Module.idleDeflectionTime = PlayerPrefs.GetFloat("deflectTime");
            ewRadar.SetRangeRWR();
            rwr.SetConfigRWR();
            ewSphere.transform.localScale = new Vector3(ewRadar.radar_Range * 2, 1, ewRadar.radar_Range * 2);
        }
    }


    public void runSimulation()
    {
        ewRadar.enabled = true;
        rwr.enabled = true;
        foreach(var go in selector.filterObj)
        {
            if (go == null) continue;
            if (go.tag.ToLower().Contains("missile"))
            {
                go.GetComponent<Rigidbody>().isKinematic = false;
                //selectedObject.transform.SetPositionAndRotation(transform.position + (transform.forward * 7f) + (transform.up * -4f), transform.rotation);
                go.GetComponent<missileEngine>().fire = true;
                if (Vector3.Angle(selector.mainPlane.position - go.transform.position, go.transform.forward) < 15)
                    go.GetComponent<missileNavigation>().GivenTarget = selector.mainPlane;
            }
            else
            {
                go.GetComponent<MoveDummy>().move = true;
            }
        }
        selector.mainPlane.GetComponent<MoveDummy>().controle = true;
    }

    public void EwRangeSet(float i)
    {
        ewRange.text = i.ToString("0.0");
        PlayerPrefs.SetFloat("ewRange", i);
    }
    public void ChaffRangeSet(float i)
    {
        chaffRange.text = i.ToString("0.0");
        PlayerPrefs.SetFloat("chaffRange", i);
    }
    public void FlareRange(float i)
    {
        flareRange.text = i.ToString("0.0");
        PlayerPrefs.SetFloat("flareRange", i);
    }
    public void LaserRange(float i)
    {
        laserRange.text = i.ToString("0.0");
        PlayerPrefs.SetFloat("laserRange", i);
    }
    public void DeflectTime(float i)
    {
        deflectTime.text = i.ToString("0");
        PlayerPrefs.SetFloat("deflectTime", i);
    }
    public void SetFlares(bool t)
    {
        flare.SetActive(t);
        PlayerPrefs.SetInt("autoFlare", t ? 1 : 0);
    }
    public void SetChaffs(bool t)
    {
        chaff.SetActive(t);
        PlayerPrefs.SetInt("autoChaff", t ? 1 : 0);
    }
    public void SetDircm(bool t)
    {
        foreach (GameObject dircmO in dircm)
        { dircmO.SetActive(t); }
        PlayerPrefs.SetInt("Dircm", t ? 1 : 0);
    }

}

