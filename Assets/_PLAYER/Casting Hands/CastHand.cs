using UnityEngine;

public class CastHand : MonoBehaviour
{
    private GameObject hand;
    private SpriteRenderer handSprite;
    private Rotator rotator;
    public HandState status;

    private INPUT input;
    private PLAYER player;

    public GameObject Hand { get { return hand; } }
    public HandState Status { get { return status; } protected set { status = value; } }
    public float Speed { get; set; }

    public enum HandState { AIMING, IDLE, PRECAST, STARTCAST, HOLDING, COUNTERING, DUALCASTING, UPCASTING };

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
        SetStatus(HandState.IDLE);
    }

    void Update ()
    {
        float? inputAngle = input.LSAngle;
        switch ( Status )
        {
            case HandState.AIMING:
            case HandState.IDLE:
            case HandState.HOLDING:
                if ( inputAngle == null && Status == HandState.AIMING )
                    AimHandUp();
                else if ( inputAngle != null )
                    rotator.rotate((float) inputAngle, Speed);
                break;
        }
    }

    public void SetStatus (HandState newState)
    {
        //Debug.Log(name + " Hand Set From " + Status + " to " + newState);
        Status = newState;
        switch ( Status )
        {
            case HandState.AIMING:
                FadeHand(false);
                Speed = player.aimingHandSpeed;
                break;
            case HandState.IDLE:
                FadeHand(true);
                Speed = player.aimingHandSpeed * 0.3f;
                break;
        }
    }

    void AimHandUp () { } // Behaviour where AIMING hand is not being directed by stick.

    public void FadeHand (bool isFadeOut)
    {
        Color thisColor = handSprite.color;
        thisColor.a = isFadeOut ? 0.3f : 1f;
        handSprite.color = thisColor;
    }
}
