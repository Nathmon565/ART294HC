using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public static PlayerController pc;
	public bool helmetEquipped = false;

	public Vector3 deltaRot = new Vector3();
	public Vector3 lastRot = new Vector3();
	public Vector3 deltaV = new Vector3();
	public Vector3 lastV = new Vector3();
	public float sensitivity = 10;
	public Vector3 mouseMovement = new Vector3();
	public Vector3 aimDir = new Vector3();
	public Animator suitAnimator;
	public Animator helmetAnimator;
	public GameObject view;
	public Rigidbody rb;
	public float moveSpeed = 5;
	public float interactionDistance = 3;
	public bool lockCamera = false;
	public Vector3 lockedMouseMovement;
	public Vector3 originalMouseMovement;
	public GameObject verticalArrows;
	public GameObject horizontalArrows;
	private void Awake() {
		if (pc == null) { pc = this; } else { Destroy(gameObject); }
		if (view == null) { view = Camera.main.gameObject; }
		if (rb == null) { rb = GetComponent<Rigidbody>(); }
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update() {
		//Debug.DrawRay(view.transform.position, view.transform.forward * HighlightPlus.HighlightManager.instance.maxDistance, Color.red);
		verticalArrows.SetActive(false);
		horizontalArrows.SetActive(false);
		Debug.DrawRay(view.transform.position, view.transform.forward * interactionDistance, Color.red);
		if(Physics.Raycast(view.transform.position, view.transform.forward, out RaycastHit hit , interactionDistance)) {
			if(hit.transform.TryGetComponent<ControlBoardButton>(out ControlBoardButton button) || (hit.transform.childCount > 0 && hit.transform.GetChild(0).TryGetComponent<ControlBoardButton>(out button))) {
				if(button.buttonType == ControlBoardButton.ButtonType.onoff && Input.GetMouseButtonDown(0)) {
					button.ToggleState();
				} else if(button.buttonType != ControlBoardButton.ButtonType.onoff) {
					if(Input.GetMouseButtonDown(0)) {
						lockCamera = true;
						originalMouseMovement = mouseMovement;
						lockedMouseMovement = mouseMovement;
					} else if(Input.GetMouseButton(0) && lockCamera) {
						//which axis to check (for rudder vs. other switches)
						if(button.horizontal) { horizontalArrows.SetActive(true); } else { verticalArrows.SetActive(true); }
						float movement = button.horizontal ? -(lockedMouseMovement.y - mouseMovement.y) : (lockedMouseMovement.x - mouseMovement.x);
						float val = Mathf.Clamp(button.associatedValue + movement/sensitivity/3, 0, 1);
						lockedMouseMovement = mouseMovement;
						button.SetState(val);
					} else if(Input.GetMouseButtonUp(0)) {
						lockCamera = false;
						mouseMovement = originalMouseMovement;
					} else if(Input.GetMouseButtonDown(1)) {
						button.SetState(0.5f);
					}
				}
			} else {
				lockCamera = false;
				mouseMovement = originalMouseMovement;
			}
			if(hit.transform.TryGetComponent<BulkheadDoor>(out BulkheadDoor door) || (hit.transform.parent != null && hit.transform.parent.TryGetComponent<BulkheadDoor>(out door))) {
				if(Input.GetMouseButtonDown(0)) {
					if(door.isOpen) { door.Close(); }
					else { door.Open(); }
				}
			}
		}
		

		deltaRot = view.transform.eulerAngles - lastRot;
		deltaV = rb.velocity - lastV;
		
		// transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sensitivity, 0);
		// float yrot = GameControl.ClampAngle(view.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity, -89.5f, 89.5f);
		// view.transform.localEulerAngles = new Vector3(yrot, view.transform.localEulerAngles.y, view.transform.localEulerAngles.z);

		
		mouseMovement += new Vector3(-Input.GetAxis("Mouse Y") * sensitivity, Input.GetAxis("Mouse X") * sensitivity, 0);
		//mouseMovement = new Vector3(GameControl.ClampAngle(mouseMovement.x, -89.5f, 89.5f), mouseMovement.y, 0);
		if(!lockCamera) {
			mouseMovement = new Vector3(Mathf.Clamp(mouseMovement.x, -89.5f, 89.5f), Mathf.Repeat(mouseMovement.y, 360), 0);
			lockedMouseMovement = mouseMovement;
			originalMouseMovement = mouseMovement;
			//transform.eulerAngles = new Vector3(0, mouseMovement.y, 0);
			transform.localEulerAngles = new Vector3(0, mouseMovement.y, 0);
			view.transform.eulerAngles = new Vector3(mouseMovement.x, view.transform.eulerAngles.y, 0);
		}
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
		rb.velocity = transform.TransformVector(input * moveSpeed) + new Vector3(0, rb.velocity.y, 0);
	}

	private void LateUpdate() {
		lastRot = view.transform.eulerAngles;
		lastV = rb.velocity;
	}
}
