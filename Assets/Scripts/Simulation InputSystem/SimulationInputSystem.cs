using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationInputSystem : MonoBehaviour
{
    public PlayerControle planeContole;
    public FeedBackRecorderAndPlayer feedback;
    public FeedBackCameraController feedbackCameraController;

    private void OnEnable()
    {
        planeContole.Enable();
    }

    private void OnDisable()
    {
        planeContole.Disable();
    }
    private void Awake()
    {
        planeContole = new PlayerControle();
    }

    // Start is called before the first frame update
    void Start()
    {
        planeContole.Feedback.Pause.performed += (ctx) => { feedback.PauseRecording(); };
        planeContole.Feedback.BirdEye.performed += (ctx) => { feedback.BirdEye(); };

        planeContole.Feedback.ViewNext.performed += (ctx) => { feedback.SelectViewAircraft(1); };
        planeContole.Feedback.ViewPrev.performed += (ctx) => { feedback.SelectViewAircraft(-1); };

        planeContole.Feedback.camForward.performed += (ctx) => { feedbackCameraController.SetVerticle(ctx.ReadValue<float>()); };
        planeContole.Feedback.camForward.canceled += (ctx) => { feedbackCameraController.SetVerticle(ctx.ReadValue<float>()); };
        planeContole.Feedback.cambackward.performed += (ctx) => { feedbackCameraController.SetVerticle(-ctx.ReadValue<float>()); };
        planeContole.Feedback.cambackward.canceled += (ctx) => { feedbackCameraController.SetVerticle(ctx.ReadValue<float>()); };

        planeContole.Feedback.camright.performed += (ctx) => { feedbackCameraController.SetHorizontal(ctx.ReadValue<float>()); };
        planeContole.Feedback.camright.canceled += (ctx) => { feedbackCameraController.SetHorizontal(ctx.ReadValue<float>()); };
        planeContole.Feedback.camleft.performed += (ctx) => { feedbackCameraController.SetHorizontal(-ctx.ReadValue<float>()); };
        planeContole.Feedback.camleft.canceled += (ctx) => { feedbackCameraController.SetHorizontal(ctx.ReadValue<float>()); };

        planeContole.Feedback.camright.performed += (ctx) => { feedbackCameraController.SetHorizontal(ctx.ReadValue<float>()); };
        planeContole.Feedback.camright.canceled += (ctx) => { feedbackCameraController.SetHorizontal(ctx.ReadValue<float>()); };
        planeContole.Feedback.camleft.performed += (ctx) => { feedbackCameraController.SetHorizontal(-ctx.ReadValue<float>()); };
        planeContole.Feedback.camleft.canceled += (ctx) => { feedbackCameraController.SetHorizontal(ctx.ReadValue<float>()); };

        planeContole.Feedback.camup.performed += (ctx) => { feedbackCameraController.SetElevation(ctx.ReadValue<float>()); };
        planeContole.Feedback.camup.canceled += (ctx) => { feedbackCameraController.SetElevation(ctx.ReadValue<float>()); };
        planeContole.Feedback.camdown.performed += (ctx) => { feedbackCameraController.SetElevation(-ctx.ReadValue<float>()); };
        planeContole.Feedback.camdown.canceled += (ctx) => { feedbackCameraController.SetElevation(ctx.ReadValue<float>()); };

        planeContole.Feedback.mouseX.performed += (ctx) => { feedbackCameraController.SetMouseX(ctx.ReadValue<float>()); };
        planeContole.Feedback.mouseX.canceled += (ctx) => { feedbackCameraController.SetMouseX(ctx.ReadValue<float>()); };
        planeContole.Feedback.mouseY.performed += (ctx) => { feedbackCameraController.SetMouseY(-ctx.ReadValue<float>()); };
        planeContole.Feedback.mouseY.canceled += (ctx) => { feedbackCameraController.SetMouseY(ctx.ReadValue<float>()); };

        planeContole.Feedback.SpeedUp.performed += (ctx) => { feedbackCameraController.SetSpeedUp(true); };
        planeContole.Feedback.SpeedUp.canceled += (ctx) => { feedbackCameraController.SetSpeedUp(false); };

        planeContole.Feedback.mouseLock.performed += (ctx) => { feedbackCameraController.SetMouseLock(true); };
        planeContole.Feedback.mouseLock.canceled += (ctx) => { feedbackCameraController.SetMouseLock(false); };

        planeContole.Feedback.zoom.performed += (ctx) => { feedbackCameraController.SetZoom(ctx.ReadValue<float>()); };
        planeContole.Feedback.zoom.canceled += (ctx) => { feedbackCameraController.SetZoom(ctx.ReadValue<float>()); };

    }


}
