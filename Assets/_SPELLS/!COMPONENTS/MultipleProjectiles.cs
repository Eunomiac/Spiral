using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultipleProjectiles : SpellEffect
{

    // SEPARATE THIS INTO A "Projectiles" CLASS & A "Impact" CLASS (which is instantiated by Projectiles).
    public int numBolts = 1;
    public float searchAngle = 60f;
    public float damage = 25f;
    public float knockback = 100000f;
    public float minDist = 0f;
    public float maxDist = GAME.BIGINT;
    public int numSwerves = 1;
    public AudioClip[] boltLaunchSFX, boltImpactSFX;
    public GameObject boltPrefab, impactPrefab;

    private List<GameObject> bolts = new List<GameObject>();
    private Vector3 startPos, endPos;
    private float yValue;
    private int boltsFired = 0;

    public override void SetParent (Transform masterTransform)
    {
        base.SetParent(masterTransform);
        //Debug.Log("[SE*] Setting Parent to " + GAME.Magic.transform.name);
        transform.SetParent(GAME.Magic.transform, true);
        transform.position = masterTransform.position;
        transform.rotation = masterTransform.rotation;
    }

    public override void Initialize ()
    {
        base.Initialize();
        yValue = transform.position.y;
        float facingAngle = transform.forward.FacingAngle();
        float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
        float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
        //Debug.Log("Details: " + minAngle + ", " + maxAngle + ", " + minDist + ", " + maxDist + ", " + numBolts);
        List<EnemyAI> enemyTargets = GAME.Threats.GetClosestEnemies(minAngle, maxAngle, minDist, maxDist, numBolts);
        startPos = transform.position.Flatten(yValue);
        StartCoroutine(FireBolts(enemyTargets, bolts));
        CastingHand.Status = CastHand.HandState.TAPCASTING;
        Invoke("End", 2f);
    }

    IEnumerator FireBolts (List<EnemyAI> enemies, List<GameObject> bolts)
    {
        //Debug.Log("Coroutine Running.  Enemies = " + enemies.Count + ", Bolts = " + bolts.Count);
        foreach ( EnemyAI enemy in enemies )
        {
            SetTarget(enemy);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void SetTarget (EnemyAI enemy)
    {
        //Debug.Log("Targeting enemy.");
        endPos = enemy.transform.position.Flatten(yValue);
        GameObject thisBolt = Instantiate(boltPrefab, startPos, transform.rotation) as GameObject;
        GAME.Audio.PlaySound(boltLaunchSFX.Random(), gameObject);
        boltsFired++;
        bolts.Add(thisBolt);
        thisBolt.transform.SetParent(transform, true);
        Vector3[] thisPath = startPos.TweenPathTo(endPos, numSwerves, startPos.Distance2D(endPos) * 0.5f, true, boltsFired % 2 == 0);
        //GAME.DrawIndicators(thisPath, "Path " + boltsFired);
        LTDescr thisTween = LeanTween.moveSpline(thisBolt, thisPath, 0.5f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true);
        thisTween.setOnComplete(TriggerImpact).setOnCompleteParam(new object[] { enemy, thisBolt, thisTween });
    }

    void TriggerImpact (object val)
    {
        object[] parameters = (object[]) val;
        EnemyAI enemy = (EnemyAI) parameters[0];
        GameObject bolt = (GameObject) parameters[1];
        LTDescr tween = (LTDescr) parameters[2];
        GAME.Audio.PlaySound(boltImpactSFX.Random(), gameObject);
        LeanTween.cancel(tween.id);
        bolts.Remove(bolt);
        Destroy(bolt);
        GameObject impactExplosion = Instantiate(impactPrefab, bolt.transform.position, bolt.transform.rotation) as GameObject;
        impactExplosion.transform.SetParent(transform, true);
        enemy.TakeHit(damage);
        Vector3 knockbackVec = (endPos - startPos).Normalize2D(yValue) * knockback;
        StartCoroutine(ApplyKnockback(enemy, knockbackVec));
    }

    IEnumerator ApplyKnockback (EnemyAI enemy, Vector3 knockbackVec)
    {
        for ( int i = 0; i < 20; i++ )
        {
            enemy.Knockback(knockbackVec * (1 - 0.05f * i));
            yield return new WaitForFixedUpdate();
        }
    }

    protected override void End ()
    {
        //Debug.Log("[SE*] Ending " + name);
        base.End();
    }

}