using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LongPress : MonoBehaviour
{
    bool isButtonPressed = false;
    float pressStartTime = 0.0f;
    public float PressDuration = 0.2f;
    public float longPressDuration = 1.0f; // Adjust this value as needed
    public UnityEvent PressEvent;
    public UnityEvent longPressEvent;


    public void down()
    {
        isButtonPressed = true;
        pressStartTime = Time.time;
    }

    public void up()
    {
        if (isButtonPressed && Time.time - pressStartTime <= PressDuration)
        {
            PressEvent.Invoke();
        }else
        if (isButtonPressed && Time.time - pressStartTime >= longPressDuration)
        {
            longPressEvent.Invoke();
        }
        isButtonPressed = false;
    }

}
