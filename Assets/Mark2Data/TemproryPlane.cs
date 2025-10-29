using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemproryPlane : MonoBehaviour
{
    public float speed;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(100, 5, 100));
    }
        void Update()
    {
        transform.position += transform.forward * (speed / 2) * Time.deltaTime;
    }
}
