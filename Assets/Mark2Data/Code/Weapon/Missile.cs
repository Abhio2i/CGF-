using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.MissileAction
{
    public class Missile : MonoBehaviour
    {
        public GameObject explosionEffect;
        public GameObject hitEffect;
        private float speed;
        private void Start()
        {
            speed = 300f;
            Destroy(gameObject,5f);
            Invoke(nameof(ExplosionEffect), 4.5f);

        }
        private void Update()
        {
            if (gameObject)
            {
                transform.position += speed * Time.deltaTime * transform.forward;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground")){
               
                ExplosionEffect(); Destroy(this.gameObject,.3f);
            }
        }
        private void ExplosionEffect()
        {
            GameObject particles = (GameObject)Instantiate(explosionEffect);
            particles.transform.parent = this.transform;
            particles.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            particles.transform.localScale = new UnityEngine.Vector3(10,10,10);
            Destroy(particles, 5f);
        }
    }
}

