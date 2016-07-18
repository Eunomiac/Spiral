using UnityEngine;

public class CastHand : MonoBehaviour
{
    private GameObject hand;
    private SpriteRenderer handSprite;
    private HandState status;
    private Rotator rotator;

    private INPUT input;
    private PLAYER player;

    public GameObject Hand { get { return hand; } }
    public GameObject PreCastFX { get; set; }
    public GameObject StartCastFX { get; set; }
    public GameObject SpellDef { get; set; }
    public int? ButtonAxis { get; set; }
    public SpellDef SpellDefPrefab { get; set; }

    public enum HandState { AIMING, IDLE, PRECAST, STARTCAST, TAPCASTING, HOLDCASTING, ENDHOLDCAST, COUNTERCASTING, DUALCASTING, UPCASTING };

    public HandState Status
    {
        get { return status; }
        set {
            status = value;
            Debug.Log(name + " = " + value.ToString());
            switch ( status )
            {
                case HandState.AIMING:
                    player.ActiveHand = this;
                    FadeHand(false);
                    break;
                case HandState.IDLE:
                    player.IdleHands.Push(this);
                    player.ActiveHand = player.ActiveHand == this ? null : player.ActiveHand;
                    player.CurrentHand = player.ActiveHand == this ? null : player.CurrentHand;
                    FadeHand(true);
                    ClearSpells();
                    break;
                case HandState.PRECAST:
                    player.CurrentHand = this;
                    player.ActiveHand = null;
                    break;
                case HandState.STARTCAST:
                    break;
                case HandState.TAPCASTING:
                    Status = HandState.IDLE;
                    break;
                case HandState.HOLDCASTING:
                    break;
                case HandState.ENDHOLDCAST:
                    break;
                case HandState.COUNTERCASTING:
                    break;
                case HandState.DUALCASTING:
                    break;
                case HandState.UPCASTING:
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

    public void PreCast (GameObject preCastFX, int buttonAxis)
    {
        ButtonAxis = buttonAxis;
        PreCastFX = Instantiate(preCastFX);
        PreCastFX.transform.SetParent(Hand.transform, false);
        Status = HandState.PRECAST;
    }

    public void StartCast (SpellDef spellDefinition)
    {
        System.Diagnostics.Trace.Assert(Status == HandState.PRECAST);
        Destroy(PreCastFX.gameObject);
        SpellDefPrefab = spellDefinition;
        //Status = HandState.STARTCAST;
        SpellDef = Instantiate(SpellDefPrefab).gameObject;
        SpellDef.transform.SetParent(Hand.transform, false);
    }

    public void ClearSpells ()
    {
        ButtonAxis = null;
        SpellDefPrefab = null;
        if ( PreCastFX )
            Destroy(PreCastFX.gameObject);
        if ( StartCastFX )
            Destroy(StartCastFX.gameObject);
        PreCastFX = null;
        StartCastFX = null;
    }
}
