using Assets.Scripts.Feed_Scene.newFeed;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombScript : MonoBehaviour
{
    public GameObject prefab;
    public float power = 0f;
    public float radius = 0f;
    public float collisonForce = 24000;

    private void OnCollisionEnter(Collision collision)
    {
        // Get the collision force from the contact point
        Vector3 force = collision.impulse;

        // You can also get the relative velocity at the collision point
        Vector3 relativeVelocity = collision.relativeVelocity;
        if (force.magnitude > collisonForce) { 
            var obj = Instantiate(prefab);
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            explosion(transform.position);
            //Destroy(obj,4f);
            gameObject.SetActive(false);
            //Destroy(gameObject, 0.01f);
        }
    }

    public void explosion(Vector3 pos)
    {
        Vector3 explosionPos = pos;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
            if (hit.name.ToLower().Contains("tejaslca"))
            {
                //hit.gameObject.SetActive(false);
                //SaveRoutine(hit.gameObject);
                //GameManager.isPlayerDestroyed = true;
                Debug.Log("tejas");
            }
            else
            if(hit.tag.ToLower().Contains("enemy")|| hit.tag.ToLower().Contains("player")|| hit.tag.ToLower().Contains("ally"))
            {
                hit.gameObject.SetActive(false);
                //Destroy(hit.gameObject);
            }
        }
    }
    void SaveRoutine(GameObject gameObject)
    {
        if (gameObject.GetComponent<SaveEntityDatas>())
        {
            Save.saveFunctionsCount--;
            gameObject.GetComponent<SaveEntityDatas>().SaveFinalData();
        }
    }
}
