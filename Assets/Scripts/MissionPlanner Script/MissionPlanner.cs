using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[Serializable]
public class HardpointsData
{
    [SerializeField]
    public List<string> Data;
}

[Serializable]
public class MissileDataInfo
{
    [SerializeField]
    public InputField Speed;
    [SerializeField]
    public InputField Range;
    [SerializeField]
    public InputField TurnRadius;
    [SerializeField]
    public InputField count;
}

public class MissionPlanner : MonoBehaviour
{
    public MissionPlan Default;
    public MissionPlan Master;
    public MissionWaypoints missionWaypoints;
    public MissionInfo.Scenario Scenario;
    
    [Header("Waypoints Panel UI")]
    [Header("Start Point A")]
    public InputField Latitude_A;
    public InputField Longitude_A;
    public InputField Altitude_A;
    public Dropdown Type_A;
    public Dropdown Formation_A;
    public InputField AircraftCount_A;
    public InputField Speed_A;

    [Header("A Point Aircrafts")]
    public RectTransform AllyAircraftParent;
    public GameObject AllyAircraftPrefab;
    public List<Transform> AllyAircrafts;

    [Header("End Point B")]
    public InputField Latitude_B;
    public InputField Longitude_B;
    public InputField Altitude_B;
    public Dropdown Type_B;
    public Dropdown Formation_B;
    public InputField AircraftCount_B;
    public InputField SamsCount_B;
    public InputField WarshipCount_B;
    public InputField Speed_B;

    [Header("B Point Aircrafts")]
    public RectTransform AdversaryAircraftParent;
    public GameObject AdversaryAircraftPrefab;
    public List<Transform> AdversaryAircrafts;

    [Header("B Point Sams")]
    public RectTransform AdversarySamsParent;
    public GameObject AdversarySamsPrefab;
    public List<Transform> AdversarySams;

    [Header("B Point Warship")]
    public RectTransform AdversaryWarshipParent;
    public GameObject AdversaryWarshipPrefab;
    public List<Transform> AdversaryWarship;

    [Header("Neutral")]
    public RectTransform NeutralParent;
    public GameObject NeutralPrefab;
    public List<Transform> Neutrals;

    [Header("Role/Weapons Panel UI")]
    public RectTransform ScrollBarContent;
    public GameObject CraftIconPrefab;
    public Sprite AllySprite;
    public Sprite AdversarySprite;
    public Sprite AdversarySamsSprite;
    public Sprite AdversaryWarshipSprite;
    public Sprite NeutralSprite;

    [Header("Aircraft Position")]
    public InputField Latitude;
    public InputField Longitude;
    public InputField Altitude;
    public Dropdown StartMode;
    public InputField Bearing;
    public InputField Speed;

    [Header("Weapons")]
    public InputField Chaffs;
    public InputField Flares;
    public InputField Bullets;
    public GameObject Hardpoint1;
    public GameObject Hardpoint2;
    public List<Dropdown> Hardpoints_Type1;
    public List<Dropdown> Hardpoints_Type2;
    public List<Dropdown> Hardpoints_Type3;
    public List<HardpointsData> Hardpoints_Data1 = new List<HardpointsData>();
    public List<HardpointsData> Hardpoints_Data2 = new List<HardpointsData>();
    public List<MissileDataInfo> missileDataInfo = new List<MissileDataInfo>();
    public List<MissileDataInfo> missileDataInfo2 = new List<MissileDataInfo>();

    [Header("Radar Config")]
    public InputField Power;
    public InputField Gain;
    public InputField Frequency;
    public InputField Temp;
    public InputField NoiseFactor;
    public InputField MinimalNoise;
    public InputField TRCS;
    public InputField Range;

    [Header("Electronic Warfare Config")]
    public InputField EWPower;
    public InputField EWGain;
    public InputField EWFrequency;
    public InputField ReceiveGain;
    public InputField Threshod;
    public InputField EwRange;
    public Toggle AutoChaffs;
    public Toggle AutoFlares;
    public Toggle DIRCM;
    public Toggle Jammer;

    [Header("JAMMER Configuration")]
    public InputField TransmittedPower;
    public InputField RadarReceiverGain;
    public InputField RadarFrequency;
    public InputField TargetCrossSection;
    public InputField TargetDistance;
    public InputField JammerTransPower;
    public InputField JammerReceiverGain;
    public InputField JammerTransFreq;
    public InputField TargetReceiJamSignalGain;
    public InputField SignalRatio;


    [Header("Weather")]
    public WeatherPlanner weatherPlanner;

    [Header("Mission Save/Load Related Variables")]
    public MissionSave missionSave;
    public TMP_InputField Mission_Name_Field;
    public Text WarningText;
    public string MissionName = "";
    public TextMeshProUGUI MissionNameUI;

    [Header("Mission Panels")]
    public GameObject ScenarioSelectionPanel;
    public GameObject MapPanel;
    public GameObject EditorTab;
    public GameObject ATA_Text;
    public GameObject ATG_Text;
    public GameObject ATS_Text;
    public GameObject WaypointsPanel;
    public GameObject RoleWeaponsPanel;
    public GameObject WeatherPanel;
    public GameObject LoadPanel;
    public GameObject SamsRoleButton;
    public GameObject WarshipRoleButton;

    [Header("Others")]
    public TextMeshProUGUI Lattext;
    public TextMeshProUGUI Longtext;
    public TextMeshProUGUI LatUnittext;
    public TextMeshProUGUI LongUnittext;

    [Header("MAPS")]
    public GameObject BangloreMap;
    public GameObject SeaMap;

    private int SelectedAircraft = 0;
    private string Role = "Ally";

