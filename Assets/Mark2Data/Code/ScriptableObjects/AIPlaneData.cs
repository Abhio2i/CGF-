using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/AI_PlaneSpawning")]

public class AIPlaneData : ScriptableObject
{
    public string _name, type, model, country, pilot, skill, count;
    public int unit;
    public int callSign1, callSign2;
    public int formationType;
    public List<float> spawnPositionsAtY, spawnPositionsAtZ, spawnPositionsAtX;
    public bool radio, hiddenOnMap, hiddenOnPlanner, hiddenOnRadar, playerChase;
    public string callSign_code;
    //public Vector3 aroundPoint;
    public CinemachineSmoothPath track;
    public Vector3[] wayPointsPos;
    private readonly float maxDistanceBTWayPoints = 1000f, setlatAndLong = 40000, setalt = 3000f;
    private bool isRandom, side1, side2, side3;
    int numCheckpoints;
    private int call;

    public void SetWaypoints()
    {
        call = 0;
        int countCall = int.Parse(count);
        numCheckpoints = (int)track.MaxUnit(CinemachinePathBase.PositionUnits.PathUnits);
        wayPointsPos = new Vector3[numCheckpoints* countCall];
        for(int i = 0; i < countCall; i++)
        {
            ResetWaypoints();
            MapGenerating();
            call++;
        }
        
    }
    private void ResetWaypoints()
    {
        for (int i = 0; i < numCheckpoints; i++)
        {
            track.m_Waypoints[i].position = Vector3.zero;
            wayPointsPos[i] = Vector3.zero;

        }
    }

    private void MapGenerating()
    {
        Vector3 newpos;
        float x, y, z;
        int i;
        for (i = 0; i < numCheckpoints; i++)
        {
            side1 = side2 = side3 = false;
            isRandom = true;
            newpos = Vector3.zero;
            x = track.m_Waypoints[i].position.x;
            y = track.m_Waypoints[i].position.y;
            z = track.m_Waypoints[i].position.z;
            while (isRandom)
            {
                if (Mathf.Abs(x - newpos.x) > maxDistanceBTWayPoints)
                {
                    side1 = true;
                }
                else
                {
                    newpos.x = RandomLatAndLong();
                }

                if (Mathf.Abs(y - newpos.y) > 15)
                {
                    side2 = true;
                }
                else
                {
                    newpos.y = RandomAlt();
                }

                if (Mathf.Abs(z - newpos.z) > maxDistanceBTWayPoints)
                {
                    side3 = true;
                }
                else
                {
                    newpos.z = RandomLatAndLong();
                }

                if (side1 && side2 && side3)
                {
                    isRandom = false;
                    track.m_Waypoints[i].position = newpos;
                    wayPointsPos[i+ (call * numCheckpoints)] = track.m_Waypoints[i].position;
                }

            }
        }
        

    }
    float RandomLatAndLong()
    {
        return Random.Range(-setlatAndLong, setlatAndLong);
    }
    float RandomAlt()
    {
        return Random.Range(500, setalt);
    }
}
