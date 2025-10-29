using Assets.Scripts.Feed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using Assets.Scripts.Utility;
using TMPro;


public class SaveAllData : MonoBehaviour
{
    [SerializeField] PlaneData planeData;
    [SerializeField] string filename;
    [SerializeField] public SaveDataManager gameDataManager;

    [SerializeField] bool isEnemy, isPlayer, isAlly,isNeutral;

    string event_message="";
    public bool save;

    private bool isStoring;
    private string type;
    SaveData savedata;
    private void Awake()
    {
        Save.save = false;
        filename = (Random.Range(-100000f, 1000000f) * Random.Range(1, 10)) . ToString();
        planeData = GetComponent<PlaneData>();

        if (isAlly) type = "A";
        else if(isEnemy)type = "B";
        else if(isPlayer)type = "C";
        else if(isNeutral)type = "D";
    }
    private void Start()
    {
        savedata=new SaveData();
        savedata.setFileName=filename;
        savedata._NPC_type= type;
        StartCoroutine(nameof(savingDataRoutine));
    }
    public void SetMessage(string message)
    {
        event_message=message;
    }
    private void FixedUpdate()
    {
        if(Save.save)
        {
            SaveDataManager.saveAllDatas.Add(GetComponent<SaveAllData>());
            return;
        }
        SaveEverything();
    }
    int skip = 0;
    private void SaveEverything()
    {
        if (savedata != null)
        {
            string pos = planeData.planeTransform.position.x.ToString() +","+
                planeData.planeTransform.position.y.ToString() + "," +planeData.planeTransform.position.z.ToString();

            string rot = planeData.planeTransform.rotation.eulerAngles.x.ToString() + "," +
                planeData.planeTransform.rotation.eulerAngles.y.ToString() + "," + planeData.planeTransform.rotation.eulerAngles.z.ToString();

            savedata.storePosition.Add(pos);
            savedata.storeRosition.Add(rot);
            savedata.storeGforce.Add(planeData.g_force);
            savedata.storeMissileCount.Add(planeData.missileCount + 30f);
            savedata.storeEvents.Add(event_message);
            savedata.storeTime.Add(NewClock.time);

            isStoring = false;
        }
    }
    IEnumerator savingDataRoutine()
    {
        while(true)
        {
            yield return null;
            isStoring= true;
        }
    }
    
    public void GameSave()
    {
        print("Callled!");
        string file = (Time.realtimeSinceStartup * Random.Range(1, 10f)).ToString();
        if (savedata == null) print("savedata");
        if (gameDataManager == null) { gameDataManager = FindObjectOfType<SaveDataManager>(); }
        gameDataManager.writeFile(savedata, Application.dataPath + "/EntityData/" + file + ".dat");
        print("Saved");
        
    }
}
