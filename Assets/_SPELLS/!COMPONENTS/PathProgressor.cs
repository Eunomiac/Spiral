using System.Collections;
using System.Linq;
using UnityEngine;

public class PathProgressor : MonoBehaviour
{

    public GameObject target;
    public float travelTime = 1f;
    public int numDeviations = -1;
    public float minDev = 1f;
    public float maxDev = 1f;
    public bool isTurningToFaceTarget = true;
    public bool isRandomizingStepDistance = true;
    public PathMaximum pathMax = PathMaximum.ATEND;
    public LeanTweenType easingMode;
    public AudioClip[] boltLaunchSFX;
    public GameObject boltPrefab;


    public enum PathMaximum { ATSTART, ATEND, ATMIDDLE }

    private Vector3 startPos, endPos;
    private float yValue, distance;
    private int numBends;


    void Start ()
    {
        transform.position = new Vector3(-1.08f, 1.78f, 1.28f);
        yValue = transform.position.y;
        startPos = transform.position.Flatten(yValue);
        endPos = target.transform.position.Flatten(yValue);
        distance = startPos.Distance2D(endPos);
        numBends = (numDeviations == -1) ? Mathf.RoundToInt(distance / 2) : numDeviations;


        float[] deviations = new float[numBends];
        deviations[0] = 0f;
        float devStep = 1f / deviations.Length;
        float absDev = devStep;
        float prevDev;
        for ( int i = 0; i < deviations.Length; i += 2 )
        {
            prevDev = absDev;
            absDev = Mathf.Min(1f, prevDev + devStep * (pathMax == PathMaximum.ATMIDDLE && i > (deviations.Length * 0.5f) ? -1 : 1));
            deviations[i] = absDev;
            if ( i != (deviations.Length - 1) )
                deviations[i + 1] = absDev * -1;
        }
        if ( pathMax == PathMaximum.ATSTART )
            deviations.Reverse();
        deviations = deviations.Normalize();

        //foreach ( float dev in deviations )
        //{
        //    Debug.Log(dev);
        //}
        //float[] newDeviations = deviations.CalcDeviations(1f, 3f);
        //foreach ( float dev in newDeviations )
        //{
        //    Debug.Log(dev);
        //}
        Vector3[] thisPath = startPos.TweenPathTo(endPos, deviations.CalcDeviations(minDev, maxDev), isTurningToFaceTarget, isRandomizingStepDistance, false);

        StartCoroutine(LaunchFireball(thisPath));



    }

    IEnumerator LaunchFireball (Vector3[] path)
    {
        yield return new WaitForSeconds(2f);
        LTDescr thisTween = LeanTween.moveSpline(gameObject, path, travelTime).setEase(easingMode).setOrientToPath(true);
    }

}
