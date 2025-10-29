using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using newSaveSystem;
using Utility.DropDownData;
using TMPro;

namespace Utility.Counter
{
    public class CounterMeasuers : MonoBehaviour
    {
        public MasterSpawn masterSpawn;
        public bool Enemy = false;
        public Toggle cockpit;
        public Toggle gun;
        public Toggle flares;
        public Toggle chaffs;
        public Toggle jammers;
        public Toggle towedDecoys;
        public TMP_InputField gunt;//, flaret, chafft;
        private string sub = "";
        public int countScriptableObject = 0;
        public EnemyDropDownMenu ui;
        private int gunW, gunW2, flaresW, chaffsW, jammersW, towedDecoysW;
        public bool player;
        private void Start()
        {
            if (player && countScriptableObject == 0) 
            { 
                cockpit.gameObject.SetActive(true);
                flares.GetComponentInChildren<Text>().text = "Auto Flares";
                chaffs.GetComponentInChildren<Text>().text = "Auto Chaffs";
                flares.gameObject.SetActive(true);
                chaffs.gameObject.SetActive(true);
            }
            ui = GetComponent<EnemyDropDownMenu>();
            gunW = gunW2 = flaresW = chaffsW = jammersW = towedDecoysW = 0;
            //sub = Enemy ? "Enemy" : "";
            //PlayerPrefs.SetInt(sub + "flares", 0);
            //PlayerPrefs.SetInt(sub + "chaffs", 0);          
            gun.onValueChanged.AddListener(delegate
            {
                if (gun.isOn)
                {
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].gun = true;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].gun = true;
                    gunW = 100; updateWieght();
                    gunt.interactable = true;
                }
                else
                {
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].gun = false;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].gun = false;
                    gunW = 0; updateWieght();
                    gunt.interactable = false;
                }
            });

            flares.onValueChanged.AddListener(delegate
            {
                if (flares.isOn)
                {
                    //flaret.interactable = true;
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].flares = true;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].flares = true;
                }
                else
                {
                    //flaret.interactable = false;
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].flares = false;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].flares = false;
                }
            });

            chaffs.onValueChanged.AddListener(delegate
            {
                if (chaffs.isOn)
                {
                    //chafft.interactable = true;
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].chaffs = true;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].chaffs = true;
                }
                else
                {
                    //chafft.interactable = false;
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].chaffs = false;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].chaffs = false;
                }
            });

            jammers.onValueChanged.AddListener(delegate
            {
                if (jammers.isOn)
                {
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].jammer = true;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].jammer = true;
                }
                else
                {
                    if (player)
                        masterSpawn.ally_spawnPlanes[countScriptableObject].jammer = false;
                    else
                        masterSpawn.adversary_spawnPlanes[countScriptableObject].jammer = false;
                }
            });
            if (cockpit != null)
            {
                cockpit.onValueChanged.AddListener(delegate
                {
                    if (cockpit.isOn)
                    {
                        //PlayerPrefs.SetInt("cockpit", 0);
                        SavingSystem.cockpitView = 1;
                    }
                    else
                    {
                        //PlayerPrefs.SetInt("cockpit", 1);
                        SavingSystem.cockpitView = 0;
                    }
                });
            }
            towedDecoys.onValueChanged.AddListener(delegate
            {
                if (towedDecoys.isOn)
                {
                    PlayerPrefs.SetInt(sub + "towed", 1);
                }
                else
                {
                    PlayerPrefs.SetInt(sub + "towed", 0);
                }
            });
        }
        public void ChaffCount(string s)
        {
            int count = 0;
            int.TryParse(s, out count);
            if (count != 0)
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].chaffCount = count;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].chaffCount = count;
                chaffsW = (int)(count * 0.1f); updateWieght();
            }
            else
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].chaffCount = 0;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].chaffCount = 0;
            }
        }
        public void FlareCount(string s)
        {
            int count = 0;
            int.TryParse(s, out count);
            if (count != 0)
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].flareCount = count;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].flareCount = count;
                flaresW = (int)(count * 0.1f); updateWieght();
            }
            else
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].flareCount = 0;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].flareCount = 0;
            }
        }
        public void FlareBurst(string s)
        {
            int count = 0;
            int.TryParse(s, out count);
            if (count != 0)
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].flareBurst = count;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].flareBurst = count;
            }
            else
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].flareBurst = 24;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].flareBurst = 24;
            }
        }
        public void ChaffBurst(string s)
        {
            int count = 0;
            int.TryParse(s, out count);
            if (count != 0)
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].chaffBurst = count;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].chaffBurst = count;
            }
            else
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].chaffBurst = 24;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].chaffBurst = 24;
            }
        }
        public void GunCount(string s)
        {
            int count = 0;
            int.TryParse(s, out count);
            if (count != 0)
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].gunAmmo = count;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].gunAmmo = count;
                gunW2 += (int)(count * 0.1f); updateWieght();
            }
            else
            {
                if (player)
                    masterSpawn.ally_spawnPlanes[countScriptableObject].gunAmmo = 0;
                else
                    masterSpawn.adversary_spawnPlanes[countScriptableObject].gunAmmo = 0;
            }
        }
        public void updateWieght()
        {
            ui.otherWeight = gunW + gunW2 + flaresW + chaffsW + jammersW + towedDecoysW;
            ui.UpdatePlayload();
        }
    }
}