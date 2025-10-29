using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Feed;
using System;

[Serializable]
public class RWRLog
{
    public float rot;
    public bool missileWarning;
    public List<Vector3> detect = new List<Vector3>();
    public List<String> icon = new List<string>();
}

public class RWR : MonoBehaviour
{
    public enum BurstSize
    {
        Four,
        Eight,
        Twelve,
        Twentyfou,
        Max
    };

    //Make sure to have all the parent rect transform scale of 1 only
    public float DetectLimit = 10f;
    public Transform Plane, BG,Sign;
    public RectTransform Display;
    public GameObject EnemySpritePrefab, MissileApproachWarning , DircmOn;
    public EWRadar radar;
    public PlaneData planeData;
    public string Log="";
    public TextMeshProUGUI ChaffsCount;
    public TextMeshProUGUI FlaresCount;
    public TextMeshProUGUI EwRange;
    public Transform ChaffsNobe;
    public Transform FlaresNobe;
    public BurstSize chaffsSize = BurstSize.Four;
    public BurstSize FlareSize = BurstSize.Four;

    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    public float Distance = 3.4f;     //Mapping of Radar Display with Radar Range
    private Transform radarDummy;      //To Get local position of Target on Player's axis
    public List<GameObject> enemys;    //list of target icons on radar screen
    private float timeStamp;            //for Coroutines
    private bool MissileApproach = false;
    private int prevHigh = 11;            //Previsouly detected high threat

    void OnEnable()         //As Radar script is currently on RadarCanvas that can be disabled by player
    {
        StartCoroutine(MAWS());
        StartCoroutine(TargetingYou());
        StartCoroutine(ThreatAnalysis());
    }
    void Start()
    {
        loadSystemAndSpawnPlanes = GameObject.FindObjectOfType<LoadSystemAndSpawnPlanes>(); 
        if (radar != null)
        {
            radarDummy = radar.transform.GetChild(0).transform;
        }
        enemys = new List<GameObject>();

        for (var i = 0f; i < DetectLimit; i++)           //Instansiating prefabe of enemy targets on radar and disabling them
        {
            var obj = Instantiate(EnemySpritePrefab);
            obj.transform.parent = Display.transform;
            obj.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
            obj.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            obj.SetActive(false);
            enemys.Add(obj);
        }
        SetConfigRWR();
        StartCoroutine(MAWS());
        StartCoroutine(TargetingYou());
        StartCoroutine(ThreatAnalysis());
    }
    public void SetConfigRWR()
    {
        Distance = ((Display.rect.width / 2f) / (radar.radar_Range)) * 0.75f;      //Radar Screen Size
    }

    public void ChaffsBurstSet(int i)
    {

        if (i >= 0)
        {
            if (i == 0)
            {
                if ((int)chaffsSize - 1 >= 0)
                {
                    chaffsSize = (BurstSize)((int)chaffsSize) - 1;
                }
            }
            else
            {
                if ((int)chaffsSize + 1 < 5)
                {
                    chaffsSize = (BurstSize)((int)chaffsSize) + 1;
                }
            }
        }
        ChaffsNobe.rotation = Quaternion.Euler(0, 0, ((int)chaffsSize) * -58.45f);
        int burstSize = 4;
        switch (chaffsSize)
        {
            case BurstSize.Four: burstSize = 4; break;
            case BurstSize.Eight : burstSize = 8; break;
            case BurstSize.Twelve : burstSize = 12; break;
            case BurstSize.Twentyfou : burstSize = 24; break;
            case BurstSize.Max: burstSize = radar.maxChaff; break;
        }

        radar.burstChaffSize = burstSize;

    }

    public void FlaresBurstSet(int i)
    {

        if (i >= 0)
        {
            if (i == 0)
            {
                if ((int)FlareSize - 1 >= 0)
                {
                    FlareSize = (BurstSize)((int)FlareSize) - 1;
                }
            }
            else
            {
                if ((int)FlareSize + 1 < 5)
                {
                    FlareSize = (BurstSize)((int)FlareSize) + 1;
                }
            }
        }
        FlaresNobe.rotation = Quaternion.Euler(0, 0, ((int)FlareSize) * -58.45f);
        int burstSize = 4;
        switch (FlareSize)
        {
            case BurstSize.Four: burstSize = 4; break;
            case BurstSize.Eight: burstSize = 8; break;
            case BurstSize.Twelve: burstSize = 12; break;
            case BurstSize.Twentyfou: burstSize = 24; break;
            case BurstSize.Max: burstSize = radar.maxFlares; break;
        }

        radar.burstFlareSize = burstSize;

    }

    public void AutoChaffsSet(bool i)
    {
        radar.autoChaffFire = i;
        UITool.ToolTip("Chaffs " +  i.ToString());
    }

    public void AutoFlaresSet(bool i)
    {
        radar.autoFlareFire = i;
    }
    public void AutoDircmSet(bool i)
    {
        radar.DIRCM_Module.active = i;
    }


