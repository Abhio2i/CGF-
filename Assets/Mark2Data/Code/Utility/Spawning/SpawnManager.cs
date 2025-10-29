using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Enemy.Formation;
using Cinemachine;
using newSaveSystem;
using System;
using Random = UnityEngine.Random;
//using Enemy.UImanager;
#region Script info
//Input spawning data from user eg model,type,name etc
//Output spawning an PNG on the map and the PNG locations and data are used for the next scene
#endregion
namespace Enemy.Spawning
{
    [Serializable]
    public class formation
    {
        [SerializeField]
        public List<Vector3> form = new List<Vector3>();
    }

    public class SpawnManager : MonoBehaviour
    {
        #region variables and misc.
        [Header("ScriptableObjectsRef")]
        public SpawnPlanesData ally_SpawnPlanes;
        public SpawnPlanesData enemy_SpawnPlanes;
        public SpawnPlanesData neutral_SpawnPlanes;
        public SpawnPlanesData sams_SpawnPlanes;
        public AIPlaneData Ai_SpawnPlanes;
        public MasterSpawn master;

        [Header("Lists")]
        //public List<string> AirToAirModels = new List<string>() { "Ally", "Adversary" };
        //public List<string> AirToGroundModels = new List<string>() { "Ally", "Adversary" };
        //public List<string> AirToSeaModels = new List<string>() { "Ally", "Adversary" };
        public List<GameObject> Enemy_aircraftTypes;
        public List<GameObject> SAMSTypes;
        public List<GameObject> WarshipTypes;
        public List<GameObject> Neutral_aircraftTypes;
        public List<GameObject> Ally_aircraftTypes;
        public List<GameObject> Ai_aircraftTypes;
        public List<string> countryNames;
        public List<GameObject> allObjects;
        public List<string> skillsNames;
        public List<string> groupNames;
        public List<Vector3> circlePos;

        [Header("TMP")]
        public TMP_Dropdown Type;
        public TMP_Dropdown model;
        public TMP_Dropdown formationType;
        public TMP_Dropdown type;
        public TMP_Dropdown country;
        public TMP_Dropdown skill;
        //public TMP_Dropdown LaucherCount;
        //public TMP_Dropdown MissilesCount;
        public TMP_InputField callSign;
        public Toggle map, radar, planner, radio, chasePlayer;
        public TMP_InputField count;
        public TMP_InputField pilot;
        public TMP_InputField _name;
        public TMP_InputField _X;
        public TMP_InputField _Y;
        public TMP_InputField _Z;
        public TMP_Text displayEmpty;
        public TMP_Text tip;
        //public TMP_Text vmodel,vcountry;

        [Header("GameObjects")]
        //public GameManager gameManager;
        public GameObject selectedPlane;
        public GameObject plane;
        public GameObject spawnAreas;
        public GameObject allyPNG;
        public GameObject aiPNG;
        public GameObject neutralPNG;
        public GameObject enemyPNG;
        public GameObject playerChase;
        public CinemachineSmoothPath AiPath;
        [Header("others")]
        public MapChanger mapchange;
        public bool spawnPlane;

        public int spawnCount;
        private bool isAlly, isNeutral, isAdversary, isAi,isSAMS,isWarship;
        public Vector3[] SAMSSpawnPosition;
        public Vector3[] AllySpawnPosition;
        public Vector3[] EnemySpawnPosition;
        
        public bool onRadar, onPlanner, onMap, isRadio, isPlaneChae;
        public string countryname;
        public string mySkill;
        public string planeType;
        [Range(1000, 10000)]
        public float circleRadius;
        public MapLatLong latLong;




