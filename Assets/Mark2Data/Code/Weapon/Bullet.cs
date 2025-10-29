using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.BulletAction
{
    public class Bullet : MonoBehaviour
    {
        public GameObject particalSpawn;
        private float speed;
        private void Start()
        {
            speed = 500f;
            Destroy(this.gameObject,5f);
            Invoke(nameof(SpawnPartical), 4.5f);
        }
        // Update is called once per frame
        void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))//add another tag
            {
                SpawnPartical();
                Destroy(gameObject, 0.1f);
            }
        }

        private void SpawnPartical()
        {
            GameObject particles = (GameObject)Instantiate(particalSpawn);
            particles.transform.parent = this.transform;
            particles.transform.position = this.transform.position;
            particles.transform.localScale = new UnityEngine.Vector3(5, 5, 5);
            particles.transform.rotation = this.transform.rotation;
            Destroy(particles, 0.5f);

        }
    }
}

