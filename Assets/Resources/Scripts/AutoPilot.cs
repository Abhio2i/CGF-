using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    public bool Activate = false;
    public bool TCF = false;
    public float ver = 0;
    public float hor = 0;
    public float thro = 0;
    [Range(-180, 180)]
    public float TargetPitch = 0f;
    [Range(-180, 180)]
    public float TargetRoll = 0f;
    [Range(-180, 180)]
    public float TargetYaw = 0f;
    [Range(100, 500)]
    public float TargetSpeed = 0f;
    [Range(100, 10000)]
    public float TargetAltitude = 0f;
    [Range(-180, 180)]
    public float TargetHeading = 0f;
    [Range(50, 500)]
    public float TargetOffset = 50f;
    [Range(50, 500)]
    public float forwardDistance = 50f;
    public float StoreYaw = 0;

    public float currentRoll = 0f;
    public float currentPitch = 0f;
    public float currentYaw = 0f;
    public float currentSpeed = 0f;
    public float currentAltitude = 0f;
    public float currentHeading = 0f;
    public float RadioAltitude = 0f;
    public float climbRate = 0f;
    public bool ControlClimb = false;
    public int currentMode = 0;
    public LayerMask layerMask;
    [Header("UI")]
    public TextMeshProUGUI TcfText;
    public TextMeshProUGUI AutoPilotText;
    public TextMeshProUGUI AutoPilotValue;
    public TextMeshProUGUI currentModeText;
    public TMP_InputField TCFOffsetText;
    private SilantroController controller;
    private Rigidbody rb;
    private EnemyFollowCustomSilantro inputss;
    private SilantroController.InputType originalType;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<SilantroController>();
        originalType = controller.inputType;
        rb = GetComponent<Rigidbody>();
    }

    public void AUTOpilotActivate()
    {
        Activate = !Activate;
        AutoPilotText.color = Activate ? Color.green : Color.white;
        if (Activate)
        {
            controller.inputType = SilantroController.InputType.AI;
            TargetAltitude = currentAltitude;
            TargetHeading = currentHeading; 
            TargetSpeed = currentSpeed;

            if (currentMode == 0)
            {
                currentModeText.text = "Alt";
                AutoPilotValue.text = TargetAltitude.ToString("0");
            }
            else
        if (currentMode == 1)
            {
                currentModeText.text = "Head";
                AutoPilotValue.text = TargetHeading.ToString("0");
            }
            else
        if (currentMode == 2)
            {
                currentModeText.text = "Spd";
                AutoPilotValue.text = TargetSpeed.ToString("0");
            }
        }
        else
        {
            controller.inputType = originalType;
        }
    }

    public void CurrentModeSet()
    {

        currentMode++;
        currentMode = currentMode > 2 ? 0 : currentMode;
        if(currentMode == 0)
        {
            currentModeText.text = "Alt";
            AutoPilotValue.text = TargetAltitude.ToString("0");
        }
        else
        if (currentMode == 1)
        {
            currentModeText.text = "Head";
            AutoPilotValue.text = TargetHeading.ToString("0");
        }
        else
        if (currentMode == 2)
        {
            currentModeText.text = "Spd";
            AutoPilotValue.text = TargetSpeed.ToString("0");
        }
    }

    public void CurrentModeValue(int i)
    {

        if (currentMode == 0)
        {
            TargetAltitude += i * 100;
            TargetAltitude = Mathf.Clamp(TargetAltitude, 100, 10000);
            currentModeText.text = "Alt";
            AutoPilotValue.text = TargetAltitude.ToString("0");
        }
        else
        if (currentMode == 1)
        {
            TargetHeading += i;
            TargetHeading = Mathf.Clamp(TargetHeading, -180, 180);
            currentModeText.text = "Head";
            AutoPilotValue.text = TargetHeading.ToString("0");
        }
        else
        if (currentMode == 2)
        {
            TargetSpeed += i*10;
            TargetSpeed = Mathf.Clamp(TargetSpeed, 100, 500);
            currentModeText.text = "Spd";
            AutoPilotValue.text = TargetSpeed.ToString("0");
        }
    }


    public void TCFActivate()
    {
        TCF = !TCF;
        TcfText.color = TCF? Color.green : Color.white;
        if(TCF)
        {
            //if auto pilot not activate
            if (!Activate) AUTOpilotActivate();

            TargetHeading = currentHeading;
            TargetOffset = 200;
            TCFOffsetText.text = TargetOffset.ToString();
        }
        else
        {
            //if auto pilot activate
            if (Activate) AUTOpilotActivate();
        }
    }

    public void TCFoffsetSet(int i)
    {
        if (i == 0)
        {
            TargetOffset -= 50;
        }
        else 
        {
            TargetOffset += 50;
        }
        TargetOffset = Mathf.Clamp(TargetOffset, 150, 500);
        TCFOffsetText.text = TargetOffset.ToString();
    }

    public void TCFoffsetSet(string i)
    {
        TargetOffset = float.Parse(i);
        TargetOffset = Mathf.Clamp(TargetOffset, 150, 500);
        TCFOffsetText.text = TargetOffset.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the forward position ignoring the object's pitch
        Vector3 forwardDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 forwardPosition = transform.position + forwardDirection * forwardDistance + Vector3.up*500f;

        // Calculate the downward direction (global down)
        Vector3 downwardDirection = Vector3.down;

        if (TCF)
        {
            TargetAltitude = 0f;
            // Perform the raycast downward from the forward position
            RaycastHit hit;
            if (Physics.Raycast(forwardPosition, downwardDirection, out hit, 1500f, layerMask))
            {
                float height = hit.point.y * 3.281f;
                TargetAltitude = height + TargetOffset;
                Debug.DrawRay(forwardPosition, downwardDirection * hit.distance, Color.red);
            }
            else
            {
                Debug.DrawRay(forwardPosition, downwardDirection * 1500f, Color.green);
                Debug.Log("Raycast did not hit.");
            }
        }

        ver = 0;
        hor = 0;


        climbRate = rb.velocity.y * 3.28084f;
        currentPitch = ConvertAngle(transform.localEulerAngles.x);
        currentRoll = ConvertAngle(transform.localEulerAngles.z);
        currentYaw = transform.eulerAngles.y;
        float p = currentPitch < 0 ? -currentPitch : currentPitch;
        float tp = TargetPitch < 0 ? -TargetPitch : TargetPitch;

        float r = currentRoll < 0 ? -currentRoll : currentRoll;
        float cp = r > 90 ? (currentPitch < 0 ? (-90 - (90 + currentPitch)) : 90 + (90 - currentPitch)) : currentPitch;
        currentPitch = cp;
        currentSpeed =  rb.velocity.magnitude * 1.944f;
        currentAltitude = transform.position.y * 3.281f;
        currentHeading = ConvertAngle(transform.eulerAngles.y);
        RadioAltitude = (currentAltitude - (TargetAltitude-TargetOffset));

        if (Activate)
        {
            
            bool disableRoll = false;
            bool disablePitch = false;
            bool overideContorls = false;

            if (TCF)
            {
                TargetPitch = (currentAltitude - TargetAltitude) / 20f;
                TargetPitch = Mathf.Clamp(TargetPitch, -50, 10);
                float pt = Mathf.Pow(TargetPitch < 0 ? -TargetPitch : TargetPitch, TargetPitch < 0 ? 10 : 1f);
                TargetPitch = TargetPitch < 0 ? -pt : pt;
                TargetPitch = Mathf.Clamp(TargetPitch, -50, 10);

                float n = climbRate < 0 ? -1 : 1;
                float c = climbRate;// * n;
                float dis = (RadioAltitude - TargetOffset);
                dis = dis < 0 ? -dis : dis;
                ControlClimb = false;
                if ( ((dis < 200 && c < -4) ||  c < -dis/10))  
                {
                    if (TargetPitch > 0)
                    {
                        TargetPitch = TargetPitch * n;
                        ControlClimb = true;
                    }
                }
                else
                if(c > 20)
                {
                    if(TargetPitch < 0)
                    {
                        TargetPitch = TargetPitch * n;
                        ControlClimb = true;
                    }
                }

            }
            else
            {
                TargetPitch = (currentAltitude - TargetAltitude) / 50f;
                TargetPitch = Mathf.Clamp(TargetPitch, -10, 10);
                float pt = Mathf.Pow(TargetPitch < 0 ? -TargetPitch : TargetPitch, TargetPitch < 0 ? 3 : 1f);
                TargetPitch = TargetPitch < 0 ? -pt : pt;
                TargetPitch = Mathf.Clamp(TargetPitch, -15f, 15f);

                //float n = climbRate < 0 ? -1 : 1;
                //float c = climbRate;// * n;
                //float dis = (currentAltitude - TargetAltitude);
                //dis = dis < 0 ? -dis : dis;
                //ControlClimb = false;
                //if (((dis < 200 && c < -4) || c < -dis / 10))
                //{
                //    if (TargetPitch > 0)
                //    {
                //        TargetPitch = TargetPitch * n;
                //        ControlClimb = true;
                //    }
                //}
                //else
                //if (c > 20)
                //{
                //    if (TargetPitch < 0)
                //    {
                //        TargetPitch = TargetPitch * n;
                //        ControlClimb = true;
                //    }
                //}

            }
            

            TargetRoll = (currentHeading - TargetHeading) ;
            TargetRoll = Mathf.Clamp(TargetRoll, -20, 20);
            float rl = Mathf.Pow(TargetRoll < 0 ? -TargetRoll : TargetRoll, 1);
            TargetRoll = TargetRoll < 0 ? -rl : rl;
             

            //hor = currentRoll > TargetRoll ? 1 : -1;

            hor = (currentRoll - TargetRoll) / 180;
            float h = Mathf.Pow(hor < 0 ? -hor : hor, 0.27f);
            hor = hor < 0 ? -h : h;
            ///pitch -> disable roll for certain angle
            hor = ((p > 60) || tp > 80) && disableRoll ? 0 : hor;

            ver = -(currentPitch - TargetPitch) / 45;
            float v = Mathf.Pow(ver < 0 ? -ver : ver, 0.27f);
            ver = ver < 0 ? -v : v;

            thro = currentSpeed<TargetSpeed?1:0;
            float d = currentSpeed - TargetSpeed;
            d = d<0 ? -d : d;
            //float th = Mathf.Pow(thro < 0 ? -thro : thro, 0.1f);
            //thro = th < 0 ? 0 : th;

            controller.input.rawPitchInput = ver;
            controller.input.rawRollInput = hor;
            controller.input.rawYawInput = -TargetRoll / 20;
            controller.input.rawThrottleInput = thro;
            controller.input.brakeLeverHeld = thro==0 && d>20f?true:false;
        }
        else
        {
            //controller.inputType = originalType;
        }
    }

    public float ConvertAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