        private List<string> aircraftNames = new List<string>();
        private List<string> typeNames = new List<string>();
        private List<float> positionsAtX = new List<float>();
        private List<float> positionsAtY = new List<float>();
        private List<float> positionsAtZ = new List<float>();
        private int totalPlanesCount;
        private Vector3 circleCentrePos;
        [SerializeField] private int formationNumber;
        private UnityEngine.Vector3 planrot;
        private bool missileActive, flareActive;
        private List<GameObject> neutralSpawnSpots = new List<GameObject>();
        private List<GameObject> iconsSpawned = new List<GameObject>();
        public List<bool> positionsOccupied;
        private int randomVal;
        private Transform leaderCraft;
        private int allyPosCount = 0, enemyPosCount = 0,samsPosCount = 0;
        private SpawnPlanesData spawnPlanesData;
        #endregion
        #region Unity functions
        private void Awake()
        {
            SavingSystem.ClearAndResetDirectory("Neutral");
            SavingSystem.ClearAndResetDirectory("Ally");
            SavingSystem.ClearAndResetDirectory("Adversary");

            if (PlayerPrefs.GetInt("PlanesCount") != 0)
            {
                PlayerPrefs.SetInt("PlanesCount", 0);
            }
            playerChase.SetActive(false);
        }
        private void Start()
        {

            //planrot = plane.transform.eulerAngles;

            for (int i = 0; i < spawnAreas.transform.childCount; i++)
            {
                neutralSpawnSpots.Add(spawnAreas.transform.GetChild(i).gameObject);
                positionsOccupied.Add(false);
            }
            ClearAllDataOFSpawnPlanes(ally_SpawnPlanes);
            ClearAllDataOFSpawnPlanes(enemy_SpawnPlanes);
            ClearAllDataOFSpawnPlanes(neutral_SpawnPlanes);
            ClearAllDataOFSpawnPlanes(sams_SpawnPlanes);
            ClearAllDataOFAISpawnPlanes(Ai_SpawnPlanes);
            master.ally_spawnPlanes.Clear();
            master.ally_spawnPlanes.Add(new SpawnPlanesData(ally_SpawnPlanes));
            master.adversary_spawnPlanes.Clear();
            master.neutral_spawnPlanes.Clear();
            //master.ai_spawnPlanes.Clear();
            master.sams_spawnPlanes.Clear();
            OnTypeChange(0);
        }

        private void ClearAllDataOFAISpawnPlanes(AIPlaneData ai_SpawnPlanes)
        {
            //strings
            ai_SpawnPlanes.name = ai_SpawnPlanes.type = ai_SpawnPlanes.model = ai_SpawnPlanes.country = ai_SpawnPlanes.pilot = ai_SpawnPlanes.skill = ai_SpawnPlanes.count = ai_SpawnPlanes.callSign_code = null;

            ai_SpawnPlanes.unit = ai_SpawnPlanes.callSign1 = ai_SpawnPlanes.callSign2 = ai_SpawnPlanes.formationType = 0;

            ai_SpawnPlanes.radio = ai_SpawnPlanes.hiddenOnMap = ai_SpawnPlanes.hiddenOnPlanner = ai_SpawnPlanes.hiddenOnRadar = ai_SpawnPlanes.playerChase = false;

            ai_SpawnPlanes.track = null;

            ai_SpawnPlanes.spawnPositionsAtX.Clear();
            ai_SpawnPlanes.spawnPositionsAtY.Clear();
            ai_SpawnPlanes.spawnPositionsAtZ.Clear();
        }

        private void ClearAllDataOFSpawnPlanes(SpawnPlanesData spawnPlanes)
        {
            //strings
            spawnPlanes.type = spawnPlanes.name = spawnPlanes.type = spawnPlanes.model = spawnPlanes.country = spawnPlanes.pilot = spawnPlanes.skill = spawnPlanes.count = spawnPlanes.callSignCode = null;

            //int
            spawnPlanes.unit = spawnPlanes.callSign1 = spawnPlanes.callSign2 = spawnPlanes.formationType = 0;
            //bool
            spawnPlanes.radio = spawnPlanes.hiddenOnMap = spawnPlanes.hiddenOnPlanner = spawnPlanes.hiddenOnRadar = spawnPlanes.playerChase = false;
            //vector
            // spawnPlanes.aroundPoint = Vector3.zero;

            //list
            spawnPlanes.spawnPosition = Vector3.zero;

        }

