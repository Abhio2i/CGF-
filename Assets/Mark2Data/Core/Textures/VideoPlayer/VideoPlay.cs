using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
//public class VideoPlay : MonoBehaviour
//{
//    private string path;
//    private string path1;
//    private string path2;
//    private string path3;
//    private string path4;
//    public string VidFileName;
//    public Texture texture;
//    public VideoPlayer player;
//    public RawImage screen;

//    //D:/Mark_2/Mark_2/Assets/Captures
//    void Start()
//    {        
//        //path = Application.streamingAssetsPath + "/Capture";
//        path = Application.dataPath + "/Captures";
//        path1 = Application.persistentDataPath + "/Captures";
//        path2 = Application.streamingAssetsPath + "/Captures";
//        print(path);
//        print(path1);
//        print(path2);
//        path = path.Replace("Assets/", "");
//        player.url = path + "/" + VidFileName +".mp4";

//        //check if the video Found Then Play the Video.
//        if (player.url != null)
//        {
//            texture = player.texture;
//            player.Play();
//        }
//        //check if the video not Found Then Dispay Error.
//        else
//        {
//            Debug.LogError((player.url).ToString() + " Not Found.");
//        }
//    }
//}
