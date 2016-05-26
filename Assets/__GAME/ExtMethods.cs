using UnityEngine;
using System;
using System.Collections;

public static class ExtMethods {

	// Returns value in an array given ANY (int)index, wrapping as necessary.
	public static T Wrap<T>(this T[] array, int index)
	{
		int thisIndex = index;
		while ( thisIndex >= index )
			thisIndex -= array.Length;
		while ( thisIndex < 0 )
			thisIndex += array.Length;
		return array[thisIndex];
	}

	// Wraps an angle value to between 0 and 360.
	public static float Clamp(this float angle)
	{
		return Mathf.Repeat(angle, 360f);	
	}

	// Returns closest angular distance to given angle, between -180f and 180f.
	public static float Diff(this float angle, float angleA)
	{
		float thisAngle = angle.Clamp();
		angleA = angleA.Clamp();
		float diff = (angleA - thisAngle).Clamp();
		diff += (diff <= -180f) ? 360f : 0f;
		diff -= (diff > 180f) ? 360f : 0f;
		return diff;
	}

	// Returns TRUE if given angle is between parameter angles on acute side.
	public static bool IsBetween(this float angle, float angleA, float angleB)
	{
		float minAngle = angleA.Diff(angleB) > 0f ? angleA : angleB;
		float maxAngle = angleA.Diff(angleB) <= 0f ? angleA : angleB;
		return angle.Diff(minAngle) < 0f && angle.Diff(maxAngle) > 0f;
	}
}
