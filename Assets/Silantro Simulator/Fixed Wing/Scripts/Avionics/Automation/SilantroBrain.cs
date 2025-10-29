using Oyedoyin;
using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// 
/// Use:		 Handles the AI control section of the flight computer
/// </summary>




[Serializable]
public class SilantroBrain
{
    // ----------------------------------------- Selectibles
    public enum FlightState { Grounded, Taxi, Takeoff, Cruise, Loiter, Decent, Landing }
    public FlightState flightState = FlightState.Grounded;

    public enum TaxiState { Stationary, Moving, Holding }
    public TaxiState taxiState = TaxiState.Stationary;
    public enum LandingState { BaseA, BaseB, BaseC, BaseD }
    public LandingState landingState = LandingState.BaseA;
    public enum LoiterState { Hold, Cruise, Final}
    public LoiterState loiterState = LoiterState.Cruise;
    public enum FinalLoiterState { ApproachBreak, LevelBreak, Downwind, BaseLeg}
    public FinalLoiterState finalLoiter = FinalLoiterState.ApproachBreak;


    // ----------------------------------------- Connections
    public SilantroWaypointPlug tracker;
    public SilantroController controller;
    public AnimationCurve steerCurve;
    public SilantroFlightComputer computer;



    // ----------------------------------------- Variables
    public float currentSpeed;
    public float takeoffSpeed = 100f;
    public float climboutPitchAngle = 8f;
    public float checkListTime = 2f;
    public float transitionTime = 5f;
    private float evaluateTime = 12f;
    public float takeoffHeading;
    public float inputCheckFactor;
    private float currentTestTime;



    // -------------------------------- Control Markers
    public bool flightInitiated;
    public bool checkedSurfaces;
    public bool groundChecklistComplete;
    public bool isTaxing;
    public bool checkingSurfaces;
    public bool clearedForTakeoff;
    bool flapSet;
    public bool landingFlapSetA;
    public bool landingFlapSetB;
    public bool landingSpoilerEngaged;


    // -------------------------------- Taxi
    public float maximumTaxiSpeed = 10f;
    public float recommendedTaxiSpeed = 8f;
    public float maximumTurnSpeed = 5f;
    public float maximumSteeringAngle = 30f;
    public float minimumSteeringAngle = 15f;
    public float steeringAngle;
    public float targetTaxiSpeed;

    [Range(0, 1)] public float steerSensitivity = 0.05f;
    [Range(0, 2)] public float brakeSensitivity = 0.85f;
    public float brakeInput;


    // -------------------------------- Cruise
    public float cruiseAltitude = 1000f;
    public float cruiseSpeed = 300f;
    public float cruiseHeading = 0f;
    public float cruiseClimbRate = 1500f;


    // -------------------------------- Landing
    public float landingTargetDistance;
    public float landingGlideSlope = 3.5f;
    public float targetDecentHeight;
    public float decentSpeed = 250f;
    public float landingSpeedSemi = 180f;
    public float landingSpeedFinal = 130f;
    public float currentGlideSlope;
    public float innerGlideslopeDistance = 2000f;




    // ----------------------------------------------------------------------------------------------------------------------------------------------------------
    public void InitializeBrain()
    {
        // -------------------------
        if (tracker != null) { tracker.aircraft = controller; tracker.InitializePlug(); }

        // ------------------------ Plot
        PlotSteerCurve();
    }




    // ----------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UpdateBrain()
    {
        //---------------------------------------------OPERATION MODE
        switch (flightState)
        {
            case FlightState.Grounded: GroundMode(); break;
            case FlightState.Taxi: TaxiMode(); break;
            case FlightState.Takeoff: TakeoffMode(); break;
            case FlightState.Cruise: CruiseMode(); break;
            case FlightState.Decent: DecentMode(); break;
            case FlightState.Landing: LandingMode(); break;
            case FlightState.Loiter: LoiterMode(); break;
        }


        // ---------------------------------------------- COLLECT DATA
        currentSpeed = controller.core.currentSpeed;

        // ---------------------------------------------- SURFACE CHECK
        if (checkingSurfaces) { CheckControlSurfaces(inputCheckFactor); }
    }

   









    // ----------------------------------------------------------------------------------------------------------------------------------------------------------
    public void PlotSteerCurve()
    {
        // ------------------------- Plot Steer Curve
        steerCurve = new AnimationCurve();

        steerCurve.AddKey(new Keyframe(0.0f * maximumSteeringAngle, maximumTaxiSpeed));
        steerCurve.AddKey(new Keyframe(0.5f * maximumSteeringAngle, recommendedTaxiSpeed));
        steerCurve.AddKey(new Keyframe(0.8f * maximumSteeringAngle, maximumTurnSpeed));

#if UNITY_EDITOR
        for (int i = 0; i < steerCurve.keys.Length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(steerCurve, i, AnimationUtility.TangentMode.Auto);
            AnimationUtility.SetKeyRightTangentMode(steerCurve, i, AnimationUtility.TangentMode.Auto);
        }
#endif
    }




