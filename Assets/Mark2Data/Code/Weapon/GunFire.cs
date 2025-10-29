using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.BulletAction;
using newSaveSystem;
using TMPro;
using Assets.Scripts.Utility;

namespace Weapon.Gun
{
    public class GunFire : MonoBehaviour
    {
        public GameObject bulletPartical;
        public GameObject projectile;
        public GameObject bulletPref;
        public GameObject airplane;
        public SilantroGun gun;
        public float bulletForce;
        
        PlayerControle gunInput;
        private bool gunEnabled = false;
        private float bulletCOunt = 150f;
        private float gunIsActive;

        [SerializeField] TMP_Text bulletCount;
        [SerializeField] PlaneDataRecorder data;
        [SerializeField] Clock clock;
        private void OnEnable()
        {
            gunInput.Enable();
        }

        private void OnDisable()
        {
            gunInput.Disable();
        }

        private void Awake()
        {
            gunInput = new PlayerControle();
            gunIsActive = SavingSystem.gun;//PlayerPrefs.GetInt("gun");
            if (gunIsActive == 1)
            {
                bulletCount.text = bulletCOunt.ToString();
                PlayerPrefs.SetInt("ammoCount", (int)bulletCOunt);
            }
        }

        private void Start()
        {
            gunInput.FireGun.Fire.started += ctx => gunEnabled = true;
            gunInput.FireGun.Fire.started += ctx => once = true;
            gunInput.FireGun.Fire.canceled += ctx => gunEnabled = false;

            gunInput.FireGun.Fire2.started += ctx => gunEnabled = true;
            gunInput.FireGun.Fire2.started += ctx => once = true;
            gunInput.FireGun.Fire2.canceled += ctx => gunEnabled = false;

        }
        private void FixedUpdate()
        {
            bulletCount.text = gun.currentAmmo.ToString();
            if (gunIsActive != 1)
                return;
            if (gunEnabled && bulletCOunt>=0)
            {
                Shoot();
            }
            if(gunEnabled && bulletCOunt <= 0 && once)
            {
                data.messages.Add(NewClock.time + " No Ammo");
                once = false;
            }
            
        }
        bool once;
        void Shoot()
        {
            //gunEnabled = false;
            GameObject _particle = SpawnParticle();
            StartCoroutine(SpawnBullet());
            Destroy(_particle,.1f);
            data.messages.Add(NewClock.time + " Gun Fired");
        }

        private IEnumerator SpawnBullet()
        {
            yield return new WaitForSeconds(.2f);
            GameObject bullet = (GameObject)Instantiate(bulletPref);
            if (bulletCOunt >= 0)
            {
                bulletCount.text = bulletCOunt.ToString();
                PlayerPrefs.SetInt("ammoCount", (int)bulletCOunt);
            }
            bulletCOunt--;
            bullet.transform.SetPositionAndRotation(airplane.transform.position, airplane.transform.rotation);
            
            var bul = bullet.AddComponent<Bullet>();
            bul.particalSpawn = bulletPartical;
        }

        private GameObject SpawnParticle()
        {
            GameObject particles = (GameObject)Instantiate(projectile);
            particles.transform.parent = airplane.transform;
            
            particles.transform.SetPositionAndRotation(airplane.transform.position, airplane.transform.rotation);
            projectile.transform.localScale = new UnityEngine.Vector3(3,3,3);

            UnityEngine.Vector3 rot = particles.transform.eulerAngles;
            rot.y += -90f;
            particles.transform.rotation = Quaternion.Euler(rot);

            return particles;
        }
    }
}

