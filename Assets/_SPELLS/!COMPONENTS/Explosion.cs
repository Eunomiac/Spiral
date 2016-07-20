using System.Collections;
using UnityEngine;

[System.Serializable]
public class Explosion : SpellEffect
{
    public float damage = 25f;
    public float knockback = 100000f;
    public AudioClip[] explosionSFX;
    public GameObject explosionPrefab;


    private EnemyAI targetEnemy;

    public void SetParent (Transform masterTransform, GameObject parent, GameObject impactPosition)
    {
        base.SetParent(masterTransform);
        //Debug.Log("[SE*] Setting Parent to " + GAME.Magic.transform.name);
        gameObject.SetParent(parent, true, impactPosition.gameObject);
    }

    public void Initialize (GameObject target, Vector3 forceDirection)
    {
        base.Initialize();
        GAME.Audio.PlaySound(explosionSFX.Random(), gameObject);
        GameObject thisExplosion = Instantiate(explosionPrefab) as GameObject;
        thisExplosion.gameObject.SetParent(gameObject, false);
        EnemyAI targetEnemy = target.GetComponent<EnemyAI>();
        if ( targetEnemy )
        {
            targetEnemy.TakeHit(damage);
            StartCoroutine(ApplyKnockback(targetEnemy, forceDirection * knockback));
        }
        else
        {
            End();
        }
    }

    IEnumerator ApplyKnockback (EnemyAI enemy, Vector3 knockbackVec)
    {
        for ( int i = 0; i < 20; i++ )
        {
            enemy.Knockback(knockbackVec * (1 - 0.05f * i));
            yield return new WaitForFixedUpdate();
        }
        End();
    }
}
