using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
	public float speed;
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
	public GameObject compass;
	public TextMeshPro depthGauge;
	[Header("References")]
	public GameObject sonarCamera;
	public GameObject helm;
	public Material sonarScreenMaterial;
	public Material blackScreenMaterial;
	public Coroutine pingCoroutine;
	[Header("Sounds")]
	public AudioSource ambience;
	public AudioSource groanAmbience;
	public AudioSource reactorLoop;
	public AudioSource engineLoop;
	public AudioSource ballastFrontLoop;
	public AudioSource ballastRearLoop;
	public AudioSource pumpFrontLoop;
	public AudioSource pumpRearLoop;
	public AudioSource waterRushLoop;
	

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
		// if(isInternal) { internalLights = isActive; }
		// else { externalLights = isActive; }
	}

	public void SetButton(ControlBoardButton button, float value) {
		if(button.associatedValue == value) { return; }
		button.associatedValue = value;
		button.UpdateVisual();
	}

	public void ReceiveCommand(ControlBoardButton button) {
		if(button == buttonBallastFront) {
			ballastLevelFrontTarget = button.associatedValue;
		} else if(button == buttonBallastBack) {
			ballastLevelBackTarget = button.associatedValue;
		} else if(button == buttonBallastLink) {
			ballastLink = button.associatedValue == 1;
		} else if(button == buttonBallastPower) {
			ballastPower = button.associatedValue == 1;
		} else if(button == buttonEngineThrust) {
			engineThrustTarget = button.associatedValue;
		} else if(button == buttonEnginePower) {
			enginePower = button.associatedValue == 1;
		} else if(button == buttonEngineReverse) {
			engineReverse = button.associatedValue == 1;
		} else if(button == buttonRudderPower) {
			rudderPower = button.associatedValue == 1;
		} else if(button == buttonRudder) {
			rudderDirectionTarget = button.associatedValue;
		} else if(button == buttonSonarType) {
			sonarDirectionType = button.associatedValue == 1;
		} else if(button == buttonSonarDirectionDial) {
			sonarDirection = button.associatedValue;
		} else if(button == buttonSonarPower) {
			sonarPower = button.associatedValue == 1;
		} else if(button == buttonExternalLights) {
			externalLights = button.associatedValue == 1;
		} else if(button == buttonInternalLights) {
			internalLights = button.associatedValue == 1;
		} else if(button == buttonCo2Scrubber) {
			co2Scrubber = button.associatedValue == 1;
		} else if(button == buttonO2Compressor) {
			o2Compressor = button.associatedValue == 1;
		} else if(button == buttonAirflowVentilation) {
			airflowVentilation = button.associatedValue == 1;
		}
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

	private void Start() {
		InitializeSounds();
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log("collided with " + other.transform.name);
		if(other.transform.root == transform) { return; }
		if(velocity.magnitude > 5) {
			GameControl.gc.ac.PlaySound("collilsion_death", transform);
		}
		velocity *= -0.25f;
		angularVelocity *= 0;
		GameControl.gc.ac.PlaySound("collision", transform);
	}

	private void FixedUpdate() {
		//update position based on velocity
		transform.position += new Vector3(0, velocity.y, 0) * Time.deltaTime;
		transform.position += transform.TransformVector(new Vector3(0, 0, velocity.z) * Time.deltaTime);
		//transform.position += transform.TransformVector(velocity * Time.fixedDeltaTime);
		//update rotation based on angular velocity
		transform.localEulerAngles += transform.TransformVector(angularVelocity) * Time.fixedDeltaTime;
		transform.localEulerAngles = new Vector3(GameControl.ClampAngle(transform.localEulerAngles.x, -45, 45), transform.localEulerAngles.y, GameControl.ClampAngle(transform.localEulerAngles.z, -15, 15));

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

		//increase velocity
		velocity += new Vector3(0,
			(-(ballastLevelFrontActual + ballastLevelBackActual)/2 + 0.5f) / 10f, //ballast
			engineThrustActual * 0.1f); //engine
		//rudder & ballast
		angularVelocity += new Vector3(
			(ballastLevelFrontActual - ballastLevelBackActual) * 0.2f,
			(rudderDirectionActual - 0.5f) * (velocity.z) * 0.2f, //rudder
			0);

		//slow down from friction/drag
		velocity -= velocity * 0.01f;
		angularVelocity -= new Vector3(angularVelocity.x * 0.2f, angularVelocity.y * 0.05f, angularVelocity.z * 0.1f);
		// if(angularVelocity.magnitude > 0.05f) {
		// 	angularVelocity -= angularVelocity * 0.05f;
		// }
		speed = velocity.magnitude;

		//out of water
		if(transform.position.y > 0) {
			velocity += Physics.gravity * Time.fixedDeltaTime;
		}
		waterRushLoop.volume = velocity.magnitude/10;
		waterRushLoop.pitch = velocity.magnitude/10;
	}

	public IEnumerator SonarPings() {
		GameControl.gc.ac.PlaySound("ping", 0, buttonSonarPowerIndicator.transform);
		yield return new WaitForSeconds(3);
		pingCoroutine = StartCoroutine(SonarPings());
	}

	public void InitializeSounds() {
		engineLoop = GameControl.gc.ac.PlaySound("engine_loop", transform, new Vector3(0, 0, -11));
		reactorLoop = GameControl.gc.ac.PlaySound("reactor_loop", transform, new Vector3(0.5f, 0, -5));
		reactorLoop.pitch = 0;
		reactorLoop.volume = 0;

		ballastFrontLoop = GameControl.gc.ac.PlaySound("ballast_fill", transform, new Vector3(0, -1.25f, 6.5f));
		ballastFrontLoop.volume = 0;
		ballastRearLoop = GameControl.gc.ac.PlaySound("ballast_fill", transform, new Vector3(0, -1.25f, -6));
		ballastRearLoop.volume = 0;
		pumpFrontLoop = GameControl.gc.ac.PlaySound("pump_loop", transform, new Vector3(0, -1.25f, 6.5f));
		pumpFrontLoop.volume = 0;
		pumpRearLoop = GameControl.gc.ac.PlaySound("pump_loop", transform, new Vector3(0, -1.25f, -6));
		pumpRearLoop.volume = 0;

		waterRushLoop = GameControl.gc.ac.PlaySound("water_movement", transform);
		waterRushLoop.volume = 0;
		waterRushLoop.pitch = 0;

		StartCoroutine(RandomAmbience());
		StartCoroutine(RandomHullGroan());

		
	}

	public IEnumerator RandomAmbience() {
		ambience = GameControl.gc.ac.PlaySound("ambience", transform);
		yield return new WaitUntil(()=> ambience == null);
		yield return new WaitForSeconds(Random.Range(25, 80));
		StartCoroutine(RandomAmbience());
	}

	public IEnumerator RandomHullGroan() {
		groanAmbience = GameControl.gc.ac.PlaySound("hull_groan", transform);
		yield return new WaitUntil(()=> groanAmbience == null);
		yield return new WaitForSeconds(Random.Range(25, 80));
		StartCoroutine(RandomHullGroan());
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
			reactorLoop.pitch = Mathf.MoveTowards(reactorLoop.pitch, 1, Time.deltaTime / 5);
			reactorLoop.volume = Mathf.MoveTowards(reactorLoop.volume, 1, Time.deltaTime / 5);

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
				float fv = Mathf.Abs(ballastLevelFrontVelocity) * 10;
				if(ballastLevelFrontVelocity > 0) { ballastFrontLoop.volume = fv; }
				pumpFrontLoop.volume = fv;
				pumpFrontLoop.pitch = fv;
				ballastLevelBackActual = Mathf.SmoothDamp(ballastLevelBackActual, ballastLevelBackTarget, ref ballastLevelBackVelocity, dampSmoothTime, dampMaxSpeedBallast);
				float bv = Mathf.Abs(ballastLevelBackVelocity) * 10;
				if(ballastLevelBackVelocity > 0) { ballastRearLoop.volume = fv; }
				pumpRearLoop.volume = bv;
				pumpRearLoop.pitch = bv;
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
				sonarCamera.transform.localEulerAngles = new Vector3(0, sonarDirection * 360f, 0);
				if(pingCoroutine == null) {
					pingCoroutine = StartCoroutine(SonarPings());
					Material[] mats = helm.GetComponent<MeshRenderer>().materials;
					mats[3] = sonarScreenMaterial;
					helm.GetComponent<MeshRenderer>().materials = mats;
				}
				if(sonarDirectionType) {
					
				} else {

				}
			} else if(pingCoroutine != null) {
				Material[] mats = helm.GetComponent<MeshRenderer>().materials;
				mats[3] = blackScreenMaterial;
				helm.GetComponent<MeshRenderer>().materials = mats;
				StopCoroutine(pingCoroutine);
				pingCoroutine = null;
			}

			//Lights
			SetLights(true, internalLights);
			SetLights(false, externalLights);
		} else {
			reactorLoop.pitch = Mathf.MoveTowards(reactorLoop.pitch, 0, Time.deltaTime / 5);
			reactorLoop.volume = Mathf.MoveTowards(reactorLoop.volume, 0, Time.deltaTime / 5);
			if(pingCoroutine != null) {
				Material[] mats = helm.GetComponent<MeshRenderer>().materials;
				mats[3] = blackScreenMaterial;
				helm.GetComponent<MeshRenderer>().materials = mats;
				StopCoroutine(pingCoroutine);  pingCoroutine = null;
			}
			//Lights off
			SetLights(true, false);
			SetLights(false, false);
			//Engine spooldown
			engineThrustActual = Mathf.SmoothDamp(engineThrustActual, 0, ref engineVelocity, dampSmoothTime, dampMaxSpeedEngine * 3);
		}
		engineLoop.volume = engineThrustActual;
		engineLoop.pitch = engineThrustActual/3 + 0.5f;

		compass.transform.localEulerAngles = new Vector3(0, -transform.eulerAngles.y, 0);

		depthGauge.text = "Depth: " + ((-transform.position.y).ToString("n1") + "m");
		
		UpdateVisuals();
	}
}
