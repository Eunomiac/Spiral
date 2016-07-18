using UnityEngine;

[System.Serializable]
public class SpellDef : MonoBehaviour
{
    public string spellName;
    public float castDurationInBeats;
    public SpellType spellType;
    public float handSpeedWhileCasting = 0f;
    public StartCastFX startCastFXPrefab;
    public AudioClip startCastSFX;

    protected StartCastFX startCastFX;
    protected CastHand castingHand;

    [HideInInspector]
    public enum SpellType { TAP, COUNTER, HOLD };

    //private PLAYER player;
    private MAGIC magic;

    public virtual void Awake ()
    {
        //player = GAME.Player;
        magic = GAME.Magic;
    }

    public virtual void Start ()
    {
        castingHand = GetComponentInParent<CastHand>();
        if ( castDurationInBeats > 0f && startCastFXPrefab != null )
        {
            Debug.Log("castDurationInBeats = " + castDurationInBeats.ToString());
            StartCasting(castDurationInBeats * GAME.BeatDuration * 2f);
            Invoke("InitializeSpellEffect", castDurationInBeats * GAME.BeatDuration);
        }
    }

    public virtual void StartCasting (float duration)
    {
        castingHand.Status = CastHand.HandState.STARTCAST;
        startCastFX = Instantiate(startCastFXPrefab);
        startCastFX.transform.SetParent(transform, false);
        GameObject startSound = GAME.Audio.PlaySound(startCastSFX);
        GAME.Audio.VolumeTween(startSound, duration);
    }

    public virtual void InitializeSpellEffect ()
    {
        System.Diagnostics.Trace.Assert(castingHand.Status == CastHand.HandState.STARTCAST);
        Destroy(startCastFX.gameObject);
        transform.SetParent(magic.transform, true);
        //switch ( spellType )
        //{
        //    case SpellType.TAP:
        //        castingHand.Status = CastHand.HandState.TAPCASTING;
        //        break;
        //    case SpellType.COUNTER:
        //        castingHand.Status = CastHand.HandState.COUNTERCASTING;
        //        break;
        //    case SpellType.HOLD:
        //        castingHand.Status = CastHand.HandState.HOLDCASTING;
        //        break;
        //}
    }

}
