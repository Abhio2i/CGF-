#region Script info
//input : Take Data from joystick and keyboard
//output : store the data on Global variables and transfer them to those script that needed.
#endregion
using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace InputActions
{

    public class InputActionManager : MonoBehaviour
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
        public float thrust;
        public bool flareFire;
        public float yawleft;
        public float yawright;
        public bool Airbrake;
        [Header("Keyboard active")]
        public bool IsKeyboard = false;
        public float dynamicThrust = 200f;
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

            #region KeyboardControles
            //left right.
            if (IsKeyboard)
            {
                inputs.KeyboardControles.Right.performed += OnRight;
                inputs.KeyboardControles.Right.canceled += _ => rMove = 0f;
                inputs.KeyboardControles.Left.performed += OnLeft;
                inputs.KeyboardControles.Left.canceled += _ => lMove = 0f;

                //up down
                inputs.KeyboardControles.Up.performed += OnUp;
                inputs.KeyboardControles.Up.canceled += _ => uMove = 0f;
                inputs.KeyboardControles.Down.performed += OnDown;
                inputs.KeyboardControles.Down.canceled += _ => dMove = 0f;

                //thrust
                inputs.KeyboardControles.Thrust.performed += OnThrustAction;
            }
            #endregion
        }
        #endregion

        #region Created Logics Functions

        #region joystick_Function
        /// <summary>
        /// Thrust Function
        /// </summary>
        /// <param name="_thrust"></param>
        private void OnThrust(InputAction.CallbackContext _thrust)
        {
            float inputdata = _thrust.ReadValue<float>();
            thrust = inputdata + 1;
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
        #endregion

        #region Keyboard_Function
        /// <summary>
        /// The Keyboard input for Thrust 
        /// </summary>
        /// <param name="_thrust"></param>
        private void OnThrustAction(InputAction.CallbackContext _thrust)
        {
            float input = _thrust.ReadValue<float>();
            thrust += Time.deltaTime * input;
        }

        /// <summary>
        /// The Keyboard input for Down move.
        /// </summary>
        /// <param name="_down"></param>
        private void OnDown(InputAction.CallbackContext _down)
        {
            float input = _down.ReadValue<float>();
            dMove = -input;
            uMove = 0f;
        }

        /// <summary>
        /// The Keyboard input for Up move.
        /// </summary>
        /// <param name="_up"></param>
        private void OnUp(InputAction.CallbackContext _up)
        {
            float input = _up.ReadValue<float>();
            uMove = input;
            dMove = 0f;
        }

        /// <summary>
        /// The Keyboard input for right move.
        /// </summary>
        /// <param name="_right"></param>
        private void OnRight(InputAction.CallbackContext _right)
        {
            float input = _right.ReadValue<float>();
            rMove = input;
            lMove = 0f;
        }

        /// <summary>
        /// The Keyboard input for left move.
        /// </summary>
        /// <param name="_left"></param>
        private void OnLeft(InputAction.CallbackContext _left)
        {
            float input = _left.ReadValue<float>();
            lMove = -input;
            rMove = 0f;
        }
        #endregion

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


/*#region Script info
//input : Take Data from joystick and keyboard
//output : store the data on Global variables and transfer them to those script that needed.
#endregion
using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace InputActions
{
    public class InputActionManager : MonoBehaviour
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
        public float thrust;
        public bool flareFire;
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

}*/
