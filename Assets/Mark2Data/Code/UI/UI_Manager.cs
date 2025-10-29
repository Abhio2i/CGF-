#region Script Info
//inp: UI Objects (Spawn Manager, Way-point manager, previewCamera).
//out: On Selecting the Buttons (Aircraft, Way-point , Weather) The other UI Element is get Deactivate.   
#endregion
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.UImanager
{
    public class UI_Manager : MonoBehaviour
    {
        #region local perimeters
        [SerializeField] GameObject spawnManager;
        [SerializeField] GameObject waypointManager;
        [SerializeField] GameObject previewCamera;
        [SerializeField] Button AircraftButton;
        [SerializeField] Button WayPointButton;
        [SerializeField] Button WeatherButton;
        [SerializeField] Button WeaponButton;
        #endregion

        #region local function
        private void Start()
        {
            HideEverything();
        }
        #endregion

        #region Global Functions
        public void Spawn_Manager()
        {
            spawnManager.SetActive(true); 
            previewCamera.SetActive(true);
            waypointManager.SetActive(false);
        }
        public void Waypoint_Manager()
        {
            spawnManager.SetActive(false);
            previewCamera.SetActive(false);
            waypointManager.SetActive(true);
        }

        public void HideEverything()
        {

            spawnManager.SetActive(false);
            previewCamera.SetActive(false);
            waypointManager.SetActive(false);
        }

        //public void Interact(bool val)
        //{
        //    AircraftButton.interactable = val;
        //    WayPointButton.interactable = val;
        //    WeaponButton.interactable = val;
        //    WeaponButton.interactable = val;
        //}
        #endregion
    }
}