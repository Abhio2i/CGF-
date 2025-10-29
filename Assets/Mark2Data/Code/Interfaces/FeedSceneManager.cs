using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using camera.Record;
using Assets.Code.ShareData;
public class FeedSceneManager : MonoBehaviour
{
    [SerializeField] GameObject videoPanel;
    [SerializeField] GameObject dataPanel;
    [SerializeField] GameObject logsPanel;

    [SerializeField] GameObject videoPanelObject;
    [SerializeField] GameObject dataPanelObject;
    [SerializeField] GameObject logsPanelObject;

    [SerializeField] Slider camera;
    [SerializeField] Slider data;
    [SerializeField] GameObject waitText;
    [SerializeField] GameObject options;
    

    public float waitTime = 60f;

    private void Awake()
    {
        
        videoPanelObject.SetActive(false);
        dataPanelObject.SetActive(false);
        logsPanelObject.SetActive(false);
        videoPanel.SetActive(false);
        dataPanel.SetActive(false);
        logsPanel.SetActive(false);
        options.SetActive(false);
        waitText.SetActive(true);
    }
    bool stop;
    private void Update()
    {
        if (waitTime <= 0)
        {
            waitText.SetActive(false);
            options.SetActive(true);
            return;
        }
        else
        {
            waitTime -= Time.fixedDeltaTime;
        }
    }
    public void ActivateWindowField()
    {
        videoPanel.SetActive(true);
        dataPanel.SetActive(false);
        logsPanel.SetActive(false);

        videoPanelObject.SetActive(true);
    }
    public void ActivateDataField()
    {
        videoPanel.SetActive(false);
        dataPanel.SetActive(true);
        logsPanel.SetActive(false);

        dataPanelObject.SetActive(true);
    }
    public void ActivateLogsField()
    {
        videoPanel.SetActive(false);
        dataPanel.SetActive(false);
        logsPanel.SetActive(true);

        logsPanelObject.SetActive(true);
    }

    public void Reset()
    {
        camera.maxValue = camera.value = 0;
        data.maxValue = 0; data.value = 0;
        videoPanel.GetComponent<CameraStreaming>().rewindTime = 0;
        dataPanel.GetComponent<DisplayRecord>().Awake();
        videoPanelObject.SetActive(false);
        dataPanelObject.SetActive(false);
        logsPanelObject.SetActive(false);
        
        
    }

}
