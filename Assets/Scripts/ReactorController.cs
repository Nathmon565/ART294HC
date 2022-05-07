using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorController : MonoBehaviour {
	public SubmarineController sc;
	[Range(0, 1)]
	public float temperature;
	public Transform temperatureNeedle;
	[Range(0, 4)]
	public float powerDraw;
	public Transform powerDrawNeedle;
	[Range(0, 1)]
	public float efficiency;
	public Transform efficiencyNeedle;
	[Range(0, 1)]
	public float voltage;
	public Transform voltageNeedle;

	public void CalculatePowerDraw() {
		if(sc == null) { return; }
		powerDraw = 0;
		powerDraw += sc.enginePower ? sc.engineThrustTarget : 0;
		powerDraw += sc.ballastPower ? sc.GetPumpWork() : 0;
		powerDraw += sc.rudderPower ? sc.GetRudderWork() : 0;
		powerDraw += sc.sonarPower ? 0.05f : 0;
		powerDraw += sc.externalLights ? 0.01f : 0;
		powerDraw += sc.internalLights ? 0.005f : 0;
		powerDraw += sc.co2Scrubber ? 0.02f : 0;
		powerDraw += sc.o2Compressor ? 0.05f : 0;
		powerDraw += sc.airflowVentilation ? 0.01f: 0;
	}

	private void Update() {
		CalculatePowerDraw();
		UpdateNeedles();
	}

	public void UpdateNeedles() {
		powerDrawNeedle.transform.localEulerAngles = new Vector3(0, 0, powerDraw * Random.Range(0.85f, 0.95f)) * 360f;
		temperatureNeedle.transform.localEulerAngles = new Vector3(0, 0, temperature * Random.Range(0.85f, 0.95f)) * 360f;
		voltageNeedle.transform.localEulerAngles = new Vector3(0, 0, voltage * Random.Range(0.55f, 0.65f)) * 360f;
		efficiencyNeedle.transform.localEulerAngles = new Vector3(0, 0, efficiency * Random.Range(0.98f, 1)) * 360f;
	}
}
