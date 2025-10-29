using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudConfigState : MonoBehaviour
{
    public Toggle hud;
    public Toggle GameStatistics;
    public Toggle Event;
    public Toggle Fps;
    public Toggle UDP;
    // Start is called before the first frame update
    void Start()
    {
        hud.isOn = PlayerPrefs.GetInt("hud") == 1 ? true : false;
        GameStatistics.isOn = PlayerPrefs.GetInt("GameStatistics") == 1 ? true : false;
        Event.isOn = PlayerPrefs.GetInt("Event") == 1 ? true : false;
        Fps.isOn = PlayerPrefs.GetInt("Fps") == 1 ? true : false;
        UDP.isOn = PlayerPrefs.GetInt("UDP") == 1 ? true : false;
    }

    public void setHudState(bool state)
    {
        PlayerPrefs.SetInt("hud",state?1:0);
    }
    public void setGameStatisticsState(bool state)
    {
        PlayerPrefs.SetInt("GameStatistics", state ? 1 : 0);
    }
    public void setEventState(bool state)
    {
        PlayerPrefs.SetInt("Event", state ? 1 : 0);
    }
    public void setFpsState(bool state)
    {
        PlayerPrefs.SetInt("Fps", state ? 1 : 0);
    }
    public void setUDPState(bool state)
    {
        PlayerPrefs.SetInt("UDP", state ? 1 : 0);
    }

}
