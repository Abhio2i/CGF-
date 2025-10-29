using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

namespace Assets.Scripts.Utility
{
    public class Save : MonoBehaviour
    {
        public static bool save,startSave;
        public string entityData_path;
        public string entityDataScriptableObj_path;

        public static int saveFunctionsCount;
        public static int currentCount;
        private void Awake()
        {
            saveFunctionsCount = 1000;
            currentCount= 0;

            entityData_path = Application.dataPath + "/EntityData";
            entityDataScriptableObj_path = Application.dataPath + "/EntityScriptableObj";

            CreateDirectory(entityData_path);
            CreateDirectory(entityDataScriptableObj_path);

            startSave = false;
            save = false;
        }
        public void SaveGame()
        {
            save = true;
        }

        public void CreateDirectory(string path)
        {
            //if(!Directory.Exists(path))
            //{
            //    print(path + " NE");
            //    Directory.CreateDirectory(path);
            //}
        }
    }
}