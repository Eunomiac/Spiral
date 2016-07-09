using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	// [Overloaded] Returns the LAST value from a given array OR list.
	public static T Last<T> (this T[] array)
	{
		return array[array.Length - 1];
	}

	public static T Last<T> (this List<T> list)
	{
		return list[list.Count - 1];
	}
	
	// PUSH operation, adding item to the end of a list.  Returns true if item is unique, but will still add duplicates unless noDuplicates set to false.
	public static bool Push<T> (this List<T> list, T item, bool noDuplicates = true)
	{
		if ( list.Contains(item) )
			return false;
		else
		{
			list.Add(item);
			return true;
		}
	}

	// POP operation, removing item from the end of a list and returning item, or default value if list is empty.  Set "isRandom" to true for random item.
	public static T Pop<T> (this List<T> list, bool isRandom = false)
	{
		if (list.Count == 0)
			return default(T);
		int index = isRandom ? UnityEngine.Random.Range(0, list.Count) : list.Count - 1;
		T item = list[index];
		list.RemoveAt(index);
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
	public static bool FuzzyEquals (this Vector3 vec1, Vector3 vec2, float fuzziness = 0.2f)
	{
		return (vec1 - vec2).sqrMagnitude < fuzziness;
	}

	// Returns 2D distance between two Vector3's, ignoring y-value.
	public static float Distance2D (this Vector3 vec1, Vector3 vec2)
	{
		vec1 = new Vector3(vec1.x, 0f, vec1.z);
		vec2 = new Vector3(vec2.x, 0f, vec2.z);
		return Vector3.Distance(vec1, vec2);
	}
	#endregion

}
