using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

[RequireComponent(typeof(HighlightEffect))]
public class Interactable : MonoBehaviour {
	public HighlightEffect highlight;
	public HighlightProfile active;
	public HighlightProfile disabled;
	public GameObject target;
	public BasicDoor door;
	private Transform playerView;
	private void Awake() {
		if (highlight == null) { highlight = GetComponent<HighlightEffect>(); }
		if (target == null) { target = transform.parent.gameObject; }
		if (door == null) { door = target.GetComponent<BasicDoor>(); }
	}

	private void Start() {
		if (playerView == null) {
			playerView = PlayerController.pc.view.transform;
		}
	}

	private void Update() {
		if (Physics.Raycast(playerView.position, playerView.forward, out RaycastHit hit, HighlightManager.instance.maxDistance) && hit.transform == transform) {
			if (door.moving) {
				if (highlight.profile != disabled) {
					highlight.profile = disabled;
					highlight.ProfileReload();
				}
				if (Input.GetKeyDown(KeyCode.E)) {
					GameControl.gc.ac.PlaySound("door_denied", 0, transform);
				}
			} else {
				if (highlight.profile != active) {
					highlight.profile = active;
					highlight.ProfileReload();
				}
				if (Input.GetKeyDown(KeyCode.E)) {
					target.GetComponent<BasicDoor>().isOpen = !target.GetComponent<BasicDoor>().isOpen;
					GameControl.gc.ac.PlaySound("door_granted", 0, transform);
				}
			}
		}
	}
}
