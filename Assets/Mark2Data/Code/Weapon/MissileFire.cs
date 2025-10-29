using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Weapon.MissileAction;
using Weapon.Gun;
using Assets.Code.UI;
using Assets.Scripts.Feed;
using Assets.Scripts.Utility;
using Assets.Scripts.Feed_Scene.newFeed;

namespace Weapon.FireMissile
{
    
    public class MissileFire : MonoBehaviour
    {
        public MasterSpawn masterSpawn;
        [SerializeField] TMP_Text enemyTakenDown;
        [SerializeField] TMP_Text missilesLeft,bombsLeft;
        [SerializeField] WeaponTracker tracker;
        PlayerControle missile;
        [SerializeField] UttamRadar uttam;
        Inputs controller;
        [SerializeField]PlaneDataRecorder data;
        [SerializeField] Clock clock;
        [SerializeField] GameObject player;
        [SerializeField] PlaneData planeData;
        string message = "Missile Fired";
        public GameObject placeMisile1;
        public GameObject placeMisile2;
        public GameObject missilePrefab;
        public GameObject Bombprefab;
        public GameObject missileExploadEffect;

        private int missilesFired = 0;
        private int dropbombs;
        private bool fireGun;
        #region NewInputManager
        private void OnEnable()
        {
            missile.Enable();
            controller.Enable();
        }
        private void OnDisable()
        {
            missile.Disable();
            controller.Disable();
        }

        private void Awake()
        {
            PlayerPrefs.SetInt("EnemyDown", 0);
            missile = new PlayerControle();
            controller = new Inputs();
        }
        private void Start()
        {
            Invoke(nameof(CheckMissiles),1f);
            //if (missilesFired != 0)
            //{
            //    PlayerPrefs.SetInt("MissileLeft", missilesFired);
            //}
            //dropbombs = PlayerPrefs.GetInt("BombCount");
            //if (missilesFired != 0)
            //{
            //    PlayerPrefs.SetInt("BombsLeft", dropbombs);
            //}

            missile.FireMissile.Fire1.started += fire => { FireMissile(placeMisile1); };
            missile.FireMissile.Fire3.started += fire => { FireMissile(placeMisile1); };
            missile.FireMissile.Fire2.started += fire => DropBomb(placeMisile1);
            missile.FireMissile.Fire4.started += fire => DropBomb(placeMisile1);
            //controller.PlaneMove.Fire.performed += fire => FireMissile(placeMisile1);
            //missile.FireMissile.Fire2.performed += fire => FireMissile(placeMisile2);
            //missile.FireMissile.Fire2.performed += fire => FireGun();
        }
        void CheckMissiles()
        {
            missilesFired = tracker.missileCount;
            missilesLeft.text = missilesFired.ToString();
            dropbombs = tracker.bombCount;
            bombsLeft.text = dropbombs.ToString();
            //if (masterSpawn.ally_spawnPlanes[0].s1 != "") { missilesFired++; }
            //if (masterSpawn.ally_spawnPlanes[0].s2 != "") { missilesFired++; }
            //if (masterSpawn.ally_spawnPlanes[0].s3 != "") { missilesFired++; }
            //if (masterSpawn.ally_spawnPlanes[0].s4 != "") { dropbombs++; }
            //if (masterSpawn.ally_spawnPlanes[0].s5 != "") { dropbombs++; }
            //if (masterSpawn.ally_spawnPlanes[0].s5L !="") { dropbombs++; }
            //if (masterSpawn.ally_spawnPlanes[0].s5R !="") { dropbombs++; }
            //if (masterSpawn.ally_spawnPlanes[0].s6 != "") { dropbombs++; }
            //if (masterSpawn.ally_spawnPlanes[0].s7 != "") { missilesFired++; }
            //if (masterSpawn.ally_spawnPlanes[0].s8 != "") { missilesFired++; }
            //if (masterSpawn.ally_spawnPlanes[0].s9 != "") { missilesFired++; }
        }
        private void LateUpdate()
        {
            enemyTakenDown.text=PlayerPrefs.GetInt("EnemyDown").ToString(); //enemyTakenDown
        }
        #endregion
        private int firingCount;

        public void mFire()
        {
            FireMissile(placeMisile1);
        }

        public void mDrop()
        {
            DropBomb(placeMisile1);
        }

        private void FireMissile(GameObject place)
        {
            Debug.Log("MISSILE fIRE");
            var missile = tracker.UpdateMissile(); //Use this index to choose missile from masterspawn
            if (missilesFired <= 0)
            {
                Debug.Log(missilesFired);
                planeData.SetMessage("Missle Empty");
                return;
            }
            if (!missile)
            {
                Debug.Log("NO Missile Selected");
                return;
            }
            missilesFired--;
            planeData.SetMessage("Missile Fired");
            PlayerPrefs.SetInt("MissileLeft", missilesFired);
            //data.messages.Add(NewClock.time + " Missile Fired");
            missilesLeft.text = missilesFired.ToString();
            
            string targt = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(uttam.HardLockTargetObject);
            FeedBackRecorderAndPlayer.AddEvent("Player Fired " + missile.name + " Missile towards " + targt);
            //GameObject _missile = (GameObject)Instantiate(missile);
            GameObject _missile = missile;
            _missile.SetActive(true);
            _missile.transform.SetPositionAndRotation(place.transform.position, place.transform.rotation);
            _missile.GetComponent<missileEngine>().fire = true;
            //_missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.None;
            _missile.GetComponent<SaveEntityDatas>().type = EntityType.Missile_Player;
            if (uttam.HardLockTargetObject != null)
            {
                _missile.GetComponent<missileNavigation>().GivenTarget = uttam.HardLockTargetObject.transform;
                _missile.GetComponent<missileNavigation>().objt.Add(uttam.HardLockTargetObject);
                //_missile.GetComponent<missileNavigation>().NavigationType = missileNavigation.mode.ActiveRadar;
            }
            //_missile.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            //var missileScript = _missile.AddComponent<Missile>();
            //missileScript.explosion = missileExploadEffect;

           // _missile.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

        }

        private void DropBomb(GameObject place)
        {
            var Bomb = tracker.UpdateBomb();
            if (dropbombs <= 0)
            {
                Debug.Log(dropbombs);
                data.messages.Add(NewClock.time + " No Bombs");
                return;
            }
            if (!Bomb)
            {
                Debug.Log("NO Bomb Selected");
                return;
            }
            dropbombs--;
            //PlayerPrefs.SetInt("BombsLeft", dropbombs);
            data.messages.Add(NewClock.time + " Bomb Drop");
            bombsLeft.text = dropbombs.ToString();
            //GameObject _bomb = (GameObject)Instantiate(Bomb);
            FeedBackRecorderAndPlayer.AddEvent("Player Drop "+Bomb.name+" Bomb");
            GameObject _bomb = Bomb;
            _bomb.SetActive(true);
            _bomb.transform.SetPositionAndRotation(place.transform.root.position +new Vector3(0,-12f,0), place.transform.rotation);
            var rb = _bomb.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = place.transform.root.forward*place.transform.root.GetComponent<Rigidbody>().velocity.magnitude;
            }
        }
    }
}

