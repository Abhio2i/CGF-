#region script info
//this script is used for detecting the planes and displaying on UI element on canvas according to their type
//attach this script directly to the gameObject which has a collider
#endregion
using Data.Plane;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using EW.Flare;
public class HUD : MonoBehaviour
{

    #region Variables
    [SerializeField] PlaneData planeData;
    [SerializeField] GameObject altitudeReference;
    [SerializeField] GameObject player;
    [SerializeField] Camera _camera;
    [SerializeField] float colliderSize = 1f;
    [SerializeField] SphereCollider colliderObject;
    //[SerializeField] MAWS_System MAWS;
   // [SerializeField] Flares flares;

    [Header("Aircraft Box PNG Prefabs")]
    [SerializeField] GameObject ally;
    [SerializeField] GameObject adversary;
    [SerializeField] GameObject neutral;
    [Header("Missile Triangle PNG Prefabs")]
    [SerializeField] GameObject neutralMissile;
    [SerializeField] GameObject adversaryMissile;
    [SerializeField] GameObject allyMissile;
    [Header("Aircraf Tags")]
    [SerializeField] string enemyTag;
    [SerializeField] string neutralTag;
    [SerializeField] string allyTag;
    [Header("Missile Tags")]
    [SerializeField] string allyMissileTag;
    [SerializeField] string enemyMissileTag;
    [SerializeField] string neutralMissileTag;
    [SerializeField] string PlayerMissileTag;

    [SerializeField] GameObject BoxCanvasReference;
    [SerializeField] GameObject TriangleCanvasReference;
    [SerializeField] List<GameObject> KeyList = new List<GameObject>();
    [SerializeField] List<GameObject> ValueList = new List<GameObject>();
    [SerializeField] Slider throttle;
    [SerializeField] TextMeshProUGUI throttleText;
    public Dictionary<Transform,GameObject> radarSpecs=new Dictionary<Transform,GameObject>();
    Dictionary<Transform, GameObject> clampUI = new Dictionary<Transform, GameObject>();
    [SerializeField] SilantroController silantro;

    private float refreshTime=5f;
    private float waitTime = 5f;
    private GameObject missile;
    public bool isRefreshing;
    #endregion
    #region not in use will require late
    /*Vector3 TransformToHUDPos(Vector3 worldSpace)
    {
        var screenSpace=_camera.WorldToScreenPoint(worldSpace);
        return screenSpace-new Vector3(_camera.pixelWidth/2,_camera.pixelHeight/2);
    }*/

    #endregion

    #region unity functions
    private void FixedUpdate()
    {
        if(missile!=null)
        {
            if (Vector3.Distance(gameObject.transform.position, missile.gameObject.transform.position) < 1000f)
            {
                //MAWS.ActivateDeactivate(true);
                //flares.missile = missile;
                //flares.SpawnFlare();
            }
        }
        //CalculateSpeed();//speed
        //CalculateAltitude();//altitude
        //colliderObject.radius = colliderSize;
        isRefreshing = false;
        refreshTime -= Time.fixedDeltaTime;
        //if (throttle)
            //throttle.value = PlayerPrefs.GetFloat("Throttle");
        throttleText.text = (silantro.input.rawThrottleInput * 100).ToString("0") + "%";
        if (refreshTime<=0)
        {
            isRefreshing = true;
            //colliderObject.radius = 0f;
            refreshTime = waitTime;
        }

        if (radarSpecs.Count <= 0)
        {
                        return;
        }
        KeyList.Clear();
        ValueList.Clear();
        try
        {
            foreach (KeyValuePair<Transform, GameObject> keyValuePair in radarSpecs)
            {
                if (keyValuePair.Value != null && keyValuePair.Key != null)
                {
                    if (isAhead(keyValuePair.Key))
                        keyValuePair.Value.SetActive(false);
                    else
                        keyValuePair.Value.SetActive(true);
                    KeyList.Add(keyValuePair.Key.gameObject);
                    ValueList.Add(keyValuePair.Value.gameObject);
                    TransformToCanvasScreen(keyValuePair.Key.gameObject, keyValuePair.Value);
                }
                if (keyValuePair.Key == null)
                {
                    Destroy(keyValuePair.Value);
                }
            }
        }
        catch
        {
            print("null");
        }
       
    }
    public void TransformToCanvasScreen(GameObject WorldObject, GameObject UI_Element)
    {
        Vector3 pos = _camera.WorldToScreenPoint(WorldObject.transform.position);
        pos.z = Mathf.Clamp(pos.z, -1f,1f);
        UI_Element.transform.position = pos;
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.CompareTag(enemyMissileTag))
    //    {
            
