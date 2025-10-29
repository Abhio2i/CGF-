using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissileHeatSystem : MonoBehaviour
{
    public float BaseTempreture = 40f;
    public float Temprature = 40f;
    public float MaxTemprature = 100f;
    public float HeatExpendationFactor = 1f;
    public bool Blinded;

    public Transform ExplosionPrefabs;
    public Transform ExplosionParticle;
    public missileNavigation navig;
    private void Start()
    {
        navig = GetComponent<missileNavigation>();
    }

    void Update()
    {
        if(navig.NavigationType != missileNavigation.mode.Infrared) { return;}
        if(Temprature > MaxTemprature)
        {
            Blinded = true;
            navig.NavigationType = missileNavigation.mode.None;
        }
        if(Temprature > 40f)
        {
            Temprature -= 20f*Time.deltaTime;   //Setting back to normal
        }
        if (Temprature < MaxTemprature)
        {
            Blinded = false;
        }
        
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            destroyMissile();
        }
    }
    */
    public void destroyMissile()
    {
        if (ExplosionPrefabs)
        {
            var obj = Instantiate(ExplosionPrefabs, transform.position + new Vector3(0f, -5f, 0f), Quaternion.identity);
            Destroy(obj.gameObject, 2f);
        }
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
    public void ApplyHeat(float temp)
    {
        //if(Temprature > 90)
        //    Temprature = BaseTempreture + 10;
        Temprature += temp * HeatExpendationFactor;   
    }


    //public void ExplosiveParticles()        //just for the explosion animation
    //{
    //    if (!ExplosionParticle) return;
    //    Vector3 explosenPos = transform.position;
    //    var ExplosionRadius = 10f;
    //    var ExplosionPower = 1000f;
    //    var obj = Instantiate(ExplosionParticle, transform.position , Quaternion.identity);
    //    Destroy(obj.gameObject, 4f);
    //    Collider[] colliders = Physics.OverlapSphere(explosenPos, ExplosionRadius);
    //    foreach(Collider Hit in colliders)
    //    {
    //        Rigidbody rb = Hit.GetComponent<Rigidbody>();
    //        if (rb != null)
    //        {
    //           rb.AddExplosionForce(Random.Range(ExplosionPower, ExplosionPower+200), explosenPos, ExplosionRadius, 3f);
    //        }
    //    }
    //}
}
