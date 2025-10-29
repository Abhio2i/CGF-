using System.Collections;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
//csv to scriptable object
namespace Assets.Scripts.Feed
{
    public class CSVtoSO
    {
        public static string path;
        public bool trigger;
        public Entity entity;
        public void Create()
        {
            _CSVtoSO();
        }

        private float ReturnStringFormat(string value)
        {
            if (value.StartsWith('('))
                return float.Parse(value.Substring(1));
            else if (value.EndsWith(')'))
                return float.Parse(value.Substring(0, value.Length - 1));
            else
                return float.Parse(value);
        }
        private Vector3 ReturnVectorFromString(string _vector)
        {
            string[] values=new string[3];

            values = _vector.Split(',');

            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), 0);
        }
        public void _CSVtoSO()
        {
            string[] allLines = File.ReadAllLines(path);
            entity = ScriptableObject.CreateInstance<Entity>();

            entity.positions = new System.Collections.Generic.List<Vector3>();
            entity.rotations = new System.Collections.Generic.List<Vector3>();
            entity.gforce = new System.Collections.Generic.List<float>();
            entity.missileCount = new System.Collections.Generic.List<float>();
            entity.eventType = new System.Collections.Generic.List<string>();
            entity.time=new System.Collections.Generic.List<string>(); 
            entity.type = "";

            foreach (string line in allLines)
            {
                string[] splitData = line.Split(',');


                if (splitData.Length == 7)
                {
#if UNITY_EDITOR
                    //AssetDatabase.CreateAsset(entity, $"Assets/EntityScriptableObj/NPC.asset");
#endif
                    continue;
                }
                Vector3 pos = new Vector3(ReturnStringFormat(splitData[1]),
                ReturnStringFormat(splitData[2]), ReturnStringFormat(splitData[3]));

                Vector3 rot = new Vector3(ReturnStringFormat(splitData[4]),
                    ReturnStringFormat(splitData[5]), ReturnStringFormat(splitData[6]));
                
                float _gForce = ReturnStringFormat(splitData[7]);
                
                int missileCount = (int)ReturnStringFormat(splitData[8]);


                    

               entity.rotations.Add(rot);
               entity.positions.Add(pos);
               entity.gforce.Add(_gForce);
               entity.missileCount.Add(missileCount);
               entity.eventType.Add(splitData[9]);
                entity.time.Add(splitData[10]);
               entity.type = splitData[0];

            }
            PlayerPrefs.SetInt("MaxFrames", entity.rotations.Count);
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }
        }
}

