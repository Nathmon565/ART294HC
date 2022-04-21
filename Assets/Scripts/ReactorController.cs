using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorController : MonoBehaviour {
	public SubmarineController sc;
	[Range(0, 1)]
	public float temperature;
	[Range(0, 4)]
	public float powerDraw;
	[Range(0, 1)]
	public float efficiency;
	[Range(0, 20)]
	public float voltage;

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
	}
}
