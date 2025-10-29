using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static RadarScreenUpdate_new;

public class CIT : MonoBehaviour
{
    public enum CITModes
    {
        None,
        M1,
        M2,
        M3,
        MC,
        MS
    }
    public CITModes CitModes = CITModes.None;
    public Transform CITModeNobe;
    public float CollisonWarnAlltitude = 300;
    public float CollisonWarnRange = 1000;
    public float CollisonAngle = 60;
    public float CollisonScanRange = 1500;
    public GameObject CollisonAlert;
    public Specification OwnSpecification;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI modeValue;
    public List<Transform> Aircrafts = new List<Transform>();
    public List<Specification> Specifications = new List<Specification>();
    public blink BlinkAlert;
    public LoadSystemAndSpawnPlanes loadPlanes;
    public UttamRadar radar;
    // Start is called before the first frame update
    void Start()
    {
        BlinkAlert = CollisonAlert.GetComponent<blink>();
        OwnSpecification = GetComponent<Specification>();
        Invoke("getAircraftData", 3);
        modeText.text = "";
        modeValue.text = "";
    }

    public void getAircraftData()
    {
        foreach (GameObject ally in loadPlanes.AllyPlanes)
        {
            Aircrafts.Add(ally.transform);
            Specifications.Add(ally.GetComponent<Specification>());
        }
        foreach (GameObject Adversary in loadPlanes.AdversaryPlanes)
        {
            Aircrafts.Add(Adversary.transform);
            Specifications.Add(Adversary.GetComponent<Specification>());
        }
        foreach (GameObject Neutral in loadPlanes.NeutralPlanes)
        {
            Aircrafts.Add(Neutral.transform);
            Specifications.Add(Neutral.GetComponent<Specification>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Transform craft in  Aircrafts)
        {
            if (craft != null&&craft.gameObject.activeSelf && radar.SelectedTarget != null && radar.SelectedTarget == craft.gameObject)
            {
                if (CitModes == CITModes.None)
                {
                    CollisonAlert.SetActive(false);
                }
                else
                if (CitModes == CITModes.M1)
                {
                    modeValue.text = Specifications[i].M1.ToString();
                }
                else
                if (CitModes == CITModes.M2)
                {
                    modeValue.text = Specifications[i].M2.ToString();
                }
                else
                if (CitModes == CITModes.M3)
                {
                    modeValue.text = Specifications[i].M3.ToString();
                }
                else
                if (CitModes == CITModes.MC)
                {
                    modeValue.text = Specifications[i].entityInfo.Altitude.ToString();
                }
                else
                if (CitModes == CITModes.MS)
                {
                    modeValue.text = Specifications[i].M1 == OwnSpecification.M1 && Specifications[i].M2 == OwnSpecification.M2 && Specifications[i].M3 == OwnSpecification.M3 ? "OK" : "Not OK";
                }

                /*if (CitModes == CITModes.M1)
                {
                    float Distance = Vector3.Distance(craft.position, gameObject.transform.position);
                    if (CollisonScanRange > Distance)
                    {
                        float AlltiudeDistance = craft.position.y - gameObject.transform.position.y;
                        AlltiudeDistance = AlltiudeDistance < 0 ? -AlltiudeDistance : AlltiudeDistance;
                        if (AlltiudeDistance < CollisonWarnAlltitude && Distance < CollisonWarnRange)
                        {
                            CollisonAlert.SetActive(true);
                            BlinkAlert.time = 0.5f;
                        }
                        else
                        {
                            CollisonAlert.SetActive(false);
                        }
                    }
                }else
                if (CitModes == CITModes.M2)
                {
                    float Distance = Vector3.Distance(craft.position, gameObject.transform.position);
                    if (CollisonScanRange > Distance)
                    {
                        float AlltiudeDistance = craft.position.y - gameObject.transform.position.y;
                        AlltiudeDistance = AlltiudeDistance < 0 ? -AlltiudeDistance : AlltiudeDistance;
                        Vector3 c = craft.position;
                        c.y = 0;
                        Vector3 p = gameObject.transform.position;
                        p.y = 0;
                        Vector3 dir = c - p;
                        Vector3 dir2 = p - c;
                        float angle = Vector3.Angle(dir, transform.forward);
                        float angle2 = Vector3.Angle(dir2, craft.forward);

                        if (AlltiudeDistance < CollisonWarnAlltitude && Distance < CollisonWarnRange && (angle < CollisonAngle || angle2 < CollisonAngle))
                        {
                            CollisonAlert.SetActive(true);
                            BlinkAlert.time = 0.1f;
                        }
                        else
                        {
                            CollisonAlert.SetActive(false);
                        }
                    }
                }
                else
                if (CitModes == CITModes.M3)
                {
                    float Distance = Vector3.Distance(craft.position, gameObject.transform.position);
                    if (CollisonScanRange > Distance)
                    {
                        float AlltiudeDistance = craft.position.y - gameObject.transform.position.y;
                        AlltiudeDistance = AlltiudeDistance < 0 ? -AlltiudeDistance : AlltiudeDistance;
                        Vector3 c = craft.position;
                        c.y = 0;
                        Vector3 p = gameObject.transform.position;
                        p.y = 0;
                        Vector3 dir = c - p;
                        Vector3 dir2 = p - c;
                        float angle = Vector3.Angle(dir, transform.forward);
                        float angle2 = Vector3.Angle(dir2, craft.forward);

                        if (AlltiudeDistance < CollisonWarnAlltitude && Distance < CollisonWarnRange && OwnSpecification.iff != Specifications[i].iff && (angle < CollisonAngle || angle2 < CollisonAngle))
                        {
                            CollisonAlert.SetActive(true);
                            BlinkAlert.time = 0.1f;
                        }
                        else
                        {
                            CollisonAlert.SetActive(false);
                        }
                    }
                }*/

            }
            i++;
        }


    }


    public void CITModeSet(int i)
    {
         
        if (i >= 0)
        {
            if (i == 0)
            {
                if ((int)CitModes - 1 >= 0)
                {
                    CitModes = (CITModes)((int)CitModes) - 1;
                }
            }
            else
            {
                if ((int)CitModes + 1 < 6)
                {
                    CitModes = (CITModes)((int)CitModes) + 1;
                }
            }
        }
        CITModeNobe.rotation = Quaternion.Euler(0, 0, ((int)CitModes) * -45f);
        if (CitModes == CITModes.None)
        {
            modeText.text = "";
            modeValue.text = "";
        }
        else
        {
            modeText.text = CitModes.ToString();
        }

    }

}


