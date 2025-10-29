using Assets.Scripts.Feed_Scene.newFeed;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vazgriz.Plane;
using Weapon.MissileAction;
using Plane = Vazgriz.Plane.Plane;

public class HitDestroy : MonoBehaviour
{
    public enum playerType
    {
        Player,
        Ally,
        Adversary
    }

    
    public playerType type = playerType.Player;
    public GameObject exploPrefab;
    public float hitImpulse = 24000f;
    public float SmokeTime = 180f;
    public bool Finished = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Finished)
        {

            // Get the collision force from the contact point
            Vector3 force = collision.impulse;

            // You can also get the relative velocity at the collision point
            Vector3 relativeVelocity = collision.relativeVelocity;
            float impl = hitImpulse;
            if (!collision.transform.name.Contains("TejasLca"))
            {
                impl = collision.transform.tag.ToLower().Contains("ground") || collision.transform.tag.ToLower().Contains("plane") || collision.transform.tag.ToLower().Contains("ground") || collision.transform.tag.ToLower().Contains("player") ? hitImpulse : 24000f;
            }

            if (force.magnitude > impl)
            {
                /*
                //SaveRoutine(gameObject);
                if(type == playerType.Player)
                {

                    //GameManager.isPlayerDestroyed = true;
                }
                else
                if(type == playerType.Ally)
                {
                    PlayerPrefs.SetInt("AllyCount2", PlayerPrefs.GetInt("AllyCount2") - 1);
                }
                else
                if (type == playerType.Adversary)
                {
                    PlayerPrefs.SetInt("EnemyCount2", PlayerPrefs.GetInt("EnemyCount2") - 1);
                }
                */

                string targt = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(collision.gameObject);
                targt = targt == null ? (collision.gameObject == null ? " Expolsion" : (collision.gameObject.name.Contains("(Clone)") ? collision.gameObject.name : "entity")) : targt;
                string By = FindObjectOfType<LoadSystemAndSpawnPlanes>().findCraftType(gameObject);
                FeedBackRecorderAndPlayer.AddEvent(By + " hitt by " + targt);

                if (TryGetComponent<Plane>(out Plane plane))
                {
                    plane.enabled = false;
                }
                if (collision.gameObject.tag.ToLower().Contains("ground"))
                {
                    var obj = Instantiate(exploPrefab, transform.position, transform.rotation);
                    Destroy(obj.gameObject, SmokeTime);
                }
                StartCoroutine(destroy());
                Finished = true;
                GetComponent<Rigidbody>().isKinematic = true;
                //gameObject.SetActive(false);

            }
            // Do something with the collision force or relative velocity
            Debug.Log("Collision Force: " + force.magnitude);

        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);           
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
