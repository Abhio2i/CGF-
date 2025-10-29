using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UnitConversion : MonoBehaviour
{
    public Dropdown Aircraftaltitude;
    public Dropdown Aircraftspeed;
    public Dropdown A2AWeaponSpeed;
    public Dropdown A2AWeaponRange;
    public Dropdown A2SurfaceWeaponSpeed;
    public Dropdown A2SurfaceAWeaponRange;
    public Dropdown A2SeaWeaponSpeed;
    public Dropdown A2SeaWeaponRange;
    public Dropdown Radarrange;
    public Dropdown WarfareRange;
    public Dropdown jamarRange;

    public string path = "D:/json";
    string fpath = Path.Combine("D:/json", "DropdownValues.txt");



    public class Dropdowns {
        public  int AircraftaltitudeValue;
        public  int AircraftspeedValue;
        public int  A2AWeaponspeedValue;
        public int A2AWeaponRangeValue;
        public int A2SurfaceWeaponSpeedValue;
        public int A2SurfaceAWeaponRangeValue;
        public int A2SeaWeaponSpeedValue;
        public int  A2SeaWeaponRangeValue;
        public int RadarrangeValue;
        public int WarfareRangeValue;
        public int jamarRangeValue;


    }


    //static functions for speed convert meter to...
    public static float MpsToFPs(float mps)
    {
        return mps * 3.28084f; 
    }
  
    public static float MpsToKmph(float mps)
    {
        return mps * 3.6f; 
    }

  
    public static float MpsToKnots(float mps)
    {
        return mps * 1.94384f;  
    }


    public static float MpsToMach(float mps)
    {
        return mps / 343.0f;

    }
    public static float FpsToMps(float fps)
        {
            return fps / 3.28084f;
        }
    public static float FpsToKmph(float fps)
    {
        return fps* 1.097f;

    }
    public static float fpsToKnots(float fps)
    {
        return fps/ 1.94384f;
    }
    
    //static functions for Range convert
    public static float fpsToMach(float fps)
    {
        return fps / 1116.4f;
    }
    public static float kmphtoMps(float kmph)
    {
        return kmph / 3.6f;
    }
    public static float kmphtofps(float kmph)
    {
        return (kmph / 3.6f) * 3.281f;
    }
    public static float kmphtoKnots(float kmph)
    {
        return kmph / 1.852f;
    }
    public static float kmphtoMach(float kmph)
    {
        return kmph / 1235.5f;
    }
    public static float knotsToMps(float knots)
    {
        return knots * 0.514444f;
    }
    public static float knotsToFps(float knots)
    {
        return knots * 1.68781f;
    }
    public static float knotsToKmph(float knots)
    {
        return knots * 1.852f;
    }
    public static float KnotsToMach(float knots)
    {
        return knots / 661.5f;

    }
    public static float MachToMps(float mach)
    {
        return mach * 343f; 
    }


    public static float MachToKmph(float mach)
    {
        return mach * 1235.5f;
    }
   
    public static float MachToKnots(float mach)
    {
        return mach * 661.5f; 

    }
    public static float MachToFps(float mach)
    {
        return mach * 1125.33f;
    }

    //Convertors for Range
    // Meter to Feet
    public static float MeterToFeet(float meters)
    {
        return meters * 3.28084f; 
    }

    // Meter to Kilometers
    public static float MeterToKilometers(float meters)
    {
        return meters / 1000f;
    }

    // Meter to Nautical Miles
    public static float MeterToNauticalMiles(float meters)
    {
        return meters / 1852f; 
    }
    // Feet to Meters
    public static float FeetToMeters(float feet)
    {
        return feet / 3.28084f; 
    }

    // Feet to Kilometers
    public static float FeetToKilometers(float feet)
    {
        return feet / 3280.84f;
    }

    // Feet to Nautical Miles
    public static float FeetToNauticalMiles(float feet)
    {
        return feet / 6076.12f; 
    }
    // Kilometers to Meters
    public static float KilometersToMeters(float kilometers)
    {
        return kilometers * 1000f; 
    }

    // Kilometers to Feet
    public static float KilometersToFeet(float kilometers)
    {
        return kilometers * 3280.84f; 
    }

    // Kilometers to Nautical Miles
    public static float KilometersToNauticalMiles(float kilometers)
    {
        return kilometers / 1.852f; 
    }
    // Nautical Miles to Meters
    public static float NauticalMilesToMeters(float nauticalMiles)
    {
        return nauticalMiles * 1852f;
    }

    // Nautical Miles to Feet
    public static float NauticalMilesToFeet(float nauticalMiles)
    {
        return nauticalMiles * 6076.12f;

    }

    // Nautical Miles to Kilometers
    public static float NauticalMilesToKilometers(float nauticalMiles)
    {
        return nauticalMiles * 1.852f; 
    }




    private void Start()
    {
        loadDropdownValue();  
    }

    void savedropdownValue() {
        
        Dropdowns drop=new Dropdowns();
        drop.AircraftaltitudeValue = Aircraftaltitude.value;
        drop.AircraftspeedValue=Aircraftspeed.value; 
        drop.A2AWeaponRangeValue = A2AWeaponRange.value;
        drop.A2AWeaponspeedValue = A2AWeaponSpeed.value;    
        drop.A2SurfaceAWeaponRangeValue= A2SurfaceAWeaponRange.value;
        drop.A2SurfaceWeaponSpeedValue = A2SurfaceWeaponSpeed.value;    
        drop.A2SeaWeaponRangeValue = A2SeaWeaponRange.value;
        drop.A2SeaWeaponSpeedValue = A2SeaWeaponSpeed.value;
        drop.RadarrangeValue = Radarrange.value;
        drop.WarfareRangeValue = WarfareRange.value;    
        drop.jamarRangeValue= jamarRange.value; 
         
        string Dropdown=JsonUtility.ToJson(drop);
       
        File.WriteAllText(fpath,Dropdown);
    }
     void loadDropdownValue()
    {
        string dropdown = File.ReadAllText(fpath);
        Dropdowns drop = JsonUtility.FromJson<Dropdowns>(dropdown);
        Aircraftaltitude.value = drop.AircraftaltitudeValue;
        Aircraftspeed.value = drop.AircraftspeedValue;
        A2AWeaponRange.value = drop.A2AWeaponRangeValue;
        A2AWeaponSpeed.value = drop.A2AWeaponspeedValue;
        A2SurfaceAWeaponRange.value = drop.A2SurfaceAWeaponRangeValue;
        A2SurfaceWeaponSpeed.value = drop.A2SurfaceWeaponSpeedValue;
        A2SeaWeaponRange.value = drop.A2SeaWeaponRangeValue;
        A2SeaWeaponSpeed.value = drop.A2SeaWeaponSpeedValue;
        Radarrange.value = drop.RadarrangeValue;
        WarfareRange.value=drop.WarfareRangeValue;
        jamarRange.value=drop.jamarRangeValue;  
    }
    private void Update()
    {
        savedropdownValue();
    }
}
