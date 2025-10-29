using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Weapons_Data;
using Mewlist.MassiveClouds;

public class EnvironmentManager : MonoBehaviour
{
    public MissionPlan Master;
    public AtmosPad atmosPad;
    [Range(0f, 24f)]
    public float Hour = 6;
    public WeatherData.Weather weather = WeatherData.Weather.Sunny;
    [Range(0f,1f)]
    public float FogIntensity = 0;
    public Light directionalLight;
    public Transform Light;
    public Material skyboxMaterial;
    public ParticleSystem FogVolumetric;
    public Material cloudMaterial;
    public Material fogMaterial;
    public GameObject Clouds;
    public float targetweather = 0.8546554f;
    public float weatherVariation = 0.8546554f;


    /*
    #region Public_Parameters
    public float rotateSpeed;
    public GameObject volumatricFog;
    public ParticleSystem fog;
    public Light currrentLight;
    public Material[] Clouds;
    public Color[] CloudsColor;
    public int index = 0;
    public bool DayCycle = false;
    [Range(0, 1f)]
    public float cycleSpeed = 0.1f;
    #endregion

    #region private_Parameters
    [Range(0f, 1f)]
    public float fogThick;
    public float dustVis;
    public float fogVis;


    public int isFog;

    public Color lightColor;
    public Color fogColor;
    
    #endregion
    */
    private void Awake()
    {
        Hour = Master.WeatherData.Time;
        weather = Master.WeatherData.weather;
        FogIntensity = Master.WeatherData.Fog;

        FogVolumetric.emissionRate = (int)(FogIntensity * 100f);

        if(weather == WeatherData.Weather.Cloud)
        {
            Clouds.SetActive(true);
        }

        //Light.localEulerAngles = new Vector3(-60f, 110, 0);
        //Day cloud D8DBDD
        //night cloud 3E3E41
        //day fog F3F3F3
        //night fog 3F3F3F
    }

    private void Start()
    {
        Light.localEulerAngles = new Vector3(20f, 110, 0);
    }

    public void setWeather(int v)
    {
        switch (v)
        {
            case 0:
                //Clear
                targetweather = 0.8954133f;
                //atmosPad.SetVariation(0.8954133f);
                break;
            case 1:
                //Light
                targetweather = 0.8733361f;
                //atmosPad.SetVariation(0.8733361f);
                break;
            case 2:
                //Moderate
                targetweather = 0.8546554f;
                ///atmosPad.SetVariation(0.8546554f);
                break;
            case 3:
                //Severe
                targetweather = 0.275554f;
                //atmosPad.SetVariation(0.275554f); 
                break;
            case 4:
                //Intense
                targetweather = 0.3010277f;
                //atmosPad.SetVariation(0.3010277f);
                break;
        }
    }

    private void OnDestroy()
    {
        setWeather(2);
    }

    public void Update()
    {
        weatherVariation = Mathf.Lerp(weatherVariation, targetweather, Time.deltaTime * 1f);
        atmosPad.SetVariation(weatherVariation);
        float dayStart = 5;//hour
        float dayEnd = 18;//hour
        float sunRise = -8f;//deg
        float sunSet = 188f;//deg

        if (Hour >= dayStart && Hour <= dayEnd)
        {
            float DayRange = dayEnd - dayStart;
            float hour = Hour - dayStart;
            float dayPercent = hour/ DayRange;
            float SunDegRange = (sunRise * -1) + sunSet;
            float sunDeg = SunDegRange * dayPercent;
            sunDeg = sunDeg + sunRise;
            
            //Light.localEulerAngles = new Vector3(sunDeg, 110, 0);

        }
        
        // Get the current Skybox material
        

        // Check if the Skybox material has a property for atmospheric thickness
        if (skyboxMaterial.HasProperty("_AtmosphereThickness"))
        {
            Color TintColor = new Color();
            //Color FogColor = new Color();
            if (Hour >= dayStart && Hour <= dayEnd)
            {
                TintColor.a = 1;
                TintColor.r = TintColor.g = TintColor.b = 255/255;
                
                directionalLight.intensity = 1.52f;
                directionalLight.colorTemperature = 6153;
                skyboxMaterial.SetFloat("_AtmosphereThickness", 0.47f);
                skyboxMaterial.SetColor("_Tint", TintColor);
                skyboxMaterial.SetFloat("_Exposure", 1.85f);
                RenderSettings.fogColor = HexToColor("#587884");
                
            }
            else
            {
                TintColor.a = 1;
                TintColor.r = TintColor.g = TintColor.b = 41/255;
                directionalLight.intensity = 0.04f;
                directionalLight.colorTemperature = 8602;
                skyboxMaterial.SetFloat("_AtmosphereThickness", 0.61f);
                skyboxMaterial.SetFloat("_Exposure", 0.03f);
                RenderSettings.fogColor = HexToColor("#1F2623");
            }
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 60000;
            // Apply changes to the Skybox
            //RenderSettings.skybox = skyboxMaterial;
        }
        else
        {
            Debug.LogError("The Skybox material does not have an _AtmosphereThickness property.");
        }

    }

