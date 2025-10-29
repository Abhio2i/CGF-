using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
namespace newTest
{
    public class Test : MonoBehaviour
    {
        public float maxForce = 1;

        public string axisNameX = "", axisNameY = ""; // paste axes' Names from InputManager
        public float sensitivity = .1f; // paste value from InputManager. Must be lesser than or equal to .3254!
        public bool invertedX = false; // check if checked in the InputManager
        public bool invertedY = true; // ditto
        public bool doubleY = false; // for right stick

        private float eps;
        private Transform eye;

        void Start()
        {
            eps = 2 * sensitivity / 127;
            //eye = transform.FindChild("eye");
        }
        public float xMap, yMap;
        public float temp;
        bool passedOnce;
        void FixedUpdate()
        {
            // get input force from stick
            Vector3 force = new Vector3(0f, 0f, 0f);
            if (axisNameX.Length > 0)
            {
                float inv = invertedX ? -1 : 1;
                xMap = inv * ((((inv * Input.GetAxis(axisNameX) + 3f * sensitivity) / 2f) % (2f * sensitivity + eps / 3f)) / sensitivity - 1f);
                force += new Vector3(xMap * maxForce, 0f, 0f);
            }
            if (axisNameY.Length > 0)
            {
                float inv = invertedY ? -1 : 1;
                if (doubleY)
                    yMap = inv * ((((inv * Input.GetAxis(axisNameY) + 3f * sensitivity) / 2f) % (2f * sensitivity + eps / 3f)) / sensitivity - 1.5f) * 2;
                else
                    yMap = inv * ((((inv * Input.GetAxis(axisNameY) + 3f * sensitivity) / 2f) % (2f * sensitivity + eps / 3f)) / sensitivity - 1f);
            }

            temp = Input.GetAxisRaw("Throttle");
            temp = Mathf.Round(temp * 10.0f) * 0.1f;
            temp = 1 - temp;
        }
        void TriggerVal()
        {
            passedOnce = !passedOnce;
        }
    }
}