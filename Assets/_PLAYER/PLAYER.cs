using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PLAYER : MonoBehaviour
{
    public int[] nodesPerTier = new int[3] { 8, 16, 32 };
    public float[] radiusOfTier = new float[3] { 3f, 6f, 9f };
    public float maxNeighbourDistMult = 1.5f;
    public float armLength = 2f;

    private List<CastHand> idleHands = new List<CastHand>();
    private List<CastHand> busyHands = new List<CastHand>();
    private List<string> spellButtons = new List<string>(new string[] { "A", "B", "X", "Y" });

    private ARENA arena;
    private INPUT input;
    //private MANTLE mantle;
    private SPELLS spells;

    public NavNetwork NavNetwork { get; set; }
    public CastHand ActiveHand { get; set; }
    public CastHand CurrentHand { get; set; }

    void Awake ()
    {
        arena = GAME.Arena;
        input = GAME.Input;
        //mantle = GAME.Mantle;
        spells = GAME.Spells;
    }

    void Start ()
    {
        NavNetwork = arena.InitializeNavNetwork(gameObject, nodesPerTier, radiusOfTier, maxNeighbourDistMult);
        idleHands = GetComponentsInChildren<CastHand>().ToList();
        foreach ( CastHand hand in idleHands )
            hand.FadeHand(true);
    }

    void Update ()
    {
        if ( transform.hasChanged )
        {
            NavNetwork.BuildNavNodes(gameObject, nodesPerTier, radiusOfTier);
            transform.hasChanged = false;
        }
        AimHand();
    }

    bool AimHand ()
    {
        if ( !ActiveHand && !ClaimHand() )
            return false;
        if ( (input.LSVector == null) == ActiveHand.gameObject.activeSelf )
            ActiveHand.gameObject.SetActive(input.LSVector != null);
        if ( input.LSVector != null )
            ActiveHand.transform.localRotation = Quaternion.Euler(0, (float) input.LSAngle, 0);
        return true;
    }

    // Get an idle hand and set it active for aiming spells with Left Stick.  Returns false if no idle hand available.
    bool ClaimHand ()
    {
        if ( idleHands.Count == 0 )
            return false;
        ActiveHand = idleHands.Pop();
        ActiveHand.FadeHand(false);
        return true;
    }

    // Lock the active hand after it begins casting a spell.  Returns false if there is no active hand.
    public bool LockHand ()
    {
        if ( ActiveHand == null )
            return false;
        busyHands.Push(ActiveHand);
        ActiveHand = null;
        return true;
    }

    // Release the given hand, returning it to the idle hands pool.  Returns false if given hand is not in the busy pool.
    public bool ReleaseHand (CastHand hand)
    {
        if ( busyHands.Remove(hand) )
        {
            idleHands.Push(hand);
            hand.FadeHand(true);
            return true;
        }
        return false;
    }

    public void FirstTap (int axis, Vector3? startDirLS)
    {
        if ( ActiveHand != null && spellButtons.Contains(INPUT.ButtonAxes[axis]) )
            spells.PreCast(axis, startDirLS);

    }

    public void MultiTap (int axis, int taps)
    {
        if ( spells.isPreCasting )
            spells.StartCast(taps);
    }

    public void StartHold (int axis, int taps)
    {
        spells.StartHold(CurrentHand, taps);
    }

    public void EndHold ()
    {
        spells.EndHold(CurrentHand);
        CurrentHand = null;
    }

    public void TakeHit (float strength)
    {
        Debug.Log("Took hit of " + strength + " strength!");
    }

}
