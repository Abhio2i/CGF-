#region script info
// inp: The All Parameters In Weather UI manager.
//out: Stores all The Data of Weather in UI Manger and Transfer to the Land Scene.
#endregion
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace UI.Weather
{
    public class WeatherManager : MonoBehaviour
    {
        #region Control_Parameters
        [Header("Time Date")]
        public TMP_Dropdown month;


        [Header("Temp Cloud")]
        public RawImage mainCloudDisplay;

        [Header("Fog")]
        public Toggle isFog;
        public Slider fogVisibility;
        public Slider fogThickness;

        private bool fog = false;

        [Header("Dust")]
        public Toggle isDustSmoke;
        public Slider dustVisibility;
        private bool dust = false;


        #endregion

        #region Local Functions
        private void Start()
        {
            //fog_Toggle
            isFog.onValueChanged.AddListener(delegate
            {
                if (isFog.isOn)
                {
                    fog = true;
                    PlayerPrefs.SetInt("isfog", 1);
                }
                else
                {
                    fog = false;
                    PlayerPrefs.SetInt("isfog", 0);


                }
            });

            //fog_visibility
            fogVisibility.onValueChanged.AddListener(delegate
            {
                FogVisibility();
            });
            //fog_thickness
            fogThickness.onValueChanged.AddListener(delegate
            {
                FogThickness();
            });

            //dust_toggle
            isDustSmoke.onValueChanged.AddListener(delegate
            {
                if (isDustSmoke.isOn)
                {
                    dust = true;

                }
                else
                {
                    dust = false;

                }
            });

            //dust_visibility
            dustVisibility.onValueChanged.AddListener(delegate
            {
                DustVisibility();
            });
        }

        private void DustVisibility()
        {
            if (dust)
            {
                float val = (dustVisibility.value);
                PlayerPrefs.SetFloat("dustv", val);
            }
        }

        private void FogThickness()
        {
            if (fog)
            {
                float val = (fogThickness.value);
                PlayerPrefs.SetFloat("fogt", val);
            }
        }

        private void FogVisibility()
        {
            if (fog)
            {
                float val = (fogVisibility.value);
                PlayerPrefs.SetFloat("fogv", val);
            }
        }
        #endregion

        #region Global Functions
        public void OnChangeTexture(RawImage _texture)
        {
            string s = _texture.gameObject.name;
            PlayerPrefs.SetString("cloud", s);
            mainCloudDisplay.texture = _texture.texture;
        }

        public void DayChange(string s)
        {
            int val = Int16.Parse(s);
            PlayerPrefs.SetInt("day", val);
        }
        public void YearChange(string s)
        {
            int val = Int16.Parse(s);
            PlayerPrefs.SetInt("year", val);
        }
        public void HourChange(string s)
        {
            int val = Int16.Parse(s);
            if (val <= 0)
            {
                val = 0;
            }
            else if (val >= 23)
            {
                val = 23;
            }
            PlayerPrefs.SetInt("hour", val);
        }
        public void MinuteChange(string s)
        {
            int val = Int16.Parse(s);
            if (val <= 0)
            {
                val = 0;
            }
            else if (val >= 60)
            {
                val = 60;
            }
            PlayerPrefs.SetInt("min", val);
        }
        public void SecondChange(string s)
        {
            int val = Int16.Parse(s);
            if (val <= 0)
            {
                val = 0;
            }
            else if (val >= 60)
            {
                val = 60;
            }
            PlayerPrefs.SetInt("sec", val);
        }
        public void TemperatureChange(string s)
        {
            int val = Int16.Parse(s);
            if (val > -10 && val < 50)
            {
                PlayerPrefs.SetInt("temp", val);
            }
        }

        public void MonthChange(int index)
        {
            string s = month.options[index].text;
            PlayerPrefs.SetString("month", s);
            PlayerPrefs.SetInt("month", index+1);
        }

        public void SaveBtn(GameObject _obj)
        {
            _obj.SetActive(false);
        }
        #endregion
    }
}

