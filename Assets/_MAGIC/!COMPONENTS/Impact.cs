using UnityEngine;

public class Impact : SPELLEFFECT
{
    public float damage = 25f;
    public float knockback = 100000f;
    public AudioClip[] explosionSFX;

    private ParticleSystem explosionParticle;
    private GameObject explosionSound;
    private Transform target;
    private Vector3 targetPos;
    private EnemyAI enemy;
    private Vector3 forceDirection;
    private float duration;

    //public virtual void Initialize (SPELLDIRECTOR spellDir, SPELLEFFECT source, Vector3 targetPosition, Vector3 forceDir)
    //{
    //    Initialize(spellDir, AllowedParents.MAGIC, source.transform.position, source.transform.rotation);
    //    SetTarget(targetPosition, forceDir);
    //    Detonate();
    //}

    public virtual void Initialize (SPELLDIRECTOR spellDir, SPELLEFFECT source, Transform thisTarget, Vector3 forceDir)
    {
        Initialize(spellDir, AllowedParents.MAGIC, source.transform.position, source.transform.rotation);
        SetTarget(thisTarget, forceDir);
        Detonate();
    }

    public override void Initialize (SPELLDIRECTOR spellDir, AllowedParents parentType, Vector3? altPosition = default(Vector3?), Quaternion? altRotation = default(Quaternion?))
    {
        base.Initialize(spellDir, parentType, altPosition, altRotation);
        explosionParticle = GetComponent<ParticleSystem>();
        explosionSound = GAME.Audio.PlaySound(explosionSFX.Random());
        duration = Mathf.Max(explosionSFX.Length, explosionParticle.duration);
    }

    public virtual void SetTarget (Transform thisTarget, Vector3 forceDir)
    {
        if ( thisTarget == null )
            CancelEffect();
        else
        {
            target = thisTarget;
            targetPos = thisTarget.position;
            forceDirection = forceDir;
            enemy = target.GetComponent<EnemyAI>();
        }
    }

    protected virtual void Detonate ()
    {
        if ( enemy )
        {
            enemy.TakeHit(damage);
            enemy.Knockback(forceDirection * knockback);
        }
        Invoke("EndEffect", duration);
    }

    public override void CancelEffect ()
    {
        CancelInvoke();
        GAME.Audio.KillSound(explosionSound);
        EndEffect();
    }

}
