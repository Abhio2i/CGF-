using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Vazgriz.Plane;

namespace Assets.Code.UI
{
    public class WeaponTracker : MonoBehaviour
    {
        public int gunAmmo = 0;
        public SilantroGun gun;
        public int WeaponType = -1;
        public int missileCount=0, bombCount =0;
        public MasterSpawn masterSpawn;
        public MissionPlan missionPlan;
        public List<GameObject> HardPointsMissileUI, HardPointBombUI;
        public List<Toggle> HardPointsMissileUIToggle, HardPointBombUIToggle;
        public string[] s = new string[8];
        public List<MissileInfo> missileInfos = new List<MissileInfo>();
        public Transform[] HardPoints;
        public GameObject MissileDummyPrefab,BombDummy, WarningText, WarningTextBomb;
        public GameObject[] MissilesList, BombsList;
        public GameObject[] MissileTogleList;
        public GameObject[] BombTogleList;
        public List<GameObject> Missiles = new List<GameObject>();
        public List<GameObject> Bombs = new List<GameObject>();
        public TextMeshProUGUI MissileIndication;
        PlayerControle gunInput;
        public UttamRadar radar;
        public float MaxMissile = 0;
        public float hit = 0;
        public GameObject destroyby;
        public int select = 0;
        public bool mSelect = false;
        public int nSelect = 0; 
        public int last = 0;
        public bool gunfire = false;
        public delegate void autoSelectMissile();
        public float WeaponSelect = 0;
        private void OnEnable()
        {
            gunInput.Enable();
        }

        private void OnDisable()
        {
            gunInput.Disable();
        }

        public void Awake()
        {
            gunInput = new PlayerControle();
        }
        public void Start()
        {
            radar =  gun.transform.root.GetComponentInChildren<UttamRadar>();
            //s[7] = masterSpawn.ally_spawnPlanes[0].s9;//1st missile
            //s[6] = masterSpawn.ally_spawnPlanes[0].s8;//2nd missile/Bomb
            //s[5] = masterSpawn.ally_spawnPlanes[0].s7;//3rd Bomb
            //s[4] = masterSpawn.ally_spawnPlanes[0].s6;//4th Bomb/Fuel
            //s[3] = masterSpawn.ally_spawnPlanes[0].s4;//5th Bomb/Fuel
            //s[2] = masterSpawn.ally_spawnPlanes[0].s3;//3rd missile
            //s[1] = masterSpawn.ally_spawnPlanes[0].s2;//4th missile
            //s[0] = masterSpawn.ally_spawnPlanes[0].s1;//5th missile

            s[7] = missionPlan.ally_spawnPlanes[0].Hardpoints[7];//1st missile
            s[6] = missionPlan.ally_spawnPlanes[0].Hardpoints[5];//2nd missile/Bomb
            s[5] = missionPlan.ally_spawnPlanes[0].Hardpoints[3];//3rd Bomb
            s[4] = missionPlan.ally_spawnPlanes[0].Hardpoints[1];//4th Bomb/Fuel
            s[3] = missionPlan.ally_spawnPlanes[0].Hardpoints[0];//5th Bomb/Fuel
            s[2] = missionPlan.ally_spawnPlanes[0].Hardpoints[2];//3rd missile
            s[1] = missionPlan.ally_spawnPlanes[0].Hardpoints[4];//4th missile
            s[0] = missionPlan.ally_spawnPlanes[0].Hardpoints[6];//5th missile

            foreach (MissileInfo info in missionPlan.ally_spawnPlanes[0].missileInfo)
            {
                MissileInfo inf = new MissileInfo(info);
                missileInfos.Add(inf);
            }

            SetDummies();
             
 
            gunInput.FireGun.LockTarget.performed += ctx => { radar.ManualLockTarget(); AutoMissileSelect(); };
            gunInput.FireGun.BreakLockTarget.performed += ctx => { MissileDSelect(); radar.ManualBreakLockTarget(); };
            gunInput.FireMissile.WeaponSelectDown.performed += ctx => { WeaponSelect = -1; };
            gunInput.FireMissile.WeaponSelectDown.canceled += ctx => { WeaponSelect = 0; };
            gunInput.FireMissile.WeaponSelectUp.performed += ctx => { WeaponSelect = 1; };
            gunInput.FireMissile.WeaponSelectUp.canceled += ctx => { WeaponSelect = 0; };
            radar.weaponTracker = this;
            if (true)
            {
                gunAmmo =  missionPlan.ally_spawnPlanes[0].Bullets;
                gun.ammoCapacity = gunAmmo;
                gunInput.FireGun.Fire.started += ctx => { gunfire = true; gun.running = true; };
                gunInput.FireGun.Fire2.started += ctx => { gunfire = true; gun.running = true; };
                gunInput.FireGun.Fire2.canceled += ctx => { gunfire = false; gun.running = false; };
                gunInput.FireGun.Fire.canceled += ctx => { gunfire = false; gun.running = false; };
            }
            //else
            //{
            //    gunAmmo = 0;
            //    gun.ammoCapacity = gunAmmo;
            //}

            foreach(GameObject g in HardPointsMissileUI)
            {
                HardPointsMissileUIToggle.Add(g.GetComponent<Toggle>());
            }

            foreach (GameObject g in HardPointBombUI)
            {
                HardPointBombUIToggle.Add(g.GetComponent<Toggle>());
            }

        }
        public void AutoMissileSelect()
        {
            if (!radar.missileSelect)
            {
                int i = 0;
                foreach(GameObject g in HardPointsMissileUI)
                {
                    if (g.activeSelf)
                    {
                        if (Missiles[i].TryGetComponent<missileNavigation>(out missileNavigation navig))
                        {
                            if ((navig.missileType == missileNavigation.RangeType.BVR && WeaponType == 1) || (navig.missileType == missileNavigation.RangeType.CCM && WeaponType == 2) || WeaponType == -1)
                            {
                                g.GetComponent<Toggle>().isOn = true;
                                break;
                            }
                        }

                    }
                    else
                    if (HardPointBombUI[i].activeSelf)
                    {
                        if (Bombs[i].TryGetComponent<bombScript>(out bombScript bomb))
                        {
                            if (WeaponType == 3)
                            {
                                g.GetComponent<Toggle>().isOn = true;
                                break;
                            }
                        }
                    }
                    i++;
                }
            }
        }