        private void FixedUpdate()
        {
            SetToggle();
            if (spawnPlane)
            {
                Spawning();
                PlayerPrefs.SetInt("PlanesCount", totalPlanesCount);
                spawnPlane = false;
            }
        }
        #endregion
        #region required functions
        //this functions adds the required options to the dropDown as per the type of AI
        void Add_Options(List<GameObject> aircraftTypes)
        {
            //model.ClearOptions();
            aircraftNames.Clear();
            //aircraftNames.Add("NONE");
            if (aircraftTypes != null)
            {
                foreach (GameObject gameObject in aircraftTypes)
                {
                    aircraftNames.Add(gameObject.name);
                }
            }
            model.AddOptions(aircraftNames);
        }
        public void ModeChange(int mode)
        {
            //Type.ClearOptions();
            if (mode == 0)
            {
                PlayerPrefs.SetInt("Scenerio", 0);
                //Type.AddOptions(AirToAirModels);
            }
            if (mode == 1)
            {
                PlayerPrefs.SetInt("Scenerio", 1);
                //Type.AddOptions(AirToGroundModels);
            }
            if (mode == 2)
            {
                PlayerPrefs.SetInt("Scenerio", 2);
                //Type.AddOptions(AirToSeaModels);
            }
            type.value = 0;
            OnTypeChange(0);
            foreach (GameObject obj in iconsSpawned) { Destroy(obj); }
            iconsSpawned.Clear();
            master.ally_spawnPlanes.Clear();
            master.ally_spawnPlanes.Add(ally_SpawnPlanes);
            master.adversary_spawnPlanes.Clear();
            master.neutral_spawnPlanes.Clear();
            //master.ai_spawnPlanes.Clear();
            master.sams_spawnPlanes.Clear();
            enemyPosCount = allyPosCount = samsPosCount = 0;
            mapchange.ChangeScene();
            StartCoroutine(ModeChange());
        }
        public void OnTypeChange(int val)
        {
            planeType = type.options[val].text;
            if (val == 0)   //Ally
            {
                isAlly = true;
                isWarship = isSAMS = isAdversary = isNeutral = isAi = false;
                //callSign.interactable = false;
                callSign.text = "1234";
                //formationType.interactable = true;
                //_X.interactable = _Y.interactable = _Z.interactable = false;
                model.ClearOptions();
                Add_Options(Ally_aircraftTypes);
                SelectAircraftModel(0);
            }
            if (val == 1)   //Adversary
            {
                isAdversary = true;
                isWarship = isSAMS = isAlly = isNeutral = isAi = false;
                //callSign.interactable = false;
                //formationType.interactable = true;
                //_X.interactable = _Y.interactable = _Z.interactable = false;
                callSign.text = "2222";
                model.ClearOptions();
                Add_Options(Enemy_aircraftTypes);
                if (PlayerPrefs.GetInt("Scenerio") == 1)
                    Add_Options(SAMSTypes);
                else if (PlayerPrefs.GetInt("Scenerio") == 2)
                    Add_Options(WarshipTypes);
                //playerChase.SetActive(true);
                    SelectAircraftModel(0);
                return;
            }
            //if (val == 2)   //SAMS
            //{
            //    isSAMS = true;
            //    isAdversary = isAlly = isNeutral = isAi = false;
            //    //callSign.interactable = false;
            //    //country.interactable = false;
            //    formationType.interactable = false;
            //    if (PlayerPrefs.GetInt("Scenerio") == 1)
            //        Add_Options(SAMSTypes);
            //    else Add_Options(WarshipTypes);
            //    SelectAircraftModel(0);
            //}
            #region oldcode
            //if(val == 4)
            //{
            //    isAi = true;
            //    isAlly = isNeutral = isAdversary = false;
            //    callSign.interactable = false;
            //    //formationType.interactable = true;
            //    _X.interactable = _Y.interactable = _Z.interactable = false;
            //    Add_Options(Ai_aircraftTypes);
            //    playerChase.SetActive(true);
            //    SelectAircraftModel(0);
            //    return;
            //}
            //playerChase.SetActive(false);
            #endregion
        } //setting value based on type
        void Spawning()
        {

            if (count.text == "")
                spawnCount = 0;
            else if (int.Parse(count.text) > 5)
            {
                count.text = "5";
                spawnCount = int.Parse(count.text);
            }
            else
                spawnCount = int.Parse(count.text);
            if (selectedPlane != null)
            {

                if (isAdversary)
                {
                    PlayerPrefs.SetInt("EnemySpawnCount", spawnCount);
                    #region oldcode
                    //spawn enemy
                    //positionsAtX = new List<float>();
                    //positionsAtY = new List<float>();
                    //positionsAtZ = new List<float>();
                    //Vector2 pos = new Vector2(float.Parse(_X.text), float.Parse(_Y.text));
                    //pos = latLong.LatLongToVector2(pos);
                    #endregion
                    if (formationNumber == 1)
                    {
                        #region oldcode
                        //float x = Random.Range(-20222, 20222);
                        //float y = Random.Range(500, 600);//changed here from -100 to 100
                        //float z = Random.Range(-20222, 20222);
                        //GameObject plane = Instantiate(enemyPNG, new UnityEngine.Vector3(x, y, z), Quaternion.identity);
                        //FormationScript formationScript = new FormationScript();
                        //formationScript.leaderCraft = plane.transform;
                        //formationScript.CreateFormation(200f, spawnCount);
                        //positionsAtX.Add(plane.transform.position.x);
                        //positionsAtY.Add(plane.transform.position.y);
                        //positionsAtZ.Add(plane.transform.position.z);
                        //List<Vector3> formationPos = formationScript.positions;
                        //for (int _i = 0; _i < formationPos.Count - 1; _i++)
                        //{
                        //    positionsAtX.Add(formationPos[_i].x);
                        //    positionsAtY.Add(formationPos[_i].y);
                        //    positionsAtZ.Add(formationPos[_i].z);
                        //    plane = Instantiate(enemyPNG);
                        //    plane.transform.position = formationPos[_i];
                        //}
                        #endregion
                        for (int i = 0; i < spawnCount; i++)
                        {
                            plane = Instantiate(enemyPNG); 
                            plane.GetComponent<dragger>().index = enemyPosCount;
                            plane.transform.localScale *= 5;
                            plane.transform.position = new Vector3(EnemySpawnPosition[enemyPosCount].x,2000, EnemySpawnPosition[enemyPosCount].z);
                            enemyPosCount++;
                            iconsSpawned.Add(plane);
                        }
                    }
                    else if (formationNumber == 2)
                    {
                        #region oldcode
                        //GameObject plane = Instantiate(enemyPNG, new UnityEngine.Vector3(x, y, z), Quaternion.identity);
                        //C_Formation c_Formation = new C_Formation();
                        //PlayerPrefs.SetFloat("CircleRadius", circleRadius);
                        //circleCentrePos.y = Random.Range(500, 600);
                        //Vector2 temp = latLong.LatLongToVector2(pos);
                        //circleCentrePos.x = pos.x;
                        //circleCentrePos.z = pos.y;
                        //print(circleCentrePos + " " + temp);
                        //c_Formation.CreateEnemiesAroundPoint(spawnCount, circleCentrePos, circleRadius * (spawnCount / 2), true);
                        //circlePos = c_Formation.pos;
                        //foreach (Vector3 positions in circlePos)
                        //{
                        //    GameObject plane = Instantiate(enemyPNG, positions, Quaternion.identity);
                        //    positionsAtX.Add(positions.x);
                        //    positionsAtY.Add(positions.y);
                        //    positionsAtZ.Add(positions.z);
                        //}
                        #endregion
                        for (int i = 0; i < spawnCount; i++)
                        {
                            plane = Instantiate(enemyPNG);
                            plane.transform.localScale *= 5;
                            plane.GetComponent<dragger>().index = enemyPosCount ;
                            plane.transform.position = new Vector3(EnemySpawnPosition[enemyPosCount].x, 2000, EnemySpawnPosition[enemyPosCount].z);
                            enemyPosCount++;
                            iconsSpawned.Add(plane);
                        }

                    }
                    else
                    {
                        #region oldcode
                        //for (int i = 0; i < spawnCount; i++)
                        //{
                        //    totalPlanesCount++;
                        //    //float x = Random.Range(-5222, 5222) + EnemySpawnPosition.x;
                        //    //float y = Random.Range(300, 300) + EnemySpawnPosition.y;//changed here from -100 to 100
                        //    //float z = Random.Range(-5222, 5222) + EnemySpawnPosition.z;
                        //    positionsAtX.Add(x);
                        //    positionsAtY.Add(y);
                        //    positionsAtZ.Add(z);
                        //    plane = Instantiate(enemyPNG);
                        //    plane.transform.position = new Vector3(x, y, z);
                        //}
                        #endregion
                        for (int i = 0; i < spawnCount; i++)
                        {
                            plane = Instantiate(enemyPNG);
                            plane.transform.localScale *= 5;
                            plane.GetComponent<dragger>().index = enemyPosCount;
                            plane.transform.position = new Vector3(EnemySpawnPosition[enemyPosCount].x, 2000, EnemySpawnPosition[enemyPosCount].z);
                            enemyPosCount++;
                            iconsSpawned.Add(plane);
                        }
                    }
                    StartCoroutine(SetScriptableObj(enemy_SpawnPlanes, EnemySpawnPosition[0], formationNumber/*, pos*/));
                    // ClearEverything(); //clears everything except type
                }
                if (isAi)
                {
                    //spawn Ai
                    positionsAtX = new List<float>();
                    positionsAtY = new List<float>();
                    positionsAtZ = new List<float>();


                    for (int i = 0; i < spawnCount; i++)
                    {
                        totalPlanesCount++;
                        float x = Random.Range(-5222, 5222);
                        float y = Random.Range(300, 300);//changed here from -100 to 100
                        float z = Random.Range(-5222, 5222);
                        positionsAtX.Add(x);
                        positionsAtY.Add(y);
                        positionsAtZ.Add(z);
                        plane = Instantiate(aiPNG);
                        plane.transform.position = new Vector3(x, y, z);
                    }


                    //Vector3 pos = new Vector3(int.Parse(_X.text), int.Parse(_Y.text), int.Parse(_Z.text));
                    StartCoroutine(SetAIScriptableObj(Ai_SpawnPlanes, positionsAtX, positionsAtY, positionsAtZ));
                }
                if (isSAMS || isWarship) {
                    for (int i = 0; i < spawnCount; i++)
                    {
                        plane = Instantiate(neutralPNG); 
                        plane.GetComponent<dragger>().index = samsPosCount;
                        plane.transform.position = new Vector3(SAMSSpawnPosition[samsPosCount].x, 2000, SAMSSpawnPosition[samsPosCount].z);
                        samsPosCount++;
                        iconsSpawned.Add(plane);
                        plane.transform.localScale *= 5;
                    }
                    StartCoroutine(SetScriptableObj(neutral_SpawnPlanes, SAMSSpawnPosition[0], 0/*,Vector3.zero*/));
                    //ClearEverything(); //clears everything except type
                }
                #region oldcode
                //if (isNeutral && neutralSpawnSpots.Count > 0)
                //{

                //    int currentNeutralCount = 0;
                //    positionsAtX = new List<float>();
                //    positionsAtY = new List<float>();
                //    positionsAtZ = new List<float>();
                //    //spawn neutral
                //    for (int i = 0; i < spawnCount; i++)
                //    {
                //        totalPlanesCount++;
                //        if (currentNeutralCount < neutralSpawnSpots.Count)
                //        {

                //            randomVal = Random.Range(0, neutralSpawnSpots.Count - 1);

                //            if (positionsOccupied[randomVal])
                //                IsAlreadyPresentThere();

                //            if (i == spawnCount - 1 || currentNeutralCount == neutralSpawnSpots.Count - 1)
                //            {
                //                GameObject myPlane = new GameObject();
                //                myPlane.name = "neutral";
                //                // myPlane.AddComponent<PlaneSpecsAndDetails>();
                //                // setPlaneSpecs(myPlane,  positionsAtX, positionsAtY, positionsAtZ);
                //            }
                //            UnityEngine.Vector3 spawnPosition = neutralSpawnSpots[randomVal].transform.position;
                //            spawnPosition.y = Random.Range(200, 300);
                //            positionsAtX.Add(spawnPosition.x);
                //            positionsAtY.Add(spawnPosition.y);
                //            positionsAtZ.Add(spawnPosition.z);
                //            Instantiate(neutralPNG, new UnityEngine.Vector3(spawnPosition.x, 300f, spawnPosition.z), Quaternion.identity);

                //            positionsOccupied[randomVal] = true;
                //            currentNeutralCount++;
                //        }

                //    }

                //    StartCoroutine(SetScriptableObj(neutral_SpawnPlanes, EnemySpawnPosition, 0/*,Vector3.zero*/));
                //    //ClearEverything(); //clears everything except "type"
                //}
                #endregion
                if (isAlly)
                {
                    #region oldcode
                    //positionsAtX = new List<float>();
                    //positionsAtY = new List<float>();
                    //positionsAtZ = new List<float>();
                    ////spawn ally
                    //float distBetween = 30f;
                    //for (int i = 0; i < spawnCount; i++)
                    //{
                    //    totalPlanesCount++;
                    //    if (i % 2 == 0)
                    //        distBetween = -1000;//Random.Range(0,-500);
                    //    else
                    //        distBetween = +1000;// Random.Range(0, 500);

                    //    UnityEngine.Vector3 newPos = plane.transform.position + new UnityEngine.Vector3(distBetween * (i + 1), Random.Range(200, 300), -10f) + AllySpawnPosition;
                    //    if (i == spawnCount - 1)
                    //    {
                    //        GameObject plane = new GameObject();
                    //        plane.name = "ally";
                    //        // plane.AddComponent<PlaneSpecsAndDetails>();
                    //        // setPlaneSpecs(plane, positionsAtX, positionsAtY, positionsAtZ);
                    //        PlayerPrefs.SetString("IFFCode", callSign.text);
                    //    }

                    //    Instantiate(allyPNG, new UnityEngine.Vector3(newPos.x, newPos.y, newPos.z), Quaternion.identity);
                    //    positionsAtX.Add(newPos.x);
                    //    positionsAtY.Add(newPos.y);
                    //    positionsAtZ.Add(newPos.z);
                    //}
                    #endregion
                    for (int i = 0; i < spawnCount; i++)
                    {
                        plane = Instantiate(allyPNG);
                        plane.GetComponent<dragger>().index = allyPosCount +1;
                        plane.transform.position = new Vector3(AllySpawnPosition[allyPosCount].x, 2000, AllySpawnPosition[allyPosCount].z);
                        allyPosCount++;
                        iconsSpawned.Add(plane);
                    }
                    StartCoroutine(SetScriptableObj(ally_SpawnPlanes, AllySpawnPosition[0], 0/*,Vector3.zero*/));
                    //ClearEverything(); //clears everything except type
                }
            }
        } //spawning on map