    void Start()
    {
        //Init Hardpoint Weapoint
        int i = 0;
        foreach (Dropdown dropdown in Hardpoints_Type1)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(Hardpoints_Data1[i].Data);
            dropdown.onValueChanged.AddListener(delegate { SelectDropdownIcon(dropdown); });
            SelectDropdownIcon(dropdown);
            i++;
        }
        i = 0;
        foreach (Dropdown dropdown in Hardpoints_Type2)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(Hardpoints_Data2[i].Data);
            dropdown.onValueChanged.AddListener(delegate { SelectDropdownIcon(dropdown); });
            SelectDropdownIcon(dropdown);
            i++;
        }
    }

    public void MissionClose()
    {
        ScenarioSelectionPanel.SetActive(false);
        MapPanel.SetActive(false);
        EditorTab.SetActive(false);
        ResetThings();
    }

    public void ResetThings()
    {
        WaypointsPanel.SetActive(true);
        LoadPanel.SetActive(false);
        RoleWeaponsPanel.SetActive(false);
        WeatherPanel.SetActive(false);
        WarningText.gameObject.SetActive(false);
        ATA_Text.SetActive(false);
        ATG_Text.SetActive(false);
        ATS_Text.SetActive(false);
        Mission_Name_Field.text = "";
        Altitude_A.text = "1000";
        Altitude_B.text = "1000";
        Speed_A.text = "100";
        Speed_B.text = "100";
        AircraftCount_A.text = "1";
        AircraftCount_B.text = "1";
        SamsCount_B.text = "0";
        WarshipCount_B.text = "0";
        SamsCount_B.gameObject.SetActive(false);
        WarshipCount_B.gameObject.SetActive(false);
        missionWaypoints.RemoveAllWaypoints();
    }

    public void SelectMissionScenario(int i)
    {
        Scenario = (MissionInfo.Scenario)i;
    }

    public void MissionOpen()
    {
        ScenarioSelectionPanel.SetActive(true);
        MapPanel.SetActive(false);
        EditorTab.SetActive(false);
        ResetThings();
    }

    public void MissionCreate()
    {
        string name = Mission_Name_Field.text;
        string valid = IsValidString(name);
        if (valid == null)
        {
            if (!missionSave.CheckMissionExist(name))
            {
                
                MissionLoad(true,name);
            }
            else
            {
                WarningText.gameObject.SetActive(true);
                WarningText.text = "'"+name+"' : Mission Already Exists";
            }
        }
        else
        {
            WarningText.gameObject.SetActive(true);
            WarningText.text = valid;
        }
    }


    public void MissionLoad(bool Default=false,string name="")
    {
        ScenarioSelectionPanel.SetActive(false);
        MapPanel.SetActive(true);
        EditorTab.SetActive(true);
        ResetThings();
        MissionName = name;
        MissionNameUI.text = MissionName;
        if (Default)
        {
            missionSave.LoadDefaultMission();
        }
        else
        {
            Scenario = missionSave.masterSpawn.missionInfo.scenario;
        }
        if (Scenario == MissionInfo.Scenario.ATS)
        {
            BangloreMap.SetActive(false);
            SeaMap.SetActive(true);
        }
        else
        {
            BangloreMap.SetActive(true);
            SeaMap.SetActive(false);
        }

        Master.missionInfo.scenario = Scenario;
        Master.missionInfo.Name = MissionName;
        SamsCount_B.text = "0";
        WarshipCount_B.text = "0";
        SamsCount_B.gameObject.SetActive(false);
        WarshipCount_B.gameObject.SetActive(false);
        SamsRoleButton.SetActive(false);
        WarshipRoleButton.SetActive(false);
        weatherPlanner.LoadData(Master.WeatherData);
        int i = 0;
        foreach (Cordinates cordinates in missionSave.masterSpawn.waypoints)
        {
            if (i == 0)
            {
                missionWaypoints.startWaypoint.localPosition = new Vector2(cordinates.Latitude, cordinates.Longitude);
                missionWaypoints.UpdateWaypoints();
            }
            else
            if (missionSave.masterSpawn.waypoints.Count > 2&&i==1)
            {

            }else
                missionWaypoints.CreateWaypointFinal(new Vector2(cordinates.Latitude, cordinates.Longitude), true);
            i++;
        }
        if (missionSave.masterSpawn.waypoints.Count > 2)
        {
            missionWaypoints.CreateWaypointFinal(new Vector2(missionSave.masterSpawn.waypoints[1].Latitude, missionSave.masterSpawn.waypoints[1].Longitude), true);
        }


        //Ally 
        List<Vector3> cords = new List<Vector3>();
        List<Vector3> Bearings = new List<Vector3>();   
        int len = missionSave.masterSpawn.ally_spawnPlanes.Count;
        if(Scenario == MissionInfo.Scenario.ATA)
            ATA_Text.SetActive(true);
        
        for (i = 0; i < len; i++)
        {
            cords.Add(new Vector3(missionSave.masterSpawn.ally_spawnPlanes[i].Latitude, missionSave.masterSpawn.ally_spawnPlanes[i].Longitude, 0));
            Bearings.Add(new Vector3(0,0, missionSave.masterSpawn.ally_spawnPlanes[i].Bearing));
        }
        AircraftCount_A.text = len.ToString();
        CreateAllyAircraftsIconOnMap(cords,Bearings);

        //Adversary
        List<Vector3> cord = new List<Vector3>();
        List<Vector3> Bearing = new List<Vector3>();
        int lenth = missionSave.masterSpawn.adversary_spawnPlanes.Count;
        for (i = 0; i < lenth; i++)
        {
            cord.Add(new Vector3(missionSave.masterSpawn.adversary_spawnPlanes[i].Latitude, missionSave.masterSpawn.adversary_spawnPlanes[i].Longitude, 0));
            Bearing.Add(new Vector3(0, 0, missionSave.masterSpawn.adversary_spawnPlanes[i].Bearing));
        }
        AircraftCount_B.text = lenth.ToString();
        CreateAdversaryAircraftsIconOnMap(cord, Bearing);

        //Sams
        List<Vector3> SamsCord = new List<Vector3>();
        List<Vector3> SamsBearing = new List<Vector3>();
        if (Scenario == MissionInfo.Scenario.ATG)
        {
            SamsCount_B.gameObject.SetActive(true);
            SamsRoleButton.SetActive(true);
            
            ATG_Text.SetActive(true);
            int Samslenth = missionSave.masterSpawn.sams_spawnPlanes.Count;

            for (i = 0; i < Samslenth; i++)
            {
                SamsCord.Add(new Vector3(missionSave.masterSpawn.sams_spawnPlanes[i].Latitude, missionSave.masterSpawn.sams_spawnPlanes[i].Longitude, 0));
                SamsBearing.Add(new Vector3(0, 0, missionSave.masterSpawn.sams_spawnPlanes[i].Bearing));
            }
            SamsCount_B.text = Samslenth.ToString();
        }
        CreateAdversarySamsIconOnMap(SamsCord, SamsBearing);
        //warship
        List<Vector3> WarshipCord = new List<Vector3>();
        List<Vector3> WarshipBearing = new List<Vector3>();
        if (Scenario == MissionInfo.Scenario.ATS)
        {
            WarshipCount_B.gameObject.SetActive(true);
            WarshipRoleButton.SetActive(true);
            ATS_Text.SetActive(true);
            int Warshiplenth = missionSave.masterSpawn.Warship_spawnPlanes.Count;

            for (i = 0; i < Warshiplenth; i++)
            {
                WarshipCord.Add(new Vector3(missionSave.masterSpawn.Warship_spawnPlanes[i].Latitude, missionSave.masterSpawn.Warship_spawnPlanes[i].Longitude, 0));
                WarshipBearing.Add(new Vector3(0, 0, missionSave.masterSpawn.Warship_spawnPlanes[i].Bearing));
            }
            WarshipCount_B.text = Warshiplenth.ToString();
        }
        CreateAdversaryWarshipIconOnMap(WarshipCord, WarshipBearing);

        //Neutral
        List<Vector3> NeutralCord = new List<Vector3>();
        List<Vector3> NeutralBearing = new List<Vector3>();
        int Neutrallenth = missionSave.masterSpawn.neutral_spawnPlanes.Count;

        for (i = 0; i < Neutrallenth; i++)
        {
            NeutralCord.Add(new Vector3(missionSave.masterSpawn.neutral_spawnPlanes[i].Latitude, missionSave.masterSpawn.neutral_spawnPlanes[i].Longitude, 0));
            NeutralBearing.Add(new Vector3(0, 0, missionSave.masterSpawn.neutral_spawnPlanes[i].Bearing));
        }
        //NeutralCount_B.text = Neutrallenth.ToString();
        CreateAdversaryNeutralIconOnMap(NeutralCord, NeutralBearing);



    }

    public void MissionSave()
    {

        Master.missionInfo.scenario = Scenario;
        Master.missionInfo.Name = MissionName;

        Master.waypoints.Clear();
        Cordinates cordinates = new Cordinates();
        cordinates.Latitude = missionWaypoints.startWaypoint.localPosition.x;
        cordinates.Longitude = missionWaypoints.startWaypoint.localPosition.y;
        cordinates.position = missionWaypoints.MapToWordCords(missionWaypoints.MapCamera, missionWaypoints.MapUi, missionWaypoints.startWaypoint.GetComponent<RectTransform>(), 1000, 1000);
        Master.waypoints.Add(cordinates);

        foreach(Transform waypoint in missionWaypoints.Waypoints)
        {
            Cordinates cordinate = new Cordinates();
            cordinate.Latitude = waypoint.localPosition.x;
            cordinate.Longitude = waypoint.localPosition.y;
            cordinate.position = missionWaypoints.MapToWordCords(missionWaypoints.MapCamera, missionWaypoints.MapUi, waypoint.GetComponent<RectTransform>(), 1000, 1000);
            Master.waypoints.Add(cordinate);
        }

        //Convert Local Cordinate to World Cordinate
        //Ally
        int i = 0;
        foreach(AircraftPlanData plan in Master.ally_spawnPlanes)
        {
            RectTransform AllyIcon = AllyAircrafts[i].GetComponent<RectTransform>();
            Vector3 Position = missionWaypoints.MapToWordCords(missionWaypoints.MapCamera, missionWaypoints.MapUi, AllyIcon, 1000,1000);
            Position.y = plan.Altitude;
            plan.spawnPosition = Position;
            plan.Heading = AllyAircrafts[i].eulerAngles.z;
            i++;
        }
        //Adversary
        i = 0;
        foreach (AircraftPlanData plan in Master.adversary_spawnPlanes)
        {
            RectTransform AdversaryIcon = AdversaryAircrafts[i].GetComponent<RectTransform>();
            Vector3 Position = missionWaypoints.MapToWordCords(missionWaypoints.MapCamera, missionWaypoints.MapUi, AdversaryIcon, 1000, 1000);
            Position.y = plan.Altitude;
            plan.spawnPosition = Position;
            plan.Heading = AdversaryAircrafts[i].eulerAngles.z;
            i++;
        }
        //Sams
        i = 0;
        foreach (AircraftPlanData plan in Master.sams_spawnPlanes)
        {
            RectTransform SamsIcon = AdversarySams[i].GetComponent<RectTransform>();
            Vector3 Position = missionWaypoints.MapToWordCords(missionWaypoints.MapCamera, missionWaypoints.MapUi, SamsIcon, 1000, 1000);
            Position.y = plan.Altitude;
            plan.spawnPosition = Position;
            plan.Heading = AdversarySams[i].eulerAngles.z;
            i++;
        }

        //Warship
        i = 0;
        foreach (AircraftPlanData plan in Master.Warship_spawnPlanes)
        {
            RectTransform ShipIcon = AdversaryWarship[i].GetComponent<RectTransform>();
            Vector3 Position = missionWaypoints.MapToWordCords(missionWaypoints.MapCamera, missionWaypoints.MapUi, ShipIcon, 1000, 1000);
            Position.y = plan.Altitude;
            plan.spawnPosition = Position;
            plan.Heading = AdversaryWarship[i].eulerAngles.z;
            i++;
        }

        //Neutral
        i = 0;
        foreach (AircraftPlanData plan in Master.neutral_spawnPlanes)
        {
            RectTransform NeutralIcon = Neutrals[i].GetComponent<RectTransform>();
            Vector3 Position = missionWaypoints.MapToWordCords(missionWaypoints.MapCamera, missionWaypoints.MapUi, NeutralIcon, 1000, 1000);
            Position.y = plan.Altitude;
            plan.spawnPosition = Position;
            plan.Heading = Neutrals[i].eulerAngles.z;
            i++;
        }

        missionSave.SaveMission(MissionName,Master,true);
        //SceneManager.LoadScene(1);
    }

    static string IsValidString(string input)
    {
        // Check minimum length
        if (input.Length < 6)
        {
            return "The string must have a minimum length of 6 characters.";
        }

        // Check for forbidden characters using a regular expression
        string forbiddenCharactersPattern = @"[\/&""\\:;]"; // Matches '/', '&', '"', '\', ':', or ';'
        if (Regex.IsMatch(input, forbiddenCharactersPattern))
        {
            return "The string cannot contain '/', '&', '\"', '\\', ':', or ';'.";
        }

        // If both conditions pass, return null (indicating no error)
        return null;
    }


    public void SelectDropdownIcon(Dropdown dropdown)
    {
        GameObject Empty = dropdown.transform.parent.GetChild(1).gameObject;
        GameObject missile = dropdown.transform.parent.GetChild(2).gameObject;
        GameObject Bomb = dropdown.transform.parent.GetChild(3).gameObject;
        GameObject Fuel = dropdown.transform.parent.GetChild(4).gameObject;

        Empty.SetActive(false);
        missile.SetActive(false);
        Bomb.SetActive(false);
        Fuel.SetActive(false);

        // Get the selected option's name from the changedDropdown
        string selectedOptionName = dropdown.options[dropdown.value].text;
        if (selectedOptionName.ToLower().Contains("bomb"))
        {
            Bomb.SetActive(true);
        }else
        if (selectedOptionName.ToLower().Contains("fuel"))
        {
            Fuel.SetActive(true);
        }else
        if (selectedOptionName.ToLower().Contains("empty"))
        {
            Empty.SetActive(true);
        }else
        {
            missile.SetActive(true);
        }

        // Log or use the selected option's name as needed
        //Debug.Log("Selected Option in " + dropdown.name + ": " + selectedOptionName);
    }

    public float minMax(float value,InputField inputField, float min , float max)
    {
        if (value < min)
        {
            value = min;
            inputField.text = value.ToString();
        }else
        if (value > max)
        {
            value = max;
            inputField.text = value.ToString();
        }
        return value;
    }


    public void InitAPointAircraft()
    {
        float count = float.Parse(AircraftCount_A.text);
        float Altitude = float.Parse(Altitude_A.text);
        float speed = float.Parse(Speed_A.text);
        count = minMax(count, AircraftCount_A, 1, 10);
        speed = minMax(speed, Speed_A, 100, 400);
        Altitude = minMax(Altitude, Altitude_A, 100, 30000);

        String formation = Formation_A.options[Formation_A.value].text ;
        Master.ally_spawnPlanes = new List<AircraftPlanData>();
        List<Vector3> cords = Formation((int)count, formation);
        count = cords.Count;
        AircraftCount_A.text = count.ToString();
        CreateAllyAircraftsIconOnMap(cords);
        Master.missionInfo.FormationA = formation;

        for (int i = 0; i < count; i++)
        {

            AircraftPlanData aircraftPlanData = new AircraftPlanData(Default.ally_spawnPlanes[0]);
            aircraftPlanData.Latitude = cords[i].x;
            aircraftPlanData.Longitude = cords[i].y;
            aircraftPlanData.speed = speed;
            aircraftPlanData.Altitude = Altitude;
            aircraftPlanData.Bearing = AllyAircrafts[i].localEulerAngles.z;
            aircraftPlanData.Heading = AllyAircrafts[i].eulerAngles.z;
            Master.ally_spawnPlanes.Add(aircraftPlanData); 
        }
    }

    public void CreateAllyAircraftsIconOnMap(List<Vector3>cords,List<Vector3> Bearing=null)
    {
        //Remove Old Icons
        for (int k = 0; k < AllyAircrafts.Count; k++)
        {
            Destroy(AllyAircrafts[k].gameObject);
        }
        AllyAircrafts.Clear();

        for (int i = 0; i < cords.Count; i++)
        {
            GameObject obj = Instantiate(AllyAircraftPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(AllyAircraftParent);
            obj.transform.localScale = Vector3.one; 
            obj.transform.localPosition = cords[i];
            obj.transform.GetChild(0).GetComponent<Text>().text = (i+1).ToString();  
            if(Bearing==null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Bearing[i];
            AllyAircrafts.Add(obj.transform);
        }
    }

    public List<Vector3> Formation(int count,string formation)
    {
        if(count == 0) return new List<Vector3>();
        List<Vector3> cords = new List<Vector3>();
        if (formation=="V Shape")
        {
            count = Mathf.Clamp(count,3,5);
            float disX = 15f;
            float disY = 15f;
            float x = 0;
            float y = 0;
            string dir = "R";
            cords.Add(new Vector3(x, y, 0));
            for (int i = 1; i < count; i++)
            {
                if (dir == "R")
                {
                    dir = "L";
                    x += disX;
                    y -= disY;
                    cords.Add(new Vector3(x, y, 0));
                }
                else
                {
                    dir = "R";
                    cords.Add(new Vector3(-x, y, 0));
                }
            }
        }else
        if (formation == "Echelon Right")
        {
            count = Mathf.Clamp(count, 2, 4);
            float disX = 15f;
            float disY = 15f;
            float x = 0;
            float y = 0;
            //string dir = "R";
            cords.Add(new Vector3(x, y, 0));
            for (int i = 1; i < count; i++)
            {
                //if (dir == "R")
                //{
                    //dir = "L";
                    x += disX;
                    y -= disY;
                    cords.Add(new Vector3(x, y, 0));
                //}
                //else
                //{
                //    dir = "R";
                //    cords.Add(new Vector3(-x, y, 0));
                //}
            }
        }else
        if (formation == "Echelon Left")
        {
            count = Mathf.Clamp(count, 2, 4);
            float disX = 15f;
            float disY = 15f;
            float x = 0;
            float y = 0;
            //string dir = "R";
            cords.Add(new Vector3(x, y, 0));
            for (int i = 1; i < count; i++)
            {
                //if (dir == "R")
                //{
                //dir = "L";
                x -= disX;
                y -= disY;
                cords.Add(new Vector3(x, y, 0));
                //}
                //else
                //{
                //    dir = "R";
                //    cords.Add(new Vector3(-x, y, 0));
                //}
            }
        }
        else
        if (formation == "Line" || formation == "None")
        {

            float disX = 15f;
            float disY = 15f;
            float x = 0;
            float y = 0;
            //string dir = "R";
            cords.Add(new Vector3(x, y, 0));
            for (int i = 1; i < count; i++)
            {
                //if (dir == "R")
                //{
                //dir = "L";
                x += disX;
                //y  = disY;
                cords.Add(new Vector3(x, y, 0));
                //}
                //else
                //{
                //    dir = "R";
                //    cords.Add(new Vector3(-x, y, 0));
                //}
            }
        }else
        if (formation == "Trail")
        {
            count = Mathf.Clamp(count, 2, 6);
            float disX = 15f;
            float disY = 15f;
            float x = 0;
            float y = 0;
            //string dir = "R";
            cords.Add(new Vector3(x, y, 0));
            for (int i = 1; i < count; i++)
            {
                //if (dir == "R")
                //{
                //dir = "L";
                //x += disX;
                y  -= disY;
                cords.Add(new Vector3(x, y, 0));
                //}
                //else
                //{
                //    dir = "R";
                //    cords.Add(new Vector3(-x, y, 0));
                //}
            }
        }
        else
        if (formation == "Box")
        {
            count = 4;
            float disX = 15f;
            float disY = 15f;
            float x = disX;
            float y = disY;
            string dir = "R";
            cords.Add(new Vector3(-x, 0, 0));
            for (int i = 1; i < count; i++)
            {
                if (dir == "R")
                {
                    dir = "B";
                    //x += disX;

                    y -= disY;
                    y -= (i > 1 ? 10 : 0);
                    cords.Add(new Vector3(x, y, 0));
                    if (i > 1 && i<count-1)
                    {
                        cords.Add(new Vector3(-x, y, 0));
                        i++;
                    }
                }
                else
                if (dir == "B")
                {
                    dir = "L";
                    y -= disY;
                    cords.Add(new Vector3(x, y, 0));
                }
                else
                {
                    dir = "R";
                    cords.Add(new Vector3(-x, y, 0));

                }
            }
        }else
        if (formation == "Diamond")
        {
            count = 4;
            float disX = 15f;
            float disY = 15f;
            float x = disX;
            float y = 0;
            string dir = "R";
            cords.Add(new Vector3(0, 0, 0));
            for (int i = 1; i < count; i++)
            {
                if (dir == "R")
                {
                    dir = "L";
                    //x += disX;

                    y -= disY;
                    cords.Add(new Vector3(x, y, 0));
                }
                else
                if (dir == "B")
                {
                    dir = "R";
                    y -= disY;
                    y -= 10;
                    cords.Add(new Vector3(0, y, 0));
                }
                else
                {
                    dir = "B";
                    cords.Add(new Vector3(-x, y, 0));

                }
            }
        }


        return cords;
    }

    public void updateAllyAircraftPosition(Transform ally)
    {
        int index = AllyAircrafts.IndexOf(ally);
        Master.ally_spawnPlanes[index].Latitude = ally.localPosition.x;
        Master.ally_spawnPlanes[index].Longitude = ally.localPosition.y;
    }

    int GetChildIndex(Transform parent, Transform child)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) == child)
            {
                return i;
            }
        }
        return -1; // Return -1 if the child is not found under the parent
    }

    public void FixNumber(List<Transform> crafts)
    {
        int i = 1;
        foreach (Transform t in crafts)
        {
            t.GetChild(0).GetComponent<Text>().text = i.ToString();
            i++;
        }
    }

    public void AddCraft()
    {
        if (Role == "Sams")
        {
            GameObject obj = Instantiate(AdversarySamsPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(AdversarySamsParent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            if (Bearing == null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Vector3.zero;
            AdversarySams.Add(obj.transform);
            AircraftPlanData SamsData = new AircraftPlanData(Default.sams_spawnPlanes[0]);
            SamsData.Latitude = 0;
            SamsData.Longitude = 0;
            SamsData.speed = 0;
            SamsData.Altitude = 0;
            SamsData.Bearing = 0;
            SamsData.Heading = 0;
            Master.sams_spawnPlanes.Add(SamsData);
            InitRoleWeaponsPanel("Sams");
            FixNumber(AdversarySams);
            updateCraftIcon(obj);
        }
        else
        if (Role == "Warship")
        {
            GameObject obj = Instantiate(AdversaryWarshipPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(AdversaryWarshipParent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            if (Bearing == null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Vector3.zero;
            AdversaryWarship.Add(obj.transform);
            AircraftPlanData WarshipData = new AircraftPlanData(Default.Warship_spawnPlanes[0]);
            WarshipData.Latitude = 0;
            WarshipData.Longitude = 0;
            WarshipData.speed = 100;
            WarshipData.Altitude = 1000;
            WarshipData.Bearing = 0;
            WarshipData.Heading = 0;
            Master.Warship_spawnPlanes.Add(WarshipData);
            InitRoleWeaponsPanel("Warship");
            FixNumber(AdversaryWarship);
            updateCraftIcon(obj);
        }
        else
        if (Role == "Neutral")
        {
            GameObject obj = Instantiate(NeutralPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(NeutralParent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            if (Bearing == null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Vector3.zero;
            Neutrals.Add(obj.transform);
            AircraftPlanData NeutralData = new AircraftPlanData(Default.neutral_spawnPlanes[0]);
            NeutralData.Latitude = 0;
            NeutralData.Longitude = 0;
            NeutralData.speed = 800;
            NeutralData.Altitude = 1000;
            NeutralData.Bearing = 0;
            NeutralData.Heading = 0;
            Master.neutral_spawnPlanes.Add(NeutralData);
            InitRoleWeaponsPanel("Neutral");
            FixNumber(Neutrals);
            updateCraftIcon(obj);
        }
    }

    public void RemoveCraft()
    {
        //Sams
        if (Role == "Sams")
        {
            int i = 0;
            foreach (Transform sam in AdversarySams)
            {
                if (i == SelectedAircraft)
                {
                    Master.sams_spawnPlanes.RemoveAt(i);
                    Destroy(sam.gameObject);
                    AdversarySams.RemoveAt(i);
                    InitRoleWeaponsPanel("Sams");
                    break;
                }
                i++;
            }
            FixNumber(AdversarySams);
        }
        else
        //Warship
        if (Role == "Warship")
        {
            int i = 0;
            foreach (Transform ship in AdversaryWarship)
            {
                if (i == SelectedAircraft)
                {
                    Master.Warship_spawnPlanes.RemoveAt(i);
                    Destroy(ship.gameObject);
                    AdversaryWarship.RemoveAt(i);
                    InitRoleWeaponsPanel("Warship");
                    break;
                }
                i++;
            }
            FixNumber(AdversaryWarship);
        }
        else
        //Neutral
        if (Role == "Neutral")
        {
            int i = 0;
            foreach (Transform Neutral in Neutrals)
            {
                if (i == SelectedAircraft)
                {
                    Master.neutral_spawnPlanes.RemoveAt(i);
                    Destroy(Neutral.gameObject);
                    Neutrals.RemoveAt(i);
                    InitRoleWeaponsPanel("Neutral");
                    break;
                }
                i++;
            }
            FixNumber(Neutrals);
        }


    }

    public void InitBPointAircraft(bool Remove = false)
    {
        float AircraftCount = float.Parse(AircraftCount_B.text);
        float SamsCount = float.Parse(SamsCount_B.text);
        float WarshipCount = float.Parse(WarshipCount_B.text);
        float Altitude = float.Parse(Altitude_B.text);
        float speed = float.Parse(Speed_B.text);
        string formation = Formation_B.options[Formation_B.value].text;
        AircraftCount = minMax(AircraftCount, AircraftCount_B, 0, 10);
        speed = minMax(speed, Speed_B, 100, 400);
        Altitude = minMax(Altitude, Altitude_A, 100, 30000);
        Master.missionInfo.FormationB = formation;

        if (Scenario ==  MissionInfo.Scenario.ATG)
        {
            SamsCount = minMax(SamsCount, SamsCount_B, 0, 10);
        }
        if(Scenario == MissionInfo.Scenario.ATS)
        {
            WarshipCount = minMax(WarshipCount, WarshipCount_B, 0, 10);
        }

        if (Remove)
        {
            if (missionWaypoints.Waypoints.Count > 0)
            {
                return;
            }
            AircraftCount = 0;
            SamsCount = 0;
            WarshipCount = 0;
        }
        //Spawn Aircrafts
        Master.adversary_spawnPlanes = new List<AircraftPlanData>();
        List<Vector3> AircraftsCords = Formation((int)AircraftCount, formation);
        AircraftCount = AircraftsCords.Count;
        AircraftCount_B.text = AircraftCount.ToString();
        CreateAdversaryAircraftsIconOnMap(AircraftsCords);

        for (int i = 0; i < AircraftCount; i++)
        {
            AircraftPlanData aircraftPlanData = new AircraftPlanData(Default.adversary_spawnPlanes[0]);
            aircraftPlanData.Latitude = AircraftsCords[i].x;
            aircraftPlanData.Longitude = AircraftsCords[i].y;
            aircraftPlanData.speed = speed;
            aircraftPlanData.Altitude = Altitude;
            aircraftPlanData.Bearing = AdversaryAircrafts[i].localEulerAngles.z;
            aircraftPlanData.Heading = AdversaryAircrafts[i].eulerAngles.z;
            Master.adversary_spawnPlanes.Add(aircraftPlanData);
        }
        /*
        //Spawn Sams
        Master.sams_spawnPlanes = new List<AircraftPlanData>();
        List<Vector3> SamsCords = Formation((int)SamsCount, "V Shape");
        CreateAdversarySamsIconOnMap(SamsCords);
        for (int i = 0; i < SamsCount; i++)
        {
            AircraftPlanData SamsData = new AircraftPlanData(Default.sams_spawnPlanes[0]);
            AdversarySams[i].localPosition = SamsCords[i];
            Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, AdversarySams[i], 1000, 1000);
            Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
            SamsData.Latitude = latlongs.x;
            SamsData.Longitude = latlongs.y;
            SamsData.speed = speed;
            SamsData.Altitude = Altitude;
            SamsData.Bearing = AdversarySams[i].localEulerAngles.z;
            SamsData.Heading = AdversarySams[i].eulerAngles.z;
            Master.sams_spawnPlanes.Add(SamsData);
        }
        

        //Spawn Warship
        Master.Warship_spawnPlanes = new List<AircraftPlanData>();
        List<Vector3> warshipCords = Formation((int)WarshipCount, "V Shape");
        CreateAdversaryWarshipIconOnMap(warshipCords);

        for (int i = 0; i < WarshipCount; i++)
        {
            AircraftPlanData WarshipData = new AircraftPlanData(Default.Warship_spawnPlanes[0]);
            AdversaryWarship[i].localPosition = warshipCords[i];
            Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, AdversaryWarship[i], 1000, 1000);
            Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
            WarshipData.Latitude = latlongs.x;
            WarshipData.Longitude = latlongs.y;
            WarshipData.speed = speed;
            WarshipData.Altitude = Altitude;
            WarshipData.Bearing = AdversaryWarship[i].localEulerAngles.z;
            WarshipData.Heading = AdversaryWarship[i].eulerAngles.z;
            Master.Warship_spawnPlanes.Add(WarshipData);
        }
        */
    }

    public void CreateAdversaryAircraftsIconOnMap(List<Vector3> cords, List<Vector3> Bearing = null)
    {
        //Remove Old Icons
        for (int k = 0; k < AdversaryAircrafts.Count; k++)
        {
            Destroy(AdversaryAircrafts[k].gameObject);
        }
        AdversaryAircrafts.Clear();

        for (int i = 0; i < cords.Count; i++)
        {
            GameObject obj = Instantiate(AdversaryAircraftPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(AdversaryAircraftParent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = cords[i];
            obj.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            if (Bearing == null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Bearing[i];
            AdversaryAircrafts.Add(obj.transform);
        }
    }
    public void CreateAdversarySamsIconOnMap(List<Vector3> cords, List<Vector3> Bearing = null)
    {
        //Remove Old Icons
        for (int k = 0; k < AdversarySams.Count; k++)
        {
            Destroy(AdversarySams[k].gameObject);
        }
        AdversarySams.Clear();

        for (int i = 0; i < cords.Count; i++)
        {
            GameObject obj = Instantiate(AdversarySamsPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(AdversarySamsParent);
            obj.transform.localScale = Vector3.one;
            Vector2 world = missionWaypoints.LatLongToWorld(new Vector2(cords[i].x, cords[i].y));
            Vector3 viewport = missionWaypoints.WordCordsToMap(missionWaypoints.MapCamera, new Vector3(world.x, 0, world.y), 1000, 1000);

            obj.transform.localPosition = viewport;
            if (Bearing == null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Bearing[i];
            obj.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            obj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = cords[i].x + "," + cords[i].y;
            AdversarySams.Add(obj.transform);
        }
    }
    public void CreateAdversaryWarshipIconOnMap(List<Vector3> cords, List<Vector3> Bearing = null)
    {
        //Remove Old Icons
        for (int k = 0; k < AdversaryWarship.Count; k++)
        {
            Destroy(AdversaryWarship[k].gameObject);
        }
        AdversaryWarship.Clear();

        for (int i = 0; i < cords.Count; i++)
        {
            GameObject obj = Instantiate(AdversaryWarshipPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(AdversaryWarshipParent);
            obj.transform.localScale = Vector3.one;
            Vector2 world = missionWaypoints.LatLongToWorld(new Vector2(cords[i].x, cords[i].y));
            Vector3 viewport = missionWaypoints.WordCordsToMap(missionWaypoints.MapCamera, new Vector3(world.x, 0, world.y), 1000, 1000);

            obj.transform.localPosition = viewport;
            if (Bearing == null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Bearing[i];
            obj.transform.GetChild(0).GetComponent<Text>().text = (i+1).ToString();
            obj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = cords[i].x + "," + cords[i].y;
            AdversaryWarship.Add(obj.transform);
        }
    }

    public void CreateAdversaryNeutralIconOnMap(List<Vector3> cords, List<Vector3> Bearing = null)
    {
        //Remove Old Icons
        for (int k = 0; k < Neutrals.Count; k++)
        {
            Destroy(Neutrals[k].gameObject);
        }
        Neutrals.Clear();

        for (int i = 0; i < cords.Count; i++)
        {
            GameObject obj = Instantiate(NeutralPrefab);
            obj.SetActive(true);
            obj.transform.SetParent(NeutralParent);
            obj.transform.localScale = Vector3.one;
            Vector2 world = missionWaypoints.LatLongToWorld(new Vector2(cords[i].x, cords[i].y));
            Vector3 viewport = missionWaypoints.WordCordsToMap(missionWaypoints.MapCamera, new Vector3(world.x, 0, world.y), 1000, 1000);

            obj.transform.localPosition = viewport;
            if (Bearing == null)
                obj.transform.localRotation = Quaternion.identity;
            else
                obj.transform.localEulerAngles = Bearing[i];
            obj.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            obj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = cords[i].x + "," + cords[i].y;
            Neutrals.Add(obj.transform);
        }
    }

    public void updateAdversaryAircraftPosition(Transform adversary)
    {
        int index = AdversaryAircrafts.IndexOf(adversary);
        Master.adversary_spawnPlanes[index].Latitude = adversary.localPosition.x;
        Master.adversary_spawnPlanes[index].Longitude = adversary.localPosition.y;
    }

    public void InitRoleWeaponsPanel(string type)
    {
        SelectedAircraft = 0;
        Role = type;
        int count = 0;

        if (Role == "Ally")
        {
            count = Master.ally_spawnPlanes.Count;
        }
        else
        if (Role == "Adversary")
        {
            count = Master.adversary_spawnPlanes.Count;
        }
        else
        if (Role == "Sams")
        {
            count = Master.sams_spawnPlanes.Count;
        }
        else
        if (Role == "Warship")
        {
            count = Master.Warship_spawnPlanes.Count;
        }
        else
        if(Role == "Neutral")
        {
            count = Master.neutral_spawnPlanes.Count;
        }



        //Genrate Select Aircraft Icons
        Transform Panel = ScrollBarContent.GetChild(0);

        //Remove
        int coun = Panel.childCount;
        for(int s = coun-1; s > 0; s--)
        {
            Destroy(Panel.GetChild(s).gameObject);
        }

        //Create
        float width = 30;
        for (int i = 0;i < count;i++)
        {
            AircraftPlanData data;
            if (Role == "Ally")
            {
                data = Master.ally_spawnPlanes[i];
            }
            else
            if (Role == "Adversary")
            {
                data = Master.adversary_spawnPlanes[i];
            }
            else
            if (Role == "Sams")
            {
                data = Master.sams_spawnPlanes[i];
            }
            else
            if (Role == "Warship")
            {
                data = Master.Warship_spawnPlanes[i];
            }
            else
            if (Role == "Neutral")
            {
                data = Master.neutral_spawnPlanes[i];
            }
            else
            {
                data = Master.ally_spawnPlanes[i];
            }


            GameObject obj = Instantiate(CraftIconPrefab);
            obj.transform.SetParent(Panel);
            obj.SetActive(true);
            float iconWidth = 100;
            float iconoffset = 50;
            width += iconWidth + iconoffset;
            //NO
            obj.transform.GetChild(1).GetComponent<Text>().text = i==0?"Main":(i+1).ToString();
            //Name
            obj.transform.GetChild(2).GetComponent<Text>().text = data.name;
            Image image = obj.GetComponent<Image>();
            Image planeIcon = obj.transform.GetChild(0).GetComponent<Image>();
            Hardpoint1.SetActive(false);
            Hardpoint2.SetActive(false);
            //set Icon
            if (Role == "Ally")
            {
                planeIcon.sprite = AllySprite;
                Hardpoint1.SetActive(true);
            }
            else
            if (Role == "Adversary")
            {
                planeIcon.sprite = AdversarySprite;
                Hardpoint2.SetActive(true);
            }
            else
            if (Role == "Sams")
            {
                planeIcon.sprite = AdversarySamsSprite;
                Hardpoint2.SetActive(true);
            }
            else
            if (Role == "Warship")
            {
                planeIcon.sprite = AdversaryWarshipSprite;
                Hardpoint2.SetActive(true);
            }
            else
            if (Role == "Neutral")
            {
                planeIcon.sprite = NeutralSprite;
            }

            ////////////
            if (i==0)
            {
                obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                image.color = Color.white;
                fillDataSelectedAircraft(0);
            }
            else
            {
                obj.transform.localScale = Vector3.one;
                image.color = Color.gray;
            }
        }
        ScrollBarContent.sizeDelta = new Vector2(width,ScrollBarContent.sizeDelta.y);
        //Debug.Log(width);

    }

    public void selectAllyAircraft(Transform obj)
    {
        int count = obj.parent.childCount;
        for (int i = 0; i < count;i++)
        {
            Transform child = obj.parent.GetChild(i);
            Image image = child.GetComponent<Image>();
            if (child == obj)
            {
                SelectedAircraft = i-1;
                child.localScale = new Vector3(1.2f,1.2f,1.2f);
                image.color = Color.white;
                fillDataSelectedAircraft(i-1);
            }
            else
            {
                child.localScale = Vector3.one;
                image.color = Color.gray;
            }
        }
    }

    public void updateCraftIcon(GameObject gameObject)
    {
        //Sams
        int i = 0;
        foreach(Transform sam in AdversarySams)
        {
            if (sam == gameObject.transform)
            {
                Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, sam.transform, 1000, 1000);
                Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                //Master.sams_spawnPlanes[i].Latitude = sam.localPosition.x;
                //Master.sams_spawnPlanes[i].Longitude = sam.localPosition.y;
                //sam.GetChild(1).GetChild(0).GetComponent<Text>().text = sam.localPosition.x + "," + sam.localPosition.y;
                Master.sams_spawnPlanes[i].Latitude = latlongs.x;
                Master.sams_spawnPlanes[i].Longitude = latlongs.y;
                sam.GetChild(1).GetChild(0).GetComponent<Text>().text = latlongs.x + "," + latlongs.y;

                break;
            }
            i++;
        }
        //Warship
        i = 0;
        foreach (Transform ship in AdversaryWarship)
        {
            if (ship == gameObject.transform)
            {
                Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, ship.transform, 1000, 1000);
                Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                //Master.Warship_spawnPlanes[i].Latitude = ship.localPosition.x;
                //Master.Warship_spawnPlanes[i].Longitude = ship.localPosition.y;
                //ship.GetChild(1).GetChild(0).GetComponent<Text>().text = ship.localPosition.x + "," + ship.localPosition.y;
                Master.Warship_spawnPlanes[i].Latitude = latlongs.x;
                Master.Warship_spawnPlanes[i].Longitude = latlongs.y;
                ship.GetChild(1).GetChild(0).GetComponent<Text>().text = latlongs.x + "," + latlongs.y;

                break;
            }
            i++;
        }
        //Neutral
        i = 0;
        foreach (Transform Neutral in Neutrals)
        {
            if (Neutral == gameObject.transform)
            {
                Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, Neutral.transform, 1000, 1000);
                Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                //Master.neutral_spawnPlanes[i].Latitude = Neutral.localPosition.x;
                //Master.neutral_spawnPlanes[i].Longitude = Neutral.localPosition.y;
                //Neutral.GetChild(1).GetChild(0).GetComponent<Text>().text = Neutral.localPosition.x + "," + Neutral.localPosition.y;
                Master.neutral_spawnPlanes[i].Latitude = latlongs.x;
                Master.neutral_spawnPlanes[i].Longitude = latlongs.y;
                Neutral.GetChild(1).GetChild(0).GetComponent<Text>().text = latlongs.x + "," + latlongs.y;

                break;
            }
            i++;
        }

        if (Role == "Ally")
        {
            int index = AllyAircrafts.IndexOf(gameObject.transform);
            if (index == SelectedAircraft)
            {
                //Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, gameObject.transform, 1000, 1000);
                //Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                Latitude.text = gameObject.transform.localPosition.x.ToString();
                Longitude.text = gameObject.transform.localPosition.y.ToString();
                //AdversarySams[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text+","+Longitude.text;
                //Latitude.text = latlongs.x.ToString();
                //Longitude.text = latlongs.y.ToString();
                //AllyAircrafts[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;

            }
        }
        else
        if (Role == "Adversary")
        {
            int index = AdversaryAircrafts.IndexOf(gameObject.transform);
            if (index == SelectedAircraft)
            {
                //Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, gameObject.transform, 1000, 1000);
                //Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                Latitude.text = gameObject.transform.localPosition.x.ToString();
                Longitude.text = gameObject.transform.localPosition.y.ToString();
                //AdversarySams[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text+","+Longitude.text;
                //Latitude.text = latlongs.x.ToString();
                //Longitude.text = latlongs.y.ToString();
                //AllyAircrafts[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;

            }
        }
        else
        if (Role == "Sams")
        {
            int index = AdversarySams.IndexOf(gameObject.transform);
            if (index == SelectedAircraft)
            {
                Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, gameObject.transform, 1000, 1000);
                Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                //Latitude.text = gameObject.transform.localPosition.x.ToString();
                //Longitude.text = gameObject.transform.localPosition.y.ToString();
                //AdversarySams[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text+","+Longitude.text;
                Latitude.text = latlongs.x.ToString();
                Longitude.text = latlongs.y.ToString();
                AdversarySams[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;

            }
        }
        else
        if (Role == "Warship")
        {
            int index = AdversaryWarship.IndexOf(gameObject.transform);
            if (index == SelectedAircraft)
            {
                Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, gameObject.transform, 1000, 1000);
                Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                //Latitude.text = gameObject.transform.localPosition.x.ToString();
                //Longitude.text = gameObject.transform.localPosition.y.ToString();
                Latitude.text = latlongs.x.ToString();
                Longitude.text = latlongs.y.ToString();
                AdversaryWarship[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;
            }
        }
        else
        if (Role == "Neutral")
        {
            int index = Neutrals.IndexOf(gameObject.transform);
            if (index == SelectedAircraft)
            {
                Vector3 worlds = missionWaypoints.MapToWordCords2(missionWaypoints.MapCamera, missionWaypoints.MapUi, gameObject.transform, 1000, 1000);
                Vector2 latlongs = missionWaypoints.worldToLatLong(new Vector2(worlds.x, worlds.z));
                //Latitude.text = gameObject.transform.localPosition.x.ToString();
                //Longitude.text = gameObject.transform.localPosition.y.ToString();
                Latitude.text = latlongs.x.ToString();
                Longitude.text = latlongs.y.ToString();

                //Neutrals[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;
                Neutrals[index].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;

            }
        }
    }

    public void fillDataSelectedAircraft(int count)
    {
        AircraftPlanData data;
        if (Role == "Ally")
        {
            data = Master.ally_spawnPlanes[count];
        }
        else
        if (Role == "Adversary")
        {
            data = Master.adversary_spawnPlanes[count];
        }
        else
        if (Role == "Sams")
        {
            data = Master.sams_spawnPlanes[count];
        }
        else
        if (Role == "Warship")
        {
            data = Master.Warship_spawnPlanes[count];
        }
        else
        if (Role == "Neutral")
        {
            data = Master.neutral_spawnPlanes[count];
        }
        else
        {
            data = Master.ally_spawnPlanes[count];
        }

        //Aircraft Position
        Latitude.text = data.Latitude.ToString();
        Longitude.text = data.Longitude.ToString();
        Altitude.text = data.Altitude.ToString();
        StartMode.value = (int)data.startMode;
        Bearing.text = data.Bearing.ToString();
        Speed.text = data.speed.ToString();

        //Weapons
        Chaffs.text = data.Chaffs.ToString();
        Flares.text = data.Flares.ToString();
        Bullets.text = data.Bullets.ToString();
        int i = 0;
        
        foreach(Dropdown dropdown in Role=="Ally"?Hardpoints_Type1:Hardpoints_Type2)
        {
            if(Role!="Sams"&&Role!="Warship"&&Role!="Neutral")
            dropdown.value = (Role == "Ally" ? Hardpoints_Data1 : Hardpoints_Data2)[i].Data.IndexOf(data.Hardpoints[i]);
            else 
            dropdown.value = 0;
            i++;
        }

        
       

        i = 0;
        foreach(MissileInfo info in data.missileInfo)
        {
            if (Role == "Ally" || Role == "Adversary")
            {
                missileDataInfo[i].Speed.text = data.missileInfo[i].Speed.ToString();
                missileDataInfo[i].Range.text = data.missileInfo[i].Range.ToString();
                missileDataInfo[i].TurnRadius.text = data.missileInfo[i].TurnRadius.ToString();
            }
            else
            {
                missileDataInfo2[i].Speed.text = data.missileInfo[i].Speed.ToString();
                missileDataInfo2[i].Range.text = data.missileInfo[i].Range.ToString();
                missileDataInfo2[i].TurnRadius.text = data.missileInfo[i].TurnRadius.ToString();
                string[] m = { "HQ12", "Akash" };
                int cont  = 0;
                foreach(string s in data.Hardpoints)
                {
                    if (s.Contains(m[i]))
                    {
                        cont++;
                    }
                }
                missileDataInfo2[i].count.text = cont.ToString();

            }
            i++;
        }


        //Radar
        Power.text = data.Power.ToString();
        Gain.text = data.Gain.ToString();
        Frequency.text = data.Frequency.ToString();
        Temp.text = data.Temp.ToString();
        NoiseFactor.text = data.NoiseFactor.ToString();
        MinimalNoise.text = data.MinimalNoise.ToString();
        TRCS.text = data.TRCS.ToString();

        float mile = 1852f;
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
        Coverage = Mathf.Round(Coverage / (mile / 1000));

        Range.text = Coverage.ToString();

        //Electronic Warfare
        EWPower.text = data.EWPower.ToString();
        EWFrequency.text = data.EWFrequency.ToString();
        EWGain.text = data.EWGain.ToString();
        ReceiveGain.text = data.RecieveGain.ToString();
        Threshod.text = data.Threshold.ToString();
        AutoChaffs.isOn = data.AutoChaffs;
        AutoFlares.isOn = data.AutoFlares;
        DIRCM.isOn = data.DIRCM;
        Jammer.isOn = data.Jammer;

        float pt = data.EWPower;
        float Gt = data.EWGain;
        float Gr = data.RecieveGain;
        float Pr = data.Threshold;
        float f = data.EWFrequency;
        float PT = 10 * Mathf.Log(pt / 0.001f);//watt to dBm
        //PR = PT + Gt - 32.4 - (20 * Mathf.Log(f)) - (20 * Mathf.Log(d)) + Gr;
        float D = Mathf.Exp((float)(PT + Gt - 32.4 - (20 * Mathf.Log(f)) + Gr - Pr) / 20f);
        EwRange.text = Mathf.Round(D /mile).ToString();


        //JAMMER Configuration
        TransmittedPower.text = data.TransmittedPower.ToString();
        RadarReceiverGain.text = data.RadarReceiverGain.ToString(); 
        RadarFrequency.text = data.RadarFrequency.ToString();
        TargetCrossSection.text = data.TargetCrossSection.ToString();
        TargetDistance.text = data.TargetDistance.ToString(); 
        JammerTransPower.text = data.JammerTransPower.ToString(); 
        JammerReceiverGain.text = data.JammerReceiverGain.ToString(); 
        JammerTransFreq.text = data.JammerTransFreq.ToString(); 
        TargetReceiJamSignalGain.text = data.TargetReceiJamSignalGain.ToString();

        float Tp = data.TransmittedPower;
        float RG = data.RadarReceiverGain;
        float Rf = data.RadarFrequency * 1000000000;
        float σ = data.TargetCrossSection;
        float TD = data.TargetDistance * 1.852f * 1000;//NM to m
        float Pj = data.JammerTransPower;
        float Gj = data.JammerReceiverGain;
        float Fj = data.JammerTransFreq * 1000000000;
        float Grj = data.TargetReceiJamSignalGain;


        // Convert transmitter power to dBm
        var Ptt = 10 * Mathf.Log10(Tp / 0.001f); // Watt to dBm conversion

        // Calculate Signal (S)
        float S = Ptt + (2 * RG) - 103 - (20 * Mathf.Log10(Rf)) - (40 * Mathf.Log10(TD * 2)) + (10 * Mathf.Log10(σ));

        // Convert jamming power to dBm
        var PJ = 10 * Mathf.Log10(Pj / 0.001f); // Watt to dBm conversion

        // Calculate Jamming (J)
        float J = PJ + Gj - 32 - (20 * Mathf.Log10(Fj)) - (20 * Mathf.Log10(TD)) + Grj;

        // Calculate Jamming-to-Signal Ratio (JSR)
        float jS = 0;//= J / S;
        jS = (PJ + Gj +  (20 * Mathf.Log10(TD)) ) - (Ptt + RG  + (20 * Mathf.Log10(TD * 2)));

        SignalRatio.text = jS.ToString();

    }



    public void SaveAircraftPositionData(bool apply)
    {
        AircraftPlanData data;
        if (Role == "Ally")
        {
            data = Master.ally_spawnPlanes[SelectedAircraft];
            //Icon Position Update
            AllyAircrafts[SelectedAircraft].localPosition = new Vector3(float.Parse(Latitude.text), float.Parse(Longitude.text), 0);
            AllyAircrafts[SelectedAircraft].localEulerAngles = new Vector3(0, 0, float.Parse(Bearing.text));
        }
        else
        if (Role == "Adversary")
        {
            data = Master.adversary_spawnPlanes[SelectedAircraft];
            //Icon Position Update
            AdversaryAircrafts[SelectedAircraft].localPosition = new Vector3(float.Parse(Latitude.text), float.Parse(Longitude.text), 0);
            AdversaryAircrafts[SelectedAircraft].localEulerAngles = new Vector3(0, 0, float.Parse(Bearing.text));
        }
        else
        if (Role == "Sams")
        {
            data = Master.sams_spawnPlanes[SelectedAircraft];
            Vector2 world = missionWaypoints.LatLongToWorld(new Vector2(float.Parse(Latitude.text), float.Parse(Longitude.text)));
            Vector3 viewport = missionWaypoints.WordCordsToMap(missionWaypoints.MapCamera, new Vector3(world.x, 0, world.y), 1000, 1000);

            //Icon Position Update
            //AdversarySams[SelectedAircraft].localPosition = new Vector3(float.Parse(Latitude.text), float.Parse(Longitude.text), 0);
            AdversarySams[SelectedAircraft].localPosition = new Vector3(viewport.x, viewport.y, 0);
            AdversarySams[SelectedAircraft].localEulerAngles = new Vector3(0, 0, float.Parse(Bearing.text));
            AdversarySams[SelectedAircraft].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;
        }
        else
        if (Role == "Warship")
        {
            data = Master.Warship_spawnPlanes[SelectedAircraft];
            Vector2 world = missionWaypoints.LatLongToWorld(new Vector2(float.Parse(Latitude.text), float.Parse(Longitude.text)));
            Vector3 viewport = missionWaypoints.WordCordsToMap(missionWaypoints.MapCamera, new Vector3(world.x, 0, world.y), 1000, 1000);

            //Icon Position Update
            //AdversaryWarship[SelectedAircraft].localPosition = new Vector3(float.Parse(Latitude.text), float.Parse(Longitude.text), 0);
            AdversaryWarship[SelectedAircraft].localPosition = new Vector3(viewport.x, viewport.y, 0);
            AdversaryWarship[SelectedAircraft].localEulerAngles = new Vector3(0, 0, float.Parse(Bearing.text));
            AdversaryWarship[SelectedAircraft].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;
        }
        else
        if (Role == "Neutral")
        {
            data = Master.neutral_spawnPlanes[SelectedAircraft];

            Vector2 world = missionWaypoints.LatLongToWorld(new Vector2(float.Parse(Latitude.text), float.Parse(Longitude.text)));
            Vector3 viewport = missionWaypoints.WordCordsToMap(missionWaypoints.MapCamera, new Vector3(world.x, 0, world.y), 1000, 1000);


            //Icon Position Update
            //Neutrals[SelectedAircraft].localPosition = new Vector3(float.Parse(Latitude.text), float.Parse(Longitude.text), 0);
            Neutrals[SelectedAircraft].localPosition = new Vector3(viewport.x, viewport.y, 0);
            Neutrals[SelectedAircraft].localEulerAngles = new Vector3(0, 0, float.Parse(Bearing.text));
            Neutrals[SelectedAircraft].GetChild(1).GetChild(0).GetComponent<Text>().text = Latitude.text + "," + Longitude.text;
        }
        else
        {
            return;
        }
        data.Latitude = float.Parse(Latitude.text);
        data.Longitude = float.Parse(Longitude.text);
        data.Altitude = minMax(float.Parse(Altitude.text), Altitude, 200f, 50000f); 
        data.startMode = (AircraftPlanData.StartMode)StartMode.value;
        data.Bearing = float.Parse(Bearing.text);
        data.speed = minMax(float.Parse(Speed.text), Speed, 100f, 500f); 
        

        if (apply)
        {
            int d = 0;
            foreach(AircraftPlanData item in Role=="Ally"?Master.ally_spawnPlanes:(Role=="Adversary"?Master.adversary_spawnPlanes:(Role=="Sams"?Master.sams_spawnPlanes: (Role=="Warship"?Master.Warship_spawnPlanes: (Role == "Neutral" ? Master.neutral_spawnPlanes : Master.ally_spawnPlanes)))))
            {
                if (d != SelectedAircraft)
                {
                    //item.Latitude = data.Latitude; 
                    //item.Longitude = data.Longitude;
                    item.Altitude = data.Altitude;
                    item.startMode = data.startMode;
                    item.speed = data.speed;
                    if(Role == "Neutral")
                    {
                        Neutrals[d].localEulerAngles = new Vector3(0, 0, data.Bearing);
                        item.Bearing = data.Bearing;
                        item.Heading = data.Heading;
                    }
                    else
                    if(Role == "Warship")
                    {
                        AdversaryWarship[d].localEulerAngles = new Vector3(0, 0, data.Bearing);
                        item.Bearing = data.Bearing;
                        item.Heading = data.Heading;
                    }
                }
                d++;
            }
        }
    }

    public void SaveAircraftWeaponsData(bool apply)
    {
        AircraftPlanData data;// = Master.ally_spawnPlanes[SelectedAircraft];
        if (Role == "Ally")
        {
            data = Master.ally_spawnPlanes[SelectedAircraft];
        }
        else
            if (Role == "Adversary")
        {
            data = Master.adversary_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Neutral")
        {
            data = Master.neutral_spawnPlanes[SelectedAircraft];
        }
        else
        {
            return;
        }
        data.Chaffs = (int)minMax(float.Parse(Chaffs.text), Chaffs, 0, 500f);
        data.Flares = (int)minMax(float.Parse(Flares.text), Flares, 0, 500f);
        data.Bullets = (int)minMax(float.Parse(Bullets.text), Bullets, 0, 3000f);
        int i = 0;
        foreach (Dropdown dropdown in Role == "Ally" ? Hardpoints_Type1 : Hardpoints_Type2)
        {
            data.Hardpoints[i] = dropdown.options[dropdown.value].text;
            i++;
        }
        if (apply)
        {
            int d = 0;
            foreach (AircraftPlanData item in Role == "Ally" ? Master.ally_spawnPlanes : (Role == "Adversary" ? Master.adversary_spawnPlanes : (Role == "Neutral" ? Master.neutral_spawnPlanes : Master.ally_spawnPlanes)))
            {
                if (d != SelectedAircraft)
                {
                    item.Chaffs = data.Chaffs;
                    item.Flares = data.Flares;
                    item.Bullets = data.Bullets;
                    item.Hardpoints = new List<string>(data.Hardpoints);
                }
                d++;
            }
        }
    }


    public void SaveWeaponsConfigData(bool apply)
    {
        AircraftPlanData data;// = Master.ally_spawnPlanes[SelectedAircraft];
        if (Role == "Ally")
        {
            data = Master.ally_spawnPlanes[SelectedAircraft];
        }
        else
            if (Role == "Adversary")
        {
            data = Master.adversary_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Neutral")
        {
            data = Master.neutral_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Sams")
        {
            data = Master.sams_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Warship")
        {
            data = Master.Warship_spawnPlanes[SelectedAircraft];
        }
        else
        {
            return;
        }

        
         
        int i = 0;
        if (Role == "Ally" || Role == "Adversary" || Role == "Neutral")
        {
            foreach (Dropdown dropdown in Role == "Ally" ? Hardpoints_Type1 : Hardpoints_Type2)
            {
                data.Hardpoints[i] = dropdown.options[dropdown.value].text;
                i++;
            }
        }
        else
        {
            string[] m = { "HQ12", "Akash" };
            data.Hardpoints = new List<string>();
            i = 0;
            foreach (MissileDataInfo info in missileDataInfo2)
            {
                minMax(float.Parse(info.count.text), info.count, 0, 20f);
                for (int j=0;j<float.Parse(info.count.text);j++)
                {
                    data.Hardpoints.Add(m[i]);
                }
                i++;
            }
        }


        if (Role == "Ally" || Role == "Adversary" || Role == "Neutral")
        {
            //Ally Adversary
            i = 0;
            foreach (MissileDataInfo info in missileDataInfo)
            {
                float MinRange = data.missileInfo[i].RangeType == "CCM"?3:30f;
                float MaxRange = data.missileInfo[i].RangeType == "CCM"?30:500;

                data.missileInfo[i].Speed = minMax(float.Parse(missileDataInfo[i].Speed.text), missileDataInfo[i].Speed, 100f, 7000f);
                data.missileInfo[i].Range = minMax(float.Parse(missileDataInfo[i].Range.text), missileDataInfo[i].Range, MinRange, MaxRange);// * 1.60934f;
                data.missileInfo[i].TurnRadius = minMax(float.Parse(missileDataInfo[i].TurnRadius.text), missileDataInfo[i].TurnRadius, 10f, 1000f);
                i++;
            }
        }
        else
        {

            ///Sams ,Warships
            i = 0;
            foreach (MissileDataInfo info in missileDataInfo2)
            {
                float MinRange = data.missileInfo[i].RangeType == "CCM" ? 3 : 30f;
                float MaxRange = data.missileInfo[i].RangeType == "CCM" ? 30 : 500;

                data.missileInfo[i].Speed = minMax(float.Parse(missileDataInfo2[i].Speed.text), missileDataInfo2[i].Speed, 100f, 7000f);
                data.missileInfo[i].Range = minMax(float.Parse(missileDataInfo2[i].Range.text), missileDataInfo2[i].Range, MinRange, MaxRange);// * 1.60934f;
                data.missileInfo[i].TurnRadius = minMax(float.Parse(missileDataInfo2[i].TurnRadius.text), missileDataInfo2[i].TurnRadius, 10f, 1000f);
                i++;
            }
        }


        if (apply)
        {
            int d = 0;
            foreach (AircraftPlanData item in Role == "Ally" ? Master.ally_spawnPlanes : (Role == "Adversary" ? Master.adversary_spawnPlanes : (Role == "Neutral" ? Master.neutral_spawnPlanes : (Role == "Sams" ? Master.sams_spawnPlanes : (Role == "Warship" ? Master.Warship_spawnPlanes : Master.ally_spawnPlanes)))))
            {
                if (d != SelectedAircraft) 
                {
                    List<MissileInfo> missileInfos = new List<MissileInfo>(data.missileInfo);
                    item.missileInfo = missileInfos;
                    item.Hardpoints = new List<string>(data.Hardpoints);
                } 
                d++;
            }
        }
    }

    public void SaveAircraftRadarData(bool apply)
    {
        AircraftPlanData data;// = Master.ally_spawnPlanes[SelectedAircraft];
        if (Role == "Ally")
        {
            data = Master.ally_spawnPlanes[SelectedAircraft];
        }
        else
            if (Role == "Adversary")
        {
            data = Master.adversary_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Neutral")
        {
            data = Master.neutral_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Sams")
        {
            data = Master.sams_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Warship")
        {
            data = Master.Warship_spawnPlanes[SelectedAircraft];
        }
        else
        {
            return;
        }
        data.Power = minMax(float.Parse(Power.text), Power, 10f, 3000f);
        data.Gain = minMax(float.Parse(Gain.text), Gain, 1f, 5f);
        data.Frequency = minMax(float.Parse(Frequency.text), Frequency, 8f, 12f);
        data.Temp = minMax(float.Parse(Temp.text), Temp, 275f, 323f);
        data.NoiseFactor = minMax(float.Parse(NoiseFactor.text), NoiseFactor, 0.2f, 5f);
        data.MinimalNoise = minMax(float.Parse(MinimalNoise.text), MinimalNoise, 0.1f, 5f);
        data.TRCS = minMax(float.Parse(TRCS.text), TRCS, 0.1f, 0.4f);

        
        float mile = 1852f;
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
        Coverage = Mathf.Round(Coverage / (mile / 1000));
         
        Range.text = Coverage.ToString();


        if (apply)
        {
            int d = 0;
            foreach (AircraftPlanData item in Role == "Ally" ? Master.ally_spawnPlanes : (Role == "Adversary" ? Master.adversary_spawnPlanes : (Role == "Neutral" ? Master.neutral_spawnPlanes : (Role == "Sams" ? Master.sams_spawnPlanes : (Role == "Warship" ? Master.Warship_spawnPlanes : Master.ally_spawnPlanes)))))
            {
                if (d != SelectedAircraft)
                {
                    item.Power = data.Power;
                    item.Gain = data.Gain;
                    item.Frequency = data.Frequency;
                    item.Temp = data.Temp;
                    item.NoiseFactor = data.NoiseFactor;
                    item.MinimalNoise = data.MinimalNoise;
                    item.TRCS = data.TRCS;
                }
                d++;
            }
        }
    }

    public void SaveAircraftElectronicWarfareData(bool apply)
    {
        AircraftPlanData data;// = Master.ally_spawnPlanes[SelectedAircraft];
        if (Role == "Ally")
        {
            data = Master.ally_spawnPlanes[SelectedAircraft];
        }
        else
            if (Role == "Adversary")
        {
            data = Master.adversary_spawnPlanes[SelectedAircraft];
        }
        else
        if (Role == "Neutral")
        {
            data = Master.neutral_spawnPlanes[SelectedAircraft];
        }
        else
        {
            return;
        }

       

        data.EWPower = minMax(float.Parse(EWPower.text), EWPower, 100,2000);
        data.EWFrequency = minMax(float.Parse(EWFrequency.text), EWFrequency, 2, 12f);
        data.EWGain = minMax(float.Parse(EWGain.text), EWGain, 0.1f, 5f);
        data.RecieveGain = minMax(float.Parse(ReceiveGain.text), ReceiveGain, 0.1f, 5f);
        data.Threshold = minMax(float.Parse(Threshod.text), Threshod, -120f, -50f);
        data.AutoChaffs = AutoChaffs.isOn;
        data.AutoFlares = AutoFlares.isOn;
        data.DIRCM = DIRCM.isOn;
        data.Jammer = Jammer.isOn;
        float mile = 1852f;
        float pt = data.EWPower;
        float Gt = data.EWGain;
        float Gr = data.RecieveGain;
        float Pr = data.Threshold;
        float f = data.EWFrequency;
        float PT = 10 * Mathf.Log(pt / 0.001f);//watt to dBm
                                               //PR = PT + Gt - 32.4 - (20 * Mathf.Log(f)) - (20 * Mathf.Log(d)) + Gr;
        float D = Mathf.Exp((float)(PT + Gt - 32.4 - (20 * Mathf.Log(f)) + Gr - Pr) / 20f);
        EwRange.text = Mathf.Round(D / mile).ToString();


        //JAMMER Configuration
        data.TransmittedPower = minMax(float.Parse(TransmittedPower.text), TransmittedPower, 100, 3000); 
        data.RadarReceiverGain = minMax(float.Parse(RadarReceiverGain.text), RadarReceiverGain, 0.1f, 5f);
        data.RadarFrequency = minMax(float.Parse(RadarFrequency.text), RadarFrequency, 8, 12);
        data.TargetCrossSection = minMax(float.Parse(TargetCrossSection.text), TargetCrossSection, 0.1f, 0.4f);
        data.TargetDistance = minMax(float.Parse(TargetDistance.text), TargetDistance, 1f, 200f);
        data.JammerTransPower = minMax(float.Parse(JammerTransPower.text), JammerTransPower, 100f, 300000f);
        data.JammerReceiverGain = minMax(float.Parse(JammerReceiverGain.text), JammerReceiverGain, 0.1f, 500f);
        data.JammerTransFreq = minMax(float.Parse(JammerTransFreq.text), JammerTransFreq, 1, 12);
        data.TargetReceiJamSignalGain = minMax(float.Parse(TargetReceiJamSignalGain.text), TargetReceiJamSignalGain, 0.1f, 500f);

        float Tp = data.TransmittedPower;
        float RG = data.RadarReceiverGain;
        float Rf = data.RadarFrequency * 1000000000;
        float σ = data.TargetCrossSection;
        float TD = data.TargetDistance * 1.852f * 1000;//Nm to m
        float Pj = data.JammerTransPower;
        float Gj = data.JammerReceiverGain;
        float Fj = data.JammerTransFreq * 1000000000;
        float Grj = data.TargetReceiJamSignalGain;

        // Convert transmitter power to dBm
        var Ptt = 10 * Mathf.Log10(Tp / 0.001f); // Watt to dBm conversion

        // Calculate Signal (S)
        float S = Ptt + (2 * RG) - 103 - (20 * Mathf.Log10(Rf)) - (40 * Mathf.Log10(TD * 2)) + (10 * Mathf.Log10(σ));

        // Convert jamming power to dBm
        var PJ = 10 * Mathf.Log10(Pj / 0.001f); // Watt to dBm conversion

        // Calculate Jamming (J)
        float J = PJ + Gj - 32 - (20 * Mathf.Log10(Fj)) - (20 * Mathf.Log10(TD)) + Grj;
        
        // Calculate Jamming-to-Signal Ratio (JSR)
        float jS = J / S;
        jS = (PJ + Gj + (20 * Mathf.Log10(TD)) ) - (Ptt + RG + (20 * Mathf.Log10(TD * 2)));
        SignalRatio.text = jS.ToString();

        if (apply)
        {
            int d = 0;
            foreach (AircraftPlanData item in Role == "Ally" ? Master.ally_spawnPlanes : (Role == "Adversary" ? Master.adversary_spawnPlanes : (Role == "Neutral" ? Master.neutral_spawnPlanes : Master.ally_spawnPlanes)))
            {
                if (d != SelectedAircraft)
                {
                    item.EWPower = data.EWPower;
                    item.EWGain = data.EWGain;
                    item.EWFrequency = data.EWFrequency;
                    item.RecieveGain = data.RecieveGain;
                    item.Threshold = data.Threshold;
                    item.AutoChaffs = data.AutoChaffs;
                    item.AutoFlares = data.AutoFlares;
                    item.DIRCM = data.DIRCM;
                    item.Jammer = Jammer.isOn;

                    //JAMMER Configuration
                    item.TransmittedPower = data.TransmittedPower;
                    item.RadarReceiverGain = data.RadarReceiverGain;
                    item.RadarFrequency = data.RadarFrequency;
                    item.TargetCrossSection = data.TargetCrossSection;
                    item.TargetDistance = data.TargetDistance; 
                    item.JammerTransPower = data.JammerTransPower;
                    item.JammerReceiverGain = data.JammerReceiverGain;
                    item.JammerTransFreq = data.JammerTransFreq;
                    item.TargetReceiJamSignalGain = data.TargetReceiJamSignalGain;
                }
                d++;
            }
        }
    }


    public void LatLongUnitChange(bool unit)
    {
        if (!unit)
        {
            Lattext.text = "Latitude";
            Longtext.text = "Longitude";
            LatUnittext.text = "Deg°";
            LongUnittext.text = "Deg°";
        }
        else
        {
            Lattext.text = "X Offset";
            Longtext.text = "Y Offset";
            LatUnittext.text = "m";
            LongUnittext.text = "m";
        }
    }
}
