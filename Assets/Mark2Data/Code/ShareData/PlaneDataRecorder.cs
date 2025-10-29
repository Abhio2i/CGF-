#region Script Info
//output: Its stores total Flight Time plane roll,pitch,yaw, speed, and positions.
#endregion
using System.Collections.Generic;
using System.Collections;
using UnityEngine; 
using Data.Plane;
using System;
using Assets.Scripts.Utility;

public class PlaneDataRecorder : MonoBehaviour
{
    #region Global Parameters
    //public GameObject obj;
	public PlaneData planeData;
	[SerializeField]Clock clock;
	#endregion

	#region Local Perimeters
	[HideInInspector] public Vector3 currentTime;
    List<Vector3> positions;
	List<Vector3> rotations;
	List<Vector3> killAssesment;
	List<Vector3> takeOffTime;
	List<Vector3> waypointSpecs;

	public List<string> messages;

	List<float> speeds;
	List<float> fuels;

	List<float> GForce;

	int waypointDist, wayPointRemaining, baseDist;

	int missileCount, ammoCount, enemyDownCount;

	PointInTime pointInTime;
	private bool isRecording = false;
    #endregion

    #region Local Functions
    void Start()
	{
		currentTime = new Vector3();
		pointInTime = new PointInTime();
		positions = new List<Vector3>();
		rotations = new List<Vector3>();
		killAssesment = new List<Vector3>();
		takeOffTime = new List<Vector3>();
		waypointSpecs = new List<Vector3>();
		speeds = new List<float>();
		fuels = new List<float>();
		GForce = new List<float>();
		Invoke(nameof(StartRecord),5f);
	}

    private void StartRecord()
    {
        isRecording = true;
	
    }
	float frames = 0f;
	void FixedUpdate()
	{
		if (planeData.body == null) return;
		if (isRecording)
		{
			Record();
			AddPositionsToLog();
			AddRotationToLog();
			frames += Time.fixedDeltaTime;
			PlayerPrefs.SetFloat("frames", (int)frames / Time.fixedDeltaTime);
		}
	}
    void Record()
	{
		positions.Insert(0, new Vector3(planeData.latLong.x,planeData.latLong.y,planeData.body.transform.position.y));
		rotations.Insert(0, planeData.planRotation);
		speeds.Insert(0,planeData.bodySpeed);
		fuels.Insert(0,planeData.planeFuel);

		enemyDownCount = PlayerPrefs.GetInt("EnemyDown");
		missileCount = PlayerPrefs.GetInt("MissileLeft");
		ammoCount = PlayerPrefs.GetInt("ammoCount");

		waypointDist = PlayerPrefs.GetInt("waypointDist");
		baseDist = PlayerPrefs.GetInt("baseDist");
		wayPointRemaining = PlayerPrefs.GetInt("waypointRemaining");

		float _gforce = PlayerPrefs.GetFloat("GForce");

		time += Time.fixedDeltaTime;
		SecondsToHours((int)time);
		GForce.Insert(0, _gforce);
		killAssesment.Insert(0, new Vector3(missileCount, ammoCount, enemyDownCount));
		waypointSpecs.Insert(0, new Vector3(waypointDist, baseDist, wayPointRemaining));

		pointInTime.AddValToList(positions[0], rotations[0], speeds[0], fuels[0], killAssesment[0], GForce[0],takeOffTime[0],waypointSpecs[0],messages);

	}
	float time;
	float timeCounter = 0f;
	
	void SecondsToHours(float elapsedSeconds)
    {
		TimeSpan time=TimeSpan.FromSeconds(elapsedSeconds);
		float hours = time.Hours;
		float minutes = time.Minutes;
		float seconds = time.Seconds;
		takeOffTime.Insert(0,new Vector3(hours, minutes, seconds));
		currentTime = new Vector3(hours, minutes, seconds);

	}
	public float currentX,currentY,currentZ;
	string str;
	 void AddPositionsToLog()
    {
		if(currentX!=planeData.latLong.x || currentY != planeData.latLong.y)
        {
			currentX=planeData.latLong.x;
			currentY = planeData.latLong.y;

			str = " latitude : " + currentX.ToString() + " longitude : " + currentY.ToString();
			AddSpeedToLogs(str);
		}
		

	}
	public float rot_currentX, rot_currentY, rot_currentZ;

	void AddRotationToLog()
	{
		if ((int)rot_currentX != (int)planeData.planRotation.x || (int)rot_currentY != (int)planeData.planRotation.y || (int)rot_currentZ != (int)planeData.planRotation.z)
		{
			rot_currentX = planeData.planRotation.x;
			rot_currentY = planeData.planRotation.y;
			rot_currentZ = planeData.planRotation.z;
			str = NewClock.time + " Pitch : " + rot_currentX.ToString() + " Yaw : " + rot_currentY.ToString()+ " Roll : " + rot_currentZ.ToString();
			messages.Add(str);
		}


	}
	float tempSpeed;
	void AddSpeedToLogs(string position)
    {
		if((int)tempSpeed!=(int)planeData.bodySpeed)
        {
			tempSpeed = planeData.bodySpeed;
			tempSpeed = Mathf.Clamp((float)tempSpeed, 0,(float)tempSpeed + 10f);
			str = NewClock.time + " speed : " + tempSpeed.ToString() + " "+position;
			messages.Add(str);
		}
    }
    #endregion
}
