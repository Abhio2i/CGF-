using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroVirtualLever : MonoBehaviour
{
    // ------------------------------ Selectibles
    public enum LeverType { Throttle, ControlStick, Component }
    public LeverType leverType = LeverType.ControlStick;

    public enum RotationState { Normal, Invert }
    public RotationState rotationState = RotationState.Normal;

    public enum RotationAxis { X, Y, Z }
    public RotationAxis rollAxis = RotationAxis.X;
    public RotationAxis leverAxis = RotationAxis.X;
    public RotationAxis pitchAxis = RotationAxis.X;
    public enum LeverAction { SelfCentering, NonCentering}
    public LeverAction leverAction = LeverAction.NonCentering;


    // ------------------------------ Connections
    public Transform leverCore;
    public Transform vehicle;


    // ------------------------------ Output
    public float maximumDeflection = 20f;
    public float maximumPitchDeflection = 20f, maximumRollDeflection = 20f;
    public float currentDeflection;
    public float currentPitchDeflection, currentRollDeflection;
    public float controlOutput;
    float leverInput;
    public float pitchOutput, rollOutput;

    // ------------------------------ Value Storage
    Vector3 originalPosition, originalEulerAngles;
    Quaternion originalLeverRotation, currentLeverRotation;
    Quaternion originalCoreRotation;
    public Vector3 handPosition, currentRotation;

    // ------------------------------ Triggers
    public bool rightControlHold;
    public bool leftControlHold;
    public bool leverHeld;

    // ------------------------------ Control Variables
    private readonly float leftTriggerInput;
    private readonly float rightTriggerInput;
    private readonly float leftPalmInput;
    private readonly float rightPalmInput;
    //float xRotation, yRotation, zRotation;

    public float snapSpeed = 10f;
    public float MinXDegrees = -45f;
    public float MaxXDegrees = 45f;
    public float angleDifference;


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        if (leverCore != null)
        {
            originalCoreRotation = Quaternion.Inverse(vehicle.rotation) * leverCore.rotation;
            originalLeverRotation = transform.localRotation;
            originalEulerAngles = transform.localEulerAngles;
        }
    }




    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            //Component e.g Gear Lever
            if (leverType == LeverType.Component && leftControlHold) { leverHeld = true; }
            //Control Stick
            if (leverType == LeverType.ControlStick && rightControlHold) { leverHeld = true; }
            //Throttle
            if (leverType == LeverType.Throttle && leftControlHold) { leverHeld = true; }

            //HAND DATA
            handPosition = other.transform.position;
        }
    }


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand")) { leverHeld = false; }
    }



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Update()
    {
        //ACCESS TOUCH INPUTS
        CollectTouchState();


        //PROCESS ROTATION MAIN
        if (leverHeld) { RotateLever(); }


        //RETURN TO BASE ROTATION
        if (!leverHeld && leverAction == LeverAction.SelfCentering) { transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalLeverRotation, Time.deltaTime * snapSpeed); }
        if (leverType == LeverType.Component || leverType == LeverType.Throttle) { if (!leftControlHold && leverHeld) { leverHeld = false; } }
        if (leverType == LeverType.ControlStick) { if (!rightControlHold && leverHeld) { leverHeld = false; } }


        //PROCESS ROTATION CONSTRAINT
        //xRotation = CalculateAngleDifference(1);
        //yRotation = CalculateAngleDifference(2);
        //zRotation = CalculateAngleDifference(3);
        currentRotation = transform.localRotation.eulerAngles;




        //PROCESS INPUT
        //1. 1D LEVER
        if (leverType == LeverType.Component || leverType == LeverType.Throttle)
        {
            if (leverAxis == RotationAxis.X) { currentDeflection = CalculateAngleDifference(1); }
            if (leverAxis == RotationAxis.Y) { currentDeflection = CalculateAngleDifference(2); }
            if (leverAxis == RotationAxis.Z) { currentDeflection = CalculateAngleDifference(3); }

            //OUTPUT
            if (rotationState == RotationState.Normal) { leverInput = currentDeflection / maximumDeflection; }
            if (rotationState == RotationState.Invert) { leverInput = -currentDeflection / maximumDeflection; }

            if (leverInput > 1) { leverInput = 1f; }
            if (leverInput < 0) { leverInput = 0f; }

            controlOutput = leverInput;
        }

        //2. 2D LEVER
        if (leverType == LeverType.ControlStick)
        {
            //PITCH
            if (pitchAxis == RotationAxis.X) { currentPitchDeflection = CalculateAngleDifference(1); }
            if (pitchAxis == RotationAxis.Y) { currentPitchDeflection = CalculateAngleDifference(2); }
            if (pitchAxis == RotationAxis.Z) { currentPitchDeflection = CalculateAngleDifference(3); }

            //OUTPUT
            float pitch = currentPitchDeflection / maximumPitchDeflection;
            if (pitch > 1f) { pitch = 1f; }
            if (pitch < -1) { pitch = -1f; }
            pitchOutput = pitch;


            //ROLL
            if (rollAxis == RotationAxis.X) { currentRollDeflection = CalculateAngleDifference(1); }
            if (rollAxis == RotationAxis.Y) { currentRollDeflection = CalculateAngleDifference(2); }
            if (rollAxis == RotationAxis.Z) { currentRollDeflection = CalculateAngleDifference(3); }

            //OUTPUT
            float roll = currentRollDeflection / maximumRollDeflection;
            if (roll > 1f) { roll = 1f; }
            if (roll < -1f) { roll = -1f; }
            rollOutput = roll;
        }
    }




    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void RotateLever()
    {
        Vector3 handDirection = handPosition - transform.position;
        Vector3 forward = vehicle.forward;
        angleDifference = Vector3.SignedAngle(handDirection, forward, Vector3.up);


        if (angleDifference <= MaxXDegrees && angleDifference >= MinXDegrees)
        {
            transform.LookAt(handPosition, transform.up);

            if (leverType == LeverType.Component)
            {
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, originalEulerAngles.y, originalEulerAngles.z);
            }
        }
    }



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float CalculateAngleDifference(int axis)
    {
        Vector3 delta = Vector3.zero; float deflection = 0f;

        if (leverCore != null)
        {
            Quaternion localLeverRotation = Quaternion.Inverse(vehicle.rotation) * leverCore.rotation;
            delta = originalCoreRotation.eulerAngles - localLeverRotation.eulerAngles;
        }

        //X ANGLE
        if (delta.x > 180) { delta.x -= 360; }
        else if (delta.x < -180) { delta.x += 360; }
        //Y ANGLE
        if (delta.y > 180) { delta.y -= 360; }
        else if (delta.y < -180) { delta.y += 360; }
        //Z ANGLE
        if (delta.z > 180) { delta.z -= 360; }
        else if (delta.z < -180) { delta.z += 360; }

        //DELTA ANGLE
        if (axis == 1) { deflection = delta.x; }
        if (axis == 2) { deflection = delta.y; }
        if (axis == 3) { deflection = delta.z; }

        return deflection;
    }



    //Remove the line comments to use VR
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void CollectTouchState()
    {
        //RIGHT CONTROLLER
        //rightTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        //rightPalmInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

        //LEFT CONTROLLER
        //leftTriggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        //leftPalmInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        //STATE
        if (rightTriggerInput > 0.9f && rightPalmInput > 0.9f) { rightControlHold = true; } else { rightControlHold = false; }
        if (leftTriggerInput > 0.9f && leftPalmInput > 0.9f) { leftControlHold = true; } else { leftControlHold = false; }
    }
}


