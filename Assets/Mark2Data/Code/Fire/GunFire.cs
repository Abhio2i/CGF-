using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fire.Gun
{
    public class GunFire : MonoBehaviour
    {
        public ParticleSystem gunMaze;
        PlayerControle gunInput;
        private void OnEnable()
        {
            gunInput.FireGun.Enable();

        }

        private void OnDisable()
        {
            gunInput.FireGun.Disable();
        }
        void Awake()
        {
            gunInput = new PlayerControle();
            gunInput.FireGun.Fire.performed += ctx => Shoot();
        }

        
        void Shoot()
        {
            print("shoot");
            gunMaze.Play();
        }
    }
}

