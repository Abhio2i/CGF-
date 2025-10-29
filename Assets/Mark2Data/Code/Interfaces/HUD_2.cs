using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#region script info
//controlling HUD elements
//input :player position and rotation
#endregion
public class HUD_2 : MonoBehaviour
{
    #region SerializeField
    float range = 45f;
    float interval=25f;
    [SerializeField] float x;
    [SerializeField] Canvas canvas;
    [SerializeField] Camera mainCam;
    [SerializeField] Camera playerCam;

    [SerializeField] GameObject positive;
    [SerializeField] GameObject negative;
    [SerializeField] GameObject nosePointer;

    [SerializeField] RectTransform horizonLine;
    [SerializeField] RectTransform pathVector;

    [SerializeField] Transform centre;
    [SerializeField] Transform player;

    [SerializeField] RawImage compassLine;
    [SerializeField] RawImage altitudeImage;
    [SerializeField] RawImage speedImage;
    [SerializeField] RawImage waypointMarker;
    [SerializeField] Image noseSprite;

    [SerializeField] Text alt;
    [SerializeField] Text spd;
    [SerializeField] Text GForce;
    [SerializeField] Text throttle;

    [SerializeField] public Vector3[] waypointList;
    [SerializeField] Vector3 pos;
    [SerializeField] string waypointTag;

    #endregion

    #region private
    GameObject referenceVect;
    float slowFactor;


    RectTransform centrePos;

    Text negativeRight;
    Text negativeLeft;
    Text positiveRight;
    Text positiveLeft;

    Camera cam;

    int passOnce;
    float factor;
    #endregion

    #region Unity main Functions

    private void Start()
    {
        int camNo = PlayerPrefs.GetInt("cockpit");
        if (camNo == 1)
            cam = playerCam;
        else
            cam = mainCam;
        referenceVect=new GameObject();
        referenceVect.name = "ReferenceVector";
        referenceVect.transform.SetParent(player.transform,false);
        referenceVect.transform.localPosition = new Vector3(0, 1, 1);
        slowFactor = 0.1f;
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
        Spawn();
       // Spawn(negative,-1);
    }
    
    

    private void FixedUpdate()
    {
        if (player == null) return;

        altitudeImage.uvRect = new Rect(-player.transform.position.y * slowFactor, 0, 1, 1);
        float k = (int)player.transform.position.y*3.28f;
        alt.text = k.ToString();

        float playerSpeed = player.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        int temp = (int)playerSpeed;
        spd.text = temp.ToString();
        speedImage.uvRect = new Rect(-playerSpeed * slowFactor, 0, 1, 1);

        float gForce = PlayerPrefs.GetFloat("GForce");
        GForce.text=gForce.ToString()+"G";

        VelocityMarker();
        HorizonMarker();
        CompassMarker();
        PathVector();
        if(nosePointer != null)
        NoseMarker(nosePointer,noseSprite.GetComponent<RectTransform>());
    }

    #endregion

    #region necessary functions
    void Spawn() //spawning pitch ladders
    {
        GameObject gameObject=positive;
        Vector3 oldPos = centre.GetComponent<RectTransform>().anchoredPosition;// -new Vector3(0, 200, 0);
        for (int i=(int)range;i>=-range;i--)
        {
            if (i > 0)
            {
                gameObject = positive;
                positiveLeft.text = i.ToString();
                positiveRight.text=i.ToString();
            }
            if (i < 0)
            {
                gameObject = negative;
                negativeLeft.text =  i.ToString();
                negativeRight.text = i.ToString();
            }
            if (i % 5 == 0 && i!=0)
            {
                GameObject obj=Instantiate(gameObject);
                float temp = oldPos.y + i * interval;
                obj.transform.position=new Vector3(0,temp,oldPos.z);
                obj.transform.SetParent(centre,false);
            }
        }
        centrePos = centre.GetComponent<RectTransform>();

        //centre.GetComponent<RectTransform>().transform.position = new Vector3(0, -120f, 0);
    }
    void HorizonMarker() //horizon level
    {
        Vector3 angle=player.transform.rotation.eulerAngles;
        // print(angle);
        if (angle.z >= 180f)
            angle.z -= 360f;
        horizonLine.rotation=Quaternion.Euler(new Vector3(0,0,-angle.z));
    }
    void VelocityMarker() //moving pitch ladders w.r.t pitch and roll
    {
        //float pos = UnityEngine.Vector3.Dot(player.transform.forward, UnityEngine.Vector3.up) * Mathf.Rad2Deg;
        float pos = player.transform.rotation.eulerAngles.x;
        if (pos >= 180)
            pos -= 360f;
        centrePos.anchoredPosition =new Vector3(centrePos.anchoredPosition.x, pos*25, centrePos.position.z);
    }
    //[Range(0f, 1f)]
    [SerializeField] float compassFactor;
    void CompassMarker() //pointing compass 
    {
        double bankAngle = player.transform.rotation.eulerAngles.y;
        if (bankAngle > 180f)
            bankAngle -= 360f;
        bankAngle /=360f;
        // Debug.Log(bankAngle);
        compassLine.uvRect = new Rect((float)bankAngle, 0, 1, 1);
    }

    void PathVector() //flight path vector
    {
        float pitch= Vector3.Dot(player.transform.forward, UnityEngine.Vector3.up) * Mathf.Rad2Deg;
        float yaw= Vector3.Dot(player.transform.forward, UnityEngine.Vector3.right) * Mathf.Rad2Deg;
        //Vector3 angleDiff =new Vector3( player.transform.rotation.eulerAngles.x-pitch, player.transform.rotation.eulerAngles.y - yaw,0f);
        Vector3 angleDiff=new Vector3(yaw,pitch,0);
        pathVector.localPosition=angleDiff;
    }

    void NoseMarker(GameObject WorldObject, RectTransform UI_Element)
    {
        Vector3 refVel = Vector3.zero;
        Vector3 pos = cam.WorldToScreenPoint(WorldObject.transform.position);
        UI_Element.transform.position = Vector3.Lerp(UI_Element.transform.position, pos, 0.85f * Time.fixedDeltaTime);
    } //point plane nose
    Vector3 TransformWorldToCamera(Vector3 worldObj, RectTransform UI_Element)
    {
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();


        Vector2 ViewportPosition = cam.WorldToViewportPoint(worldObj);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        return WorldObject_ScreenPosition;
    }
    #endregion
}
