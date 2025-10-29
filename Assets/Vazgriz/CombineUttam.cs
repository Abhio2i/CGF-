using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Feed_Scene.newFeed;
using System;


// Combine Uttam - Weapons - Targets locking and Firing 

[Serializable]
public class AIRadarLog
{
    [SerializeField]
    public float Range;
    [SerializeField]
    public float Azimuth;
    [SerializeField]
    public float Bar;
    [SerializeField]
    public float F;
    [SerializeField]
    public float G;
    [SerializeField]
    public List<Vector4> info = new List<Vector4>();
    public List<Vector3> detect = new List<Vector3>();
    public List<bool> Friend = new List<bool>();
    public List<bool> locks = new List<bool>();
    public Maneuvers Maneuvers;
}

public class CombineUttam : MonoBehaviour
{
    public Transform Target;
    public bool Enemy = false;
    //private AIController controller;
    public AIRadar Radar;
    public string targetTag;
    public GameObject lateTarget;
    public GameObject LockTarget;
    public GameObject missilePrefab;
    public GameObject missilePrefab2;
    public List<GameObject> MissilePrefabsList = new List<GameObject> ();
    public bool AJ = false;
    public float maxMissile = 15f;
    public float count = 1;
    public float hit = 0;
    public GameObject destroyby;
    public float locktime = 1f;
    public string Log;
    public Transform MissileFirePos;
    public ManeuverControls maneuverControls;
    public List<string> missileName = new List<string> ();
    public List<MissileInfo> missileInfos = new List<MissileInfo> ();
    
    public GameObject _missile = null;
    public List<GameObject> _missiles = new List<GameObject>();
    public List<GameObject> friends = new List<GameObject>();
    public List<CombineUttam> friendsRadar = new List<CombineUttam> ();
    public Dictionary<GameObject,missileNavigation> missileNavigs = new Dictionary<GameObject,missileNavigation>();
    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    public AIRadarLog aiRadarLog;
    private bool shareTarget = false;
    private bool MissileFired = false;
    private string sub = "";
    void OnEnable()
    {
        loadSystemAndSpawnPlanes = FindObjectOfType<LoadSystemAndSpawnPlanes>();
        sub = Enemy ? "Enemy" : "";
        //maxMissile = PlayerPrefs.GetInt(sub+"MissileCount");
        //controller = gameObject.GetComponent<AIController>();
        Radar = GetComponentInChildren<AIRadar>();
        CustomUpdate();
        Invoke("CustomUpdate",0.5f);
        count = maxMissile;
         
        
    }
    private void Start()
    {
        
        count = maxMissile = 0;
        /*
        bool infrared = true;
        for (int i = 0; i < maxMissile; i++)
        {
            if (infrared)
            {
                GameObject missile = (GameObject)Instantiate(missilePrefab);
                infrared = false;
                _missiles.Add(missile);
                missile.SetActive(false);
                FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
            }
            else
            {
                GameObject missile = (GameObject)Instantiate(missilePrefab);
                infrared = true;
                _missiles.Add(missile);
                missile.SetActive(false);
                FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
            }
        }
*/

        foreach(GameObject obj in friends)
        {
            friendsRadar.Add(obj.GetComponent<CombineUttam>());
        }

        foreach (GameObject m_prefab in MissilePrefabsList)
        {
            for(int i = 0; i < missileName.Count; i++)
            {
                if (m_prefab.name.ToLower().Contains(missileName[i].ToLower()))
                {
                    GameObject missile = (GameObject)Instantiate(m_prefab);
                    if (Enemy)
                    {
                        missile.tag = "EnemyMissile";
                    }
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
                    missileNavigs.Add(missile,navig);
                    _missiles.Add(missile);
                    missile.SetActive(false);
                    FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
                }
            }
        }
        count = maxMissile;
    }
    private void FixedUpdate()
    {
        //count = maxMissile;
        if (LockTarget != null && Radar.DetectedObjects.Contains(LockTarget)&&lateTarget!=null&&lateTarget!=LockTarget)
        {
            LockTarget = null;
            //fir = true;
        }
    }
    private void Update()
    {
        if(_missile != null&& !_missile.activeSelf)
        {
            _missile = null;
        }

        
        AIRadarLog log = new AIRadarLog();
        if(maneuverControls)
        log.Maneuvers = maneuverControls.maneuver;

        log.Range = Radar.Range;
        log.Azimuth = Radar.azimuth;
        log.Bar = Radar.bar;
        log.F = Radar.f;
        log.G = Radar.G;
        foreach(var i in Radar.DetectedObjects)
        {
           
            if (i != null)
            {
                Vector4 info = Vector4.zero;
                float altitude = (i.transform.position.y * 3.281f) / 1000;//feet
                float bearing = transform.eulerAngles.y - i.transform.eulerAngles.y;//degree
                float Distance = (Vector3.Distance(i.transform.position, transform.position) / 1852);//Nauticle Mile
                info.x = Distance;
                info.y = altitude;
                info.z = 0;
                info.w = bearing;
                log.info.Add(info);
                log.detect.Add(i.transform.position);
                log.Friend.Add(i.tag.Contains("Player"));
                log.locks.Add(LockTarget!=null&& i == LockTarget);
            }

        }
        aiRadarLog = log;
        //Log = JsonUtility.ToJson(log);
        

    }

