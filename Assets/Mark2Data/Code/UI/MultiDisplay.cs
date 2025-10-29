#region Script Info
//out: Find the all connected displays and active them.
#endregion
using UnityEngine;

namespace UI.Monitor
{
    public class MultiDisplay : MonoBehaviour
    {
        #region Local Function
        private void Awake()
        {
            DontDestroyOnLoad(this);
            MapCameraToDisplay();

        }
        void MapCameraToDisplay()
        {
            //Loop over Connected Displays
            for (int i = 0; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate(); //Enable the display
            }
        }
        #endregion
    }

}
