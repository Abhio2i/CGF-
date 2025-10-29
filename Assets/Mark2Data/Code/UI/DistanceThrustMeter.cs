#region Script Info
//inp: Plane potion and thrust value of joystick
//out: The meter is move according to the plane conditions.
#endregion
using UnityEngine;
using UnityEngine.UI;

public class DistanceThrustMeter : MonoBehaviour
{
    #region Global Parameters
    public Slider displayMeter;
    public Slider thrustMeter;
    #endregion

    #region Global Functions
    public void OnChangeMeter(float _val)
    {
        displayMeter.value = _val;
    }

    public void OnChangeThrust(float _val)
    {
        thrustMeter.value = _val;
    }
    #endregion
}
