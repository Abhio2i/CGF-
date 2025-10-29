using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationCreate : MonoBehaviour
{

    public enum Formation {
        Trail,
        FingerTipRight,
        EchelonRight,
        EchelonLeft,
        Diamond,
        Spread,
        Fluid
    }
    public Formation formation = Formation.Trail;
    public bool upate = false;
    public float Smooth = 1;
    public Vector3 offset = Vector3.zero;
    public List<Transform> points = new List<Transform>();
    public List<Transform> FollowPoints = new List<Transform>();
    public List<Vector3> VPoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (upate)
        {
            upate = false;
            switch(formation)
            {
                case Formation.Trail:
                    VPoints[0] = points[0].localPosition + offset;
                    VPoints[1] = points[1].localPosition + offset;
                    VPoints[2] = points[2].localPosition + offset;
                    break;
                case Formation.FingerTipRight:
                    VPoints[0] = points[4].localPosition + offset;
                    VPoints[1] = points[3].localPosition + offset;
                    VPoints[2] = points[6].localPosition + offset;
                    break;
                case Formation.EchelonRight:
                    VPoints[0] = points[3].localPosition + offset;
                    VPoints[1] = points[6].localPosition + offset;
                    VPoints[2] = points[7].localPosition + offset;
                    break;
                case Formation.EchelonLeft:
                    VPoints[0] = points[4].localPosition + offset;
                    VPoints[1] = points[5].localPosition + offset;
                    VPoints[2] = points[8].localPosition + offset;
                    break;
                case Formation.Diamond:
                    VPoints[0] = points[4].localPosition + offset;
                    VPoints[1] = points[3].localPosition + offset;
                    VPoints[2] = points[1].localPosition + offset;
                    break;
                case Formation.Spread:
                    VPoints[0] = points[11].localPosition + offset;
                    VPoints[1] = points[10].localPosition + offset;
                    VPoints[2] = points[9].localPosition + offset;
                    break;
                case Formation.Fluid:
                    VPoints[0] = points[11].localPosition + offset;
                    VPoints[1] = points[3].localPosition + offset;
                    VPoints[2] = points[12].localPosition + offset;
                    break;
            }

        }

        FollowPoints[0].localPosition = Vector3.Lerp(FollowPoints[0].localPosition, VPoints[0], Time.deltaTime * Smooth);
        FollowPoints[1].localPosition = Vector3.Lerp(FollowPoints[1].localPosition, VPoints[1], Time.deltaTime * Smooth);
        FollowPoints[2].localPosition = Vector3.Lerp(FollowPoints[2].localPosition, VPoints[2], Time.deltaTime * Smooth);
    }


    public void setFormation(int i)
    {
        formation = (Formation)i;
        upate = true;
    }
}
