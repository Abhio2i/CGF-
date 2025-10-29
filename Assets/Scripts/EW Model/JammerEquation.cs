using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammerEquation : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var PT = 10 * Mathf.Log(Pt / 0.001f);//watt to dBm
        S = PT + (2 * Gr) - 103 - (20 * Mathf.Log(F)) - (40 * Mathf.Log(D)) + (10 * Mathf.Log(σ));

        var PJ = 10 * Mathf.Log(Pj / 0.001f);//watt to dBm
        J = PJ + Gj - 32 - (20 * Mathf.Log(Fj)) - (20 * Mathf.Log(Dj)) + Grj;

        jS = J/S;
    }
}
