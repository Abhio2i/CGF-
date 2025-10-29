#region Script Info
// output : The Script holds all the missiles data that is stored on the game.
#endregion
using UnityEngine;

public class Displaydata : MonoBehaviour
{
    string s1;
    string s2;
    string s3;
    string s4;
    string s5L;
    string s5;
    string s5R;
    string s6;
    string s7;
    string s8;
    string s9;
    
    int w1;
    int w2;
    int w3;
    int w4;
    int w5L;
    int w5;
    int w5R;
    int w6;
    int w7;
    int w8;
    int w9;

    int gun;
    int flare;
    int jam;
    int towed;
    int chaff;
    private void Start()
    {
        s1 = PlayerPrefs.GetString("s1");
        s2 = PlayerPrefs.GetString("s2");
        s3 = PlayerPrefs.GetString("s3");
        s4 = PlayerPrefs.GetString("s4");
        s5L = PlayerPrefs.GetString("s5L");
        s5 = PlayerPrefs.GetString("s5");
        s5R = PlayerPrefs.GetString("s5R");
        s6 = PlayerPrefs.GetString("s6");
        s7 = PlayerPrefs.GetString("s7");
        s8 = PlayerPrefs.GetString("s8");
        s9 = PlayerPrefs.GetString("s9");
        
        w1 = PlayerPrefs.GetInt("w1");
        w2 = PlayerPrefs.GetInt("w2");
        w3 = PlayerPrefs.GetInt("w3");
        w4 = PlayerPrefs.GetInt("w4");
        w5L = PlayerPrefs.GetInt("w5L");
        w5 = PlayerPrefs.GetInt("w5");
        w5R = PlayerPrefs.GetInt("w5R");
        w6 = PlayerPrefs.GetInt("w6");
        w7 = PlayerPrefs.GetInt("w7");
        w8 = PlayerPrefs.GetInt("w8");
        w9 = PlayerPrefs.GetInt("w9");

        gun = PlayerPrefs.GetInt("gun");
        flare = PlayerPrefs.GetInt("flares");
        chaff = PlayerPrefs.GetInt("chaffs");
        jam = PlayerPrefs.GetInt("jammers");
        towed = PlayerPrefs.GetInt("towed");


        //print(s1);
        //print(s2);
        //print(s3);
        //print(s4);
        //print(s5L);
        //print(s5);
        //print(s5R);
        //print(s6);
        //print(s7);
        //print(s8);
        //print(s9);
        
        //print(w1);
        //print(w2);
        //print(w3);
        //print(w4);
        //print(w5L);
        //print(w5);
        //print(w5R);
        //print(w6);
        //print(w7);
        //print(w8);
        //print(w9);

        //print(gun);
        //print(flare);
        //print(chaff);
        //print(jam);
        //print(towed);

    }

}
