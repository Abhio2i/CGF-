using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utility.Weapons_Data;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace Utility.DropDownData
{
    public class EnemyDropDownMenu : MonoBehaviour
    {
        #region Control_Parameters
        public TMP_Text name;
        public MasterSpawn masterSpawn;
        [Header("PayLoad")]
        public TMP_Text totalLoad;
        public TMP_Text missileCounting;
        public TMP_Text missileLoad;
        public TMP_Text rocketCounting;
        public TMP_Text rocketLoad;
        public TMP_Text bomCounting;
        public TMP_Text bomLoad;
        public TMP_Text fuleCounting;
        public TMP_Text fuleLoad;
        public TMP_Text otherLoad;


        [Header("1")]
        public Image img1;
        public TMP_Text text1;
        public TMP_Dropdown S1;

        [Header("2")]
        public Image img2;
        public TMP_Text text2;
        public TMP_Dropdown S2;

        [Header("3")]
        public Image img3;
        public TMP_Text text3;
        public TMP_Dropdown S3_M;
        public TMP_Dropdown S3_B;
        public TMP_Dropdown S3_R;

        [Header("4")]
        public Image img4;
        public TMP_Text text4;
        public TMP_Dropdown S4_B;
        public TMP_Dropdown S4_R;
        public TMP_Dropdown S4_F;
        [Header("5")]
        public Image img5L;
        public TMP_Text text5L;
        public TMP_Dropdown S5L;
        public Image img5;
        public TMP_Text text5;
        public TMP_Dropdown S5;
        public Image img5R;
        public TMP_Text text5R;
        public TMP_Dropdown S5R;
        [Header("6")]
        public Image img6;
        public TMP_Text text6;
        public TMP_Dropdown S6_B;
        public TMP_Dropdown S6_R;
        public TMP_Dropdown S6_F;
        [Header("7")]
        public Image img7;
        public TMP_Text text7;
        public TMP_Dropdown S7_M;
        public TMP_Dropdown S7_B;
        public TMP_Dropdown S7_R;
        [Header("8")]
        public Image img8;
        public TMP_Text text8;
        public TMP_Dropdown S8;
        [Header("9")]
        public Image img9;
        public TMP_Text text9;
        public TMP_Dropdown S9;
        [Header("References")]
        public WeaponsData Weapons;

        [HideInInspector] public int totalWeight;

        private int index1;
        private int index2;
        private int index3;
        private int index3_M;
        private int index3_B;
        private int index3_R;
        private int index4;
        private int index4_B;
        private int index4_R;
        private int index4_F;
        private int index5L;
        private int index5;
        private int index5R;
        private int index6;
        private int index6_B;
        private int index6_R;
        private int index6_F;
        private int index7;
        private int index7_M;
        private int index7_B;
        private int index7_R;
        private int index8;
        private int index9;

        private int missileCount;
        private int missileWeight;
        private int rocketCount;
        private int rocketWeight;
        private int bomCount;
        private int bomWeight;
        private int fuleCount;
        private int fuleWeight;
        [NonSerialized] public int otherWeight;
        public int countScriptableObject = 0;
        public bool player;

        #endregion
        private void Start()
        {
            Weapons = GetComponent<WeaponsData>();
            index1 = index2 = index3 = index3_B = index3_M = index3_R = index4 = index4_B = index4_F = index4_R = index5 = index5L = index5R = index6 = index6_B = index6_F = index6_R = index7 = index7_B = index7_M = index7_R = index8 = index9 = missileCount = missileWeight = rocketCount = rocketWeight = bomCount = bomWeight = fuleCount = fuleWeight = totalWeight = 0;
            Station1();
            Station2();
            Station3();
            Station4();
            Station5L();
            Station5();
            Station5R();
            Station6();
            Station7();
            Station8();
            Station9();
            if (player)
            {
                foreach (var i in masterSpawn.ally_spawnPlanes)
                {
                    i.resetme();
                }
            }
            else
            {

                foreach (var i in masterSpawn.adversary_spawnPlanes)
                {
                    i.resetme();
                }
            }

            UpdatePlayload();

        }

        #region OldStation
        //#region Station1
        //private void Station1()
        //{
        //    List<string> lis = new List<string>(Weapons._1);
        //    S1.AddOptions(lis);
        //}
        //public void S1_IndexChange(int index)
        //{
        //    List<string> lis = new List<string>(Weapons._1);
        //    Station_1_Weight name;
        //    if (index <= 3)
        //    {
        //        name = (Station_1_Weight)1;
        //    }
        //    else
        //    {
        //        name = (Station_1_Weight)2;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index1 += Int16.Parse(weight);
        //    missileWeight += Int16.Parse(weight);
        //    text1.text = index1.ToString("0000");
        //    img1.color = Color.green;

        //    S1.gameObject.SetActive(false);
        //    missileCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s1 = lis[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s1 = lis[index];
        //    PlayerPrefs.SetInt("w1", index1);
        //}
        //#endregion
        //#region Station2
        //private void Station2()
        //{
        //    List<string> list = new List<string>(Weapons._2);

        //    S2.AddOptions(list);
        //}
        //public void S2_IndexChange(int index)
        //{
        //    List<string> list = new List<string>(Weapons._2);
        //    Station_2_Weight name;
        //    bool inLimit = true;
        //    if (index <= 3)
        //    {
        //        name = (Station_2_Weight)1;
        //    }
        //    else
        //    {
        //        name = (Station_2_Weight)2;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index2 += Int16.Parse(weight);
        //    missileWeight += Int16.Parse(weight);
        //    text2.text = index2.ToString("0000");

        //    if (index2 > 700)
        //    {
        //        inLimit = false;
        //        img2.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img2.color = Color.green;
        //    }
        //    S2.gameObject.SetActive(false);
        //    missileCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s2 = list[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s2 = list[index];
        //    PlayerPrefs.SetInt("w2", index2);
        //}
        //#endregion
        //#region Station3
        //private void Station3()
        //{
        //    List<string> listM = new List<string>(Weapons._3M);
        //    List<string> listB = new List<string>(Weapons._3B);
        //    List<string> listR = new List<string>(Weapons._3R);

        //    S3_M.AddOptions(listM);
        //    S3_B.AddOptions(listB);
        //    S3_R.AddOptions(listR);
        //}
        //public void S3_M_IndexChange(int index)
        //{
        //    List<string> listM = new List<string>(Weapons._3M);
        //    Station_3M_Weight name;
        //    bool inLimit = true;
        //    if (index <= 3)
        //    {
        //        name = 0;
        //    }
        //    else
        //    {
        //        name = (Station_3M_Weight)1;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    missileWeight += Int16.Parse(weight);
        //    index3_M += Int16.Parse(weight);
        //    index3 += index3_M;
        //    text3.text = index3.ToString("0000");

        //    if (index3 > 3500)
        //    {
        //        inLimit = false;
        //        img3.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img3.color = Color.green;
        //    }
        //    S3_M.gameObject.SetActive(false);
        //    S3_B.gameObject.SetActive(false);
        //    S3_R.gameObject.SetActive(false);
        //    missileCount++;

        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s3 = listM[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s3 = listM[index];
        //    PlayerPrefs.SetInt("w3", index3_M);
        //}
        //public void S3_B_IndexChange(int index)
        //{

        //    List<string> listB = new List<string>(Weapons._3B);

        //    Station_3B_Weight name;
        //    bool inLimit = true;
        //    if (index == 4 || index == 6)
        //    {
        //        name = (Station_3B_Weight)4;
        //    }
        //    else
        //    {
        //        name = (Station_3B_Weight)index;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index3_B += Int16.Parse(weight);
        //    index3 += index3_B;
        //    bomWeight += Int16.Parse(weight);
        //    text3.text = index3.ToString("0000");

        //    if (index3 > 3500)
        //    {
        //        inLimit = false;
        //        img3.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img3.color = Color.green;
        //    }
        //    S3_M.gameObject.SetActive(false);
        //    S3_B.gameObject.SetActive(false);
        //    S3_R.gameObject.SetActive(false);
        //    bomCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s3 = listB[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s3 = listB[index];
        //    PlayerPrefs.SetInt("w3", index3_B);
        //}
        //public void S3_R_IndexChange(int index)
        //{
        //    List<string> listR = new List<string>(Weapons._3R);
        //    Station_3R_Weight name;
        //    bool inLimit = true;
        //    name = 0;

        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index3_R += Int16.Parse(weight);
        //    index3 += index3_R;
        //    rocketWeight += Int16.Parse(weight);
        //    text3.text = index3.ToString("0000");

        //    if (index3 > 3500)
        //    {
        //        inLimit = false;
        //        img3.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img3.color = Color.green;
        //    }
        //    S3_M.gameObject.SetActive(false);
        //    S3_B.gameObject.SetActive(false);
        //    S3_R.gameObject.SetActive(false);
        //    rocketCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s3 = listR[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s3 = listR[index];
        //    PlayerPrefs.SetInt("w3", index3_R);
        //}
        //#endregion
        //#region Station4
        //private void Station4()
        //{
        //    List<string> listB = new List<string>(Weapons._4B);
        //    List<string> listR = new List<string>(Weapons._4R);
        //    List<string> listF = new List<string>(Weapons._4F);

        //    S4_B.AddOptions(listB);
        //    S4_R.AddOptions(listR);
        //    S4_F.AddOptions(listF);
        //}
        //public void S4_B_IndexChange(int index)
        //{
        //    List<string> listB = new List<string>(Weapons._4B);
        //    Station_4B_Weight name;
        //    bool inLimit = true;
        //    if (index == 4 || index == 6)
        //    {
        //        name = (Station_4B_Weight)4;
        //    }
        //    else
        //    {
        //        name = (Station_4B_Weight)index;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index4_B += Int16.Parse(weight);
        //    index4 += index4_B;
        //    bomWeight += Int16.Parse(weight);
        //    text4.text = index4.ToString("0000");

        //    if (index4 > 4500)
        //    {
        //        inLimit = false;
        //        img4.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img4.color = Color.green;
        //    }
        //    S4_B.gameObject.SetActive(false);
        //    S4_R.gameObject.SetActive(false);
        //    S4_F.gameObject.SetActive(false);
        //    bomCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s4 = listB[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s4 = listB[index];
        //    PlayerPrefs.SetInt("w4", index4_B);
        //}
        //public void S4_R_IndexChange(int index)
        //{
        //    List<string> listR = new List<string>(Weapons._4R);
        //    Station_4R_Weight name;
        //    bool inLimit = true;
        //    name = 0;
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index4_R += Int16.Parse(weight);
        //    index4 += index4_R;
        //    rocketWeight += Int16.Parse(weight);
        //    text4.text = index4.ToString("0000");

        //    if (index4 > 4500)
        //    {
        //        inLimit = false;
        //        img4.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img4.color = Color.green;
        //    }
        //    S4_B.gameObject.SetActive(false);
        //    S4_R.gameObject.SetActive(false);
        //    S4_F.gameObject.SetActive(false);
        //    rocketCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s4 = listR[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s4 = listR[index];
        //    PlayerPrefs.SetInt("w4", index4_R);
        //}
        //public void S4_F_IndexChange(int index)
        //{
        //    List<string> listF = new List<string>(Weapons._4F);
        //    Station_4F_Weight name;
        //    bool inLimit = true;
        //    name = 0;
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index4_F += Int16.Parse(weight);
        //    index4 += index4_F;
        //    fuleWeight += Int16.Parse(weight);

        //    text4.text = index4.ToString("0000");

        //    if (index4 > 4500)
        //    {
        //        inLimit = false;
        //        img4.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img4.color = Color.green;
        //    }
        //    S4_B.gameObject.SetActive(false);
        //    S4_R.gameObject.SetActive(false);
        //    S4_F.gameObject.SetActive(false);
        //    fuleCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s4 = listF[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s4 = listF[index];
        //    PlayerPrefs.SetInt("w4", index4_F);
        //}
        //#endregion
        //#region Station5L
        //private void Station5L()
        //{
        //    List<string> list = new List<string>(Weapons._5L);

        //    S5L.AddOptions(list);
        //}
        //public void S5L_IndexChange(int index)
        //{
        //    List<string> list = new List<string>(Weapons._5L);

        //    Station_5L_Weight name;
        //    name = 0;
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index5L += Int16.Parse(weight);
        //    rocketWeight += Int16.Parse(weight);
        //    text5L.text = index5L.ToString("0000");
        //    img5L.color = Color.green;
        //    S5L.gameObject.SetActive(false);

        //    rocketCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s5L = list[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s5L = list[index];
        //    PlayerPrefs.SetInt("w5L", index5L);
        //}
        //#endregion
        //#region Station5
        //private void Station5()
        //{
        //    List<string> list = new List<string>(Weapons._5);

        //    S5.AddOptions(list);
        //}
        //public void S5_IndexChange(int index)
        //{
        //    List<string> list = new List<string>(Weapons._5);

        //    Station_5_Weight name;
        //    name = 0;
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index5 += Int16.Parse(weight);
        //    fuleWeight += Int16.Parse(weight);
        //    text5.text = index5.ToString("0000");
        //    img5.color = Color.green;
        //    S5.gameObject.SetActive(false);

        //    fuleCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s5 = list[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s5 = list[index];
        //    PlayerPrefs.SetInt("w5", index5);
        //}
        //#endregion
        //#region Station5R
        //private void Station5R()
        //{
        //    List<string> list = new List<string>(Weapons._5R);

        //    S5R.AddOptions(list);
        //}
        //public void S5R_IndexChange(int index)
        //{
        //    List<string> list = new List<string>(Weapons._5R);

        //    Station_5R_Weight name;
        //    name = 0;
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index5R += Int16.Parse(weight);
        //    rocketWeight += Int16.Parse(weight);
        //    text5R.text = index5R.ToString("0000");
        //    img5R.color = Color.green;
        //    S5R.gameObject.SetActive(false);

        //    rocketCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s5R = list[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s5R = list[index];
        //    PlayerPrefs.SetInt("w5R", index5R);
        //}
        //#endregion
        //#region Station6
        //private void Station6()
        //{
        //    List<string> listB = new List<string>(Weapons._6B);
        //    List<string> listR = new List<string>(Weapons._6R);
        //    List<string> listF = new List<string>(Weapons._6F);

        //    S6_B.AddOptions(listB);
        //    S6_R.AddOptions(listR);
        //    S6_F.AddOptions(listF);
        //}
        //public void S6_B_IndexChange(int index)
        //{
        //    List<string> listB = new List<string>(Weapons._6B);
        //    Station_6B_Weight name;
        //    bool inLimit = true;
        //    if (index == 4 || index == 6)
        //    {
        //        name = (Station_6B_Weight)4;
        //    }
        //    else
        //    {
        //        name = (Station_6B_Weight)index;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index6_B += Int16.Parse(weight);
        //    index6 += index6_B;
        //    bomWeight += Int16.Parse(weight);

        //    text6.text = index6.ToString("0000");

        //    if (index6 > 4500)
        //    {
        //        inLimit = false;
        //        img6.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img6.color = Color.green;
        //    }

        //    S6_B.gameObject.SetActive(false);
        //    S6_R.gameObject.SetActive(false);
        //    S6_F.gameObject.SetActive(false);
        //    bomCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s6 = listB[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s6 = listB[index];
        //    PlayerPrefs.SetInt("w6", index6_B);
        //}
        //public void S6_R_IndexChange(int index)
        //{
        //    List<string> listR = new List<string>(Weapons._6R);

        //    Station_6R_Weight name;
        //    bool inLimit = true;
        //    name = 0;
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index6_R += Int16.Parse(weight);
        //    index6 += index6_R;
        //    rocketWeight += Int16.Parse(weight);
        //    text6.text = index6.ToString("0000");

        //    if (index6 > 4500)
        //    {


        //        inLimit = false;
        //        img6.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img6.color = Color.green;
        //    }

        //    S6_B.gameObject.SetActive(false);
        //    S6_R.gameObject.SetActive(false);
        //    S6_F.gameObject.SetActive(false);

        //    rocketCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s6 = listR[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s6 = listR[index];
        //    PlayerPrefs.SetInt("w6", index6_R);
        //}
        //public void S6_F_IndexChange(int index)
        //{
        //    List<string> listF = new List<string>(Weapons._6F);
        //    Station_6F_Weight name;
        //    bool inLimit = true;
        //    name = 0;
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index6_F += Int16.Parse(weight);
        //    index6 += index6_F;
        //    fuleWeight += Int16.Parse(weight);
        //    text6.text = index6.ToString("0000");

        //    if (index6 > 4500)
        //    {
        //        inLimit = false;
        //        img6.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img6.color = Color.green;
        //    }
        //    S6_B.gameObject.SetActive(false);
        //    S6_R.gameObject.SetActive(false);
        //    S6_F.gameObject.SetActive(false);
        //    fuleCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s6 = listF[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s6 = listF[index];
        //    PlayerPrefs.SetInt("w6", index6_F);
        //}
        //#endregion
        //#region Station7
        //private void Station7()
        //{
        //    List<string> listM = new List<string>(Weapons._7M);
        //    List<string> listB = new List<string>(Weapons._7B);
        //    List<string> listR = new List<string>(Weapons._7R);

        //    S7_M.AddOptions(listM);
        //    S7_B.AddOptions(listB);
        //    S7_R.AddOptions(listR);
        //}
        //public void S7_M_IndexChange(int index)
        //{
        //    List<string> listM = new List<string>(Weapons._7M);
        //    Station_7M_Weight name;
        //    bool inLimit = true;
        //    if (index <= 3)
        //    {
        //        name = 0;
        //    }
        //    else
        //    {
        //        name = (Station_7M_Weight)1;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    missileWeight += Int16.Parse(weight);
        //    index7_M += Int16.Parse(weight);
        //    index7 += index7_M;
        //    text7.text = index7.ToString("0000");

        //    if (index7 > 3500)
        //    {
        //        inLimit = false;
        //        img7.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img7.color = Color.green;
        //    }
        //    S7_M.gameObject.SetActive(false);
        //    S7_B.gameObject.SetActive(false);
        //    S7_R.gameObject.SetActive(false);
        //    missileCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s7 = listM[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s7 = listM[index];
        //    PlayerPrefs.SetInt("w7", index7_M);
        //}
        //public void S7_B_IndexChange(int index)
        //{
        //    List<string> listB = new List<string>(Weapons._7B);
        //    Station_7B_Weight name;
        //    bool inLimit = true;
        //    if (index == 4 || index == 6)
        //    {
        //        name = (Station_7B_Weight)4;
        //    }
        //    else
        //    {
        //        name = (Station_7B_Weight)index;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index7_B += Int16.Parse(weight);
        //    index7 += index7_B;
        //    bomWeight += Int16.Parse(weight);
        //    text7.text = index7.ToString("0000");

        //    if (index7 > 3500)
        //    {

        //        inLimit = false;
        //        img7.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img7.color = Color.green;
        //    }
        //    S7_M.gameObject.SetActive(false);
        //    S7_B.gameObject.SetActive(false);
        //    S7_R.gameObject.SetActive(false);

        //    bomCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s7 = listB[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s7 = listB[index];
        //    PlayerPrefs.SetInt("w7", index7_B);
        //}
        //public void S7_R_IndexChange(int index)
        //{
        //    List<string> listR = new List<string>(Weapons._7R);

        //    Station_7R_Weight name;
        //    bool inLimit = true;
        //    name = 0;

        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index7_R += Int16.Parse(weight);
        //    index7 += index7_R;
        //    rocketWeight += Int16.Parse(weight);
        //    text7.text = index7.ToString("0000");

        //    if (index7 > 3500)
        //    {

        //        inLimit = false;
        //        img7.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img7.color = Color.green;
        //    }
        //    S7_M.gameObject.SetActive(false);
        //    S7_B.gameObject.SetActive(false);
        //    S7_R.gameObject.SetActive(false);

        //    rocketCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s7 = listR[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s7 = listR[index];
        //    PlayerPrefs.SetInt("w7", index7_R);
        //}
        //#endregion
        //#region Station8
        //private void Station8()
        //{
        //    List<string> list = new List<string>(Weapons._8);

        //    S8.AddOptions(list);
        //}
        //public void S8_IndexChange(int index)
        //{
        //    List<string> list = new List<string>(Weapons._8);

        //    Station_8_Weight name;
        //    bool inLimit = true;
        //    if (index <= 3)
        //    {
        //        name = (Station_8_Weight)1;
        //    }
        //    else
        //    {
        //        name = (Station_8_Weight)2;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index8 += Int16.Parse(weight);
        //    missileWeight += Int16.Parse(weight);
        //    text8.text = index8.ToString("0000");

        //    if (index8 > 700)
        //    {

        //        inLimit = false;
        //        img8.color = Color.red;
        //    }

        //    if (inLimit)
        //    {
        //        img8.color = Color.green;
        //    }
        //    S8.gameObject.SetActive(false);
        //    missileCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s8 = list[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s8 = list[index];
        //    PlayerPrefs.SetInt("w8", index8);
        //}
        //#endregion
        //#region Station9
        //private void Station9()
        //{
        //    List<string> list = new List<string>(Weapons._9);

        //    S9.AddOptions(list);
        //}
        //public void S9_IndexChange(int index)
        //{
        //    Station_9_Weight name;
        //    List<string> lis = new List<string>(Weapons._9);
        //    if (index <= 3)
        //    {
        //        name = (Station_9_Weight)1;
        //    }
        //    else
        //    {
        //        name = (Station_9_Weight)2;
        //    }
        //    string weight = name.ToString();
        //    weight = weight.Replace("_", "");
        //    index9 = Int16.Parse(weight);
        //    missileWeight += Int16.Parse(weight);
        //    text9.text = index9.ToString("0000");
        //    img9.color = Color.green;
        //    S9.gameObject.SetActive(false);
        //    missileCount++;
        //    UpdatePlayload();
        //    if (player)
        //        masterSpawn.ally_spawnPlanes[countScriptableObject].s9 = lis[index];
        //    else
        //        masterSpawn.adversary_spawnPlanes[countScriptableObject].s9 = lis[index];
        //    PlayerPrefs.SetInt("w9", index9);
        //}
        //#endregion
        #endregion
        #region newWeightSystem

        #region Station1
        private void Station1()
        {
            List<string> lis = new List<string>(Weapons._1);
            S1.AddOptions(lis);
        }
        public void S1_IndexChange(int index)
        {
            List<string> lis = new List<string>(Weapons._1);
            index1 += Weapons._1weight[index];
            missileWeight += Weapons._1weight[index];
            text1.text = index1.ToString("0000");
            img1.color = Color.green;
            S1.gameObject.SetActive(false);
            missileCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s1 = lis[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s1 = lis[index];
            PlayerPrefs.SetInt("w1", index1);
        }
        #endregion
        #region Station2
        private void Station2()
        {
            List<string> list = new List<string>(Weapons._2);

            S2.AddOptions(list);
        }
        public void S2_IndexChange(int index)
        {
            List<string> list = new List<string>(Weapons._2);
            Station_2_Weight name;
            index2 += Weapons._2weight[index];
            missileWeight += Weapons._2weight[index];
            text2.text = index2.ToString("0000");
            img2.color = Color.green;
            S2.gameObject.SetActive(false);
            missileCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s2 = list[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s2 = list[index];
            PlayerPrefs.SetInt("w2", index2);
        }
        #endregion
        #region Station3
        private void Station3()
        {
            List<string> listM = new List<string>(Weapons._3M);
            List<string> listB = new List<string>(Weapons._3B);
            List<string> listR = new List<string>(Weapons._3R);

            S3_M.AddOptions(listM);
            S3_B.AddOptions(listB);
            S3_R.AddOptions(listR);
        }
        public void S3_M_IndexChange(int index)
        {
            List<string> listM = new List<string>(Weapons._3M);
            missileWeight += Weapons._3Mweight[index];
            index3_M += Weapons._3Mweight[index];
            index3 += index3_M;
            text3.text = index3.ToString("0000");
                img3.color = Color.green;
            S3_M.gameObject.SetActive(false);
            S3_B.gameObject.SetActive(false);
            S3_R.gameObject.SetActive(false);
            missileCount++;

            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s3 = listM[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s3 = listM[index];
            PlayerPrefs.SetInt("w3", index3_M);
        }
        public void S3_B_IndexChange(int index)
        {

            List<string> listB = new List<string>(Weapons._3B);
            index3_B += Weapons._3Bweight[index];
            index3 += index3_B;
            bomWeight += Weapons._3Bweight[index];
            text3.text = index3.ToString("0000");
                img3.color = Color.green;
            S3_M.gameObject.SetActive(false);
            S3_B.gameObject.SetActive(false);
            S3_R.gameObject.SetActive(false);
            bomCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s3 = listB[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s3 = listB[index];
            PlayerPrefs.SetInt("w3", index3_B);
        }
        public void S3_R_IndexChange(int index)
        {
            List<string> listR = new List<string>(Weapons._3R);
            index3_R += Weapons._3Rweight[index];
            index3 += index3_R;
            rocketWeight += Weapons._3Rweight[index];
            text3.text = index3.ToString("0000");
                img3.color = Color.green;
            S3_M.gameObject.SetActive(false);
            S3_B.gameObject.SetActive(false);
            S3_R.gameObject.SetActive(false);
            rocketCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s3 = listR[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s3 = listR[index];
            PlayerPrefs.SetInt("w3", index3_R);
        }
        #endregion
        #region Station4
        private void Station4()
        {
            List<string> listB = new List<string>(Weapons._4B);
            List<string> listR = new List<string>(Weapons._4R);
            List<string> listF = new List<string>(Weapons._4F);

            S4_B.AddOptions(listB);
            S4_R.AddOptions(listR);
            S4_F.AddOptions(listF);
        }
        public void S4_B_IndexChange(int index)
        {
            List<string> listB = new List<string>(Weapons._4B);
            index4_B += Weapons._4Bweight[index];
            index4 += index4_B;
            bomWeight += Weapons._4Bweight[index];
            text4.text = index4.ToString("0000");
                img4.color = Color.green;
            S4_B.gameObject.SetActive(false);
            S4_R.gameObject.SetActive(false);
            S4_F.gameObject.SetActive(false);
            bomCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s4 = listB[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s4 = listB[index];
            PlayerPrefs.SetInt("w4", index4_B);
        }
        public void S4_R_IndexChange(int index)
        {
            List<string> listR = new List<string>(Weapons._4R);
            index4_R += Weapons._4Rweight[index];
            index4 += index4_R;
            rocketWeight += Weapons._4Rweight[index];
            text4.text = index4.ToString("0000");
                img4.color = Color.green;
            S4_B.gameObject.SetActive(false);
            S4_R.gameObject.SetActive(false);
            S4_F.gameObject.SetActive(false);
            rocketCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s4 = listR[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s4 = listR[index];
            PlayerPrefs.SetInt("w4", index4_R);
        }
        public void S4_F_IndexChange(int index)
        {
            List<string> listF = new List<string>(Weapons._4F);
            index4_F += Weapons._4Fweight[index];
            index4 += index4_F;
            fuleWeight += Weapons._4Fweight[index];
            text4.text = index4.ToString("0000");
                img4.color = Color.green;
            S4_B.gameObject.SetActive(false);
            S4_R.gameObject.SetActive(false);
            S4_F.gameObject.SetActive(false);
            fuleCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s4 = listF[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s4 = listF[index];
            PlayerPrefs.SetInt("w4", index4_F);
        }
        #endregion
        #region Station5L
        private void Station5L()
        {
            List<string> list = new List<string>(Weapons._5L);

            S5L.AddOptions(list);
        }
        public void S5L_IndexChange(int index)
        {
            List<string> list = new List<string>(Weapons._5L);
            index5L += Weapons._5Lweight[index];
            rocketWeight += Weapons._5Lweight[index];
            text5L.text = index5L.ToString("0000");
            img5L.color = Color.green;
            S5L.gameObject.SetActive(false);

            rocketCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s5L = list[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s5L = list[index];
            PlayerPrefs.SetInt("w5L", index5L);
        }
        #endregion
        #region Station5
        private void Station5()
        {
            List<string> list = new List<string>(Weapons._5);

            S5.AddOptions(list);
        }
        public void S5_IndexChange(int index)
        {
            List<string> list = new List<string>(Weapons._5);
            index5 += Weapons._5weight[index];
            fuleWeight += Weapons._5weight[index];
            text5.text = index5.ToString("0000");
            img5.color = Color.green;
            S5.gameObject.SetActive(false);

            fuleCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s5 = list[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s5 = list[index];
            PlayerPrefs.SetInt("w5", index5);
        }
        #endregion
        #region Station5R
        private void Station5R()
        {
            List<string> list = new List<string>(Weapons._5R);

            S5R.AddOptions(list);
        }
        public void S5R_IndexChange(int index)
        {
            List<string> list = new List<string>(Weapons._5R);
            index5R += Weapons._5Rweight[index];
            rocketWeight += Weapons._5Rweight[index];
            text5R.text = index5R.ToString("0000");
            img5R.color = Color.green;
            S5R.gameObject.SetActive(false);

            rocketCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s5R = list[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s5R = list[index];
            PlayerPrefs.SetInt("w5R", index5R);
        }
        #endregion
        #region Station6
        private void Station6()
        {
            List<string> listB = new List<string>(Weapons._6B);
            List<string> listR = new List<string>(Weapons._6R);
            List<string> listF = new List<string>(Weapons._6F);

            S6_B.AddOptions(listB);
            S6_R.AddOptions(listR);
            S6_F.AddOptions(listF);
        }
        public void S6_B_IndexChange(int index)
        {
            List<string> listB = new List<string>(Weapons._6B);
            index6_B += Weapons._6Bweight[index];
            index6 += index6_B;
            bomWeight += Weapons._6Bweight[index];

            text6.text = index6.ToString("0000");
                img6.color = Color.green;
            S6_B.gameObject.SetActive(false);
            S6_R.gameObject.SetActive(false);
            S6_F.gameObject.SetActive(false);
            bomCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s6 = listB[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s6 = listB[index];
            PlayerPrefs.SetInt("w6", index6_B);
        }
        public void S6_R_IndexChange(int index)
        {
            List<string> listR = new List<string>(Weapons._6R);
            index6_R += Weapons._6Rweight[index];
            index6 += index6_R;
            rocketWeight += Weapons._6Rweight[index];
            text6.text = index6.ToString("0000");
                img6.color = Color.green;

            S6_B.gameObject.SetActive(false);
            S6_R.gameObject.SetActive(false);
            S6_F.gameObject.SetActive(false);

            rocketCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s6 = listR[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s6 = listR[index];
            PlayerPrefs.SetInt("w6", index6_R);
        }
        public void S6_F_IndexChange(int index)
        {
            List<string> listF = new List<string>(Weapons._6F);
            index6_F += Weapons._6Fweight[index];
            index6 += index6_F;
            fuleWeight += Weapons._6Fweight[index];
            text6.text = index6.ToString("0000");
                img6.color = Color.green;
            S6_B.gameObject.SetActive(false);
            S6_R.gameObject.SetActive(false);
            S6_F.gameObject.SetActive(false);
            fuleCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s6 = listF[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s6 = listF[index];
            PlayerPrefs.SetInt("w6", index6_F);
        }
        #endregion
        #region Station7
        private void Station7()
        {
            List<string> listM = new List<string>(Weapons._7M);
            List<string> listB = new List<string>(Weapons._7B);
            List<string> listR = new List<string>(Weapons._7R);

            S7_M.AddOptions(listM);
            S7_B.AddOptions(listB);
            S7_R.AddOptions(listR);
        }
        public void S7_M_IndexChange(int index)
        {
            List<string> listM = new List<string>(Weapons._7M);
            missileWeight += Weapons._7Mweight[index];
            index7_M += Weapons._7Mweight[index];
            index7 += index7_M;
            text7.text = index7.ToString("0000");
                img7.color = Color.green;
            S7_M.gameObject.SetActive(false);
            S7_B.gameObject.SetActive(false);
            S7_R.gameObject.SetActive(false);
            missileCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s7 = listM[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s7 = listM[index];
            PlayerPrefs.SetInt("w7", index7_M);
        }
        public void S7_B_IndexChange(int index)
        {
            List<string> listB = new List<string>(Weapons._7B);
            index7_B += Weapons._7Bweight[index];
            index7 += index7_B;
            bomWeight += Weapons._7Bweight[index];
            text7.text = index7.ToString("0000");
                img7.color = Color.green;
            S7_M.gameObject.SetActive(false);
            S7_B.gameObject.SetActive(false);
            S7_R.gameObject.SetActive(false);

            bomCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s7 = listB[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s7 = listB[index];
            PlayerPrefs.SetInt("w7", index7_B);
        }
        public void S7_R_IndexChange(int index)
        {
            List<string> listR = new List<string>(Weapons._7R);
            index7_R += Weapons._7Rweight[index];
            index7 += index7_R;
            rocketWeight += Weapons._7Rweight[index];
            text7.text = index7.ToString("0000");
                img7.color = Color.green;
            S7_M.gameObject.SetActive(false);
            S7_B.gameObject.SetActive(false);
            S7_R.gameObject.SetActive(false);

            rocketCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s7 = listR[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s7 = listR[index];
            PlayerPrefs.SetInt("w7", index7_R);
        }
        #endregion
        #region Station8
        private void Station8()
        {
            List<string> list = new List<string>(Weapons._8);

            S8.AddOptions(list);
        }
        public void S8_IndexChange(int index)
        {
            List<string> list = new List<string>(Weapons._8);
            index8 += Weapons._8weight[index];
            missileWeight += Weapons._8weight[index];
            text8.text = index8.ToString("0000");
                img8.color = Color.green;
            S8.gameObject.SetActive(false);
            missileCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s8 = list[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s8 = list[index];
            PlayerPrefs.SetInt("w8", index8);
        }
        #endregion
        #region Station9
        private void Station9()
        {
            List<string> list = new List<string>(Weapons._9);

            S9.AddOptions(list);
        }
        public void S9_IndexChange(int index)
        {
            List<string> lis = new List<string>(Weapons._9);
            index9 = Weapons._9weight[index];
            missileWeight += index9 = Weapons._9weight[index];
            text9.text = index9.ToString("0000");
            img9.color = Color.green;
            S9.gameObject.SetActive(false);
            missileCount++;
            UpdatePlayload();
            if (player)
                masterSpawn.ally_spawnPlanes[countScriptableObject].s9 = lis[index];
            else
                masterSpawn.adversary_spawnPlanes[countScriptableObject].s9 = lis[index];
           
            PlayerPrefs.SetInt("w9", index9);
        }
        #endregion

        #endregion
        public void ResetBtn(Image _img)
        {
            _img.color = Color.white;

        }
        public void ResetCount(int count)
        {
            if (count == 1)
            {
                missileCount--;
                missileWeight -= index1;
                index1 = 0;
                text1.text = index1.ToString("0000");
                S1.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s1 = "";
                else   masterSpawn.adversary_spawnPlanes[countScriptableObject].s1 = "";
            }
            else if (count == 2)
            {
                missileCount--;
                missileWeight -= index2;
                index2 = 0;
                text2.text = index2.ToString("0000");
                S2.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s2 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s2 = "";
            }
            else if (count == 3)
            {
                missileWeight -= index3_M;
                bomWeight -= index3_B;
                rocketWeight -= index3_R;

                if (index3_M > 0) { missileCount--; }
                if (index3_B > 0) { bomCount--; }
                if (index3_R > 0) { rocketCount--; }

                index3_M = index3_B = index3_R = index3 = 0;
                text3.text = index3.ToString("0000");
                S3_M.gameObject.SetActive(true);
                S3_B.gameObject.SetActive(true);
                S3_R.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s3 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s3 = "";
            }
            else if (count == 4)
            {
                //4
                bomWeight -= index4_B;
                rocketWeight -= index4_R;
                fuleWeight -= index4_F;

                if (index4_B > 0) { bomCount--; }
                if (index4_R > 0) { rocketCount--; }
                if (index4_F > 0) { fuleCount--; }

                index4 = index4_B = index4_R = index4_F = 0;
                text4.text = index4.ToString("0000");
                S4_B.gameObject.SetActive(true);
                S4_R.gameObject.SetActive(true);
                S4_F.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s4 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s4 = "";
            }
            else if (count == 5)
            {
                //5L
                rocketCount--;
                rocketWeight -= index5L;
                index5L = 0;
                text5L.text = index5L.ToString("0000");
                S5L.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s5L = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s5L = "";
            }
            else if (count == 6)
            {
                //5
                fuleCount--;
                fuleWeight -= index5;
                index5 = 0;
                text5.text = index5.ToString("0000");
                S5.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s5 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s5 = "";
            }
            else if (count == 7)
            {
                //5r
                rocketCount--;
                rocketWeight -= index5R;
                index5R = 0;
                text5R.text = index5R.ToString("0000");
                S5R.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s5R = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s5R = "";
            }
            else if (count == 8)
            {
                //6
                bomWeight -= index6_B;
                rocketWeight -= index6_R;
                fuleWeight -= index6_F;

                if (index6_B > 0) { bomCount--; }
                if (index6_R > 0) { rocketCount--; }
                if (index6_F > 0) { fuleCount--; }

                index6 = index6_B = index6_R = index6_F = 0;
                text6.text = index6.ToString("0000");
                S6_B.gameObject.SetActive(true);
                S6_R.gameObject.SetActive(true);
                S6_F.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s6 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s6 = "";

            }
            else if (count == 9)
            {
                //7
                missileWeight -= index7_M;
                bomWeight -= index7_B;
                rocketWeight -= index7_R;

                if (index7_M > 0) { missileCount--; }
                if (index7_B > 0) { bomCount--; }
                if (index7_R > 0) { rocketCount--; }

                index7 = index7_M = index7_B = index7_R = 0;
                text7.text = index7.ToString("0000");
                S7_M.gameObject.SetActive(true);
                S7_B.gameObject.SetActive(true);
                S7_R.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s7 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s7 = "";

            }
            else if (count == 10)
            {
                //8
                missileCount--;
                missileWeight -= index8;
                index8 = 0;
                text8.text = index8.ToString("0000");
                S8.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s8 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s8 = "";
            }
            else if (count == 11)
            {
                //9
                missileCount--;
                missileWeight -= index9;
                index9 = 0;
                text9.text = index8.ToString("0000");
                S9.gameObject.SetActive(true);
                if (player) masterSpawn.ally_spawnPlanes[countScriptableObject].s9 = "";
                else masterSpawn.adversary_spawnPlanes[countScriptableObject].s9 = "";
            }
            UpdatePlayload();
        }

        public void UpdatePlayload()
        {
            totalWeight = missileWeight + bomWeight + rocketWeight + fuleWeight + otherWeight;
            totalLoad.text = (totalWeight).ToString("00000") + "/10800(Lbs)";
            missileCounting.text = missileCount.ToString("00");
            PlayerPrefs.SetInt("EnemyMissileCount", int.Parse(missileCounting.text));
            missileLoad.text = missileWeight.ToString("0000");

            bomCounting.text = bomCount.ToString("00");
            PlayerPrefs.SetInt("EnemyBombCount", int.Parse(bomCounting.text));
            bomLoad.text = bomWeight.ToString("0000");

            rocketCounting.text = rocketCount.ToString("00");
            PlayerPrefs.SetInt("EnemyRocketCount", int.Parse(rocketCounting.text));
            rocketLoad.text = rocketWeight.ToString("0000");

            fuleCounting.text = fuleCount.ToString("00");
            fuleLoad.text = fuleWeight.ToString("0000");
            otherLoad.text = otherWeight.ToString("0000");
        }

        private void OnDisable()
        {
            PlayerPrefs.Save();
        }
        public void Next()
        {
            if (countScriptableObject > masterSpawn.adversary_spawnPlanes.Count)
            {
                countScriptableObject++;
            }
            else { GetComponent<SwitchScene>().ChangeScene(); }
        }
    }
}