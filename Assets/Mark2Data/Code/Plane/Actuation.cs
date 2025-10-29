using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Code.Plane
{
    public class Actuation : MonoBehaviour
    {
        public GameObject plane;
        public Animator takeOff_Landing;
        private bool trigger=true;
        private SilantroControl inputActions;

        [SerializeField]WheelCollider front,rearLeft,rearRight;
        // Use this for initialization
        private void Awake()
        {
            inputActions = new SilantroControl();
            inputActions.Gear.OnOff.performed += _ => { CheckAnimation(); };

        }
        private void OnEnable()
        {
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }
        public void CheckAnimation()
        {
            if (trigger)
            {
                //takeOff_Landing.SetTrigger("TakeOff");
                takeOff_Landing.SetBool("s", trigger);
                //front.enabled=rearLeft.enabled=rearRight.enabled=false;
                trigger=false;
            }
            else
            {
                takeOff_Landing.SetBool("s", trigger);
                //takeOff_Landing.SetTrigger("Landing");
                //front.enabled = rearLeft.enabled = rearRight.enabled = true;
                trigger = true;
            }
        }
    }
}