using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Counter;
using Utility.DropDownData;

public class AircraftSelection : MonoBehaviour
{
    public GameObject toggle,toggleparent,loadout,loadoutparent,loadingScreen;
    public MasterSpawn masterspawn;
    public List<GameObject> loadlist = new List<GameObject>();
    public List<GameObject> togglelist = new List<GameObject>();
    public bool player;
    void Awake()
    {
        togglelist.Add(toggle);
        loadlist.Add(loadout);
        if (player)
        {
            for (int i = 1; i < masterspawn.ally_spawnPlanes.Count; i++)
            {
                togglelist.Add(Instantiate(toggle, toggleparent.transform));
                loadlist.Add(Instantiate(loadout, loadoutparent.transform));
                togglelist[togglelist.Count - 1].GetComponentInChildren<Text>().text = masterspawn.ally_spawnPlanes[0].name + " - " + (i).ToString();
                loadlist[loadlist.Count - 1].GetComponent<EnemyDropDownMenu>().countScriptableObject = loadlist.Count - 1;
                loadlist[loadlist.Count - 1].GetComponent<CounterMeasuers>().countScriptableObject = loadlist.Count - 1;
            }
            toggle.GetComponentInChildren<Text>().text = "Main Player";
        }
        else
        {
            for (int i = 1; i < masterspawn.adversary_spawnPlanes.Count; i++)
            {
                togglelist.Add(Instantiate(toggle, toggleparent.transform));
                loadlist.Add(Instantiate(loadout, loadoutparent.transform));
                togglelist[togglelist.Count - 1].GetComponentInChildren<Text>().text = masterspawn.adversary_spawnPlanes[i].name + " - " + (i + 1).ToString();
                loadlist[loadlist.Count - 1].GetComponent<EnemyDropDownMenu>().countScriptableObject = loadlist.Count - 1;
                loadlist[loadlist.Count - 1].GetComponent<CounterMeasuers>().countScriptableObject = loadlist.Count - 1;
            }
            toggle.GetComponentInChildren<Text>().text = masterspawn.adversary_spawnPlanes[0].name + " - 1";
        }
        toggle.GetComponent<Toggle>().isOn = true;
        StartCoroutine(updation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateUI(bool t)
    {
        if (!t) { return; }
        loadingScreen.SetActive(true);
        StartCoroutine(updation());
    }
    IEnumerator updation()
    {
        yield return new WaitForSeconds(1);
        int i = -1;
        foreach (GameObject go in togglelist)
        {
            i++;
            if (go.GetComponent<Toggle>().isOn)
                break;
        }
        foreach (GameObject go in loadlist)
        {
            go.SetActive(false);
        }
        loadlist[i].SetActive(true);
        loadingScreen.SetActive(false);
    }

}
