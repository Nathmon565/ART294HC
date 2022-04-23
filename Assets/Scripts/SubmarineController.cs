using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour {
	[Header("Controls")]
	public ControlBoardButton buttonBallastFront;
	[Range(0, 1)]
	public float ballastLevelFrontTarget;
	[Range(0, 1)]
	public float ballastLevelFrontActual;
	public float ballastLevelFrontVelocity = 0;
	public ControlBoardButton buttonBallastBack;
	[Range(0, 1)]
	public float ballastLevelBackTarget;
	[Range(0, 1)]
	public float ballastLevelBackActual;
	public float ballastLevelBackVelocity = 0;
	public ControlBoardButton buttonBallastLink;
	public bool ballastLink;
	public ControlBoardButton buttonBallastPower;
	public ControlBoardButton buttonBallastPowerIndicator;
	public bool ballastPower;
	[Space(5)]
	public ControlBoardButton buttonEngineThrust;
	[Range(0, 1)]
	public float engineThrustTarget;
	[Range(-1, 1)]
	public float engineThrustActual;
	public float engineVelocity = 0;
	public ControlBoardButton buttonEnginePowerIndicator;
	public ControlBoardButton buttonEnginePower;
	public bool enginePower;
	public ControlBoardButton buttonEngineReverse;
	public bool engineReverse;
	[Space(5)]
	public ControlBoardButton buttonRudderPowerIndicator;
	public ControlBoardButton buttonRudderPower;
	public bool rudderPower;
	public ControlBoardButton buttonRudder;
	[Range(0, 1)]
	public float rudderDirectionTarget;
	[Range(0, 1)]
	public float rudderDirectionActual;
	public float rudderVelocity = 0;
	[Space(5)]
	public ControlBoardButton buttonSonarType;
	public bool sonarDirectionType;
	public ControlBoardButton buttonSonarDirectionDial;
	[Range(0, 1)]
	public float sonarDirection;
	public ControlBoardButton buttonSonarPowerIndicator;
	public ControlBoardButton buttonSonarPower;
	public bool sonarPower;
	
	[Space(5)]
	public ControlBoardButton buttonExternalLights;
	public bool externalLights;
	public ControlBoardButton buttonInternalLights;
	public bool internalLights;
	public ControlBoardButton buttonCo2Scrubber;
	public bool co2Scrubber;
	public ControlBoardButton buttonO2Compressor;
	public bool o2Compressor;
	public ControlBoardButton buttonAirflowVentilation;
	public bool airflowVentilation;
	[Space(5)]
	public ControlBoardButton buttonIsReactorOn;
	public bool isReactorOn;
	public ControlBoardButton buttonIsReactorWarn;
	public bool isReactorWarn;
	[Header("Stats")]
	[Range(0, 1)]
	public float airQuality = 1;
	public Vector3 velocity = Vector3.zero;
	public Vector3 angularVelocity = Vector3.zero;
	[Header("Settings")]
	public float dampSmoothTime = 1f;
	public float dampMaxSpeedBallast = 0.1f;
	public float dampMaxSpeedEngine = 0.25f;
	public float dampMaxSpeedRudder = 0.35f;
	[Header("Objects")]
	public OxygenTank o2Tank;
	public List<LightController> internalLightList;
	public List<LightController> externalLightList;
	public SubPropellor propellor;
	public GameObject rudder;

	public float GetPumpWork() {
		return Mathf.Abs(ballastLevelBackActual - ballastLevelBackTarget) + Mathf.Abs(ballastLevelFrontActual - ballastLevelFrontTarget);
	}

	public float GetRudderWork() {
		return Mathf.Abs(rudderDirectionActual - rudderDirectionTarget);
	}

	public void ToggleLights(bool isInternal) {
		SetLights(isInternal, isInternal ? internalLights : externalLights);
	}

	public void SetLights(bool isInternal, bool isActive) {
		foreach(LightController l in isInternal ? internalLightList : externalLightList) {
			l.SetActive(isActive);
		}
		if(isInternal) { internalLights = isActive; }
		else { externalLights = isActive; }
	}

	public void SetButton(ControlBoardButton button, float value) {
		if(button.associatedValue == value) { return; }
		button.associatedValue = value;
		button.UpdateVisual();
	}

	public void UpdateVisuals() {
		SetButton(buttonAirflowVentilation, airflowVentilation ? 1 : 0);
		SetButton(buttonBallastBack, ballastLevelBackTarget);
		SetButton(buttonBallastFront, ballastLevelFrontTarget);
		SetButton(buttonBallastLink, ballastLink ? 1 : 0);
		SetButton(buttonBallastPower, ballastPower ? 1 : 0);
		SetButton(buttonBallastPowerIndicator, ballastPower && isReactorOn ? 1 : 0);
		SetButton(buttonCo2Scrubber, co2Scrubber ? 1 : 0);
		SetButton(buttonEnginePower, enginePower ? 1 : 0);
		SetButton(buttonEnginePowerIndicator, enginePower && isReactorOn ? 1 : 0);
		SetButton(buttonEngineReverse, engineReverse ? 1 : 0);
		SetButton(buttonEngineThrust, engineThrustTarget);
		SetButton(buttonExternalLights, externalLights ? 1 : 0);
		SetButton(buttonInternalLights, internalLights? 1 : 0);
		SetButton(buttonIsReactorOn, isReactorOn ? 1 : 0);
		SetButton(buttonIsReactorWarn, isReactorWarn ? 1 : 0);
		SetButton(buttonO2Compressor, o2Compressor ? 1 : 0);
		SetButton(buttonRudder, rudderDirectionTarget);
		SetButton(buttonRudderPower, rudderPower ? 1 : 0);
		SetButton(buttonRudderPowerIndicator, rudderPower && isReactorOn ? 1 : 0);
		SetButton(buttonSonarDirectionDial, sonarDirection);
		SetButton(buttonSonarPower, sonarPower ? 1 : 0);
		SetButton(buttonSonarPowerIndicator, sonarPower && isReactorOn ? 1 : 0);
		SetButton(buttonSonarType, sonarDirectionType ? 1 : 0);

		propellor.speed = engineThrustActual * 240;
		rudder.transform.localEulerAngles = new Vector3(rudder.transform.localEulerAngles.x, -(rudderDirectionActual-0.5f) * 45, rudder.transform.localEulerAngles.z);
	}

	private void FixedUpdate() {
		//update position based on velocity
		transform.position += new Vector3(0, velocity.y, 0) * Time.deltaTime;
		transform.position += transform.TransformVector(new Vector3(0, 0, velocity.z) * Time.deltaTime);
		//transform.position += transform.TransformVector(velocity * Time.fixedDeltaTime);
		//update rotation based on angular velocity
		transform.eulerAngles += transform.TransformVector(angularVelocity * Time.fixedDeltaTime);
		transform.eulerAngles = new Vector3(GameControl.ClampAngle(transform.eulerAngles.x, -45, 45), transform.eulerAngles.y, GameControl.ClampAngle(transform.eulerAngles.z, -15, 15));

		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z), Time.fixedDeltaTime / 5f);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0), Time.fixedDeltaTime);

		//increase velocity
		velocity += Time.fixedDeltaTime * new Vector3(0,
			(-(ballastLevelFrontActual + ballastLevelBackActual)/2 + 0.5f) / 3f, //ballast
			engineThrustActual); //engine
		//rudder & ballast
		angularVelocity += Time.fixedDeltaTime * new Vector3(
			(ballastLevelFrontActual - ballastLevelBackActual) * 5,
			5 * (rudderDirectionActual - 0.5f) * (velocity.z), //rudder
			0);

		//slow dowm from friction/drag
		velocity -= velocity * 0.01f;
		angularVelocity -= angularVelocity * 0.05f;
	}

	private void Update() {
		//CO2 Scrubber and airflow (for sub air)
		// if(isReactorOn && co2Scrubber && airflowVentilation) {
		// 	airQuality = Mathf.Clamp(airQuality + Time.deltaTime / 60, 0, 1);
		// } else {
		// 	airQuality = Mathf.Clamp(airQuality - Time.deltaTime / 60, 0, 1);
		// }

		//Reactor power
		if(isReactorOn) {
			//Oxygen compressor (for air tank)
			if(o2Compressor && o2Tank != null) {
				if(o2Tank.AddAir()) {
					o2Compressor = false;
				}
			}
			//Ballast link average out the two
			if(ballastLink) {
				float avg = (ballastLevelFrontTarget + ballastLevelBackTarget) / 2f;
				ballastLevelFrontTarget = avg;
				ballastLevelBackTarget = avg;
			}

			//Interpolate ballasts
			if(ballastPower) {
				ballastLevelFrontActual = Mathf.SmoothDamp(ballastLevelFrontActual, ballastLevelFrontTarget, ref ballastLevelFrontVelocity, dampSmoothTime, dampMaxSpeedBallast);
				ballastLevelBackActual = Mathf.SmoothDamp(ballastLevelBackActual, ballastLevelBackTarget, ref ballastLevelBackVelocity, dampSmoothTime, dampMaxSpeedBallast);
			}

			//Interpolate engine
			if(enginePower) {
				engineThrustActual = Mathf.SmoothDamp(engineThrustActual, engineThrustTarget * (engineReverse ? -1 : 1), ref engineVelocity, dampSmoothTime, dampMaxSpeedEngine);
			} else {
				engineThrustActual = Mathf.SmoothDamp(engineThrustActual, 0, ref engineVelocity, dampSmoothTime, dampMaxSpeedEngine * 3);
			}

			//Interpolate rudder
			if(rudderPower) {
				rudderDirectionActual = Mathf.SmoothDamp(rudderDirectionActual, rudderDirectionTarget, ref rudderVelocity, dampSmoothTime, dampMaxSpeedRudder);
			}
			
			//Sonar
			if(sonarPower) {
				if(sonarDirectionType) {
					
				} else {

				}
			}

			//Lights
			SetLights(true, internalLights);
			SetLights(false, externalLights);
		} else {
			//Lights off
			SetLights(true, false);
			SetLights(false, false);
			//Engine spooldown
			engineThrustActual = Mathf.SmoothDamp(engineThrustActual, 0, ref engineVelocity, dampSmoothTime, dampMaxSpeedEngine * 3);
		}
		
		UpdateVisuals();
	}
}
