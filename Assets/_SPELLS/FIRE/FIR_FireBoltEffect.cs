using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIR_FireBoltEffect : SpellEffect
{
    public int numBolts = 1;
    public float searchAngle = 60f;
    public float damage = 25f;
    public float knockback = 1000f;
    public AudioClip[] boltLaunchSFX = new AudioClip[5];
    public AudioClip[] boltImpactSFX;
    public GameObject boltPrefab;
    public GameObject impactPrefab;

    private List<GameObject> bolts = new List<GameObject>();
    private Vector3 playerPos, handPos, enemyPos;
    private Vector3 handToEnemy, handToEnemyNorm, playerToEnemy, playerToEnemyNorm;
    private float yValue;
    private bool isFinishedFiring = false;

    public override void Initialize (SpellDef spell)
    {
        base.Initialize(spell);
        FIR_FireBoltEffect template = spell.gameObject.GetComponent<SpellEffect>() as FIR_FireBoltEffect;
        numBolts = template.numBolts;
        searchAngle = template.searchAngle;
        damage = template.damage;
        knockback = template.knockback;
        boltLaunchSFX = template.boltLaunchSFX;
        boltImpactSFX = template.boltImpactSFX;
        boltPrefab = template.boltPrefab;
        impactPrefab = template.impactPrefab;
        yValue = transform.position.y;
        float facingAngle = transform.forward.FacingAngle();
        float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
        float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
        List<EnemyAI> enemyTargets = GAME.Threats.GetClosestEnemies(minAngle, maxAngle, numBolts);
        playerPos = GAME.Player.transform.position.Flatten(yValue);
        handPos = transform.position.Flatten(yValue);
        StartCoroutine(FireBolts(enemyTargets, bolts));
    }

    IEnumerator FireBolts (List<EnemyAI> enemies, List<GameObject> bolts)
    {
        foreach ( EnemyAI enemy in enemies )
        {
            SetTarget(enemy);
            yield return new WaitForSeconds(0.1f);
        }
        isFinishedFiring = true;
    }

    void SetTarget (EnemyAI enemy)
    {
        enemyPos = enemy.transform.position.Flatten(yValue);
        handToEnemy = enemyPos - handPos;
        handToEnemyNorm = handToEnemy.Normalize2D(yValue);
        playerToEnemy = enemyPos - playerPos;
        playerToEnemyNorm = playerToEnemy.Normalize2D(yValue);
        GameObject thisBolt = Instantiate(boltPrefab, transform.position.Flatten(yValue), transform.rotation) as GameObject;
        audioPlayer.PlaySound(boltLaunchSFX.Random(), gameObject);
        bolts.Add(thisBolt);
        thisBolt.transform.SetParent(transform, true);
        Vector3[] thisPath = new Vector3[] { playerPos, handPos, GetSplineMidpoint(enemy), enemyPos, enemyPos - handToEnemyNorm * 0.25f, enemyPos + handToEnemyNorm * 0.25f };
        LTDescr thisTween = LeanTween.moveSpline(thisBolt, thisPath, 0.5f).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true);
        thisTween.setOnComplete(TriggerImpact).setOnCompleteParam(new object[] { enemy, thisBolt, thisTween });
        //StartCoroutine(UpdateEndpoint(enemy, thisTween));
    }

    Vector3 GetSplineMidpoint (EnemyAI enemy)
    {
        float halfDistance = 0.5f * handToEnemy.magnitude;
        Vector3 midPoint = handToEnemyNorm * halfDistance;
        float length = halfDistance * (numBolts - bolts.Count) / numBolts * (bolts.Count % 2 == 1 ? -1 : 1);
        Vector3 orthoVec = handPos.Normal2D(midPoint, length) + midPoint + handPos;
        return orthoVec.Flatten(yValue);
    }

    void TriggerImpact (object val)
    {
        object[] parameters = (object[]) val;
        EnemyAI enemy = (EnemyAI) parameters[0];
        GameObject bolt = (GameObject) parameters[1];
        LTDescr tween = (LTDescr) parameters[2];
        audioPlayer.PlaySound(boltImpactSFX.Random(), gameObject);
        LeanTween.cancel(tween.id);
        GameObject impactExplosion = Instantiate(impactPrefab, bolt.transform.position, bolt.transform.rotation) as GameObject;
        bolts.Remove(bolt);
        Destroy(bolt);
        impactExplosion.transform.SetParent(transform, true);
        Destroy(impactExplosion.gameObject, 0.5f);
        Vector3 knockbackVec = playerToEnemyNorm * knockback;
        Destroy(gameObject, 2f);
        enemy.TakeHit(damage);
        StartCoroutine(ApplyKnockback(enemy, knockbackVec));
    }

    IEnumerator UpdateEndpoint (EnemyAI enemy, LTDescr tween)
    {
        while ( true )
        {
            enemyPos = enemy.transform.position.Flatten(yValue);
            tween.spline.pts[3] = enemyPos;
            tween.spline.pts[4] = enemyPos - handToEnemyNorm * 0.25f;
            tween.spline.pts[5] = enemyPos + handToEnemyNorm * 0.25f;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ApplyKnockback (EnemyAI enemy, Vector3 knockbackVec)
    {
        for ( int i = 0; i < 20; i++ )
        {
            enemy.Knockback(knockbackVec * (1 - 0.05f * i));
            yield return new WaitForFixedUpdate();
        }
    }

}
