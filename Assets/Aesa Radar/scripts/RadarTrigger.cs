using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTrigger : MonoBehaviour
{
    public UttamRadar Radar;
    private float clock = 0.1f;
    public bool triggerUpdate = false;

    private void Start()
    {
        StartCoroutine(triggerUpdateTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        Radar.Trigger(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if(triggerUpdate)
        Radar.Trigger(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Radar.TriggerExit(other);
    }

    IEnumerator triggerUpdateTimer()
    {
        triggerUpdate = true;
        yield return new WaitForSeconds(0.02f);
        triggerUpdate = false;
        yield return new WaitForSeconds(clock);
        StartCoroutine(triggerUpdateTimer());
    }


}