    void Update()
    { 

        ChaffsCount.text = "C"+radar.maxChaff.ToString();
        FlaresCount.text = "F"+radar.maxFlares.ToString();
        EwRange.text = (radar.radar_Range / 1852).ToString("0");
        BG.rotation = Quaternion.Euler(0, 0, Plane.eulerAngles.y);
        Sign.localEulerAngles = new Vector3(0,0,-BG.eulerAngles.z);
        DircmOn.SetActive(radar.DIRCM_Module.isON);
        if (radar.ListUpdated)
        {
            IconSetter();
            //radar.ListUpdated = false;
        }
        if (radar.QuickUpdate)          //When indexes in list shifts due to removal of any item
        {
            StopAllCoroutines();
            timeStamp = 0f;
            StartCoroutine(MAWS());
            StartCoroutine(TargetingYou());
            StartCoroutine(ThreatAnalysis());
            radar.QuickUpdate = false;
            StartCoroutine(QuickUpdateOff());       //To Reset timeStamp
        }
        if (Display != null && EnemySpritePrefab != null)
        {
            RWRLog log = new RWRLog();
            log.rot = Plane.eulerAngles.y;
            log.missileWarning = MissileApproachWarning.activeSelf;
            RadarIconUpdate(log);
            Log = JsonUtility.ToJson(log);
        }
        
    }

