using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Feed_Scene.newFeed
{
    public class NewSaveManager : MonoBehaviour
    {
        FileStream fileStream;
        SaveData saveData;
        BinaryFormatter converter = new BinaryFormatter();

        public static List<SaveEntityDatas> saveAllDatas = new List<SaveEntityDatas>();

        private void Awake()
        {
            saveAllDatas.Clear();
            print(Application.dataPath);
            print(Application.streamingAssetsPath);
            ClearAndResetDirectory();
        }
        public void ClearAndResetDirectory()
        {
            string temp = Application.dataPath + "/EntityData";
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);

            }
            else
            {
                string[] files = Directory.GetFiles(@temp);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }
        public void writeFile(DatasToBeSaved data, string filePath)
        {
            fileStream = new FileStream(filePath, FileMode.Create);
            converter.Serialize(fileStream, data);
            fileStream.Close();
        }

    }
}