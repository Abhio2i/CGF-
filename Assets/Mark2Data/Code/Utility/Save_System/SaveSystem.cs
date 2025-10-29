using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloToolkit.Unity
{
    [System.Serializable]
    public class SaveSystem
    {
        //public string extension = "/AI_PlaneSpecs/"+Time.realtimeSinceStartup+"planeDetails.dat";
        public string name, type, model, country, pilot, skill, count;
        public int unit;
        public int callSign1, callSign2;
        public bool radio, hiddenOnMap, hiddenOnPlanner, hiddenOnRadar;
        public List<float> spawnPositionsAtX, spawnPositionsAtY, spawnPositionsAtZ;
        public string callSign_code;
    }
}