    // -------------------------------------------GROUND MODE----------------------------------------------------------------------------------------------------------
    void GroundMode()
    {
        //------------------------- Start Flight Process
        if (flightInitiated && !controller.engineRunning) { controller.TurnOnEngines(); flightInitiated = false; }
        if (flightInitiated && controller.engineRunning) { flightInitiated = false; }


        if (!controller.engineRunning)
        {
            // Check States
            if (controller.lightState == SilantroController.LightState.On) { controller.input.TurnOffLights(); }
            if (controller.gearHelper && controller.gearHelper.brakeState == SilantroGearSystem.BrakeState.Disengaged) { controller.gearHelper.EngageBrakes(); }
        }
        else
        {
            // -------------------------------------------------------------------------------------- Check List
            if (!groundChecklistComplete)
            {
                //Debug.Log("Engine Start Complete, commencing ground checklist");
                computer.StartCoroutine(GroundCheckList());
            }
        }
    }

    public float maximumDeviation;
    // ----------------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator GroundCheckList()
    {
        // --------------------------- Lights
        controller.input.TurnOnLights();


        // --------------------------- Actuators
        if (controller.canopyActuator && controller.canopyActuator.actuatorState == SilantroActuator.ActuatorState.Engaged) { controller.canopyActuator.DisengageActuator(); }
        if (controller.speedBrakeActuator && controller.speedBrakeActuator.actuatorState == SilantroActuator.ActuatorState.Engaged) { controller.speedBrakeActuator.DisengageActuator(); }
        if (controller.wingActuator && controller.wingActuator.actuatorState == SilantroActuator.ActuatorState.Engaged) { controller.wingActuator.DisengageActuator(); }


        // --------------------------- Flaps
        yield return new WaitForSeconds(checkListTime);
        foreach (SilantroAerofoil foil in controller.wings)
        {
            if (foil.flapSetting != 1 && !flapSet && foil.flapAngleSetting == SilantroAerofoil.FlapAngleSetting.ThreeStep) { foil.SetFlaps(1, 1); }
            if (foil.flapSetting != 2 && !flapSet && foil.flapAngleSetting == SilantroAerofoil.FlapAngleSetting.FiveStep) { foil.SetFlaps(2, 1); }
        }
        flapSet = true;


        // --------------------------- Slats
        yield return new WaitForSeconds(checkListTime);
        foreach (SilantroAerofoil foil in controller.flightComputer.wingFoils)
        {
            if (foil.slatState == SilantroAerofoil.ControlState.Active) { foil.baseSlat = Mathf.MoveTowards(foil.baseSlat, controller.flightComputer.takeOffSlat, foil.slatActuationSpeed * Time.fixedDeltaTime); }
        }


        // --------------------------- Control Surfaces
        yield return new WaitForSeconds(checkListTime);
        if (!checkingSurfaces && currentTestTime < 1f) { currentTestTime = evaluateTime; checkingSurfaces = true; }
        if (!checkedSurfaces)
        {
            float startRange = -1.0f; float endRange = 1.0f; float cycleRange = (endRange - startRange) / 2f;
            float offset = cycleRange + startRange;
            inputCheckFactor = offset + Mathf.Sin(Time.time * 5f) * cycleRange;
        }

        yield return new WaitForSeconds(evaluateTime);
        checkedSurfaces = true; checkingSurfaces = false;
        computer.processedPitch = 0f;
        computer.processedRoll = 0f;
        computer.processedYaw = 0f;
        computer.processedStabilizerTrim = 0f;

        groundChecklistComplete = true;


        // ---------------------------- Transition
        yield return new WaitForSeconds(transitionTime);
        flightState = FlightState.Taxi;
        if (controller.gearHelper != null) { controller.gearHelper.ReleaseBrakes(); }
    }


    // ----------------------------------------------------------------------------------------------------------------------------------------------------------
    void CheckControlSurfaces(float controlInput)
    {
        if (checkingSurfaces)
        {
            currentTestTime -= Time.deltaTime;
            if (currentTestTime < 0) { currentTestTime = 0f; }

            //--------------------- Pitch
            if (currentTestTime < evaluateTime && currentTestTime > (0.75f * evaluateTime))
            {
                computer.processedPitch = controlInput;
                computer.processedRoll = 0f;
                computer.processedYaw = 0f;
                computer.processedStabilizerTrim = 0f;
            }
            //--------------------- Roll
            if (currentTestTime < (0.75f * evaluateTime) && currentTestTime > (0.50f * evaluateTime))
            {
                computer.processedPitch = 0f;
                computer.processedRoll = controlInput;
                computer.processedYaw = 0f;
                computer.processedStabilizerTrim = 0f;
            }
            //--------------------- Yaw
            if (currentTestTime < (0.50f * evaluateTime) && currentTestTime > (0.25f * evaluateTime))
            {
                computer.processedPitch = 0f;
                computer.processedRoll = 0f;
                computer.processedYaw = controlInput;
                computer.processedStabilizerTrim = 0f;
            }
            //--------------------- Trim
            if (currentTestTime < (0.25f * evaluateTime) && currentTestTime > (0.00f * evaluateTime))
            {
                computer.processedPitch = 0f;
                computer.processedRoll = 0f;
                computer.processedYaw = 0f;
                computer.processedStabilizerTrim = controlInput;
            }
        }
    }



