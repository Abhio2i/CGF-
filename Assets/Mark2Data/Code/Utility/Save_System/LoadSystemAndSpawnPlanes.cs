using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using newSaveSystem;
using Enemy.Formation;
using Assets.Scripts.Feed;
using Assets.Scripts.Feed_Scene.newFeed;
using TMPro;

#region Script info
//Input data from menu Selection Scene waypoints,no of ally/enemy/neutral planes 
//Output spawn GameObjects
#endregion
public class LoadSystemAndSpawnPlanes : MonoBehaviour
{
    #region SerializeField
    [SerializeField] List<GameObject> enemyPrefabs, neutralPrefabs, allyPrefabs, aiPrefabs,samsPrefabs, warshipPrefabs;
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] MasterSpawn masterSpawn;
    [SerializeField] MissionPlan missionPlan;
    [SerializeField] AIPlaneData Ai;
    [SerializeField] SpawnPlanesData Ally;
    [SerializeField] SpawnPlanesData Neutral;
    [SerializeField] SpawnPlanesData Enemy;
    [SerializeField] SaveDataManager SaveDataManager;

    [SerializeField] GameObject smokePrefab;
    [SerializeField] List<GameObject> waypoint;
    #endregion
    #region public variables
    public static LoadSystemAndSpawnPlanes instance;
    
    public SilantroStarter MainPlaneStarter;
    public UttamRadar radar;
    public EWRadar ewRadar;
    public WaypointsViewer waypointsViewer;
    public List<GameObject> AllyPlanes;
    public List<GameObject> NeutralPlanes;
    public List<GameObject> AdversaryPlanes;
    public List<GameObject> Sams;
    public List<GameObject> Warships;

    public GameObject AiPng;
    public GameObject AllyPng;
    public GameObject NeutralPng;
    public GameObject EnemyPng;
    public GameObject explosion;
    public GameObject bullet;
    public GameObject missile;
    public GameObject flare;
    public GameObject allyMissile;
    public GameObject enemyMissile;
    public GameObject FormationManager;
    public GameObject explosionPrefab;

    public GameObject newAllyObject;
    public GameObject newEnemyObject;
    //public NNModel newBrain;


    public string[] names;
    public string myName;
    public string enemyTag;
    public string enemyMissileTag;
    public string playerTag;
    public string playerMissileTag;
    public string allyTag;
    public string allyMissileTag;
    public string neutralTag;
    public string allyAvoidTag;

    public float allySpeed;
    public float neutralSpeed;
    public float adversarySpeed;
    public float aiSpeed;
    public float playerSpeed;

    public bool isDone;

    public int spawnedPlanesCount;
    public int progress;
    #endregion

    public TextMeshProUGUI EnemysLeft;
    public TextMeshProUGUI AllysLeft;

    #region private variables
    List<GameObject> selectedPrefab;

    string[] arr = { "Ally", "Adversary", "Neutral", "AI" };

    public int progressCountEnemy = 0;
    private int progressCountAlly = 0;
    private int progressCountNeutral = 0;
    private int progressCountAI = 0;
    private Rigidbody rigidbody;
    List<GameObject> _tempArray = new List<GameObject>();

    public string[] AllyspawnFiles;
    public string[] AdversaryspawnFiles;
    public string[] NeutralspawnFiles;
    public GameObject BangloreMap;
    public GameObject SeaMap;

    #endregion
    private void Awake()
    {
        //AllyspawnFiles = SavingSystem.LoadFile("/Ally");
        //AdversaryspawnFiles = SavingSystem.LoadFile("/Adversary");
        //NeutralspawnFiles = SavingSystem.LoadFile("/Neutral");
    }
    void Start()
    {
        //SpawnSystem();
        Invoke(nameof(NewAllSpawn), 1);
    }

    public string findCraftType(GameObject craft)
    {
        if (craft == null)
            return "";
        craft = craft.transform.root.gameObject;
        if(craft == MainPlaneStarter.gameObject)
        {
            return "Player"; 
        }

        int i = 0;
        foreach(var obj in AllyPlanes) 
        {
            if(obj == craft)
            {
                return "Ally_" + (i+1);
            }
            i++;
        }

        i = 0;
        foreach (var obj in AdversaryPlanes)
        {
            if (obj == craft)
            {
                return "Enemy_" + (i + 0);
            }
            i++;
        }
        i = 0;
        foreach (var obj in NeutralPlanes)
        {
            if (obj == craft)
            {
                return "Neutral_" + (i + 0);
            }
            i++;
        }
        i = 0;
        foreach (var obj in Warships)
        {
            if (obj == craft)
            {
                return "warship_" + (i + 0);
            }
            i++;
        }
        i = 0;
        foreach (var obj in Sams)
        {
            if (obj == craft)
            {
                return "sam_" + (i + 0);
            }
            i++;
        }

        return "";
    }

    private void Update()
    {
        int count = 0;
        foreach (GameObject obj in AllyPlanes)
        {
            if (obj.activeSelf)
            {
                count++;
            }
        }
        AllysLeft.text = "AllysLeft : "+count;
        count = 0;
        foreach (GameObject obj in AdversaryPlanes)
        {
            if (obj.activeSelf)
            {
                count++;
            }
        }
        EnemysLeft.text = "EnemysLeft : "+count;
    }

    #region oldcode
    void SpawnSystem()
    {
        foreach (string file in AllyspawnFiles)
        {
            Ally = SavingSystem.PlanesData(file);
            Spawn("Ally", "AllySpawnCount", "ally not spawn", 1, "AllyCount");
        }

        foreach (string file in AdversaryspawnFiles)
        {
            Enemy = SavingSystem.PlanesData(file);
            Spawn("Adversary", "AdversarySpawnCount", "adversary not spawn", 2, "EnemyCount");
        }
        foreach (string file in NeutralspawnFiles)
        {
            Neutral = SavingSystem.PlanesData(file);
            Spawn("Neutral", "NeutralSpawnCount", "neutral not spawn", 3, "NeutralCount");
        }
        PlayerPrefs.SetInt("Progress", 4);
        PlayerPrefs.SetInt("AiCount", progressCountAI);
    }
    void Spawn(string type, string typeSpawnCount, string emptyMessage, int incerement, string playerPrefsTag)
    {
        if (PlayerPrefs.GetInt(typeSpawnCount) > 0)
        {
            try
            {

                //if (type == "Ally")
                //{
                //    Ally_SpawnPlanesScriptObject(Ally.model, allyPrefabs, int.Parse(Ally.count), Ally.skill,
                //    Ally.spawnPosition, Ally.callSignCode);
                //    progressCountAlly = int.Parse(Ally.count);
                //}
                //else if (type =="Neutral")
                //{
                //   // Neutral_SpawnPlanesScriptObject(Neutral.model, neutralPrefabs, int.Parse(Neutral.count), Neutral.skill,
                //   //Neutral.spawnPosition, Neutral.callSignCode);
                //   // progressCountNeutral = int.Parse(Neutral.count);
                //}
                //else if(type =="Adversary")
                //{
                //    Adversary_SpawnPlanesScriptObject(Enemy.model, enemyPrefabs, int.Parse(Enemy.count), Enemy.skill,
                //    Enemy.spawnPosition, Enemy.callSignCode, Enemy.formationType, /*Enemy.aroundPoint*/ Enemy.playerChase);
                //progressCountEnemy = int.Parse(Enemy.count);
                //}

            }
            catch
            {
                Debug.Log(emptyMessage);
            }
        }
        PlayerPrefs.SetInt("Progress", incerement); //to increment loading screen bar
        PlayerPrefs.SetInt(playerPrefsTag, progressCountAlly); //to increment loading screen bar
        /*
        
        if (PlayerPrefs.GetInt("NeutralSpawnCount") > 0&& type == "Neutral")
        {
            try
            {
                
                
               
                
            }
            catch
            {
                Debug.Log("Neutral not spawn");
            }

        }
        PlayerPrefs.SetInt("Progress", 2);
        PlayerPrefs.SetInt("NeutralCount", progressCountNeutral);

        if (PlayerPrefs.GetInt("AdversarySpawnCount") > 0 && type == "Adversary")
        {
            try
            {
                
                
                
                
            }
            catch
            {
                Debug.Log("Enemy not spawn");
            }
        }
        PlayerPrefs.SetInt("Progress", 3);
        PlayerPrefs.SetInt("EnemyCount", progressCountEnemy);
        if (PlayerPrefs.GetInt("AiSpawnCount") > 0)
        {
            print(PlayerPrefs.GetInt("AiSpawnCount"));
            try
            {
                
                
                Ai_SpawnPlanesScriptObject(Ai.model, aiPrefabs, int.Parse(Ai.count),Ai.spawnPositionsAtX, Ai.spawnPositionsAtY, Ai.spawnPositionsAtZ, Ai.wayPointsPos);
                progressCountAI = int.Parse(Ai.count);
                
            }
            catch
            {
                Debug.Log("Ai not spawn");
            }
        }
        PlayerPrefs.SetInt("Progress", 4);
        PlayerPrefs.SetInt("AiCount", progressCountAI);
        */
    }
    #endregion

    #region NewestFuntion
    public void NewAllSpawn()
    {
        if(missionPlan.missionInfo.scenario == MissionInfo.Scenario.ATS)
        {
            SeaMap.SetActive(true);
            BangloreMap.SetActive(false);
        }
        else
        {
            SeaMap.SetActive(false);
            BangloreMap.SetActive(true);
        }

        //Spawn position Rotation Config for MainPlayer
        if (missionPlan.ally_spawnPlanes.Count > 0)
        {
            if (missionPlan.ally_spawnPlanes[0].startMode == AircraftPlanData.StartMode.Hot)
            {
                MainPlaneStarter.transform.position = new Vector3(missionPlan.ally_spawnPlanes[0].spawnPosition.x, missionPlan.ally_spawnPlanes[0].spawnPosition.y + 100, missionPlan.ally_spawnPlanes[0].spawnPosition.z);
                MainPlaneStarter.transform.localEulerAngles = new Vector3(0, -missionPlan.ally_spawnPlanes[0].Heading, 0);
                MainPlaneStarter.GetComponent<SilantroController>().startAltitude = missionPlan.ally_spawnPlanes[0].spawnPosition.y + 100;
                MainPlaneStarter.GetComponent<SilantroController>().startSpeed = missionPlan.ally_spawnPlanes[0].speed;
                MainPlaneStarter.startByScript();
            }
            else
            {
                MainPlaneStarter.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else
        {
            MainPlaneStarter.GetComponent<Rigidbody>().isKinematic = false;
        }

        //Add in Feedback;
        FeedBackRecorderAndPlayer.Allys.Add(MainPlaneStarter.transform);
        //
        radar.TRMPower = missionPlan.ally_spawnPlanes[0].Power/radar.TRM;
        radar.G =(int) missionPlan.ally_spawnPlanes[0].Gain;
        radar.f = missionPlan.ally_spawnPlanes[0].Frequency;
        radar.T = missionPlan.ally_spawnPlanes[0].Temp;
        radar.N = missionPlan.ally_spawnPlanes[0].NoiseFactor;
        radar.NR = missionPlan.ally_spawnPlanes[0].MinimalNoise;
        radar.r = missionPlan.ally_spawnPlanes[0].TRCS;
        radar.MinimumDetectSize = missionPlan.ally_spawnPlanes[0].TRCS;

        ewRadar.Pt = missionPlan.ally_spawnPlanes[0].EWPower;
        ewRadar.Gt = missionPlan.ally_spawnPlanes[0].EWGain;
        ewRadar.Gr = missionPlan.ally_spawnPlanes[0].RecieveGain;
        ewRadar.Pr = missionPlan.ally_spawnPlanes[0].Threshold;
        ewRadar.radar_Range = ewRadar.getRange();

        var jam = MainPlaneStarter.GetComponentInChildren<Jammer>();
        jam.JammerActive = missionPlan.ally_spawnPlanes[0].Jammer;
        jam.Pt = missionPlan.ally_spawnPlanes[0].TransmittedPower;
        jam.Gr = missionPlan.ally_spawnPlanes[0].RadarReceiverGain;
        jam.F = missionPlan.ally_spawnPlanes[0].RadarFrequency;
        jam.σ = missionPlan.ally_spawnPlanes[0].TargetCrossSection;
        jam.D = missionPlan.ally_spawnPlanes[0].TargetDistance;
        jam.Dj = missionPlan.ally_spawnPlanes[0].TargetDistance / 2;
        jam.Pj = missionPlan.ally_spawnPlanes[0].JammerTransPower;
        jam.Gj = missionPlan.ally_spawnPlanes[0].JammerReceiverGain;
        jam.Fj = missionPlan.ally_spawnPlanes[0].JammerTransFreq;
        jam.Grj = missionPlan.ally_spawnPlanes[0].TargetReceiJamSignalGain;

        ///






        //Ally Planes
        for (int i = 1; i < missionPlan.ally_spawnPlanes.Count; i++)
        {
            int j = 0;
            foreach (GameObject prefab in allyPrefabs)
            {
                if (prefab.name.ToLower() == missionPlan.ally_spawnPlanes[i].name.ToLower()) { break; }
                j++;
            }
            var ally = Instantiate(allyPrefabs[j]);
            ally.transform.position = new Vector3(missionPlan.ally_spawnPlanes[i].spawnPosition.x, missionPlan.ally_spawnPlanes[i].spawnPosition.y+100, missionPlan.ally_spawnPlanes[i].spawnPosition.z);
            ally.transform.localEulerAngles = new Vector3(0, -missionPlan.ally_spawnPlanes[i].Heading, 0);

            //Add in Feedback;
            FeedBackRecorderAndPlayer.Allys.Add(ally.transform);
            //AI Radar
            AIRadar radar = ally.GetComponentInChildren<AIRadar>();
            AircraftPlanData data = missionPlan.ally_spawnPlanes[i];
            //float mile = 1852f;
            float G = data.Gain;
            float TRM = 763;
            float power = data.Power / TRM;
            float applyPower = 1;
            float r = data.TRCS;// radar aperture radius
            float NR = data.MinimalNoise;
            float N = data.NoiseFactor;
            float T = data.Temp; //Temprature in kelvin
            float bandwidth = data.Frequency * Mathf.Pow(10f, 9f);
            float lamda = (3 * Mathf.Pow(10, 8)) / bandwidth;
            float Pmin = (1.38f * Mathf.Pow(10, -23) * T * bandwidth * N * NR);//minimun detectable signal
            float gamma = Mathf.PI * r * r;////Target Radar Cross Section/ radar aperture
            float Pt = power * TRM * applyPower;//total consume power
            float Coverage = Mathf.Sqrt(Mathf.Sqrt((float)((Pt * Mathf.Pow(G, 2) * Mathf.Pow(lamda, 2) * Mathf.Pow(gamma, 2)) / (Mathf.Pow(4f * Mathf.PI, 3) * Pmin))));
            //Coverage = Mathf.Round(Coverage / (mile / 1000));
            radar.Range = Coverage * 1000;
            radar.G = (int)data.Gain;
            radar.f = (int)data.Frequency;
            radar.power = data.Power;
            radar.aperture = data.TRCS;
            

            //EW Config
            //ally.GetComponent<Specification>().iff = masterSpawn.ally_spawnPlanes[i].callSignCode;
            var ew = ally.GetComponentInChildren<EWRadar>();
            ew.maxFlares = missionPlan.ally_spawnPlanes[i].Flares;
            bool autoflare = missionPlan.ally_spawnPlanes[i].AutoFlares;
            ew.autoFlareFire = autoflare;
            ew.maxChaff = missionPlan.ally_spawnPlanes[i].Chaffs;
            bool autochaff = missionPlan.ally_spawnPlanes[i].AutoChaffs;
            ew.autoChaffFire = autochaff;
            ew.Pt = missionPlan.ally_spawnPlanes[i].EWPower;
            ew.f = missionPlan.ally_spawnPlanes[i].EWFrequency;
            ew.Gt = missionPlan.ally_spawnPlanes[i].EWGain;
            ew.Gr = missionPlan.ally_spawnPlanes[i].RecieveGain;
            ew.Pr = missionPlan.ally_spawnPlanes[i].Threshold;
            ew.radar_Range = ew.getRange();

            var Jam = ally.GetComponentInChildren<Jammer>();
            Jam.JammerActive = missionPlan.ally_spawnPlanes[i].Jammer;
            Jam.Pt = missionPlan.ally_spawnPlanes[i].TransmittedPower;
            Jam.Gr = missionPlan.ally_spawnPlanes[i].RadarReceiverGain;
            Jam.F = missionPlan.ally_spawnPlanes[i].RadarFrequency;
            Jam.σ = missionPlan.ally_spawnPlanes[i].TargetCrossSection;
            Jam.D = missionPlan.ally_spawnPlanes[i].TargetDistance;
            Jam.Dj = missionPlan.ally_spawnPlanes[i].TargetDistance/2;
            Jam.Pj = missionPlan.ally_spawnPlanes[i].JammerTransPower;
            Jam.Gj = missionPlan.ally_spawnPlanes[i].JammerReceiverGain;
            Jam.Fj = missionPlan.ally_spawnPlanes[i].JammerTransFreq;
            Jam.Grj = missionPlan.ally_spawnPlanes[i].TargetReceiJamSignalGain;


            //ew.burstChaffSize = missionPlan.ally_spawnPlanes[i].chaffBurst;
            //ew.burstFlareSize = missionPlan.ally_spawnPlanes[i].flareBurst;

            //missiles
            CheckMissiles(i, ally);
            CombineUttam combineUttam  = ally.GetComponent<CombineUttam>();
            foreach (MissileInfo info in missionPlan.ally_spawnPlanes[i].missileInfo)
            {
                MissileInfo inf = new MissileInfo(info);
                combineUttam.missileInfos.Add(inf);
            }



            
            waypointsViewer.Allys.Add(ally.transform);
            AllyPlanes.Add(ally);
            ally.GetComponent<Rigidbody>().velocity = Vector3.zero;
           
            int parsedValue;

            if (int.TryParse(missionPlan.ally_spawnPlanes[i].callSignCode, out parsedValue))
            {
                ally.GetComponent<Specification>().iff = parsedValue;
                //Debug.Log("Conversion successful! Parsed value: " + parsedValue);
            }
            else
            {
                Debug.Log("Conversion failed. Invalid iff.");
            }

            //ally.GetComponentInChildren<Gunner>().active = true;
            //ally.GetComponentInChildren<Gunner>().ammo = masterSpawn.ally_spawnPlanes[i].gunAmmo;
            
        }
        int k = 0;
        Transform last = MainPlaneStarter.transform;
        //Assiagn Formation target
        foreach (GameObject ally in AllyPlanes)
        {
            foreach (GameObject ally2 in AllyPlanes)
            {
                if(ally2 != ally)
                ally.GetComponent<CombineUttam>().friends.Add(ally2);
            }
            if (k >= 0)
            {
                Brain brain = ally.GetComponent<Brain>();

                var x = missionPlan.ally_spawnPlanes[k].Latitude;
                var y = missionPlan.ally_spawnPlanes[k].Longitude;

                if (Vector3.Distance(Vector3.zero, new Vector3(x, 0, y)) < 80f)
                {
                    x = missionPlan.ally_spawnPlanes[k].Latitude;
                    y = missionPlan.ally_spawnPlanes[k].Longitude;
                    brain.Formation = true;
                    Transform target = new GameObject().transform;
                    target.SetParent(last);
                    brain.FormationTargetPlane = target;
                    x = x == 0 ? 0 : ((x / 15) * 70);
                    y = y == 0 ? 0 : ((y / 15) * 70);

                    target.localPosition = new Vector3(x, -10* k, y - 100);
                    brain.transform.position = target.position;
                    target.localPosition = new Vector3(x, -10*k, y + 100f);
                    target.localRotation = Quaternion.identity;
                }

                //brain.Formation = true;
                //Transform target = new GameObject().transform;
                //target.SetParent(last);
                //brain.FormationTargetPlane = target;
                //var x = missionPlan.ally_spawnPlanes[k].Latitude;
                //var y = missionPlan.ally_spawnPlanes[k].Longitude;

                //x = x == 0 ? 0 : ((x / 15) * 50);
                //y = y == 0 ? 0 : ((y / 15) * 50);

                //target.localPosition = new Vector3(x, -10, y - 100);
                //brain.transform.position = target.position;
                //target.localPosition = new Vector3(x, -10, y + 100f);
                //target.localRotation = Quaternion.identity;

            }
            else
            {
                //last = ally.transform;
            }

            k++;
        }


        //Adversary Planes
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < missionPlan.adversary_spawnPlanes.Count; i++)
        {
            int j = 0;
            foreach (GameObject prefab in enemyPrefabs)
            {
                if (prefab.name.ToLower() == missionPlan.adversary_spawnPlanes[i].name.ToLower()) { break; }
                j++;
            }
            var enemy = Instantiate(enemyPrefabs[j]);
            enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            enemy.transform.position = new Vector3(missionPlan.adversary_spawnPlanes[i].spawnPosition.x, missionPlan.adversary_spawnPlanes[i].spawnPosition.y + 100, missionPlan.adversary_spawnPlanes[i].spawnPosition.z);
            enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            enemy.transform.localEulerAngles = new Vector3(0, -missionPlan.adversary_spawnPlanes[i].Heading, 0);
            //Add in Feedback;
            FeedBackRecorderAndPlayer.Allys.Add(enemy.transform);
            //AI Radar
            AIRadar radar = enemy.GetComponentInChildren<AIRadar>();
            AircraftPlanData data = missionPlan.adversary_spawnPlanes[i];
            //float mile = 1852f;
            float G = data.Gain;
            float TRM = 763;
            float power = data.Power / TRM;
            float applyPower = 1;
            float r = data.TRCS;// radar aperture radius
            float NR = data.MinimalNoise;
            float N = data.NoiseFactor;
            float T = data.Temp; //Temprature in kelvin
            float bandwidth = data.Frequency * Mathf.Pow(10f, 9f);
            float lamda = (3 * Mathf.Pow(10, 8)) / bandwidth;
            float Pmin = (1.38f * Mathf.Pow(10, -23) * T * bandwidth * N * NR);//minimun detectable signal
            float gamma = Mathf.PI * r * r;////Target Radar Cross Section/ radar aperture
            float Pt = power * TRM * applyPower;//total consume power
            float Coverage = Mathf.Sqrt(Mathf.Sqrt((float)((Pt * Mathf.Pow(G, 2) * Mathf.Pow(lamda, 2) * Mathf.Pow(gamma, 2)) / (Mathf.Pow(4f * Mathf.PI, 3) * Pmin))));
            //Coverage = Mathf.Round(Coverage / (mile / 1000));
            radar.Range = Coverage * 1000;
            radar.G = (int)data.Gain;
            radar.f = (int)data.Frequency;
            radar.power = data.Power;
            radar.aperture = data.TRCS;

            //missiles
            CheckMissilesEnemy(i, enemy);
            CombineUttam combineUttam = enemy.GetComponent<CombineUttam>();
            foreach (MissileInfo info in missionPlan.adversary_spawnPlanes[i].missileInfo)
            {
                MissileInfo inf = new MissileInfo(info);
                combineUttam.missileInfos.Add(inf);
            }

            //enemy.GetComponent<Specification>().iff = masterSpawn.adversary_spawnPlanes[i].callSignCode;
            var ew = enemy.GetComponentInChildren<EWRadar>();
            if (ew != null)
            {
                ew.maxFlares = missionPlan.adversary_spawnPlanes[i].Flares;
                bool autoflare = missionPlan.adversary_spawnPlanes[i].AutoFlares;
                ew.autoFlareFire = autoflare;
                ew.maxChaff = missionPlan.adversary_spawnPlanes[i].Chaffs;
                bool autochaff = missionPlan.adversary_spawnPlanes[i].AutoChaffs;
                ew.autoChaffFire = autochaff;

                ew.Pt = missionPlan.adversary_spawnPlanes[i].EWPower;
                ew.f = missionPlan.adversary_spawnPlanes[i].EWFrequency;
                ew.Gt = missionPlan.adversary_spawnPlanes[i].EWGain;
                ew.Gr = missionPlan.adversary_spawnPlanes[i].RecieveGain;
                ew.Pr = missionPlan.adversary_spawnPlanes[i].Threshold;
                ew.radar_Range = ew.getRange();

                //ew.burstChaffSize = missionPlan.adversary_spawnPlanes[i].chaffBurst;
                //ew.burstFlareSize = missionPlan.adversary_spawnPlanes[i].flareBurst;
                var Jam = enemy.GetComponentInChildren<Jammer>();
                Jam.JammerActive = missionPlan.adversary_spawnPlanes[i].Jammer;
                Jam.Pt = missionPlan.adversary_spawnPlanes[i].TransmittedPower;
                Jam.Gr = missionPlan.adversary_spawnPlanes[i].RadarReceiverGain;
                Jam.F = missionPlan.adversary_spawnPlanes[i].RadarFrequency;
                Jam.σ = missionPlan.adversary_spawnPlanes[i].TargetCrossSection;
                Jam.D = missionPlan.adversary_spawnPlanes[i].TargetDistance;
                Jam.Dj = missionPlan.adversary_spawnPlanes[i].TargetDistance / 2;
                Jam.Pj = missionPlan.adversary_spawnPlanes[i].JammerTransPower;
                Jam.Gj = missionPlan.adversary_spawnPlanes[i].JammerReceiverGain;
                Jam.Fj = missionPlan.adversary_spawnPlanes[i].JammerTransFreq;
                Jam.Grj = missionPlan.adversary_spawnPlanes[i].TargetReceiJamSignalGain;
            }
            AdversaryPlanes.Add(enemy);
            waypointsViewer.Adversary.Add(enemy.transform);
            enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
            int parsedValue;

            if (int.TryParse(missionPlan.adversary_spawnPlanes[i].callSignCode, out parsedValue))
            {
                enemy.GetComponent<Specification>().iff = parsedValue;
                //Debug.Log("Conversion successful! Parsed value: " + parsedValue);
            }
            else
            {
                Debug.Log("Conversion failed. Invalid iff.");
            }
            
            enemies.Add(enemy);
            //enemy.GetComponentInChildren<Gunner>().active = true;
            //enemy.GetComponentInChildren<Gunner>().ammo = masterSpawn.adversary_spawnPlanes[i].gunAmmo;
           
        }


        k = 0;
        last = null;
        Vector2 poss = Vector2.zero;
        //Assiagn Formation target
        foreach (GameObject enemy in enemies)
        {
            foreach (GameObject enemy2 in enemies)
            {
                if (enemy2 != enemy)
                    enemy.GetComponent<CombineUttam>().friends.Add(enemy2);
            }
            //

            if (k > 0)
            {
                Brain brain = enemy.GetComponent<Brain>();
                
                float x = missionPlan.adversary_spawnPlanes[k].Latitude;
                float y = missionPlan.adversary_spawnPlanes[k].Longitude;

                if (Vector3.Distance(Vector3.zero, new Vector3(x, 0, y)) < 80f){

                    x = missionPlan.adversary_spawnPlanes[k].Latitude;
                    y = missionPlan.adversary_spawnPlanes[k].Longitude;
                    brain.Formation = true;
                    Transform target = new GameObject().transform;
                    target.SetParent(last);
                    brain.FormationTargetPlane = target;
                    x -= poss.x;
                    x = x == 0 ? 0 : ((x / 15) * 70);
                    y = y == 0 ? 0 : ((y / 15) * 70);

                    target.localPosition = new Vector3(x, -10*k, y);
                    brain.transform.position = target.position;
                    target.localPosition = new Vector3(x, -10*k, y + 100f);
                    target.localRotation = Quaternion.identity;
                }
                

            }
            else
            {
                last = enemy.transform;
                poss.x = missionPlan.adversary_spawnPlanes[k].Latitude;
                poss.y = missionPlan.adversary_spawnPlanes[k].Longitude;
            }

            k ++;
        }


        //Neutral Planes
        List<GameObject> Neutrals = new List<GameObject>();
        for (int i = 0; i < missionPlan.neutral_spawnPlanes.Count; i++)
        {
            int j = 0;
            foreach (GameObject prefab in neutralPrefabs)
            {
                if (prefab.name.ToLower() == missionPlan.neutral_spawnPlanes[i].name.ToLower()) { break; }
                j++;
            }
            var Neutral = Instantiate(neutralPrefabs[j]);
            Neutral.transform.position = new Vector3(missionPlan.neutral_spawnPlanes[i].spawnPosition.x, missionPlan.neutral_spawnPlanes[i].spawnPosition.y + 100, missionPlan.neutral_spawnPlanes[i].spawnPosition.z);
            Neutral.transform.localEulerAngles = new Vector3(0, -missionPlan.neutral_spawnPlanes[i].Heading, 0);
            Neutral.GetComponent<NuteralPlaneController>().speed = missionPlan.neutral_spawnPlanes[i].speed;
            //Add in Feedback;
            FeedBackRecorderAndPlayer.Allys.Add(Neutral.transform);
            //enemy.GetComponent<Specification>().iff = masterSpawn.adversary_spawnPlanes[i].callSignCode;
            //var ew = Neutral.GetComponentInChildren<EWRadar>();
            //ew.maxFlares = missionPlan.adversary_spawnPlanes[i].Flares;
            //ew.autoFlareFire = masterSpawn.adversary_spawnPlanes[i].flares;
            //ew.maxChaff = missionPlan.adversary_spawnPlanes[i].Chaffs;
            //ew.autoChaffFire = masterSpawn.adversary_spawnPlanes[i].chaffs;
            //ew.burstChaffSize = missionPlan.adversary_spawnPlanes[i].chaffBurst;
            //ew.burstFlareSize = missionPlan.adversary_spawnPlanes[i].flareBurst;
            //Neutral.GetComponentInChildren<Jammer>().JammerActive = true;
            NeutralPlanes.Add(Neutral);
            waypointsViewer.Neutrals.Add(Neutral.transform);
            //int parsedValue;

            //if (int.TryParse(missionPlan.adversary_spawnPlanes[i].callSignCode, out parsedValue))
            {
               // Neutral.GetComponent<Specification>().iff = parsedValue;
                //Debug.Log("Conversion successful! Parsed value: " + parsedValue);
            }
           // else
            {
                Debug.Log("Conversion failed. Invalid iff.");
            }

            Neutrals.Add(Neutral);
            //enemy.GetComponentInChildren<Gunner>().active = true;
            //enemy.GetComponentInChildren<Gunner>().ammo = masterSpawn.adversary_spawnPlanes[i].gunAmmo;
            //missiles
            //CheckMissilesEnemy(i, enemy);
        }

        var feedbck = GameObject.FindObjectOfType<FeedBackRecorderAndPlayer>();
        if (PlayerPrefs.GetInt("Scenerio") == 2||true)
        {
            //Sams
            for (int i = 0; i < missionPlan.sams_spawnPlanes.Count; i++)
            {
                int j = 0;
                foreach (GameObject prefab in samsPrefabs)
                {
                    if (prefab.name == missionPlan.sams_spawnPlanes[i].name) { break; }
                    j++;
                }
                Debug.Log(j);
                Vector3 pos = missionPlan.sams_spawnPlanes[i].spawnPosition;
                RaycastHit hit;
                if (Physics.Raycast(missionPlan.sams_spawnPlanes[i].spawnPosition, Vector3.down, out hit, 5000))
                {
                    pos = hit.point;
                }
                var sam = Instantiate(samsPrefabs[j]);
                sam.transform.position = pos;
                //AI Radar
                GroundRadar radar = sam.GetComponentInChildren<GroundRadar>();
                AircraftPlanData data = missionPlan.sams_spawnPlanes[i];
                //float mile = 1852f;
                float G = data.Gain;
                float TRM = 763;
                float power = data.Power / TRM;
                float applyPower = 1;
                float r = data.TRCS;// radar aperture radius
                float NR = data.MinimalNoise;
                float N = data.NoiseFactor;
                float T = data.Temp; //Temprature in kelvin
                float bandwidth = data.Frequency * Mathf.Pow(10f, 9f);
                float lamda = (3 * Mathf.Pow(10, 8)) / bandwidth;
                float Pmin = (1.38f * Mathf.Pow(10, -23) * T * bandwidth * N * NR);//minimun detectable signal
                float gamma = Mathf.PI * r * r;////Target Radar Cross Section/ radar aperture
                float Pt = power * TRM * applyPower;//total consume power
                float Coverage = Mathf.Sqrt(Mathf.Sqrt((float)((Pt * Mathf.Pow(G, 2) * Mathf.Pow(lamda, 2) * Mathf.Pow(gamma, 2)) / (Mathf.Pow(4f * Mathf.PI, 3) * Pmin))));
                //Coverage = Mathf.Round(Coverage / (mile / 1000));
                radar.radarRange = Coverage * 1000;
                radar.G = (int)data.Gain;
                radar.f = (int)data.Frequency;

                foreach (MissileInfo info in missionPlan.sams_spawnPlanes[i].missileInfo)
                {
                    MissileInfo inf = new MissileInfo(info);
                    radar.missileInfos.Add(inf);
                }
                foreach(string str in data.Hardpoints)
                {
                    radar.missileName.Add(str);
                }

                //Add in Feedback;
                feedbck.ExtraEntity.Add(sam.transform);
                //FeedBackRecorderAndPlayer.Allys.Add(sam.transform);
                waypointsViewer.Sams.Add(sam.transform);
                Sams.Add(sam);
            }

            //Warships
            for (int i = 0; i < missionPlan.Warship_spawnPlanes.Count; i++)
            {
                int j = 0;
                foreach (GameObject prefab in warshipPrefabs)
                {
                    if (prefab.name == missionPlan.Warship_spawnPlanes[i].name) { break; }
                    j++;
                }
                Debug.Log(j);
                Vector3 pos = missionPlan.Warship_spawnPlanes[i].spawnPosition;
                RaycastHit hit;
                if (Physics.Raycast(missionPlan.Warship_spawnPlanes[i].spawnPosition, Vector3.down, out hit, 5000))
                {
                    pos = hit.point;
                }
                var warship = Instantiate(warshipPrefabs[j]);
                warship.transform.position = pos;
                //AI Radar
                CarrierShipController radar = warship.GetComponentInChildren<CarrierShipController>();
                AircraftPlanData data = missionPlan.Warship_spawnPlanes[i];
                //float mile = 1852f;
                float G = data.Gain;
                float TRM = 763;
                float power = data.Power / TRM;
                float applyPower = 1;
                float r = data.TRCS;// radar aperture radius
                float NR = data.MinimalNoise;
                float N = data.NoiseFactor;
                float T = data.Temp; //Temprature in kelvin
                float bandwidth = data.Frequency * Mathf.Pow(10f, 9f);
                float lamda = (3 * Mathf.Pow(10, 8)) / bandwidth;
                float Pmin = (1.38f * Mathf.Pow(10, -23) * T * bandwidth * N * NR);//minimun detectable signal
                float gamma = Mathf.PI * r * r;////Target Radar Cross Section/ radar aperture
                float Pt = power * TRM * applyPower;//total consume power
                float Coverage = Mathf.Sqrt(Mathf.Sqrt((float)((Pt * Mathf.Pow(G, 2) * Mathf.Pow(lamda, 2) * Mathf.Pow(gamma, 2)) / (Mathf.Pow(4f * Mathf.PI, 3) * Pmin))));
                //Coverage = Mathf.Round(Coverage / (mile / 1000));
                radar.radarRange = Coverage * 1000;
                radar.G = (int)data.Gain;
                radar.f = (int)data.Frequency;
                foreach (MissileInfo info in missionPlan.Warship_spawnPlanes[i].missileInfo)
                {
                    MissileInfo inf = new MissileInfo(info);
                    radar.missileInfos.Add(inf);
                }
                foreach (string str in data.Hardpoints)
                {
                    radar.missileName.Add(str);
                }
                //Add in Feedback;
                feedbck.ExtraEntity.Add(warship.transform);
                //FeedBackRecorderAndPlayer.Allys.Add(warship.transform);
                waypointsViewer.Warships.Add(warship.transform);
                Warships.Add(warship);
            }

            waypointsViewer.InitPlanesPoints();
            return;
        }
        return;
        List<GameObject> sams = new List<GameObject>();
        for (int i = 0; i < masterSpawn.sams_spawnPlanes.Count; i++)
        {
            int j = 0;
            foreach (GameObject prefab in samsPrefabs)
            {
                if (prefab.name == masterSpawn.sams_spawnPlanes[i].model) { break; }
                j++;
            }
            var sam = Instantiate(samsPrefabs[j]);
            sam.transform.position = masterSpawn.sams_spawnPlanes[i].spawnPosition;
            sams.Add(sam);
        }
        if(sams.Count == 0) { return; }
        foreach(GameObject enem in enemies)
        {
            int j = 0;
            var dist = Vector3.Distance(sams[0].transform.position, enem.transform.position);
            for (int i = 1; i < sams.Count; i++)
            {
                if(dist>Vector3.Distance(sams[i].transform.position, enem.transform.position)) { j = i; }
            }
            sams[j].GetComponentInChildren<GroundRadar>().aircrafts.Add(enem);
        }
    }
    void CheckMissiles(int i, GameObject obj)
    {
        obj.GetComponent<CombineUttam>().maxMissile = 0;
        foreach (string data in missionPlan.ally_spawnPlanes[i].Hardpoints)
        {
            if (!data.ToLower().Contains("empty")&& !data.ToLower().Contains("bomb")&& !data.ToLower().Contains("fuel"))
            {
                obj.GetComponent<CombineUttam>().maxMissile++;
                obj.GetComponent<CombineUttam>().missileName.Add(data);
            }
        }

        //if (masterSpawn.ally_spawnPlanes[i].s1 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s2 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s3 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s4 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s5 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s5L!= "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s5R!= "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s6 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s7 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s8 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.ally_spawnPlanes[i].s9 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
    }
    void CheckMissilesEnemy(int i, GameObject obj)
    {
        obj.GetComponent<CombineUttam>().maxMissile = 0;
        foreach (string data in missionPlan.adversary_spawnPlanes[i].Hardpoints)
        {
            if (!data.ToLower().Contains("empty") && !data.ToLower().Contains("bomb") && !data.ToLower().Contains("fuel"))
            {
                obj.GetComponent<CombineUttam>().maxMissile++;
                obj.GetComponent<CombineUttam>().missileName.Add(data);
            }
        }

        //if (masterSpawn.adversary_spawnPlanes[i].s1 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s2 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s3 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s4 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s5 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s5L!= "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s5R!= "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s6 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s7 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s8 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
        //if (masterSpawn.adversary_spawnPlanes[i].s9 != "") { obj.GetComponent<CombineUttam>().maxMissile++; }
    }
    #endregion

    #region old required functions
    void Ally_SpawnPlanesScriptObject(string model, List<GameObject> allyPrefabs, int count, string skill,
        Vector3 spawnPos, string code)
    {
        PlayerPrefs.SetInt("AllyCount", count);
        PlayerPrefs.SetInt("AllyCount2", count);
        foreach (GameObject gameObject in allyPrefabs)
        {
            if (model == gameObject.name)
            {
                for (int i = 0; i < count; i++)
                {
                    UnityEngine.Vector3 pos = new UnityEngine.Vector3(spawnPos.x, UnityEngine.Random.Range(1000f, 1500f), spawnPos.z);
                    GameObject plane = Instantiate(gameObject);
                    plane.transform.position = pos;
                    //plane.GetComponent<Rigidbody>().freezeRotation = true;
                    //plane.tag = allyTag;
                    progressCountAlly += 1;
                    plane.GetComponent<SaveEntityDatas>().type = EntityType.Plane_Ally;
                    #region
                    /*
                    var script = plane.AddComponent<AllyBehaviour>();
                    var _script = plane.AddComponent<AllyRespond>();
                    script.speed = allySpeed;
                    if (skill == "Beginner")
                        script.SetSkillBased_Speed(1.25f, 1f, 1f, allySpeed);
                    else if (skill == "Amateur")
                        script.SetSkillBased_Speed(1.35f, 1.2f, 1.1f, allySpeed);
                    else if (skill == "Professional")
                        script.SetSkillBased_Speed(1.45f, 1.2f, 1.3f, allySpeed);
                    else
                        script.SetSkillBased_Speed(1.25f, 1f, 1f, allySpeed);
                    script.explosionPrefab = explosionPrefab;
                    script.Missile = allyMissile;
                    script.Flare = flare;
                    script.IFFcode = code;
                    _script.enemyTag = enemyTag;
                    _script.enemyMissileTag = enemyMissileTag;                */
                    #endregion
                    #region 
                    //var script = plane.GetComponent<newEnemyBehaviour>();
                    //script.speed = allySpeed;
                    /*var enemyRadar = plane.GetComponent<EnemyRadar>();
                    var avoidObj=plane.GetComponent<AvoidObject>();
                    enemyRadar.enemyTag = enemyTag;
                    enemyRadar.playerTag = enemyTag;
                    enemyRadar.EnemyMissile = enemyMissileTag;
                    enemyRadar.allyTag=allyTag;
                    script.speed = adversarySpeed;
                    script.isFormationActive = false;
                    avoidObj.avoidAllyTag=allyTag;
                    avoidObj.enemyTag = enemyTag;*/
                    #endregion
                    //GameObject png = Instantiate(AllyPng, plane.transform.position, Quaternion.identity);
                    //png.transform.parent = plane.transform;
                }
            }
        }
    }
    //void Neutral_SpawnPlanesScriptObject(string model, List<GameObject> neutralPrefabs, int count, string skill,
    //    List<float> spawnPosAtX, List<float> spawnPosAtY, List<float> spawnPosAtZ, string code)
    //{
    //    foreach (GameObject gameObject in neutralPrefabs)
    //    {
    //        if (model == gameObject.name)
    //        {
    //            PlayerPrefs.SetInt("NeutralCount", count);//set neutral count.
    //            for (int i = 0; i < count; i++)
    //            {
    //                UnityEngine.Vector3 pos = new UnityEngine.Vector3(spawnPosAtX[i], 1500, spawnPosAtZ[i]);
    //                GameObject plane = Instantiate(gameObject);
    //                plane.transform.position = pos;
    //                plane.GetComponent<SaveAllData>().gameDataManager = SaveDataManager;
    //                //plane.GetComponent<Rigidbody>().freezeRotation = true;
    //                progressCountNeutral += 1;

    //                //plane.tag = neutralTag;
    //                var script = plane.AddComponent<NeutralBehaviour>();
    //                script.speed = neutralSpeed;
    //                script.IFFcode = "neutral";
    //                //GameObject png = Instantiate(NeutralPng, plane.transform.position, Quaternion.identity);
    //                //png.transform.parent = plane.transform;
    //            }
    //        }
    //    }
    //}
    void Adversary_SpawnPlanesScriptObject(string model, List<GameObject> adversaryPrefabs, int count, string skill,
        Vector3 spawnPos, string code, int formationType/*,Vector3 aroundPoint,*/, bool isChase)
    {
        _tempArray.Clear();
        foreach (GameObject gameObject in adversaryPrefabs)
        {
            if (model == gameObject.name)
            {
                PlayerPrefs.SetInt("EnemyCount", count);
                PlayerPrefs.SetInt("EnemyCount2", count);
                print(count);
                for (int i = 0; i < count; i++)
                {
                    UnityEngine.Vector3 pos = new UnityEngine.Vector3(spawnPos.x, UnityEngine.Random.Range(2000f, 2500f), spawnPos.z);
                    GameObject plane = Instantiate(gameObject);
                    plane.transform.position = pos;
                    plane.GetComponent<SaveAllData>().gameDataManager = SaveDataManager;

                    //plane.GetComponent<Rigidbody>().freezeRotation = true;
                    _tempArray.Add(plane);
                    progressCountEnemy += 1;
                    plane.GetComponent<SaveEntityDatas>().type = EntityType.Plane_Adversary;
                    //var script = plane.GetComponent<newEnemyBehaviour>();
                    //var radarScript = plane.GetComponent<EnemyRadar>();
                    //script.SelectLeader();
                    //script.SelectSquad();
                    //script.FormationActive(false);
                    //radarScript.isChasePlayer= isChase;
                    //script.speed = adversarySpeed;
                    #region
                    /*
                    var _script = plane.AddComponent<EnemyRepond>();
                    var combatManeuvers = plane.AddComponent<CombatManeuvers>();
                    var _combat = plane.AddComponent<CombatManeuver_Respond>();
                    //var_c
                    plane.GetComponent<Rigidbody>().freezeRotation = true;
                    plane.tag = enemyTag;
                    script.speed = adversarySpeed;
                    script.formationWaypoints = waypoint;
                    script.SelectLeader();
                    if (skill == "Beginner")
                        script.SetSkillBased_Speed(0.95f, 0.8f, 1f, adversarySpeed);
                    else if (skill == "Amateur")
                        script.SetSkillBased_Speed(1f, 0.95f, 1.1f, adversarySpeed);
                    else if (skill == "Professional")
                        script.SetSkillBased_Speed(1.3f, 1.1f, 1.3f, adversarySpeed);
                    else
                        script.SetSkillBased_Speed(1.25f, 1f, 1f, adversarySpeed);
                    _script.PlayerTag = playerTag;
                    script.explosionPrefab = explosionPrefab;
                    script.SelectSquad();
                    _script.PlayerMissileTag = playerMissileTag;
                    _script.EnemyMissileTag = enemyMissileTag;
                    _script.AllyTag = allyTag;
                    _script.AllyMissileTag = allyMissileTag;
                    _script.playerChase = isChase;
                    script.Missile = enemyMissile;
                    script.Flare = flare;
                    script.IFFcode = code;
                    //script.bullet = bullet;
                    //script.flareCount = 2;
                    //script.missileCount = 3;
                    */
                    #endregion
                    #region
                    /*var script = plane.GetComponent<newEnemyBehaviour>();
                    var enemyRadar = plane.GetComponent<EnemyRadar>();
                    var avoidObj = plane.GetComponent<AvoidObject>();
                    enemyRadar.enemyTag = allyTag;
                    enemyRadar.playerTag = playerTag;
                    enemyRadar.EnemyMissile = allyMissileTag;
                    enemyRadar.allyTag =enemyTag;
                    script.speed = adversarySpeed;
                    avoidObj.avoidAllyTag = enemyTag;
                    avoidObj.enemyTag = allyTag;
                    avoidObj.playerTag = playerTag;
                    */
                    #endregion
                    #region

                    if (i == count - 1)
                    {
                        if (formationType == 1)
                        {
                            var formationScript = FormationManager.AddComponent<FormationScript>();
                            formationScript.leaderCraft = _tempArray[0].transform;
                            formationScript.spawnCount = count;
                            formationScript.enemyPlane = _tempArray;
                            formationScript.enabled = true;
                            formationScript.Initialize();
                        }
                        #region
                        else if (formationType == 2)
                        {
                            var formationScript = FormationManager.AddComponent<C_Formation>();
                            formationScript.enabled = false;
                            formationScript.radius = (int)PlayerPrefs.GetFloat("CircleRadius") * (count / 2);
                            formationScript.amount = count;
                            formationScript.enemyPlanes = _tempArray;
                            formationScript.rotationSpeed = 1f;
                            formationScript.aroundPoint = new Vector3(0, 300f, 0f);//aroundPoint;
                            formationScript.enabled = true;
                            formationScript.Initialize();
                        }
                        #endregion
                    }

                    #endregion
                    //GameObject png = Instantiate(EnemyPng, plane.transform.position, Quaternion.identity);
                    //png.transform.parent = plane.transform;

                }
            }
        }
    }

    //private void Ai_SpawnPlanesScriptObject(string model, List<GameObject> aiPrefab, int count, List<float> spawnPositionsAtX, List<float> spawnPositionsAtY, List<float> spawnPositionsAtZ, Vector3[] wayPoints)
    //{
    //    int each_Data = wayPoints.GetLength(0)/count;
    //    Vector3[] positions = new Vector3[wayPoints.Length];
    //    positions = wayPoints;
    //    _tempArray.Clear();
    //    foreach (GameObject gameObject in aiPrefab)
    //    {
    //        if (model == gameObject.name)
    //        {
    //            for (int i = 1; i < count; i++)
    //            {
    //                Vector3 pos = new Vector3(spawnPositionsAtX[i], spawnPositionsAtY[i], spawnPositionsAtZ[i]);
    //                GameObject plane = Instantiate(gameObject);
    //                plane.transform.position = pos;
    //                plane.GetComponent<SaveAllData>().gameDataManager = SaveDataManager;
    //                _tempArray.Add(plane);
    //                progressCountAI += 1;
    //                plane.tag = enemyTag;
    //                //var aiScriptValues = plane.GetComponent<SetAiDataValuesChase>();
    //                //aiScriptValues.neuralBrain = newBrain;
    //                //aiScriptValues.track = new List<Vector3>(wayPoints.ToList());
    //                //int addedIndex = each_Data * i;
    //                //aiScriptValues.min = addedIndex;
    //                //aiScriptValues.max = addedIndex + each_Data;
    //                //for(int j = 0; j < each_Data; j++)
    //                //{
    //                //    Vector3 posdata = (positions[j + addedIndex]);
    //                //    aiScriptValues.track[j] = posdata;
    //                //}
    //                GameObject png = Instantiate(AiPng, plane.transform.position, Quaternion.identity);
    //                png.transform.parent = plane.transform;
    //                //aiScriptValues.enabled = true;
    //            }
    //        }
    //    }
    //}
    #endregion
}
