#region Script info
//input : Take Data from joystick and keyboard
//output : store the data on Global variables and transfer them to those script that needed.
#endregion
using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace InputActions
{
    public class InputActionManager2_script : MonoBehaviour
    {
        #region Required Data
        PlayerControle inputs;
        GameManagerInput menuControler;
        #endregion

        #region Global Data
        [Header("Manager Reference")]
        public Manager exitGame;
        //joystick inputs data
        [Header("Joystick Data")]
        public float rMove;
        public float lMove;
        public float uMove;
        public float dMove;
        //public float yaw;
        public float yawleft;
        public float yawright;
        public float thrust;
        public bool flareFire;
        public bool Airbrake;
        #endregion

        #region Necessary Functions
        private void OnEnable()
        {
            inputs.Enable();
            menuControler.Enable();
        }
        private void OnDisable()
        {
            inputs.Disable();
            menuControler.Disable();
        }
        private void Awake()
        {
            inputs = new PlayerControle();
            menuControler = new GameManagerInput();
        }
        void Start()
        {
            #region joystick_inp
            //move
            inputs.JoystickInput.RL.performed += OnRightLeft;
            inputs.JoystickInput.UD.performed += OnUpDown;
            inputs.JoystickInput.S.performed += OnThrust;
            inputs.PlaneMove.YawL.performed += _yawLeft => yawleft = _yawLeft.ReadValue<float>();
            inputs.PlaneMove.YawL.canceled += _ => yawleft = 0;

            inputs.PlaneMove.YawR.performed += _yawRight => yawright = _yawRight.ReadValue<float>();
            inputs.PlaneMove.YawR.canceled += _ => yawright = 0;

            inputs.PlaneMove.Airbrake.performed += _ => Airbrake = true;
            inputs.PlaneMove.Airbrake.canceled += _ => Airbrake = false;
            //fire_flare
            inputs.JoystickInput.Flares.performed += _ => flareFire = true;
            inputs.JoystickInput.Flares.canceled += _ => flareFire = false;
            #endregion
            #region MenuControle
            menuControler.ExitBtn.ExitGame.performed += ExitGame;
            #endregion
        }

        #endregion

        #region Created Logics Functions
        /// <summary>
        /// Thrust Function
        /// </summary>
        /// <param name="_thrust"></param>
        private void OnThrust(InputAction.CallbackContext _thrust)
        {
            float inputdata = _thrust.ReadValue<float>();
            //thrust = inputdata + 1; //Anoop
            thrust = inputdata;
        }

        /// <summary>
        /// Move UpDown the plane
        /// </summary>
        /// <param name="_upDown"></param>
        private void OnUpDown(InputAction.CallbackContext _upDown)
        {
            float inputdata = _upDown.ReadValue<float>();
            if (inputdata > 0)
            {
                uMove = (inputdata - 1);
                dMove = 0;
            }
            else
            {
                dMove = inputdata + 1;
                uMove = 0;
            }
        }

        /// <summary>
        /// Move Left right the plane.
        /// </summary>
        /// <param name="_rightLeft"></param>
        private void OnRightLeft(InputAction.CallbackContext _rightLeft)
        {
            float inputdata = _rightLeft.ReadValue<float>();
            if (inputdata > 0)
            {
                rMove = (inputdata - 1);
                lMove = 0;
            }
            else
            {
                lMove = inputdata + 1;
                rMove = 0;
            }
        }
        /// <summary>
        /// On Exit Game Clicked.
        /// </summary>
        /// <param name="_exit"></param>
        private void ExitGame(InputAction.CallbackContext _exit)
        {
            if (exitGame == null) return;
            else
            {
                exitGame.ExitGame();
            }
        }
        #endregion
    }

}