    // -------------------------------------------TAXI----------------------------------------------------------------------------------------------------------
    void TaxiMode()
    {
        // ------------------------------------- Clamp
        float thresholdSpeed = maximumTaxiSpeed * 0.1f;


        // ------------------------------------- States
        if (taxiState == TaxiState.Stationary)
        {
            if (controller.gearHelper != null && controller.gearHelper.brakeState == SilantroGearSystem.BrakeState.Disengaged && tracker.currentPoint <= tracker.taxiTrack.pathPoints.Count)
            { taxiState = TaxiState.Moving; isTaxing = true; }
        }
        if (taxiState == TaxiState.Moving)
        {
            if (tracker.currentPoint > tracker.taxiTrack.pathPoints.Count - 2 && controller.gearHelper != null && controller.gearHelper.brakeState == SilantroGearSystem.BrakeState.Disengaged)
            {
                taxiState = TaxiState.Holding;
                targetTaxiSpeed = 0; brakeInput = 0f;
                if (controller.gearHelper != null) { controller.gearHelper.EngageBrakes(); }
                isTaxing = false;

                //--------------- Set Takeoff Parameters
                takeoffHeading = computer.currentHeading;
            }
            else
            {
                if (tracker.currentPoint > tracker.taxiTrack.pathPoints.Count - 6 && tracker.currentPoint < tracker.taxiTrack.pathPoints.Count - 4) { targetTaxiSpeed = (0.6f * maximumTaxiSpeed) / 1.94384f; } //96
                else if (tracker.currentPoint > tracker.taxiTrack.pathPoints.Count - 5 && tracker.currentPoint < tracker.taxiTrack.pathPoints.Count - 3) { targetTaxiSpeed = (0.5f * maximumTaxiSpeed) / 1.94384f; } //97
                else if (tracker.currentPoint > tracker.taxiTrack.pathPoints.Count - 4 && tracker.currentPoint < tracker.taxiTrack.pathPoints.Count - 2) { targetTaxiSpeed = (0.25f * maximumTaxiSpeed) / 1.94384f; } //98
                else if (tracker.currentPoint > tracker.taxiTrack.pathPoints.Count - 3 && tracker.currentPoint < tracker.taxiTrack.pathPoints.Count - 1) { targetTaxiSpeed = (0.1f * maximumTaxiSpeed) / 1.94384f; } //98
                else { targetTaxiSpeed = steerCurve.Evaluate(steeringAngle) / 1.94384f; }

                isTaxing = true;
            }
        }
        if (taxiState == TaxiState.Holding)
        {
            // -------------------------- Perform function while on hold

            // -------------------------- Receive clearance
            if (clearedForTakeoff) { flightState = FlightState.Takeoff; }
        }






        if (isTaxing)
        {
            // ------------------------------------- Speed Control
            float speedError = (targetTaxiSpeed - currentSpeed) * 1.94384f;
            if (computer.autoThrottle == SilantroFlightComputer.ControlState.Off) { computer.autoThrottle = SilantroFlightComputer.ControlState.Active; }
            if (speedError > 5 && tracker.currentPoint < tracker.taxiTrack.pathPoints.Count - 2)
            {
                // ---------------- Auto Throttle
                float presetSpeed = targetTaxiSpeed;
                computer.speedError = (presetSpeed - currentSpeed);
                computer.processedThrottle = computer.throttleSolver.CalculateOutput(computer.speedError, computer.timeStep);
                if (float.IsNaN(computer.processedThrottle) || float.IsInfinity(computer.processedThrottle)) { computer.processedThrottle = 0f; }
            }
            else { computer.processedThrottle = 0f; }



            // ------------------------------------- Point
            tracker.UpdateTrack(tracker.taxiTrack);


            // ------------------------------------- Steer
            float taxiSpeed = controller.transform.InverseTransformDirection(controller.aircraft.velocity).z;
            brakeInput = -1 * Mathf.Clamp((targetTaxiSpeed - currentSpeed) * brakeSensitivity, -1, 0);
            Vector3 offsetTargetPos = tracker.target.position;
            Vector3 localTarget = controller.transform.InverseTransformPoint(offsetTargetPos);
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            float steer = Mathf.Clamp(targetAngle * steerSensitivity, -1, 1) * Mathf.Sign(taxiSpeed);
            steeringAngle = Mathf.Lerp(maximumSteeringAngle, minimumSteeringAngle, Mathf.Abs(taxiSpeed) * 0.015f) * steer;


            // ------------------------------------- Send
            if (controller.gearHelper != null)
            {
                controller.gearHelper.brakeInput = brakeInput;
                controller.gearHelper.currentSteerAngle = steeringAngle;
            }
        }
    }


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------
    public void TakeoffClearance()
    {
        if (flightState == FlightState.Taxi && taxiState == TaxiState.Holding)
        {
            if (!clearedForTakeoff) { clearedForTakeoff = true; }
            else { Debug.Log(controller.transform.name + " has been cleared for takeoff"); }
        }
        else { Debug.Log(controller.transform.name + " clearance invalid! Aircraft not in holding pattern"); }
    }