    Color HexToColor(string hex)
    {
        Color color = Color.white;

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hexadecimal color code: " + hex);
            return Color.white; // Default to white if the conversion fails
        }
    }
    /*
    public void Start(){

        //volumatricFog.SetActive(false);

        ActiveCloud();
        Fog();
        DayNight();
    }
    private void Fog()
    {
        if(isFog == 0)
        {
            //print("not fog");
            volumatricFog.SetActive(false);
            return;
        }
        // if (dustVis > 0 | fogVis > 0 | fogThick > 0)
        // {
        //     RenderSettings.fog = true;
        // }

        // RenderSettings.fogMode = FogMode.Linear;
        // RenderSettings.fogStartDistance = 0;
        if (fogThick > 0)
        {
            float size = 0;
            
            // fogColor.r = fogColor.g = fogColor.b = 130f / 255f;
            // fogColor.a = 120f / 255f;
            // RenderSettings.fogColor = fogColor;
            // RenderSettings.fogEndDistance = (fogThick + fogVis) * 100;
            
            
            size = fogThick * 20000;
            var fogThickness = fog.main;
            fogThickness.startSizeX = size;
            fogThickness.startSizeY = size;
            fogThickness.startSizeZ = size;
            //Debug.Log(size);            
        }

        if (dustVis > 0)
        {
            fogColor.r = 180f / 255f;
            fogColor.g = 160f / 255f;
            fogColor.b = 150f / 255f;
            fogColor.a = 120f / 255f;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogEndDistance = (dustVis) * 100;
        }
        volumatricFog.SetActive(true);
        //print("not done");
    }


    private void DayNight()
    {

        if (hour > 6 && hour < 10)
        {
            // start day
            lightColor.r = lightColor.g = lightColor.b = 200f / 255f;

        }
        else if (hour > 10 && hour < 14)
        {
            lightColor.r = lightColor.g = lightColor.b = 240f / 255f;

            // at noon
        }
        else if (hour > 14 && hour < 18)
        {
            lightColor.r = lightColor.g = lightColor.b = 150f / 255f;

            //evening
        }
        else if (hour > 18 && hour < 23)
        {
            lightColor.r = lightColor.g = lightColor.b = 100f / 255f;

            //night
        }
        else if(hour > 0 && hour < 5)
        {
            lightColor.r = lightColor.g = lightColor.b = 50f / 255f;
            // midnight
        }
        else
        {
            lightColor.r = lightColor.g = lightColor.b = 240f / 255f;
            //Default Get To Day.
        }
        lightColor.a = 200f / 255f;
        currrentLight.color = lightColor;
    }
    private void Update()
    {
        if (DayCycle)
        {

            hour += Time.deltaTime * cycleSpeed;
            if (hour > 24)
            {
                hour = 0;
            }
            //RotateSkyBox();
            float h = hour - 6;
            float f = hour;
            float r = 0;
            float g = 0;
            float b = 0;

            if (f > 4f && f <= 5.9f)
            {
                f = f - 4f;
                r = (f / 2) * 70f + ((f / 2) * 10f) + ((f / 2) * 10f * fogThick);
                g = (f / 2) * 40f + ((f / 2) * 30f) + ((f / 2) * 30f * fogThick);
                b = (f / 2) * 20f + ((f / 2) * 30f) + ((f / 2) * 30f * fogThick);
            }
            else
            if (f > 5.9f && f < 12)
            {
                f = f - 5.9f;
                //r = (f / 6) *155f + ((f / 6) * 5f);
                //g = (f / 6) * 143f + ((f / 6) * 17f);
                //b = (f / 6) * 110f + ((f / 6) * 50f);
                r = 80 + ((f / 6) * 80f) + ((f / 6) * 20f * fogThick);
                g = 70 + ((f / 6) * 90f) + ((f / 6) * 30f * fogThick);
                b = 50 + ((f / 6) * 110f) + ((f / 6) * 50f * fogThick); ;
            }
            else
            if (f >= 12 && f < 18)
            {
                f = f - 12f;
                //f = (1-(f / 9)) * 160f;
                r = 80 + ((1 - (f / 6f)) * 80f) + ((1 - (f / 6f)) * 20f * fogThick);
                g = 70 + ((1 - (f / 6f)) * 90f) + ((1 - (f / 6f)) * 30f * fogThick);
                b = 50 + ((1 - (f / 6f)) * 110f) + ((1 - (f / 6f)) * 50f * fogThick);
            }

            r = r > 180 ? 180 : r;
            g = g > 180 ? 180 : g;
            b = b > 180 ? 180 : b;
            fogColor.r = r / 255f;
            fogColor.g = g / 255f;
            fogColor.b = b / 255f;
            fogColor.a = 255f;
            RenderSettings.fogColor = fogColor;
            float t = hour;
            if (t > 6 && t < 18)
            {
                t = t - 6;
                t = (t / 12) * 5000;
            }

            currrentLight.colorTemperature = t + 3000f;
            currrentLight.transform.localEulerAngles = new Vector3((h / 24f) * 360f, 110, 0);
        }
    }
    void RotateSkyBox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
    */
}
