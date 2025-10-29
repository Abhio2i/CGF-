using Assets.Code.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PilotRadarIntegration : MonoBehaviour
{
    public WaypointsFollow waypointFollewer;
    public SilantroDisplay autoPilotSilantro;
    public SilantroFlightComputer silantroFlightComputer;
    public TerrainRaycast terrainAvoidance;
    public TMP_InputField TAHeight;
    public TerrainContourFlying terrainContourFlying;
    public TMP_InputField TCFHeight;
    public GameObject ControleOverride;
    public SilantroController silantrocontroler;
    public TMP_Text wfText, apText, updownText, tcfText, taText;
    public WeaponTracker tracker;
    public GameObject TAMap, TACamera;
    private bool apbool, wfbool, tcfbool, tabool, dbool, sbool, abool;
    private Color fade = new Color(0, 0.4f, 0);


    private void Update()
    {
        if(silantrocontroler.inputType == SilantroController.InputType.AI)
        {
            ControleOverride.SetActive(true);
        }
        else
        {
            ControleOverride.SetActive(false);
        }
    }

    public void WaypointsFollowSwitch()
    {
        if (apbool||true)
        {
            if (dbool) { dbool = false; abool = true; }
            else if (abool) { abool = false; sbool = true; }
            else if (sbool) { sbool = false; dbool = true; }
            customUpdateText();
        }
        else if (wfbool) { /*waypointFollewer.enabled = false; wfbool = false; wfText.color = fade; */}
        else if (Save_Waypoints.waypoints.Count > 1)
        {
            /*waypointFollewer.enabled = true; wfbool = true; wfText.color = Color.green;*/
        }
    }
    public void AutoPilotSwitch()
    {
        if (apbool) { autoPilotSilantro.SetAutopilot(); silantroFlightComputer.operationMode = SilantroFlightComputer.AugmentationType.Manual; apbool = false; apText.color = fade; wfText.text = "WF"; wfText.color = fade; updownText.text = ""; }
        else
        {
            if (wfbool) { waypointFollewer.enabled = false; wfbool = false; }
            wfText.text = "D"; dbool = true; wfText.color = Color.green;
            updownText.color = Color.green; updownText.text = autoPilotSilantro. presetHeading.ToString() + " deg";
            autoPilotSilantro.SetAutopilot(); apbool = true; apText.color = Color.green;
            silantroFlightComputer.operationMode = SilantroFlightComputer.AugmentationType.StabilityAugmentation;
        }
        
    }

    public void upTerrainAvoidenseHeight()
    {
        int height = (int)terrainAvoidance.ActivationRange+100;
        height = height > 1500 ? 1500 : height;
        TAHeight.text = height.ToString();
        terrainAvoidance.ActivationRange = height;
    }

    public void downTerrainAvoidenseHeight()
    {
        int height = (int)terrainAvoidance.ActivationRange-100;
        height = height < 100 ? 100 : height;
        TAHeight.text = height.ToString();
        terrainAvoidance.ActivationRange = height;
    }

    public void setTerrainAvoidenseHeight(TMP_InputField input)
    {
        int height = int.Parse(input.text);
        height = height > 1500 ? 1500 : height;
        height = height < 100 ? 100 : height;
        input.text = height.ToString();
        terrainAvoidance.ActivationRange = height;
    }

    public void upTerrainContourHeight()
    {
        int height = (int)terrainContourFlying.tcfDistance + 100;
        height = height > 1500 ? 1500 : height;
        TCFHeight.text = height.ToString();
        terrainContourFlying.tcfDistance = height;
    }

    public void downTerrainContourHeight()
    {
        int height = (int)terrainContourFlying.tcfDistance - 100;
        height = height < 100 ? 100 : height;
        TCFHeight.text = height.ToString();
        terrainContourFlying.tcfDistance = height;
    }

    public void setTerrainContourHeight()
    {
        int height = int.Parse(TCFHeight.text);
        height = height > 1500 ? 1500 : height;
        height = height < 100 ? 100 : height;
        TCFHeight.text = height.ToString();
        terrainContourFlying.tcfDistance = height;
    }


    public void UpButton()
    {
        if (dbool) { autoPilotSilantro.IncreaseHeading(); }
        else if (abool) { autoPilotSilantro.IncreaseAltitude(); }
        else if (sbool) { autoPilotSilantro.IncreaseSpeed(); }
        customUpdateText();
    }
    public void DownButton()
    {
        if (dbool) { autoPilotSilantro.DecreaseHeading(); }
        else if (abool) { autoPilotSilantro.DecreaseAltitude(); }
        else if (sbool) { autoPilotSilantro.DecreaseSpeed(); }
        customUpdateText();
    }
    public void TerrainAvoidanceSwitch()
    {
        if (tabool) { terrainAvoidance.enabled = false; tabool = false; taText.color = fade; TAMap.SetActive(false); TACamera.SetActive(false); }
        else if (!tcfbool) { terrainAvoidance.tcf = false; terrainAvoidance.enabled = true; tabool = true; taText.color = Color.green; TAMap.SetActive(true); TACamera.SetActive(true); }
    }
    public void TerrainContourFlyingSwitch()
    {
        if (tcfbool)
        {
            if (wfbool) { waypointFollewer.contourFlying = false; }
            else { terrainContourFlying.enabled = false; }
            tcfbool = false; tcfText.color = fade;  
        }
        else
        {
            if (wfbool) { waypointFollewer.contourFlying = true; }
            else { terrainContourFlying.enabled = true; if (!tabool) { TerrainAvoidanceSwitch(); terrainAvoidance.tcf = true; TAMap.SetActive(false); TACamera.SetActive(false); } }
            terrainContourFlying.enabled = true; tcfbool = true; tcfText.color = Color.green;
        }
    }
    void customUpdateText()
    {
        if (dbool) { wfText.text = "BR"; updownText.text = autoPilotSilantro.presetHeading.ToString() + " deg"; }
        else if (abool) { wfText.text = "ALT"; updownText.text = (autoPilotSilantro.presetAltitude/3.281f).ToString("0") + " m"; }
        else if (sbool) { wfText.text = "SP"; updownText.text = (autoPilotSilantro.presetSpeed/1.944f).ToString("0") + " m/s"; }
    }
}
