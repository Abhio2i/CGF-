using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWequation : MonoBehaviour
{
    [Header("TARGET RADAR ESTIMATED VALUES")]
    [Header("Power (watt)")]
    [Tooltip("Transmitted Output Power by Radar")]
    public float Pt = 1000f;
    [Header("Gain (dbm)")]
    [Tooltip("Transmitted Antenna Gain by Radar")]
    public float Gt = 2f;
    [Header("Frequency (MHz)")]
    [Tooltip("Transmitted Frequency in (MHz) by Radar")]
    public float f = 2f;
    [Header("EW RADAR CONFIG VALUES")]
    [Header("Recieve Gain (dB)")]
    [Tooltip("Receiving Antenna Gain")]
    public float Gr = 2f;
    [Header("ReceivePower (dbm) and Threshold")]
    [Range(-1f,-118.9f)]
    public double Pr = 1000f;
    //[Header("Set Distance (km)")]
    //[Tooltip("Distance in km from target")]
    //public float d = 2f;

    [Header("ResolveDistance (km)")]
    [Tooltip("Distance in km from target")]
    [ReadOnlyWhenPlaying]
    public float D = 2f;
    [ReadOnlyWhenPlaying]
    public float PT = 2f;
    //[Header("ResolveReceivePower (dbm)")]
    //[ReadOnlyWhenPlaying]
    //public double PR = 1000f;
    [Header("Formula Range = exp( PT + GT - 32.4 - 20log(f) + Gr - Pr / 20 )")]
    [ReadOnlyWhenPlaying]
    public string formula = "exp( PT + GT - 32.4 - 20log(f) + Gr - Pr / 20 )";

    [Header("external update")]
    public bool AutoUpdate = false;
    void Start()
    {
        
    }

    public float getRange()
    {
        PT = 10 * Mathf.Log(Pt / 0.001f);//watt to dBm
        //PR = PT + Gt - 32.4 - (20 * Mathf.Log(f)) - (20 * Mathf.Log(d)) + Gr;
        D = Mathf.Exp((float)(PT + Gt - 32.4 - (20 * Mathf.Log(f)) + Gr - Pr) / 20f);
        return D;
    }
    //Update is called once per frame
    void Update()
    {
        if (AutoUpdate)
        {
            PT = 10 * Mathf.Log(Pt / 0.001f);//watt to dBm
            //PR = PT + Gt - 32.4 - (20 * Mathf.Log(f)) - (20 * Mathf.Log(d)) + Gr;
            D = Mathf.Exp((float)(PT + Gt - 32.4 - (20 * Mathf.Log(f)) + Gr - Pr) / 20f);
            Debug.Log("true");
        }
       
    }
}
