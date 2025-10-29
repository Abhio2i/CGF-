using EW.Flare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Jammer;
using static RadarScreenUpdate_new;

public class Jammer : MonoBehaviour
{
    public bool JammerActive;
    private EWRadar ewradar;
    public bool enemy;
    public Toggle jam;
    public enum JamMode { SpotJamming, SweepJamming, DRFMjamming,Barring };
    public enum JamFrequency { _8ghz, _9ghz, _10ghz, _11ghz, _12ghz };
    public enum JamPower { _11W, _12W, _13W, _14W, _16W };

    public JamMode jamMode;
    public JamFrequency jamFrequency;
    public JamPower jamPower;
    public float SweepTime = 2f;
    public Transform freqNobe;
    public Transform jamModeNobe;

    [Header("-------------------------------")]
    [Header("-----------RADAR-------------")]
    [Header("Radar estimitted value)")]
    [Header("Radar Transmitted Power (watt)")]
    public float Pt = 1000;
    [Header("Radar Receiver Gain (db)")]
    public float Gr = 1f;
    [Header("Radar Transmitted Frequency (MHz)")]
    public float F = 4000;
    [Header("Distance (M) between Radar and Target")]
    public float D = 60f;
    [Header("Target cross section area in m square")]
    public float σ = 5;

    [Header("Signal Receive by Radar (db)")]
    [ReadOnlyWhenPlaying]
    public float S = 0;
    [Header("---------------------------------")]
    [Header("-----------JAMMER-------------")]
    [Header("Jammer Transmitted Power (watt)")]
    public float Pj = 1000;
    [Header("Jammer Receiver Gain (db)")]
    public float Gj = 1f;
    [Header("Jammer Transmitted Frequency (MHz)")]
    public float Fj = 4000;
    [Header("Distance (m) between Jammer and Target")]
    public float Dj = 60f;
    [Header("Target Receiveing jamming signal Gain (db)")]
    public float Grj = 1f;
    [Header("jamming Signal Receive gain by Radar (db)")]
    [ReadOnlyWhenPlaying]
    public float J = 0;
    [Header("------------------------------")]
    [Header("-----------JAMMER SIGNAL RATIO db-------------")]
    [ReadOnlyWhenPlaying]
    public float jS = 0;

    void Start()
    {
        ewradar = GetComponent<EWRadar>();
    }
    private void OnEnable()
    {
        var sub = enemy ? "Enemy" : "";
        if (PlayerPrefs.GetInt(sub + "Jammer") ==+ 1) { JammerActive = true; if (jam) jam.isOn = true; }
    }

    // Update is called once per frame
    void Update()
    {

        var PT = 10 * Mathf.Log(Pt / 0.001f);//watt to dBm
        S = PT + (2 * Gr) - 103 - (20 * Mathf.Log(F)) - (40 * Mathf.Log(D)) + (10 * Mathf.Log(σ));

        //Jammer Calculation
        var PJ = 10 * Mathf.Log(Pj / 0.001f);//watt to dBm
        J = PJ + Gj - 32 - (20 * Mathf.Log(Fj)) - (20 * Mathf.Log(Dj)) + Grj;

        jS = J / S;
    }

    public void Activate(bool i)
    {
        JammerActive = i;
    }

    public void JenFreSet(int i)
    {

        if (i >= 0)
        {
            if (i == 0)
            {
                if ((int)jamFrequency - 1 >= 0)
                {
                    jamFrequency = (JamFrequency)((int)jamFrequency) - 1;
                }
            }
            else
            {
                if ((int)jamFrequency + 1 < 5)
                {
                    jamFrequency = (JamFrequency)((int)jamFrequency) + 1;
                }
            }
        }
        if(freqNobe != null)
        freqNobe.rotation = Quaternion.Euler(0, 0, ((int)jamFrequency) * -58.45f);

    }

    public void JemModeSet(int i)
    {

        if (i >= 0)
        {
            if (i == 0)
            {
                if ((int)jamMode - 1 >= 0)
                {
                    jamMode = (JamMode)((int)jamMode) - 1;
                }
            }
            else
            {
                if ((int)jamMode + 1 < 4)
                {
                    jamMode = (JamMode)((int)jamMode) + 1;
                }
            }
        }
        if(jamModeNobe)
        jamModeNobe.rotation = Quaternion.Euler(0, 0, ((int)jamMode) * -52.45f);
        if(jamMode == JamMode.SweepJamming&&!Sweeping)
        {
            Sweeping = true;
            Invoke("SweepJamming", SweepTime);
        }

    }

    public bool SweepLeft = true;
    public bool Sweeping = false;
    public void SweepJamming()
    {   

        if((int)jamFrequency == 0)
        {
            SweepLeft = false;
        }else
        if((int)jamFrequency == 4)
        {
            SweepLeft = true;
        }

        int i = SweepLeft ? 0 : 1;

        JenFreSet(i);

        if (jamMode==JamMode.SweepJamming && JammerActive)
        {
            Invoke("SweepJamming", SweepTime);
        }
        else
        {
            Sweeping = false;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (JammerActive)
        {
            if (jamMode == JamMode.SweepJamming && !Sweeping)
            {
                Sweeping = true;
                Invoke("SweepJamming", SweepTime);
            }
            ewradar.JamObjects = new Dictionary<GameObject, float>();

            foreach (GameObject aircraft in ewradar.objectsDetected)
            {
                if (aircraft != null)
                {
                    if (Vector3.Angle(aircraft.transform.position - transform.position, transform.forward) < 60)
                    {
                        float distance = Vector3.Distance(aircraft.transform.position, transform.root.position);
                        //Jammer Calculation
                        var PJ = 10 * Mathf.Log10(Pj / 0.001f);//watt to dBm
                        //J = PJ + Gj - 32 - (20 * Mathf.Log(Fj* 1000000000)) - (20 * Mathf.Log(distance)) + Grj;
                        J = PJ + Gj + (20 * Mathf.Log10(distance));
                        if (aircraft.GetComponentInChildren<UttamRadar>())
                        {
                            aircraft.GetComponentInChildren<UttamRadar>().JamSignal(jamMode, jamFrequency, jamPower, gameObject);
                        }
                        
                        if (aircraft.TryGetComponent<CombineUttam>(out CombineUttam comnineUttam))
                        {
                            float JS = comnineUttam.Radar.JamSignal(jamMode, (int)jamFrequency+8, (int)jamPower, gameObject, J);
                            if (JS>1)
                            {
                                ewradar.JamObjects.Add(aircraft,JS);
                            }
                        }



                        
                    }
                }
            }
        }
        else
        {
            if(ewradar.JamObjects.Count > 0)
            {
                ewradar.JamObjects = new Dictionary<GameObject, float>();
            }
            
        }
    }
    public void JamActivate(bool t)
    {
        JammerActive = t;
    }
    public void JamModeSet(int i)
    {
        jamMode = (JamMode)i;
    }
    public void JamFrequencySet(int i)
    {
        jamFrequency = (JamFrequency)i;
    }
    public void JamPowerSet(int i)
    {
        jamPower = (JamPower)i;
    }
}
