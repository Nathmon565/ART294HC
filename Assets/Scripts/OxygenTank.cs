using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTank : MonoBehaviour {
	//measured in liters
	public float currentUncompressedFill = 2400;
	public float maxUncompressedFill = 2400;

	public void TakeBreath(float activity) {
		currentUncompressedFill -= Mathf.Lerp(0.5f, 3, activity);
		if(currentUncompressedFill < 0) {
			currentUncompressedFill = 0;
		}
	}

	public float getPercentRemaining() { return currentUncompressedFill / maxUncompressedFill; }

	public bool AddAir() {
		return AddAir(Time.deltaTime * 5);
	}
	
	public bool AddAir(float q) {
		currentUncompressedFill += q;
		if(currentUncompressedFill >= maxUncompressedFill) {
			currentUncompressedFill = maxUncompressedFill;
			return true;
		}
		return false;
	}
}
