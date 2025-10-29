using System;
using System.Collections.Generic;
using UnityEngine;
namespace Utility.Weapons_Data
{
    #region Station_1
    public enum Station_1_Weight
    {
        _330 = 1,
        _190 = 2,
    }
    #endregion
    #region Station_2

    public enum Station_2_Weight
    {
        _330 = 1,
        _190 = 2,
    }
    #endregion
    #region Station_3
    public enum Station_3M_Weight
    {
        _330,
        _190,
    }
    public enum Station_3B_Weight
    {
        _950,
        _910,
        _1980,
        _500,
        _530,//4,6
        _560,
        _2040 = 7,
        _25
    }
    public enum Station_3R_Weight
    {
        _520
    }
    #endregion
    #region Station_4
    public enum Station_4B_Weight
    {
        _950,
        _910,
        _1980,
        _500,
        _530,//4,6
        _560,
        _2040 = 7,
        _25
    }
    public enum Station_4R_Weight
    {
        _520
    }
    public enum Station_4F_Weight
    {
        _2580
    }
    #endregion
    #region Station_5L
    public enum Station_5L_Weight
    {
        _660
    }
    #endregion
    #region Station_5
    public enum Station_5_Weight
    {
        _2110
    }
    #endregion
    #region Station_5R
    public enum Station_5R_Weight
    {
        _660
    }
    #endregion
    #region Station_6
    public enum Station_6B_Weight
    {
        _950,
        _910,
        _1980,
        _500,
        _530,//4,6
        _560,
        _2040 = 7,
        _25
    }
    public enum Station_6R_Weight
    {
        _520
    }
    public enum Station_6F_Weight
    {
        _2580
    }
    #endregion
    #region Station_7
    public enum Station_7M_Weight
    {
        _330,
        _190,
    }
    public enum Station_7B_Weight
    {
        _950,
        _910,
        _1980,
        _500,
        _530,//4,6
        _560,
        _2040 = 7,
        _25
    }
    public enum Station_7R_Weight
    {
        _520
    }
    #endregion
    #region Station_8
    public enum Station_8_Weight
    {
        _330 = 1,
        _190 = 2,
    }
    #endregion
    #region Station_9
    public enum Station_9_Weight
    {
        _330 = 1,
        _190 = 2,
    }
    #endregion
    public class WeaponsData : MonoBehaviour
    {
        public List<string> _1;
        public List<int> _1weight;
        public List<string> _2;
        public List<int> _2weight;
        public List<string> _3M;
        public List<int> _3Mweight;
        public List<string> _3B;
        public List<int> _3Bweight;
        public List<string> _3R;
        public List<int> _3Rweight;
        public List<string> _4B;
        public List<int> _4Bweight;
        public List<string> _4R;
        public List<int> _4Rweight;
        public List<string> _4F;
        public List<int> _4Fweight;
        public List<string> _5L;
        public List<int> _5Lweight;
        public List<string> _5;
        public List<int> _5weight;
        public List<string> _5R;
        public List<int> _5Rweight;
        public List<string> _6B;
        public List<int> _6Bweight;
        public List<string> _6R;
        public List<int> _6Rweight;
        public List<string> _6F;
        public List<int> _6Fweight;
        public List<string> _7M;
        public List<int> _7Mweight;
        public List<string> _7B;
        public List<int> _7Bweight;
        public List<string> _7R;
        public List<int> _7Rweight;
        public List<string> _8;
        public List<int> _8weight;
        public List<string> _9;
        public List<int> _9weight;

        private void Awake()
        {
            PlayerPrefs.SetString("s8", "");
            PlayerPrefs.SetString("s7", "");
            PlayerPrefs.SetString("s6", "");
            PlayerPrefs.SetString("s5", "");
            PlayerPrefs.SetString("s4", "");
            PlayerPrefs.SetString("s3", "");
            PlayerPrefs.SetString("s2", "");
            PlayerPrefs.SetString("s1", "");
            PlayerPrefs.SetInt("s8", 0);
            PlayerPrefs.SetInt("s7", 0);
            PlayerPrefs.SetInt("s6", 0);
            PlayerPrefs.SetInt("s5", 0);
            PlayerPrefs.SetInt("s4", 0);
            PlayerPrefs.SetInt("s3", 0);
            PlayerPrefs.SetInt("s2", 0);
            PlayerPrefs.SetInt("s1", 0);
            Station1Data();
        }

