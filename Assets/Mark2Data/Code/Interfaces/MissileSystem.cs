using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    public GameObject missile;
    public GameObject explosion;
    public GameObject _explosion;
    public bool iAmPlayer;
    public string Enemytag;
    public bool ignoreCollision;
    public GameObject currentObject;
    public GameObject _other;
    public Transform target;
    public PlaneDataRecorder data;
    public Clock clock;
    private void Start()
    {
        if (currentObject == null)
        {
            currentObject = this.gameObject;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (ignoreCollision)
            return;
        if (other.gameObject !=this.gameObject && !other.gameObject.CompareTag(this.gameObject.tag) && !other.gameObject.CompareTag("Ground"))
        {
            _explosion=Instantiate(explosion);
            
            _explosion.transform.position=transform.position;
            
            _explosion.GetComponent<ParticleSystem>().Play();
            
            _other = other.collider.gameObject;
            
            Destroy(_explosion, 1f);
            
            IDamage damage = _other.gameObject.GetComponent<IDamage>();
            
            if (damage != null)
                damage.Count();
            
            if(iAmPlayer)
            {
                if (other.gameObject.CompareTag(Enemytag))
                {
                    int k = PlayerPrefs.GetInt("EnemyDown");
                    k++;
                    PlayerPrefs.SetInt("EnemyDown", k);
                    int current = PlayerPrefs.GetInt("EnemyCount");
                    PlayerPrefs.SetInt("EnemyCount", current - k);
                    data.messages.Add(NewClock.time + " Target Hit");
                    data.messages.Add(NewClock.time + " Enemey Down");
                    data.messages.Add(NewClock.time + " Total Enemy Down = " + k);
                }
                else
                {
                    data.messages.Add(NewClock.time + " Target Miss");
                }
            }
            Destroy(_other.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
            transform.position = Vector3.MoveTowards(transform.position, target.position, 30f);
        else
            transform.position += transform.forward * 30f;
    }
    void AfterExplosionTask()
    {
        
        Destroy(_other);
        Destroy(gameObject);
    }
}
