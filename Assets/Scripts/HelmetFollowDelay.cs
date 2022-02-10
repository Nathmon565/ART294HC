using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetFollowDelay : MonoBehaviour {
	public Transform target;
	public float mult = 2;
	public float smooth = 8;

	public Animator animator;
	public Animation equipAnimation;
	private void Start() {
		if(target == null) { target = Camera.main.transform; }
		if(animator == null) { animator = GetComponent<Animator>(); }
	}
	void LateUpdate() {

		if(PlayerController.pc.helmetEquipped && animator.GetCurrentAnimatorStateInfo(0).IsName("helmet_equipped_idle")) {
			float mouseX = Input.GetAxisRaw("Mouse X") * mult;
			float mouseY = Input.GetAxisRaw("Mouse Y") * mult;

			Quaternion rotX = Quaternion.AngleAxis(mouseY, Vector3.right);
			Quaternion rotY = Quaternion.AngleAxis(-mouseX, Vector3.up);

			Quaternion rotTarget = rotX * rotY;

			transform.localRotation = Quaternion.Slerp(transform.localRotation, rotTarget, smooth * Time.deltaTime);
		}

		// transform.localPosition += PlayerController.pc.rb.velocity;
		// transform.localPosition = Vector3.Slerp(transform.localPosition, new Vector3(), smooth * Time.deltaTime);
	}
}
