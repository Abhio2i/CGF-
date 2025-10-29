using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParticleControl : MonoBehaviour
{
    [SerializeField] Rigidbody player;
    [SerializeField] public GameObject fireStart;
    [SerializeField] GameObject smokeStart;

    SilantroControl inputActions;
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void Awake()
    {
        fireStart.SetActive(false);
        smokeStart.SetActive(false);
        inputActions = new SilantroControl();
        inputActions.General.StartEngineGlobal.performed += ctx => fireStart.SetActive(true);
        inputActions.General.StopEngineGlobal.performed += ctx => fireStart.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Start Engine"))
        {
            fireStart.SetActive(true);
        }
        if (Input.GetButtonDown("Stop Engine"))
        {
            fireStart.SetActive(false);
        }
    }

}
