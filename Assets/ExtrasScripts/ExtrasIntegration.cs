using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExtrasIntegration : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public Transform BombDropSpot; 
    public TextMeshProUGUI DropDistance;
    public RectTransform BombDropDirection;
    public RadarScreenUpdate_new radarScreenUpdate;
    public UttamRadar uttamRadar;
    public Camera _camera;
    public Transform GunNose;
    private List<Sprits> EnemyPrefabs;
    public GameObject Missile;
    public bool localTransform = false;
    public LoadSystemAndSpawnPlanes loadSystemAndSpawnPlanes;
    public bool Info = false;
    private Transform player;
    void Start()
    {
        player = uttamRadar.transform.root;
        EnemyPrefabs = new List<Sprits>();
        for (var i = 0; i < 50; i++)
        {
            var obj = Instantiate(EnemyPrefab, transform);
            obj.transform.SetParent(transform);
            Transform t = obj.transform;
            Sprits sprits = new Sprits();
            sprits.sprite = obj;
            sprits.ally = t.GetChild(0).gameObject;
            sprits.enemy = t.GetChild(1).gameObject;
            //sprits.dir = t.GetChild(2).gameObject;
            sprits.Select = t.GetChild(2).gameObject;
            sprits.HighThreat = t.GetChild(3).gameObject;
            sprits.Lock = t.GetChild(4).gameObject;
            sprits.HardLock = t.GetChild(5).gameObject;
            sprits.Distance = t.GetChild(6).GetComponent<TextMeshProUGUI>();
            sprits.Altitude = t.GetChild(7).GetComponent<TextMeshProUGUI>();
            sprits.Speed = t.GetChild(8).GetComponent<TextMeshProUGUI>();
            sprits.Heading = t.GetChild(9).GetComponent<TextMeshProUGUI>();
            sprits.name = t.GetChild(10).GetComponent<TextMeshProUGUI>();
            sprits.Id = t.GetChild(11).GetComponent<TextMeshProUGUI>();
            obj.SetActive(false);

            EnemyPrefabs.Add(sprits);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HudUpdate();
    }
    //Method for update all radar modes on screen
    public void HudUpdate()
    {

        Vector3 pos1 = _camera.ViewportToScreenPoint(new Vector3(1, 1, 0));
        //bomb
        Vector3 pos = _camera.WorldToScreenPoint(radarScreenUpdate.BombDropPosition);

        pos.z = Mathf.Clamp(pos.z, -1f, 1f);
        pos.x = pos.x < 0 ? 2f : (pos.x > pos1.x ? pos1.x - 2 : pos.x);
        pos.y = pos.y < 0 ? 2f : (pos.y > pos1.y ? pos1.y - 2 : pos.y);
        
        //Debug.Log(BombDropSpot.localPosition.x);
        if (uttamRadar.HardLockTargetObject != null&&(uttamRadar.SubMode == UttamRadar.Mode2.AGR|| uttamRadar.SubMode == UttamRadar.Mode2.STCT))
        {
            BombDropSpot.position = pos;
            if(!BombDropSpot.gameObject.activeSelf)
                BombDropSpot.gameObject.SetActive(true);

            if(!DropDistance.gameObject.activeSelf)
                DropDistance.gameObject.SetActive(true);

            DropDistance.text = Vector3.Distance(radarScreenUpdate.BombDropPosition, uttamRadar.HardLockTargetObject.transform.position).ToString("0");
            Vector3 dir = (BombDropSpot.localPosition - Vector3.zero);
            float ang = Vector3.Angle(dir, transform.up);
            ang = BombDropSpot.localPosition.x < 0 ? ang : -ang;
            BombDropSpot.localEulerAngles = new Vector3(0, 0, ang);
            BombDropDirection.localEulerAngles = new Vector3(0, 0, ang);
            BombDropDirection.sizeDelta = new Vector2 (2, dir.magnitude);

            if (!BombDropDirection.gameObject.activeSelf)
                BombDropDirection.gameObject.SetActive(true);
        }
        else
        {
            if (DropDistance.gameObject.activeSelf)
                DropDistance.gameObject.SetActive(false);

            if (BombDropSpot.gameObject.activeSelf)
                BombDropSpot.gameObject.SetActive(false);

            if (BombDropDirection.gameObject.activeSelf)
                BombDropDirection.gameObject.SetActive(false);
        }

        if (uttamRadar.hitActive)
        {
            GunNose.gameObject.SetActive(true);
            pos = _camera.WorldToScreenPoint(uttamRadar.hitPoint);
            pos.z = Mathf.Clamp(pos.z, -1f, 1f);
            pos.x = pos.x < 0 ? 2f : (pos.x > pos1.x ? pos1.x - 2 : pos.x);
            pos.y = pos.y < 0 ? 2f : (pos.y > pos1.y ? pos1.y - 2 : pos.y);
            GunNose.position = pos;
        }
        else
        {
            GunNose.gameObject.SetActive(false);
        }


        var i = 0;
        foreach (var ob in EnemyPrefabs)
        {
            if (i < uttamRadar.DetectedObjects.Count)
            {
                var detect = uttamRadar.DetectedObjects[i];

                if (detect != null)
                {
                    if(!ob.sprite.activeSelf)
                        ob.sprite.SetActive(true);

                    //ob.transform.localRotation = Quaternion.Euler(0, 0, -RadarParentRot.z);
                    var obj = uttamRadar.DetectedObjects[i];
                    
                    
                    pos = _camera.WorldToScreenPoint(obj.transform.position);
                    
                    pos.z = Mathf.Clamp(pos.z, -1f, 1f);
                    if (localTransform)
                    {
                        
                        ob.sprite.transform.localPosition = pos-(pos1/2f);
                        //Debug.Log(pos - (pos1 / 2f));
                    }
                    else
                    {
                        pos.x = pos.x < 0?2f:(pos.x>pos1.x?pos1.x-2:pos.x);
                        pos.y = pos.y < 0 ? 2f : (pos.y > pos1.y ? pos1.y - 2 : pos.y);
                        ob.sprite.transform.position = pos;
                        
                    }
                    if (!uttamRadar.LockTargetList.Contains(obj) && uttamRadar.PtOnly && (uttamRadar.SubMode == UttamRadar.Mode2.HPT || uttamRadar.SubMode == UttamRadar.Mode2.ACM))
                    {
                        if (ob.sprite.activeSelf)
                            ob.sprite.SetActive(false);
                    }
                    if (ob.sprite.activeSelf)
                    {
                        //Distance Altitude Speed Bearing
                        Vector4 info = Vector4.zero;
                        float speed = 0;
                        if (uttamRadar.FoundedObjectsRB.ContainsKey(obj))
                            speed = Mathf.Round(uttamRadar.FoundedObjectsRB[obj].velocity.magnitude * 1.944f);//knots
                        else
                        if (obj.GetComponent<MoveDummy>())
                            speed = Mathf.Round(obj.GetComponent<MoveDummy>().speed * 1.944f);

                        float altitude = (obj.transform.position.y * 3.281f) / 1000;//feet
                        float bearing = uttamRadar.transform.root.eulerAngles.y - obj.transform.eulerAngles.y;//degree
                        float Distance = (Vector3.Distance(obj.transform.position, uttamRadar.transform.position) / 1852);//Nauticle Mile
                        info.x = Distance;
                        info.y = altitude;
                        info.z = speed;
                        info.w = bearing;

                        SetIcon(ob, obj, info);
                    }

                    /*
                    //DummyTarget.position = obj.transform.position;


                    //var x = 0f;
                    //var y = 0f;
                    //var z = 0f;

                    //if (SubMode == Mode2.TWS || SubMode == Mode2.HPT || SubMode == Mode2.ACM)
                    //{
                    //    var w = (Mathf.Tan(uttamRadar.Azimuth * Mathf.Deg2Rad) * uttamRadar.Range * uttamRadar.mile) * 2f;
                    //    Display.transform.localPosition = new Vector3(0, -148.5f, 0);
                    //    x = (DummyTarget.localPosition.x / (w / 2f)) * (Display.rect.width / 2f);
                    //    y = (DummyTarget.localPosition.z / (uttamRadar.Range * uttamRadar.mile)) * (Display.rect.height / 2f);
                    //    z = 0f;
                    //}
                    //else if (SubMode == Mode2.HRM || SubMode == Mode2.GMTI || SubMode == Mode2.AGR)
                    //{
                    //    Display.transform.localPosition = new Vector3(0, 0, 0);
                    //    var AZ = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;
                    //    var BR = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;

                    //    //Debug.Log(DummyTarget.localPosition.z);
                    //    //Debug.Log(AZ);

                    //    x = (DummyTarget.localPosition.x / (AZ / 0.92f)) * (Display.rect.width / 2f);
                    //    y = (DummyTarget.localPosition.y / (BR / 0.92f)) * (Display.rect.height / 2f);
                    //    z = 0f;
                    //}
                    //else if (SubMode == Mode2.STWS || SubMode == Mode2.STCT)
                    //{
                    //    Display.transform.localPosition = new Vector3(0, 0, 0);
                    //    var AZ = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;
                    //    var BR = MathF.Sin((uttamRadar.MapCamera.fieldOfView / 2f) * (MathF.PI / 180)) * DummyTarget.localPosition.z;

                    //    //Debug.Log(DummyTarget.localPosition.z);
                    //    //Debug.Log(AZ);

                    //    x = (DummyTarget.localPosition.x / (AZ / 0.92f)) * (Display.rect.width / 2f);
                    //    y = (DummyTarget.localPosition.y / (BR / 0.92f)) * (Display.rect.height / 2f);
                    //    z = 0f;
                    //}
                    //else
                    //{
                    //    ob.SetActive(false);
                    //}


                    //ob.transform.localPosition = new Vector3(x, y, z);


                    //DummyTarget.localPosition = Vector3.zero;
                    */

                }

            }
            else
            {
                if(ob.sprite.activeSelf)
                    ob.sprite.SetActive(false);
                for (var b = 0; b < ob.sprite.transform.childCount; b++)
                {
                    if (ob.sprite.transform.GetChild(b).gameObject.activeSelf)
                        ob.sprite.transform.GetChild(b).gameObject.SetActive(false);
                }
            }
            i++;
        }
    }



    //update detected objects icon
    /*square = track object
     * traingle = missile
     * diamond = lock
     * cross = hard lock ready for fire
     * dot = object
     */

    public void SetIcon(Sprits sprits, GameObject enemy, Vector4 info)
    {
        ///Check Enemy Or Player
        if (enemy.transform.tag.ToLower().Contains("player"))
        {
            if(!sprits.ally.activeSelf)
                sprits.ally.SetActive(true);

            if (sprits.enemy.activeSelf)
                sprits.enemy.SetActive(false);
        }
        else
        {
            if (sprits.ally.activeSelf)
                sprits.ally.SetActive(false);

            if (!sprits.enemy.activeSelf)
                sprits.enemy.SetActive(true);
            /*
            //if (/*(enemy.transform.tag.ToLower().Contains("player") || enemy.transform.tag.ToLower().Contains("ally")) &&Cit)
            {
                
                var spec = enemy.GetComponent<Specification>();
                if (spec!=null&&spec.iff == uttamRadar.iff)
                {
                    ping.transform.GetChild(0).gameObject.SetActive(true);
                    ping.transform.GetChild(1).gameObject.SetActive(false);
                }
                

            }
            */
        }
        /*
        //Target Bearing
        //AAST / NAST
        var a = uttamRadar.transform.root.eulerAngles.y;
        var b = enemy.transform.eulerAngles.y;
        var Angle = info.w; //uttamRadar.transform.root.eulerAngles.y - enemy.transform.eulerAngles.y;
        
        if (AAST)
        {
            sprits.dir.SetActive(true);
            sprits.dir.transform.eulerAngles = new Vector3(0, 0, Angle);
        }
        else
        {
            var ang = ((a - b) < 0 ? -(a - b) : (a - b));
            if (ang > 160 && ang < 200)
            {
                sprits.dir.SetActive(true);
                sprits.dir.transform.eulerAngles = new Vector3(0, 0, Angle);
            }
            else
            {
                sprits.sprite.SetActive(false);

            }

        }
        */


        //Traget is Selected or not
        if (uttamRadar.SelectedTarget == enemy)
        {
            sprits.Select.SetActive(true);
        }
        else
        {
            sprits.Select.SetActive(false);

        }

        //Target is High Threat or not 
        if (uttamRadar.LockTargetList.Contains(enemy))
        {
            if(!sprits.HighThreat.activeSelf)
                sprits.HighThreat.SetActive(true);
        }
        else
        {
            if (sprits.HighThreat.activeSelf)
                sprits.HighThreat.SetActive(false);
        }

        //Target is lock or not
        if (uttamRadar.tempLockTarget && enemy == uttamRadar.lockTargetProcess)
        {
            if (sprits.HighThreat.activeSelf)
                sprits.HighThreat.SetActive(false);
            if (!sprits.Lock.activeSelf)
                sprits.Lock.SetActive(true);
        }
        else
        {
            if (sprits.Lock.activeSelf)
                sprits.Lock.SetActive(false);
        }

        //Target is HardLock
        if (uttamRadar.TargetLock && enemy == uttamRadar.HardLockTargetObject)
        {
            if (sprits.Lock.activeSelf)
                sprits.Lock.SetActive(false);
            if (!sprits.HardLock.activeSelf)
                sprits.HardLock.SetActive(true);
        }
        else
        {
            if (sprits.HardLock.activeSelf)
                sprits.HardLock.SetActive(false);
        }


        //Taregt Distance
        if (!sprits.Distance.gameObject.activeSelf)
            sprits.Distance.gameObject.SetActive(true);
        sprits.Distance.text = info.x.ToString("0.0")+"NM";
        //Taregt Altitude
        if (!sprits.Altitude.gameObject.activeSelf)
            sprits.Altitude.gameObject.SetActive(true);
        sprits.Altitude.text = info.y.ToString("0.0"+"Ft");
        //Taregt Speed
        if (!sprits.Speed.gameObject.activeSelf)
            sprits.Speed.gameObject.SetActive(true);
        sprits.Speed.text = info.z.ToString("0.0")+"Kn";
        //Taregt Heading
        //sprits.Heading.gameObject.SetActive(true);
        //sprits.Heading.text = info.w.ToString("0.0");

        //Target Name
        if (enemy == uttamRadar.SelectedTarget)
        {
            if (!sprits.name.gameObject.activeSelf)
                sprits.name.gameObject.SetActive(true);
            if (enemy.name.ToLower().Contains("tejas"))
            {
                sprits.name.text = uttamRadar.JEM ? "TJ" : "";
            }
            else
            if (enemy.name.ToLower().Contains("f16"))
            {
                sprits.name.text = uttamRadar.JEM ? "F16" : "";
            }
            else
            if (enemy.name.ToLower().Contains("f 18"))
            {
                sprits.name.text = uttamRadar.JEM ? "F18" : "";
            }
            else
            if (enemy.name.ToLower().Contains("sukhoi"))
            {
                sprits.name.text = uttamRadar.JEM ? "S30" : "";
            }
            else
            {
                sprits.name.text = uttamRadar.JEM ? "" : "";
            }
        }
        else
        {
            if (sprits.name.gameObject.activeSelf)
                sprits.name.gameObject.SetActive(false);
        }

        if (Info)
        {
            if (!sprits.Id.gameObject.activeSelf)
                sprits.Id.gameObject.SetActive(true);
            string text = loadSystemAndSpawnPlanes.findCraftType(enemy);
            text = text.Replace("Ally", "Blue");
            text = text.Replace("Enemy", "Red");
            sprits.Id.text = text;
        }
        else
        {
            if (sprits.Id.gameObject.activeSelf)
                sprits.Id.gameObject.SetActive(false);
        }
    }
    /*
    public void SetIcon(GameObject ping, GameObject enemy)
    {
        var dis = Mathf.Round(Vector3.Distance(enemy.transform.position, player.position)/1852);
        float speed=0;
        if (uttamRadar.FoundedObjectsRB.ContainsKey(enemy))
            speed = Mathf.Round(uttamRadar.FoundedObjectsRB[enemy].velocity.magnitude * 1.944f);
        else
        if (enemy.GetComponent<MoveDummy>())
            speed = Mathf.Round(enemy.GetComponent<MoveDummy>().speed * 3.6f);
        ping.transform.GetChild(7).gameObject.SetActive(true);
        ping.transform.GetChild(12).gameObject.SetActive(true);
        ping.transform.GetChild(10).gameObject.SetActive(true);
        ping.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "D" + dis + "NM";
        ping.transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = speed + "NM/h";
        if (uttamRadar.HardLockTargetObject == enemy)
        {
            if (enemy.name.ToLower().Contains("tejas"))
            {
                //ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? enemy.name[0].ToString() : "";
                ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? "TJ" : "";
            }
            else
            if (enemy.name.ToLower().Contains("f16"))
            {
                ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? "F16" : "";
            }
            else
            if (enemy.name.ToLower().Contains("f 18"))
            {
                ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? "F18" : "";
            }
            else
            if (enemy.name.ToLower().Contains("sukhoi"))
            {
                ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? "S30" : "";
            }
            else
            if (enemy.name.ToLower().Contains("airplane") || enemy.name.ToLower().Contains("neutral"))
            {
                ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? "NU" : "";
            }
            else
            {
                ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? "" : "";
            }
        }
        else
        {
            ping.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = uttamRadar.JEM ? "" : "";
        }
        if (enemy.transform.tag.ToLower().Contains("player"))
        {
            ping.transform.GetChild(0).gameObject.SetActive(true);
            ping.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ping.transform.GetChild(2).gameObject.SetActive(false);
            ping.transform.GetChild(3).gameObject.SetActive(false);
            ping.transform.GetChild(0).gameObject.SetActive(false);
            ping.transform.GetChild(1).gameObject.SetActive(true);
            //if (/*(enemy.transform.tag.ToLower().Contains("player") || enemy.transform.tag.ToLower().Contains("ally")) &&* Cit)
            //{
            //    var spec = enemy.GetComponent<Specification>();
            //    if (spec != null && spec.iff == uttamRadar.iff)
            //    {
            //        ping.transform.GetChild(0).gameObject.SetActive(true);
            //        ping.transform.GetChild(1).gameObject.SetActive(false);
            //    }

            //}
        }
        if (uttamRadar.LockTargetList.Contains(enemy) )
        {
            ping.transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            ping.transform.GetChild(4).gameObject.SetActive(false);
        }

        if (uttamRadar.tempLockTarget && enemy == uttamRadar.lockTargetProcess)
        {
            ping.transform.GetChild(4).gameObject.SetActive(false);
            ping.transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            ping.transform.GetChild(5).gameObject.SetActive(false);
        }
        if (uttamRadar.TargetLock && enemy == uttamRadar.HardLockTargetObject)
        {
            ping.transform.GetChild(5).gameObject.SetActive(false);
            ping.transform.GetChild(6).gameObject.SetActive(true);
        }
        else
        {
            ping.transform.GetChild(6).gameObject.SetActive(false);
        }

    }*/
}
