using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public static PlayerController pc;

	public Vector3 deltaRot = new Vector3();
	public Vector3 lastRot = new Vector3();
	public float sensitivity = 10;
	public Vector3 aimDir = new Vector3();
	private void Awake() {
		if (pc == null) { pc = this; }
		else { Destroy(gameObject); }
	}

	private void Update() {
		deltaRot = transform.eulerAngles - lastRot;
		//TODO better rotation
		transform.localEulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * sensitivity;
		transform.localEulerAngles = new Vector3(GameControl.ClampAngle((transform.localEulerAngles.x), 270, 90), transform.localEulerAngles.y, transform.localEulerAngles.z);
	}

	private void LateUpdate() {
		lastRot = transform.eulerAngles;
	}
}