        public void MissileDSelect()
        {
            if (radar.missileSelect)
            {
                int i = 0;
                foreach (GameObject g in HardPointsMissileUI)
                {
                    if (g.activeSelf)
                    {
                        g.GetComponent<Toggle>().isOn = false;
                    }
                    i++;
                }
            }
        }





        private void FixedUpdate()
        {
            if (gunfire)
            {
                gun.FireGun();
            }

            float v = WeaponSelect;//Input.GetAxis("Weapon Select"); 
            if (v != last && v!=0)
            {
                if (v < 0)
                {
                    select--;
                    if (select < 0) { select = 0; }
                }
                else if (v > 0)
                {
                    select++;
                    if (select > s.Length - 1) { select = s.Length - 1; }
                }
                for(int i = 0; i < s.Length; i++)
                {
                    if (select == i)
                    {
                        nSelect = select;
                        HardPointBombUIToggle[i].isOn = true;
                        HardPointsMissileUIToggle[i].isOn = true;
                        foreach (GameObject m in MissilesList)
                        {
                            if (m.name.ToLower() == s[select].ToLower())
                            {
                                GameObject missile = Missiles[select];
                                missileEngine e = missile.GetComponent<missileEngine>();
                                radar.Vmissile = e.MaxSpeed;
                                radar.Tburn = (e.Range / e.MaxSpeed) * 3600;
                                radar.rmax2 = e.Rmax;
                                radar.rmin2 = e.Rmin;
                                radar.VRange = e.Range;

                                break;
                            }
                        }
                    }
                    else
                    {
                        HardPointBombUIToggle[i].isOn = false;
                        HardPointsMissileUIToggle[i].isOn = false;
                    }
                }
                /*
                MainPlane.GetComponentInChildren<UttamRadar>().Vmissile = missileData[i].Speed;
                MainPlane.GetComponentInChildren<UttamRadar>().Tburn = (missileData[i].Range / missileData[i].Speed) * 3600;
                MainPlane.GetComponentInChildren<UttamRadar>().rmin2 = missileData[i].rmin2;
                MainPlane.GetComponentInChildren<UttamRadar>().rmax2 = missileData[i].rmax2;
                */
            }
            last = (int) v;
            int k = 0;
            mSelect = false;
            foreach(var n in HardPointsMissileUIToggle)
            {
                if (n.gameObject.activeSelf)
                {
                    if (n.isOn)
                    {
                        nSelect = k;
                        mSelect = true;
                        break;
                    }
                }
                k++;
            }
            radar.missileSelect = mSelect;
            if(select!=nSelect)
            {
                select = nSelect;
                foreach (GameObject m in MissilesList)
                {
                    if (m.name.ToLower() == s[select].ToLower())
                    {
                        GameObject missile = Missiles[select];
                        missileEngine e = missile.GetComponent<missileEngine>();
                        radar.Vmissile = e.MaxSpeed;
                        radar.Tburn = (e.Range / e.MaxSpeed) * 3600;
                        radar.rmax2 = e.Rmax;
                        radar.rmin2 = e.Rmin;
                        radar.VRange = e.Range;
                        missileNavigation c = missile.GetComponent<missileNavigation>();
                       
                        MissileIndication.text = c.missileType.ToString() +" " + c.type + " "+(e.Range/1.852f).ToString("0")+"NM";
                        
                        break;
                    }
                    MissileIndication.text = "";
                }
            }
            if (!mSelect)
            {
                MissileIndication.text = "";
            }
            

        }


