using UnityEngine;

public class StartCast : SpellEffect
{
    public GameObject StartCastFXPrefab;
    public AudioClip StartCastSound;
    public float Duration;

    public override void SetParent (Transform masterTransform)
    {
        base.SetParent(masterTransform);
        //Debug.Log("[SE*] Setting Parent to " + masterTransform.name);
        transform.SetParent(masterTransform, false);
    }

    public override void Initialize ()
    {
        base.Initialize();
        GameObject startCastObject = Instantiate(StartCastFXPrefab);
        startCastObject.transform.SetParent(transform, false);
        //Debug.Log("[SE*] Setting EffectObject of " + name + ": Name = " + startCastObject.name + ", Parent = " + transform.name);
        GameObject startSound = GAME.Audio.PlaySound(StartCastSound);
        GAME.Audio.VolumeTween(startSound, Duration);
        Invoke("End", Duration);
    }

    protected override void End ()
    {
        //Debug.Log("[SE*] Ending " + name);
        base.End();
    }

}
