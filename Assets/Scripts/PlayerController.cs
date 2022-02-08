using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public static PlayerController pc;

	public bool helmetEquipped = false;

	public Vector3 deltaRot = new Vector3();
	public Vector3 lastRot = new Vector3();
	public float sensitivity = 10;
	public Vector3 aimDir = new Vector3();
	public Animator suitAnimator;
	public Animator helmetAnimator;
	private void Awake() {
		if (pc == null) { pc = this; }
		else { Destroy(gameObject); }
	}

	private void Update() {
		deltaRot = transform.eulerAngles - lastRot;
		//TODO better rotation
		transform.localEulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * sensitivity;
		// transform.localEulerAngles = new Vector3(GameControl.ClampAngle((transform.localEulerAngles.x), 270, 90), transform.localEulerAngles.y, transform.localEulerAngles.z);

		if(Input.GetKeyDown(KeyCode.F)) {
			suitAnimator.Play("suit_equip");
		}
		if(Input.GetKeyDown(KeyCode.G)) {
			if(helmetEquipped) {
				helmetAnimator.Play("helmet_unequip");
			} else {
				helmetAnimator.Play("helmet_equip");
			}
			helmetEquipped = !helmetEquipped;
		}
	}

	private void LateUpdate() {
		lastRot = transform.eulerAngles;
	}
}
