using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BasicControle : MonoBehaviour
{
    public GameObject MainPlane;
    public GameObject DeathCamera;
    public GameObject pauseText;
    public GameObject MenuPanel;
    public SilantroControl control;
    public bool pause = false;
    public bool gameEnd = false;
    private void Awake()
    {
        control = new SilantroControl();
    }

    private void OnEnable()
    {
        control.Enable();
    }

    private void OnDisable()
    {
        control.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //control = new SilantroControl();
        control.General.Pause.performed += (ctx) => {
            if(!MenuPanel.activeSelf)
            {
                pause = !pause;
                PauseGame(pause);
            }
        };

        control.General.MainMenu.performed += (ctx) => { 
            MenuPanel.SetActive(!MenuPanel.activeSelf);
            PauseGame(MenuPanel.activeSelf);
        };
    }

    private void FixedUpdate()
    {
        if ( (MainPlane.activeSelf == false || MainPlane==null) && !gameEnd)
        {
            gameEnd = true;
            DeathCamera.SetActive(true);
            MenuPanel.SetActive(true);
            PauseGame(MenuPanel.activeSelf);
        }
    }

    public void PauseGame(bool pause)
    {
        
        pauseText.SetActive(pause);
        if (pause)
        {
            MuteAllAudioSources(pause);
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
            MuteAllAudioSources(pause);
        }
    }


    void MuteAllAudioSources(bool mute)
    {
        // Find all AudioSources in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Mute each AudioSource
        foreach (AudioSource audioSource in audioSources)
        {
            
            audioSource.mute = mute;
            if(audioSource.gameObject.name == "Rear Sound Point")
            {
                audioSource.enabled = !mute;
                if (!mute)
                {
                    audioSource.Play();
                }
            }
            
        }

        // Print a message indicating that all audio is muted
        Debug.Log("All audio sources muted");
    }

    public void ExitGame()
    {
        PauseGame(false);
        Application.Quit();
    }

    public void MainMenu()
    {
        // Get the name of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex-1;
        PauseGame(false);
        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Restart()
    {
        // Get the name of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PauseGame(false);
        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

}
