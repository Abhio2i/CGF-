#region Script Info
//output: The Plane Recorded data is passed through the required scene.
#endregion
using UnityEngine;
using System.Collections.Generic;
public class PointInTime
{
    #region Global Perimeters
    public Vector3 pos;
	public Vector3 rot;
	public Vector3 killAssesment;
	public Vector3 waypointAssesment;
	public Vector3 timeAfterTakeOff;
	public float speed;
	public float fuel;
	public float gForce;
	public static List<Vector3> recordPos = new List<Vector3>();
	public static List<Vector3> recordTimeAfterTakeOff= new List<Vector3>();
	public static List<Vector3> recordRot = new List<Vector3>();
	public static List<float> recordSpeed = new List<float>();
	public static List<float> recordFuel = new List<float>();
	public static List<float> recordGForce = new List<float>();
	
	public static List<Vector3> recordKillAssesment = new List<Vector3>();
	public static List<Vector3> recordWaypointAssesment = new List<Vector3>();

	public static List<string> recordLogsMessages = new List<string>();
	#endregion

	#region Global Functions
	public void AddValToList(Vector3 _pos, Vector3 _rot, float _speed, float _fuel, Vector3 _killAssesment, float _Gforce,
		Vector3 TimeAfterTakeOff,Vector3 waypointsSpecs,List<string> messages)
	{
		pos = _pos;
		rot = _rot;
		speed = _speed;
		fuel = _fuel;
		killAssesment= _killAssesment;
		waypointAssesment = waypointsSpecs;
		gForce = _Gforce;
		timeAfterTakeOff = TimeAfterTakeOff;

		recordPos.Add(pos);
		recordRot.Add(rot);
		recordSpeed.Add(speed);
		recordFuel.Add(fuel);

		recordGForce.Add(gForce);
		recordKillAssesment.Add(killAssesment);
		recordWaypointAssesment.Add(waypointAssesment);
		recordTimeAfterTakeOff.Add(TimeAfterTakeOff);

		recordLogsMessages = messages;
	}
	public List<Vector3> Positions(){ return recordPos; }
	public List<Vector3> Rotations(){ return recordRot; }
	public List<Vector3> KillAssesment() { return recordKillAssesment; }
	public List<float> GetSpeed() { return recordSpeed; }
	public List<float> GetFuel() { return recordFuel; }
	public List<float> GetGforce() { return recordGForce; }

	public List<Vector3> GetTimeAfterTakeOff() { return recordTimeAfterTakeOff; }
	public List<Vector3> GetWaypointsAssesment() { return recordWaypointAssesment; }

	public List<string> GetMessages()
    {
		return recordLogsMessages;
    }

	
	#endregion
}
