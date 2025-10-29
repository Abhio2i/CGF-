using Assets.Scripts.Feed;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Rendering;

namespace Assets.Scripts.Feed_Scene.newFeed
{
    public class NewFeedSceneManager : MonoBehaviour
    {

            [SerializeField] NewReadDatas read;

            [SerializeField] public List<Entity> entities;

            [SerializeField] GameObject sliderHandler;
            [SerializeField] GameObject textObject, parent;
            [SerializeField] GameObject aircraft, missile;
            [SerializeField] GameObject player, enemy, ally, neutral, warship;
        
            [SerializeField] GameObject _scrollView;

            [Header("Script Reference")]
            [SerializeField] Emulator emulator;
            [SerializeField] SetEvents setEvents;
            [SerializeField] FreeRoamCam freeRoamCam;
            [SerializeField] CameraManager camManager;
            [SerializeField] CameraBehaviourForMissiles camBehaviourForMissiles;

            [Header("Text_Reference")]
            [SerializeField] TextMeshProUGUI position, rotation, gforce, timetext;
            [SerializeField] public static List<Transform> textReference = new List<Transform>();

            [Header("MaterialForNPC")]
            [SerializeField] Material allyMaterial, enemyMaterial, playerMaterial;
            
            [Header("DestroyMaterial")]
            [SerializeField] Material destroyAllyMaterial, destroyEnemyMaterial, destroyPlayerMaterial;
            
            Inputs inputActions;

            bool cameraSelection, enableCamera;
            bool isMissile;
            bool switchCam=true;
            public static int selectionIndex;


            List<GameObject> aircrafts;
            public CanvasController gc;


            private void Awake()
            {
                entities = new List<Entity>();

                aircrafts = new List<GameObject>();

                read.Read();

                emulator.SetMaxFrames();

                InstantiateObjects();

                //InstantiateNPC();

                camManager.objects = aircrafts;
                camManager.objects.Reverse();
                camManager.ActiveVirtualCam();

                //input intialize
                inputActions = new Inputs();

                FreeRoamCamInputs();

                CameraManagerInputs();

                freeRoamCam.EnableCamera(true);

        }
            private void OnEnable()
            {
                inputActions.Enable();
            }
            private void OnDisable()
            {
                inputActions.Disable();
            }
            private void FixedUpdate()
            {
                CameraGeneralInputs();

                CollectCamInputs(); //free roam cam inputs

                DisplayDatas();

                foreach (GameObject gameObject in aircrafts)
                {
                    bool active = false;
                    if (CameraManager.selected == gameObject) active = true;

                    if(gameObject.GetComponent<SetEvents>().IAmactive!=active)
                    {
                        //ActiveMessages(gameObject, active);
                        gameObject.GetComponent<SetEvents>().EnableDisable(active);
                    
                    }
                }
            }
            public static void ResetScene()
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            void ActiveMessages(GameObject gameObject, bool active)
            {
                var temp = gameObject.GetComponent<EventManager>();
                temp.Active(active);
            }
            void PausePlay(bool pause)
            {
                if (pause)
                    emulator.Pause();
                else
                    emulator.Play();
            }
            bool pause;
            void CameraGeneralInputs()
            {
                inputActions.Camera.FreeRoam.performed += _ =>
                {
                    //enableCamera = !enableCamera;
                    //freeRoamCam.EnableCamera(enableCamera);
                    if(!freeRoamCam.SatelliteCam)
                    //PausePlay(enableCamera);
                    Debug.Log("godcam");
                };
                inputActions.Camera.PausePlay.performed += _ =>
                {
                    pause = !pause;
                    PausePlay(pause);

                };
            }
            void FreeRoamCamInputs()
            {
                inputActions.Camera.Position.performed += _ =>
                {
                    FreeRoamCam.positioning = true;
                };
                inputActions.Camera.Position.canceled += _ =>
                {
                    FreeRoamCam.positioning = false;
                };
                inputActions.Camera.Rotation.performed += _ =>
                {
                    FreeRoamCam.rotating = true;
                };

                inputActions.Camera.Rotation.canceled += _ =>
                {
                    FreeRoamCam.rotating = false;
                };

            }
            void CameraManagerInputs()
            {

                inputActions.Camera.CameraSelection.performed += _ =>
                {
                    //gc.gameObject.SetActive(!gc.gameObject.activeSelf);
                    camManager.SwitchCamPosition(switchCam);
                    switchCam = !switchCam;
                };
            
                //inputActions.Camera.Pan_X.performed += _ =>
                //{
                //    float temp = inputActions.Camera.Pan_X.ReadValue<float>();
                //    if (temp < -0.5f)
                //    {
                //        selectionIndex = -1;
                //    }
                //    else if (temp > 0.5f)
                //    {
                //        selectionIndex = 1;
                //    }
                //    else
                //    {
                //        selectionIndex = 0;
                //    }
                //    camManager.selectionDirection = selectionIndex;
                //    camManager.SelectTarget();
                //};

                
            //camManager.SelectTarget();

            //inputActions.Camera.Pan_X.canceled += _ =>
            //    {
            //        selectionIndex = 0;
            //    };
            }
       void CollectCamInputs()
       {
                float temp = inputActions.Camera.Pan_X.ReadValue<float>();

                FreeRoamCam.x_axis = temp;
                FreeRoamCam.y_axis = inputActions.Camera.Pan_Y.ReadValue<float>();
                FreeRoamCam.z_axis = inputActions.Camera.Pan_Z.ReadValue<float>();
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectionIndex = -1;
                camManager.selectionDirection = selectionIndex;
                camManager.SelectTarget();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectionIndex = 1;
                camManager.selectionDirection = selectionIndex;
                camManager.SelectTarget();
            }
            else
            {
                selectionIndex = 0;
                camManager.selectionDirection = selectionIndex;
                camManager.SelectTarget();
            }
            
        }

