using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainAvoidanceOverrider : MonoBehaviour
{
    [Header("Values To Change")]
    public GameObject Text;
    public float pitch, roll, pitchI = 0.04f, rollI = 0.012f; //,pitchSmooth=0.12f, rollSmooth=0.16f;

    [Header("Values To Observe")]
    public Vector3 WayPoint;
    public Vector3 error;
    public float maxPitch = 90, maxRoll = 0, currentPitchAngle, currentRoll;


    private SilantroController controller;
    private EnemyFollowCustomSilantro inputss;
    private SilantroController.InputType originalType;
    private bool tcf;
    private float mPitch, mRoll,vel,vel2;


    void Awake()
    {
        //inputss = GetComponent<EnemyFollowCustomSilantro>();
        controller = GetComponent<SilantroController>();
        originalType = controller.inputType;
    }
    private void OnEnable()
    {
        originalType = controller.inputType;
        controller.inputType = SilantroController.InputType.AI;
        if (GetComponent<TerrainContourFlying>())
        { if (GetComponent<TerrainContourFlying>().enabled == true) { tcf = true; } GetComponent<TerrainContourFlying>().enabled = false; }
        //else if (inputss == null || inputss.enabled == true) { controller.inputType = SilantroController.InputType.AI; }
        //else { inputss.enabled = false; }
    }
    private void OnDisable()
    {
        controller.inputType = originalType;
        if (GetComponent<TerrainContourFlying>() && tcf)
        { GetComponent<TerrainContourFlying>().enabled = true;tcf = false; }
        //else if (inputss == null || inputss.enabled == true) { controller.inputType = originalType; }
        //else { inputss.enabled = true; }
    }
    void FixedUpdate()
    {
        error = WayPoint - GetComponent<Rigidbody>().position;
        error = Quaternion.Inverse(GetComponent<Rigidbody>().rotation) * error;

        var flatRight = Vector3.Cross(Vector3.up, transform.forward);
        var localFlatRight = transform.InverseTransformDirection(flatRight);
        currentRoll = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * 60;
        RollSetter(maxRoll);
        
        if ((currentRoll > 0 ? currentRoll : -currentRoll) > 60) { return; } //Could use Yaw to Controll

        var flatForward = transform.forward;
        flatForward.y = 0;
        flatForward.Normalize();
        var localFlatForward = transform.InverseTransformDirection(flatForward);
        currentPitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * 60;
        PitchSetter(-maxPitch);

        //Throttle
        controller.input.rawThrottleInput = 1;
    }
    void PitchSetter(float targetAngle)
    {
        var deltaAngle = targetAngle - currentPitchAngle;
        pitch = Mathf.Clamp(deltaAngle * pitchI, -1, 1);
        //mPitch = Mathf.SmoothDamp(mPitch, pitch, ref vel, pitchSmooth); 
        mPitch = Mathf.Lerp(mPitch, pitch, 0.3f);
        controller.input.rawPitchInput = mPitch;
    }
    void RollSetter(float targetAngle)
    {
        var deltaAngle = targetAngle - currentRoll;
        roll = Mathf.Clamp(deltaAngle * rollI, -1, 1);
        //mRoll = Mathf.SmoothDamp(mRoll, roll, ref vel2, rollSmooth);
        mRoll = Mathf.Lerp(mRoll, roll, 0.2f);
        controller.input.rawRollInput = mRoll;
    }
}
