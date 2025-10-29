using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TMP_Text timeText;
    public Slider timeSlider;

    private const float MaxHours = 24f;

    // Update is called once per frame
    void Update()
    {
        if (timeText && timeSlider)
        {
            float sliderValue = timeSlider.value;

            // Ensure the slider value stays within the range of 0 to 24
            sliderValue = Mathf.Clamp(sliderValue, 0f, MaxHours);

            // Calculate the time based on the slider value
            float currentTime = Mathf.Lerp(0f, 24f, sliderValue / MaxHours);

            // Update the time text display
            TimeSpan timeSpan = TimeSpan.FromHours(currentTime);
            string timeString = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            timeText.text = timeString;
        }
    }
}
