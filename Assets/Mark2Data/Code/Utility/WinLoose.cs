using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Evereal.VideoCapture;
using System;
using Assets.Scripts.Feed;
using Assets.Scripts.Utility;


public class WinLoose : MonoBehaviour
{
    public MasterSpawn masterspawn;
    public GameObject plane;
    public TMP_Text enemy;
    public TMP_Text ally;
    public TMP_Text TimerText;
    public string sceneName = "Feed_Scene";
    public bool trigger;
    private int enemyCount = 0;
    private int allyCount = 0;
    private bool isScanning = false;
    private bool changeSceneAllow = true;

    [SerializeField]Save saveData;
    [SerializeField]LoadingManager loadingManagerForFeed;


    void Start()
    {
        //float x = PlayerPrefs.GetInt("EnemySpawnCount") > 0 ? 60f : 0f;
        SavePlaneData();
        Starting();
        Destroy(TimerText.transform.parent.gameObject);
    }
    void SavePlaneData()
    {
        Save.startSave = true;
        //plane.GetComponent<SaveAllData>().enabled = true;
        //plane.GetComponent<PlaneData>().enabled = true;
    }
    IEnumerator Timer()
    {
        int i = 58;
        while (i >= -1)
        {
            yield return new WaitForSeconds(1f);
            TimerText.text = i.ToString();
            i--;
        }
        Destroy(TimerText.transform.parent.gameObject);
    }
    void Starting()
    {
        PlayerPrefs.SetInt("EnemyCount2",masterspawn.adversary_spawnPlanes.Count);
        PlayerPrefs.SetInt("AllyCount2", masterspawn.ally_spawnPlanes.Count);
        enemyCount = PlayerPrefs.GetInt("EnemyCount2");
        allyCount = PlayerPrefs.GetInt("AllyCount2")-1;
        if ((enemyCount == 0 && allyCount == 0)|| enemyCount==0)
        {
            enemy.text = "You did Not Select Enemy or Ally";
            ally.text = "But You Can Test The Simulator";
        }
        else
        {
            enemy.text = "Enemy :" + enemyCount.ToString("00");
            ally.text = "Ally :" + allyCount.ToString("00");
            Invoke(nameof(IsSanning), 5f);
        }
    }

    private void IsSanning()
    {
        isScanning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (plane.gameObject.activeSelf == false)
        {
            //Win
            isScanning = false;
            print("change by scan");
            //Invoke("ChangeScene",5f);
            GameManager.isPlayerDestroyed = true;
            ChangeScene(sceneName);

        }

        if (trigger)
        {
            print("change by trigger");
            ChangeScene(sceneName);
        }
        if (isScanning) 
        {
            enemyCount = PlayerPrefs.GetInt("EnemyCount2");
            allyCount = PlayerPrefs.GetInt("AllyCount2")-1;

            enemy.text = "Enemy : " + enemyCount.ToString("00");
            ally.text = "Ally : " + allyCount.ToString("00");

            if (enemyCount == 0)
            {
                //Win
                isScanning = false;
                print("change by scan");
                ChangeScene(sceneName);
            }
            if(allyCount == 0) 
            {
                //Loose;
                //isScanning = false;
                
            }
           
        }

        if(GameManager.isPlayerDestroyed)
        {
            print("change by plane");
            //Invoke("ChangeScene", 5f); 
            ChangeScene(sceneName);
        }
        try
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                print("change by key");
                ChangeScene(sceneName);
            }
        }
        catch { }
    }
    public void ChangeScene(string _name)
    {
        if (changeSceneAllow)
        {
            changeSceneAllow=false;
            if (saveData != null)
                saveData.SaveGame();
        }
    }
}
