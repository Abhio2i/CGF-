//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;


//public class PlaneSpecsAndDetails : MonoBehaviour
//{
//    public string _name, type, model, country, pilot, skill,count;
//    public int unit;
//    public int callSign1,callSign2;
//    public List<float> spawnPositionsAtX, spawnPositionsAtY, spawnPositionsAtZ;
//    public bool radio, hiddenOnMap, hiddenOnPlanner, hiddenOnRadar;
//    public string callSign_code;
//    bool saveNow;
//    bool passOnce=true;

//    string modelType;
//    void SaveData()
//    {
//        BinaryFormatter binary=new BinaryFormatter();
//        SaveSystem data=new SaveSystem();
//        //print(Application.persistentDataPath);
//        FileStream file = new FileStream(Application.persistentDataPath + "/AI_PlaneSpecs/" +type+"/" + Time.realtimeSinceStartup + "Specs.fun", FileMode.Create);
//        data.name = _name;
//        data.callSign_code = callSign_code;
//        data.type = type;
//        data.model = model;
//        data.country = country;
//        data.count = count;
//        data.pilot = pilot;
//        data.skill = skill;
//        data.unit = unit;
//        data.callSign1 = callSign1;
//        data.callSign2 = callSign2;
//        data.hiddenOnMap = hiddenOnMap;
//        data.hiddenOnPlanner = hiddenOnPlanner;
//        data.hiddenOnRadar = hiddenOnRadar;
//        data.radio = radio;
//        data.spawnPositionsAtX = spawnPositionsAtX;
//        data.spawnPositionsAtY = spawnPositionsAtY;
//        data.spawnPositionsAtZ = spawnPositionsAtZ;
//       // data.spawnPositions = spawnPosition;
//        binary.Serialize(file, data);
//        file.Close();
        
//    }
//    void CheckCount()
//    {
       
//        if (spawnPositionsAtX.Count == int.Parse(count) && spawnPositionsAtX.Count == int.Parse(count) && spawnPositionsAtX.Count == int.Parse(count) && passOnce)
//        {
//            passOnce = false;
//            saveNow = true;
//        }
//    }

//    private void Update()
//    {
//        CheckCount();
//        if(saveNow)
//        {
//            SaveData();
//            //Debug.Log("saved");
//            saveNow = false;
//        }
//    }
//}
