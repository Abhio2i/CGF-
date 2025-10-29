using Esri.ArcGISRuntime.MapView;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;

public class AIHud : MonoBehaviour
{
    [Range(1f, 8f)]
    public int Display = 4;

    [Header("Aircraft Information")]
    public float Speed = 1f;
    public float Altitude = 0f;
    public float Throttle = 0f;
    public float AOA = 0f;
    public float pitch = 0f;
    public float roll = 0f;
    public float yaw = 0f;

    [Header("UI Information")]
    public bool UiEnable;
    public Text Speed_Text;
    public Text Speed2_Text;
    public Text Altitude_Text;
    public Text throttle_Text;
    public Text TargetLock;
    public Text detected;
    public Text distance;
    public Text Angle;
    public Text Fire;
    public Text pitch_Text;
    public Text roll_Text;
    public Text yaw_Text;

    public Canvas canvas;
    public bool localTransform = false;
    public UnityEngine.Camera cam;
    private List<GameObject> EnemyPrefabs;

    [Header("Refrences")]
    public AeroplaneController aeroplaneController;
    public AeroplaneAiControl aeroplaneAiControl;
    public GameObject EnemyPrefab;
    public UnityEngine.Camera _camera;
    public AIRadar aiRadar;
    public CombineUttam combineUttam;
    public Brain brain;
    public Rigidbody rb;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = combineUttam.transform.root;
        rb = GetComponent<Rigidbody>();
        canvas.targetDisplay = Display;
        cam.targetDisplay = Display;
        EnemyPrefabs = new List<GameObject>();
        for (var i = 0; i < 50; i++)
        {
            var obj = Instantiate(EnemyPrefab, transform);
            obj.transform.SetParent(canvas.transform);
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            obj.SetActive(false);
            EnemyPrefabs.Add(obj.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Aircraft Information
        AOA = Vector3.Angle(transform.forward, -Vector3.up);
        Altitude = (transform.position.y * 3.281f);
        Speed = (rb.velocity.magnitude * 3.6f);
        pitch = aeroplaneAiControl.pitch;
        roll = aeroplaneAiControl.roll;
        yaw = aeroplaneAiControl.yaw;
        #endregion


        pitch_Text.text = pitch.ToString();
        roll_Text.text = roll.ToString();
        yaw_Text.text = yaw.ToString();

        float kn = (Speed * 0.539957f);
        kn = kn > 667f ? kn / 667 : kn;
        Speed_Text.text = kn.ToString("0.0") + (kn > 667f ? "Mach" : "kn");
        if (Speed2_Text != null)
            Speed2_Text.text = Speed.ToString("0") + "km/hr";
        Altitude_Text.text = Altitude.ToString("0") + "ft";
        throttle_Text.text = (aeroplaneAiControl.throttleInput * 100f).ToString("0") + "%";

        TargetLock.text = brain.combineUttam.Target != null ? "targetLock" : "";
        distance.text = brain.Distance.ToString("0");
        Angle.text = brain.Angle.ToString("0"); 
        Fire.text = brain.combineUttam._missile != null ? "FireMissle" : "";
        ScreenUpdate();
    }

    public void ScreenUpdate()
    {

        Vector3 pos1 = _camera.ViewportToScreenPoint(new Vector3(1, 1, 0));
        Vector3 pos;



        var i = 0;
        foreach (var ob in EnemyPrefabs)
        {
            if (i < aiRadar.DetectedObjects.Count)
            {
                var detect = aiRadar.DetectedObjects[i];
                ob.SetActive(true);

                if (detect != null)
                {
                    ob.SetActive(true);

                    //ob.transform.localRotation = Quaternion.Euler(0, 0, -RadarParentRot.z);
                    var obj = aiRadar.DetectedObjects[i];

                    SetIcon(ob, obj);
                    pos = _camera.WorldToScreenPoint(obj.transform.position);

                    pos.z = Mathf.Clamp(pos.z, -1f, 1f);
                    if (localTransform)
                    {

                        ob.transform.localPosition = pos - (pos1 / 2f);
                        //Debug.Log(pos - (pos1 / 2f));
                    }
                    else
                    {
                        pos.x = pos.x < 0 ? 2f : (pos.x > pos1.x ? pos1.x - 2 : pos.x);
                        pos.y = pos.y < 0 ? 2f : (pos.y > pos1.y ? pos1.y - 2 : pos.y);
                        ob.transform.position = pos;

                    }
                    if (false)
                    {
                        ob.SetActive(false);

                    }
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

                }

            }
            else
            {
                ob.SetActive(false);
                for (var b = 0; b < ob.transform.childCount; b++)
                {
                    ob.transform.GetChild(b).gameObject.SetActive(false);
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
    public void SetIcon(GameObject ping, GameObject enemy)
    {
        var dis = Mathf.Round(Vector3.Distance(enemy.transform.position, player.position) / 1000f);
        //float speed = 0;
        //if (aiRadar.DetectedObjects.ContainsKey(enemy))
        //speed = Mathf.Round(uttamRadar.FoundedObjectsRB[enemy].velocity.magnitude * 3.6f);
        //else
        //if (enemy.GetComponent<MoveDummy>())
        //speed = Mathf.Round(enemy.GetComponent<MoveDummy>().speed * 3.6f);

        ping.transform.GetChild(0).gameObject.SetActive(true);
        ping.transform.GetChild(7).gameObject.SetActive(true);
        //ping.transform.GetChild(8).gameObject.SetActive(true);
        //ping.transform.GetChild(10).gameObject.SetActive(true);
        ping.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "D " + dis + "km";
        //ping.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = speed + "km/hr";

        /*
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
        if (enemy.transform.tag.ToLower().Contains("missile"))
        {
            if (enemy.transform.tag.ToLower().Contains("good"))
            {
                ping.transform.GetChild(2).gameObject.SetActive(true);
                ping.transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                ping.transform.GetChild(2).gameObject.SetActive(false);
                ping.transform.GetChild(3).gameObject.SetActive(true);
            }

            ping.transform.GetChild(0).gameObject.SetActive(false);
            ping.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ping.transform.GetChild(2).gameObject.SetActive(false);
            ping.transform.GetChild(3).gameObject.SetActive(false);
            ping.transform.GetChild(0).gameObject.SetActive(false);
            ping.transform.GetChild(1).gameObject.SetActive(true);
            if (/*(enemy.transform.tag.ToLower().Contains("player") || enemy.transform.tag.ToLower().Contains("ally"))&&*uttamRadar.Cit)
            {
                var spec = enemy.GetComponent<Specification>();
                if (spec != null && spec.iff == uttamRadar.iff)
                {
                    ping.transform.GetChild(0).gameObject.SetActive(true);
                    ping.transform.GetChild(1).gameObject.SetActive(false);
                }

            }
        }
        */

       /*
        if (uttamRadar.LockTargetList.Contains(enemy))
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
        }*/
        if (combineUttam.Target != null)
        {
            //ping.transform.GetChild(5).gameObject.SetActive(false);
            ping.transform.GetChild(6).gameObject.SetActive(true);
        }
        else
        {
            ping.transform.GetChild(6).gameObject.SetActive(false);
        }

    }
}
