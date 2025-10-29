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

namespace Assets.Scripts.Feed
{
    public class FeedSceneManager : MonoBehaviour
    {
        [SerializeField] ReadDatas read;

        [SerializeField] public List<Entity> entities;

        [SerializeField] GameObject sliderHandler;
        [SerializeField] GameObject textObject, parent;
        [SerializeField] GameObject aircraft, missile;
        [SerializeField] GameObject player, enemy, ally, neutral;
        [SerializeField] GameObject _scrollView;

        [Header("Script Reference")]
        [SerializeField] Emulator emulator;
        [SerializeField] SetEvents setEvents;
        [SerializeField] FreeRoamCam freeRoamCam;
        [SerializeField] CameraManager camManager;

        [Header("Text_Reference")]
        [SerializeField] TextMeshProUGUI position, rotation, gforce, timetext;
        [SerializeField] public static List<Transform> textReference = new List<Transform>();


        Inputs inputActions;

        bool cameraSelection, enableCamera;

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
            camManager.ActiveVirtualCam();

            //input intialize
            inputActions = new Inputs();

            FreeRoamCamInputs();

            CameraManagerInputs();



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

                ActiveMessages(gameObject, active);
                gameObject.GetComponent<SetEvents>().EnableDisable(active);
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
                enableCamera = !enableCamera;
                freeRoamCam.EnableCamera(enableCamera);
                PausePlay(enableCamera);
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
                gc.gameObject.SetActive(!gc.gameObject.activeSelf);
            };

            inputActions.Camera.Pan_X.performed += _ =>
            {
                float temp = inputActions.Camera.Pan_X.ReadValue<float>();
                if (temp < -0.5f)
                {
                    selectionIndex = -1;
                }
                else if (temp > 0.5f)
                {
                    selectionIndex = 1;
                }
                else
                {
                    selectionIndex = 0;
                }
                camManager.selectionDirection = selectionIndex;
                camManager.SelectTarget();
            };

            inputActions.Camera.Pan_X.canceled += _ =>
            {
                selectionIndex = 0;
            };
        }
        void CollectCamInputs()
        {
            float temp = inputActions.Camera.Pan_X.ReadValue<float>();

            FreeRoamCam.x_axis = temp;
            FreeRoamCam.y_axis = inputActions.Camera.Pan_Y.ReadValue<float>();
            FreeRoamCam.z_axis = inputActions.Camera.Pan_Z.ReadValue<float>();
        }

        GameObject plane;

        void InstantiateObjects()
        {
            foreach (SaveData planeData in read.saveDatas)
            {
                if (planeData._NPC_type == "B")
                {
                    plane = Instantiate(enemy);
                }
                else if (planeData._NPC_type == "A")
                {
                    plane = Instantiate(ally);
                }
                else
                {
                    plane = player;
                    CameraManager.selected = plane;
                }
                aircrafts.Add(plane);

                var data = plane.AddComponent<SetEntityPositionAndRotation>();
                data.str_positions = planeData.storePosition;
                data.str_rotation = planeData.storeRosition;
                data._gforce = planeData.storeGforce;
                data.Initialize();
                data.enabled = true;


                var _event = plane.AddComponent<SetEvents>();

                _event.events = planeData.storeEvents;
                _event.time = planeData.storeTime;
                _event.eventSymbol = sliderHandler;
                _event.emulator = emulator;
                _event.maxFrames = PlayerPrefs.GetInt("MaxFrames");
                _event.parent = sliderHandler.transform.parent.gameObject;
                _event.CreateEventIcons();
                _event.CreateTextObjects(textObject, parent.transform);
                _event.enabled = true;
            }
            gc.aircrafts = aircrafts;
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

                position.text = details.currentPos.ToString();
                rotation.text = details.currentRot.ToString();
                gforce.text = details.gforce.ToString();
                //if (CameraManager.selected.GetComponent<SetEvents>() != null)
                //    timetext.text = CameraManager.selected.GetComponent<SetEvents>().time.ToString();
            }
        }
    }
}