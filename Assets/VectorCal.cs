using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class PID_Controller
{
    public enum DerivativeMeasurement
    {
        Velocity,
        ErrorRateOfChange
    }

    //PID coefficients
    public float proportionalGain;
    public float integralGain;
    public float derivativeGain;

    public float outputMin = -1;
    public float outputMax = 1;
    public float integralSaturation;
    public DerivativeMeasurement derivativeMeasurement;

    public float valueLast;
    public float errorLast;
    public float integrationStored;
    public float velocity;  //only used for the info display
    public bool derivativeInitialized;

    public void Reset()
    {
        derivativeInitialized = false;
    }

    public float Update(float dt, float currentValue, float targetValue)
    {
        if (dt <= 0) throw new ArgumentOutOfRangeException(nameof(dt));

        float error = targetValue - currentValue;

        //calculate P term
        float P = proportionalGain * error;

        //calculate I term
        integrationStored = Mathf.Clamp(integrationStored + (error * dt), -integralSaturation, integralSaturation);
        float I = integralGain * integrationStored;

        //calculate both D terms
        float errorRateOfChange = (error - errorLast) / dt;
        errorLast = error;

        float valueRateOfChange = (currentValue - valueLast) / dt;
        valueLast = currentValue;
        velocity = valueRateOfChange;

        //choose D term to use
        float deriveMeasure = 0;

        if (derivativeInitialized)
        {
            if (derivativeMeasurement == DerivativeMeasurement.Velocity)
            {
                deriveMeasure = -valueRateOfChange;
            }
            else
            {
                deriveMeasure = errorRateOfChange;
            }
        }
        else
        {
            derivativeInitialized = true;
        }

        float D = derivativeGain * deriveMeasure;

        float result = P + I + D;

        return Mathf.Clamp(result, outputMin, outputMax);
    }
    float AngleDifference(float a, float b)
    {
        return (a - b + 540) % 360 - 180;   //calculate modular difference, and remap to [-180, 180]
    }

    public float UpdateAngle(float dt, float currentAngle, float targetAngle)
    {
        if (dt <= 0) throw new ArgumentOutOfRangeException(nameof(dt));
        float error = AngleDifference(targetAngle, currentAngle);
        errorLast = error;

        //calculate P term
        float P = proportionalGain * error;

        //calculate I term
        integrationStored = Mathf.Clamp(integrationStored + (error * dt), -integralSaturation, integralSaturation);
        float I = integralGain * integrationStored;

        //calculate both D terms
        float errorRateOfChange = AngleDifference(error, errorLast) / dt;
        errorLast = error;

        float valueRateOfChange = AngleDifference(currentAngle, valueLast) / dt;
        valueLast = currentAngle;
        velocity = valueRateOfChange;

        //choose D term to use
        float deriveMeasure = 0;

        if (derivativeInitialized)
        {
            if (derivativeMeasurement == DerivativeMeasurement.Velocity)
            {
                deriveMeasure = -valueRateOfChange;
            }
            else
            {
                deriveMeasure = errorRateOfChange;
            }
        }
        else
        {
            derivativeInitialized = true;
        }

        float D = derivativeGain * deriveMeasure;

        float result = P + I + D;

        return Mathf.Clamp(result, outputMin, outputMax);
    }
}
public class VectorCal:MonoBehaviour
{
    [SerializeField] float Kp, Ki, Kd;
    [SerializeField] float windowSize = 50f;
    [SerializeField] Transform Target;

    [SerializeField] SilantroController flightController;

    PID_Controller controller;

    float pitch;

    private void Awake()
    {
        controller = new PID_Controller();
    }

    private void FixedUpdate()
    {
        controller.proportionalGain = Kp;
        controller.derivativeGain = Kd;
        controller.integralGain = Ki;

        if (transform.position.y < Target.position.y - windowSize || transform.position.y > Target.position.y + windowSize)
        {
            pitch = controller.Update(Time.fixedDeltaTime, transform.position.y, Target.position.y);
        }
        else
            pitch = 0;

        var targetPosition = Target.position;
        targetPosition.y = GetComponent<Rigidbody>().position.y;    //ignore difference in Y
        var targetDir = (targetPosition - GetComponent<Rigidbody>().position).normalized;
        var forwardDir = GetComponent<Rigidbody>().rotation * Vector3.forward;

        var currentAngle = Vector3.SignedAngle(Vector3.forward, forwardDir, Vector3.up);
        var targetAngle = Vector3.SignedAngle(Vector3.forward, targetDir, Vector3.up);

        float input = controller.UpdateAngle(Time.fixedDeltaTime, currentAngle, targetAngle);
        var roll = controller.UpdateAngle(Time.fixedDeltaTime, transform.position.x, Target.position.x);

        flightController.input.rawPitchInput=pitch;
        if (pitch == 0)
        {
            flightController.input.rawRollInput = roll;
        }
    }
}
