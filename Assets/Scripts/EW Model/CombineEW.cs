using AirPlane.Radar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using Vazgriz.Plane;
using Random = UnityEngine.Random;

[Serializable]
public class AIEWLog
{
    public List<Vector3> detect = new List<Vector3>();
    //public List<bool> locks = new List<bool>();
}
public class CombineEW : MonoBehaviour
{
    public string Log;
    private EWRadar ew;
    private AIController ai;
    string targetTag;
    void Start()
    {
        targetTag = GetComponent<CombineUttam>().targetTag;
        ai = GetComponent<AIController>();
        ew = GetComponentInChildren<EWRadar>();
        Invoke(nameof(EWActivate), 5f);
    }
    void EWActivate()
    {
        ew.autoChaffFire = true;
        ew.autoFlareFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool dodge = false;
        foreach (var i in ew.objectsDetected)
        {
            if (i != null)
                if (i.tag.Contains(targetTag) && !i.tag.Contains("Missle"))
                {
                    if (Vector3.Angle(i.transform.position - transform.position, transform.forward) < 15)
                    {
                        if (ai.LeftRight == 0) { ai.LeftRight = Random.Range(0, 2); }
                        dodge = true;
                    }
                }
        }
        ai.dodge = dodge;
        //ai.missile = transform;
        if (!dodge) { ai.LeftRight = 0; }
        if (ai.targetPos == null)
        {
            var max = 0f;
            GameObject loc = null;
            foreach (var i in ew.objectsDetected)
            {
                if(i != null)
                if (i.tag.Contains(targetTag) && !i.tag.Contains("Missle"))
                {
                    float d = Vector3.Distance(transform.position, i.transform.position);
                    if (d < max || max == 0f)
                    {
                        max = d;
                        loc = i;
                    }
                }
            }
            if(loc != null)
            {
                ai.targetPos = loc.transform;
            }
        }

        AIEWLog log = new AIEWLog();
        foreach (var i in ew.objectsDetected)
        {

            if (i != null)
            {
                log.detect.Add(i.transform.position);
            }

        }
        Log = JsonUtility.ToJson(log);
    }
}
