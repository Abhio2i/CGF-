using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FeedackUIReplay : MonoBehaviour
{
    [Header("Radar UI")]
    public Toggle TxSil;
    public Toggle SAR;
    public Toggle Jem;
    public Toggle ABdutyCycle;
    public Toggle PeactimeWartime;
    public Toggle HiLo;
    public Toggle BScope;
    public Toggle Freeze;
    public Toggle Isar;
    public Toggle Hrm;
    public Toggle Rbm;
    public Toggle Cued;
    public Toggle Pass;
    public Toggle Hist;
    public TextMeshProUGUI TWSText;
    public TextMeshProUGUI CRMText;
    public TextMeshProUGUI ACMText;
    public TextMeshProUGUI Distance;
    public TextMeshProUGUI VisibleRange;
    public TextMeshProUGUI Azimuth;
    public TextMeshProUGUI Bar;
    public TextMeshProUGUI Elevation;
    public TextMeshProUGUI MainMode;
    public TextMeshProUGUI Aast;
    public TextMeshProUGUI Altitude;
    public Transform AcmNobe;
    public Transform GainNobe;
    public Transform FrequencyNobe;
    public GameObject Map;
    public Transform AZimuthLineA;
    public Transform AZimuthLineB;
    public GameObject BScopeView;
    public Transform Display;
    public GameObject DLZ;

    [Header("WeaponUI")]
    public GameObject Weapons;

    [Header("EW UI")]
    public TextMeshProUGUI EWRange;
    public TextMeshProUGUI Flares;
    public TextMeshProUGUI Chaffs;
    public Toggle AutoFlare;
    public Toggle AutoChaffs;
    public Toggle AutoDircm;
    public GameObject MissileWarning;

    [Header("BirdEye")] 
    public Camera birdCamera;
    public Camera PlaneCamera;
    public List<Transform> icons;
    public List<Sprits> sprits;
    public FeedBackRecorderAndPlayer feed;
    public FeedBackCameraController feedCam;
    public bool feedbackPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in icons)
        {
            Sprits sprit = new Sprits();
            sprit.sprite = t.gameObject;
            sprit.name = t.GetChild(0).GetComponent<TextMeshProUGUI>();
            sprit.Speed = t.GetChild(1).GetComponent<TextMeshProUGUI>();
            sprit.Altitude = t.GetChild(2).GetComponent<TextMeshProUGUI>();
            sprits.Add(sprit);
        }
    }
     
    // Update is called once per frame
    void Update()
    {
        if (feed != null && feed.Playing)
        {
            if(!feedbackPlayed)
            {
                feedbackPlayed = true;
                for (int i = 0; i < sprits.Count; i++)
                {
                    int RealAllysCount = FeedBackRecorderAndPlayer.Allys.Count - feed.ExtraEntity.Count - FeedBackRecorderAndPlayer.Missiles.Count;
                    if(i<= RealAllysCount)
                    {
                        string name = feed.loadSystemAndSpawn.findCraftType(FeedBackRecorderAndPlayer.Allys[i].gameObject);
                        sprits[i].sprite.SetActive(true);
                        sprits[i].name.text = name;
                        // Create a new entry for the OnPointerClick event
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;

                        Transform onj = FeedBackRecorderAndPlayer.Allys[i];
                        // Add a callback function to the event
                        entry.callback.AddListener((data) => { feedCam.GoToPosition(onj); });

                        // Add the entry to the EventTrigger
                        sprits[i].sprite.AddComponent<EventTrigger>().triggers.Add(entry);


                        if (name.Contains("Ally"))
                        {
                            sprits[i].name.text = name.Replace("Ally", "Blue");
                            sprits[i].name.color = Color.blue;
                            sprits[i].Speed.color = Color.blue;
                            sprits[i].Altitude.color = Color.blue;
                        }
                        else
                        if (name.Contains("Enemy"))
                        {
                            sprits[i].name.text = name.Replace("Enemy", "Red");
                            sprits[i].name.color = Color.red;
                            sprits[i].Speed.color = Color.red;
                            sprits[i].Altitude.color = Color.red;
                        }
                        else
                        if (name.Contains("Neutral"))
                        {
                            sprits[i].name.color = Color.white;
                            sprits[i].Speed.color = Color.white;
                            sprits[i].Altitude.color = Color.white;
                        }
                    }

                }
            }

            Bird();
            if (feed.radarLog != null)
            {
                MainMode.text = feed.radarLog.MainMode.ToString();
                Altitude.text = "";
                Distance.text = (feed.radarLog.Range).ToString("0") + "NM";
                VisibleRange.text = (feed.radarLog.RangeScale).ToString("0");
                Azimuth.text = (feed.radarLog.Azimuth / 10).ToString() + "A";
                Bar.text = (feed.radarLog.Bar / 10).ToString() + "B";
                TWSText.text = feed.radarLog.TWS ? "TWS" : (feed.radarLog.MainMode == RadarScreenUpdate_new.Mode1.ATA ? "TWS" : "TWS");
                TWSText.text = feed.radarLog.MainMode == RadarScreenUpdate_new.Mode1.ATS && !feed.radarLog.TWS ? "STCT" : TWSText.text;
                TWSText.text = feed.radarLog.MainMode == RadarScreenUpdate_new.Mode1.ATG ? (feed.radarLog.TWS ? "GMTI" : "AGR") : TWSText.text;
                //TWSText.color = tws ? Color.green : (MainMode == Mode1.ATG ? new Color(0, 0.4f, 0) : Color.green);
                CRMText.color = feed.radarLog.CRM ? Color.green : new Color(0, 0.4f, 0);
                ACMText.color = feed.radarLog.ACM ? Color.green : new Color(0, 0.4f, 0);


                TxSil.isOn = feed.radarLog.TxSil;
                SAR.isOn = feed.radarLog.SAR;
                Jem.isOn = feed.radarLog.Jem;
                ABdutyCycle.isOn = feed.radarLog.ABDutyCycle;
                PeactimeWartime.isOn = false;//Pending
                HiLo.isOn = feed.radarLog.HILo;
                BScope.isOn = feed.radarLog.BScope;
                Freeze.isOn = false;//Pending
                Isar.isOn = feed.radarLog.ISAR;
                Hrm.isOn = false;//Pending
                Rbm.isOn = feed.radarLog.RBM;
                Cued.isOn = false;//Pending
                Pass.isOn = feed.radarLog.PassiveMode;
                Hist.isOn = false;//Pending
                BScopeView.SetActive(feed.radarLog.BScope);
                Map.SetActive((int)feed.radarLog.MainMode != 0);
                AZimuthLineA.rotation = Quaternion.Euler(0f, 0f, -feed.radarLog.Azimuth);
                AZimuthLineB.rotation = Quaternion.Euler(0f, 0f, feed.radarLog.Azimuth);
                AcmNobe.rotation = Quaternion.Euler(0, 0, ((int)feed.radarLog.ACMModes) * -45.45f);
                FrequencyNobe.rotation = Quaternion.Euler(0, 0, ((int)feed.radarLog.bandwidth) * -58.45f);
                GainNobe.rotation = Quaternion.Euler(0, 0, ((int)feed.radarLog.G) * -58.45f);
                DLZ.SetActive(true);
                Weapons.SetActive(true);
                SetRadarIcon();


            }
            else
            if (feed.aiRadarLog != null)
            {
                MainMode.text = "ATA";
                Altitude.text = "";
                Distance.text = (feed.aiRadarLog.Range / 1852).ToString("0") + "NM";
                VisibleRange.text = (feed.aiRadarLog.Range / 1852).ToString("0");
                Azimuth.text = (feed.aiRadarLog.Azimuth / 10).ToString("0") + "A";
                Bar.text = (feed.aiRadarLog.Bar / 10).ToString("0") + "B";
                TWSText.text = "TWS";
                //TWSText.color = tws ? Color.green : (MainMode == Mode1.ATG ? new Color(0, 0.4f, 0) : Color.green);
                CRMText.color = false ? Color.green : new Color(0, 0.4f, 0);
                ACMText.color = true ? Color.green : new Color(0, 0.4f, 0);

                TxSil.isOn = false;
                SAR.isOn = false;
                Jem.isOn = false;
                ABdutyCycle.isOn = feed;
                PeactimeWartime.isOn = false;
                HiLo.isOn = false;
                BScope.isOn = false;
                Freeze.isOn = false;
                Isar.isOn = false;
                Hrm.isOn = false;
                Rbm.isOn = false;
                Cued.isOn = false;
                Pass.isOn = false;
                Hist.isOn = false;
                BScopeView.SetActive(false);
                Map.SetActive(false);
                AZimuthLineA.rotation = Quaternion.Euler(0f, 0f, -feed.aiRadarLog.Azimuth);
                AZimuthLineB.rotation = Quaternion.Euler(0f, 0f, feed.aiRadarLog.Azimuth);
                AcmNobe.rotation = Quaternion.Euler(0, 0, (3) * -45.45f);
                FrequencyNobe.rotation = Quaternion.Euler(0, 0, ((int)feed.aiRadarLog.F-8) * -58.45f);
                GainNobe.rotation = Quaternion.Euler(0, 0, ((int)feed.aiRadarLog.G) * -58.45f);
                DLZ.SetActive(false);
                Weapons.SetActive(false);
                SetRadarIcon(true);
            }

            if (feed.ewRadarLog != null)
            {
                EWRange.text = (feed.ewRadarLog.Radar_Range/ 1852).ToString("0");
                Flares.text = feed.ewRadarLog.Flares.ToString();
                Chaffs.text = feed.ewRadarLog.Chaffs.ToString();
                AutoFlare.isOn = feed.ewRadarLog.autoFlare;
                AutoChaffs.isOn = feed.ewRadarLog.autoChaff;
                AutoDircm.isOn = feed.ewRadarLog.autoDircm; 
                MissileWarning.SetActive(feed.ewRadarLog.MissileWarning);
                SetEWIcon();
            }
        }
    }

    public void SetRadarIcon(bool Ai = false)
    {
        //var str = "UIDetect";
        var i = 0;
        for (var ict = 0; ict < feed.RadarScreen.EnemyPrefabs.Count; ict++)
        {
            var ob = feed.RadarScreen.EnemyPrefabs[ict];
            //var ob2 = EnemyPrefabs2[ict];
            int count=0;
            if (Ai) count = feed.aiRadarLog.detect.Count;
            else
            count = feed.radarLog.detect.Count;
            
            if (i < count)
            {
                ob.sprite.SetActive(true);
                
                var x = 0f;
                var y = 0f;
                var z = 0f;

                if (Ai)
                {
                    float Azimuth = feed.aiRadarLog.Azimuth;
                    var w = (((feed.aiRadarLog.Range / 1852) * feed.RadarScreen.uttamRadar.mile) / Mathf.Tan((90 - Azimuth) * Mathf.Deg2Rad));
                    Display.transform.localPosition = feed.RadarScreen.ForgroundOffset;
                    Vector3 DummyTarget = FeedBackCameraController.Target.InverseTransformPoint(feed.aiRadarLog.detect[ict]);
                    x = (DummyTarget.x / w) * (feed.RadarScreen.Display.rect.height / (Mathf.Tan((90 - Azimuth) * Mathf.Deg2Rad)));
                    y = ((DummyTarget.z / ((feed.aiRadarLog.Range / 1852) * feed.RadarScreen.uttamRadar.mile)) * (feed.RadarScreen.Display.rect.height /* 2f*/)) - feed.RadarScreen.ForgroundOffset2;
                    z = 0f;
                    ob.sprite.transform.localPosition = new Vector3(x,y,z);
                }
                else
                {
                    ob.sprite.transform.localPosition = feed.radarLog.detect[ict];
                }

                    //Check Enemy Or Player
                    bool friend = true;
                if (Ai)
                    friend = feed.aiRadarLog.Friend[ict];
                else
                    friend = feed.radarLog.Data[ict].x == 1;
                if (friend)
                {
                    ob.ally.SetActive(true);
                    ob.enemy.SetActive(false);
                }
                else
                {
                    ob.ally.SetActive(false);
                    ob.enemy.SetActive(true);
                }
                ob.dir.SetActive(false);
                bool Select = false;
                if (!Ai)
                    Select =feed.radarLog.Data[ict].z == 1;
                ob.Select.SetActive(Select);
                ob.HighThreat.SetActive(false);
                bool HardLOck = false;
                if (Ai)
                    HardLOck = feed.aiRadarLog.locks[ict];
                else
                    HardLOck = feed.radarLog.Data[ict].y == 1;
                ob.HardLock.SetActive(HardLOck);

                Vector4 info = Vector4.zero;
                if (Ai)
                    info = feed.aiRadarLog.info[ict];
                else
                    info = feed.radarLog.Info[ict];

                //Taregt Distance
                ob.Distance.gameObject.SetActive(true);
                ob.Distance.text = info.x.ToString("0.0");
                //Taregt Altitude
                ob.Altitude.gameObject.SetActive(true);
                ob.Altitude.text = info.y.ToString("0.0");
                //Taregt Speed
                ob.Speed.gameObject.SetActive(true);
                ob.Speed.text = info.z.ToString("0.0");
                //Taregt Heading
                ob.Heading.gameObject.SetActive(true);
                ob.Heading.text = info.w.ToString("0.0");

            }
            else
            {
                ob.sprite.SetActive(false);
            }
            i++;
        }
    }

    public void SetEWIcon()      //Setting up the Symbols for Radar Display
    {

        for (int i = 0; i < feed.rwr.DetectLimit; i++)
        {
            if (i < feed.ewRadarLog.detect.Count)
            {

                feed.rwr.enemys[i].SetActive(true);
                //Setting Enemy icon position on Radar
                Vector3 radarDummy = FeedBackCameraController.Target.InverseTransformPoint(feed.ewRadarLog.detect[i]);
                feed.rwr.enemys[i].transform.localPosition = new Vector3(radarDummy.x * feed.rwr.Distance, radarDummy.z * feed.rwr.Distance, 0);
                feed.rwr.enemys[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = feed.ewRadarLog.info[i];
                feed.rwr.enemys[i].transform.GetChild(0).gameObject.SetActive(true);
                feed.rwr.enemys[i].transform.GetChild(1).gameObject.SetActive(false);
                feed.rwr.enemys[i].transform.GetChild(2).gameObject.SetActive(false);
                feed.rwr.enemys[i].transform.GetChild(3).gameObject.SetActive(false);
                feed.rwr.enemys[i].transform.GetChild(4).gameObject.SetActive(true);
                feed.rwr.enemys[i].transform.GetChild(5).gameObject.SetActive(false);
            }
            else
            {
                feed.rwr.enemys[i].SetActive(false);
            }
        }


    }

    public void Bird()
    {
        UnityEngine.Camera _camera = feed.Camera2.activeSelf ? birdCamera : PlaneCamera;
        Vector3 pos1 = _camera.ViewportToScreenPoint(new Vector3(1, 1, 0));
        for (int i = 0; i < sprits.Count; i++)
        {
            if (sprits[i].sprite.activeSelf)
            {

                Transform obj = FeedBackRecorderAndPlayer.Allys[i];
                Vector3 pos = _camera.WorldToScreenPoint(obj.position);

                pos.z = Mathf.Clamp(pos.z, -1f, 1f);
                pos.x = pos.x < 0 ? 2f : (pos.x > pos1.x ? pos1.x - 2 : pos.x);
                pos.y = pos.y < 0 ? 2f : (pos.y > pos1.y ? pos1.y - 2 : pos.y);
                sprits[i].sprite.transform.position = pos;
                sprits[i].name.gameObject.SetActive(pos.z>0);
                sprits[i].Speed.gameObject.SetActive(pos.z > 0);
                sprits[i].Altitude.gameObject.SetActive(pos.z > 0);
                if (pos.z > 0 && feed.RecordingEntityData[i].entityInfoLog.Count>0)
                {
                    EntityInfoLog enitiyLog = feed.RecordingEntityData[i].entityInfoLog[FeedBackRecorderAndPlayer.currentFrame];
                    sprits[i].Speed.text = enitiyLog.Speed.ToString("0") + "knots";
                    sprits[i].Altitude.text = enitiyLog.Altitude.ToString("0") + "ft";
                }
                else
                {
                    sprits[i].Speed.text = "";
                    sprits[i].Altitude.text = "";
                }
            }
        }
    }
}
