using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPropellor : MonoBehaviour {
	[Range(-240, 240)]
	public float speed = 0;

	private ParticleSystem ps;

	private void Awake() {
		ps = GetComponent<ParticleSystem>();
	}
	private void Update() {
		transform.localEulerAngles += new Vector3(0, 0, speed * 360 / 60 * Time.deltaTime);
		ParticleSystem.EmissionModule m = ps.emission;
		m.rateOverTime = speed / 4;
		ParticleSystem.MainModule mm = ps.main;
		mm.startSpeed = speed/24;
		mm.startSize = speed/120;
	}
}