#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(SilantroVirtualLever))]
public class SilantroVirtualLeverEditor : Editor
{
    Color backgroundColor;
    Color silantroColor = new Color(1, 0.4f, 0);
    SilantroVirtualLever lever;
    private void OnEnable() { lever = (SilantroVirtualLever)target; }


    public override void OnInspectorGUI()
    {
        backgroundColor = GUI.backgroundColor;
        //DrawDefaultInspector ();
        serializedObject.UpdateIfRequiredOrScript();

        GUI.color = silantroColor;
        EditorGUILayout.HelpBox("Lever Config", MessageType.None);
        GUI.color = backgroundColor;
        GUILayout.Space(2f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("leverType"), new GUIContent("Function"));
        GUILayout.Space(2f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("leverAction"), new GUIContent("Mechanism"));

        GUILayout.Space(10f);
        GUI.color = silantroColor;
        EditorGUILayout.HelpBox("Deflection Settings", MessageType.None);
        GUI.color = backgroundColor;
        GUILayout.Space(2f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationState"), new GUIContent("Rotation State"));
        
        if(lever.leverType == SilantroVirtualLever.LeverType.ControlStick)
        {
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pitchAxis"), new GUIContent("Pitch Axis"));
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rollAxis"), new GUIContent("Roll Axis"));
        }
        else if(lever.leverType == SilantroVirtualLever.LeverType.Component)
        {
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pitchAxis"), new GUIContent("Lever Axis"));
        }
        else if (lever.leverType == SilantroVirtualLever.LeverType.Throttle)
        {
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pitchAxis"), new GUIContent("Throttle Axis"));
        }

        if (lever.leverAction == SilantroVirtualLever.LeverAction.SelfCentering)
        {
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("snapSpeed"), new GUIContent("Centering Speed"));
        }
       

        GUILayout.Space(10f);
        GUI.color = silantroColor;
        EditorGUILayout.HelpBox("Value Extraction", MessageType.None);
        GUI.color = backgroundColor;
        GUILayout.Space(3f);
        if (lever.leverType == SilantroVirtualLever.LeverType.ControlStick)
        {
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumPitchDeflection"), new GUIContent("Full Pitch Deflection"));
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumRollDeflection"), new GUIContent("Full Roll Deflection"));
        }
        else if (lever.leverType == SilantroVirtualLever.LeverType.Component)
        {
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumDeflection"), new GUIContent("Full Lever Deflection"));
        }
        else if (lever.leverType == SilantroVirtualLever.LeverType.Throttle)
        {
            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumDeflection"), new GUIContent("Full Throttle Deflection"));
        }

        GUILayout.Space(5f);
        GUI.color = Color.white;
        EditorGUILayout.HelpBox("Deflection Limits", MessageType.None);
        GUI.color = backgroundColor;
        GUILayout.Space(3f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxXDegrees"), new GUIContent("Maximum Deflection"));
        GUILayout.Space(3f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MinXDegrees"), new GUIContent("Minimum Deflection"));



        GUILayout.Space(5f);
        GUI.color = Color.white;
        EditorGUILayout.HelpBox("Output", MessageType.None);
        GUI.color = backgroundColor;
        if (lever.leverType == SilantroVirtualLever.LeverType.ControlStick)
        {
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("Roll Ouput", lever.rollOutput.ToString("0.000"));
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("Pitch Ouput", lever.pitchOutput.ToString("0.000"));
        }
        else if (lever.leverType == SilantroVirtualLever.LeverType.Component)
        {
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("Lever Ouput", lever.rollOutput.ToString("0.000"));
        }
        else if (lever.leverType == SilantroVirtualLever.LeverType.Throttle)
        {
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("Throttle Ouput", lever.rollOutput.ToString("0.000"));
        }


        serializedObject.ApplyModifiedProperties();
    }
}
#endif
