using newTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainContourFlying : MonoBehaviour
{
    [Header("Values To Change")]
    public LayerMask groundLayer = 1 << 6; // | 1 << 5;// Ignore Raycast (Layer 2) AND UI (Layer 5)
    public float tcfDistance=250,pitchI=0.12f,pitchSmooth=0.12f,maxPitch=15, stableAngle=3, stableSpeed=200, angleConst = 70;
    [Header("Values To Observe")]
    public SilantroController controller;
    public float pitch, mPitch, angle, currentPitchAngle;
    private float vel;
    private GameObject rot;
    Rigidbody rb;
    void Start()
    {
        rot = new GameObject("Dummytcf");
        rot.transform.parent = this.transform;
        rot.transform.position = transform.position;
        rot.transform.rotation = transform.rotation;
        rot.transform.Rotate(15, 0, 0);
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<SilantroController>();
    }
    private void OnEnable()
    {
        GetComponent<SilantroController>().inputType = SilantroController.InputType.AI;
    }
    private void OnDisable()
    {
        GetComponent<SilantroController>().inputType = SilantroController.InputType.Default;
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > stableSpeed+10) controller.input.rawThrottleInput = 0;
        else if (rb.velocity.magnitude < stableSpeed-10) controller.input.rawThrottleInput = 1;
        else controller.input.rawThrottleInput = 1 / (rb.velocity.magnitude / (stableSpeed - 10));
        angle = 15;
        //Ray ray = new Ray(transform.position, rot.transform.forward);
        //RaycastHit[] hits = Physics.RaycastAll(ray, 10000);
        //foreach (RaycastHit hit in hits)
        //{
        //    if (hit.collider.gameObject.layer == 6)
        //    {
        //        angle = Mathf.Clamp((hit.distance - tcfDistance )/ angleConst, -maxPitch, maxPitch);
        //    }
        //}
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position + rot.transform.forward * 1000, Color.black);
        if (Physics.Raycast(transform.position, rot.transform.forward, out hit,30000, groundLayer))
        {
            angle = Mathf.Clamp((hit.distance - tcfDistance) / angleConst, -maxPitch, maxPitch);
        }
        var flatForward = transform.forward;
        flatForward.y = 0;
        flatForward.Normalize();
        var localFlatForward = transform.InverseTransformDirection(flatForward);
        currentPitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * 60;
        PitchSetter(angle+stableAngle);
    }
    void PitchSetter(float targetAngle)
    {
        var deltaAngle = targetAngle - currentPitchAngle;
        pitch = Mathf.Clamp(deltaAngle * pitchI, -1, 1);
        mPitch = Mathf.SmoothDamp(mPitch, pitch, ref vel, pitchSmooth);
        controller.input.rawPitchInput = mPitch;
        controller.input.rawRollInput = 0;

    }
}