        public void ChangeChildrenColorAccordingToType(Transform _object,Material colorMaterial)
        {
            Transform[] allChildren = _object.GetComponentsInChildren<Transform>();
            for(int i=0;i<allChildren.Length; i++)
            {
                var mat = allChildren[i].gameObject.GetComponent<MeshRenderer>();
                if (mat != null) mat.material = colorMaterial;
            }
        }
        public void HideGameObject(Transform _object,bool hide)
        {
           
            Transform[] allChildren = _object.GetComponentsInChildren<Transform>();
            for (int i = 0; i < allChildren.Length; i++)
            {
                var mat = allChildren[i].gameObject.GetComponent<MeshRenderer>();
                if (mat != null) mat.enabled = hide;
                //allChildren[i].gameObject.SetActive(hide);
            }
        }
        GameObject plane;
            Material activeMat,destroyMat;

            void InstantiateObjects()
            {
                foreach (DatasToBeSaved planeData in read.saveDatas)
                {
                
                    if (planeData._NPC_type == EntityType.Plane_Adversary.ToString())
                    {
                        plane = Instantiate(enemy);
                        plane.name = "Enemy";
                        activeMat = enemyMaterial;
                        destroyMat = destroyEnemyMaterial;
                        ChangeChildrenColorAccordingToType(plane.transform, enemyMaterial);
                        aircrafts.Add(plane);
                        isMissile=false;
                }
                else if (planeData._NPC_type == EntityType.Plane_Ally.ToString())
                    {
                        plane = Instantiate(ally);
                        plane.name = "Ally";
                        activeMat=allyMaterial;
                        destroyMat = destroyAllyMaterial;
                        ChangeChildrenColorAccordingToType(plane.transform, allyMaterial);
                        aircrafts.Add(plane);
                        isMissile = false;
                }
                //else if (planeData._NPC_type == EntityType.EnemyWarship.ToString())
                //{
                //    plane = Instantiate(warship);
                //    plane.name = "EnemyWarship";
                //    activeMat = enemyMaterial;
                //    destroyMat = destroyEnemyMaterial;
                //    ChangeChildrenColorAccordingToType(plane.transform, enemyMaterial);
                //    //.Add(plane);
                //    isMissile = false;
                //}
                else if (planeData._NPC_type == EntityType.Plane_Player.ToString())
                {
                        plane = player;
                        activeMat = playerMaterial;
                        destroyMat = destroyPlayerMaterial;
                        ChangeChildrenColorAccordingToType(plane.transform, playerMaterial);
                        CameraManager.selected = plane;
                    aircrafts.Add(plane);
                    isMissile = false;
                }

                if (planeData._NPC_type == EntityType.Missile_Adversary.ToString())
                {
                    plane = Instantiate(missile);
                    plane.name = "Bad Missile";
                    activeMat = enemyMaterial;
                    destroyMat = destroyEnemyMaterial;
                    ChangeChildrenColorAccordingToType(plane.transform, enemyMaterial);
                    camBehaviourForMissiles.missiles.Add(plane);
                    isMissile = true;
                }
                else if (planeData._NPC_type == EntityType.Missile_Ally.ToString())
                {
                    plane = Instantiate(missile);
                    plane.name = "Good Missile";
                    activeMat = allyMaterial;
                    destroyMat = destroyAllyMaterial;
                    ChangeChildrenColorAccordingToType(plane.transform, allyMaterial);
                    camBehaviourForMissiles.missiles.Add(plane);
                    isMissile = true;

                }
                else if (planeData._NPC_type == EntityType.Missile_Player.ToString())
                {
                    plane = Instantiate(missile);
                    plane.name = "My Missile";
                    activeMat = playerMaterial;
                    destroyMat = destroyPlayerMaterial;
                    ChangeChildrenColorAccordingToType(plane.transform, playerMaterial);
                    camBehaviourForMissiles.missiles.Add(plane);
                    isMissile = true;
                }


                /*var trial = plane.AddComponent<TrailRenderer>();
                trial.endWidth = trial.startWidth = 0.5f;
                trial.material = activeMat;*/
                if (!isMissile)
                {
                    var _event = plane.AddComponent<SetEvents>();
                    _event.events = planeData.storeEvents;
                    _event.time = planeData.storeTime;
                    _event.eventSymbol = sliderHandler;
                    _event.emulator = emulator;
                    _event.maxFrames = PlayerPrefs.GetInt("MAX_Frames");
                    _event.parent = sliderHandler.transform.parent.gameObject;
                    //_event.CreateEventIcons();
                    //_event.CreateTextObjects(textObject, parent.transform);
                    _event.enabled = true;

                    var _ui = plane.AddComponent<newUiIntegrate>();
                    _ui.setupUI = GetComponent<newUiIntegrate>();
                    _ui.enabled = true;
                    _ui.setup();
                }
              
                var data = plane.AddComponent<SetEntityPositionAndRotation>();
                data.str_positions = planeData.storePosition;
                data.str_rotation = planeData.storeRosition;
                data.latLong = planeData.LatLong;
                data._gforce = planeData.storeGforce;
                data.firstframe = int.Parse(planeData.firstFrame);
                data.lastframe = int.Parse(planeData.lastFrame);
                data.ActiveMaterial = activeMat;
                data.DestroyMaterial = destroyMat;
                data.Initialize();
                data.manager = GetComponent<NewFeedSceneManager>();
                data.enabled = true;
                data.missile = isMissile;
            }
            gc.aircrafts = aircrafts;
            gc.missiles = camBehaviourForMissiles.missiles;
        }
        public void DisplayDatas()
            {
                if (CameraManager.selected == null)
                {
                    CameraManager.selected = player;
                }
                if (CameraManager.selected.GetComponent<SetEntityPositionAndRotation>() != null)
                {
                    var details = CameraManager.selected.GetComponent<SetEntityPositionAndRotation>();

                /*position.text = details.currentPos.ToString();
                rotation.text = details.currentRot.ToString();*/
                position.text = details.latitudeLong.x.ToString();
                rotation.text = details.latitudeLong.y.ToString();
                    gforce.text = details.gforce.ToString();
                    //if (CameraManager.selected.GetComponent<SetEvents>() != null)
                    //    timetext.text = CameraManager.selected.GetComponent<SetEvents>().time.ToString();
                }
            }
        }
    }
