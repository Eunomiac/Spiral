using System.Collections.Generic;
using System.Linq;
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
        if ( noDuplicates && list.Contains(item) )
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

    // Normalize values in float array or List to between -1f and 1f.
    public static float[] Normalize (this float[] array)
    {
        return array.ToList().Normalize().ToArray();
    }

    public static List<float> Normalize (this List<float> list)
    {
        float maxVal = 0f;
        foreach ( float value in list )
            maxVal = Mathf.Max(Mathf.Abs(value), maxVal);
        List<float> normVals = new List<float>();
        for ( int i = 0; i < list.Count; i++ )
            normVals.Add(list[i] / maxVal);
        return normVals;
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
    public static Vector3 Normal2D (this Vector3 vec1, Vector3 vec2, float length, bool isDebugging = false)
    {
        //float yVal = vec1.y;
        Vector3 diffVec = vec2 - vec1;
        if ( isDebugging )
        {
            Vector3.zero.DrawLine(vec1, Color.red, "Vec1: " + vec1.ToString());
            Vector3.zero.DrawLine(vec2, new Vector4(1f, 0.5f, 0f, 1f), "Vec2: " + vec2.ToString());
            Vector3.zero.DrawLine(diffVec, Color.blue, "DiffVec: " + diffVec.ToString());
            Vector3 orthoVec = Vector3.Normalize(new Vector3(-diffVec.z, 0f, diffVec.x)) * length;
            Vector3.zero.DrawLine(orthoVec, new Vector4(1f, 0f, 0.5f, 1f), "Normalized: " + orthoVec.ToString());
        }
        return Vector3.Normalize(new Vector3(-diffVec.z, 0f, diffVec.x)) * length;
    }

    // Returns 2D distance between two Vector3's, ignoring y-value.
    public static float Distance2D (this Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Distance(vec1.Flatten(), vec2.Flatten());
    }

    // Returns facing angle of a given vector, ignoring y-value, with (0,1) equalling 0 degrees.
    public static float FacingAngle (this Vector3 vec)
    {
        return vec.x >= 0 ? Vector3.Angle(Vector3.forward, vec.Flatten()) : (360f - Vector3.Angle(Vector3.forward, vec.Flatten()));
    }

    public static LineRenderer DrawLine (this Vector3 start, Vector3 end, Color color, string name = "Line", GameObject parent = null)
    {
        LineRenderer thisLine = new GameObject(name, typeof(LineRenderer)).GetComponent<LineRenderer>();
        thisLine.SetColors(color, color);
        Vector3[] targetVertices = new Vector3[2] { start, end };
        thisLine.SetPositions(targetVertices);
        thisLine.SetWidth(0.3f, 0.15f);
        thisLine.material = Resources.Load("DefaultMaterial") as Material;
        return thisLine;
    }

    public static GameObject DrawPoint (this Vector3 position, Color color, string name = "Indicator", GameObject parent = null)
    {
        SpriteRenderer thisSprite = new GameObject(name, typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        Sprite newSprite = (Resources.Load("IndicatorSprite") as GameObject).GetComponent<SpriteRenderer>().sprite;
        thisSprite.sprite = newSprite;
        //Debug.Log(newSprite.ToString());
        //thisSprite.material = Resources.Load("DefaultMaterial") as Material;
        thisSprite.color = color;
        thisSprite.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        if ( parent )
            thisSprite.transform.SetParent(parent.transform);
        thisSprite.transform.position = position;
        thisSprite.transform.forward = Vector3.up;
        return thisSprite.gameObject;
    }



    #endregion

    #region GameObject, Transform, Child Methods

    // Iterates through children, destroying them all.
    public static void DestroyAllChildren (this Transform transform, bool isSelfDestructing = true)
    {
        foreach ( Transform child in transform )
            child.DestroyAllChildren();
        if ( isSelfDestructing )
            Object.Destroy(transform.gameObject);
    }

    // Sets parent of one GameObject to supplied GameObject, with local vs. world coordinates as specified.
    public static void SetParent (this GameObject child, GameObject parent, bool isKeepingWorldCoords = false, GameObject finalCoordinates = null)
    {
        child.transform.SetParent(parent.transform, isKeepingWorldCoords);
        if ( finalCoordinates )
        {
            child.transform.position = finalCoordinates.transform.position;
            child.transform.rotation = finalCoordinates.transform.rotation;
        }
    }

    // Returns a list of a GameObject's child components EXCLUDING any such components on the GameObject itself.
    public static List<T> GetChildrenOfType<T> (this GameObject parent, bool includeInactive = true)
    {
        List<T> childComps = parent.GetComponentsInChildren<T>(true).ToList();
        childComps.Remove(parent.GetComponent<T>());
        return childComps;
    }

    #endregion

    #region Tweening Methods

    public static Vector3[] TweenPathTo (this Vector3 startPos, Vector3 endPos, int numBends, float deviationDist, bool isStraightening = false, bool isFirstBendPositive = true)
    {
        float[] deviations = new float[numBends];
        for ( int i = 0; i < numBends; i++ )
            deviations[i] = deviationDist * (i % 2 == 0 ? 1 : -1) * (isFirstBendPositive ? 1 : -1);
        return startPos.TweenPathTo(endPos, deviations, isStraightening, false, isFirstBendPositive);
    }

    public static Vector3[] TweenPathTo (this Vector3 startPos, Vector3 endPos, float[] deviations, bool isStraightening = false, bool isRandomizingStep = false, bool isFirstBendPositive = true)
    {
        float yVal = startPos.y;
        startPos = startPos.Flatten();
        endPos = endPos.Flatten();
        //Debug.Log("Start: " + startPos.ToString() + ", End: " + endPos.ToString());
        int numBends = deviations.Length;
        if ( deviations[0] < 0 == isFirstBendPositive )
            for ( int i = 0; i < deviations.Length; i++ )
                deviations[i] *= -1;
        Vector3[] thisPath = new Vector3[numBends + 4 + (isStraightening ? 1 : 0)];
        Vector3 pathNormalized = Vector3.Normalize(endPos - startPos);
        float travelDist = startPos.Distance2D(endPos);
        //LineRenderer thisPathLine = startPos.Flatten(3.5f).DrawLine((pathNormalized * travelDist + startPos).Flatten(3.5f), Color.magenta, "PATH NORMALIZED");
        //float stepDist = travelDist / (numBends + 1) * (isStraightening ? 0.75f : 1f);
        float stepDist = travelDist / (numBends + 1);
        float stepRandomDev = stepDist * 0.4f;
        thisPath[0] = startPos - pathNormalized * travelDist * 0.25f;
        thisPath[1] = startPos;
        //Debug.Log("RANDOMIZING? " + isRandomizingStep);
        for ( int i = 2; i < 2 + numBends; i++ )
        {
            float thisDistance = stepDist * (i - 1) + (isRandomizingStep ? UnityEngine.Random.Range(-stepRandomDev, stepRandomDev) : 0f);
            thisDistance = Mathf.Max(stepDist, thisDistance);
            thisDistance = Mathf.Min(thisDistance, travelDist - stepDist);
            //Debug.Log(i + "i Distance = " + thisDistance.ToString() + ", Deviation = " + deviations[i - 2]);
            Vector3 pointOnPath = pathNormalized * thisDistance + startPos;
            float thisDeviation = deviations[i - 2];
            thisPath[i] = startPos.Normal2D(pointOnPath, thisDeviation) + pointOnPath;
            //thisPath[i] += thisPath[i] + pointOnPath + startPos;
            //pointOnPath.Flatten(5f).DrawPoint(Color.yellow, i + ": PointOnPath");
            //thisPath[i].Flatten(5f).DrawPoint(Color.cyan, i + ": Deviation");
            //pointOnPath.Flatten(5f).DrawLine(thisPath[i].Flatten(5f), Color.green, i + ": NORMAL");

        }
        if ( isStraightening )
            thisPath[numBends + 2] = endPos - pathNormalized * stepDist * 0.5f;
        thisPath[numBends + 2 + (isStraightening ? 1 : 0)] = endPos;
        thisPath[numBends + 3 + (isStraightening ? 1 : 0)] = endPos + pathNormalized * travelDist * 0.25f;
        for ( int i = 0; i < thisPath.Length; i++ )
        {
            thisPath[i] = thisPath[i].Flatten(yVal);
            //Debug.Log(i + ": " + thisPath[i].ToString());
        }
        return thisPath;
    }

    public static float[] CalcDeviations (this float[] deviations, float minDev, float maxDev)
    {
        int numBends = deviations.Length;
        float[] properDeviations = new float[numBends];
        for ( int i = 0; i < numBends; i++ )
        {
            float absDev = minDev + Mathf.Abs(deviations[i]) * (maxDev - minDev);
            properDeviations[i] = absDev * (deviations[i] < 0 ? -1 : 1);
        }
        return properDeviations;
    }

    #endregion

    #region Debug Methods






    #endregion


}
