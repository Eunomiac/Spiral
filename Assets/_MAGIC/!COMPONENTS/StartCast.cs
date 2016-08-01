using UnityEngine;

public class StartCast : SPELLEFFECT
{
    public AudioClip StartCastSound;
    public float Duration;

    private GameObject startSound;

    public override void Initialize (SPELLDIRECTOR spellDir)
    {
        base.Initialize(spellDir, AllowedParents.HAND);
        startSound = GAME.Audio.PlaySound(StartCastSound);
        GAME.Audio.VolumeTween(startSound, Duration);
        Invoke("EndEffect", Duration);
    }

    public override void CancelEffect ()
    {
        CancelInvoke();
        GAME.Audio.KillSound(startSound);
        base.EndEffect();
    }

    public override void EndEffect ()
    {
        SpellDirector.ActivateNextEffect().Initialize(SpellDirector);
        base.EndEffect();
    }
}
