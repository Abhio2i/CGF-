using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vazgriz.Plane;
using static UnityEngine.UI.Image;

//Does Raycast in four 15deg angles left right down and straight to check for possible collision

public class TerrainRaycast : MonoBehaviour
{
    [Header("Values To Change")]
    public LayerMask ground = new LayerMask();
    public float ActivationRange = 2000, smoothing = 250;
    public float RollAdjuster = 2, PitchAdjuster = 8;
    public bool Vazgriz = false, tcf = false, activated = false;
    private TerrainAvoidanceOverrider AutoController;
    private AIController controller;
    private Transform AutoPosition;
    private float speed = 200;
    private Vector3 Originpos, RaycastDirection, RaycastPos;
    private GameObject rot;
    private RaycastHit hit0, hit1, hit2;
    private bool h0, h1, h2;
    private Rigidbody rb;
    void Start()
    {
        rot = new GameObject("Dummy");
        rot.transform.parent = this.transform;
        var A = new GameObject("AutoPos");
        A.transform.parent = this.transform;
        A.transform.position = transform.position + transform.forward * ActivationRange;
        AutoPosition = A.transform;
        if (Vazgriz) activated = true;
        if (Vazgriz)
            controller = GetComponent<AIController>();
        else
            AutoController = GetComponent<TerrainAvoidanceOverrider>();
        rb = GetComponent<Rigidbody>();
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawSphere(AutoPosition.position,100);
    //}
    void Update()
    {
        Originpos = transform.position + transform.forward * ActivationRange;
        rot.transform.position = transform.position;
        rot.transform.rotation = transform.rotation;
        speed = rb.velocity.magnitude;
        if (speed < 100) { return; }
        speed = (speed / 200);
        if (!activated) return;
        RaycastPos = transform.position + transform.forward * 15;
        PitchSetter();
        RollSetter();

    }
    void PitchSetter()
    {
        //Raycasting
        if (h0 = Physics.Raycast(RaycastPos, transform.forward, out hit0, 1.2f * ActivationRange / speed,ground))     //Racast 1 for pitch {froward}
        {
                AutoPosition.position = Originpos + (ActivationRange / speed - hit0.distance) * transform.up;
        }
        //Debug.DrawLine(RaycastPos, RaycastPos + transform.forward * ActivationRange / speed, Color.magenta);
        rot.transform.Rotate(-15, 0, 0);
        RaycastDirection = rot.transform.forward;
        if (h1 = Physics.Raycast(RaycastPos, RaycastDirection, out hit1,1.4f * ActivationRange / speed,ground))     //Racast 1 for pitch {up 15deg}
        {
                AutoPosition.position = Originpos + (ActivationRange / speed - hit1.distance) * transform.up;
        }
        //Debug.DrawLine(RaycastPos, RaycastPos + RaycastDirection * ActivationRange / speed, Color.magenta);
        rot.transform.Rotate(30, 0, 0);
        RaycastDirection = rot.transform.forward;
        if (h2 = Physics.Raycast(RaycastPos, RaycastDirection, out hit2, ActivationRange / speed / 2,ground))        //Racast 2 for pitch {down 15deg}
        {
            if (tcf) { h2 = false; }
            else { AutoPosition.position = Originpos + (ActivationRange / speed - hit2.distance) * transform.up; }
        }
        //Debug.DrawLine(RaycastPos, RaycastPos + RaycastDirection * ActivationRange / speed, Color.magenta);
        //Activating AutoControll
        if (!h0 && !h1 && !h2) { AutoPosition.position = Vector3.MoveTowards(AutoPosition.position, Originpos, Time.deltaTime * smoothing * (Vazgriz ? 5 : 1)); }
        if ((AutoPosition.position - Originpos).magnitude > 10)
        {
            if (Vazgriz)
            {
                controller.TerrainAvoidance = true;
                controller.targetPosition = AutoPosition.position;
                return;
            }
            else
                AutoController.enabled = true;
        }
        else
        {
            if (Vazgriz) { controller.TerrainAvoidance = false; return; }
            AutoController.enabled = false;
        }

        //Assigning value to AutoController
        AutoController.maxPitch = Vector3.Angle(transform.forward,AutoPosition.position-transform.position)* PitchAdjuster;
    }

    void RollSetter()
    {
        //RayCasting
        rot.transform.Rotate(-15, 15, 0);
        RaycastDirection = rot.transform.forward;
        if (h1 = Physics.Raycast(RaycastPos, RaycastDirection, out hit1, ActivationRange / speed / 2,ground))      //Racast 1 for roll  {left 15deg}
        {
                AutoPosition.position = Originpos + (ActivationRange / speed - hit1.distance) * -transform.right;
        }
        Debug.DrawLine(RaycastPos, RaycastPos + RaycastDirection * ActivationRange / speed, Color.cyan);
        rot.transform.Rotate(0, -30, 0);
        RaycastDirection = rot.transform.forward;
        if (h2 = Physics.Raycast(RaycastPos, RaycastDirection, out hit2, ActivationRange / speed / 2,ground))      //Racast 2 for roll  {right 15deg}
        {
                AutoPosition.position = Originpos + (ActivationRange / speed - hit2.distance) * transform.right;
        }
        Debug.DrawLine(RaycastPos, RaycastPos + RaycastDirection * ActivationRange / speed, Color.black);

        //Activating AutoControll
        if (!h1 && !h2) { AutoPosition.position = Vector3.MoveTowards(AutoPosition.position, Originpos, Time.deltaTime * smoothing * (Vazgriz ? 5 : 1)); }
        if ((AutoPosition.position - Originpos).magnitude > 10)
        {
            if (Vazgriz)
            {
                controller.TerrainAvoidance = true;
                controller.targetPosition = AutoPosition.position;
                return;
            }
            else
                AutoController.enabled = true;
        }
        else
        {
            if (Vazgriz) { controller.TerrainAvoidance = false; return; }
            AutoController.enabled = false;
        }

        //Assigning values to AutoController
        if (h1 && h2) { AutoController.maxRoll = 0; }
        else if (h1) { AutoController.maxRoll = -Vector3.Angle(transform.forward, AutoPosition.position - transform.position)/hit1.distance* RollAdjuster; }
        else if (h2) { AutoController.maxRoll = Vector3.Angle(transform.forward, AutoPosition.position - transform.position)/hit2.distance*RollAdjuster; }
        else { AutoController.maxRoll = 0; }
    }
    public void activate(bool t)
    {
        activated = t;
    }
}
