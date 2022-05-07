using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkheadDoor : MonoBehaviour {
	public GameObject valve;
	public float openAngle;
	public bool isMoving = false;
	public bool isOpen = false;
	[ContextMenu("Open")]
	public void Open() {
		if(isOpen || isMoving) { return; }
		StartCoroutine(OpenDoor());
	}
	[ContextMenu("Close")]
	public void Close() {
		if(!isOpen || isMoving) { return; }
		StartCoroutine(CloseDoor());
	}

	public IEnumerator OpenDoor() {
		isMoving = true;
		float t = 0;
		while(t < 2.5f) {
			valve.transform.localEulerAngles += new Vector3(0, 0, 720) * Time.deltaTime / 2.5f;
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		t = 0;
		while(t < 1) {
			transform.localRotation *= Quaternion.Euler(0, openAngle * Time.deltaTime, 0);
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		isOpen = true;
		isMoving = false;
	}

	public IEnumerator CloseDoor() {
		isMoving = true;
		float t = 0;
		while(t < 1) {
			transform.localRotation *= Quaternion.Euler(0, -openAngle * Time.deltaTime, 0);
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		t = 0;
		while(t < 2.5f) {
			valve.transform.localEulerAngles -= new Vector3(0, 0, 720) * Time.deltaTime / 2.5f;
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		isOpen = false;
		isMoving = false;
	}
}
