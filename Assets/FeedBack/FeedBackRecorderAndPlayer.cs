using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;
using static UnityEngine.Rendering.DebugUI;
using Debug = UnityEngine.Debug;

public class FeedBackRecorderAndPlayer : MonoBehaviour
{

    public MissionPlan missionPlan;
    [Header("Data")]
    [HideInInspector]
    public List<EntityData> RecordingEntityData = new List<EntityData>();
    [HideInInspector]
    public List<EventData> RecordingEventData = new List<EventData>();
    public static EventData CurrentEventData = new EventData();

    [Header("Entity")]
    public static List<Transform> Allys = new List<Transform>();
    public List<Transform> ExtraEntity = new List<Transform>();
    public static List<Transform> Missiles = new List<Transform>();

    [Header("Recording Controls")]
    public bool Recording = false;
    public bool Playing = false;
    public static bool isPlaying = false;
    public bool resume = false;
    public bool seeking = false;
    public bool BirdEyeView = false;
    public int FrameCounter = 0;
    public static int currentFrame = 0;
    private int frameSeeking = 0;
    public int MaxFrames = 0;
    public int ViewCraft = 0;
    public AIRadar CurrentRadar;
    public TextMeshProUGUI EntityText;
    public Slider slider;
    public Toggle PauseToggle;
    public TMP_InputField frameINput;
    public TextMeshProUGUI TextMaxFrame;
    public GameObject FeedbackUI;
    public List<LineRenderer> lineRenderers = new List<LineRenderer>();
    public bool visiblePath = true;
    [Header("camers and Canvas")]
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject Camera1;
    public GameObject Camera2;
    public List<GameObject> otherUI;

    [Header("Entity Trail Materials")]
    public Material MainAircrafts;
    public Material AllyAircraft;
    public Material EnemyAircraft;
    public Material NeutralAircraft;
    public Material Missile;
    public Material Bomb;
    public Material waypoint;

    [Header("Log")]
    [SerializeField]
    public RadarLog radarLog;
    [SerializeField]
    public AIRadarLog aiRadarLog;
    [SerializeField]
    public EWRadarLog ewRadarLog;
    [SerializeField]
    public EntityInfoLog entityInfoLog;

    [Header("Neccessory Script Shutdown")]
    public LoadSystemAndSpawnPlanes loadSystemAndSpawn;
    public RadarScreenUpdate_new RadarScreen;
    public RWR rwr;
    public ExtrasIntegration extrasIntegration;
    public UttamRadar Radar;
    public LineRenderer lineRenderer;
    public KillAssesment_Info info;

    [Header("DataBase")]
    public string DatabasePath = "C:/";
    public string path = "";
    // Start is called before the first frame update
    private void Awake()
    {
        Allys.Clear();
        Missiles.Clear();
    }

    void Start()
    {
        

        DatabasePath = PlayerPrefs.GetString("DatabasePathe");
        DatabasePath = DatabasePath == "" ? "E:/" : DatabasePath;
        CreateDirectory(DatabasePath);
        
        Invoke("StartRecording",2f);
    }


    void OpenFileInNotepad(string filePath)
    {
        // Ensure we're on a Windows platform before trying to open Notepad
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        Process.Start(new ProcessStartInfo
        {
            FileName = "notepad.exe",
            Arguments = filePath,
            UseShellExecute = true,
        });
#else
        Debug.LogWarning("This feature is only supported on Windows.");
#endif
    }



    public void PathVisile(bool i)
    {
        visiblePath = i;
        foreach(LineRenderer l in lineRenderers)
        {
            l.enabled = visiblePath;
        }
    }

