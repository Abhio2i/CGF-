using Enemy.Spawning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
//using UnityEngine.WSA;

public class MissionSave : MonoBehaviour
{
    [SerializeField]
    public MissionPlan DefaultPlan;
    public MissionPlan masterSpawn;
    public MissionPlanner missionPlanner;
    public Transform MissionListPanel;
    public GameObject MissionPrefab;
    public TextMeshProUGUI NameUI;
    public GameObject ChooseDirectory;
    public string json = "";
    public string CurrentMission = "";
    public List<string> missionList = new List<string>();
    public List<GameObject> missionsUI = new List<GameObject>();
    [Header("DataBase")]
    public string DatabasePath = "C:/";
    public string path = "";
    // Start is called before the first frame update
    void Start()
    {

        //json = JsonUtility.ToJson(masterSpawn);
        DatabasePath = PlayerPrefs.GetString("DatabasePathe");
        DatabasePath = DatabasePath == "" ? "E:/":DatabasePath;
        CreateDirectory(DatabasePath);
        
        //SaveMission("Mission3",masterSpawn,true);
        //LoadMission("Mission3");
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void CreateDirectory(string path)
    {
        if(Directory.Exists(path))
        {
            DatabasePath = path; 
            PlayerPrefs.SetString("DatabasePathe",path);
            if (!Directory.Exists(DatabasePath + "CGF_Mission_DataBase"))
            {
                Directory.CreateDirectory(DatabasePath + "CGF_Mission_DataBase");

            }
            ChooseDirectory.SetActive(false);
            MissionListUpdate();
        }
        else
        {
            ChooseDirectory.SetActive(true);
        }

        

    }

    public void MissionListUpdate()
    {
        path = Path.Combine(DatabasePath,"CGF_Mission_DataBase");
         
        
        //Remove Old Mission UI
        foreach (GameObject mission in missionsUI)
        {
            Destroy(mission);
        }
        missionsUI.Clear();
        missionList.Clear();
        string[] missionsList = Directory.GetDirectories(path);
        foreach (string mission in missionsList)
        {
            missionList.Add(mission);
            string name = Path.GetFileName(mission);
            string time = Directory.GetLastWriteTime(mission).ToString("MM-dd-yyyy h:mm tt");
            GameObject obj = Instantiate(MissionPrefab);
            obj.transform.parent = MissionListPanel;
            obj.SetActive(true);
            obj.transform.localScale = Vector3.one;
            //Name
            obj.transform.GetChild(0).GetComponent<Text>().text = name;
            //Time
            obj.transform.GetChild(1).GetComponent<Text>().text = time;
            //obj.GetComponent<Button>().onClick.AddListener(delegate { LoadMission(name);missionPlanner.MissionOpen(); missionPlanner.MissionLoad(false, name); });
            obj.GetComponent<Button>().onClick.AddListener(delegate { CurrentMission = name;NameUI.text = name; });
            missionsUI.Add(obj);
        }

    }

    public void MissionLoadByUI()
    {
        LoadMission(CurrentMission);
        missionPlanner.MissionOpen();
        missionPlanner.MissionLoad(false, CurrentMission);
    }

    public void DeleteMission()
    {
        string missionName = CurrentMission;
        string filePath = path + "/" + missionName;
        if (Directory.Exists(filePath))
        {
            // Delete the folder and its contents
            Directory.Delete(filePath, true);
            Debug.Log("Folder deleted: " + filePath);
            MissionListUpdate();
        }
    }

    public bool CheckMissionExist(string name)
    {
        foreach (string mission in missionList)
        {
            if(Path.GetFileName(mission) == name)
            {
                return true;
            }
        }
        return false;
    }


    public void SaveMission(string missionName,MissionPlan scriptableObject,bool Override=false)
    {
        DateTime localTime = DateTime.Now;

        // Get current UTC time
        DateTime utcTime = DateTime.UtcNow;

        string TimeStamp = localTime.ToString("MM_dd_yyyy h_mm tt");

        string filePath = path + "/" + missionName;// +"@"+TimeStamp;
        if (!Directory.Exists(filePath)|| Override)
        {
            if(!Directory.Exists(filePath))
             Directory.CreateDirectory(filePath);
            //File.CreateText(filePath);
            string MissionInfoData = JsonUtility.ToJson(scriptableObject.missionInfo);
            File.WriteAllText(filePath + "/info", MissionInfoData);
            string MissionDataAlly = JsonHelper.ToJson<AircraftPlanData>(scriptableObject.ally_spawnPlanes);
            File.WriteAllText(filePath+"/ally", MissionDataAlly);
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
            MissionListUpdate();
        }
        else
        {
            Debug.LogError(filePath + " already exist");
        }
    }

    public void LoadDefaultMission()
    {
        masterSpawn.missionInfo = new MissionInfo(DefaultPlan.missionInfo);
        masterSpawn.WeatherData = new WeatherData(DefaultPlan.WeatherData);
        masterSpawn.ally_spawnPlanes = new List<AircraftPlanData>();
        foreach (AircraftPlanData plan in DefaultPlan.ally_spawnPlanes)
        {
            masterSpawn.ally_spawnPlanes.Add( new AircraftPlanData(plan));
        }
        masterSpawn.adversary_spawnPlanes = new List<AircraftPlanData>();
        foreach (AircraftPlanData plan in DefaultPlan.adversary_spawnPlanes)
        {
            masterSpawn.adversary_spawnPlanes.Add(new AircraftPlanData(plan));
        }

        masterSpawn.neutral_spawnPlanes = new List<AircraftPlanData>();
        foreach (AircraftPlanData plan in DefaultPlan.neutral_spawnPlanes)
        {
            masterSpawn.neutral_spawnPlanes.Add(new AircraftPlanData(plan));
        }

        masterSpawn.waypoints = new List<Cordinates>();
        foreach (Cordinates plan in DefaultPlan.waypoints)
        {
            masterSpawn.waypoints.Add(new Cordinates(plan));
        }

        masterSpawn.sams_spawnPlanes.Clear();
        masterSpawn.Warship_spawnPlanes.Clear();
        return;
        

        masterSpawn.sams_spawnPlanes = new List<AircraftPlanData>();
        foreach (AircraftPlanData plan in DefaultPlan.sams_spawnPlanes)
        {
            masterSpawn.sams_spawnPlanes.Add(new AircraftPlanData(plan));
        }
        masterSpawn.Warship_spawnPlanes = new List<AircraftPlanData>();
        foreach (AircraftPlanData plan in DefaultPlan.Warship_spawnPlanes)
        {
            masterSpawn.Warship_spawnPlanes.Add(new AircraftPlanData(plan));
        }
        
    }

    public void LoadMission(string missionName)
    {
        string filePath = path + "/" + missionName;
        if (Directory.Exists(filePath))
        {
            string missionInfo = File.ReadAllText(filePath + "/info");
            masterSpawn.missionInfo= JsonUtility.FromJson<MissionInfo>(missionInfo);
            string AllyData = File.ReadAllText(filePath+"/ally");
            masterSpawn.ally_spawnPlanes = JsonHelper.FromJson<AircraftPlanData>(AllyData);
            string NeutralData = File.ReadAllText(filePath + "/neutral");
            masterSpawn.neutral_spawnPlanes = JsonHelper.FromJson<AircraftPlanData>(NeutralData);
            string AdversaryData = File.ReadAllText(filePath + "/adversary");
            masterSpawn.adversary_spawnPlanes = JsonHelper.FromJson<AircraftPlanData>(AdversaryData);
            string SamsData = File.ReadAllText(filePath + "/sams");
            masterSpawn.sams_spawnPlanes = JsonHelper.FromJson<AircraftPlanData>(SamsData);
            string WarshipData = File.ReadAllText(filePath + "/warship");
            masterSpawn.Warship_spawnPlanes = JsonHelper.FromJson<AircraftPlanData>(WarshipData);
            string WaypointsData = File.ReadAllText(filePath + "/waypoints");
            masterSpawn.waypoints = JsonHelper.FromJson<Cordinates>(WaypointsData);
            string WeatherData = File.ReadAllText(filePath + "/weather");
            masterSpawn.WeatherData = JsonUtility.FromJson<WeatherData>(WeatherData);
            Debug.Log("Data loaded");
            DateTime lastModifiedDate = Directory.GetLastWriteTime(filePath);
            Debug.Log(lastModifiedDate.ToString("MM_dd_yyyy h_mm tt"));
        }
        else
        {
            Debug.LogError(filePath + " not exist");
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
