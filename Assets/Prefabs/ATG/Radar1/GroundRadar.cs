using Assets.Scripts.Feed_Scene.newFeed;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using Vazgriz.Plane;
using Weapon.MissileAction;

public class GroundRadar : MonoBehaviour
{
    public int f = 0, G = 0;
    public float missileActivationRange=0.7f, tcfDistance;
    public LayerMask groundLayer;
    private bool jam;
    public GameObject twin,pac,missilePrefabTwin,missilePrefabPac;
    public float FireRatePerSecond = 2f;
    public bool Fire = true;
    public float maxMissile = 0f;
    public float hit = 0;
    public GameObject destroyby;
    public GameObject _missile = null;
    public List<GameObject> MissilePrefabsList = new List<GameObject>();
    public List<string> missileName = new List<string>();
    public List<MissileInfo> missileInfos = new List<MissileInfo>();
    public List<GameObject> _missiles = new List<GameObject>();

    public int twinMissiles=5, pacMissiles=5;
    public List<string> TargetTag = new List<string>();
    public List<GameObject> DetectedObjects = new List<GameObject>();
    public List<GameObject> SightObjects = new List<GameObject>();
    public float radarRange = 30000;
    private bool twinFire, pacFire;
    public List<GameObject> aircrafts;
    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    public SphereCollider collder;
    private int clock=1;
    void Start()
    {
        loadSystemAndSpawnPlanes = FindObjectOfType<LoadSystemAndSpawnPlanes>();
        /*
        bool infrared = true;
        
        for (int i = 0; i < maxMissile; i++)
        {
            if (infrared)
            {
                GameObject missile = (GameObject)Instantiate(missilePrefabTwin);
                infrared = false;
                _missiles.Add(missile);
                missile.transform.position = transform.position;
                missile.SetActive(false);
                FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
            }
            else
            {
                GameObject missile = (GameObject)Instantiate(missilePrefabPac);
                infrared = true;
                _missiles.Add(missile);
                missile.transform.position = transform.position;
                missile.SetActive(false);
                FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
            }
        }
        */
        maxMissile =0;
        foreach (GameObject m_prefab in MissilePrefabsList)
        {
            for (int i = 0; i < missileName.Count; i++)
            {
                if (m_prefab.name.ToLower().Contains(missileName[i].ToLower()))
                {
                    GameObject missile = (GameObject)Instantiate(m_prefab);

                    missileEngine missileEngine = missile.GetComponent<missileEngine>();
                    missileNavigation navig = missile.GetComponent<missileNavigation>();
                    navig.FiredBy = gameObject;
                    foreach (MissileInfo info in missileInfos)
                    {
                        if (info.Name.ToLower() == m_prefab.name.ToLower())
                        {
                            missileEngine.MaxSpeed = info.Speed;
                            missileEngine.Range = info.Range;
                            missileEngine.turnSpeed = info.TurnRadius;
                            break;
                        }
                    }
                    maxMissile++;
                    _missiles.Add(missile);
                    missile.SetActive(false);
                    FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
                }
            }
        }

        foreach (GameObject aircraft in aircrafts) 
        {
            toggleAircraft(aircraft, false);
        }
        collder.radius = radarRange;
    }
    void toggleAircraft(GameObject aircraft,bool t)
    {
        aircraft.GetComponent<Rigidbody>().isKinematic = !t;
        aircraft.GetComponent<AIController>().enabled = t;
        aircraft.GetComponent<CombineEW>().enabled = t;
        aircraft.GetComponent<CombineUttam>().enabled = t;
        aircraft.GetComponent<Vazgriz.Plane.Plane>().enabled = t;
        aircraft.GetComponentInChildren<AIRadar>().enabled = t;
        aircraft.GetComponentInChildren<EWRadar>().enabled = t;
    }
    private void Update()
    {
        int v = Random.Range(0, DetectedObjects.Count);
        int i = 0;
        foreach (GameObject obj in DetectedObjects)
        {
            //if (_missile == null || !_missile.activeSelf)
            if(Fire)
            {
                if (_missiles.Count > 0 && i == v)
                {
                    Fire = false;
                    Transform target = obj.transform;
                    _missile = _missiles[_missiles.Count - 1];
                    _missile.transform.position = new Vector3(twin.transform.position.x, twin.transform.position.y + 100, twin.transform.position.z);
                    _missile.transform.LookAt(target.position);
                    _missile.SetActive(true);
                    _missile.GetComponent<missileEngine>().fire = true;
                    //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.None;
                    _missile.GetComponent<SaveEntityDatas>().type = EntityType.Missile_Adversary;
                    _missile.GetComponent<missileNavigation>().GivenTarget = target;
                    _missile.GetComponent<missileNavigation>().target = target.gameObject;
                    _missiles.Remove(_missile);
                    string targt = loadSystemAndSpawnPlanes.findCraftType(target.gameObject);
                    string By = loadSystemAndSpawnPlanes.findCraftType(transform.root.gameObject);
                    FeedBackRecorderAndPlayer.AddEvent(By + " Fired " + _missile.name + " Missile towards " + targt);
                    StartCoroutine(FireAllow());

                }
            }
            i++;
            /*
            if (twin.activeSelf && !twinFire && twinMissiles > 0) { TwinFire(obj.transform); twinFire = true; }
            if (pac.activeSelf && !pacFire && pacMissiles > 0) { PacFire(obj.transform); pacFire = true; }*/
            if (Vector3.Distance(transform.position, obj.transform.position) < radarRange * missileActivationRange)
            {
                foreach (GameObject aircraft in aircrafts) { toggleAircraft(aircraft, true); }
            }
        }
    } 

