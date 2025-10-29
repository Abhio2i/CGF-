using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Clock : MonoBehaviour
{
    int hours,minutes, seconds;


    private float timer = 0f;
    public TextMeshProUGUI timerText;
    private void Awake()
    {
       
    }
    private void Update()
    {
        if (!FeedBackRecorderAndPlayer.isPlaying)
        {
            // Increment the timer by the time passed since the last frame
            timer += Time.deltaTime;

            // Format the timer value as minutes and seconds
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");

            // Print the timer to the console (optional)
            //Debug.Log(minutes + ":" + seconds);

            // Update the UI Text component if available 
            if (timerText != null)
            {
                timerText.text = minutes + ":" + seconds;
            }
        }
    }
}
