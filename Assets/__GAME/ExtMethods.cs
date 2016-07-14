using System.Collections.Generic;
using UnityEngine;

public static class ExtMethods
{

    #region Array, List & Dictionary Methods
    // Returns value in an array given ANY (int)index, wrapping as necessary.
    public static T Wrap<T> (this T[] array, int index)
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
        if ( list.Count == 0 )
            return default(T);
        int index = isRandom ? UnityEngine.Random.Range(0, list.Count) : list.Count - 1;
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

    #endregion

    #region Angle Methods
    // Wraps an angle value to between 0 and 360.
    public static float Clamp (this float angle)
    {
        return Mathf.Repeat(angle, 360f);
    }

    // Returns closest angular distance to given angle, between -180f and 180f.
    public static float Diff (this float angle, float angleA)
    {
        float diff = (angleA.Clamp() - angle.Clamp()).Clamp();
        return diff + (diff <= -180 ? 360 : 0) - (diff > 180 ? 360 : 0);
    }

    // Returns TRUE if given angle is between parameter angles on acute side.
    public static bool IsBetween (this float angle, float angleA, float angleB)
    {
        float minAngle = angleA.Diff(angleB) > 0f ? angleA : angleB;
        float maxAngle = angleA.Diff(angleB) <= 0f ? angleA : angleB;
        return angle.Diff(minAngle) < 0f && angle.Diff(maxAngle) > 0f;
    }
    #endregion

    #region Vector3 Methods

    // "Flattens" a Vector3 on the xz-plane, setting its y-value to zero OR the provided value.
    public static Vector3 Flatten (this Vector3 vec, float yVal = 0f)
    {
        return new Vector3(vec.x, yVal, vec.z);
    }

    // Returns TRUE if given vector equals parameter vector, with fuzziness parameter.
    public static bool FuzzyEquals (this Vector3 vec1, Vector3 vec2, float fuzziness = 0.2f)
    {
        return (vec1.Flatten() - vec2.Flatten()).sqrMagnitude < fuzziness;
    }

    // Returns a flattened orthogonal Vector3 from two points, of a supplied length, counter-clockwise from point 1 to point 2. 
    public static Vector3 Normal2D (this Vector3 vec1, Vector3 vec2, float length, float yVal = 0f)
    {
        Vector3 diffVec = vec2.Flatten(yVal) - vec1.Flatten(yVal);
        Vector3 orthoVec = new Vector3(-diffVec.z, 0f, diffVec.x) / diffVec.magnitude;
        return orthoVec * length;
    }

    // Returns 2D distance between two Vector3's, ignoring y-value.
    public static float Distance2D (this Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Distance(vec1.Flatten(), vec2.Flatten());
    }

    // Returns 2D normalized Vector3 given a Vector3, ignoring y-values.
    public static Vector3 Normalize2D (this Vector3 vec, float yVal = 0f)
    {
        return Vector3.Normalize(vec.Flatten(0f)).Flatten(yVal);
    }

    // Returns facing angle of a given vector, ignoring y-value, with (0,1) equalling 0 degrees.
    public static float FacingAngle (this Vector3 vec)
    {
        return vec.x >= 0 ? Vector3.Angle(Vector3.forward, vec.Flatten()) : (360f - Vector3.Angle(Vector3.forward, vec.Flatten()));
    }
    #endregion

}
