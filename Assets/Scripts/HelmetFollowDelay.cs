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
		if (target == null) { target = Camera.main.transform; }
		if (animator == null) { animator = GetComponent<Animator>(); }
	}
	void LateUpdate() {
		transform.GetChild(0).gameObject.SetActive(PlayerController.pc.helmetEquipped);

		if (PlayerController.pc.helmetEquipped && animator.GetCurrentAnimatorStateInfo(0).IsName("helmet_equipped_idle")) {
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

	public void PlaySound(string sound) {

		string[] s = sound.Split('=');
		Vector3 v = new Vector3(0, 0.1f, 0);
		if (s[0] == "f" && animator.GetCurrentAnimatorStateInfo(0).IsName("helmet_equip")) {
			GameControl.gc.ac.PlaySound(s[1], transform, v);
		} else if (s[0] == "b" && animator.GetCurrentAnimatorStateInfo(0).IsName("helmet_unequip")) {
			GameControl.gc.ac.PlaySound(s[1], transform, v);
		}
	}
}
