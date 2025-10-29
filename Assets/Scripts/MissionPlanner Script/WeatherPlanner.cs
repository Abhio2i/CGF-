using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherPlanner : MonoBehaviour
{
    public FillTransparency fillTransparency;
    public MissionPlan Master;
    public GameObject RainBar;
    public GameObject SunnyBar;
    public GameObject CloudBar;

    public Scrollbar Fogbar;
    public Slider Time;

    public GameObject DayLandscape;
    public GameObject NightLandscape;

    public GameObject DayHightlight;
    public GameObject NightHightlight;

    public void selectWeather(int i)
    {
        Master.WeatherData.weather = (WeatherData.Weather)i;
    }

    public void SetFogIntensity(float f)
    {
        Master.WeatherData.Fog = f;
    }

    public void SetTime(float time)
    {
        Master.WeatherData.Time = time;
    }

    public void LoadData(WeatherData data)
    {
        Master.WeatherData = data;
        Time.value = data.Time;
        Fogbar.value = data.Fog;
        if(data.weather == WeatherData.Weather.Sunny)
        {
            SunnyBar.SetActive(true);
            RainBar.SetActive(false);    
            CloudBar.SetActive(false);
        }else
        if (data.weather == WeatherData.Weather.Rain)
        {
            SunnyBar.SetActive(false);
            RainBar.SetActive(true);
            CloudBar.SetActive(false);
        }else
        if (data.weather == WeatherData.Weather.Cloud)
        {
            SunnyBar.SetActive(false);
            RainBar.SetActive(false);
            CloudBar.SetActive(true);
        }


    }
    


}