    IEnumerator FireAllow()
    {
        yield return new WaitForSeconds(FireRatePerSecond);
        Fire = true;
    }

    public void TwinFire(Transform target)
    {
        twinMissiles--;
        var missile = Instantiate(missilePrefabTwin);
        missile.transform.position=new Vector3(twin.transform.position.x, twin.transform.position.y+100, twin.transform.position.z);
        missile.transform.LookAt(target.position);
        missile.GetComponent<missileEngine>().fire = true;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.None;
        missile.GetComponent<SaveEntityDatas>().type = EntityType.Missile_Adversary;
        missile.GetComponent<missileNavigation>().GivenTarget = target;
        missile.GetComponent<missileNavigation>().target = target.gameObject;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.ActiveRadar;
        Invoke(nameof(DisableBool), 7);
    }
    public void PacFire(Transform target)
    {
        pacMissiles--;
        var missile = Instantiate(missilePrefabPac);
        missile.transform.position = new Vector3(pac.transform.position.x,pac.transform.position.y+100, pac.transform.position.z);
        missile.transform.LookAt(target.position);
        missile.GetComponent<missileEngine>().fire = true;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.None;
        missile.GetComponent<SaveEntityDatas>().type = EntityType.Missile_Adversary;
        missile.GetComponent<missileNavigation>().GivenTarget = target;
        missile.GetComponent<missileNavigation>().target = target.gameObject;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.ActiveRadar;
        Invoke(nameof(DisableBool2), 7);
    }
    public void DisableBool()
    {
        twinFire = false;
        //pacFire = false;
        var wait = new WaitForSeconds(1);
    }

    public void DisableBool2()
    {
        //twinFire = false;
        pacFire = false;
        var wait = new WaitForSeconds(1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!DetectedObjects.Contains(other.gameObject) && TargetTag.Contains(other.tag))
        {
            if (!(SightObjects.Contains(other.gameObject) && jam))
            {
                //Raycasting to check if is close to ground
                RaycastHit hit;
                if (Physics.Raycast(other.transform.position, Vector3.down, out hit, tcfDistance, groundLayer))
                {
                    if (hit.distance < tcfDistance / 2) return;
                }
                DetectedObjects.Add(other.gameObject);
                string targt = loadSystemAndSpawnPlanes.findCraftType(other.gameObject);
                string By = loadSystemAndSpawnPlanes.findCraftType(transform.root.gameObject);
                FeedBackRecorderAndPlayer.AddEvent(By + " Detect " + targt);

            }
            else if (SightObjects.Contains(other.gameObject))
            {
                RaycastHit hit;
                if (Physics.Raycast(other.transform.position, Vector3.down, out hit, tcfDistance, groundLayer))
                    if (hit.distance < tcfDistance / 2)
                        DetectedObjects.Remove(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DetectedObjects.Contains(other.gameObject))
        {
            DetectedObjects.Remove(other.gameObject);
        }
    }
    public void JamSignal(Jammer.JamMode mode, Jammer.JamFrequency frequency, Jammer.JamPower power, GameObject jammer)
    {
        if ((int)frequency == f && (int)power > G)//JamValid or Not
        {
            var jamAngle = Vector3.SignedAngle(jammer.transform.position - transform.position, transform.forward, transform.up);
            foreach (GameObject aircraft in DetectedObjects)
            {
                var anglediff = Vector3.SignedAngle(aircraft.transform.position - transform.position, transform.forward, transform.up) - jamAngle;
                if ((anglediff > 0 ? anglediff : -anglediff) < 3)
                {
                    if (!SightObjects.Contains(aircraft))
                        StartCoroutine(JamIT(aircraft));
                }
            }
            foreach (GameObject aircraft in SightObjects)
            {
                jam = true;
                DetectedObjects.Remove(aircraft);
            }
        }
        else { SightObjects.Clear(); }
    }
    IEnumerator JamIT(GameObject aircraft)
    {
        yield return new WaitForSeconds(2);
        SightObjects.Add(aircraft);
    }
}