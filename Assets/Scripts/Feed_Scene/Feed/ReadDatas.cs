using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Feed
{
    public class ReadDatas : MonoBehaviour
    {
        private CSVtoSO convert;
        private List<string> csvFiles=new List<string>();
        private string folderPath;
        private FeedSceneManager sceneManager;
        private FileStream stream;
        private BinaryFormatter formatter=new BinaryFormatter();

        public Entity entity;
        public List<Entity> entities=new List<Entity>();
        //public Dictionary<string,SaveData> saveAllData_Dict = new Dictionary<string,SaveData>();
        public List<SaveData> saveDatas=new List<SaveData>();

        SaveData saveData;

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
                    stream = new FileStream(file, FileMode.Open,FileAccess.Read);
                    saveData = formatter.Deserialize(stream) as SaveData;
                    saveDatas.Add(saveData);
                }
            }
        }
    }
}