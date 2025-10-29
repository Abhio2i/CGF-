using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeatherData
{
    public enum Weather
    {
        Sunny,
        Rain,
        Cloud
    }

    public float Time;
    public float Fog;
    public Weather weather;

    public WeatherData(WeatherData data = null)
    {
        if (data != null)
        {
            Time = data.Time;
            Fog = data.Fog;
            weather = data.weather;
        }
    }
}
