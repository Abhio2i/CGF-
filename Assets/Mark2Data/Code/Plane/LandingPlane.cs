#region script Info
//input: current plane data like speed, position etc.
//output: based on the input the plane is going to land on ground.
#endregion
using System.Collections.Generic;
using UnityEngine;

public class LandingPlane : MonoBehaviour
{
    #region Global parameters
    [SerializeField] bool AImode;
    [SerializeField] List<Transform> waypoints;
    [SerializeField] GameObject mainBody;
    
    public bool stop=false;
    public float speed;
    public Transform runwayPoint;
    #endregion

    #region Local Parameters
    private float _speed;
    private bool _toggle;
    private int indexOfWayPont=0;
    #endregion

    #region local functions
    private void Start()
    {
        _speed = speed;
    }
    private void FixedUpdate()
    {
        if(indexOfWayPont==waypoints.Count)
        {
            if(Vector3.Distance(mainBody.transform.position,runwayPoint.position)<30f)
            {
                mainBody.transform.rotation = Quaternion.Slerp(mainBody.transform.rotation, Quaternion.Euler(new Vector3(0f, 0, 0)), 0.85f * Time.deltaTime);
                _speed = Mathf.Lerp(_speed, 0f, 0.15f * Time.deltaTime);
            }
            else
            {
                mainBody.transform.rotation = Quaternion.Slerp(mainBody.transform.rotation, Quaternion.Euler(new Vector3(-5f, 0, 0)), 0.45f * Time.deltaTime);
                _speed = Mathf.Lerp(_speed, 0.8f * speed, 0.55f * Time.deltaTime);
            }
            mainBody.transform.position = Vector3.MoveTowards(mainBody.transform.position, runwayPoint.transform.position + new Vector3(0, 0, 300f), _speed * Time.deltaTime);

            return;
        }

        RotateTowardsRunway(waypoints[indexOfWayPont].position);

        if(Vector3.Distance(waypoints[indexOfWayPont].position, mainBody.transform.position)<200f)
        {
            mainBody.transform.position += 0.80f*_speed * mainBody.transform.forward * Time.deltaTime;
        }
        else
        {
            mainBody.transform.position += speed* mainBody.transform.forward *  Time.deltaTime;
        }

        if (Vector3.Distance(waypoints[indexOfWayPont].position, mainBody.transform.position) < 50f)
        {
            indexOfWayPont++;
        }
    }
    void RotateTowardsRunway(Vector3 wayPoint)
    {
        var direction=mainBody.transform.position - wayPoint;
        Quaternion _rot=Quaternion.LookRotation(-direction);
        mainBody.transform.rotation=Quaternion.Slerp(mainBody.transform.rotation,_rot,0.45f*Time.deltaTime);
    }
    #endregion
}