        public GameObject MissileInfo()
        {

            for (int i = 0; i < s.Length; i++)
            {
                if (HardPointsMissileUI[i].GetComponent<Toggle>().isOn && HardPointsMissileUI[i].activeSelf)
                {
                    int j = 0;
                    foreach (GameObject m in MissilesList)
                    {
                        if (m.name == s[i]) { break; }
                        j++;
                    }
                    return MissilesList[j];
                }
            }
            return null;
        }

        public GameObject UpdateMissile()
        {
            
            for (int i = 0; i < s.Length; i++)
            {
                if (HardPointsMissileUI[i].GetComponent<Toggle>().isOn && HardPointsMissileUI[i].activeSelf)
                {
                    int j = 0;
                    foreach (GameObject m in MissilesList)
                    {
                        if (m.name.ToLower() == s[i].ToLower()) { break; }
                        j++;
                    }
                    if (j>= MissilesList.Length)
                    {
                        WarningText.SetActive(true);
                        Invoke(nameof(DeactivateWarning), 3);
                        return null;
                    }
                    if(radar.HardLockTargetObject == null || !radar.HardLockTargetObject.activeSelf)
                    {
                        return null;
                    }

                    float distance = Vector3.Distance(radar.HardLockTargetObject.transform.position , radar.transform.root.position);
                    string type = Missiles[i].GetComponent<missileNavigation>().type.ToString();
                    string Rangetype = Missiles[i].GetComponent<missileNavigation>().missileType.ToString();

                    if ((radar.HardLockTargetObject.tag.Contains("EnemyTank") || radar.HardLockTargetObject.tag.Contains("EnemyShip")) && type == "ATA")
                    {
                        return null;
                    }

                    if ((radar.HardLockTargetObject.tag.Contains("EnemyPlane") || radar.HardLockTargetObject.tag.Contains("Player")) && type == "ATS")
                    {
                        return null;
                    }

                    if ((Rangetype=="BVR" && distance<30000) || (Rangetype == "CCM" && distance > 30000))
                    {
                        return null;
                    }


                    s[i] = "";
                    HardPointsMissileUI[i].SetActive(false);
                    HardPoints[i].GetChild(0).gameObject.SetActive(false);
                    //Destroy(HardPoints[i].GetChild(0).gameObject);
                    GameObject gameObt = Missiles[i];
                    gameObt.GetComponent<missileNavigation>().FiredBy = radar.transform.root.gameObject;
                    Missiles[i] = null;
                    return gameObt;
                    //return MissilesList[j];
                }
            }
            WarningText.SetActive(true);
            Invoke(nameof(DeactivateWarning), 3);
            return null;
        }
        public GameObject UpdateBomb()
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (HardPointBombUI[i].GetComponent<Toggle>().isOn && HardPointBombUI[i].activeSelf)
                {
                    int j = 0;
                    foreach (GameObject m in BombsList)
                    {
                        if (m.name == s[i]) { break; }
                        j++;
                    }
                    if (j >= BombsList.Length)
                    {
                        WarningTextBomb.SetActive(true);
                        Invoke(nameof(DeactivateWarning), 3);
                        return null;
                    }
                    s[i] = "";
                    HardPointBombUI[i].SetActive(false);
                    HardPoints[i].GetChild(0).gameObject.SetActive(false);
                    //Destroy(HardPoints[i].GetChild(0).gameObject);
                    //return BombsList[j];
                    return Bombs[i];
                }
            }
            WarningTextBomb.SetActive(true);
            Invoke(nameof(DeactivateWarning), 3);
            return null;
        }
        void DeactivateWarning()
        {
            WarningText.SetActive(false);
            WarningTextBomb.SetActive(false);
        }
        void SetDummies()
        {
            Missiles = new List<GameObject>(s.Length);
            Bombs = new List<GameObject>(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                Missiles.Add(null);
                Bombs.Add(null);
                if (s[i] != "")
                {
                    int index = 0;
                    foreach (GameObject missilename in MissilesList)
                    {
                        if (missilename.name.ToLower() == s[i].ToLower())//|| (!s[i].ToLower().Contains("empty")&& !s[i].ToLower().Contains("bomb") && !s[i].ToLower().Contains("fuel")))
                        {
                            GameObject obj = Instantiate(MissileTogleList[index]);
                            obj.SetActive(true);
                            obj.transform.SetParent(HardPointsMissileUI[i].transform.parent);
                            obj.transform.localPosition = HardPointsMissileUI[i].transform.localPosition;
                            obj.transform.localScale = Vector3.one;
                            HardPointsMissileUI[i] = obj;
                            //HardPointsMissileUI[i].SetActive(true);
                            GameObject objDummy = Instantiate(MissileDummyPrefab, HardPoints[i]);
                            GameObject.FindObjectOfType<FeedBackRecorderAndPlayer>().ExtraEntity.Add(obj.transform);

                            GameObject missile = (GameObject)Instantiate(missilename);
                            Missiles[i] = missile;
                            missileEngine missileEngine = missile.GetComponent<missileEngine>();

                            foreach (MissileInfo info in missileInfos)
                            {
                                if(info.Name.ToLower() == missilename.name.ToLower())
                                {
                                    missileEngine.MaxSpeed = info.Speed;
                                    missileEngine.Range = info.Range; 
                                    missileEngine.turnSpeed = info.TurnRadius;
                                    break;
                                }
                            }
                            MaxMissile++;
                            missile.SetActive(false);
                            FeedBackRecorderAndPlayer.Missiles.Add(missile.transform);


                            missileCount++;
                            break;
                        }
                        index++;
                    }
                    foreach (GameObject bombname in BombsList)
                    {
                        if (bombname.name == s[i]|| s[i].ToLower().Contains("bomb"))
                        {
                            GameObject obj = Instantiate(BombTogleList[0]);
                            obj.SetActive(true);
                            obj.transform.SetParent(HardPointBombUI[i].transform.parent);
                            obj.transform.localPosition = HardPointBombUI[i].transform.localPosition;
                            obj.transform.localScale = Vector3.one; 
                            HardPointBombUI[i] = obj;
                            //HardPointBombUI[i].SetActive(true);
                            GameObject objDummy = Instantiate(BombDummy, HardPoints[i]);
                            GameObject.FindObjectOfType<FeedBackRecorderAndPlayer>().ExtraEntity.Add(obj.transform);
                            GameObject bomb = (GameObject)Instantiate(bombname);
                            Bombs[i] = bomb;
                            bomb.SetActive(false);
                            FeedBackRecorderAndPlayer.Missiles.Add(bomb.transform);

                            bombCount++;
                            break;
                        }
                    }
                }
            }
        }
    }
}