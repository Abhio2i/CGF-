#region script info
//inp: plane all data like lat,lon,alt,roll,pitch etc.
//output: On UI panel all the data is displaying.
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Assets.Code.ShareData
{

    public class DisplayRecord : MonoBehaviour
    {
        #region Data_And_References
        [Header("Location")]
        public TMP_Text lat;
        public TMP_Text lon;
        public TMP_Text alt;

        [Header("KillAssesment")]
        public TMP_Text missileCount;
        public TMP_Text enemyDown;
        public TMP_Text ammoCount;


        [Header("WayPointSpecs")]
        public TMP_Text distWaypoint;
        public TMP_Text waypointsRemaining; 

        [Header("BaseDistance")]
        public TMP_Text baseDistance;

        [Header("TimeAfterTakeOff")]
        public TMP_Text takeOff_Time;

        [Header("GForce")]
        public TMP_Text gForce;


        [Header("Rotation")]
        public TMP_Text roll;
        public TMP_Text pitch;
        public TMP_Text yaw;
        
        [Header("Speed")]
        public TMP_Text speedText;

        [Header("Fuel")]        
        public TMP_Text FuelText;
        
        [Header("Slider")]
        public Slider dataSlider;
 
        PointInTime pointInTime;
        
        public int count = 0;
        int tempCount = 0;
        
        bool isMoving = true;

        List<Vector3> killAssesments;
        List<Vector3> takeOffSpecs;
        List<Vector3> waypomitSpecs;
        List<float> _gForce;

        int frameDiff;

        List<Vector3> positions;
        List<Vector3> rotations;
        List<float> speed;
        List<float> fuel;

        #endregion

        #region Local Functions
        public void Awake()
        {
            pointInTime = new PointInTime();
            positions = pointInTime.Positions();
            rotations = pointInTime.Rotations();
            speed = pointInTime.GetSpeed();
            fuel = pointInTime.GetFuel();
            tempCount = 0;
            
            _gForce = pointInTime.GetGforce();
            killAssesments = pointInTime.KillAssesment();
            //print(killAssesments.Count);
            waypomitSpecs = pointInTime.GetWaypointsAssesment();
            takeOffSpecs = pointInTime.GetTimeAfterTakeOff();
            count = positions.Count;
            dataSlider.maxValue = count;
            dataSlider.onValueChanged.AddListener(delegate { ChangeTempValue(); });
        
            if(count <= 0) 
            {
                isMoving = false;
            }
        }

        private void FixedUpdate()
        {
            if (positions == null || positions.Count <= 0)
                return;

            if (isMoving && !isPause) 
            {
                StartCoroutine(DisplayDataNext());
                isMoving = false;
            }
        }
        
        
        private void ChangeTempValue()
        {
            tempCount = (int)dataSlider.value;
            isMoving = true;
        }
        [SerializeField]bool isPause=true;
        public void IsPause()
        {
            isPause = true;
        }

        public void IsPlay()
        {
            isPause = false;
        }
        private IEnumerator DisplayDataNext()
        {
            

            //lat lon alt
            lat.text = positions[tempCount].x.ToString("000.00");
            lon.text = positions[tempCount].y.ToString("000.00");
            alt.text = positions[tempCount].z.ToString("000.00");

            //rotation
            roll.text =Mathf.Abs(rotations[tempCount].x).ToString("000.00");
            pitch.text = Mathf.Abs(rotations[tempCount].y).ToString("000.00");
            yaw.text = Mathf.Abs(rotations[tempCount].z).ToString("000.00");

            //Speed
            speedText.text = speed[tempCount].ToString("0000");

            //Fuel
            FuelText.text = fuel[tempCount].ToString();

            //KillAssesment
            missileCount.text =  killAssesments[tempCount].x.ToString();
            ammoCount.text =  killAssesments[tempCount].y.ToString();
            enemyDown.text = killAssesments[tempCount].z.ToString();

            //Gforce
            gForce.text =  _gForce[tempCount].ToString("0") + "G";

            //waypoint specs
            distWaypoint.text = waypomitSpecs[tempCount].x.ToString("0");
            waypointsRemaining.text = waypomitSpecs[tempCount].z.ToString("0");

            //base dist
            baseDistance.text= waypomitSpecs[tempCount].y.ToString("0");

            //time After takeOff
            takeOff_Time.text = takeOffSpecs[tempCount].x.ToString("00") + "H:" + takeOffSpecs[tempCount].y.ToString("00") + "M:" +
                takeOffSpecs[tempCount].z.ToString("00") + "S";

            dataSlider.value = tempCount;
            yield return null;
           
                if (tempCount < count - 1)
                {
                    tempCount++;
                    isMoving = true;
                }
                
            
            
        }
        #endregion
    }
}