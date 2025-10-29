using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillTransparency : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Image fogImage;

    public float startingAlpha = 0.5f; // Define your starting alpha value here

    // Start is called before the first frame update
    void Start()
    {
        if (fogImage)
        {
            // Set the starting alpha value
            Color fogColor = fogImage.color;
            fogColor.a = startingAlpha;
            fogImage.color = fogColor;
        }
    }

    public void fogUpdate()
    {
        if (scrollbar && fogImage)
        {
            // Map the scrollbar value to the alpha channel of the image's color
            Color fogColor = fogImage.color;
            fogColor.a = Mathf.Lerp(startingAlpha, 1f, scrollbar.value); // Adjust alpha from startingAlpha to 1
            fogImage.color = fogColor;
        }
    }

}
