using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.Feed;
using Assets.Scripts.Utility;
using Assets.Scripts.Feed_Scene.newFeed;
using Assets.Code.UI;

public class missileEngine : MonoBehaviour 
{

    public bool fire = false;
    public Vector3 Target;
    public float MaxSpeed = 100f;
    public float turnSpeed = 5f;
    public float Range = 20f;
    public float warhead = 40f;
    public float AutoDestroyRange = 5f;
    public float distance = 0f;
    public float force = 4000f;
    public float RateofSpeed = 1f;
    public float radius = 5.0F;
    public float power = 10.0F;
    public float SmokeTime = 160f;
    public float Rmax = 40;
    public float Rmin = 10;
    public float Speed = 0;
    public float SpeedPickup = 3f;
    private Rigidbody rigid;
    private missileNavigation navig;
    public GameObject exploPrefab;
    private Vector3 pos = Vector3.zero;
    public float missileVelocity = 0f;
    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    private bool blast = false;
    private PlaneData data;

    //private Vector3 previousPos;
    //public float maxRotationAngle = 30f;
    //public float acceleration = 10f;
    //private Vector3 currentVelocity;
    //private Quaternion previousRotation;

    void Start()
    {
        loadSystemAndSpawnPlanes = FindObjectOfType<LoadSystemAndSpawnPlanes>();
        //FeedBackRecorderAndPlayer.Missiles.Add(transform);
        pos = transform.position;
        rigid = GetComponent<Rigidbody>();
        navig = GetComponent<missileNavigation>();
        data = GetComponent<PlaneData>();
        //currentVelocity = Vector3.zero; 
        //previousRotation = transform.rotation;
        //previousPos = Target;
    }

