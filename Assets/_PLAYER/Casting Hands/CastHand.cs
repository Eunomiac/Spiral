using UnityEngine;

public class CastHand : MonoBehaviour
{
    private GameObject hand;
    private SpriteRenderer handSprite;
    private HandState status;
    private Rotator rotator;
    private float targetAngle;
    private SpellDef spellDef;

    private INPUT input;
    private MANTLE mantle;
    private PLAYER player;

    public GameObject Hand { get { return hand; } }
    public GameObject PreCastFX { get; set; }
    public GameObject StartCastFX { get; set; }
    public GameObject SpellDef { get; set; }
    public int? ButtonAxis { get; set; }
    public SpellDef SpellDefPrefab
    {
        get { return spellDef; }
        set {
            if ( Status == HandState.PRECAST )
            {
                spellDef = value;
                Status = HandState.STARTCAST;
            }
            else if ( value == null )
                spellDef = value;
            else
                Debug.LogError("Attempt to Register Spell Definition with non-PreCasting Hand");
        }
    }

    public enum HandState { AIMING, IDLE, FAILCAST, PRECAST, STARTCAST, TAPCASTING, HOLDCASTING, ENDHOLDCAST, COUNTERCASTING, DUALCASTING, UPCASTING };

    public HandState Status
    {
        get { return status; }
        set {
            status = value;
            //Debug.Log(name + ": " + value.ToString());
            switch ( status )
            {
                case HandState.AIMING:
                    player.ActiveHand = this;
                    FadeHand(false);
                    break;
                case HandState.IDLE:
                    player.IdleHands.Push(this);
                    FadeHand(true);
                    if ( player.ActiveHand == this )
                        player.ActiveHand = null;
                    if ( player.CurrentHand == this )
                        player.CurrentHand = null;
                    ButtonAxis = null;
                    SpellDefPrefab = null;
                    if ( PreCastFX )
                        Destroy(PreCastFX.gameObject);
                    if ( StartCastFX )
                        Destroy(StartCastFX.gameObject);
                    if ( SpellDef )
                        Destroy(SpellDef.gameObject);
                    PreCastFX = null;
                    StartCastFX = null;
                    SpellDef = null;
                    break;
                case HandState.FAILCAST:
                    Debug.Log("Casting FAILED: " + name + " Hand");
                    Status = HandState.IDLE;
                    break;
                case HandState.PRECAST:
                    targetAngle = rotator.TargetAngle;
                    PreCastFX = Instantiate(mantle.preCastFXPrefab);
                    PreCastFX.transform.SetParent(Hand.transform, false);
                    player.CurrentHand = this;
                    player.ActiveHand = null;
                    break;
                case HandState.STARTCAST:
                    Destroy(PreCastFX.gameObject);
                    if ( SpellDefPrefab == null )
                        Status = HandState.FAILCAST;
                    else
                    {
                        SpellDef spell = Instantiate(SpellDefPrefab);
                        spell.transform.SetParent(Hand.transform, false);
                        StartCastFX = Instantiate(spell.startCastFXPrefab.gameObject);
                        StartCastFX.transform.SetParent(spell.transform, false);
                        //Debug.Log("From Casting Hand, SCS = " + spell.StartCastSound.ToString());
                        GameObject startSound = GAME.Audio.PlaySound(spell.startCastSFX);
                        GAME.Audio.VolumeTween(startSound, spell.castDurationInBeats * GAME.BeatDuration * 2f);
                        SpellDef = spell.gameObject;
                    }
                    break;
                case HandState.TAPCASTING:
                    Status = HandState.IDLE;
                    break;
                case HandState.HOLDCASTING:
                    Destroy(StartCastFX);
                    break;
                case HandState.ENDHOLDCAST:
                    Status = HandState.IDLE;
                    break;
                case HandState.COUNTERCASTING:
                    Destroy(StartCastFX);
                    break;
                case HandState.DUALCASTING:
                    Destroy(StartCastFX);
                    break;
                case HandState.UPCASTING:
                    Destroy(StartCastFX);
                    break;
                default:
                    break;
            }
        }
    }

    void Awake ()
    {
        hand = transform.GetChild(0).gameObject;
        handSprite = hand.GetComponentInChildren<SpriteRenderer>();
        input = GAME.Input;
        mantle = GAME.Mantle;
        player = GAME.Player;
        rotator = GetComponent<Rotator>();
    }

    void Start ()
    {
        Status = HandState.IDLE;
    }

    void Update ()
    {
        float? inputAngle = input.LSAngle;
        switch ( Status )
        {
            case HandState.AIMING:
                if ( inputAngle == null )
                    AimHandUp();
                else
                    rotator.rotate((float) inputAngle);
                break;
            case HandState.IDLE:
                if ( inputAngle != null )
                    rotator.rotate((float) inputAngle, 3f);
                break;
            case HandState.PRECAST:
                //rotator.rotate(targetAngle);
                break;
            case HandState.HOLDCASTING:
                if ( inputAngle != null )
                    rotator.rotate((float) inputAngle, SpellDefPrefab.handSpeedWhileCasting);
                break;
            default:
                break;
        }
    }

    void AimHandUp ()
    {
        // Behaviour where AIMING hand is not being directed by stick.
    }

    public void FadeHand (bool isFadeOut)
    {
        Color thisColor = handSprite.color;
        thisColor.a = isFadeOut ? 0.3f : 1f;
        handSprite.color = thisColor;
    }

}
