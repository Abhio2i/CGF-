using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CockpitDisplay : MonoBehaviour
{
    PlayerControle inputActions;
    bool trigger;
    [SerializeField]Canvas Cockpit;
    [SerializeField]Canvas Radar;
    private void Awake()
    {
        inputActions = new PlayerControle();
        inputActions.CockpitDisplay.Cockpit.performed +=ctx=>ChangeCockpitDisplay();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    int count;
    void ChangeCockpitDisplay()
    {
        trigger = !trigger;
        Cockpit.gameObject.SetActive(trigger);
        Radar.gameObject.SetActive(false);
    }
}
