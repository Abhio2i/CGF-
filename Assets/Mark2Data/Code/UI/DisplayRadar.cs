#region Script Info
//inp: R button.
//out: If the button is pressed the Radar display is going to active and deactivate.
#endregion
using UnityEngine;
using UnityEngine.InputSystem;
public class DisplayRadar : MonoBehaviour
{
    #region local Perimeters
    GameManagerInput Manager;
    [SerializeField]Canvas Cockpit;
    #endregion

    #region Global Perimeters
    public GameObject RadarCanvas;
    #endregion

    #region InputManager
    private void OnEnable()
    {
        Manager.Enable();
    }
    private void OnDisable()
    {
        Manager.Disable();
    }

    private void Awake()
    {
        Manager = new GameManagerInput();
    }
    #endregion

    #region local functions
    private void Start()
    {
        Manager.ButtonInput.Radar.performed += btn => OpenCloseRadar(); 
    }
    private void OpenCloseRadar()
    {
        if (RadarCanvas == null) return;
        bool isChange = !RadarCanvas.activeSelf;
        RadarCanvas.SetActive(isChange);
        Cockpit.gameObject.SetActive(false);
    }
    #endregion
}