        private void Station1Data()
        {
            //_1 = new List<string> ()
            //{
            //    "R77",
            //    "Astra Mark 1",
            //    "MICA-EM",
            //    "Meteor",
            //    "AIM-9X",
            //    "CAP-9M"
            //};

            //_2 = new List<string>()
            //{
            //    "R77",
            //    "Astra Mark 1",
            //    "MICA-EM",
            //    "Meteor",
            //    "AIM-9X",
            //    "CAP-9M"
            //};

            //_3M = new List<string>()
            //{
            //    "R77",
            //    "Astra Mark 1",
            //    "MICA-EM",
            //    "Meteor",
            //    "AIM-9X",
            //    "CAP-9M"
            //};
            //_3B = new List<string>()
            //{
            //    "CBU-87x1-3",
            //    "CBU-97x1-3",
            //    "GBU-10",
            //    "GBU-12",
            //    "MK-82x1-3",
            //    "MK-82_SEx1-3",
            //    "MK-82_AIRx1-3",
            //    "MK-84",
            //    "BDU-33 x3"
            //};
            //_3R = new List<string>()
            //{
            //    "LAU-3 MK151 HEx19",
            //    "LAU-3 MK156 HEx19",
            //    "LAU-3 MK 5 HEATx19",
            //    "LAU-3 MK61 WPx19",
            //    "LAU-3 WTU-1/B WPx19"
            //};

            //_4B = new List<string>() 
            //{
            //    "CBU-87x1-3",
            //    "CBU-97x1-3",
            //    "GBU-10",
            //    "GBU-12",
            //    "MK-82x1-3",
            //    "MK-82_SEx1-3",
            //    "MK-82_AIRx1-3",
            //    "MK-84",
            //    "BDU-33 x3"
            //};
            //_4R = new List<string>()
            //{
            //    "LAU-3 MK151 HEx19",
            //    "LAU-3 MK156 HEx19",
            //    "LAU-3 MK 5 HEATx19",
            //    "LAU-3 MK61 WPx19",
            //    "LAU-3 WTU-1/B WPx19"
            //};
            //_4F = new List<string>()
            //{
            //    "370(Gallons)"
            //};

            //_5L = new List<string>()
            //{
            //    "TGP"
            //};
            //_5 = new List<string>()
            //{
            //    "300(Gallons)"
            //};
            //_5R = new List<string>()
            //{
            //    "TGP"
            //};

            //_6B = new List<string>()
            //{
            //    "CBU-87x1-3",
            //    "CBU-97x1-3",
            //    "GBU-10",
            //    "GBU-12",
            //    "MK-82x1-3",
            //    "MK-82_SEx1-3",
            //    "MK-82_AIRx1-3",
            //    "MK-84",
            //    "BDU-33 x3"
            //};
            //_6R = new List<string>()
            //{
            //    "LAU-3 MK151 HEx19",
            //    "LAU-3 MK156 HEx19",
            //    "LAU-3 MK 5 HEATx19",
            //    "LAU-3 MK61 WPx19",
            //    "LAU-3 WTU-1/B WPx19"
            //};
            //_6F = new List<string>()
            //{
            //     "370(Gallons)"
            //};

            //_7M = new List<string>()
            //{
            //     "R77",
            //    "Astra Mark 1",
            //    "MICA-EM",
            //    "Meteor",
            //    "AIM-9X",
            //    "CAP-9M"
            //};
            //_7B = new List<string>()
            //{
            //    "CBU-87x1-3",
            //    "CBU-97x1-3",
            //    "GBU-10",
            //    "GBU-12",
            //    "MK-82x1-3",
            //    "MK-82_SEx1-3",
            //    "MK-82_AIRx1-3",
            //    "MK-84",
            //    "BDU-33 x3"
            //};
            //_7R = new List<string>()
            //{
            //    "LAU-3 MK151 HEx19",
            //    "LAU-3 MK156 HEx19",
            //    "LAU-3 MK 5 HEATx19",
            //    "LAU-3 MK61 WPx19",
            //    "LAU-3 WTU-1/B WPx19"
            //};

            //_8 = new List<string>()
            //{
            //    "R77",
            //    "Astra Mark 1",
            //    "MICA-EM",
            //    "Meteor",
            //    "AIM-9X",
            //    "CAP-9M"
            //};

            //_9 = new List<string>()
            //{
            //    "R77",
            //    "Astra Mark 1",
            //    "MICA-EM",
            //    "Meteor",
            //    "AIM-9X",
            //    "CAP-9M"
            //};
        }
    }
}

