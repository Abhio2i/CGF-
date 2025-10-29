#region Script Info
//inp: Plane data Current speed
//out: The Speed UI move As the Speed of plane changes.
#endregion

using UnityEngine;
using TMPro;
using Data.Plane;
namespace UI.FlightInstruments
{
    public class SpeedMeter : MonoBehaviour
    {
        #region Global Perimeters
        public PlaneData data;
        public RectTransform pointer;
        public TMP_Text speedText;
       
        public float maxKnots = 900f;

        public const float mpsToKnots = 2f;
        #endregion

        #region Global Functions
        public void HandleAirplaneUI()
        {
            if (pointer && data)
            {
                float currentKnots = data.bodySpeed * mpsToKnots;

                float normalizedKnots = Mathf.InverseLerp(0f, maxKnots, currentKnots);
                float targetRotation = 360f * normalizedKnots;
                pointer.rotation = Quaternion.Euler(0f, 0f, -targetRotation);
                speedText.text = ((int)currentKnots).ToString("0000");
            }
        }

        public void Update()
        {
            HandleAirplaneUI();
        }

        #endregion
    }
}

