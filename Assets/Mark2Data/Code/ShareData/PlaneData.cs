#region About
//input - take all parameter required from plane.
//output - any script needed the plane data they take from hear.
//work - the script provide the plane data to any other script if they required.
#endregion

using UnityEngine;

namespace Data.Plane
{
    public class PlaneData : MonoBehaviour
    {
        #region Public player Reference
        // Collect Plane Current Location;
        //[SerializeField] AirplaneController planeData;
        [SerializeField] MapLatLong planLatLon;
        //[HideInInspector]
        public GameObject body;
        [HideInInspector] public float bodySpeed;
        //[HideInInspector]
        public Vector2 latLong;
        [HideInInspector] public Vector3 planRotation;
        [HideInInspector] public float planeFuel;
        #endregion
        #region Private Data
        //private readonly static float mpsToKnot = 1.852f;
        #endregion

        #region NecessaryFunction
        private void Update()
        {
            if (body == null) return;
            bodySpeed = body.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            latLong = planLatLon.CalculateLatLong(body.transform.position.z,body.transform.position.x);
            //print(latLong);
            //roll, pitch, yaw.
            planRotation = new Vector3(body.transform.rotation.z, body.transform.rotation.x, body.transform.rotation.y);
        }
        #endregion
    }

}
