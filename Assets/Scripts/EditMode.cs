using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditMode : MonoBehaviour
{
    public EWRadar ewRadar;
    public RWR rwr;
    public EWequation equation;
    public loadScript loadscript;
    public EWloadScript loadscriptEW;
    public GameObject ewSphere;
    public UttamRadar AesaRadar;
    public TMP_InputField script;
    public TMP_InputField scriptEW;
    public TextMeshProUGUI Distance;
    public RadarScreenUpdate_new radarScreen;
    // Start is called before the first frame update
    void Start()
    {
        loadscript = JsonUtility.FromJson<loadScript>(script.text);
        loadscriptEW = JsonUtility.FromJson<EWloadScript>(scriptEW.text);
        Time.timeScale = 0f;
    }

    public void submit()
    {
        if (AesaRadar != null)
        {
            loadscript = JsonUtility.FromJson<loadScript>(script.text);
            loadscript.azimuth = acct(loadscript.azimuth);
            loadscript.bar = acct(loadscript.bar);
            AesaRadar.TRMPower = loadscript.power * 1.37f;

            radarScreen.gain = loadscript.gain - 2;
            radarScreen.GainSet(0);
            radarScreen.band = (RadarScreenUpdate_new.Bandwidth)((int)loadscript.frequency - 7);
            radarScreen.BandwidthSet(0);
            AesaRadar.f = loadscript.frequency;
            //Debug.LogError("");
            AesaRadar.Azimuth = loadscript.azimuth * 10f;
            AesaRadar.Bar = loadscript.bar * 10f;
            AesaRadar.MinimumDetectSize = loadscript.rcs;
            AesaRadar.NR = loadscript.minimalNoise;
            Time.timeScale = 1f;
        }
    }

    public int acct(float i)
    {
        i = i > 6 ? i / 10 : i;
        i = i > 6 ? 6 : (i < 1 ? 1 : i);
        return (int)i;
    }

    public void submitEW()
    {
        if (ewRadar != null)
        {
            loadscriptEW = JsonUtility.FromJson<EWloadScript>(scriptEW.text);
            equation.Pt = loadscriptEW.power;
            equation.Gt = loadscriptEW.gain;
            equation.Gr = loadscriptEW.recieveGain;
            equation.Pr = loadscriptEW.threshold;
            ewRadar.radar_Range = equation.getRange() * 1000;
            ewRadar.autoChaffFire = loadscriptEW.autoChaff;
            ewRadar.autoFlareFire = loadscriptEW.autoFlare;
            ewRadar.chaffActivateRange = loadscriptEW.chaffActive * 1000f;
            ewRadar.flareActivateRange = loadscriptEW.flareActive * 1000f;
            ewRadar.DIRCM_Module.active = loadscriptEW.dircm;
            ewRadar.DIRCM_Module.effectRange = loadscriptEW.dircmActive * 1000;
            ewRadar.DIRCM_Module.idleDeflectionTime = loadscriptEW.deflectionTime;
            Distance.text = equation.getRange().ToString("0.0") + "km";
            ewRadar.SetRangeRWR();
            rwr.SetConfigRWR();
            ewSphere.transform.localScale = new Vector3(ewRadar.radar_Range * 2, 1, ewRadar.radar_Range * 2);
            Time.timeScale = 1f;
        }
    }
}
