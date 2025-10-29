
using UnityEngine;
using TMPro;
using Assets.Scripts.Feed;
using UnityEngine.UI;

 
public class HeadsUpDisplay : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] PlaneData planeData;
    [SerializeField] EW_Inputs ew;
    [SerializeField] Camera cam;

    [Header("Altitude and Speed Markers")]
    [SerializeField] GameObject longTick;
    [SerializeField] GameObject smallTick, indicatorValue;
    [SerializeField] Transform speedMarkerParentObj, altitudeMarkerParentObj, headingMarkerParentObj;
    [SerializeField] RectTransform speedStartPoint, altitudeStartPoint, headingStartPoint;
    [SerializeField] RectTransform ClimbRateBar1, ClimbRateBar2;
    [SerializeField] RectTransform AOABar1, AOABar2;
    [SerializeField] Transform RollRatevalue;
    [SerializeField] TMP_Text _speed, _altitude, _Mach, _ClimbRate;


    [Header("RollRate")]
    [SerializeField] Transform angleIndicator;


    [Header("NOse POinter")]
    [SerializeField] RectTransform nosePointer;
    [SerializeField] GameObject noseTarget;


    [Header("Compass")]
    [SerializeField] RawImage compassLine;
    [SerializeField] TextMeshProUGUI WaypointNumber, WaypointBearing, WaypointDistance;

    [Header("Pitch Ladder")]
    [SerializeField] GameObject positive;
    [SerializeField] GameObject negative;
    [SerializeField] Transform centre;
    RectTransform centrePos;

    Text negativeRight;
    Text negativeLeft;
    Text positiveRight;
    Text positiveLeft;



    [Header("Plane SPecs")]
    [SerializeField] TMP_Text gforce;
    [SerializeField] TMP_Text lat;
    [SerializeField] TMP_Text lon;
    [SerializeField] TMP_Text missileCount;
    [SerializeField] TMP_Text TAS, AOA, Pitch, Roll;
    [SerializeField] TMP_Text headingVal;
    //[SerializeField] TMP_Text gforce;

    private GameObject temp;
    float angle, pos;
    int objectPosition, objSpeed;
    float range = 45f;
    float interval = 50f;
    public TMP_Text Breaks;
    public SilantroController controller;

    private void Awake()
    {

        SpawnTicks(speedStartPoint, speedMarkerParentObj, -1, 50, true);
        SpawnTicks(altitudeStartPoint, altitudeMarkerParentObj, 1, 1500, true);
       
        Spawn();
    }
    private void Update()
    {
        UpdateHUD();
    }
    #region speed and altitude

    void SpeedAndAltitudeMarkers()
    {

        //MoveMarkersVertically(0.033f, altitudeMarkerParentObj, Mathf.Abs(objectPosition * 3.28084f), 108);
        //MoveMarkersVertically(1f, speedMarkerParentObj, Mathf.Abs(objSpeed * 1.94384f), 108);
        MoveMarkersVertically(1f, speedMarkerParentObj, Mathf.Abs(planeData.specification.entityInfo.IndicatedSpeed), 108);

        TAS.text = planeData.specification.entityInfo.TrueSpeed.ToString("0") + "kn";
        _Mach.text = planeData.specification.entityInfo.Mach.ToString("0.0")+"M";
        //var spd = (objSpeed * 1.94384f) > 500 ? (objSpeed * 1.94384f) / 666.7 : objSpeed * 1.94384f;
        //_speed.text = spd.ToString("0.0") + ((objSpeed * 1.94384f)>500?"Mach": " kn");
        _speed.text = planeData.specification.entityInfo.IndicatedSpeed.ToString("0"+"kn");
        _altitude.text = planeData.specification.entityInfo.Altitude.ToString("0") + " ft";
        float climbrate = (planeData.specification.entityInfo.VerticleVelocity * 60f);
        ClimbRateBar1.sizeDelta = new Vector2(2, (climbrate / 40000) * 100f);
        ClimbRateBar2.sizeDelta = new Vector2(2, (climbrate / 40000) * -100f);
        _ClimbRate.text = climbrate.ToString("0.0")+"ft";
        float AOAv = (planeData.specification.entityInfo.AngleOfAttack);
        AOAv = AOAv > 50 ? 0 : (AOAv < -50 ? 0 : AOAv);
        AOABar1.sizeDelta = new Vector2(2, (AOAv / 40) * 100f);
        AOABar2.sizeDelta = new Vector2(2, (AOAv / 40) * -100f);
        AOA.text = (planeData.specification.entityInfo.AngleOfAttack).ToString("0.0") + "°";

        WaypointNumber.text = "W"+planeData.specification.entityInfo.WaypointNumber.ToString("");
        WaypointBearing.text = planeData.specification.entityInfo.WaypointBearing.ToString("0.0") + "°";
        WaypointDistance.text = (planeData.specification.entityInfo.WaypointDistance/ 6076.115f).ToString("0.0")+ "NM";

        RollRatevalue.localEulerAngles = new Vector3(0, 0, planeData.specification.entityInfo.RollRate);
    
    }
    void SpawnTicks(RectTransform startPoint, Transform parentObj, int dir, int offset, bool isHorizontal)
    {
        Vector3 oldPos = startPoint.anchoredPosition;
        int value = 0;
        for (int i = 0; i < 800; i++)
        {
            if (i % 10 == 0)
            {
                temp = Instantiate(smallTick);
                var indicatorVal = Instantiate(indicatorValue);


                float tempPos = oldPos.y + i * 5;

                temp.transform.position = new Vector3(0, tempPos, oldPos.z);
                indicatorVal.transform.position = new Vector3(dir * 50, tempPos + 5f, oldPos.z);

                temp.transform.SetParent(parentObj, false);
                indicatorVal.transform.SetParent(parentObj, false);

                indicatorVal.GetComponent<TMP_Text>().text = (value).ToString();
                indicatorVal.gameObject.SetActive(true);
                value += offset;
            }
            else if (i % 5 == 0)
            {
                temp = Instantiate(longTick);

                float tempPos = oldPos.y + i * 5;
                temp.transform.position = new Vector3(dir * 9, tempPos, oldPos.z);

                temp.transform.SetParent(parentObj, false);
            }

        }
    }
    void MoveMarkersVertically(float verticalOffset, Transform offset, float value, float y_centerPostion)
    {
        offset.localPosition = new Vector3(offset.localPosition.x,
            y_centerPostion + (-value * verticalOffset)
            , offset.localPosition.z);
    }
    #endregion

    #region roll angle
    void RateOfRoll(float angle)
    {
        if (angle > 180) angle -= 360f;
        angle = Mathf.Clamp(angle, -60f, 60f);
        //angleIndicator.rotation =
        //    Quaternion.Euler(new Vector3(angleIndicator.rotation.x
        //    , angleIndicator.rotation.z
        //    , 180f - angle ));
        angleIndicator.rotation =
            Quaternion.Euler(new Vector3(0, 0, 19f));

    }
    #endregion

    #region heading
    void Heading()
    {
        headingVal.text = planeData.specification.entityInfo.Heading.ToString("0.0") + "°";
        Pitch.text = (ew.pitchAngle).ToString("0") + "°";
        Roll.text = (ew.rollAngle).ToString("0") + "°";
    }
    void MoveMarkersHorizontally()
    {

    }
    #endregion
    #region nose pointer
    void NoseMarker(GameObject WorldObject, RectTransform UI_Element)
    {
        Vector3 refVel = Vector3.zero;
        Vector3 pos = cam.WorldToScreenPoint(WorldObject.transform.position);
        UI_Element.transform.position = Vector3.Lerp(UI_Element.transform.position, pos, 0.85f * Time.fixedDeltaTime);
    } //point plane nose
    #endregion

    void UpdateHUD()
    {
        SupplyData();
        SpeedAndAltitudeMarkers();
        RateOfRoll(angle);
        if(noseTarget!=null)
        NoseMarker(noseTarget, nosePointer);
        VelocityMarker();
        CompassMarker();
        DisplayValues();
        Heading();
    }

    #region necessary functions
    void Spawn() //spawning pitch ladders
    {
        if (positive)
        {
            positiveLeft = positive.transform.GetChild(0).GetComponent<Text>();
            positiveRight = positive.transform.GetChild(1).GetComponent<Text>();
        }
        if (negative)
        {
            negativeLeft = negative.transform.GetChild(0).GetComponent<Text>();
            negativeRight = negative.transform.GetChild(1).GetComponent<Text>();
        }

        GameObject gameObject = positive;
        Vector3 oldPos = centre.GetComponent<RectTransform>().anchoredPosition;// -new Vector3(0, 200, 0);
        for (int i = (int)range; i >= -range; i--)
        {
            if (i > 0)
            {
                gameObject = positive;
                positiveLeft.text = i.ToString();
                positiveRight.text = i.ToString();
            }
            if (i < 0)
            {
                gameObject = negative;
                negativeLeft.text = i.ToString();
                negativeRight.text = i.ToString();
            }
            if (i % 5 == 0 && i != 0)
            {
                GameObject obj = Instantiate(gameObject);
                float temp = oldPos.y + i * interval;
                obj.transform.position = new Vector3(0, temp, oldPos.z);
                obj.transform.SetParent(centre, false);
            }
        }
        centrePos = centre.GetComponent<RectTransform>();

        //centre.GetComponent<RectTransform>().transform.position = new Vector3(0, -120f, 0);
    }
    /*
    void HorizonMarker() //horizon level
    {
        Vector3 angle = player.transform.rotation.eulerAngles;
        // print(angle);
        if (angle.z >= 180f)
            angle.z -= 360f;
        horizonLine.rotation = Quaternion.Euler(new Vector3(0, 0, angle.z));
    }
    */
    void VelocityMarker() //moving pitch ladders w.r.t pitch and roll
    {
        if (pos >= 180)
            pos -= 360f;
        centrePos.anchoredPosition = new Vector3(centrePos.anchoredPosition.x, pos * 48.8f, centrePos.position.z);

        Vector3 angle = planeData.planeTransform.transform.rotation.eulerAngles;
        // print(angle);
        if (angle.z >= 180f)
            angle.z -= 360f;
        centrePos.rotation = Quaternion.Euler(new Vector3(0, 0, -angle.z));
    }

    public void SupplyData()
    {
        objectPosition = (int)planeData.planeTransform.position.y;
        objSpeed = (int)(planeData.specification.entityInfo.Speed/ 1.94384f);
        angle = planeData.planeTransform.rotation.z;
        pos = player.transform.rotation.eulerAngles.x;
        Breaks.text = "Brake " + controller.gearHelper.brakeState.ToString();
    }
    void DisplayValues()
    {
        lat.text = planeData.latLong.x.ToString("0.0") + " lat";
        lon.text = planeData.latLong.y.ToString("0.0") + " long";
        gforce.text = planeData.g_force.ToString() + " G";
        missileCount.text = planeData.missileCount.ToString();
    }
    void CompassMarker() //pointing compass 
    {
        double bankAngle = player.transform.rotation.eulerAngles.y;
        if (bankAngle > 180f)
            bankAngle -= 360f;
        bankAngle /= 360f;
        // Debug.Log(bankAngle);
        compassLine.uvRect = new Rect((float)bankAngle, 0, 1, 1);
    }
    #endregion
}
