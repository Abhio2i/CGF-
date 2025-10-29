//creating a database of every entity 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Scripts.Utility;
[System.Serializable]
public class SaveData
{
    [HideInInspector]public List<string> storePosition=new List<string>();
    [HideInInspector]public List<string> storeRosition=new List<string>();

    [HideInInspector] public List<float> storeGforce = new List<float>();
    [HideInInspector] public List<float> storeMissileCount = new List<float>();

    [HideInInspector] public List<string> storeEvents = new List<string>();
    [HideInInspector] public List<string> storeTime = new List<string>();

    [HideInInspector] public string setFileName;
    [HideInInspector] public string _NPC_type;
}
//    public void Initialize()
//    {
//        storePosition = new List<Vector3>();
//        storeRotation = new List<Vector3>();
        
//        storeGforce = new List<float>();
//        storeMissileCount=new List<float>();

//        storeTime=new List<string>();
//        storeEvents = new List<string>();

//        Debug.Log(Application.streamingAssetsPath);
//    }
//    public void SaveAllData(SaveData data)
//    {
//        WritingData writingData = new WritingData();
//        writingData.Initialize("EntityData", setFileName);
//        writingData.SaveAllToCSV(data);
//    }
//    public void show(int i)
//    {
//        Debug.Log(storeTime[i]);
//    }
    
//}
class WritingData :MonoBehaviour    
{
    //string filename = "";
    //public void Initialize(string saveFolderPath,string file)
    //{
    //    filename = Application.streamingAssetsPath  + "/" + saveFolderPath + "/" + file+".csv";
    //    TextWriter tw =new StreamWriter(filename,false);
    //    tw.WriteLine("type"+ "," + "pos" + "," + "rot" + "," + "G" + "," + "missile" + "," + "events" + ","+"time");
    //    tw.Close();
    //}
    //public void SaveAllToCSV(SaveData saveData)    
    //{
    //    if(saveData!= null)
    //    {
    //        if(saveData.storePosition.Count>0)
    //        {
    //            TextWriter tw = new StreamWriter(filename, true);                
    //            for(int i=0;i<saveData.storePosition.Count;i++)
    //            {
    //                tw.WriteLine(saveData._NPC_type + ","+saveData.storePosition[i] + "," + saveData.storeRotation[i] + "," + saveData.storeGforce[i]
    //                    + "," + saveData.storeMissileCount[i] + "," + saveData.storeEvents[i] + "," + saveData.storeTime[i]);
    //            }
    //            tw.Close();
    //            File.SetAttributes(filename, FileAttributes.ReadOnly);
    //            Debug.Log("saved "+filename);
    //        }
    //    }
    //}
}
