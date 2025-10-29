using Assets.Scripts.Feed_Scene.newFeed;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool active;
    public static bool isPlayerDestroyed;
    public static int frameNumber;

    [SerializeField] NewSaveManager saveManager;
    [SerializeField] LoadScreen loading;
    private void Awake()
    {
        isPlayerDestroyed = false;
        frameNumber = 0;
    }
    
    private void FixedUpdate()
    {

        return;
        Save.saveFunctionsCount = FindObjectsOfType<SaveEntityDatas>().Length;
        //print(Save.saveFunctionsCount + " " + Save.currentCount);
        if (!Save.startSave) return;
        if (!Save.save)
        {
            frameNumber++;
            return;
        }                                                      
        print(frameNumber);
        SaveGameRelatedData();
        
    }
    bool pass=true;
    void SaveGameRelatedData()
    {
        if (Save.currentCount == Save.saveFunctionsCount)
        {
            ChangeScene(5);
            return;
        }
        else if (NewSaveManager.saveAllDatas.Count != 0 || NewSaveManager.saveAllDatas == null)
        {
            NewSaveManager.saveAllDatas[Save.currentCount].SaveFinalData();
            ++Save.currentCount;
        }
    }
    public void ChangeScene(int index)
    {
        //SceneManager.LoadScene(index);
        if (pass)
        {
            //loading.LoadScene(index);
            ReloadScene();
            pass = false;
        }
    }

    public void ReloadScene()
    {
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}
