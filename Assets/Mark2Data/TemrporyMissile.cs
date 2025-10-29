using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemrporyMissile : MonoBehaviour
{
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(50, 5, 50));
    }
}