        void SetToggle()
        {
            if (radar.isOn)  //aircraft is hidden on Radar
            {
                onRadar = false;
            }
            else
            {
                onRadar = true;
            }
            if (map.isOn) //aircraft is hidden on map
            {
                onMap = false;
            }
            else
            {
                onMap = true;
            }
            if (planner.isOn)  //aircraft is hidden on planner
            {
                onPlanner = false;
            }
            else
            {
                onPlanner = true;
            }
            if (radio.isOn)   //aircraft is hidden on radio
            {
                isRadio = true;
            }
            else
            {
                isRadio = false;
            }
            if (chasePlayer.isOn) // Aircraft is Chased By Enemy.
            {
                isPlaneChae = true;
            }
            else
            {
                isPlaneChae = false;
            }
        }
        public void SelectCountry(int val)
        {
            countryname = country.options[val].text; ; //aircraft country selection
        }
        public void SelectSkillLevel(int val)
        {
            mySkill = skill.options[val].text;   //aircraft skill level
        }
        //this function assigns the plane we selected from the DropDownlist to GameObject "selectedPlane"
        public void SelectAircraftModel(int val)
        {
            //if (val > 0)
            //    val = val - 1;
            if (isAlly)
                selectedPlane = Ally_aircraftTypes[val];
            else if (isNeutral)
                selectedPlane = Neutral_aircraftTypes[val];
            else if (isAdversary || isSAMS || isWarship)
            {
                isAdversary = true; isSAMS = isWarship =false;
                if (val < Enemy_aircraftTypes.Count)
                selectedPlane = Enemy_aircraftTypes[val];
                else{
                    isAdversary = false;
                    if (PlayerPrefs.GetInt("Scenerio") == 2)
                    {
                        isWarship = true;
                        selectedPlane = WarshipTypes[val - Enemy_aircraftTypes.Count];
                    }
                    else
                    {
                        selectedPlane = SAMSTypes[val - Enemy_aircraftTypes.Count];
                        isSAMS = true;
                    }
                }
            }
            else if (isAi)
                selectedPlane = Ai_aircraftTypes[val];
        }

