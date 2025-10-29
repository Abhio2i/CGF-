using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Feed_Scene.newFeed
{
    public class NewReadDatas : MonoBehaviour
    {
        public string folderPath;

        private BinaryFormatter formatter = new BinaryFormatter();
        private FileStream stream;
        public DatasToBeSaved saveData;
        public List<DatasToBeSaved> saveDatas = new List<DatasToBeSaved>();

        private void Awake()
        {
            folderPath = Application.dataPath + "/EntityData";
            Resources.Load(folderPath);
        }
        public void Read()
        {
            GetAllFilesAndCreateSaveDataObject();
        }

        private void GetAllFilesAndCreateSaveDataObject()
        {
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.dat"))
            {
                if (File.Exists(file))
                {
                    stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    saveData = formatter.Deserialize(stream) as DatasToBeSaved;
                    saveDatas.Add(saveData);
                }
            }
        }
    }
}