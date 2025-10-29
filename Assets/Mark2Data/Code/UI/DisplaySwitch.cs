using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DisplaySwitch : MonoBehaviour
{
    public Camera[] display1Cam, display2Cam, dislay3Cam;
    public Canvas[] dislay1Canvas, dislay2Canvas,display3Canvas;
    public int MainLandIndex=3;

    private int index = 60;

    PlayerControle PlayerInput;

    private void OnEnable()
    {
        PlayerInput.Enable();
    }

    private void OnDisable()
    {
        PlayerInput.Disable();
    }

    public void Awake()
    {
        PlayerInput = new PlayerControle();
    }
    void Start()
    {
        PlayerInput.CockpitDisplay.SwitchDisplay.performed += ctx => { InGameSwitch(--index % 2); };
        SwitchDisplays();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (Input.GetKeyDown(KeyCode.Tab)){ InGameSwitch(--index % 2); }
            //if (Input.GetKeyDown(KeyCode.Y)) { InGameSwitch(++index % 2); }
        }
        
    }
    void InGameSwitch(int index)
    {
        if (PlayerPrefs.GetInt("NoTouchDis") == 1 || true)
        {
            switch (index)
            {
                case 0:
                    {
                        Display1Setter(1);
                        Display2Setter(0);
                        //Display3Setter(2);
                        break;
                    }
                case 1:
                    {
                        Display1Setter(0);
                        Display2Setter(1);
                        //Display3Setter(1);
                        break;
                    }
                case 2:
                    {
                        Display1Setter(0);
                        Display2Setter(1);
                        //Display3Setter(0);
                        break;
                    }
            }
        }
    }
    public void SwitchDisplays()
    {
        if (Display.displays.Length < 2)
        {
            //if (SceneManager.GetActiveScene().buildIndex == MainLandIndex)
            //{
            //    InGameSwitch(--index % 3);
            //    return;
            //}
            Display1Setter(1);
            Display2Setter(0);
        }
    }
    public void Display1Setter(int display)
    {
        foreach (Camera cam in display1Cam)
        {
            cam.targetDisplay = display;
        }
        foreach (Canvas canvas in dislay1Canvas)
        {
            canvas.targetDisplay = display;
        }
    }
    public void Display2Setter(int display)
    {
        foreach (Camera cam in display2Cam)
        {
            cam.targetDisplay = display;
        }
        foreach (Canvas canvas in dislay2Canvas)
        {
            canvas.targetDisplay = display;
        }
    }
    public void Display3Setter(int display)
    {
        foreach (Camera cam in dislay3Cam)
        {
            cam.targetDisplay = display;
        }
        foreach (Canvas canvas in display3Canvas)
        {
            canvas.targetDisplay = display;
        }
    }
    public void TouchDisplay()
    {
        PlayerPrefs.SetInt("NoTouchDis", 0);
        Display1Setter(0);
        Display2Setter(1);
        //SceneManager.LoadScene(1);
    }
    public void NonTouchDisplay()
    {
        PlayerPrefs.SetInt("NoTouchDis", 1);
        SwitchDisplays();
    }
}