    // -------------------------------------------TAKEOFF----------------------------------------------------------------------------------------------------------
    void TakeoffMode()
    {
        if (controller.engineRunning)
        {
            //------Accelerate
            if (controller.gearHelper) { controller.gearHelper.ReleaseBrakes(); }
            computer.processedThrottle = Mathf.Lerp(computer.processedThrottle, 1, Time.deltaTime);
            if (computer.processedThrottle > 0.9f && controller.input.boostState == SilantroInput.BoostState.Off) { controller.input.EngageBoost(); }


            // ------------------------------------- Send
            if (controller.gearHelper != null)
            {
                controller.gearHelper.brakeInput = brakeInput = 0f;
                controller.gearHelper.currentSteerAngle = steeringAngle = 0f;
            }


            #region Pitch Control
            if (computer.knotSpeed < takeoffSpeed)
            {
                computer.commandPitchRate = 0f;
                computer.pitchRateError = computer.pitchRate - computer.commandPitchRate;
                computer.processedPitch = computer.pitchRateSolver.CalculateOutput(computer.pitchRateError, computer.timeStep);
            }
            else
            {
                // -------------------------------------------- Pitch Rate Required
                computer.pitchAngleError = computer.pitchAngle - (-1f * climboutPitchAngle);
                computer.pitchAngleSolver.minimum = -computer.climboutPitchRate; computer.pitchAngleSolver.maximum = computer.climboutPitchRate;
                computer.commandPitchRate = computer.pitchAngleSolver.CalculateOutput(computer.pitchAngleError, computer.timeStep);

                computer.pitchRateError = computer.pitchRate - computer.commandPitchRate;
                computer.processedPitch = computer.pitchRateSolver.CalculateOutput(computer.pitchRateError, computer.timeStep);
            }
            #endregion Pitch Control

            #region Roll/Yaw Control
            computer.yawAngleError = takeoffHeading - computer.yawAngle;
            computer.yawAngleSolver.minimum = -computer.balanceYawRate; computer.yawAngleSolver.maximum = computer.balanceYawRate;
            computer.commandYawRate = computer.yawAngleSolver.CalculateOutput(computer.yawAngleError, computer.timeStep);
            computer.yawRateError = computer.commandYawRate - computer.yawRate;
            computer.processedYaw = computer.yawRateSolver.CalculateOutput(computer.yawRateError, computer.timeStep);

            computer.commandBankAngle = 0f;
            computer.rollAngleError = computer.rollAngle - (-1f * computer.commandBankAngle);
            computer.rollAngleSolver.minimum = -computer.balanceRollRate; computer.rollAngleSolver.maximum = computer.balanceRollRate;
            computer.commandRollRate = computer.rollAngleSolver.CalculateOutput(computer.rollAngleError, computer.timeStep);
            computer.rollRateError = computer.commandRollRate - computer.rollRate;
            computer.processedRoll = computer.rollRateSolver.CalculateOutput(computer.rollRateError, computer.timeStep);
            #endregion Roll/Yaw Control

            if (computer.knotSpeed > computer.maximumGearSpeed - 10f) { computer.StartCoroutine(PostTakeoffCheckList()); }
        }
    }



    // ----------------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator PostTakeoffCheckList()
    {
        // --------------------------- Flaps
        yield return new WaitForSeconds(checkListTime);
        foreach (SilantroAerofoil foil in controller.wings) { if (foil.flapSetting != 0) { foil.SetFlaps(0, 1); } }

        // --------------------------- Gear
        if (computer.gearSolver)
        {
            yield return new WaitUntil(() => computer.knotSpeed > computer.maximumGearSpeed && computer.ftHeight > 50f);
            computer.gearSolver.DisengageActuator();
        }

        // ---------------------------- Transition
        yield return new WaitForSeconds(transitionTime);
        foreach (SilantroAerofoil foil in computer.wingFoils) { if (foil.slatState == SilantroAerofoil.ControlState.Active) { foil.baseSlat = Mathf.MoveTowards(foil.baseSlat, 0f, foil.slatActuationSpeed * Time.fixedDeltaTime); } }
        flightState = FlightState.Cruise;
    }



    // -------------------------------------------CRUISE----------------------------------------------------------------------------------------------------------
    void CruiseMode()
    {
        if (Mathf.Abs(cruiseSpeed - computer.knotSpeed) > 2) { computer.autoThrottle = SilantroFlightComputer.ControlState.Active; }
        else { computer.autoThrottle = SilantroFlightComputer.ControlState.Off; }


        // ---------------- Auto Throttle
        SetTargetSpeed(cruiseSpeed);

        // ---------------- Heading Hold
        SetTargetHeading(cruiseHeading, false);

        // ---------------- Altitude Hold
        SetTargetHeight(cruiseAltitude, cruiseClimbRate);
    }










    public LandingPoints path;
    public int currentWayPointIndex = 1;
    public bool isCircular;

    public float maxApproachAngle = 30f;
    public float pathGain = 1f;

    public float pathHeading;
    public float verticalPathDeviation;
    public float horizontalPathDeviation;

    public float desiredHeading;
    public SilantroPID pathSolver;

    Vector3 nextPoint, lastPoint, pointA, pointB, pointC;

