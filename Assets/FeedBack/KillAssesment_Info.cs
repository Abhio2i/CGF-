using Assets.Code.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KillAssesment_Info : MonoBehaviour
{
    public LoadSystemAndSpawnPlanes spawnPlanes;
    public WeaponTracker playerTracker;
    public List<TextMeshProUGUI> eventText;
    public TextMeshProUGUI enemy;
    public TextMeshProUGUI ally;
    public TextMeshProUGUI sams;
    public TextMeshProUGUI warship;
    public TextMeshProUGUI pilot;
    public List<GameObject> AllyPlanes;
    public List<GameObject> EnemyPlanes;
    public static string message;
    public List<string> evstrings = new List<string>();
    public List<string> Allevents = new List<string>();
    public int allycount, enemycount, samscount, warshipcount, pilotcount;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in eventText)
        {
            evstrings.Add("");
        }
        foreach(GameObject ally in AllyPlanes)
        {
            ally.SetActive(false);
        }
        foreach (GameObject enemy in EnemyPlanes)
        {
            enemy.SetActive(false);
        }
    }

    public void KillAssesment()
    {
        float maxMissile = playerTracker.MaxMissile;
        float leftMissile = 0;
        string destroyBy = spawnPlanes.findCraftType(playerTracker.destroyby);
        foreach (GameObject m in playerTracker.Missiles)
        {
            if (m != null)
            {
                leftMissile++;
            }
        }
        float hitMissile = playerTracker.hit;
        string color = "<color=#ffffff>";
        destroyBy = destroyBy.Replace("Ally", "<color=#0000ff>Blue");
        destroyBy = destroyBy.Replace("Enemy", "<color=#ff0000>Red");
        AllyPlanes[0].SetActive(true);
        AllyPlanes[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = color + "You_" + 0;
        AllyPlanes[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = color + maxMissile.ToString();
        AllyPlanes[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = color + (maxMissile - leftMissile).ToString();
        AllyPlanes[0].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = color + hitMissile.ToString();
        AllyPlanes[0].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = color + destroyBy.ToString();

        int i = 1;
        foreach(GameObject ally in spawnPlanes.AllyPlanes)
        {
            
            maxMissile = ally.GetComponent<CombineUttam>().maxMissile;
            leftMissile = ally.GetComponent<CombineUttam>()._missiles.Count;
            hitMissile = ally.GetComponent<CombineUttam>().hit;
            destroyBy = spawnPlanes.findCraftType(ally.GetComponent<CombineUttam>().destroyby);
            destroyBy = destroyBy.Replace("Ally", "<color=#0000ff>Blue");
            destroyBy = destroyBy.Replace("Enemy", "<color=#ff0000>Red");
            color = "<color=#ffffff>";
            AllyPlanes[i].SetActive(true);
            AllyPlanes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = color + "Blue_" + i;
            AllyPlanes[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = color + maxMissile.ToString();
            AllyPlanes[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = color + (maxMissile - leftMissile).ToString();
            AllyPlanes[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = color + hitMissile.ToString();
            AllyPlanes[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = color + destroyBy.ToString();



            i++; 
        }
        i = 0;
        foreach (GameObject enemy in spawnPlanes.AdversaryPlanes)
        {
            maxMissile = enemy.GetComponent<CombineUttam>().maxMissile;
            leftMissile = enemy.GetComponent<CombineUttam>()._missiles.Count;
            hitMissile = enemy.GetComponent<CombineUttam>().hit;
            destroyBy = spawnPlanes.findCraftType(enemy.GetComponent<CombineUttam>().destroyby);
            destroyBy = destroyBy.Replace("Ally", "<color=#0000ff>Blue");
            destroyBy = destroyBy.Replace("Enemy", "<color=#ff0000>Red");
            EnemyPlanes[i].SetActive(true);
            EnemyPlanes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Red_" + i;
            EnemyPlanes[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = maxMissile.ToString();
            EnemyPlanes[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (maxMissile - leftMissile).ToString();
            EnemyPlanes[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = hitMissile.ToString();
            EnemyPlanes[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = color + destroyBy.ToString();
            i++;
        }

        foreach (GameObject enemy in spawnPlanes.Sams)
        {
            maxMissile = enemy.GetComponentInChildren<GroundRadar>().maxMissile;
            leftMissile = enemy.GetComponentInChildren<GroundRadar>()._missiles.Count;
            hitMissile = enemy.GetComponentInChildren<GroundRadar>().hit;
            destroyBy = spawnPlanes.findCraftType(enemy.GetComponentInChildren<GroundRadar>().destroyby);
            destroyBy = destroyBy.Replace("Ally", "<color=#0000ff>Blue");
            destroyBy = destroyBy.Replace("Enemy", "<color=#ff0000>Red");
            EnemyPlanes[i].SetActive(true);
            EnemyPlanes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Sam_" + i;
            EnemyPlanes[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = maxMissile.ToString();
            EnemyPlanes[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (maxMissile - leftMissile).ToString();
            EnemyPlanes[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = hitMissile.ToString();
            EnemyPlanes[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = color + destroyBy.ToString();

            i++;
        }

        foreach (GameObject enemy in spawnPlanes.Warships)
        {
            maxMissile = enemy.GetComponentInChildren<CarrierShipController>().maxMissile;
            leftMissile = enemy.GetComponentInChildren<CarrierShipController>()._missiles.Count;
            hitMissile = enemy.GetComponentInChildren<CarrierShipController>().hit;
            destroyBy = spawnPlanes.findCraftType(enemy.GetComponentInChildren<GroundRadar>().destroyby);
            destroyBy = destroyBy.Replace("Ally", "<color=#0000ff>Blue");
            destroyBy = destroyBy.Replace("Enemy", "<color=#ff0000>Red");
            EnemyPlanes[i].SetActive(true);
            EnemyPlanes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ship_" + i;
            EnemyPlanes[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = maxMissile.ToString();
            EnemyPlanes[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (maxMissile - leftMissile).ToString();
            EnemyPlanes[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = hitMissile.ToString();
            EnemyPlanes[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = color + destroyBy.ToString();

            i++;
        }
    }


    private void FixedUpdate()
    {
        if (!FeedBackRecorderAndPlayer.isPlaying)
        {
            int i = 0; 
            foreach (var item in eventText)
            {
                item.text = evstrings[i];
                i++;
            }
            foreach (string str in FeedBackRecorderAndPlayer.CurrentEventData.events)
            {
                string s = str.Replace("Enemy", "<color=#FF0000>Red</color>");
                s = s.Replace("Enemy", "<color=#FF0000>Red</color>");
                s = s.Replace("Ally", "<color=#0000FF>Blue</color>");
                s = s.Replace("Ally", "<color=#0000FF>Blue</color>");
                s = s.Replace("Player", "<color=#0000FF>Player</color>");
                s = s.Replace("Player", "<color=#0000FF>Player</color>");
                s = s.Replace("Sam", "<color=#FFFF00>Sam</color>");
                s = s.Replace("Warship", "<color=#0000FF>Ship</color>");
                s = s.Replace("towards", "<color=#0000FF> -> </color>");

                evstrings.RemoveAt(0);
                evstrings.Add(""+FeedBackRecorderAndPlayer.currentFrame + ": " + s);
                Allevents.Add(""+FeedBackRecorderAndPlayer.currentFrame + ": " + str); 
            }
            FeedBackRecorderAndPlayer.CurrentEventData = new EventData();
        }
    }
    // Update is called once per frame
    void Update()
    {
        

        if(message != null)
        {
            if (message.Contains("ally"))
            {
                allycount++;
                ally.gameObject.SetActive(true);
                StartCoroutine(ResetPopup(message,allycount+0));
            }else
            if (message.Contains("enemy"))
            {
                enemycount++;
                enemy.gameObject.SetActive(true);
                StartCoroutine(ResetPopup(message, enemycount + 0));
            }
            else
            if (message.Contains("sams"))
            {
                samscount++;
                sams.gameObject.SetActive(true);
                StartCoroutine(ResetPopup(message, samscount + 0));
            }
            else
            if (message.Contains("warship"))
            {
                warshipcount++;
                warship.gameObject.SetActive(true);
                StartCoroutine(ResetPopup(message, warshipcount + 0));
            }

            message = null;
        }
    }

    IEnumerator ResetPopup(string message, int count)
    {
        yield return new WaitForSeconds(3f);
        if (message.Contains("ally"))
        {
            ally.gameObject.SetActive(!(count == allycount));
        }
        else
            if (message.Contains("enemy"))
        {
            enemy.gameObject.SetActive(!(count == enemycount));
        }
        else
            if (message.Contains("sams"))
        {
            sams.gameObject.SetActive(!(count == samscount));
        }
        else
            if (message.Contains("warship"))
        {
            warship.gameObject.SetActive(!(count == warshipcount));
        }
    }
    
}
