using System.Collections.Generic;
using UnityEngine;

public class MultiProjectile : Projectile
{
    public int numProjectiles = 3;
    public float timeBetweenBolts = 0.3f;

    protected List<Transform> targets = new List<Transform>();
    protected bool isMaster = true;
    protected float waitTime = 0f;

    void Initialize ()
    {
        Initialize(SpellDirector);
    }

    public virtual void Initialize (SPELLDIRECTOR spellDir, Transform thisTarget, bool isFirstBendPos, float delay = 0f)
    {
        Debug.Log("PROJECTILE: " + thisTarget.name + ", " + isFirstBendPos);
        target = thisTarget;
        isFirstBendPositive = isFirstBendPos;
        SpellDirector = spellDir;
        Invoke("Initialize", delay);
    }

    protected override void SetTarget ()
    {
        if ( isMaster )
        {
            targets = FindEnemyTargets(numProjectiles);
            if ( targets.Count == 0 )
                EndEffect();
            else
            {
                while ( targets.Count > 1 )
                {
                    waitTime += timeBetweenBolts;
                    Transform thisTarget = targets.Pop();
                    MultiProjectile thisProjectile = Instantiate(this);
                    thisProjectile.isMaster = false;
                    thisProjectile.Initialize(SpellDirector, thisTarget, targets.Count % 2 == 0, waitTime);
                }
                target = targets[0];
            }
        }
        base.SetTarget();
    }

    public virtual List<Transform> FindEnemyTargets (int numTargets)
    {
        float facingAngle = SpellDirector.CastingHand.transform.forward.FacingAngle();
        float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
        float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
        List<EnemyAI> enemyTargets = GAME.Threats.GetClosestEnemies(minAngle, maxAngle, minDist, maxDist, numTargets);
        Debug.Log("Targets = " + enemyTargets.Count);
        List<Transform> targetTransforms = new List<Transform>();
        foreach ( EnemyAI enemy in enemyTargets )
            targetTransforms.Add(enemy.transform);
        return targetTransforms;
    }
}