    public float weightA = 0.5f;
    public float weightB = 0.5f;
    public float weightC = 0.5f;

    public float desiredHeadingA;
    public float desiredHeadingB;
    public float desiredHeadingC;

    public float horizontalPathDeviationA;
    public float horizontalPathDeviationB;
    public float horizontalPathDeviationC;


    // -------------------------------------------LOITER----------------------------------------------------------------------------------------------------------
    private void LoiterMode()
    {
        if (path != null && path.pathPoints.Count > 5)
        {
            nextPoint = MathBase.GetWaypointPosition(path.pathPoints, currentWayPointIndex);

            pointA = MathBase.GetWaypointPosition(path.pathPoints, currentWayPointIndex + 1);
            pointB = MathBase.GetWaypointPosition(path.pathPoints, currentWayPointIndex + 2);
            pointC = MathBase.GetWaypointPosition(path.pathPoints, currentWayPointIndex + 3);

            lastPoint = MathBase.GetWaypointPosition(path.pathPoints, currentWayPointIndex - 1);

            Vector3 waypointDirection = (nextPoint - lastPoint).normalized;


            // ------------------------------- Go through waypoints
            if (MathBase.CalculateProgress(controller.transform.position, lastPoint, nextPoint) > 1f)
            {
                currentWayPointIndex += 1;
                if (currentWayPointIndex > path.pathPoints.Count - 1 && isCircular) { currentWayPointIndex = 0; }
                else if (currentWayPointIndex > path.pathPoints.Count - 1)
                {
                    //reached the end of the path
                }
            }

            if (loiterState == LoiterState.Final)
            {
                //---------------------------------------------OPERATION MODE
                switch (finalLoiter)
                {
                    case FinalLoiterState.ApproachBreak: ApproachBreak(); break;
                    case FinalLoiterState.LevelBreak: LevelBreak(); break;
                    case FinalLoiterState.Downwind: Downwind(); break;
                    case FinalLoiterState.BaseLeg: BaseLeg(); break;
                }
            }


            Debug.DrawLine(controller.transform.position, nextPoint, Color.red);
            Debug.DrawLine(controller.transform.position + controller.transform.forward * 50, pointA, Color.yellow);
            Debug.DrawLine(controller.transform.position + controller.transform.forward * 100, pointB, Color.green);
            Debug.DrawLine(controller.transform.position + controller.transform.forward * 150, pointC, Color.cyan);


            pathHeading = computer.currentHeading + Vector3.Angle(controller.transform.forward, waypointDirection);
            horizontalPathDeviation = MathBase.CalculateRouteDeviation(controller.transform, nextPoint, lastPoint, 1, 0);
            if (Mathf.Abs(horizontalPathDeviation) > maximumDeviation) { maximumDeviation = Mathf.Abs(horizontalPathDeviation); }
            horizontalPathDeviationA = MathBase.CalculateRouteDeviation(controller.transform, pointA, nextPoint, 1, 50);
            horizontalPathDeviationB = MathBase.CalculateRouteDeviation(controller.transform, pointB, pointA, 1, 100);
            horizontalPathDeviationC = MathBase.CalculateRouteDeviation(controller.transform, pointC, pointB, 1, 150);

            verticalPathDeviation = MathBase.CalculateRouteDeviation(controller.transform, nextPoint, lastPoint, 2, 0);

            //desiredHeading = (pathHeading * Mathf.Deg2Rad) - (maxApproachAngle * Mathf.Deg2Rad) * 0.63661f * Mathf.Atan(pathGain * pathDeviation);
            //desiredHeading *= Mathf.Rad2Deg;

            horizontalPathDeviation += (horizontalPathDeviationA * weightA) + (horizontalPathDeviationB * weightB) + (horizontalPathDeviationC * weightC);

           

            desiredHeading = pathSolver.CalculateOutput(horizontalPathDeviation, computer.timeStep);


            // ---------------------------------------------- Calculate Required Bank
            computer.commandTurnRate = desiredHeading;
            float rollFactor = (computer.commandTurnRate * computer.currentSpeed * MathBase.toKnots) / 1091f;
            computer.commandBankAngle = Mathf.Atan(rollFactor) * Mathf.Rad2Deg;
            if (computer.commandBankAngle > computer.maximumTurnBank) { computer.commandBankAngle = computer.maximumTurnBank; }
            if (computer.commandBankAngle < -computer.maximumTurnBank) { computer.commandBankAngle = -computer.maximumTurnBank; }


            computer.rollAngleError = computer.rollAngle - (-1f * computer.commandBankAngle);
            computer.rollAngleSolver.minimum = -computer.balanceRollRate; computer.rollAngleSolver.maximum = computer.balanceRollRate;
            computer.commandRollRate = computer.rollAngleSolver.CalculateOutput(computer.rollAngleError, computer.timeStep);

            computer.rollRateError = computer.commandRollRate - computer.rollRate;
            computer.processedRoll = computer.rollRateSolver.CalculateOutput(computer.rollRateError, computer.timeStep);





            computer.altitudeError = verticalPathDeviation;
            float presetClimbLimit = 3000 / MathBase.toFtMin;
            if (Mathf.Abs(desiredHeading) > 5f)
            {
                computer.turnAltitudeSolver.maximum = presetClimbLimit; computer.turnAltitudeSolver.minimum = -presetClimbLimit;
                computer.commandClimbRate = computer.turnAltitudeSolver.CalculateOutput(computer.altitudeError, computer.timeStep) * MathBase.toFtMin;
            }
            else
            {
                computer.altitudeSolver.maximum = presetClimbLimit; computer.altitudeSolver.minimum = -presetClimbLimit;
                computer.commandClimbRate = computer.altitudeSolver.CalculateOutput(computer.altitudeError, computer.timeStep) * MathBase.toFtMin;
            }


            // -------------------------------------------- Pitch Rate Required
            float presetClimbRate = computer.commandClimbRate / MathBase.toFtMin;
            computer.climbRateError = presetClimbRate - computer.currentClimbRate;

            if (Mathf.Abs(desiredHeading) > 5f)
            {
                computer.turnClimbSolver.minimum = -computer.balancePitchRate * 0.2f; computer.turnClimbSolver.maximum = computer.balancePitchRate * 0.5f;
                computer.commandPitchRate = computer.turnClimbSolver.CalculateOutput(computer.climbRateError, computer.timeStep);
            }
            else
            {
                computer.climbSolver.minimum = -computer.balancePitchRate * 0.2f; computer.climbSolver.maximum = computer.balancePitchRate * 0.5f;
                computer.commandPitchRate = computer.climbSolver.CalculateOutput(computer.climbRateError, computer.timeStep);

            }
            computer.pitchRateError = computer.pitchRate - computer.commandPitchRate;
            computer.processedPitch = computer.pitchRateSolver.CalculateOutput(computer.pitchRateError, computer.timeStep);
        }
    }




