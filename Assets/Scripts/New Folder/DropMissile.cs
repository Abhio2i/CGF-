using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMissile : MonoBehaviour
{

    public FixedJoint drop;
    public GameObject trail;
    public missileEngine engine;

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
        PlayerInput.FireMissile.Fire1.performed += (ctx) => {
            Destroy(drop);
            Invoke("fire", 0.6f);
        };
    }

    public void fire()
    {
        trail.SetActive(true);
        engine.fire = true;
    }

    
}
