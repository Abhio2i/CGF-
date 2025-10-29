#region Script Info
//input : Navigation Aid information is provided.
//output : The UI will behave the same as given input.
#endregion
using System.Collections;
using UnityEngine;
using TMPro;
using AirPlane.Radar;


namespace Navaid.Manager
{
    public class NavaidManager : MonoBehaviour
    {
        #region Control_Parameters

        [Header("Radar_Reference")]
        public Radar radar;
        
        [Header("Azimuth")]
        //azimuth
        public TMP_Text azimuth;
        private int azimuthCommon = 20;
        //azimuth_UP
        public GameObject AzimuthUP;
        //azimuth_Down
        public GameObject AzimuthDOWN;

        [Header("Angle")]
        //angle
        public TMP_Text angle;
        private int angleVal = 1;
        
        [Header("Bar")]
        //bar
        public TMP_Text bar;
        private int barVal = 1;
                
        [Header("IFF")]
        //IFF
        public TMP_Text iff;
        #endregion

        #region ButtonClick_Function
        //Azimuth_UP
        public void AzimuthDown()
        {
            AzimuthDownWork();
        }

        //Azimuth_Down
        public void AzimuthUp()
        {
            AzimuthUpWork();
        }

        //Angle
        public void Angle()
        {
            AngleWork();
        }

        //Bar
        public void Bar()
        {
            BarWork();
        }

        //IFF
        public void IFF()
        {
            StartCoroutine(IFFWork());
        }
        #endregion

        #region Button_Click_Respond

        private void AzimuthUpWork()
        {
            AzimuthDOWN.SetActive(true);
            azimuthCommon = radar.distance / 1600;
            if (azimuthCommon >= 20)
            {
                if (azimuthCommon == 40)
                {
                    azimuthCommon += 40;
                }
                else if (azimuthCommon == 20)
                {
                    azimuthCommon += 20;
                }
                else
                {
                    AzimuthUP.SetActive(false);
                }
            }
            radar.distance = azimuthCommon * 1600;
            azimuth.text = azimuthCommon.ToString("00");
        }
        
        private void AzimuthDownWork()
        {
            AzimuthUP.SetActive(true);
            azimuthCommon = radar.distance / 1600; // 1 mile = 1600 meter 
            if (azimuthCommon <= 80)
            {
                if (azimuthCommon == 40)
                {
                    azimuthCommon -= 20;
                }
                else if (azimuthCommon == 80)
                {
                    azimuthCommon -= 40;
                }
                else
                {
                    AzimuthDOWN.SetActive(false);
                }
            }
            radar.distance = azimuthCommon * 1600;
            azimuth.text = azimuthCommon.ToString("00");
        }
        
        private void AngleWork()
        {
            angle.text = "A\n" + angleVal.ToString();
            //10
            if (angleVal == 1)
            {
                angleVal = 3;
                radar.angle = 10;
            }
            //30
            else if (angleVal == 3)
            {
                angleVal = 6;
                radar.angle = 30;
            }
            //60
            else if (angleVal == 6)
            {
                angleVal = 1;
                radar.angle = 60;
            }
        }

        private void BarWork()
        {
            bar.text = barVal.ToString() + "\nB";
            //10
            if (barVal == 1)
            {
                barVal++;
                radar.bar = 10;
            }
            //20
            else if (barVal == 2)
            {
                barVal++;
                radar.bar = 20;
            }
            //30
            else if (barVal == 3)
            {
                barVal++;
                radar.bar = 30;
            }
            //40
            else if (barVal == 4)
            {
                barVal = 1;
                radar.bar = 40;
            }
            radar.angleUPDown = radar.bar;
        }

        private IEnumerator IFFWork()
        {
            iff.text = "M4\nScan..";

            yield return new WaitForSeconds(3f);

            iff.text = "M4";
            radar.pingColor = true;
            yield return new WaitForSeconds(2f);
            radar.pingColor = false;
            if (Display.displays.Length > 1)
            {
                // Activate the display 1 (second monitor connected to the system).
                Display.displays[1].Activate();
            }
        }


        #endregion
    }

}