    // -------------------------------------------Final Loiter----------------------------------------------------------------------------------------------------------
    private void BaseLeg()
    {
        if (currentWayPointIndex >= path.FinalPoint) { flightState = FlightState.Decent; }
    }

    private void Downwind()
    {
        SetTargetSpeed(160f);
        //SetTargetHeight(183f, 1200);

        if (currentWayPointIndex >= path.BaseLegPoint) { finalLoiter = FinalLoiterState.BaseLeg; }
    }

    private void LevelBreak()
    {
        SetTargetSpeed(180f);
        //S/etTargetHeight(305f, 1200);

        if (currentWayPointIndex >= path.DownwindPoint) { finalLoiter = FinalLoiterState.Downwind; }
    }

    private void ApproachBreak()
    {
        SetTargetSpeed(200f);
        //SetTargetHeight(457f, 1200);

        if (currentWayPointIndex >= path.LevelBreakPoint) { finalLoiter = FinalLoiterState.LevelBreak; }
    }





















    // -------------------------------------------DECENT----------------------------------------------------------------------------------------------------------
    private void DecentMode()
    {
        if (tracker.landingTrack != null)
        {
            if (computer.autoThrottle == SilantroFlightComputer.ControlState.Off) { computer.autoThrottle = SilantroFlightComputer.ControlState.Active; }


            // ------------------------------------- Point
            tracker.UpdateTrack(tracker.landingTrack);


            //-------------------------------------- Determine Decent 
            Vector3 difference = controller.transform.position - tracker.target.position;
            landingTargetDistance = Mathf.Abs(difference.z);
            targetDecentHeight = landingTargetDistance * Mathf.Tan(landingGlideSlope * Mathf.Deg2Rad);
            currentGlideSlope = Mathf.Atan((computer.currentAltitude / landingTargetDistance)) * Mathf.Rad2Deg;

            // ------------------------------------- Set Values
            SetTargetSpeed(decentSpeed);

            SetTargetHeading(tracker.landingTrack.baseHeading, false);

            SetTargetHeight(targetDecentHeight, 2500);

            // ----------------------------------------------- Switch To Landing Mode
            if (computer.ftHeight < 1500)
            {
                flightState = FlightState.Landing;
                landingState = LandingState.BaseA;
            }
        }
        else { flightState = FlightState.Cruise; }
    }




    // -------------------------------------------Landing----------------------------------------------------------------------------------------------------------
    private void LandingMode()
    {
        if (tracker.landingTrack != null)
        {
            // ------------------------------------- Point
            tracker.UpdateTrack(tracker.landingTrack);


            //-------------------------------------- Determine Decent 
            Vector3 difference = controller.transform.position - tracker.target.position;
            landingTargetDistance = Mathf.Abs(difference.z);
            targetDecentHeight = landingTargetDistance * Mathf.Tan(landingGlideSlope * Mathf.Deg2Rad);
            if (landingTargetDistance < innerGlideslopeDistance) { targetDecentHeight = 0f; }
            currentGlideSlope = Mathf.Atan((computer.currentAltitude / landingTargetDistance)) * Mathf.Rad2Deg;

            //---------------------------------------------OPERATION MODE
            switch (landingState)
            {
                case LandingState.BaseA: LandingStateA(); break;
                case LandingState.BaseB: LandingStateB(); break;
                case LandingState.BaseC: LandingStateC(); break;
                case LandingState.BaseD: LandingStateD(); break;
            }
        }
    }




