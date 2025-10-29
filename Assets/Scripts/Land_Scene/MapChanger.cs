using Esri.ArcGISMapsSDK.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChanger : MonoBehaviour
{
    public GameObject ATA, ATG, ATS;
    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            PlayerPrefs.SetInt("Scenerio", 0);
        ChangeScene();
    }
    public void ChangeScene()
    {
        return;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            switch (PlayerPrefs.GetInt("Scenerio"))
            {
                case 0: ATA.SetActive(true); Destroy(ATG); Destroy(ATS); break;
                case 1: ATG.SetActive(true); Destroy(ATA); Destroy(ATS); break;
                case 2: ATS.SetActive(true); Destroy(ATG); Destroy(ATA); break;
            }
        }
        else
        {
            switch (PlayerPrefs.GetInt("Scenerio"))
            {
                case 0: ATA.SetActive(true); ATS.SetActive(false); ATG.SetActive(false); break;
                case 1: ATG.SetActive(true); ATS.SetActive(false); ATA.SetActive(false); break;
                case 2: ATS.SetActive(true); ATA.SetActive(false); ATG.SetActive(false); break;
            }
        }
    }
}
