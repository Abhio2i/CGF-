#region Script Info
//out: If radar find any plane then this script is going to Place the Ping on the UI at that location where the radar is find the plane.
#endregion
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI.PingRespon
{
    public class PingRespond : MonoBehaviour
    {
        #region Global Perimeters
        public Image pingImg;
        public Transform target;
        public Transform radarPos;
        public Vector3 pos;
        public bool IFFCode;
        public bool isIFFCheck;
        public Camera camera;
        #endregion

        #region local Perimeters
        private TMP_Text distanceText;
        private bool isStart = false;
        #endregion

        #region local Functions
        private void Start()
        {
            if (isIFFCheck) 
            {
                if (IFFCode)
                {

                    pingImg.color = Color.green;
                }
                else
                {
                    pingImg.color = Color.red;
                }
            }
            
            distanceText = transform.GetChild(0).GetComponent<TMP_Text>();
            isStart = true;
            Destroy(gameObject, 1f);
        }
        private void Update()
        {
            if (!isStart || radarPos == null) return;
            if (target == null && pingImg == null) return;
            float minX = pingImg.GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = pingImg.GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minY;
            Vector2 pingPos = camera.WorldToScreenPoint(target.position);

            if (UnityEngine.Vector3.Dot((pos), radarPos.forward) < 0)
            {
                if (pingPos.x < Screen.width / 2)
                {
                    pingPos.x = maxX;
                }
                else
                {
                    pingPos.x = minX;
                }
            }

            pingPos.x = Mathf.Clamp(pingPos.x, minX, maxX);
            pingPos.y = Mathf.Clamp(pingPos.y, minY, maxY);

            pingImg.transform.position = pingPos;

            distanceText.text = UnityEngine.Vector3.Distance(target.position, radarPos.position).ToString("00");
        }
        #endregion
    }

}