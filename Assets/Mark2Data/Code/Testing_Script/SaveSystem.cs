using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
namespace Test.script
{
    public class SaveSystem : MonoBehaviour
    {
        public InputField _name;
        public InputField health;
        public List<float> healthList;

        public void SaveData()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "Specs.dat",FileMode.Open);
            PlayerData data = new PlayerData();
            data.myName = _name.text;
            data.myHealth = int.Parse(health.text);
            data.myHealthList=healthList;
            binaryFormatter.Serialize(stream, data);
            stream.Close();
            Debug.Log("saved");
        }

        public void LoadData()
        {
            if (File.Exists(Application.persistentDataPath + "Specs.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "Specs.dat", FileMode.Open);
                PlayerData data = bf.Deserialize(file) as PlayerData;
                _name.text = data.myName;
                health.text = data.myHealth.ToString();
                healthList=data.myHealthList;
                file.Close();
            }
        }
    }
}