#region Script Info
//out: Display all the plane date to the UI Mode.
#endregion
using UnityEngine;
using TMPro;

namespace UI.PlaneSpecs
{
    public class PlaneSpecs : MonoBehaviour
    {
        #region Local Perimeters
        [SerializeField] private TMP_Text planeLat;
        [SerializeField] private TMP_Text planeLon;
        [SerializeField] private TMP_Text planeAlt;
        [SerializeField] private TMP_Text planeSpeed;
        #endregion

        #region Global Perimeters
        [HideInInspector] public string lat;
        [HideInInspector] public string lon;
        [HideInInspector] public string alt;
        [HideInInspector] public string speed;
        #endregion

        #region local Functions
        private void FixedUpdate()
        {
            //return;
            UpdateAll(lat,lon,alt,speed);
        }

        private void UpdateAll(string _lat, string _lon, string _alt, string _speed)
        {
            UpdateLat(_lat);
            UpdateLon(_lon);
            UpdateAlt(_alt);
            UpdateSpeed(_speed);
        }

        private void UpdateLat(string _lat)
        {
            planeLat.text = _lat;
        }
        private void UpdateLon(string _lon)
        {
            planeLon.text = _lon;
        }
        private void UpdateAlt(string _alt)
        {
            planeAlt.text = _alt;
        }
        private void UpdateSpeed(string _speed)
        {
            planeSpeed.text = _speed;
        }
        #endregion
    }

}
