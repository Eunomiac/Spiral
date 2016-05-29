using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ExtMethods {

	#region Array, List & Dictionary Methods
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

	// [Overloaded] Returns a random value from a given array OR list.
	public static T Random<T> (this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public static T Random<T> (this List<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// PUSH operation, adding item to the end of a list and returning list.
	public static List<T> Push<T> (this List<T> list, T item)
	{
		list.Add(item);
		return list;
	}

	// POP operation, removing item from the end of a list and returning item.
	public static T Pop<T> (this List<T> list)
	{
		T item = list[list.Count];
		list.RemoveAt(list.Count);
		return item;
	}
	#endregion

	#region Angle Methods
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
	#endregion

	#region Vector3 Methods
	// Returns TRUE if given vector equals parameter vector, with fuzziness parameter.
	public static bool FuzzyEquals (this Vector2 vec1, Vector2 vec2, float fuzziness = 0.2f)
	{
		return (vec1 - vec2).sqrMagnitude < fuzziness;
	}
	#endregion

}
