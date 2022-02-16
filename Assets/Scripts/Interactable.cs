using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour {
	public Outline outline;

	private Transform playerView;
	private void Awake() {
		if (outline == null) { outline = GetComponent<Outline>(); }
	}

	private void Start() {
		if (playerView == null) {
			playerView = PlayerController.pc.view.transform;
		}
	}

	private void Update() {

		outline.eraseRenderer = Vector3.Distance(transform.position, playerView.position) > PlayerController.pc.maxInteractionDistance;
		if (Physics.Raycast(playerView.position, playerView.forward, out RaycastHit hit, PlayerController.pc.maxInteractionDistance) && hit.transform == transform) {
			outline.color = 1;
		} else {
			outline.color = 0;
		}
	}
}
