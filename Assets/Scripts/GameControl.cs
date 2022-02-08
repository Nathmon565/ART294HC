using UnityEngine;
public class GameControl : MonoBehaviour {

	///<summary>
	///Clamps a rotation inversely around two angles. Opposite of a normal clamp, works with angles.
	///<param name="angle">The value to be clamped and returned</param>
	///<param name="maxAngle">The maximum (top) value of the clamp and circle. This should be negative.</param>
	///<param name="minAngle">The minimum (bottom) value of the clamp and circle. This should be positive.</param>
	///</summary>
	public static float ClampAngle(float angle, float maxAngle, float minAngle)	{
		angle = Mathf.Repeat(angle, 360);
		maxAngle = Mathf.Repeat(maxAngle, 360);
		minAngle = Mathf.Repeat(minAngle, 360);

		float halfwayMod = Mathf.Repeat((Mathf.Abs(maxAngle) + Mathf.Abs(minAngle)) / 2, 360);
		//Debug.Log("(" + Mathf.RoundToInt(angle) + ", " + Mathf.RoundToInt(maxAngle) + ", " + Mathf.RoundToInt(minAngle) + " | " + Mathf.RoundToInt(halfwayMod) + "), (" + Mathf.RoundToInt(angle) + ", " + Mathf.RoundToInt(minAngle) + ", " + Mathf.RoundToInt(maxAngle) + " | " + Mathf.RoundToInt(halfwayMod) + ")");
		//TODO BUG tank destroyer angle when it crosses 0/360 it goes to the opposite clamp
		if (maxAngle == 360) { maxAngle++; }
		if (minAngle == 360) { minAngle++; }
		if (maxAngle >= halfwayMod) 		{
			if (angle > halfwayMod) 			{
				return Mathf.Max(angle, maxAngle);
			}
			return Mathf.Min(angle, minAngle);
		} else {
			if (angle < halfwayMod) {
				return Mathf.Max(angle, maxAngle);
			}
			return Mathf.Min(angle, minAngle);
		}
	}
}