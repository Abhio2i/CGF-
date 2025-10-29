using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data.Plane;
using UI.PlaneSpecs;
namespace Utility.LatLonAlt
{
    public class LatLong : MonoBehaviour
    {
        // x = long
        // y = lat

        public Vector2 UpperLeftLatLong = new Vector2(15.712f, 78.7203f);
        public Vector2 LowerRightLatLong = new Vector2(13.8425f, 80.6539f);
        public Vector2 CenterLatLong = new Vector2(14.7793f, 79.6871f);
        public Vector2 UpperLeftPos = new Vector2(-104065f, 104065f);
        public Vector2 LowerRightPos = new Vector2(104065f, -104065f);
        public Vector2 size = new Vector2(208130f, 208130f);
        [Space]
        [Space]
        public Vector2 UpperLeftLatLong2 = new Vector2(15.712f, 78.7203f);
        public Vector2 LowerRightLatLong2 = new Vector2(13.8425f, 80.6539f);
        public Vector2 CenterLatLong2 = new Vector2(14.7793f, 79.6871f);
        public Vector2 UpperLeftPos2 = new Vector2(-104065f, 104065f);
        public Vector2 LowerRightPos2 = new Vector2(104065f, -104065f);
        public Vector2 size2 = new Vector2(208130f, 208130f);
        /*
        [SerializeField] PlaneData planeBody;
        [SerializeField] PlaneSpecs planeUI;

        public float oldMapX = 500f;
        public float oldMapY = 500f;

        private float newMapx;
        private float newMapy;

        private float mapX;
        private float mapY;

        [SerializeField]private Vector3 _lat;
        [SerializeField]private Vector3 _lon;

        private UnityEngine.Vector3 positionDifferent;
        [SerializeField] private GameObject mapCenter;
        [SerializeField]private GameObject refrencePlane;
        private void Start()
        {
            //refrencePlane = planeBody.body;
            newMapx = oldMapX / 90;
            newMapy = oldMapY / 180;
        }
        */
        public Vector2 worldToLatLong(Vector2 world)
        {
            float Latdis = UpperLeftLatLong.x - LowerRightLatLong.x;
            float Longdis = UpperLeftLatLong.y - LowerRightLatLong.y;
            float worldlatdis = UpperLeftPos.x - LowerRightPos.x;
            float worldLongdis = UpperLeftPos.y - LowerRightPos.y;
            float xdis = UpperLeftPos.x - world.x;
            float ydis = UpperLeftPos.y - world.y;
            float xpercent = xdis / worldlatdis;
            float ypercent = ydis / worldLongdis;
            return new Vector2(UpperLeftLatLong.x - (ypercent * Latdis), UpperLeftLatLong.y - (xpercent * Longdis));
        }
        /*
        private void Update()
        {
            positionDifferent = mapCenter.transform.position - refrencePlane.transform.position;
            mapX = positionDifferent.x / newMapx;
            mapY = positionDifferent.z / newMapy;

            string lat = LatLongtoDegree(Mathf.Abs(mapX));
            string lon = LatLongtoDegree(Mathf.Abs(mapY));
            string alt = Mathf.Abs((int)(positionDifferent.y * 3.28084f)).ToString();
            if (mapX > 0)
            {
                //s
                lat += " s";
            }
            else
            {
                //n
                lat += " n";
            }

            if (mapY > 0)
            {
                //w
                lon += " w";
            }
            else
            {
                //e
                lon += " e";
            }
            //print(lat + " "+lon +" "+alt);
            planeUI.lat = lat;
            planeUI.lon = lon;
            planeUI.alt = alt + " Ft";
            planeUI.speed = ((int)planeBody.bodySpeed).ToString();
        }
        

        private string LatLongtoDegree(float _latlon)
        {
            int deg = (int)_latlon;
            float _min = (_latlon - deg) * 100 / 60;
            int min = (int)_min;
            float _sec = (_min - min) * 100 / 60;
            string output = deg.ToString() + "°" + min.ToString() + "'" + Math.Round(_sec, 2).ToString() + '"';
            return output;
        }
        */

    }
}

