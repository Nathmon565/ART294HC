using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {
	public bool isActive = false;
	public int lightIndex = 0;
	public List<Light> l = new List<Light>();
	private MeshRenderer mr;

	private void Start() {
		if(TryGetComponent<Light>(out Light light)) { l.Add(light); }
		foreach(Transform c in transform) {
			if(c.TryGetComponent<Light>(out Light lt)) { l.Add(lt); }
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
