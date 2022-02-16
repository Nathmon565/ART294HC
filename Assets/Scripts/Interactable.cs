using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour {
	public Outline outline;
	public GameObject target;
	public BasicDoor door;
	private Transform playerView;
	private void Awake() {
		if (outline == null) { outline = GetComponent<Outline>(); }
		if (target == null) { target = transform.parent.gameObject; }
		if (door == null) { door = target.GetComponent<BasicDoor>(); }
	}

	private void Start() {
		if (playerView == null) {
			playerView = PlayerController.pc.view.transform;
		}
	}

	private void Update() {

		outline.eraseRenderer = Vector3.Distance(transform.position, playerView.position) > PlayerController.pc.maxInteractionDistance;
		if (Physics.Raycast(playerView.position, playerView.forward, out RaycastHit hit, PlayerController.pc.maxInteractionDistance) && hit.transform == transform) {
			if (door.moving) {
				outline.color = 2;
				if (Input.GetKeyDown(KeyCode.E)) {
					GameControl.gc.ac.PlaySound("door_denied", 0, transform);
				}
			} else {
				outline.color = 1;
				if (Input.GetKeyDown(KeyCode.E)) {
					target.GetComponent<BasicDoor>().isOpen = !target.GetComponent<BasicDoor>().isOpen;
					GameControl.gc.ac.PlaySound("door_granted", 0, transform);
				}
			}
		} else {
			outline.color = 0;
		}
	}
}
