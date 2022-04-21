using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {
	public bool isActive = false;
	public int lightIndex = 0;
	public List<Light> l;
	private MeshRenderer mr;

	private void Start() {
		if(TryGetComponent<Light>(out Light light)) { l.Add(light); }
		foreach(GameObject c in transform) {
			if(TryGetComponent<Light>(out light)) { l.Add(light); }
		}
		mr = GetComponent<MeshRenderer>();
		SetActive(false);
	}

	[ContextMenu("Toggle Active")]
	public void ToggleActive() {
		SetActive(!isActive);
	}
	public void SetActive(bool active) {
		Material[] mats = mr.materials;
		mats[lightIndex] = active ? GameControl.gc.lightOn : GameControl.gc.lightOff;
		mr.materials = mats;
		isActive = active;
		foreach(Light light in l) {
			light.enabled = active;
		}
	}
}
