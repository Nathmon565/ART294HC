using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public static PlayerController pc;

	[Range(0, 5)]
	public float maxInteractionDistance = 2;
	public bool helmetEquipped = false;

	public Vector3 deltaRot = new Vector3();
	public Vector3 lastRot = new Vector3();
	public Vector3 deltaV = new Vector3();
	public Vector3 lastV = new Vector3();
	public float sensitivity = 10;
	public Vector3 aimDir = new Vector3();
	public Animator suitAnimator;
	public Animator helmetAnimator;
	public GameObject view;
	public Rigidbody rb;
	public float moveSpeed = 5;
	private void Awake() {
		if (pc == null) { pc = this; } else { Destroy(gameObject); }
		if (view == null) { view = Camera.main.gameObject; }
		if (rb == null) { rb = GetComponent<Rigidbody>(); }
	}

	private void Update() {
		Debug.DrawRay(view.transform.position, view.transform.forward * maxInteractionDistance, Color.red);


		deltaRot = view.transform.eulerAngles - lastRot;
		deltaV = rb.velocity - lastV;
		transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0);
		float yrot = GameControl.ClampAngle(view.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity, -89.5f, 89.5f);
		view.transform.localEulerAngles = new Vector3(yrot, view.transform.localEulerAngles.y, view.transform.localEulerAngles.z);

		if (Input.GetKeyDown(KeyCode.F)) {
			suitAnimator.Play("suit_equip");
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			if (helmetEquipped) {
				helmetAnimator.Play("helmet_unequip");
			} else {
				helmetAnimator.Play("helmet_equip");
			}
			helmetEquipped = !helmetEquipped;
		}

		Vector3 input = new Vector3();
		if (Input.GetKey(KeyCode.W)) {
			input += new Vector3(0, 0, 1);
		}
		if (Input.GetKey(KeyCode.S)) {
			input += new Vector3(0, 0, -1);
		}
		if (Input.GetKey(KeyCode.A)) {
			input += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey(KeyCode.D)) {
			input += new Vector3(1, 0, 0);
		}
		input.Normalize();
		rb.velocity = transform.TransformVector(input * moveSpeed);
	}

	private void LateUpdate() {
		lastRot = view.transform.eulerAngles;
		lastV = rb.velocity;
	}
}
