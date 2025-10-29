using Data.Plane;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField]float maxVelocity;
    [SerializeField] float maxVolume;
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlaneData planeSpeed;
    [SerializeField]GameObject player;

    private void Awake()
    {
        audioSource = player.GetComponent<AudioSource>();
        maxVolume =audioSource.volume;
    }
    //adjust speed wrt speed 
    private void FixedUpdate()
    {
        if (player == null) return;
        float velocity = player.GetComponent<Rigidbody>().velocity.magnitude;// planeSpeed.bodySpeed;
        float factor = Mathf.Clamp01(velocity / maxVelocity);
        //audioSource.volume = factor * maxVolume;
    }
}
