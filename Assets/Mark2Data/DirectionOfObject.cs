using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOfObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 1000f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
                Debug.Log(hit.transform.gameObject);
    }
}
