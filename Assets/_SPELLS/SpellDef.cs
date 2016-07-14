using UnityEngine;

public class SpellDef : MonoBehaviour
{
    public string spellName;
    public float castDurationInBeats;
    public SpellType spellType;
    public float handSpeedWhileCasting = 0f;
    public StartCastFX startCastFXPrefab;
    public AudioClip startCastSFX;

    private SpellEffect spellEffectPrefab, spellEffect;
    private CastHand castingHand;

    [HideInInspector]
    public enum SpellType { TAP, COUNTER, HOLD };

    public CastHand CastingHand { get { return castingHand; } }
    public SpellType TypeOfSpell { get { return spellType; } }

    private PLAYER player;
    private MAGIC magic;

    void Awake ()
    {
        player = GAME.Player;
        magic = GAME.Magic;
    }

    void Start ()
    {
        castingHand = GetComponentInParent<CastHand>();
        spellEffectPrefab = GetComponent<SpellEffect>();
        Invoke("LaunchSpell", castDurationInBeats * GAME.BeatDuration);
    }

    public virtual void LaunchSpell ()
    {
        spellEffect = (new GameObject(spellName + " Effect", spellEffectPrefab.GetType())).GetComponent<SpellEffect>();
        spellEffect.transform.position = transform.position;
        spellEffect.transform.rotation = transform.rotation;
        spellEffect.transform.SetParent(magic.transform, true);
        spellEffect.Initialize(this);
        switch ( spellType )
        {
            case SpellType.TAP:
                castingHand.Status = CastHand.HandState.TAPCASTING;
                break;
            case SpellType.COUNTER:
                castingHand.Status = CastHand.HandState.COUNTERCASTING;
                break;
            case SpellType.HOLD:
                castingHand.Status = CastHand.HandState.HOLDCASTING;
                break;
        }
    }


}