    void AssignMissileType()
    {
        SaveEntityDatas saveEntityDatas = GetComponent<SaveEntityDatas>();
    }
    void FixedUpdate()
    {
        var d = 0f;
        if (rigid != null && distance < Range * 1000f && fire)
        {
            #region PankajFollow
            rigid.rotation = Quaternion.RotateTowards(rigid.rotation, Quaternion.LookRotation(Target - transform.position), turnSpeed * Time.deltaTime);
            // (transform.position + transform.forward * Speed * Time.deltaTime);
            Speed = Mathf.Lerp(Speed, MaxSpeed, Time.fixedDeltaTime*SpeedPickup);
            rigid.MovePosition(transform.position + transform.forward * (Speed / 3.6f) * Time.deltaTime);
            //rigid.velocity = Vector3.Lerp(rigid.velocity,transform.forward *( MaxSpeed  / 3.6f),(1/RateofSpeed)*Time.fixedDeltaTime);
            #endregion
            #region NewFollow
            //Vector3 targetVelocity = (Target - previousPos) / Time.fixedDeltaTime;
            //previousPos = Target;
            //Vector3 direction = (Target + targetVelocity * Time.fixedDeltaTime) - transform.position;
            //// Clamp the rotation angle to the maxRotationAngle
            //direction = Quaternion.AngleAxis(Mathf.Clamp(Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)), -maxRotationAngle, maxRotationAngle), transform.up) * direction;
            //// Rotate the rigidbody towards the target
            //Quaternion targetRotation = Quaternion.LookRotation(direction);
            //rigid.AddTorque(new Vector3(Mathf.DeltaAngle(transform.eulerAngles.x, targetRotation.eulerAngles.x),
            //                         Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y),
            //                         Mathf.DeltaAngle(transform.eulerAngles.z, targetRotation.eulerAngles.z)) * turnSpeed);
            //// Reduce speed based on rotation angle
            //float angle = Quaternion.Angle(previousRotation, transform.rotation);
            //float speedReduction = 1 - (angle / 180);
            //currentVelocity = Vector3.MoveTowards(currentVelocity, transform.forward * MaxSpeed * speedReduction, acceleration * Time.fixedDeltaTime);
            //rigid.AddForce(currentVelocity);
            //previousRotation = transform.rotation;
            #endregion
            d = Vector3.Distance(pos, transform.position);
            distance += d;
            pos = transform.position;
            var dist = Vector3.Distance(Target, transform.position);
            if (dist < AutoDestroyRange && navig.NavigationType != missileNavigation.mode.None)
            {
                if (blast) { return; }
                blast = true;
                navig.hit = true;
                if (navig.GivenTarget != null && Vector3.Distance(Target, navig.GivenTarget.position) < 25 && navig.GivenTarget.gameObject.activeSelf)
                {
                    /*
                    if (navig.GivenTarget.tag.Contains("Player"))
                    {
                        if ((navig.GivenTarget.name.Contains("TejasLca")))
                        {
                            //StartCoroutine(destroy());
                            //Just For EW Testing
                            if (navig.GivenTarget.gameObject.GetComponentInChildren<EWRadar>().testing)
                            {
                                StartCoroutine(destroy());
                                navig.GivenTarget.gameObject.GetComponentInChildren<EWRadar>().hittedMissile();
                                return;
                            }
                        }

                    }*/

                    //FiredBy
                    if(navig.FiredBy !=null && navig.FiredBy.TryGetComponent<CombineUttam>(out CombineUttam radar))
                    {
                        radar.hit++;
                    }else
                    if (navig.FiredBy != null && navig.FiredBy.TryGetComponent<GroundRadar>(out GroundRadar radarG))
                    {
                        radarG.hit++;
                    }
                    else
                    if (navig.FiredBy != null && navig.FiredBy.TryGetComponent<CarrierShipController>(out CarrierShipController radarS))
                    {
                        radarS.hit++;
                    }
                    else
                    if (navig.FiredBy != null && navig.FiredBy.TryGetComponent<PilotRadarIntegration>(out PilotRadarIntegration radarP))
                    {
                        radarP.tracker.hit++;
                    }

                    //DestroyBy
                    if (navig.GivenTarget.TryGetComponent<CombineUttam>(out CombineUttam radar1))
                    {
                        radar1.destroyby = navig.FiredBy;
                    }
                    else
                       if (navig.GivenTarget.TryGetComponent<GroundRadar>(out GroundRadar radarG1))
                    {
                        radarG1.destroyby = navig.FiredBy;
                    }
                    else
                       if (navig.GivenTarget.TryGetComponent<CarrierShipController>(out CarrierShipController radarS1))
                    {
                        radarS1.destroyby = navig.FiredBy;
                    }
                    else
                   if (navig.GivenTarget.TryGetComponent<PilotRadarIntegration>(out PilotRadarIntegration radarP1))
                    {
                        radarP1.tracker.destroyby = navig.FiredBy;
                    }

                    string targt = loadSystemAndSpawnPlanes.findCraftType(navig.GivenTarget.gameObject);
                    string By = transform.name;
                    FeedBackRecorderAndPlayer.AddEvent(By + " Missile  destroy " + targt);
                    if (navig.Finish)
                        navig.GivenTarget.gameObject.SetActive(false);
                }

                StartCoroutine(destroy());
            } 

        }
        else
        {
            //Debug.Log("fuel low");
        }
        missileVelocity = d*(1/Time.fixedDeltaTime)*3.6f;

        
        pos = transform.position;
        
        if (distance >= Range*1000f||transform.position.y<0f)           /////////////////////////If Below Ground Level
        {
            if (!rigid.useGravity)
            {
                rigid.velocity = transform.forward * (MaxSpeed / 3.6f);
            }

            rigid.useGravity = true;
            StartCoroutine(destroy(5f));
           
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(/*navig.NavigationType == missileNavigation.mode.None &&*/ !blast)
        {
            var otherObj = collision.transform.root.gameObject;
            if (!otherObj.activeSelf) { return; }
            print(otherObj);
            blast = true;
            /*
            if (collision.transform.tag.Contains("Player")) //Ally or Player
            {
                if (collision.gameObject.name.Contains("General"))  //Main Player
                {
                    collision.gameObject.SetActive(false);
                    return;
                }

                string targt = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(collision.gameObject);
                string By = transform.name;
                FeedBackRecorderAndPlayer.AddEvent(By + " Missile  destroy " + targt);


                if (navig.Finish)
                    collision.gameObject.SetActive(false);
            }
            else if(collision.transform.tag.Contains("Enemy"))  //Enemy
            {

                string targt = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(collision.gameObject);
                string By = transform.name;
                FeedBackRecorderAndPlayer.AddEvent(By + " Missile  destroy " + targt);
                if (navig.Finish)
                    collision.gameObject.SetActive(false);
            }
            */



            if (collision.transform.root.tag.ToLower().Contains("player") || collision.transform.root.tag.ToLower().Contains("enemy") || collision.transform.tag.ToLower().Contains("enemy")) {

                if (navig.GivenTarget == collision.transform.root.gameObject  || navig.GivenTarget == collision.transform) {
                   
                    ///firedBy
                    if (navig.FiredBy != null && navig.FiredBy.TryGetComponent<CombineUttam>(out CombineUttam radar))
                    {
                        radar.hit++;
                    }
                    else
                        if (navig.FiredBy != null && navig.FiredBy.TryGetComponent<GroundRadar>(out GroundRadar radarG))
                    {
                        radarG.hit++;
                    }
                    else
                        if (navig.FiredBy != null && navig.FiredBy.TryGetComponent<CarrierShipController>(out CarrierShipController radarS))
                    {
                        radarS.hit++;
                    }
                    else
                    if (navig.FiredBy != null && navig.FiredBy.TryGetComponent<PilotRadarIntegration>(out PilotRadarIntegration radarP))
                    {
                        radarP.tracker.hit++;
                    }

                    //DestroyBy
                    if (collision.transform.TryGetComponent<CombineUttam>(out CombineUttam radar1))
                    {
                        radar1.destroyby = navig.FiredBy;
                    }
                    else
                       if (collision.transform.TryGetComponent<GroundRadar>(out GroundRadar radarG1))
                    {
                        radarG1.destroyby = navig.FiredBy;
                    }
                    else
                       if (collision.transform.TryGetComponent<CarrierShipController>(out CarrierShipController radarS1))
                    {
                        radarS1.destroyby = navig.FiredBy;
                    }
                    else
                   if (collision.transform.TryGetComponent<PilotRadarIntegration>(out PilotRadarIntegration radarP1))
                    {
                        radarP1.tracker.destroyby = navig.FiredBy;
                    }


                }

                string targt = loadSystemAndSpawnPlanes.findCraftType(collision.transform.root.gameObject);
                string By = transform.name;
                FeedBackRecorderAndPlayer.AddEvent(By + " Missile  destroy " + targt);
                if (navig.Finish)
                    collision.transform.root.gameObject.SetActive(false);
            }

            StartCoroutine(destroy());
        }
    }

    void SaveRoutine(GameObject gameObject)
    {
        if (gameObject.GetComponent<SaveEntityDatas>())
        {
            Save.saveFunctionsCount--;
            gameObject.GetComponent<SaveEntityDatas>().SaveFinalData();
        }
    }
    IEnumerator destroy(float t=0.0f)
    {
        
        yield return new WaitForSeconds(t);

        /// Explosion VFX
        var obj = Instantiate(exploPrefab, transform.position, transform.rotation);
        Destroy(obj.gameObject,SmokeTime);
        gameObject.SetActive(false);
        //explosion();
    }
    public void explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
}
