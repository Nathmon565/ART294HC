using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetFollowDelay : MonoBehaviour {
	public float mult = 2;
	public float smooth = 8;

	float lastX = 0;
	float lastY = 0;

	void LateUpdate() {
		float mouseX = Input.GetAxisRaw("Mouse X") * mult;
		float mouseY = Input.GetAxisRaw("Mouse Y") * mult;

		Quaternion rotX = Quaternion.AngleAxis(mouseY, Vector3.right);
		Quaternion rotY = Quaternion.AngleAxis(-mouseX, Vector3.up);

		Quaternion rotTarget = rotX * rotY;

		transform.localRotation = Quaternion.Slerp(transform.localRotation, rotTarget, smooth * Time.deltaTime);
		lastX = transform.parent.eulerAngles.y;
		lastY = transform.parent.eulerAngles.x;
	}
}
