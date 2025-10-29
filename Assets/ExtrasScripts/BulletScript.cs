using Assets.Scripts.Feed_Scene.newFeed;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 3f;
    public List<string> targetTag=new List<string>();
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.TryGetComponent<Specification>(out Specification sp))
        {
            

            var otherObj = collision.transform.root.gameObject;
            if (!otherObj.activeSelf) { return; }
            if (otherObj.tag.Contains("Player")) //Ally or Player
            {
                if (collision.gameObject.name.Contains("General"))  //Main Player
                {
                    //sp.health -= damage;
                    
                    //collision.gameObject.GetComponent<SaveEntityDatas>().SetMessage("Bullet Hit Player");
                    if (sp.health <= 0)
                    {
                        //SaveRoutine(collision.gameObject);
                    }
                    return;
                }
                if (otherObj.GetComponent<SaveEntityDatas>())
                {
                    sp.health -= damage;
                    collision.gameObject.GetComponent<SaveEntityDatas>().SetMessage("Bullet Hit " + otherObj.ToString());
                    if (sp.health <= 0)
                    {
                        //SaveRoutine(otherObj);
                        //PlayerPrefs.SetInt("AllyCount2", PlayerPrefs.GetInt("AllyCount2") - 1);
                        //Destroy(collision.gameObject, 0.5f);
                    }
                }
                
            }
            else if (otherObj.tag.Contains("EnemyPlane"))  //Enemy
            {
                
                sp.health -= damage;
                SaveEntityDatas s = collision.gameObject.GetComponent<SaveEntityDatas>();
                if(s != null)
                s.SetMessage("Missile Hit " + collision.gameObject.ToString());
                
                if (sp.health <= 0)
                {
                    SaveRoutine(collision.gameObject);
                    PlayerPrefs.SetInt("EnemyCount2", PlayerPrefs.GetInt("EnemyCount2") - 1);
                    otherObj.SetActive(false);
                    //Destroy(collision.gameObject, 0.5f);
                }
            }
            else if (otherObj.tag.Contains("EnemyShip"))  //Enemy
            {

                sp.health -= damage;
                SaveEntityDatas s = collision.gameObject.GetComponent<SaveEntityDatas>();
                if (s != null)
                    s.SetMessage("Missile Hit " + collision.gameObject.ToString());

                if (sp.health <= 0)
                {
                    SaveRoutine(collision.gameObject);
                    PlayerPrefs.SetInt("EnemyCount2", PlayerPrefs.GetInt("EnemyCount2") - 1);
                    otherObj.SetActive(false);
                    //Destroy(collision.gameObject, 0.5f);
                }
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
