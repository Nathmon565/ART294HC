using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScooterController : MonoBehaviour {
	[Range(0,240)]
	public float speed = 0;

	public GameObject propellor;

	private void Update() {
		propellor.transform.localEulerAngles -= new Vector3(0, 0, speed*360/60*Time.deltaTime);
	}
}