    //        missile=other.gameObject;
           
    //    }
    //    if (!radarSpecs.ContainsKey(other.transform))
    //    {

    //        if (other.gameObject.tag.Equals(allyTag))
    //        {
    //            GameObject obj = Instantiate(ally);
    //            obj.SetActive(false);
    //            radarSpecs.Add(other.transform, obj);
    //            obj.transform.SetParent(BoxCanvasReference.transform, false);
    //            return;
    //        }
    //        if (other.gameObject.tag.Equals(enemyTag))
    //        {
    //            GameObject obj = Instantiate(adversary);
    //            obj.SetActive(false);
    //            radarSpecs.Add(other.transform, obj);
    //            obj.transform.SetParent(BoxCanvasReference.transform, false);
    //            return;
    //        }
    //        if (other.gameObject.tag.Equals(neutralTag))
    //        {
    //            GameObject obj = Instantiate(neutral);
    //            obj.SetActive(false);
    //            radarSpecs.Add(other.transform, obj);
    //            obj.transform.SetParent(BoxCanvasReference.transform, false);
    //            return;
    //        }
    //        if (other.gameObject.tag.Equals(allyMissileTag))
    //        {
    //            GameObject obj = Instantiate(allyMissile);
    //            obj.SetActive(false);
    //            radarSpecs.Add(other.transform, obj);
    //            obj.transform.SetParent(TriangleCanvasReference.transform, false);
    //            return;
    //        }
    //        if (other.gameObject.tag.Equals(neutralMissileTag))
    //        {
    //            GameObject obj = Instantiate(neutralMissile);
    //            obj.SetActive(false);
    //            radarSpecs.Add(other.transform, obj);
    //            obj.transform.SetParent(TriangleCanvasReference.transform, false);
    //            return;
    //        }
    //        if (other.gameObject.tag.Equals(enemyMissileTag))
    //        {
    //            GameObject obj = Instantiate(adversaryMissile);
    //            obj.SetActive(false);
    //            radarSpecs.Add(other.transform, obj);
    //            obj.transform.SetParent(TriangleCanvasReference.transform, false);
    //            return;
    //        }
    //    }
    //}

    bool isAhead(Transform target)
    {
        Vector3 directionToTarget = player.transform.position - target.position;
        float angle = Vector3.Angle(player.transform.forward, directionToTarget);
        if (Mathf.Abs(angle) < 90)
        {
            //if ((player.transform.position.z-target.position.z) > 0f)
            return true;
        }
        else
            return false;
    }

    bool CheckIfIAmTarget(GameObject missile)
    {
        print(missile);
        GameObject missileTarget = missile.GetComponent<MissileSystem>().target.gameObject;
        if (missileTarget.CompareTag(gameObject.tag))
        {
            return true;
        }
        else
            return false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(enemyMissileTag))
        {
            if (other.gameObject == missile)
            {
                //MAWS.ActivateDeactivate(false);
            }
        }
        if (radarSpecs.Count > 0 && !isRefreshing)
        {
            if (radarSpecs.ContainsKey(other.transform))
            {
                GameObject myObj = radarSpecs[other.transform];
                radarSpecs.Remove(other.transform);
                Destroy(myObj);
                //Destroy(radarSpecs.)
            }
        }
    }
    #endregion
}