    public GameObject MissileSelect()
    {
        float distance = Vector3.Distance(LockTarget.transform.position, transform.position);
        foreach (var missile in _missiles)
        {
            missileNavigation navig = missileNavigs[missile];
            string type = navig.type.ToString();
            string Rangetype = navig.missileType.ToString();

            if ((LockTarget.tag.Contains("EnemyTank") || LockTarget.tag.Contains("EnemyShip")) && type == "ATA")
            {
                continue;
            } 

            if ((LockTarget.tag.Contains("EnemyPlane") || LockTarget.tag.Contains("Player")) && type == "ATS")
            {
                continue;
            }

            if ((Rangetype == "BVR" && distance < 30000) || (Rangetype == "CCM" && distance > 30000))
            {
                continue;
            }
            return missile;
        }
        return null;
    }

    public GameObject TargetAllocation()
    {

        List<GameObject> list = new List<GameObject>();
        List<GameObject> FriendsShareTargets = new List<GameObject>();
        if (Radar.DetectedObjects.Count > 0)
        {
            foreach (var i in Radar.DetectedObjects)
            {
                //Check Target not NUll and not missile
                if (i != null && Radar.TargetTag.Contains(i.tag)/* i.tag.Contains(targetTag)*/ && !i.tag.Contains("Missile"))
                {
                    int count = 0;
                    foreach (var j in friendsRadar)
                    {
                        if (j.gameObject.activeSelf && j.LockTarget == i)
                        {
                            FriendsShareTargets.Add(i);
                            count++;
                            break;
                        }
                    }
                    if (count == 0)
                    {
                        list.Add(i);
                    }

                }
            }
        }
        else
        {
            foreach (var j in friendsRadar)
            {
                if (j.gameObject.activeSelf && j.LockTarget != null)
                {
                    FriendsShareTargets.Add(j.LockTarget);
                }
            }
        }

        if(list.Count > 0)
        {
            var max = 0f;
            GameObject loc = null;
            foreach (var i in list)
            {

                float d = Vector3.Distance(transform.position, i.transform.position);
                //var lockAngle = Vector3.Angle(i.transform.position - Radar.transform.position, transform.forward);
                if (d < max || max == 0f) ///&& lockAngle < 65f)
                {
                    max = d;
                    loc = i;
                }
            }
            return loc;
        }
        else
        {
            var max = 0f;
            GameObject loc = null;
            foreach (var i in FriendsShareTargets)
            {

                float d = Vector3.Distance(transform.position, i.transform.position);
                //var lockAngle = Vector3.Angle(i.transform.position - Radar.transform.position, transform.forward);
                if (d < max || max == 0f) ///&& lockAngle < 65f)
                {
                    max = d;
                    loc = i;
                }
            }
            shareTarget = true;
            return loc;
        }


    }
    void CustomUpdate()
    {
        shareTarget = false;
        if (Target!=null && Radar.DetectedObjects.Contains(Target.gameObject))
        {

            if (lateTarget == null&& _missile == null)
            {
                //var distance = Vector3.Distance(Target.position, Radar.transform.position);
                var lockAngle = Vector3.Angle(Target.position - Radar.transform.position, transform.forward);
                if (lockAngle < 15f /*&& distance<10000*/)
                {
                    lateTarget = Target.gameObject;
                    LockTarget = Target.gameObject;
                    if (!MissileFired)
                    {
                        MissileFired = true;
                        Invoke("fire", locktime);
                    }
                }
            }

 
        }
        else
        {
            GameObject loc = null;
            /*
            foreach (var i in Radar.DetectedObjects)
            {
                //Check Detected should not be null, not missile.
                // but should contain targetTag.
                if (i!=null&&i.tag.Contains(targetTag) && !i.tag.Contains("Missile"))
                {
                    float d = Vector3.Distance(transform.position, i.transform.position);
                    var lockAngle = Vector3.Angle(i.transform.position - Radar.transform.position, transform.forward);
                    if ((d < max || max == 0f) && lockAngle < 65f)
                    {
                        max = d;
                        loc = i;
                    }
                }
            }
            */
            loc = TargetAllocation();
            if (loc != null)
            {
                Target = loc.transform;
                //controller.missile = loc.transform;

                LockTarget = loc;
                var distance = Vector3.Distance(Target.position, Radar.transform.position);
                var lockAngle = Vector3.Angle(Target.position - Radar.transform.position, transform.forward);
                if (lockAngle < 15f /*&& distance < 10000*/ && distance > 3000 && _missile == null)
                //if (!_missile)
                {
                    if (lateTarget == null)
                    {
                        lateTarget = loc;
                    }
                    string targt = loadSystemAndSpawnPlanes.findCraftType(LockTarget);
                    string By = loadSystemAndSpawnPlanes.findCraftType(gameObject);
                    if (shareTarget)
                    {
                        FeedBackRecorderAndPlayer.AddEvent(By + " Get Shared Target " + targt);
                    }
                    FeedBackRecorderAndPlayer.AddEvent(By + " Locked " + targt);
                    if (!MissileFired)
                    {
                        MissileFired = true;
                        Invoke("fire", locktime);
                    }
                     
                }


            }
            else
            {
                Target = null;
                lateTarget = LockTarget =  null;
            }
        }
       // if(controller.enabled == true)
        Invoke("CustomUpdate", 0.5f); 

    }

