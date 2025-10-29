using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Playables;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Feed
{
    public class SaveDataManager : MonoBehaviour
    {
        FileStream fileStream;
        SaveData saveData;
        BinaryFormatter converter = new BinaryFormatter();

        public static List<SaveAllData> saveAllDatas = new List<SaveAllData>();

        private void Awake()
        {
            saveAllDatas.Clear();
            print(Application.dataPath);
            print(Application.streamingAssetsPath);
            ClearAndResetDirectory();
        }
        public void ClearAndResetDirectory()
        {
            string temp = Application.dataPath+"/EntityData";
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
                
            }
            else
            {
                string[]files = Directory.GetFiles(@temp);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }
        public void writeFile(SaveData data,string filePath)
        {
            // Create a FileStream connected to the saveFile path.
            // Set the file mode to "Create".
            
            fileStream = new FileStream(filePath, FileMode.Create);
            converter.Serialize(fileStream, data);
            PlayerPrefs.SetInt("MaxFrames", data.storeEvents.Count);
            // Close stream.
            fileStream.Close();
        }
    }
}