    // -------------------------------------------Landing Mode A----------------------------------------------------------------------------------------------------------
    void LandingStateA()
    {
        // ----------------- Set Speed
        SetTargetSpeed(landingSpeedSemi);

        // ----------------- Set Altitude
        SetTargetHeight(targetDecentHeight, 1500);

        // ----------------- Set Heading
        SetTargetHeading(tracker.landingTrack.baseHeading, false);


        // --------------------------- Flaps
        if (!landingFlapSetA)
        {
            foreach (SilantroAerofoil foil in controller.wings)
            {
                if (foil.flapSetting != 1 && !flapSet && foil.flapAngleSetting == SilantroAerofoil.FlapAngleSetting.ThreeStep) { foil.SetFlaps(1, 1); }
                if (foil.flapSetting != 2 && !flapSet && foil.flapAngleSetting == SilantroAerofoil.FlapAngleSetting.FiveStep) { foil.SetFlaps(2, 1); }
            }
            landingFlapSetA = true;
        }

        // --------------------------- Gear
        if (computer.gearSolver.actuatorState == SilantroActuator.ActuatorState.Disengaged) { computer.gearSolver.EngageActuator(); }


        // ----------------------------------------------- Switch Landing State
        if (computer.ftHeight < 1000) { landingState = LandingState.BaseB; }
    }

   
    // -------------------------------------------Landing Mode B----------------------------------------------------------------------------------------------------------
    void LandingStateB()
    {
        // ----------------- Set Speed
        SetTargetSpeed(landingSpeedFinal);

        // ----------------- Set Altitude
        SetTargetHeight(targetDecentHeight, 1000);

        // ----------------- Set Heading
        SetTargetHeading(tracker.landingTrack.baseHeading, false);

        // --------------------------- Flaps
        if (!landingFlapSetB)
        {
            foreach (SilantroAerofoil foil in controller.wings)
            {
                if (foil.flapSetting != 2 && !flapSet && foil.flapAngleSetting == SilantroAerofoil.FlapAngleSetting.ThreeStep) { foil.SetFlaps(2, 1); }
                if (foil.flapSetting != 3 && !flapSet && foil.flapAngleSetting == SilantroAerofoil.FlapAngleSetting.FiveStep) { foil.SetFlaps(3, 1); }
            }
            landingFlapSetB = true;
        }

        // ----------------------------------------------- Switch Landing State
        if (computer.ftHeight < 500) { landingState = LandingState.BaseC; }
    }


    // -------------------------------------------Landing Mode C----------------------------------------------------------------------------------------------------------
    void LandingStateC()
    {
        // ----------------- Set Speed
        SetTargetSpeed(landingSpeedFinal);


        // ----------------- Set Altitude
        SetTargetHeight(targetDecentHeight, 1000);

        // ----------------- Set Heading
        SetTargetHeading(tracker.landingTrack.baseHeading, true);

        // ----------------------------------------------- Switch Landing State
        if (computer.ftHeight < 205) { landingState = LandingState.BaseD; }
    }


    // -------------------------------------------Landing Mode C----------------------------------------------------------------------------------------------------------
    void LandingStateD()
    {
       
        // ----------------- Set Heading
        SetTargetHeading(tracker.landingTrack.baseHeading, true);


        if (controller.isGrounded)
        {
            if(computer.autoThrottle == SilantroFlightComputer.ControlState.Active) { computer.autoThrottle = SilantroFlightComputer.ControlState.Off; }
            computer.processedThrottle = 0f;

            // ------------------------------------- Brake Lever
            if (controller.gearHelper != null)
            {
                //controller.gearHelper.brakeInput = 1;
            }

            // ------------------------------------ Speed Brake
            if (computer.speedBrakeSolver != null && computer.speedBrakeSolver.actuatorState == SilantroActuator.ActuatorState.Disengaged) { computer.speedBrakeSolver.EngageActuator(); }

            // ------------------------------------ Spoilers
            if (!landingSpoilerEngaged)
            {
                foreach (SilantroAerofoil foil in controller.wings)
                {
                    if (!foil.spoilerExtended)
                    {
                        foil.baseSpoiler = foil.maximumSpoilerDeflection;
                        computer.StartCoroutine(foil.ExtendSpoiler());
                    }
                }
                landingSpoilerEngaged = true;
            }
        }
        else
        {
            // ----------------- Set Speed
            if (computer.ftHeight < 200 && computer.ftHeight >= 100) { SetTargetHeight(0, 800); SetTargetSpeed(landingSpeedFinal - 5); }
            if (computer.ftHeight < 100 && computer.ftHeight >= 50) { SetTargetHeight(0, 500); SetTargetSpeed(landingSpeedFinal - 8); }
            if (computer.ftHeight < 50 && computer.ftHeight >= 00) { SetTargetHeight(0, 300); SetTargetSpeed(landingSpeedFinal - 10); }
            if (computer.ftHeight < 20) { SetTargetSpeed(0); }

        }
    }










