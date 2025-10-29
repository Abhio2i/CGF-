using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health;
    public void Damage(int value)
    {
        health-= value;
        if (health <= 0) Destroy(gameObject);
    }
}
