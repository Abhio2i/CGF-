using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class modernPanelSwitch : MonoBehaviour
{
    public GameObject Window;
    public GameObject Minimize;
    public List<GameObject> windowsList = new List<GameObject>();
    public List<GameObject> minimizeList = new List<GameObject>();
    public CanvasScaler CockpitScaler;
    void Start()
    {
        
    }

    public void setScale(float i)
    {
        CockpitScaler.matchWidthOrHeight = i;
    }



    private void OnValidate()
    {
        if (Window != null)
        {
            int i = Window.transform.childCount;
            windowsList.Clear();
            for (int j = 1; j < i; j++)
            {
                windowsList.Add(Window.transform.GetChild(j).gameObject);
            }
        }
        if (minimizeList != null)
        {
            int i = Minimize.transform.childCount;
            minimizeList.Clear();
            for (int j = 1; j < i; j++)
            {
                minimizeList.Add(Minimize.transform.GetChild(j).gameObject);
            }
        }
    }

}
