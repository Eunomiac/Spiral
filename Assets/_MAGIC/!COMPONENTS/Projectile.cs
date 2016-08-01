using System.Linq;
using UnityEngine;

public class Projectile : SPELLEFFECT
{
    public float travelTime = 1f;
    public float searchAngle = 60f;
    public float minDist = 0f;
    public float maxDist = GAME.BIGINT;
    public int numDeviations = -1;
    public float minDev = 1f;
    public float maxDev = 1f;
    public bool isTurningToFaceTarget = true;
    public bool isRandomizingStepDistance = true;
    public PathMaximum pathMax = PathMaximum.ATEND;
    public LeanTweenType easingMode;
    public AudioClip[] launchSFX;

    public enum PathMaximum { ATSTART, ATEND, ATMIDDLE }
    public enum SplineDir { POS, MID, NEG }

    private Vector3 startPos, endPos;
    private float yValue;
    private GameObject launchSound;
    protected bool isFirstBendPositive = true;

    protected Transform target;
    protected EnemyAI enemy;
    protected LTDescr tween;
    protected Impact impact;



    public override void Initialize (SPELLDIRECTOR spellDir)
    {
        Transform startTransform = spellDir.CastingHand.Hand.transform;
        startPos = startTransform.position;
        yValue = startPos.y;
        base.Initialize(spellDir, AllowedParents.MAGIC, startPos, startTransform.rotation);
        SetTarget();
        if ( target == null )
            EndEffect();
        else
            FireProjectile(target);
    }

    protected virtual void SetTarget ()
    {
        if ( !target )
            target = FindEnemyTarget();
        if ( target )
        {
            endPos = target.transform.position.Flatten(yValue);
            enemy = target.GetComponent<EnemyAI>();
        }

    }

    protected virtual Transform FindEnemyTarget ()
    {
        float facingAngle = SpellDirector.CastingHand.transform.forward.FacingAngle();
        float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
        float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
        EnemyAI thisEnemy = GAME.Threats.GetClosestEnemy(minAngle, maxAngle, minDist, maxDist);
        if ( thisEnemy )
            return thisEnemy.transform;
        else
            return null;
    }

    protected virtual void FireProjectile (Transform target, bool isFirstBendPositive = true)
    {
        Debug.Log("Target = " + target.ToString());
        if ( enemy )
            enemy.IsImmobile = true;
        launchSound = GAME.Audio.PlaySound(launchSFX.Random(), gameObject);
        float distance = startPos.Distance2D(endPos);
        int numBends = (numDeviations == -1) ? Mathf.RoundToInt(distance / 2) : numDeviations;
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
        Vector3[] thisPath = startPos.TweenPathTo(endPos, deviations.CalcDeviations(minDev, maxDev), isTurningToFaceTarget, isRandomizingStepDistance, isFirstBendPositive);
        tween = LeanTween.moveSpline(gameObject, thisPath, travelTime).setEase(easingMode).setOrientToPath(true);
        tween.setOnComplete(TriggerImpact);
    }

    protected virtual void TriggerImpact ()
    { //(SPELLDIRECTOR spellDir, SPELLEFFECT source, Vector3 targetPosition, Vector3 forceDir)
        if ( enemy )
            enemy.IsImmobile = false;
        LeanTween.cancel(tween.id);
        impact = (Impact) SubEffects[0];
        impact.gameObject.SetActive(true);
        impact.Initialize(SpellDirector, this, target, endPos - startPos);
        EndEffect();
    }

    public override void CancelEffect ()
    {
        GAME.Audio.KillSound(launchSound);
        EndEffect();
    }

}