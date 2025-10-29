using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class ClearDirectory : MonoBehaviour
{
    string extension="/AI_PlaneSpecs";
    string []arr = { "/Ally","/Adversary","/Neutral" };
    void Awake()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (Directory.Exists(Application.persistentDataPath + extension+arr[i]))
            {
                string[] allFiles = Directory.GetFiles(Application.persistentDataPath + extension+arr[i]);
                if (allFiles.Length > 0)
                {
                    foreach (string file in allFiles)
                    {
                        File.Delete(file);
                    }
                }
                Debug.Log("cleared at"+" "+Application.persistentDataPath);
            }
            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + extension+arr[i]);
                Debug.Log("created at "+Application.persistentDataPath);
            }
        }
    }
}
