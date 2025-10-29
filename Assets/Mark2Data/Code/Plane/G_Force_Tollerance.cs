#region Script info
//input : all plane data that the pilot is going to run.
//output : the script calculate the Gravitation force on the plane according to the plane running situation.
#endregion
using UnityEngine;
using Data.Plane;
namespace AirPlane.GForce 
{
    public class G_Force_Tollerance : MonoBehaviour
    {
        #region Global Parameters
        public PlaneData planData;
        public GameObject Player;
        public float velocity;
        public float mass;
        #endregion

        #region Local Parameters
        private float noramalGravity = 9.8f;
        private float main_G_Force;

        // left or right calculation variables
        private float inertialForceY;
        public float rotationValueY;
        private float g_ForceY;
        private float centrepetalForceY;
        private float radiusY;

        //Up or down calculation variables
        private float inertialForceX;
        public float rotationValueX;
        private float g_ForceX;
        private float centrepetalForceX;
        private float radiusX;
        #endregion

        #region Local Functions
        private void Start()
        {
            Player = planData.body;
        }
        private void Update()
        {

            //for calculating g force while moving left or right with x rotation
            rotationValueY = Mathf.Tan(Player.transform.rotation.y + 0.1f);
            if (rotationValueY != 0)
            {
                radiusY = (mass * (velocity * velocity)) / (11.26f * 10f * rotationValueY);
            }
            centrepetalForceY = (velocity * velocity) / (radiusY);
            inertialForceY = centrepetalForceY / noramalGravity;
            if (velocity == 0)
            {
                g_ForceY = 1.0f;
            }



            //for calculating g force while moving down or up with y position
            rotationValueX = (Player.transform.position.y + 0.1f) / 600f;

            if (rotationValueX != 0)
            {
                radiusX = (mass * (velocity * velocity)) / (11.26f * 10000f * rotationValueX);
            }
            centrepetalForceX = (velocity * velocity) / (radiusX);
            inertialForceX = centrepetalForceX / noramalGravity;



            //calculating and merging two g_forces to get actual G-Force
            if (velocity != 0)
            {

                g_ForceY = (1 + inertialForceY);
                g_ForceX = (1 + inertialForceX);

            }

            main_G_Force = g_ForceX * g_ForceY;


            print(main_G_Force);
        }
        #endregion
    }
}
