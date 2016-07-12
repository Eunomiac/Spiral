using UnityEngine;

public class Spell_Counter : MonoBehaviour
{

    public string spellName;
    public float castDurationInBeats;
    public GameObject startCastFXPrefab, spellEffectPrefab;
    private GameObject startCastFX, spellEffect;
    private CastHand castingHand;

    private PLAYER player;
    private SPELLS spells;


    void Awake ()
    {
        player = GAME.Player;
        spells = GAME.Spells;
    }

    void Start ()
    {
        castingHand = GetComponentInParent<CastHand>();
        startCastFX = Instantiate(startCastFXPrefab);
        startCastFX.transform.SetParent(transform, false);
        Invoke("LaunchSpell", castDurationInBeats * GAME.BeatDuration);
    }

    void LaunchSpell ()
    {
        // 1) Determine if number of taps equals number of attacking enemies.
        // 2) Determine enemy targets & positions.
        // 3) Interrupt LeanTween of Casting Hand, set points to tween through starting with closest enemy
        // 4) As hand reaches each enemy, launch spell effect and continue on
        spellEffect = Instantiate(spellEffectPrefab, startCastFX.transform.position, startCastFX.transform.rotation) as GameObject;
        spellEffect.transform.SetParent(spells.transform, true);
        player.ReleaseHand(castingHand);
        Destroy(castingHand.PreCastFX);
        Destroy(startCastFX);
        Destroy(gameObject);
    }
}
