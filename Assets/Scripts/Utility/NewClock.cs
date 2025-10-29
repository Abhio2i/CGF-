using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    public class NewClock : MonoBehaviour
    {
        [SerializeField] GameObject reference;

        Vector3 temp;

        public Vector3 currentTime;

        DateTime d1;
        DateTime d2;

        float _time;

        public static string time;
        private void Awake()
        { 
            d1 = DateTime.Now;
        }
        private void FixedUpdate()
        {
            temp = SecondsToHours(_time += Time.fixedDeltaTime);
            TimeSpan timeSpan = new TimeSpan((int)temp.x, (int)temp.y, (int)temp.z);
            d2 = d1.Add(timeSpan);
            time = d2.ToString();
        }

        Vector3 SecondsToHours(float elapsedSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(elapsedSeconds);
            float hours = time.Hours;
            float minutes = time.Minutes;
            float seconds = time.Seconds;
            currentTime = new Vector3(hours, minutes, seconds);
            return currentTime;
        }
    }
}