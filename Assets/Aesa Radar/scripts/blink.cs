using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class blink : MonoBehaviour
{
    public float time = 10f;
    private float count = 0f;
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (count > time)
        {
            img.enabled = !img.enabled;
            count = 0f;
        }
        count+=Time.deltaTime;
    }
}