    public void fire()
    {
        if (LockTarget != null)
        {
            if (Vector3.Angle(Target.position - Radar.transform.position, transform.forward) > 15) { lateTarget = null; return; }
            GameObject missile = MissileSelect();
            if (missile !=null)
            {
                //count--; //GetComponent<Assets.Scripts.Feed.PlaneData>().missileCount = (int)count;

                /*
                if (Random.Range(0, 10) > 5)
                {
                    _missile = (GameObject)Instantiate(missilePrefab); planeData.SetMessage("Missile Fired");
                }
                else
                {
                    _missile = (GameObject)Instantiate(missilePrefab2); planeData.SetMessage("Missile Fired");
                }
                */
                //_missile = _missiles[_missiles.Count - 1];
                _missile = missile;
                _missile.SetActive(true);
                _missiles.Remove(_missile);
                //planeData.SetMessage("Missile Fired");
                string targt = loadSystemAndSpawnPlanes.findCraftType(LockTarget);
                string By = loadSystemAndSpawnPlanes.findCraftType(gameObject);
                FeedBackRecorderAndPlayer.AddEvent(By+" Fired " + _missile.name + " Missile towards " + targt);


                //print("Called");
                MissileType(_missile);
                //EditorApplication.isPaused = true; 
                if (MissileFirePos != null)
                {
                    _missile.transform.SetPositionAndRotation(MissileFirePos.position, MissileFirePos.rotation);
                }
                else
                {
                    _missile.transform.SetPositionAndRotation(transform.position + (transform.forward * 7f) + (transform.up * -4f), transform.rotation);
                }
            _missile.GetComponent<missileEngine>().fire = true;
            _missile.GetComponent<missileNavigation>().GivenTarget = LockTarget.transform;
                //_missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.ActiveRadar;
            }
            lateTarget = null;
           
    
        }
        MissileFired = false;
    }

    void MissileType(GameObject missile) 
    {
        SaveEntityDatas missileData = missile.GetComponent<SaveEntityDatas>();
        var aircraft= GetComponent<SaveEntityDatas>();
        if(aircraft!=null)
        {
            if (aircraft.type == EntityType.Plane_Player) missileData.type = EntityType.Missile_Player;
            else if (aircraft.type == EntityType.Plane_Ally) missileData.type = EntityType.Missile_Ally;
            else if (aircraft.type == EntityType.Plane_Adversary) missileData.type = EntityType.Missile_Adversary;
            //else if (aircraft.type == EntityType.EnemyWarship) missileData.type = EntityType.Missile_Adversary;
        }
    }

}
