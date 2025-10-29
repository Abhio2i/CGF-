#region Script Info
//inp: Plane Data (Current position and rotation).
//out: The UI altitude meter is move as the plane moves.
#endregion
using UnityEngine;
using Data.Plane;
namespace UI.FlightInstruments
{
    public class AltitudeMeter : MonoBehaviour
    {
        #region Global Perimeters
        public PlaneData data;
        public Specification specification;
        public RectTransform BackgroundRect;
        public RectTransform ArrowRect;
        public RectTransform globex;
        public RectTransform globey;
        #endregion

        #region Local Function
        private void Update()
        {
            HandleAirplaneUI();
        }
        #endregion

        #region Global Function
        public void HandleAirplaneUI()
        {
            if (data.body != null)
            {
                ////create angles

                //float pitchAngle = UnityEngine.Vector3.Dot(data.body.transform.forward, UnityEngine.Vector3.up) * Mathf.Rad2Deg;
                //float bankAngle = UnityEngine.Vector3.Dot(data.body.transform.right, UnityEngine.Vector3.up) * Mathf.Rad2Deg;

                //if (BackgroundRect)
                //{
                //    Quaternion bankRotation = Quaternion.Euler(0f, 0f, -bankAngle);
                //    BackgroundRect.transform.rotation = bankRotation;

                //    UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(0f, pitchAngle, 0f);
                //    //BackgroundRect.anchoredPosition = -targetPosition;


                //    if (ArrowRect)
                //    {
                //        ArrowRect.transform.rotation = bankRotation;
                //    }
                //    if (globex)
                //    {
                //        globex.transform.localRotation = bankRotation;
                //    }
                //    if (globey)
                //    {
                //        globey.transform.rotation = Quaternion.Euler(-pitchAngle,0,0);
                //    }
                //}
                BackgroundRect.transform.localEulerAngles = new Vector3(0,0,specification.entityInfo.Roll);
                globex.transform.localEulerAngles = new Vector3(specification.entityInfo.Pitch, 0, 0);

            }

        }
        #endregion
    }

}
