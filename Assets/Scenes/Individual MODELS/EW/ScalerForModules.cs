using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerForModules : MonoBehaviour
{
    Vector3 originalScale;
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = originalScale * Camera.main.orthographicSize / 100;
    }
}