        public void SelectFormationType(int val) //select a formation for adversary only
        {
            formationNumber = formationType.value;
            if (formationNumber == 2)
            {
                _X.interactable = _Y.interactable = true;
            }
            else
            {
                _X.interactable = _Y.interactable = false;
            }
        }

        public void SpawnNow()
        {
            #region old
            //if (model.value == 0 && count.text.Trim() == "" && type.value < 0)
            //{
            //    displayEmpty.text = "Enter count and select model and type";
            //    return;
            //}
            //else if (count.text.Trim() == "" && type.value < 0)
            //{
            //    displayEmpty.text = "Enter count and select a type";
            //    return;
            //}
            //else if (type.value < 0 && model.value == 0)
            //{
            //    displayEmpty.text = "Select type and model";
            //    return;
            //}
            //else if (model.value == 0 && count.text.Trim() == "")
            //{
            //    displayEmpty.text = "Enter count and select model";
            //    return;
            //}
            //else if (model.value == 0)
            //{
            //    displayEmpty.text = "Select a model";
            //    return;
            //}
            //else if(type.value<0)
            //{
            //    displayEmpty.text = "Select a type";
            //    return;
            //}
            //else if(count.text.Trim() == "")
            //{
            //    displayEmpty.text = "Enter count";
            //    return;
            //}
            //else
            //{
            //    displayEmpty.text = "";
            //}
            if (isSAMS || isWarship) { spawnPlane = true; }
            #endregion
            if (count.text.Trim() == "" && type.value < 0)
            {
                displayEmpty.text = "Enter count and select model and type";
                return;
            }
            else if (count.text.Trim() == "" && type.value < 0)
            {
                displayEmpty.text = "Enter count and select a type";
                return;
            }
            else if (type.value < 0)
            {
                displayEmpty.text = "Select type and model";
                return;
            }
            else if (count.text.Trim() == "")
            {
                displayEmpty.text = "Enter count and select model";
                return;
            }
            else if (type.value < 0)
            {
                displayEmpty.text = "Select a type";
                return;
            }
            else if (count.text.Trim() == "")
            {
                displayEmpty.text = "Enter count";
                return;
            }
            else
            {
                displayEmpty.text = "";
            }
            spawnPlane = true;
        }
        //clear field and values after pressing done
        public void ClearEverything()
        {
            StartCoroutine(ClearAllUserProvidedData());
        }
        IEnumerator ClearAllUserProvidedData()
        {
            // var upperButtons = new UI_Manager();
            displayEmpty.text = "Saving Aircraft Data";
            tip.text = "Tip: Spawned icons can be moved to reposition it.";
            //upperButtons.Interact(false);
            Interractiables(false);
            yield return new WaitForSeconds(3f);
            //type.value = 0;
            //model.value = 0;
            country.value = 0;
            skill.value = 0;
            formationType.value = 0;
            radio.isOn = false;
            chasePlayer.isOn = false;
            map.isOn = false;
            planner.isOn = false;
            radar.isOn = false;
            _name.text = "";
            count.text = "1";
            pilot.text = "";
            callSign.text = "";
            _X.text = "74.99204";
            _Y.text = "33.87865";
            yield return null;
            displayEmpty.text = "Saving Complete";
            Interractiables(true);
            yield return null;
            displayEmpty.text = "";
            tip.text = "";
            // upperButtons.Interact(true);
        }
        private void Interractiables(bool v)
        {
            _name.interactable = v;
            type.interactable = v;
            model.interactable = v;
            country.interactable = v;
            count.interactable = v;
            //skill.interactable = v;
            //pilot.interactable = v;
            callSign.interactable = v;
            //callSign.interactable = v;
            //radio.interactable = v;
            //formationType.interactable = v;
            //chasePlayer.interactable = v;
            //map.interactable = v;
            //planner.interactable = v;
            //radar.interactable = v;
        }
        #region old code
        IEnumerator SetAIScriptableObj(AIPlaneData ai_SpawnPlanes, List<float> positionsAtX, List<float> positionsAtY, List<float> positionsAtZ)
        {
            //ai_SpawnPlanes = new AIPlaneData();
            ai_SpawnPlanes.type = planeType;
            ai_SpawnPlanes._name = _name.text;
            ai_SpawnPlanes.model = selectedPlane.name;
            ai_SpawnPlanes.callSign_code = callSign.text;
            ai_SpawnPlanes.country = countryname;
            ai_SpawnPlanes.skill = mySkill;
            ai_SpawnPlanes.pilot = pilot.text;
            ai_SpawnPlanes.count = count.text;
            ai_SpawnPlanes.spawnPositionsAtX = positionsAtX;
            ai_SpawnPlanes.spawnPositionsAtY = positionsAtY;
            ai_SpawnPlanes.spawnPositionsAtZ = positionsAtZ;

            if (onRadar)
            {
                ai_SpawnPlanes.hiddenOnRadar = false;
            }
            else
            {
                ai_SpawnPlanes.hiddenOnRadar = true;
            }
            if (onPlanner)
            {
                ai_SpawnPlanes.hiddenOnPlanner = false;
            }
            else
            {
                ai_SpawnPlanes.hiddenOnPlanner = true;
            }
            if (onMap)
            {
                ai_SpawnPlanes.hiddenOnMap = false;
            }
            else
            {
                ai_SpawnPlanes.hiddenOnMap = true;
            }
            if (isRadio)
            {
                ai_SpawnPlanes.radio = true;
            }
            else
            {
                ai_SpawnPlanes.radio = false;
            }
            if (isPlaneChae)
            {
                ai_SpawnPlanes.playerChase = true;
            }
            else
            {
                ai_SpawnPlanes.playerChase = false;
            }
            ai_SpawnPlanes.track = AiPath;

            ai_SpawnPlanes.SetWaypoints();
            yield return new WaitForSeconds(2f);

            //master.ai_spawnPlanes.IndexOf(ai_SpawnPlanes);
            PlayerPrefs.SetInt("AiSpawnCount", int.Parse(count.text));
            //print(master.ai_spawnPlanes.Count);
            print(ai_SpawnPlanes + " aiCount" + PlayerPrefs.GetInt("AiSpawnCount"));
            yield return new WaitForSeconds(2f);
        }
        #endregion
        IEnumerator SetScriptableObj(SpawnPlanesData scriptableObject, Vector3 pos, int formation_Type/*,Vector2 aroundPos*/)
        {
            //scriptableObject = new _SpawnPlanes();
            scriptableObject.type = planeType;
            scriptableObject.name = _name.text;
            scriptableObject.model = selectedPlane.name;
            scriptableObject.callSignCode = callSign.text;
            scriptableObject.country = countryname;
            scriptableObject.skill = mySkill;
            scriptableObject.pilot = pilot.text;
            scriptableObject.count = count.text;
            scriptableObject.spawnPosition = pos;
            scriptableObject.formationType = formation_Type;
            // scriptableObject.aroundPoint = aroundPos;
            #region oldcode
            if (onRadar)
            {
                scriptableObject.hiddenOnRadar = false;
            }
            else
            {
                scriptableObject.hiddenOnRadar = true;
            }
            if (onPlanner)
            {
                scriptableObject.hiddenOnPlanner = false;
            }
            else
            {
                scriptableObject.hiddenOnPlanner = true;
            }
            if (onMap)
            {
                scriptableObject.hiddenOnMap = false;
            }
            else
            {
                scriptableObject.hiddenOnMap = true;
            }
            if (isRadio)
            {
                scriptableObject.radio = true;
            }
            else
            {
                scriptableObject.radio = false;
            }
            if (isPlaneChae)
            {
                scriptableObject.playerChase = true;
            }
            else
            {
                scriptableObject.playerChase = false;
            }
            #endregion
            if (scriptableObject.type == "Ally")
            {
                int j = allyPosCount - int.Parse(scriptableObject.count);
                for (int i = 0; i < int.Parse(scriptableObject.count); i++)
                {
                    scriptableObject.spawnPosition = AllySpawnPosition[j++]; ;
                    master.ally_spawnPlanes.Add(new SpawnPlanesData(scriptableObject));
                }
                PlayerPrefs.SetInt("AllySpawnCount", int.Parse(count.text));    //just for reference
            }
            if (scriptableObject.type == "Adversary")
            {
                if (isAdversary)
                {
                    int j = enemyPosCount - int.Parse(scriptableObject.count);
                    for (int i = 0; i < int.Parse(scriptableObject.count); i++)
                    {
                        scriptableObject.spawnPosition = EnemySpawnPosition[j++];
                        master.adversary_spawnPlanes.Add(new SpawnPlanesData(scriptableObject));
                    }
                    PlayerPrefs.SetInt("AdversarySpawnCount", int.Parse(count.text));   //just for reference
                }
                else if (isSAMS)
                {
                    int j = samsPosCount - int.Parse(scriptableObject.count);
                    for (int i = 0; i < int.Parse(scriptableObject.count); i++)
                    {
                        scriptableObject.spawnPosition = SAMSSpawnPosition[j++];
                        master.sams_spawnPlanes.Add(new SpawnPlanesData(scriptableObject));
                    }
                }
                else if (isWarship)
                {
                    int j = samsPosCount - int.Parse(scriptableObject.count);
                    for (int i = 0; i < int.Parse(scriptableObject.count); i++)
                    {
                        scriptableObject.spawnPosition = new Vector3(SAMSSpawnPosition[j].x, 60, SAMSSpawnPosition[j++].z);
                        master.sams_spawnPlanes.Add(new SpawnPlanesData(scriptableObject));
                    }
                }
            }
            #region oldcode
            //if (scriptableObject.type == "Neutral")
            //{
            //    for (int i = 0; i < int.Parse(scriptableObject.count); i++)
            //    {
            //        scriptableObject.spawnPosition = pos + new Vector3(500, 0, 500) * i;
            //        master.neutral_spawnPlanes.Add(scriptableObject);
            //    }
            //    PlayerPrefs.SetInt("NeutralSpawnCount", int.Parse(count.text));
            //    print(scriptableObject + " NeutralCount" + PlayerPrefs.GetInt("NeutralSpawnCount"));
            //    print(master.neutral_spawnPlanes.Count);
            //}
            //if (scriptableObject.type != "SAMS")
            //    SavingSystem.SaveFile(scriptableObject, scriptableObject.type);
            #endregion
            yield return new WaitForSeconds(2f);
        }

        void IsAlreadyPresentThere()
        {
            //checking if the plane is already present on the spot
            for (int i = 0; i < positionsOccupied.Count; i++)
            {
                if (positionsOccupied[i] == false)
                {
                    randomVal = i;
                    break;
                }
            }
        }
        IEnumerator ModeChange()
        {
            displayEmpty.text = "Clearing Everything";
            Interractiables(false);
            yield return new WaitForSeconds(1f);
            displayEmpty.text = "";
            Interractiables(true);
        }
        #endregion
    }
}