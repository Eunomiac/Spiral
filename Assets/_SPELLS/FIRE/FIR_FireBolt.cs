using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FIR_FireBolt : SpellDef
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
    private Vector3 handPos, enemyPos;
    private float yValue;
    private int boltsFired = 0;
    //private bool isFinishedFiring = false;

    public override void InitializeSpellEffect ()
    {
        base.InitializeSpellEffect();
        yValue = transform.position.y;
        float facingAngle = transform.forward.FacingAngle();
        float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
        float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
        List<EnemyAI> enemyTargets = GAME.Threats.GetClosestEnemies(minAngle, maxAngle, 5f, GAME.BIGINT, numBolts);
        handPos = transform.position.Flatten(yValue);
        StartCoroutine(FireBolts(enemyTargets, bolts));
    }

    IEnumerator FireBolts (List<EnemyAI> enemies, List<GameObject> bolts)
    {
        foreach ( EnemyAI enemy in enemies )
        {
            SetTarget(enemy);
            yield return new WaitForSeconds(0.3f);
        }
        //isFinishedFiring = true;
    }

    void SetTarget (EnemyAI enemy)
    {
        enemyPos = enemy.transform.position.Flatten(yValue);
        GameObject thisBolt = Instantiate(boltPrefab, handPos, transform.rotation) as GameObject;
        GAME.Audio.PlaySound(boltLaunchSFX.Random(), gameObject);
        boltsFired++;
        bolts.Add(thisBolt);
        thisBolt.transform.SetParent(transform, true);
        Vector3[] thisPath = handPos.TweenPathTo(enemyPos, 1, handPos.Distance2D(enemyPos) * 0.5f, true, boltsFired % 2 == 0);
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
        Destroy(impactExplosion.gameObject, 0.5f);
        enemy.TakeHit(damage);
        Vector3 knockbackVec = (enemyPos - handPos).Normalize2D(yValue) * knockback;
        StartCoroutine(ApplyKnockback(enemy, knockbackVec));
        Destroy(gameObject, 2f);
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
