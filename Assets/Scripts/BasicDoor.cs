using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour {
	public bool isOpen = true;
	public bool moving = false;
	public GameObject mainDoor;
	public GameObject otherDoor;
	public Vector3 targetPos = new Vector3(1, 0, 0);
	public float doorSpeed = 1;
	private void Update() {
		Vector3 p = isOpen ? targetPos : new Vector3();
		moving = Vector3.Distance(mainDoor.transform.localPosition, p) > 0.1f;
		mainDoor.transform.localPosition = Vector3.MoveTowards(mainDoor.transform.localPosition, p, doorSpeed * Time.deltaTime);
		otherDoor.transform.localPosition = Vector3.MoveTowards(otherDoor.transform.localPosition, -p, doorSpeed * Time.deltaTime);
	}
	public void ToggleState() {
		isOpen = !isOpen;
	}
}