    public void CreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            DatabasePath = path;
            PlayerPrefs.SetString("DatabasePathe", path);
            if (!Directory.Exists(DatabasePath + "CGF_Mission_Feedback"))
            {
                Directory.CreateDirectory(DatabasePath + "CGF_Mission_Feedback");

            }
            //ChooseDirectory.SetActive(false);
            //MissionListUpdate();
        }
        else
        {
            //ChooseDirectory.SetActive(true);
        }



    }

    private void FixedUpdate()
    {

        currentFrame = FrameCounter;
        isPlaying = Playing;
        if (Recording)
        {
            int i = 0;
            foreach (Transform t in Allys)
            {
                RecordingEntityData[i].position.Add(t.position);
                RecordingEntityData[i].rotation.Add(t.eulerAngles);
                RecordingEntityData[i].active.Add(t.gameObject.activeSelf);
                if (RecordingEntityData[i].Text != null)
                {
                    RecordingEntityData[i].value.Add(RecordingEntityData[i].Text.text);
                }
                if (t == Radar.transform.root)
                {
                    RecordingEntityData[i].radarLog.Add(RadarScreen.radarlog);
                    RecordingEntityData[i].weaponLog.Add(RecordingEntityData[i].specification.weaponLog);
                }
                if (RecordingEntityData[i].specification != null)
                {
                    RecordingEntityData[i].entityInfoLog.Add(RecordingEntityData[i].specification.entityInfo);
                }
                if (RecordingEntityData[i].aIRadar != null)
                {
                    RecordingEntityData[i].aiRadarLog.Add(RecordingEntityData[i].aIRadar.aiRadarLog);
                    RecordingEntityData[i].weaponLog.Add(RecordingEntityData[i].specification.weaponLog);
                }
                if (RecordingEntityData[i].eWRadar != null)
                {
                    RecordingEntityData[i].ewRadarLog.Add(RecordingEntityData[i].eWRadar.ewRadarLog);
                }


                i++;
            }
            RecordingEventData.Add(CurrentEventData);

            FrameCounter++;
        }

        if (Playing)
        {
            radarLog = null;
            aiRadarLog = null;
            ewRadarLog = null;
            entityInfoLog = null;
            int i = 0;
            foreach (Transform t in Allys)
            {
                Allys[i].position = RecordingEntityData[i].position[FrameCounter];
                Allys[i].eulerAngles = RecordingEntityData[i].rotation[FrameCounter];
                if(Allys[i].gameObject.activeSelf != RecordingEntityData[i].active[FrameCounter])
                    Allys[i].gameObject.SetActive(RecordingEntityData[i].active[FrameCounter]);
                
                if (RecordingEntityData[i].Text != null)
                {
                    RecordingEntityData[i].Text.text = RecordingEntityData[i].value[FrameCounter];
                }
                if (FeedBackCameraController.Target == Allys[i])
                {
                    if (RecordingEntityData[i].specification && RecordingEntityData[i].specification.entityType == Specification.EntityType.Player)
                    {
                        radarLog = RecordingEntityData[i].radarLog[FrameCounter];
                        ewRadarLog = RecordingEntityData[i].ewRadarLog[FrameCounter];
                        entityInfoLog = RecordingEntityData[i].entityInfoLog[FrameCounter];
                    }

                    if (RecordingEntityData[i].specification && RecordingEntityData[i].specification.entityType == Specification.EntityType.AI)
                    {
                        aiRadarLog = RecordingEntityData[i].aiRadarLog[FrameCounter];
                        ewRadarLog = RecordingEntityData[i].ewRadarLog[FrameCounter];
                        entityInfoLog = RecordingEntityData[i].entityInfoLog[FrameCounter];
                    }
                }
                i++;
            }
            if (RecordingEventData.Count > 0)
                CurrentEventData = RecordingEventData[FrameCounter];
            if (!seeking)
            {
                if (!resume)
                {
                    FrameCounter++;
                }
                
                if (FrameCounter >= MaxFrames)
                {
                    FrameCounter = 0;
                    //Playing = false;
                }
                slider.value = FrameCounter;
                frameINput.text = FrameCounter.ToString();
            }
        }

    }

    public static void AddEvent(string ev)
    {
        ev = ev.Replace("(Clone)","");
        CurrentEventData.events.Add(ev);
    }

    public void StartRecording()
    {
        Debug.Log("Start Recording");
        Recording = true;
        FrameCounter = 0;

        //Merge ExtraEntity List and Ally List;
        foreach(Sprits t in RadarScreen.EnemyPrefabs)
        {
            //ExtraEntity.Add(t.transform);
        }
        
        Allys.AddRange(ExtraEntity);
        //Merge Missile List and Ally List;
        Allys.AddRange(Missiles);

        //Clear All Data And Create new Data
        RecordingEntityData.Clear();
        for (int i = 0; i < Allys.Count; i++)
        {
            EntityData data = new EntityData();
            if (Allys[i].TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI t))
            {
                data.Text = t;
            }
            int RealAllysCount = Allys.Count - ExtraEntity.Count - Missiles.Count;
            if (i <= RealAllysCount)
            {
                EWRadar eWRadar = Allys[i].GetComponentInChildren<EWRadar>();
                if (eWRadar != null)
                {
                    data.eWRadar = eWRadar;
                }
                if (Allys[i].TryGetComponent<CombineUttam>(out CombineUttam radar))
                {
                    data.aIRadar = radar;
                }
                if (Allys[i].TryGetComponent<Specification>(out Specification spec))
                {
                    data.specification = spec;
                }
            }



            RecordingEntityData.Add(data);
        }
        if (PlayerPrefs.GetInt("PlayFeedBack") == 0)
        {
            Recording = true;
        }
        else
        {
            Recording = false;
            Playing = true;
            PlayRecording(false);
        }
    }

    public void StopRecording()
    {
        Recording = false;
        FrameCounter = 0;
    }

    public void SaveRecording()
    {

    }

    public void BirdEye()
    {
        if (Playing)
        {
            BirdEyeView = !BirdEyeView;
            if (BirdEyeView)
            {
                //Switch point of view
                canvas1.SetActive(false);
                canvas2.SetActive(true);
                Camera1.SetActive(false);
                Camera2.SetActive(true);
                FeedBackCameraController.ThirdPerson = false;
            }
            else
            {
                //Switch point of view
                canvas1.SetActive(false);
                canvas2.SetActive(true);
                Camera1.SetActive(false);
                Camera2.SetActive(true);
                FeedBackCameraController.Target = Allys[ViewCraft];
                FeedBackCameraController.ThirdPerson = true;
            }
        }
    }

    public void SelectViewAircraft(float value)
    {
        if (Playing)
        {
            int RealAllysCount = Allys.Count-ExtraEntity.Count-Missiles.Count;
            if (value > 0)
            {
                ViewCraft++;
                ViewCraft = ViewCraft >= RealAllysCount ? RealAllysCount - 1 : ViewCraft;
            }
            else
            if (value < 0)
            { 
                ViewCraft--;
                ViewCraft = ViewCraft <= 0 ? 0 : ViewCraft;
            }
            EntityText.text = loadSystemAndSpawn.findCraftType(Allys[ViewCraft].gameObject);
            /*
            if (Allys[ViewCraft].TryGetComponent<CombineUttam>(out CombineUttam radar))
            {
                CurrentRadar = Allys[ViewCraft].GetComponentInChildren<AIRadar>();
                RadarScreen.enabled = false;
                if (RadarScreen.DummyTarget.parent != CurrentRadar.transform)
                {
                    RadarScreen.DummyTarget.parent = CurrentRadar.transform;
                    RadarScreen.Distance.text = (CurrentRadar.Range / 1852f).ToString("0");
                    RadarScreen.VisibleDistance.text = (CurrentRadar.Range / 1852f).ToString("0");
                }
            }
            else
            {
                CurrentRadar = null;
            }
            

            if (Allys[ViewCraft].TryGetComponent<PilotRadarIntegration>(out PilotRadarIntegration inte))
            {
                RadarScreen.enabled = true;
                RadarScreen.DummyTarget.parent = Radar.transform;
            }
            */
            FeedBackCameraController.Target = Allys[ViewCraft];
        } 
    }

    public void BirdEye(bool val)
    {
        BirdEyeView = val;
        if (BirdEyeView)
        {
            //Switch point of view
            canvas1.SetActive(false);
            canvas2.SetActive(false);
            Camera1.SetActive(false);
            Camera2.SetActive(true);
        }
        else
        {
            //Switch point of view
            canvas1.SetActive(true);
            canvas2.SetActive(true);
            Camera1.SetActive(true);
            Camera2.SetActive(false);
        }

    }

    


    public void shutdownScripts()
    {
        RadarScreen.enabled = false;
        Radar.enabled = false;
        rwr.enabled = false;
        extrasIntegration.enabled = false;
        canvas1.SetActive(false);
        extrasIntegration.gameObject.SetActive(false);
        foreach (Transform m in Missiles)
        {
            m.gameObject.SetActive(false);
        }
        foreach(GameObject o in otherUI)
        {
            o.SetActive(false);
        }
    }

    public void PlayRecording(bool AfterRecord = true)
    {


        if (AfterRecord)
        {

            DateTime localTime = DateTime.Now;

            // Get current UTC time 
            DateTime utcTime = DateTime.UtcNow;
            string path = DatabasePath;
            string TimeStamp = localTime.ToString("MM_dd_yyyy h_mm tt");
            //Database Folder
            path += "CGF_Mission_Feedback/";
            //Scenario name 
            path += missionPlan.missionInfo.Name;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }
            path += "/Frame " + (RecordingEntityData[0].position.Count - 1).ToString() + "_" + TimeStamp;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }
            SaveMission(missionPlan.missionInfo.Name, missionPlan, path);
            String JsonData = JsonHelper.ToJson<EntityData>(RecordingEntityData);

            string JsonfilePath = Path.Combine(path, "JSON_Data_" + TimeStamp + ".json");
            File.WriteAllText(JsonfilePath, JsonData);

            // Create a file path to save the file
            string filePath = Path.Combine(path, "Event_Log_" + TimeStamp + ".csv");
            StringBuilder log = new StringBuilder();
            foreach (string str in info.Allevents)
            {
                StringBuilder s = new StringBuilder(str);
                s.Replace("Enemy", "Red")
                .Replace("Enemy", "Red")
                .Replace("Ally", "Blue")
                .Replace("Ally", "Blue")
                .Replace("Player", "Player")
                .Replace("Player", "Player")
                .Replace("Sam", "Sam")
                .Replace("Warship", "Ship")
                .Replace("towards", " -> ");

                log.Append(s).Append("\n");
            }
            // Write some text to the file
            File.WriteAllText(filePath, log.ToString());

            // Output the file path to the console
            Debug.Log("File saved at: " + filePath);

            // Open the file in Notepad
            //OpenFileInNotepad(filePath);
            if (true)
            {

                int entity = Allys.Count - ExtraEntity.Count - Missiles.Count;
                for (int h = 0; h < entity; h++)
                {
                    StringBuilder s = new StringBuilder();
                    s.Append("x,y,z,rotX,rotY,rotZ,active,");
                    if (Allys[h].name.Contains("TejasLca"))
                    {
                        s.Append("Lat,Long,Altitude,Speed,WaypointNumber,WaypointDistance,WaypointBearing,TimeOfArrival,IndicatdedSpeed,TrueSpeed,Acceleration,GForce,PitchRate,RollRate,AirDensity,Temerature,Pressure,VerticalSpeed,Heading,Pitch,Roll,Mach,")
                        .Append("MainMode,SubMode,AcmModes,Gain,Azimuth,RangeScale,Bar,PtOnly,AAST,TxSil,PassiveMode,ABDutyCycle,HILo,BScope,SAR,ISAR,RBM,GACM,Cit,WA,Jem,Contrast,TRM,TRMPOWER,G,R,F,T,N,NR,Range,Iff,Elevation,MinimumDetectSize,VelocityThreshold,PulseTime,peactimeWartimeAccuracy,Frequency,Rmax,Rmax2,Rmain2,Aj,JemNoise,JemAngle,Info,Detect,")
                        .Append("EWRange,Flares,Chaffs,ChaffsBurstSize,FlareBurstSize,AutoFlare,AutoChaff,AutoFlareActiveRange,AutoChaffActiveRange,Pt,Gt,F,Gr,Pr,MissileWarning,Detect,");
                    }
                    else
                    if (Allys[h].tag.Contains("Player") || Allys[h].tag.Contains("EnemyPlane"))
                    {
                        s.Append("Lat,Long,Altitude,Speed,Pitch,Roll,Heading,")
                        .Append("RadarRange,Azimuth,Bar,F,G,Info,Detect,Lock,Maneuers,Detect")
                        .Append("EWRange,Flares,Chaffs,ChaffsBurstSize,FlareBurstSize,AutoFlare,AutoChaff,AutoFlareActiveRange,AutoChaffActiveRange,Pt,Gt,F,Gr,Pr,MissileWarning,Detect,");
                    }

                    s.Append("\n");
                    int j = 0;
                    foreach (Vector3 pos in RecordingEntityData[h].position)
                    {

                        Vector3 rot = RecordingEntityData[h].rotation[j];
                        s.Append(pos.x).Append(",").Append(pos.y).Append(",").Append(pos.z).Append(",").Append(rot.x).Append(",").Append(rot.y).Append(",").Append(rot.z).Append(",").Append(RecordingEntityData[h].active[j]);
                        //Specification
                        if (RecordingEntityData[h].specification)
                        {
                            EntityInfoLog entityLog = RecordingEntityData[h].entityInfoLog[j];
                            if (RecordingEntityData[h].specification.entityType == Specification.EntityType.Player)
                            {
                                s.Append(",").Append(entityLog.Lat.ToString())
                                .Append(",").Append(entityLog.Long.ToString())
                                .Append(",").Append(entityLog.Altitude.ToString())
                                .Append(",").Append(entityLog.WaypointNumber.ToString())
                                .Append(",").Append(entityLog.WaypointDistance.ToString())
                                .Append(",").Append(entityLog.WaypointBearing.ToString())
                                .Append(",").Append(entityLog.TimeOfArrival.ToString())
                                .Append(",").Append(entityLog.Speed.ToString())
                                .Append(",").Append(entityLog.IndicatedSpeed.ToString())
                                .Append(",").Append(entityLog.TrueSpeed.ToString())
                                .Append(",").Append(entityLog.Acceleration.ToString()) 
                                .Append(",").Append(entityLog.GForce.ToString())
                                .Append(",").Append(entityLog.PitchRate.ToString())
                                .Append(",").Append(entityLog.RollRate.ToString())
                                .Append(",").Append(entityLog.AirDensity.ToString())
                                .Append(",").Append(entityLog.Temperature.ToString())
                                .Append(",").Append(entityLog.Pressure.ToString())
                                .Append(",").Append(entityLog.VerticleVelocity.ToString())
                                .Append(",").Append(entityLog.Heading.ToString())
                                .Append(",").Append(entityLog.Pitch.ToString())
                                .Append(",").Append(entityLog.Roll.ToString())
                                .Append(",").Append(entityLog.Mach.ToString());
                            }
                            if (RecordingEntityData[h].specification.entityType == Specification.EntityType.AI)
                            {
                                s.Append(",").Append(entityLog.Lat.ToString())
                                .Append(",").Append(entityLog.Long.ToString())
                                .Append(",").Append(entityLog.Altitude.ToString())
                                .Append(",").Append(entityLog.Speed.ToString())
                                .Append(",").Append(entityLog.Heading.ToString())
                                .Append(",").Append(entityLog.Pitch.ToString())
                                .Append(",").Append(entityLog.Roll.ToString());
                            }
                        }
                        //UttamRadar
                        if (RecordingEntityData[h].radarLog.Count > 0)
                        {
                            RadarLog radarLog = RecordingEntityData[h].radarLog[j];
                            s.Append(",").Append(radarLog.MainMode.ToString())
                            .Append(",").Append(radarLog.SubMode.ToString())
                            .Append(",").Append(radarLog.ACMModes.ToString())
                            .Append(",").Append(radarLog.gain.ToString())
                            .Append(",").Append(radarLog.Azimuth.ToString())
                            .Append(",").Append(radarLog.RangeScale.ToString())
                            .Append(",").Append(radarLog.Bar.ToString())
                            .Append(",").Append(radarLog.PtOnly.ToString())
                            .Append(",").Append(radarLog.AAST.ToString())
                            .Append(",").Append(radarLog.TxSil.ToString())
                            .Append(",").Append(radarLog.PassiveMode.ToString())
                            .Append(",").Append(radarLog.ABDutyCycle.ToString())
                            .Append(",").Append(radarLog.HILo.ToString())
                            .Append(",").Append(radarLog.BScope.ToString())
                            .Append(",").Append(radarLog.SAR.ToString())
                            .Append(",").Append(radarLog.ISAR.ToString())
                            .Append(",").Append(radarLog.RBM.ToString())
                            .Append(",").Append(radarLog.GACM.ToString())
                            .Append(",").Append(radarLog.Cit.ToString())
                            .Append(",").Append(radarLog.WA.ToString())
                            .Append(",").Append(radarLog.Jem.ToString())
                            .Append(",").Append(radarLog.contrast.ToString())
                            .Append(",").Append(radarLog.TRM.ToString())
                            .Append(",").Append(radarLog.TRMPOWER.ToString())
                            .Append(",").Append(radarLog.G.ToString())
                            .Append(",").Append(radarLog.r.ToString())
                            .Append(",").Append(radarLog.f.ToString())
                            .Append(",").Append(radarLog.T.ToString())
                            .Append(",").Append(radarLog.N.ToString())
                            .Append(",").Append(radarLog.NR.ToString())
                            .Append(",").Append(radarLog.Range.ToString())
                            .Append(",").Append(radarLog.iff.ToString())
                            .Append(",").Append(radarLog.Elevation.ToString())
                            .Append(",").Append(radarLog.MinimumDetectSize.ToString())
                            .Append(",").Append(radarLog.VelocityThreshold.ToString())
                            .Append(",").Append(radarLog.pulseTime.ToString())
                            .Append(",").Append(radarLog.peactimeWartimeAccuracy.ToString())
                            .Append(",").Append(radarLog.frequency.ToString())
                            .Append(",").Append(radarLog.RMax.ToString())
                            .Append(",").Append(radarLog.RMax2.ToString())
                            .Append(",").Append(radarLog.RMin2.ToString())
                            .Append(",").Append(radarLog.Aj.ToString())
                            .Append(",").Append(radarLog.jemNoise.ToString())
                            .Append(",").Append(radarLog.jemAngle.ToString());

                            int l = 0;
                            foreach (Vector4 info in radarLog.Info)
                            {
                                s.Append(",{" + info.x + ":" + info.y + ":" + info.z + ":" + info.w + "}{" + radarLog.detect[l].x + "," + radarLog.detect[l].y + "," + radarLog.detect[l].z + "}");
                                l++;
                            }
                        }
                        //AiRadar
                        if (RecordingEntityData[h].aIRadar)
                        {
                            AIRadarLog aiRadarLog = RecordingEntityData[h].aiRadarLog[j];
                            s.Append(",").Append(aiRadarLog.Range.ToString())
                            .Append(",").Append(aiRadarLog.Azimuth.ToString())
                            .Append(",").Append(aiRadarLog.Bar.ToString())
                            .Append(",").Append(aiRadarLog.F.ToString())
                            .Append(",").Append(aiRadarLog.G.ToString())
                            .Append(",").Append(aiRadarLog.Maneuvers.ToString());
                            int l = 0;
                            foreach (Vector4 info in aiRadarLog.info)
                            {
                                s.Append(",{" + info.x + ":" + info.y + ":" + info.z + ":" + info.w + "}{" + aiRadarLog.detect[l].x + "," + aiRadarLog.detect[l].y + "," + aiRadarLog.detect[l].z + "}{" + aiRadarLog.locks[l] + "}");
                                l++;
                            }
                        }
                        //EwRadar
                        if (RecordingEntityData[h].eWRadar)
                        {
                            EWRadarLog ewRadarLog = RecordingEntityData[h].ewRadarLog[j];
                            s.Append(",").Append(ewRadarLog.Radar_Range.ToString())
                            .Append(",").Append(ewRadarLog.Flares.ToString())
                            .Append(",").Append(ewRadarLog.Chaffs.ToString())
                            .Append(",").Append(ewRadarLog.chaffBurstSize.ToString())
                            .Append(",").Append(ewRadarLog.flareBurstSize.ToString())
                            .Append(",").Append(ewRadarLog.autoFlare.ToString())
                            .Append(",").Append(ewRadarLog.autoChaff.ToString())
                            .Append(",").Append(ewRadarLog.AutoFlareActiveRange.ToString())
                            .Append(",").Append(ewRadarLog.AutoChaffActiveRange.ToString())
                            .Append(",").Append(ewRadarLog.Pt.ToString())
                            .Append(",").Append(ewRadarLog.Gt.ToString())
                            .Append(",").Append(ewRadarLog.f.ToString())
                            .Append(",").Append(ewRadarLog.Gr.ToString())
                            .Append(",").Append(ewRadarLog.Pr.ToString())
                            .Append(",").Append(ewRadarLog.MissileWarning.ToString());

                            int l = 0;
                            foreach (Vector4 info in ewRadarLog.detect)
                            {
                                s.Append(",{" + ewRadarLog.detect[l].x + "," + ewRadarLog.detect[l].y + "," + ewRadarLog.detect[l].z + "}");
                                l++;
                            }
                        }

                        s.Append("\n");
                        j++;
                    }
                    filePath = Path.Combine(path, loadSystemAndSpawn.findCraftType(Allys[h].gameObject) + "_" + TimeStamp + ".csv");
                    // Write some text to the file
                    File.WriteAllText(filePath, s.ToString());

                    // Output the file path to the console
                    Debug.Log("File saved at: " + filePath);
                }
            }
        }
        else
        {
            path = PlayerPrefs.GetString("FeedBackPath").Replace("\\", "/");
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (file.Contains(".json"))
                {
                    string jsonFile = File.ReadAllText(file.Replace("\\", "/"));
                    RecordingEntityData = JsonHelper.FromJson<EntityData>(jsonFile);

                    int s = 0;
                    foreach(EntityData entityData in RecordingEntityData)
                    {
                        if (Allys[s].TryGetComponent<Specification>(out Specification spec))
                        {
                            entityData.specification = spec;
                        }
                        s++;
                    }
                }
            }

        }



        shutdownScripts();
        BirdEye(false);
        BirdEye();
        BirdEye();
        SelectViewAircraft(1);
        SelectViewAircraft(-1);
        FeedBackCameraController.Target = Allys[ViewCraft];
        Playing = true;
        PauseRecording(false);
        slider.value = 0;
        FrameCounter = 0;
        FeedbackUI.SetActive(true);
        MaxFrames = RecordingEntityData[0].position.Count-1;
        slider.maxValue = MaxFrames;
        TextMaxFrame.text = "/"+MaxFrames.ToString();
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("explosion");
        foreach(GameObject explosion in explosions)
        {
            explosion.SetActive(false);
        }
        SilantroGun[] guns = GameObject.FindObjectsOfType<SilantroGun>();
        foreach (SilantroGun gun in guns)
        {
            gun.canFire = false;
            gun.Override = true;
            gun.enabled = false;
            gun.GunEmpty();
        }


        int i = 0;
        //Make All Rigidbody is kinematic
        foreach (Transform craft in Allys)
        {
            if(craft.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = true;
            }
            if (craft.TryGetComponent<PlaneContole>(out PlaneContole plane))
            {
                plane.enabled = false;
            }
            if (craft.TryGetComponent<Vazgriz.Plane.Plane>(out Vazgriz.Plane.Plane planeV))
            {
                planeV.enabled = false;
            }
            if (craft.TryGetComponent<AeroplaneController>(out AeroplaneController controller))
            {
                controller.enabled = false;
            }
            if (craft.TryGetComponent<AeroplaneAiControl>(out AeroplaneAiControl aiController))
            {
                aiController.enabled = false;
            }
            if (craft.TryGetComponent<CombineUttam>(out CombineUttam combineUttam))
            {
                combineUttam.enabled = false;
                combineUttam.Target = null;
                //combineUttam.maxMissile = 0;
                combineUttam.count = 0;
            }
            if (craft.TryGetComponent<missileEngine>(out missileEngine missileEng))
            {
                missileEng.enabled = false;
            }
            if (craft.TryGetComponent<missileNavigation>(out missileNavigation missilenavig))
            {
                missilenavig.enabled = false;
            }
            if (craft.TryGetComponent<MissileHeatSystem>(out MissileHeatSystem missileheat))
            {
                missileheat.enabled = false;
            }
            if (craft.TryGetComponent<CapsuleCollider>(out CapsuleCollider collider))
            {
                collider.enabled = false;
            }

            EWRadar eWRadar = craft.GetComponentInChildren<EWRadar>();
            if(eWRadar != null)
            {
                eWRadar.autoChaffFire = false;
                eWRadar.autoFlareFire = false;
                eWRadar.enabled = false;
            }

            if (craft.TryGetComponent<HitDestroy>(out HitDestroy hit))
            {
                hit.enabled = false;
            }
            if (craft.childCount > 0)
            {
                if (craft.GetChild(0).TryGetComponent<GroundRadar>(out GroundRadar radar))
                {
                    radar.enabled = false;
                }
                if (craft.GetChild(0).TryGetComponent<CarrierShipController>(out CarrierShipController shipController))
                {
                    shipController.enabled = false;
                }
            }

            //waypoint 
            gameObject.AddComponent<LineRenderer>();
            LineRenderer lineRendererr = gameObject.GetComponent<LineRenderer>();
            lineRendererr.enabled = true;
            lineRendererr.startWidth = lineRendererr.endWidth = 20;
            lineRendererr.positionCount = missionPlan.waypoints.Count;
            lineRendererr.sharedMaterial = waypoint;
            Vector3[] positons = new Vector3[lineRendererr.positionCount];
            int s = 0;
            foreach (Cordinates pos in missionPlan.waypoints)
            {
                Vector3 v = pos.position;
                v.y = 1000;
                positons[s] = v;
                
                s++;
            }
            lineRendererr.SetPositions(positons);

            if (craft.tag.Contains("Player") || craft.tag.Contains("EnemyPlane") || craft.tag.Contains("Enemy") || craft.tag.ToLower().Contains("missile") || craft.tag.ToLower().Contains("bomb")) {
                craft.AddComponent<LineRenderer>();
                LineRenderer lineRenderer = craft.GetComponent<LineRenderer>();
                lineRenderer.startWidth = lineRenderer.endWidth = 20;
                lineRenderer.positionCount = RecordingEntityData[i].position.Count;
                lineRenderers.Add(lineRenderer);
                Vector3[] positon = new Vector3[lineRenderer.positionCount];
                int p = 0;
                Vector3 startPos = Vector3.zero;
                bool st = false;
                int index = 0;
                foreach (Vector3 pos in RecordingEntityData[i].position)
                {
                    if (!st && RecordingEntityData[i].active[p])
                    {
                        st = true;
                        startPos = pos;
                        index = p;
                    }
                    positon[p] = pos;
                    p++;
                }
                
                lineRenderer.enabled = true;
                lineRenderer.sharedMaterial = MainAircrafts;
                if (craft.name.Contains("TejasLca"))
                {
                    lineRenderer.sharedMaterial = MainAircrafts;
                }
                else
                if (craft.tag.Contains("Player"))
                {
                    lineRenderer.sharedMaterial = AllyAircraft;
                } else
                if (craft.tag.Contains("EnemyPlane"))
                {
                    lineRenderer.sharedMaterial = EnemyAircraft;
                }
                else
                if (craft.tag.ToLower().Contains("missile"))
                {
                    for(int k=0;k<index;k++)
                    {   
                        positon[k] = startPos;
                    }

                    lineRenderer.sharedMaterial = Missile;
                    lineRenderer.startWidth = lineRenderer.endWidth = 20;
                }
                else
                if (craft.tag.ToLower().Contains("bomb"))
                {
                    lineRenderer.sharedMaterial = Bomb;
                    lineRenderer.startWidth = lineRenderer.endWidth = 20;
                }

                lineRenderer.SetPositions(positon);
            }


            i++;
        }

    }

    public void SaveMission(string missionName, MissionPlan scriptableObject, string path, bool Override = false)
    {
        DateTime localTime = DateTime.Now;

        // Get current UTC time
        DateTime utcTime = DateTime.UtcNow;

        string TimeStamp = localTime.ToString("MM_dd_yyyy h_mm tt");

        string filePath = path + "/" + missionName;// +"@"+TimeStamp;
        if (!Directory.Exists(filePath) || Override)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            //File.CreateText(filePath);
            string MissionInfoData = JsonUtility.ToJson(scriptableObject.missionInfo);
            File.WriteAllText(filePath + "/info", MissionInfoData);
            string MissionDataAlly = JsonHelper.ToJson<AircraftPlanData>(scriptableObject.ally_spawnPlanes);
            File.WriteAllText(filePath + "/ally", MissionDataAlly);
            string MissionDataNeutral = JsonHelper.ToJson<AircraftPlanData>(scriptableObject.neutral_spawnPlanes);
            File.WriteAllText(filePath + "/neutral", MissionDataNeutral);
            string MissionDataAdversary = JsonHelper.ToJson<AircraftPlanData>(scriptableObject.adversary_spawnPlanes);
            File.WriteAllText(filePath + "/adversary", MissionDataAdversary);
            string MissionDataSams = JsonHelper.ToJson<AircraftPlanData>(scriptableObject.sams_spawnPlanes);
            File.WriteAllText(filePath + "/sams", MissionDataSams);
            string MissionDataWarship = JsonHelper.ToJson<AircraftPlanData>(scriptableObject.Warship_spawnPlanes);
            File.WriteAllText(filePath + "/warship", MissionDataWarship);
            string MissionDataWaypoints = JsonHelper.ToJson<Cordinates>(scriptableObject.waypoints);
            File.WriteAllText(filePath + "/waypoints", MissionDataWaypoints);
            string WeatherData = JsonUtility.ToJson(scriptableObject.WeatherData);
            File.WriteAllText(filePath + "/weather", WeatherData);

        }
        else
        {
            Debug.LogError(filePath + " already exist");
        }
    }
    public void PauseRecording()
    {
        PauseToggle.isOn = !PauseToggle.isOn;
        resume = PauseToggle.isOn;
    }

    public void PauseRecording(bool val)
    {
        PauseToggle.isOn = val;
        resume = val;
    }

    
    public void Seeking(float value)
    {
        if (seeking)
        {
            FrameCounter = (int)value;
            frameSeeking = (int)value;
            frameINput.text = value.ToString();
        }
    }
    public void Seeking(string text)
    {
        if (seeking)
        {
            int value = (int)float.Parse(text);
            if (value >= MaxFrames || value < 0)
            {
                value = FrameCounter;
            }
            FrameCounter = (int)value;
            frameSeeking = (int)value;
        }
    }

    public void isSeeking(bool value)
    {
        seeking = value;
        if (!seeking)
        {
            FrameCounter = (int)frameSeeking;
        }
    }


    


    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(List<T> array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(List<T> array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }
    }
}
