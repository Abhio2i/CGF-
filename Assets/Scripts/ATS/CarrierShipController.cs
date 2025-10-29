using Assets.Scripts.Feed_Scene.newFeed;
using Evereal.VideoCapture;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarrierShipController : MonoBehaviour
{
    [SerializeField] List<DestroyerController> destroyers;
    [SerializeField] float speed;
    public LayerMask groundLayer;
    public int f = 0, G = 0;
    public float missileActivationRange = 0.7f, tcfDistance = 2500;
    private bool jam;
    public float FireRatePerSecond = 2f;
    public bool Fire = true;
    public float maxMissile = 10f;
    public float hit = 0;
    public GameObject destroyby;
    public GameObject _missile = null;
    public List<GameObject> MissilePrefabsList = new List<GameObject>();
    public List<string> missileName = new List<string>();
    public List<MissileInfo> missileInfos = new List<MissileInfo>();
    public List<GameObject> _missiles = new List<GameObject>();
    public GameObject leftShip, rightShip, missilePrefab1, missilePrefab2;
    public int leftMissiles = 5, rightMissiles = 5;
    public List<string> TargetTag = new List<string>();
    public List<GameObject> DetectedObjects = new List<GameObject>();
    public List<GameObject> SightObjects = new List<GameObject>();
    public float radarRange = 30000;
    public SphereCollider sphereCollider;
    public bool leftFire, rightFire;
    public Rigidbody body;
    public LoadSystemAndSpawnPlanes LoadSystemAndSpawnPlanes;
    public float TargetAngle = 0f;
    public float angle = 0f;
    public float distance = 5f; // Distance from the object
    public float constantY = 1f; // Constant Y value
    public List<float> data = new List<float>();
    public int count = 0;

    public List<GameObject> Colliders = new List<GameObject>();
    //public List<GameObject> aircrafts;
    private int clock = 1;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        LoadSystemAndSpawnPlanes = FindObjectOfType<LoadSystemAndSpawnPlanes>();
        ColliderSet();
        /*
        bool infrared = true;
        for (int i = 0; i < maxMissile; i++)
        {
            if (infrared)
            {
                GameObject missile = (GameObject)Instantiate(missilePrefab1);
                infrared = false;
                _missiles.Add(missile);
                missile.transform.position = transform.position;
                missile.SetActive(false);
                FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
            }
            else
            {
                GameObject missile = (GameObject)Instantiate(missilePrefab2);
                infrared = true;
                _missiles.Add(missile);
                missile.transform.position = transform.position;
                missile.SetActive(false);
                FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);
            }
        }
        */
        maxMissile = 0;
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
        sphereCollider.radius = radarRange;
        Invoke(nameof(DisableBool), 7);

        float angleInRadians = angle * Mathf.Deg2Rad;
        for (float i = 0f; i < distance; i += (distance / 10f))
        {
            // Step 2: Calculate new point
            Vector3 newPoint = new Vector3(
                transform.position.x + i * Mathf.Cos(angleInRadians), // X coordinate
            transform.position.y + constantY, // Y coordinate
                transform.position.z + i * Mathf.Sin(angleInRadians)  // Z coordinate
            );
            Debug.DrawRay(newPoint, transform.up * -100f, Color.yellow);
            newPoint = new Vector3(
                transform.position.x - i * Mathf.Cos(angleInRadians), // X coordinate
            transform.position.y + constantY, // Y coordinate
                transform.position.z - i * Mathf.Sin(angleInRadians)  // Z coordinate
            );
            Debug.DrawRay(newPoint, transform.up * -100f, Color.yellow);
            data.Add(0);
            data.Add(0);
        }
    }


    //Create DiffrentShape based collider
    public void ColliderSet()
    {
        int r = Random.Range(0,2);
        foreach(GameObject g in Colliders)
        {
            g.SetActive(false);
        }
        if(r == 0)
        {
            Colliders[0].SetActive(true);
            Colliders[1].SetActive(true);
        }else
        if (r == 1)
        {
            Colliders[0].SetActive(true);
            Colliders[2].SetActive(true);
            Colliders[3].SetActive(true);
        }
        else
        if (r == 2)
        {
            Colliders[1].SetActive(true);
            Colliders[4].SetActive(true);
        }
    }

    private void Update()
    {
        int v = Random.Range(0, DetectedObjects.Count);
        int i = 0;
        foreach (GameObject obj in DetectedObjects)
        {
            if (Fire &&false)
            {
                if(_missiles.Count > 0 && i == v)
                {
                    Fire = false;
                    Transform target = obj.transform;
                    _missile = _missiles[_missiles.Count-1];
                    _missile.transform.position = new Vector3(leftShip.transform.position.x, leftShip.transform.position.y + 100, leftShip.transform.position.z);
                    _missile.transform.LookAt(target.position);
                    _missile.SetActive(true);
                    _missile.GetComponent<missileEngine>().fire = true;
                    //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.None;
                    _missile.GetComponent<SaveEntityDatas>().type = EntityType.Missile_Adversary;
                    _missile.GetComponent<missileNavigation>().GivenTarget = target;
                    _missiles.Remove(_missile);
                    string targt = LoadSystemAndSpawnPlanes.findCraftType(target.gameObject);
                    string By = LoadSystemAndSpawnPlanes.findCraftType(transform.root.gameObject);
                    FeedBackRecorderAndPlayer.AddEvent(By + " Fired " + _missile.name + " Missile towards " + targt);
                    StartCoroutine(FireAllow());
                }
            }
            i++;
            /*
            if (leftShip.activeSelf && !leftFire && leftMissiles > 0) { TwinFire(obj.transform); leftFire = true; }
            if (rightShip.activeSelf && !rightFire && rightMissiles > 0) { PacFire(obj.transform); rightFire = true; }
            */
        }

        dataget();
        if(count > 30f)
        {
            angle = TargetAngle;
            count = 0;
        }
        count ++;
    }

    IEnumerator FireAllow()
    {
        yield return new WaitForSeconds(FireRatePerSecond);
        Fire = true;
    }

    private void dataget()
    {
        angle = Mathf.LerpAngle(angle, 0, Time.deltaTime * 50f);
        float angleInRadians = angle * Mathf.Deg2Rad;
        int count = 0;
        for (float i = 0f; i < distance; i += (distance / 10f))
        {
            // Step 2: Calculate new point
            Vector3 newPoint = new Vector3(
                transform.position.x + i * Mathf.Cos(angleInRadians), // X coordinate
            transform.position.y + constantY, // Y coordinate
                transform.position.z + i * Mathf.Sin(angleInRadians)  // Z coordinate
            );
            Debug.DrawRay(newPoint, transform.up * -130f, Color.yellow);
            RaycastHit hit;
            if (Physics.Raycast(newPoint, transform.up * -130f, out hit))
            {
                data[count] = hit.distance / 100f;
            }
            else
            {
                data[count] = 0f;
            }
            count++;
            /*newPoint = new Vector3(
                transform.position.x - i * Mathf.Cos(angleInRadians), // X coordinate
            transform.position.y + constantY, // Y coordinate
                transform.position.z - i * Mathf.Sin(angleInRadians)  // Z coordinate
            );
            Debug.DrawRay(newPoint, transform.up * -100f, Color.yellow);
            
            if (Physics.Raycast(newPoint, transform.up * -100f, out hit))
            {
                data[count] = hit.distance / 100f;
            }
            else
            {
                data[count] = 0f;
            }
            count++;*/
        }

        for (float i = distance; i >0; i -= (distance / 10f))
        {
            // Step 2: Calculate new point
            Vector3 newPoint = new Vector3(
                transform.position.x - i * Mathf.Cos(angleInRadians), // X coordinate
            transform.position.y + constantY, // Y coordinate
                transform.position.z - i * Mathf.Sin(angleInRadians)  // Z coordinate
            );
            Debug.DrawRay(newPoint, transform.up * -130f, Color.yellow);
            RaycastHit hit;
            if (Physics.Raycast(newPoint, transform.up * -130f, out hit))
            {
                data[count] = hit.distance / 100f;
            }
            else
            {
                data[count] = 0f;
            }
            count++;
            /*newPoint = new Vector3(
                transform.position.x - i * Mathf.Cos(angleInRadians), // X coordinate
            transform.position.y + constantY, // Y coordinate
                transform.position.z - i * Mathf.Sin(angleInRadians)  // Z coordinate
            );
            Debug.DrawRay(newPoint, transform.up * -100f, Color.yellow);
            
            if (Physics.Raycast(newPoint, transform.up * -100f, out hit))
            {
                data[count] = hit.distance / 100f;
            }
            else
            {
                data[count] = 0f;
            }
            count++;*/
        }
    }

    public void TwinFire(Transform target)
    {
        leftMissiles--;
        var missile = Instantiate(missilePrefab1);
        missile.transform.position = new Vector3(leftShip.transform.position.x, leftShip.transform.position.y + 100, leftShip.transform.position.z);
        missile.transform.LookAt(target.position);
        missile.GetComponent<missileEngine>().fire = true;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.None;
        missile.GetComponent<SaveEntityDatas>().type = EntityType.Missile_Adversary;
        missile.GetComponent<missileNavigation>().GivenTarget = target;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.ActiveRadar;
        Invoke(nameof(DisableBool), 7);
    }
    public void PacFire(Transform target)
    {
        rightMissiles--;
        var missile = Instantiate(missilePrefab2);
        missile.transform.position = new Vector3(rightShip.transform.position.x, rightShip.transform.position.y + 100, rightShip.transform.position.z);
        missile.transform.LookAt(target.position);
        missile.GetComponent<missileEngine>().fire = true;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.None;
        missile.GetComponent<SaveEntityDatas>().type = EntityType.Missile_Adversary;
        missile.GetComponent<missileNavigation>().GivenTarget = target;
        //missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.ActiveRadar;
        Invoke(nameof(DisableBool), 7);
    }
    public void DisableBool()
    {
        leftFire = false;
        rightFire = false;
        var wait = new WaitForSeconds(1);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!DetectedObjects.Contains(other.gameObject) && TargetTag.Contains(other.tag))
        {
            if (!(SightObjects.Contains(other.gameObject) && jam))
            {
                RaycastHit hit;
                if (Physics.Raycast(other.transform.position, Vector3.down, out hit, tcfDistance, groundLayer))
                {
                    if (hit.distance < tcfDistance / 2) return;
                }
                DetectedObjects.Add(other.gameObject);
                string targt = LoadSystemAndSpawnPlanes.findCraftType(other.gameObject);
                string By = LoadSystemAndSpawnPlanes.findCraftType(transform.root.gameObject);
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
    private void FixedUpdate()
    {
        if (destroyers.Count>0)
        {
            foreach(var destroyer in destroyers)
            {
                destroyer.UpdateDestroyerFunctions();
                Move(destroyer.transform);
            }
        }
        body.velocity  = -Vector3.right *speed; 
        //transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }
    void Move(Transform ship)
    {
        ship.position += ship.forward * speed * Time.fixedDeltaTime;
    }
}
