using Oyedoyin.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class radarmodeltest : MonoBehaviour
{

    [Header("VIEW Screen")]
    public List<GameObject> screens = new List<GameObject>();
    public Camera cams;
    private int currentScreen = 1;
    public TextAsset log;
    public TextMeshProUGUI TOPViewRange;
    [Header("Side Menu")]
    public TMP_Dropdown aircraftDropdown;
    public TMP_InputField aircraft_IFF_Code;
    public Toggle jem;
    public TMP_Dropdown jamFeq;
    public Slider angleSlider;
    public TextMeshProUGUI padAngle;
    public Slider altiudeSlider;
    public TextMeshProUGUI padAltitude;
    public TextMeshProUGUI padName;
    public Animator slider;
    public Animator RadarPanel;
    public Transform listContentView;
    public Transform ListPrefab;
    public List<GameObject> list = new List<GameObject>();
    public List<GameObject> Aircrafts = new List<GameObject>();
    public Dictionary<GameObject,GameObject> AircraftList = new Dictionary<GameObject, GameObject>();   
    public Transform SelectedOBject;
    public Transform SelectedUI;
    public Transform MainPlane;
    public Transform Pad;
    public Camera zoomCamera;
    private bool zoomInAllow = false;
    private bool zoomOutAllow = false;
    private int movebtn = -1;
    private bool sliderOpen = false;
    private int numplane = 0;
    public void NextScreen(int i)
    {
          currentScreen=currentScreen>=screens.Count?0:currentScreen;
          foreach (var screen in screens) { screen.SetActive(false); };
          screens[currentScreen].SetActive(true);
          currentScreen++;
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sliderOpen = !sliderOpen;
            sliderUpandDown(sliderOpen);
            NextScreen(0);
        }
        


        if (zoomInAllow|| Input.GetKey(KeyCode.KeypadPlus))
        {

            int low = 100;
            if (zoomCamera.orthographicSize > low)
            {
                zoomCamera.orthographicSize -= (zoomCamera.orthographicSize * 0.2f) * Time.deltaTime;
            }
            zoomCamera.transform.localPosition = new Vector3(0, 0, zoomCamera.orthographicSize - 100f);
            var pos = zoomCamera.transform.position;
            pos.y = 90000;
            zoomCamera.transform.position = pos;
            var z = zoomCamera.orthographicSize * 2f;
            TOPViewRange.text = z.ToString("0.0") + "m *" + z.ToString("0.0") + "m";
        }

        if (zoomOutAllow || Input.GetKey(KeyCode.KeypadMinus))
        {

            int max = 60000;
            if (zoomCamera.orthographicSize < max)
            {
                zoomCamera.orthographicSize += (zoomCamera.orthographicSize * 0.2f) * Time.deltaTime;
            }
            zoomCamera.transform.localPosition = new Vector3(0, 0, zoomCamera.orthographicSize - 100f);
            var pos = zoomCamera.transform.position;
            pos.y = 90000;
            zoomCamera.transform.position = pos;
            var z = zoomCamera.orthographicSize * 2f;
            TOPViewRange.text = z.ToString("0.0") + "m *" + z.ToString("0.0") + "m";
        }

        if (SelectedOBject != null)
        {
            if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0)
            {

                var pos = SelectedOBject.transform.position;
                var movespee = (zoomCamera.orthographicSize * 0.2f);
                SelectedOBject.transform.position = new Vector3(pos.x + (movespee * Time.deltaTime *Input.GetAxis("Vertical")), pos.y, pos.z + (movespee * Time.deltaTime * Input.GetAxis("Horizontal")));
               
            }
            else
            {
                var i = movebtn;
                i = Input.GetAxis("Horizontal") > 0 ? 3 : i;
                i = Input.GetAxis("Horizontal") < 0 ? 2 : i;
                i = Input.GetAxis("Vertical") > 0 ? 0 : i;
                i = Input.GetAxis("Vertical") < 0 ? 1 : i;

                var pos = SelectedOBject.transform.position;
                var movespee = (zoomCamera.orthographicSize * 0.2f);
                SelectedOBject.transform.position = new Vector3(pos.x + (i == 0 ? -movespee * Time.deltaTime : 0), pos.y, pos.z + (i == 2 ? -movespee * Time.deltaTime : 0));
                pos = SelectedOBject.transform.position;
                SelectedOBject.transform.position = new Vector3(pos.x + (i == 1 ? movespee * Time.deltaTime : 0), pos.y, pos.z + (i == 3 ? movespee * Time.deltaTime : 0));

            }


        }

        if (MainPlane != null)
        {
            if (Input.GetAxis("Horizontal2") != 0 || Input.GetAxis("Vertical2") != 0)
            {

                var pos = MainPlane.transform.position;
                var movespee = 1050f;
                MainPlane.transform.position = new Vector3(pos.x + (movespee * Time.deltaTime * -Input.GetAxis("Vertical2")), pos.y, pos.z + (movespee * Time.deltaTime * Input.GetAxis("Horizontal2")));

            }
            else
            {
                //var i = movebtn;
                //i = Input.GetAxis("Horizontal2") > 0 ? 3 : i;
                //i = Input.GetAxis("Horizontal2") < 0 ? 2 : i;
                //i = Input.GetAxis("Vertical2") > 0 ? 0 : i;
                //i = Input.GetAxis("Vertical2") < 0 ? 1 : i;

                //var pos = MainPlane.transform.position;
                //var movespee = 1050f;
                //MainPlane.transform.position = new Vector3(pos.x + (i == 0 ? -movespee * Time.deltaTime : 0), pos.y, pos.z + (i == 2 ? -movespee * Time.deltaTime : 0));
                //pos = MainPlane.transform.position;
                //MainPlane.transform.position = new Vector3(pos.x + (i == 1 ? movespee * Time.deltaTime : 0), pos.y, pos.z + (i == 3 ? movespee * Time.deltaTime : 0));

            }


        }

        if (SelectedOBject != null)
        {
            var ang = angleSlider.value;
            if (Input.GetKey(KeyCode.Keypad1))
            {
                if (ang > 0)
                {
                    ang -= Time.deltaTime*20f;
                }
                ang = ang < 0 ? 0 : ang;
                angleSlider.value = ang;
            }else
            if (Input.GetKey(KeyCode.Keypad2))
            {
                if (ang < 360)
                {
                    ang += Time.deltaTime * 20f;
                }
                ang = ang > 360 ? 360 : ang;
                angleSlider.value = ang;
            }
            
            padAngle.text = ang.ToString("0.0") + "°";
            SelectedOBject.transform.eulerAngles = new Vector3(0, ang, 0);
        }

        if (SelectedOBject != null)
        {
            var alt = altiudeSlider.value;
            if (Input.GetKey(KeyCode.Keypad3))
            {
                if (alt > 0)
                {
                    alt -= Time.deltaTime * 200.0f;
                }
                alt = alt < 0 ? 0 : alt;
                altiudeSlider.value = alt;
            }
            else
            if (Input.GetKey(KeyCode.Keypad6))
            {
                if (alt < 10000)
                {
                    alt += Time.deltaTime * 200f;
                }
                alt = alt > 10000 ? 100000 : alt;
                altiudeSlider.value = alt;
            }

            padAltitude.text = alt.ToString("00.00") + "°";
            
        }
    }

    public void topzoomAllow(bool allow)
    {
        zoomInAllow = allow;
    }

    public void topzoomoutAllow(bool allow)
    {
        zoomOutAllow = allow;
    }

    

    public void topviewZoomIN(Camera cam)
    {
        var i = 0;
        int low = 500, max = 60000;
        if (i>0&&cam.orthographicSize<max)
        {
            cam.orthographicSize += 500f;
        }else
        if(i < 1 && cam.orthographicSize > low)
        {
            cam.orthographicSize -= 50.0f;
        }
        cam.transform.localPosition = new Vector3(0, 0, cam.orthographicSize - 100f);
        var pos = cam.transform.position;
        pos.y = 90000;
        cam.transform.position = pos;
        var z = cam.orthographicSize * 2f;
        TOPViewRange.text = z.ToString() + "m *" + z.ToString()+"m";
    }
    public void topviewZoomOUT(Camera cam)
    {
        var i = 1;
        int low = 500, max = 60000;
        if (i > 0 && cam.orthographicSize < max)
        {
            cam.orthographicSize += 500f;
        }
        else
        if (i < 1 && cam.orthographicSize > low)
        {
            cam.orthographicSize -= 50.0f;
        }
        cam.transform.localPosition = new Vector3(0,0,cam.orthographicSize-100f);
        var pos = cam.transform.position;
        pos.y = 90000;
        cam.transform.position = pos;
        var z = cam.orthographicSize * 2f;
        TOPViewRange.text = z.ToString() + "m *" + z.ToString()+"m";
    }

    public void logView()
    {
        
    }
    #region sidemenu


    public void setJem(bool i)
    {
        var jam = SelectedOBject.GetComponent<JammerTest>();
        if (jam != null)
        {
            jam.JammerActive = i;
            

        }
    }

    public void setMove(bool i)
    {
        SelectedOBject.GetComponent<Rigidbody>().isKinematic = !i;
        var m = SelectedOBject.GetComponent<testtank>();
        if (m != null)
        {
            m.speed = i ? 2 : 0;


        }
    }

    public void setJemfreq(int i)
    {
        var jam = SelectedOBject.GetComponent<JammerTest>();
        if (jam != null)
        {
            jam.jamFrequency = (Jammer.JamFrequency)i;

        }
    }

    public void padAngleSet(float i)
    {
        padAngle.text=i.ToString()+"°";
        if (SelectedOBject != null)
        {
            SelectedOBject.transform.eulerAngles = new Vector3(0,i,0);
        }
        
    }
    public void padAltitudeset(float i)
    {
        padAltitude.text = i.ToString();
        if (SelectedOBject != null)
        {

            var pos = SelectedOBject.transform.position;
            SelectedOBject.transform.position = new Vector3(pos.x, i, pos.z);

        }
    }

    public void padMoveBtn(int i)
    {
        movebtn = i;
        return;
        if (SelectedOBject != null)
        {
            var pos = SelectedOBject.transform.position;
            SelectedOBject.transform.position = new Vector3(pos.x+(i==0?-1000:0), pos.y, pos.z+(i == 2 ? -100 : 0));
            pos = SelectedOBject.transform.position;
            SelectedOBject.transform.position = new Vector3(pos.x + (i == 1 ? 1000 : 0), pos.y, pos.z + (i == 3 ? 1000 : 0));


        }
    }

    public void iffcodeSet(string i)
    {
        if (SelectedOBject != null)
        {
            int number;
            bool isParsable = Int32.TryParse(aircraft_IFF_Code.text, out number);
            if (isParsable)
            {
                SelectedOBject.GetComponent<Specification>().iff = number;
                string[] name = SelectedOBject.name.Split("_");
                
                SelectedOBject.name = name[0] + "_" + i + "_" + name[2];
                SelectedUI.GetChild(0).GetComponent<TextMeshProUGUI>().text = name[0] + "_" + i + "_" + name[2];
            }
            else
            {
                aircraft_IFF_Code.text = SelectedOBject.GetComponent<Specification>().iff.ToString();
            }
        }

    }
    

    public void sliderUpandDown(bool i)
    {
        sliderOpen = i;
        RadarPanel.SetBool("big", i);
        slider.SetBool("slide", i);
    }

    public void AddItem()
    {
        if (aircraftDropdown.value < 1) { return; }


        Transform aircraft = null;
        //if (aircraftDropdown.value == 1)
        //{
        //    aircraft = Instantiate(Aircrafts[0].transform);

        //}
        //else
        //if (aircraftDropdown.value == 2)
        //{
        //    aircraft = Instantiate(Aircrafts[1].transform);
        //}
        var numm = 0;
        foreach (var item in Aircrafts)
        {
            numm++;
            if(aircraftDropdown.value == numm)
            {
                aircraft = Instantiate(item.transform);
            }
        }
        if (aircraft == null) { return; }
        gizmoshow gizmow = aircraft.GetComponent<gizmoshow>();
        if (gizmow != null)
        {
            gizmow.cm = cams;
        }
        numplane++;
        if (aircraft_IFF_Code != null)
        {
            int number;

            bool isParsable = Int32.TryParse(aircraft_IFF_Code.text, out number);
            if (isParsable)
                aircraft.GetComponent<Specification>().iff = number;
        }
        //aircraft.localPosition = MainPlane.position + MainPlane.forward * 80;
        aircraft.eulerAngles = new Vector3(0, 90, 0);

        //aircraftDropdown.value = 0;
        String code = "";
        if (aircraft_IFF_Code != null)
        {
            code = "_"+aircraft.GetComponent<Specification>().iff.ToString("0");
        }
        var jam = aircraft.GetComponent<JammerTest>();
        if (jam != null)
        {
            jam.JammerActive = jem.isOn;
            jam.jamFrequency = (Jammer.JamFrequency)jamFeq.value;
        }

        var obj = Instantiate(ListPrefab);
        obj.gameObject.name = aircraftDropdown.options[aircraftDropdown.value].text;
        var name = aircraftDropdown.options[aircraftDropdown.value].text + code + "_" + numplane;
        aircraft.name = name;
        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        obj.transform.parent = listContentView;
        obj.localScale = new Vector3(1, 1, 1);
        obj.localPosition = new Vector3(obj.GetComponent<RectTransform>().sizeDelta.x / 2 +2.72f, -listContentView.transform.GetComponent<RectTransform>().sizeDelta.y, 0);
        listContentView.transform.GetComponent<RectTransform>().sizeDelta = listContentView.transform.GetComponent<RectTransform>().sizeDelta + new Vector2(0, obj.GetComponent<RectTransform>().sizeDelta.y);
        //StartCoroutine(updateTimer(obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), obj.gameObject, 55));
        obj.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { Destroy(AircraftList[obj.gameObject]); AircraftList.Remove(obj.gameObject); Destroy(obj.gameObject); Invoke("updateUItransform", 0.3f); });
        obj.GetComponent<Button>().onClick.AddListener(() => { updateUIColor(obj.gameObject); });
        AircraftList.Add(obj.gameObject, aircraft.gameObject);
    }

    public void ClearObjects()
    {
        foreach (var obj in AircraftList)
        {
            Destroy(obj.Value);
            Destroy(obj.Key);
        }
        AircraftList.Clear();
        
        Invoke("updateUItransform", 0.3f);
    }
    IEnumerator updateTimer(TextMeshProUGUI timer,GameObject obj,int i)
    {
        yield return new WaitForSeconds(1f);
        i--;
        if (i < 0)
        {
            if (obj != null)
            {
                //Destroy(obj);
                Destroy(AircraftList[obj]);
                AircraftList.Remove(obj.gameObject);
                Destroy(timer.transform.parent.gameObject);


                Invoke("updateUItransform", 0.3f);
                
                
            }
        }
        else
        {
            if (obj != null)
            {
                timer.text = i.ToString();
            }
            StartCoroutine(updateTimer(timer, obj, i));
        }
    }
    public void updateUItransform()
    {
        listContentView.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(listContentView.transform.GetComponent<RectTransform>().sizeDelta.x, listContentView.transform.childCount * 22);

        for (int j = 0; j < listContentView.transform.childCount; j++)
        {
            var c = listContentView.transform.GetChild(j);
            c.localPosition = new Vector3(c.GetComponent<RectTransform>().sizeDelta.x/2, -j * c.GetComponent<RectTransform>().sizeDelta.y, 0);
        }
        if(SelectedOBject == null)
        {
            Pad.gameObject.SetActive(false);
        }
        
    }

    

    public void updateUIColor(GameObject obj)
    {
        Pad.gameObject.SetActive(true);
        SelectedOBject = AircraftList[obj].transform;
        SelectedUI = obj.transform;
        for (int j = 0; j < listContentView.transform.childCount; j++)
        {
            var c = listContentView.transform.GetChild(j);
            c.GetComponent<Image>().color = Color.black;
            //AircraftList[c.gameObject].GetComponent<gizmoshow>().select = false;
        }
        foreach(var cm in AircraftList)
        {
            cm.Value.GetComponent<gizmoshow>().select = false;
        }
        AircraftList[obj].GetComponent<gizmoshow>().select = true;
        obj.GetComponent<Image>().color = Color.gray;
        padName.text = obj.name;

        padAngle.text = SelectedOBject.transform.eulerAngles.y.ToString() + "°";
        angleSlider.value = SelectedOBject.transform.eulerAngles.y;

        padAltitude.text = SelectedOBject.transform.position.y.ToString();
        altiudeSlider.value = SelectedOBject.transform.position.y;

        aircraft_IFF_Code.text = SelectedOBject.GetComponent<Specification>().iff.ToString();
        var jam = SelectedOBject.GetComponent<JammerTest>();
        if (jam != null)
        {
            jem.isOn = jam.JammerActive;
            jamFeq.value = (int)jam.jamFrequency;
        }
    }
    #endregion



}
