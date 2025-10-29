using Assets.Code.Plane;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroStarter : MonoBehaviour
{
	[Header("Connected Aircraft")]
	public SilantroController aircraft;
	public Actuation actuator;
	public GameObject AfterBurner;
	public bool OnStartup = true;
	private Rigidbody rb;
	void Start()
	{
		if (OnStartup) 
		{
			startByScript();

        }
	}

	public void startByScript()
	{
        if (aircraft.startMode == SilantroController.StartMode.Hot)
        {
            StartCoroutine(StartUpAircraft());
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            StartCoroutine(Unfreeze());
        }
    }
        
		
		


	IEnumerator StartUpAircraft()
	{

		yield return new WaitForSeconds(0.002f);//JUST LAG A BIT BEHIND CONTROLLER SCRIPT
												//STARTUP AIRCRAFT	
		aircraft.StartAircraft();

		//RAISE GEAR
		if (aircraft.flightComputer.gearSolver != null && aircraft.flightComputer.gearSolver.actuatorState == SilantroActuator.ActuatorState.Engaged) { aircraft.flightComputer.gearSolver.DisengageActuator(); }
		if (AfterBurner != null)
		{
			AfterBurner.SetActive(true);
		}

		if(actuator != null)
		{
            actuator.CheckAnimation();

        }

	
	}


	IEnumerator Unfreeze()
	{
		yield return new WaitForSeconds(2f);
		rb.constraints = RigidbodyConstraints.None;
	}
}
