using Data.Plane;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EW_Inputs : MonoBehaviour
{
    public GameObject aircraft;
    public SilantroDisplay display;
    public PlaneData data;
    public bool validity;
    public float trueHeading, pitchAngle, rollAngle, presentPositionLatitude, presentPositionLongitude;
    public float inertialXVelocity, inertialYVelocity, inertialZVelocity;
    public float longAcceleration, latAccelaration, normalAcceleration;
    public float tas, groundSpeed;
    public double pitchRate, rollRate, yawRate;
    public int radioHeight, baroAltitude;
    //public int gpsYear, gpsMonth, gpsDay, gpsHour, gpsMinute, gpsSecond;
    public int gpsWeek, gpsSeconds;
    public bool ingpsParameterValidity, baroAltitudeValidity, radioAltValidity;
    public bool acOnGroundStatus, tasValidity, gsValidity;

    private Vector3 flat, localFlat;
    private float inertailX, inertailY, inertailZ;
    private double v, u;
    private int SECS_PER_WEEK = 60 * 60 * 24 * 7;
    private RaycastHit hit;

    void Start()
    {

    }
    void FixedUpdate()
    {
        trueHeading = aircraft.transform.eulerAngles.y;
        pitchAngle = -CalculatePitchAngle();
        rollAngle = CalculateRollAngle();
        presentPositionLatitude = data.latLong.x;
        presentPositionLongitude = data.latLong.y;
        inertialXVelocity = CalculateInertialX();
        inertialYVelocity = CalculateInertialY();
        inertialZVelocity = CalculateInertialZ();
        
        //u = display.m_display.m_vehicle.m_core.u;
        //v = display.m_display.m_vehicle.m_core.v;
        //groundSpeed = (float)Math.Sqrt((u * u) + (v * v));
        groundSpeed = display.connectedAircraft.core.groundSpeed;
        //turnRate.text = m_vehicle.m_core.??.ToString("0.0") + " °/s";
        //pitchRate = (display.m_display.m_vehicle.m_core.q * Mathf.Rad2Deg);
        //rollRate = (display.m_display.m_vehicle.m_core.p * Mathf.Rad2Deg);
        //yawRate = (display.m_display.m_vehicle.m_core.r * Mathf.Rad2Deg);
        pitchRate = display.connectedAircraft.core.pitchRate;
        rollRate = display.connectedAircraft.core.rollRate;
        yawRate = display.connectedAircraft.core.yawRate;
        radioHeight = RadioHight();
        baroAltitude = (int)transform.position.y;

        tas = groundSpeed + groundSpeed * 0.02f * baroAltitude / 304.8f;

        gpsWeek = GPSweek(DateTime.Now);
        gpsSeconds = GPSsecons(DateTime.Now) - 3455560;
        ValidityCheck();
    }
    float CalculatePitchAngle()
    {
        //Pitch Angle of This Aircraft
        flat = aircraft.transform.forward;
        flat.y = 0;
        flat.Normalize();
        localFlat = aircraft.transform.InverseTransformDirection(flat);
        return Mathf.Atan2(localFlat.y, localFlat.z) * 60;
    }
    float CalculateRollAngle()
    {
        //Rotation of This Aircraft
        flat = Vector3.Cross(Vector3.up, aircraft.transform.forward);
        localFlat = aircraft.transform.InverseTransformDirection(flat);
        return Mathf.Atan2(localFlat.y, localFlat.x) * 60;
    }
    float CalculateInertialX()
    {
        inertialXVelocity = ((aircraft.transform.position.x - inertailX) / Time.deltaTime);
        inertailX = aircraft.transform.position.x;
        return inertialXVelocity;
    }
    float CalculateInertialY()
    {
        inertialYVelocity = ((aircraft.transform.position.y - inertailY) / Time.deltaTime);
        inertailY = aircraft.transform.position.y;
        return inertialYVelocity;
    }
    float CalculateInertialZ()
    {
        inertialZVelocity = ((aircraft.transform.position.z - inertailZ) / Time.deltaTime);
        inertailZ = aircraft.transform.position.z;
        return inertialZVelocity;
    }
    DateTime TimeFromYMD(DateTime dt)
    {
        var a = new DateTime(dt.Year - 1900, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        return (a);
    }

    int GPSweek(DateTime dt)
    {
        // See update below
        double diff = (TimeFromYMD(dt)).Subtract(TimeFromYMD(new DateTime(1980, 1, 1))).TotalSeconds;  // See update
        return (int)((diff / SECS_PER_WEEK) - 1);
    }
    int GPSsecons(DateTime dt)
    {
        double diff = (TimeFromYMD(dt)).Subtract(TimeFromYMD(new DateTime(1980, 1, 1))).TotalSeconds;  // See update
        return (int)(diff % SECS_PER_WEEK);
    }
    int RadioHight()
    {
        Physics.Raycast(transform.position + Vector3.down * 5, Vector3.down, out hit, 762);
        return (int)hit.distance - 5;
    }
    void ValidityCheck()
    {
        validity = true; ingpsParameterValidity = true; baroAltitudeValidity = true; radioAltValidity = true;
        acOnGroundStatus = true; tasValidity = true; gsValidity = true;
    }
}
