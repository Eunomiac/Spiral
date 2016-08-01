using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PLAYER : MonoBehaviour
{
    public int[] nodesPerTier = new int[3] { 8, 16, 32 };
    public float[] radiusOfTier = new float[3] { 3f, 6f, 9f };
    public float maxNeighbourDistMult = 1.5f;
    public float armLength = 2f;
    public float aimingHandSpeed = 12f;
    public CastHand activeHand, currentHand;

    private List<CastHand> idleHands = new List<CastHand>();
    private List<string> spellButtons = new List<string>(new string[] { "A", "B", "X", "Y" });

    private ARENA arena;
    private MANTLE mantle;
    private MAGIC magic;

    public NavNetwork NavNetwork { get; set; }
    public CastHand ActiveHand { get { return activeHand; } set { activeHand = value; } }
    public CastHand CurrentHand { get { return currentHand; } set { currentHand = value; } }
    public List<CastHand> IdleHands { get { return idleHands; } }

    void Awake ()
    {
        arena = GAME.Arena;
        mantle = GAME.Mantle;
        magic = GAME.Magic;
    }

    void Start ()
    {
        NavNetwork = arena.InitializeNavNetwork(gameObject, nodesPerTier, radiusOfTier, maxNeighbourDistMult);
        idleHands = GetComponentsInChildren<CastHand>().ToList();
    }

    void Update ()
    {
        if ( transform.hasChanged )
        {
            NavNetwork.BuildNavNodes(gameObject, nodesPerTier, radiusOfTier);
            transform.hasChanged = false;
        }
        if ( ActiveHand == null && idleHands.Count > 0 )
            SetHandStatus(idleHands.Pop(), CastHand.HandState.AIMING);
    }

    public void SetHandStatus (CastHand hand, CastHand.HandState status)
    {
        if ( hand.Status != status )
        {
            switch ( status )
            {
                case CastHand.HandState.AIMING:
                    ActiveHand = hand;
                    break;
                case CastHand.HandState.IDLE:
                    IdleHands.Push(hand);
                    ActiveHand = ActiveHand == hand ? null : ActiveHand;
                    CurrentHand = CurrentHand == hand ? null : CurrentHand;
                    break;
                case CastHand.HandState.PRECAST:
                    CurrentHand = ActiveHand;
                    ActiveHand = null;
                    break;
            }
            hand.SetStatus(status);
        }
    }

    public void FirstTap (int axis, Vector3? startDirLS)
    {
        if ( INPUT.ButtonAxes[axis] == "LT" ) { mantle.UpdateSpirrus(-1); return; }
        else if ( INPUT.ButtonAxes[axis] == "RT" ) { mantle.UpdateSpirrus(+1); return; }

        if ( ActiveHand != null && spellButtons.Contains(INPUT.ButtonAxes[axis]) )
        {
            magic.PreCast(ActiveHand);
            SetHandStatus(ActiveHand, CastHand.HandState.PRECAST);
        }
    }

    public void MultiTap (int axis, int taps)
    {
        if ( INPUT.ButtonAxes[axis] == "LT" || INPUT.ButtonAxes[axis] == "RT" ) return;
        magic.StartCast(magic.GetTapSpell(axis, taps));
    }

    public void StartHold (int axis, int taps)
    {
        magic.StartCast(magic.GetHoldSpell(axis, taps));
    }

    public void EndHold ()
    {
        magic.EndHold();
    }


    public void TakeHit (float strength)
    {
        //Debug.Log("Took hit of " + strength + " strength!");
    }

}
