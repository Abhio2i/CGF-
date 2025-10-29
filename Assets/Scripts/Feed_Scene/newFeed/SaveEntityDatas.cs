using Assets.Scripts.Feed;
using Assets.Scripts.Utility;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Feed_Scene.newFeed
{
    public enum EntityType
    { 
        Plane_Ally,Plane_Player,Plane_Adversary,Missile_Ally,Missile_Player,Missile_Adversary,EnemyWarship
    }

    public class SaveEntityDatas : MonoBehaviour
    {
        [SerializeField] PlaneData planeData;
        [SerializeField] int firstFrame, lastFrame;
        NewSaveManager manager;

        DatasToBeSaved savingData;

        string savePath,file;

        public string event_message = "";
        public EntityType type;

        int localFrameCOunt;
        float frameWait;

        [SerializeField] bool save;

        private void Awake()
        {
            savingData = new DatasToBeSaved();
            manager =    FindObjectOfType<NewSaveManager>();
            planeData = GetComponent<PlaneData>();
            frameWait=Time.fixedDeltaTime;
        }
        private void Start()
        {
            firstFrame = GameManager.frameNumber;
            savingData.firstFrame=(GameManager.frameNumber.ToString());
            savingData._NPC_type = type.ToString();
            
            file = type.ToString() + Random.Range(-10000000f,1000000f)*Time.fixedDeltaTime;
            
            //StartCoroutine(SavingDataRoutine());
        }
        private void FixedUpdate()
        {
            if (!Save.startSave) return;
            SaveEverything();
        }
        public void SaveEverything()
        {
            if (Save.save)
            {
                NewSaveManager.saveAllDatas.Add(GetComponent<SaveEntityDatas>());
                return;
            }

            if (savingData != null)
            {
                string pos = planeData.planeTransform.position.x.ToString() + "," +
                    planeData.planeTransform.position.y.ToString() + "," + planeData.planeTransform.position.z.ToString();

                string rot = planeData.planeTransform.rotation.x.ToString() + "," +
                    planeData.planeTransform.rotation.y.ToString() + "," + planeData.planeTransform.rotation.z.ToString() +
                    "," + planeData.planeTransform.rotation.w.ToString();

                savingData.storePosition.Add(pos);
                savingData.storeRosition.Add(rot);
                savingData.storeGforce.Add(planeData.g_force);
                savingData.storeMissileCount.Add(planeData.missileCount + 30f);
                savingData.storeEvents.Add(event_message);
                savingData.storeTime.Add(NewClock.time);
                savingData.speed.Add(planeData.speed);
                savingData.LatLong.Add(planeData.latLong.x.ToString()+","+ planeData.latLong.y.ToString());
                
                //print(savingData.storePosition.Count + " " + GameManager.frameNumber);
            }
        }

        IEnumerator SavingDataRoutine()
        {
            while (true)
            {
                yield return null;
                
                SaveEverything();
            }
        }

        public void SetMessage(string message)
        {
            event_message = message;
        }
        public void SaveFinalData()
        {
            savingData.lastFrame = (GameManager.frameNumber.ToString());
            lastFrame = GameManager.frameNumber;
            PlayerPrefs.SetInt("MAX_Frames", GameManager.frameNumber);
            manager.writeFile(savingData, Application.dataPath + "/EntityData/" + file + ".dat");

            //print("SAVED "+ GameManager.frameNumber);

            //Destroy(this.gameObject);
        }
    }
}