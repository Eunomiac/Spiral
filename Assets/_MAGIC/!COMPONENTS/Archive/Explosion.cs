[System.Serializable]
public class Explosion : SpellEFF
{
    //public float damage = 25f;
    //public float knockback = 100000f;
    //public AudioClip[] explosionSFX;
    //public GameObject explosionPrefab;
    //public ParticleSystem explosionParticle;

    //private float testTime = 0f;

    //private EnemyAI targetEnemy;

    //public void SetParent (Transform masterTransform, GameObject parent, GameObject impactPosition)
    //{
    //    base.SetParent(masterTransform);
    //    //Debug.Log("[SE*] Setting Parent to " + GAME.Magic.transform.name);
    //    gameObject.SetParent(parent, true, impactPosition.gameObject);
    //}

    //public void Initialize (GameObject target, Vector3 forceDirection)
    //{
    //    base.Initialize();
    //    AudioClip thisClip = explosionSFX.Random();

    //    GAME.Audio.PlaySound(thisClip, gameObject);
    //    GameObject thisExplosion = Instantiate(explosionPrefab) as GameObject;
    //    explosionParticle = thisExplosion.GetComponent<ParticleSystem>();
    //    thisExplosion.gameObject.SetParent(gameObject, false);
    //    EnemyAI targetEnemy = target.GetComponent<EnemyAI>();
    //    if ( targetEnemy )
    //    {
    //        targetEnemy.TakeHit(damage);
    //        targetEnemy.Knockback(forceDirection * knockback);
    //    }
    //    Debug.Log("Ending " + name + " in Max(" + thisClip.length + ", " + explosionParticle.duration + ")");
    //    testTime = Time.time;
    //    Invoke("End", Mathf.Max(thisClip.length, explosionParticle.duration));
    //}

    //protected override void End ()
    //{
    //    Debug.Log("Ended " + name + " in " + (Time.time - testTime) + "s");
    //    transform.DestroyAllChildren();
    //}

}
