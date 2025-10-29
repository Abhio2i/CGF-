using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Feed_Scene.newFeed
{
    [Serializable]
    public class DatasToBeSaved
    {
        [HideInInspector] public List<string> storePosition = new List<string>();
        [HideInInspector] public List<string> storeRosition = new List<string>();

        [HideInInspector] public List<float> storeGforce = new List<float>();
        [HideInInspector] public List<float> storeMissileCount = new List<float>();
        [HideInInspector] public List<float> speed = new List<float>();

        [HideInInspector] public List<string> storeEvents = new List<string>();
        [HideInInspector] public List<string> storeTime = new List<string>();
        [HideInInspector] public List<string> LatLong = new List<string>();

        [HideInInspector] public string setFileName;
        [HideInInspector] public string _NPC_type;


        [HideInInspector] public string firstFrame, lastFrame;
    }


}