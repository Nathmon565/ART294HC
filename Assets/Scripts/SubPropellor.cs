using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPropellor : MonoBehaviour {
	[Range(0, 240)]
	public float speed = 0;

	private void Update() {
		transform.localEulerAngles -= new Vector3(0, 0, speed * 360 / 60 * Time.deltaTime);
	}
}
