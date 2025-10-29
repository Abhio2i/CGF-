using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackCameraController : MonoBehaviour
{
    public Camera camera;
    [SerializeField]
    public static Transform Target = null;
    public  Vector3 offset = Vector3.zero;
    public  float distance = 10;
    public  float Smoothness = 1.0f;
    [SerializeField]
    public static bool ThirdPerson = false;
    /// <summary>
    /// Normal speed of camera movement.
    /// </summary>
    public float movementSpeed = 10f;

    /// <summary>
    /// Speed of camera movement when shift is held down,
    /// </summary>
    public float fastMovementMultiplier = 100f;

    /// <summary>
    /// Sensitivity for free look.
    /// </summary>
    public float freeLookSensitivity = 3f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel.
    /// </summary>
    public float zoomSensitivity = 10f;
    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel (fast mode).
    /// </summary>
    public float fastZoomSensitivity = 50f;
    public float MinZoom = 0;
    public float MaxZoom = 0;


    /// <summary>
    /// Set to true when free looking (on right mouse button).
    /// </summary>

    private bool looking = false;
    public float ver = 0;
    public float hor = 0;
    public float elv = 0;
    public float mousX = 0;
    public float mousY = 0;
    public float Speed = 0;
    public float Zoom = 0;
    public bool SpeedUp = false;
    public bool locking = false;
    private void Start()
    {
    }
    public void SetVerticle(float value)
    {
        ver = value;
    }
    public void SetHorizontal(float value)
    {
        hor = value;
    }
    public void SetElevation(float value)
    {
        elv = value;
    }
    public void SetSpeedUp(bool value)
    {
        SpeedUp = value;
        if(SpeedUp)
        {
            Speed = movementSpeed * fastMovementMultiplier;
        }
        else
        {
            Speed = movementSpeed;
        }
    }

    public void SetMouseX(float value)
    {
        mousX = value;
    }
    public void SetMouseY(float value)
    {
        mousY = value;
    }

    public void SetMouseLock(bool value)
    {
        locking = value;
        if (locking)
        {
            StartLooking();
        }
        else
        {
            StopLooking();
        }
    }
    public void SetZoom(float value)
    {
        if (locking)
        {
            Zoom += -value * zoomSensitivity;
            Zoom = Zoom > MinZoom ? MinZoom : Zoom;
            Zoom = Zoom < MaxZoom ? MaxZoom : Zoom;
        }
        
    }

    public void GoToPosition(Transform obj)
    {
        transform.LookAt(obj);
        transform.position = obj.position;
    }

    void FixedUpdate()
    {
        if (ThirdPerson)
        {
            Vector3 position = Vector3.Lerp(transform.position, Target.position,Smoothness*Time.fixedDeltaTime);
            transform.LookAt(position);
            transform.position = position +(Target.forward*-distance)+(Target.forward*offset.z)+ (Target.right * offset.x)+(Target.up * offset.y);

        }
        else
        if (looking)
        {
            Vector3 position = transform.position + (hor * transform.right * Speed * Time.fixedDeltaTime)
                                           + (ver * transform.forward * Speed * Time.fixedDeltaTime)
                                           + (elv * transform.up * Speed * Time.fixedDeltaTime);
            transform.position =  position;
            transform.localEulerAngles = transform.localEulerAngles + (new Vector3(mousY, mousX, 0) * freeLookSensitivity);
            camera.fieldOfView = Zoom;
        }



        /*
        var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + (-transform.right * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = transform.position + (-transform.forward * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position = transform.position + (transform.up * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position = transform.position + (-transform.up * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.PageUp))
        {
            transform.position = transform.position + (Vector3.up * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.PageDown))
        {
            transform.position = transform.position + (-Vector3.up * movementSpeed * Time.deltaTime);
        }

        if (looking)
        {
            float newRotationX = Input.GetAxis("Mouse X") * freeLookSensitivity;
            float newRotationY = Input.GetAxis("Mouse Y") * freeLookSensitivity;
            transform.Rotate(-newRotationY, newRotationX, 0f);
        }

        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            var zoomSensitivity = fastMode ? this.fastZoomSensitivity : this.zoomSensitivity;
            transform.position = transform.position + transform.forward * axis * zoomSensitivity;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }
        */
    }

    void OnDisable()
    {
        StopLooking();
    }

    /// <summary>
    /// Enable free looking.
    /// </summary>
    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Disable free looking.
    /// </summary>
    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
