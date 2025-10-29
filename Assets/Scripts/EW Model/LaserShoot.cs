using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LaserShoot : MonoBehaviour
{
    //public float range = 2000; //half of radar detection range
    public float effectRange = 4000f;
    public float idleDeflectionTime = 1f;    
    [Tooltip("Temreature in Degree Celcius")]
    public float LaserTemperature = 100f;
    public float LaserMoveSpeed = 100f;
    public GameObject TargetPosition;
    public bool isON = false;

    [Header("EW Test Only")]
    public bool testing = false;
    public bool active = false;
    public Camera cam;
    public TextMeshProUGUI DIRCMtext;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
    }

    private void Update()
    {
        if (!active) { NoLaserShooting();return; }
        FollowTarget();
        LaserTrigger();
    }
    private void FollowTarget()     //Laser Direction towards targets
    {
        isON = false;
        if (TargetPosition != null)
        {
            if (Vector3.Distance(TargetPosition.transform.position, transform.position) <= effectRange+4000)
            {
                isON = true;
                var FaceDirection = Vector3.Angle(TargetPosition.transform.position + new Vector3(0, -20f, 0f) - transform.position, transform.root.up);
                if (FaceDirection > 90)
                {                // Smoothly rotate towards the target point.
                    var targetRotation = Quaternion.LookRotation(TargetPosition.transform.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, LaserMoveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.parent.rotation, LaserMoveSpeed * Time.deltaTime);
                }
            }
        }
        
    }

    private void LaserTrigger()
    {
        
        //Debug.Log(Vector3.Angle(transform.position - TargetPosition.transform.position, TargetPosition.transform.forward));
        if (TargetPosition == null || Vector3.Angle(transform.position - TargetPosition.transform.position, TargetPosition.transform.forward) > 30) { NoLaserShooting(); return; }
        
        var Direction = transform.TransformDirection(Vector3.forward);      //Laser on Plane is pointing towards which direction
        Debug.DrawRay(transform.position, Direction * effectRange, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Direction, out hit,effectRange))      //check if Laser will actually hit something
        {
            if (hit.transform.tag.Contains("Missile"))
            {
                LaserShooting();
                Debug.DrawRay(transform.position, Direction * hit.distance, Color.red);
                //Calculating Laser intensity on target missile accorging to distance and effective range of laser
                //var hitDistance = hit.distance;
                //var dstPercent = 1 - ((hitDistance / effectRange) * (idleDeflectionTime * (hitDistance / effectRange)));
                //dstPercent = (dstPercent < 0 ? 0 : dstPercent);
                //dstPercent = (dstPercent > 0.5f ? 0.5f : dstPercent);
                //var ApplyLaserTemperature = LaserTemperature * dstPercent;
                var ApplyLaserTemperature = 60/idleDeflectionTime * Time.deltaTime * (1 - (hit.distance/ effectRange));
                if (hit.transform.GetComponent<MissileHeatSystem>()!=null)
                    hit.transform.GetComponent<MissileHeatSystem>().ApplyHeat(ApplyLaserTemperature);       //Increasing the Temperature of the Missile
            }
            else { NoLaserShooting(); Debug.Log("work"); }
        }
        else { NoLaserShooting(); Debug.Log("work"); }
    }
    #region EW_Testing
    public void IdleDeflectionSet(string s)
    {
        int value = int.Parse(s);
        idleDeflectionTime = value;
    }
    void NoLaserShooting()
    {
        if (!testing) { return; }
        lineRenderer.enabled = false;
        if(DIRCMtext)
            DIRCMtext.text = "";
    }
    void LaserShooting()
    {
        if (!testing) { return; }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, TargetPosition.transform.position);
        lineRenderer.enabled = true;
        lineRenderer.widthMultiplier = cam.orthographicSize / 100;
        if (DIRCMtext)
            DIRCMtext.text = "IR shooting";
    }
    #endregion
}
