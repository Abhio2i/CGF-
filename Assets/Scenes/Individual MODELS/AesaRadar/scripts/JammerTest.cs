
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JammerTest : MonoBehaviour
{
    public bool JammerActive;
    public GameObject aircraft;
    public bool enemy;
    public Toggle jam;
    

    public Jammer.JamMode jamMode;
    public Jammer.JamFrequency jamFrequency;
    public Jammer.JamPower jamPower;
    void Start()
    {
        aircraft = GameObject.Find("TejasLcaMain");
    }
    private void OnEnable()
    {

        //var sub = enemy ? "Enemy" : "";
        //if (PlayerPrefs.GetInt(sub + "Jammer") == +1) { JammerActive = true; if (jam) jam.isOn = true; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (JammerActive)
        {

                if (aircraft != null)
                {
                    if (Vector3.Angle(aircraft.transform.position - transform.position, transform.forward) < 60)
                    {
                        if (aircraft.GetComponentInChildren<UttamRadar>())
                            aircraft.GetComponentInChildren<UttamRadar>().JamSignal(jamMode, jamFrequency, jamPower, gameObject);
                    }
                }

        }
    }
    public void JamActivate(bool t)
    {
        JammerActive = t;
    }
    public void JamModeSet(int i)
    {
        jamMode = (Jammer.JamMode)i;
    }
    public void JamFrequencySet(int i)
    {
        jamFrequency = (Jammer.JamFrequency)i;
    }
    public void JamPowerSet(int i)
    {
        jamPower = (Jammer.JamPower)i;
    }
}