    // -------------------------------------------SPEED CONTROL----------------------------------------------------------------------------------------------------------
    private void SetTargetSpeed(float targetKnotSpeed)
    {
        // ---------------- Auto Throttle
        float presetSpeed = targetKnotSpeed / MathBase.toKnots;
        computer.speedError = (presetSpeed - currentSpeed);
        computer.throttleSolver.minimum = 0.001f;
        computer.processedThrottle = computer.throttleSolver.CalculateOutput(computer.speedError, computer.timeStep);
        if (float.IsNaN(computer.processedThrottle) || float.IsInfinity(computer.processedThrottle)) { computer.processedThrottle = 0f; }


        // ----------------- Boost e.g Piston Turbo or Turbine Reheat
        if (computer.processedThrottle > 1f && computer.speedError > 5f && controller.input.boostState == SilantroInput.BoostState.Off) { controller.input.EngageBoost(); }
        if (computer.processedThrottle < 1f && computer.speedError < 3f && controller.input.boostState == SilantroInput.BoostState.Active) { controller.input.DisEngageBoost(); }

        // ----------------- Boost Deceleration with Speedbrake
        if (computer.speedBrakeSolver != null)
        {
            if (computer.speedBrakeSolver.actuatorState == SilantroActuator.ActuatorState.Disengaged && computer.speedError < -30f) { computer.speedBrakeSolver.EngageActuator(); }
            if (computer.speedBrakeSolver.actuatorState == SilantroActuator.ActuatorState.Engaged && computer.speedError > -20f) { computer.speedBrakeSolver.DisengageActuator(); }
        }
    }





    // -------------------------------------------ALTITUDE CONTROL----------------------------------------------------------------------------------------------------------
    private void SetTargetHeight(float targetHeight, float rateLimit)
    {
        // ------------------------------------------------ Altitude Hold
        computer.altitudeError = targetHeight - computer.currentAltitude;
        float presetClimbLimit = rateLimit / MathBase.toFtMin;
        computer.altitudeSolver.maximum = presetClimbLimit; computer.altitudeSolver.minimum = -presetClimbLimit;
        computer.commandClimbRate = computer.altitudeSolver.CalculateOutput(computer.altitudeError, computer.timeStep) * MathBase.toFtMin;

        // -------------------------------------------- Pitch Rate Required
        float presetClimbRate = computer.commandClimbRate / MathBase.toFtMin;
        computer.climbRateError = presetClimbRate - computer.currentClimbRate;
        computer.climbSolver.minimum = -computer.balancePitchRate * 0.2f; computer.climbSolver.maximum = computer.balancePitchRate * 0.5f;
        computer.commandPitchRate = computer.climbSolver.CalculateOutput(computer.climbRateError, computer.timeStep);

        computer.pitchRateError = computer.pitchRate - computer.commandPitchRate;
        computer.processedPitch = computer.pitchRateSolver.CalculateOutput(computer.pitchRateError, computer.timeStep);
    }



    // -------------------------------------------HEADING CONTROL----------------------------------------------------------------------------------------------------------
    private void SetTargetHeading(float targetHeading, bool rudderEngaged)
    {  
        float presetHeading = targetHeading; if (presetHeading > 180) { presetHeading -= 360f; }
        computer.headingError = presetHeading - computer.currentHeading;
        computer.turnSolver.maximum = computer.maximumTurnRate; computer.turnSolver.minimum = -computer.maximumTurnRate;
        computer.commandTurnRate = computer.turnSolver.CalculateOutput(computer.headingError, computer.timeStep);

       

        // ---------------------------------------------- Calculate Required Bank
        float rollFactor = (computer.commandTurnRate * currentSpeed * MathBase.toKnots) / 1091f;
        computer.commandBankAngle = Mathf.Atan(rollFactor) * Mathf.Rad2Deg;
        if (computer.commandBankAngle > computer.maximumTurnBank) { computer.commandBankAngle = computer.maximumTurnBank; }
        if (computer.commandBankAngle < -computer.maximumTurnBank) { computer.commandBankAngle = -computer.maximumTurnBank; }

        // -------------------------------------------- Roll Rate Required
        computer.rollAngleError = computer.rollAngle - (-1f * computer.commandBankAngle);
        computer.rollAngleSolver.minimum = -computer.balanceRollRate; computer.rollAngleSolver.maximum = computer.balanceRollRate;
        computer.commandRollRate = computer.rollAngleSolver.CalculateOutput(computer.rollAngleError, computer.timeStep);

        computer.rollRateError = computer.commandRollRate - computer.rollRate;
        computer.processedRoll = computer.rollRateSolver.CalculateOutput(computer.rollRateError, computer.timeStep);

        if (rudderEngaged)
        {
            computer.yawAngleError = takeoffHeading - computer.yawAngle;
            computer.yawAngleSolver.minimum = -computer.balanceYawRate; computer.yawAngleSolver.maximum = computer.balanceYawRate;
            computer.commandYawRate = computer.yawAngleSolver.CalculateOutput(computer.yawAngleError, computer.timeStep);
            computer.yawRateError = computer.commandYawRate - computer.yawRate;
            computer.processedYaw = computer.yawRateSolver.CalculateOutput(computer.yawRateError, computer.timeStep);
        }
        else
        {
            computer.yawRateError = 0;
            computer.processedYaw = computer.yawRateSolver.CalculateOutput(computer.yawRateError, computer.timeStep);
        }
    }
}
