using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace newSaveSystem
{
    public static class SavingSystem
    {
        public static int cockpitView;
        public static int flares;
        public static int gun;
        public static int chaffs;
        public static int jammers;
        static string newPath = Application.dataPath + "/SpawningData";
        static string[] files;
        public static void ClearAndResetDirectory(string type)
        {
            string temp = newPath + "/" + type;
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            else
            {
                files=Directory.GetFiles(@temp);
                foreach(string file in files)
                {
                    File.Delete(file);
                }
            }
        }
        public static void SaveFile(SpawnPlanesData spawnPlanesData,string type)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path =newPath+"/" + type + "/"+Time.realtimeSinceStartup + "_SpawnData.fun";
            FileStream file = new FileStream(path, FileMode.Create);
            SpawnPlanesData spawnPlanes = new SpawnPlanesData(spawnPlanesData);
            binaryFormatter.Serialize(file, spawnPlanes);
            file.Close();
        }

        public static string[] LoadFile(string type)
        {
            string temp=newPath +type;
            files = Directory.GetFiles(temp,"*.fun");
            //Debug.Log(temp + " "+ files.Length );
            return files;
        }
        public static SpawnPlanesData PlanesData(string file)
        {
            if (File.Exists(file))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(file, FileMode.Open);

                SpawnPlanesData spawnPlanesData = formatter.Deserialize(fileStream) as SpawnPlanesData;
                fileStream.Close();

                return spawnPlanesData;
            }
            else return null;
        }
    }
}