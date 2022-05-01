using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBoardButton : MonoBehaviour {
  public enum ButtonType { slider, onoff, indicator, dial };
	public ButtonType buttonType;
	public float associatedValue;
	public int lightIndex = 0;
	public bool horizontal = false;

	private SubmarineController sc;
	private MeshRenderer mr;
	private void Awake() {
		mr = GetComponent<MeshRenderer>();
		sc = transform.root.GetComponent<SubmarineController>();
	}
	public void UpdateVisual() {
		switch(buttonType) {
			case ButtonType.slider:
				transform.localEulerAngles = new Vector3(150f * (1-associatedValue) - 75f, transform.localEulerAngles.y, transform.localEulerAngles.z);
			break;
			case ButtonType.onoff:
				transform.localEulerAngles = new Vector3((1-associatedValue) * 90f - 45f, transform.localEulerAngles.y, transform.localEulerAngles.z);
			break;
			case ButtonType.indicator:
				Material[] mats = mr.materials;
				mats[lightIndex] = associatedValue == 1 ? GameControl.gc.redLED : GameControl.gc.lightOff;
				mr.materials = mats;
			break;
			case ButtonType.dial:
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, associatedValue * 360f, transform.localEulerAngles.z);
			break;
			default:
				Debug.LogWarning("Control board button type not set: " + name);
			break;
		}
	}
	public void ToggleState() {
		SetState(-1);
	}

	public void SetState(float val) {
		switch(buttonType) {
			case ButtonType.slider:
			case ButtonType.dial:
				
				associatedValue = val;
			break;
			case ButtonType.onoff:
				if(val != -1) { Debug.LogWarning("Called SetState() on a switch"); }
				associatedValue = associatedValue == 1 ? 0 : 1;
				GameControl.gc.ac.PlaySound("switch", transform);
			break;
		}
		sc.ReceiveCommand(this);
	}
}