    //Enemy symbols details on its child are:
    // Child(0) Name Symbolic
    // Child(1) Recently Detected
    // Child(2) Airborne
    // Child(3) Waterborne
    // Child(4) Highest Threat
    // Child(5) Targeting You
    private void RadarIconUpdate(RWRLog log)      //Setting up the Symbols for Radar Display
    {

        for (int i = 0; i < DetectLimit; i++)
        {
            if (i < radar.objectsDetected.Count)
            {
                if (radar.objectsDetected[i] != null)
                {   //Missile Approach Warning
                    if (radar.objectsDetected[i].tag.Contains("Missile"))
                    {
                        if (enemys[i].transform.GetChild(5).gameObject.activeSelf)
                            MissileApproach = true;
                    }
                    //Setting Enemy icon position on Radar
                    radarDummy.position = radar.objectsDetected[i].transform.position;
                    enemys[i].transform.localPosition = new Vector3(radarDummy.localPosition.x * Distance, radarDummy.localPosition.z * Distance, 0);
                    radarDummy.localPosition = Vector3.zero;
                    var str = "";
                    for (var b = 0; b < enemys[i].transform.childCount; b++)
                    {
                        str += (enemys[i].transform.GetChild(b).gameObject.activeSelf ? "1" : "0") + ",";
                    }
                    log.detect.Add(enemys[i].transform.localPosition);
                    log.icon.Add(str);
                }
            }
            else
            {
                enemys[i].SetActive(false);
            }
        }

        
    } 
    private void IconSetter()       //Setting up the Icon Symbols for different Targets
    {
        for (int i = 0; i < DetectLimit; i++)
        {
            if (i < radar.objectsDetected.Count)
            {
                if (radar.objectsDetected[i] != null)
                {
                    enemys[i].SetActive(true);

                    if (radar.JamObjects.ContainsKey(radar.objectsDetected[i]))
                    {
                        enemys[i].transform.GetChild(6).gameObject.SetActive(true);
                        enemys[i].transform.GetChild(7).gameObject.SetActive(true);
                        enemys[i].transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = radar.JamObjects[radar.objectsDetected[i]].ToString("0");
                    }
                    else
                    {
                        enemys[i].transform.GetChild(6).gameObject.SetActive(false);
                        enemys[i].transform.GetChild(7).gameObject.SetActive(false);
                    }

                    //Waterborne || Airborne || Surfaceborne
                    if (radar.objectsDetected[i].tag.Contains("Player") || radar.objectsDetected[i].tag.Contains("Ally"))
                    {
                        string id = loadSystemAndSpawnPlanes.findCraftType(radar.objectsDetected[i]);
                        id = id.Replace("Ally_", "B");
                        id = id.Replace("Enemy_", "R");
                        id = id.Replace("Neutral_", "N");
                        id = id.Replace("warship_", "W");
                        id = id.Replace("sam_", "S");
                        enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = id;
                        enemys[i].transform.GetChild(2).gameObject.SetActive(true);
                    }
                    if (radar.objectsDetected[i].tag.Contains("Enemy"))
                    {
                        string id = loadSystemAndSpawnPlanes.findCraftType(radar.objectsDetected[i]);
                        id = id.Replace("Ally_", "B");
                        id = id.Replace("Enemy_", "R");
                        id = id.Replace("Neutral_", "N");
                        id = id.Replace("warship_", "W");
                        id = id.Replace("sam_", "S");

                        enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = id;
                        /*
                        if (radar.objectsDetected[i].name.Contains("F 18"))
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "F1";
                        if (radar.objectsDetected[i].name.Contains("Hawk"))
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "H";
                        if (radar.objectsDetected[i].name.Contains("F35"))
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "F3";
                        if (radar.objectsDetected[i].name.Contains("Radar"))
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "SM";
                        */
                        enemys[i].transform.GetChild(2).gameObject.SetActive(true);
                    }
                    if (radar.objectsDetected[i].tag.Contains("Missile"))
                    {
                        enemys[i].transform.GetChild(2).gameObject.SetActive(true);
                        enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "M";
                        if (radar.objectsDetected[i].GetComponent<missileNavigation>().NavigationType == missileNavigation.mode.Infrared)
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "IR";
                        if (radar.objectsDetected[i].GetComponent<missileNavigation>().NavigationType == missileNavigation.mode.ActiveRadar)
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "AR";
                        if (radar.objectsDetected[i].GetComponent<missileNavigation>().NavigationType == missileNavigation.mode.DataLink)
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "AR";
                        if (radar.objectsDetected[i].name.Contains("RIM"))
                            enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "RI";
                    }
                    if (radar.objectsDetected[i].name.Contains("Commercial"))
                    {
                        enemys[i].transform.GetChild(2).gameObject.SetActive(true);
                        enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "C";
                    }

                    //Setting Recently Detected
                    if (radar.objectsDetected[i] == radar.LatestDetected)
                    {
                        //if (Display.parent.GetChild(0).gameObject.activeSelf)
                            //Display.parent.GetChild(0).GetComponent<TextMeshProUGUI>().text = radar.LatestDetected.name.Substring(0, radar.LatestDetected.name.Length - 7);
                        enemys[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                        enemys[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
            else
            {
                enemys[i].SetActive(false);
            }
        }
    }
    IEnumerator MAWS()  //Missile Approach Warning System
    {
        yield return new WaitForSeconds(timeStamp);
        if (MissileApproach)
        {
            MissileApproach = false;
            MissileApproachWarning.SetActive(true);
        }
        yield return new WaitForSeconds(0.6f);
        MissileApproachWarning.SetActive(false);
        StartCoroutine(MAWS());
    }
    #region HighThreat
    IEnumerator ThreatAnalysis() //HighestThreat Analysis
    {
        yield return new WaitForSeconds(timeStamp);
        int i = 0;
        int high = 11;
        for (; i < radar.objectsDetected.Count; i++)
        {
            if (radar.objectsDetected[i] == null)
                continue;
            if (radar.objectsDetected[i].tag.Contains("Missile"))
            {
                if (enemys[i].transform.GetChild(5).gameObject.activeSelf)
                {        //If a missile is targeting you
                    high = i;
                    break;
                }
                else
                    enemys[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (radar.objectsDetected[i].tag.Contains("Enemy"))        //If a Plane is Targeting You
            {
                if (enemys[i].transform.GetChild(5).gameObject.activeSelf)
                    high = i;
                else
                    StartCoroutine(HighLockRemove(i));
            }
        }
        if (high != 11)
        {
            if (prevHigh == 11)     //setting up first time
            {
                prevHigh = high;
                enemys[high].transform.GetChild(4).gameObject.SetActive(true);
            }
            else                        //Setting new target
            {
                enemys[prevHigh].transform.GetChild(4).gameObject.SetActive(false);
                prevHigh = high;
                enemys[high].transform.GetChild(4).gameObject.SetActive(true);
            }
        }
        StartCoroutine(ThreatAnalysis());
    }
    IEnumerator HighLockRemove(int i)
    {
        yield return new WaitForSeconds(timeStamp);
        enemys[i].transform.GetChild(4).gameObject.SetActive(false);
    }
    #endregion

    #region TargetingYou
    IEnumerator TargetingYou() //Checking if Targeting You
    {
        int i = 0;
        for (; i < radar.objectsDetected.Count; i++)
        {
            if (radar.objectsDetected[i] == null)
                continue;
            if (Vector3.Angle(Plane.transform.position - radar.objectsDetected[i].transform.position, radar.objectsDetected[i].transform.forward) < 15)
            {
                yield return new WaitForSeconds(timeStamp);
                if (radar.objectsDetected[i] == null)
                    continue;
                SubTargetinRoutine(radar.objectsDetected[i], i);        //currently target is around you,checking again in another routine
            }
            else
            {
                if (enemys[i]!=null)
                enemys[i].transform.GetChild(5).gameObject.SetActive(false);
            }
                
        }
        yield return new WaitForSeconds(timeStamp);
        StartCoroutine(TargetingYou());
    }
    void SubTargetinRoutine(GameObject threat, int i)
    {
        if (threat == null)
            return;
        if (Vector3.Angle(Plane.transform.position - radar.objectsDetected[i].transform.position, radar.objectsDetected[i].transform.forward) < 15)
        {
            enemys[i].transform.GetChild(5).gameObject.SetActive(true);
        }
        else
            enemys[i].transform.GetChild(5).gameObject.SetActive(false);
    }
    #endregion
    IEnumerator QuickUpdateOff()
    {
        yield return new WaitForSeconds(2f);
        timeStamp = 0.5f;
    }
}
