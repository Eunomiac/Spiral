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
    private List<string> spellButtons = new List<string>(new string[] { "A", "B", "X", "Y" });

    private ARENA arena;
    private MANTLE mantle;
    private MAGIC magic;

    public NavNetwork NavNetwork { get; set; }
    public CastHand ActiveHand { get; set; }
    public CastHand CurrentHand { get; set; }
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
        ClaimHand();
    }

    void Update ()
    {
        if ( transform.hasChanged )
        {
            NavNetwork.BuildNavNodes(gameObject, nodesPerTier, radiusOfTier);
            transform.hasChanged = false;
        }
        if ( ActiveHand == null && idleHands.Count > 0 )
            ClaimHand();
    }

    // Get an idle hand and set it active for aiming spells with Left Stick.  Returns false if no idle hand available.
    void ClaimHand ()
    {
        CastHand thisHand = idleHands.Pop();
        thisHand.Status = CastHand.HandState.AIMING;
    }

    public void FirstTap (int axis, Vector3? startDirLS)
    {
        if ( ActiveHand != null && spellButtons.Contains(INPUT.ButtonAxes[axis]) )
            ActiveHand.PreCast(mantle.preCastFXPrefab, axis);
    }

    public void MultiTap (int axis, int taps)
    {
        if ( CurrentHand.Status == CastHand.HandState.PRECAST )
        {
            SpellMaster thisSpell = magic.GetTapSpell(axis, taps);
            if ( thisSpell )
                CurrentHand.StartCast(thisSpell);
        }
    }

    public void StartHold (int axis, int taps)
    {
        if ( CurrentHand.Status == CastHand.HandState.PRECAST )
        {
            SpellMaster thisSpell = magic.GetHoldSpell(axis, taps);
            if ( thisSpell )
                CurrentHand.StartCast(thisSpell);
        }
    }

    public void EndHold ()
    {
        Debug.Log("Ending Hold!");
        CurrentHand.Status = CastHand.HandState.ENDHOLDCAST;
    }

    public void TakeHit (float strength)
    {
        //Debug.Log("Took hit of " + strength + " strength!");
    }

}
