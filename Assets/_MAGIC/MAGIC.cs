using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MAGIC : MonoBehaviour
{
    #region Spell Library & Spell Lookup Methods
    public SPELLDIRECTOR[] aTapSpells = new SPELLDIRECTOR[GAME.MaxTaps];
    public SPELLDIRECTOR[] bTapSpells = new SPELLDIRECTOR[GAME.MaxTaps];
    public SPELLDIRECTOR[] xTapSpells = new SPELLDIRECTOR[GAME.MaxTaps];
    public SPELLDIRECTOR[] yTapSpells = new SPELLDIRECTOR[GAME.MaxTaps];
    public SPELLDIRECTOR[] aContSpells = new SPELLDIRECTOR[GAME.MaxTaps];
    public SPELLDIRECTOR[] bContSpells = new SPELLDIRECTOR[GAME.MaxTaps];
    public SPELLDIRECTOR[] xContSpells = new SPELLDIRECTOR[GAME.MaxTaps];
    public SPELLDIRECTOR[] yContSpells = new SPELLDIRECTOR[GAME.MaxTaps];

    private List<SPELLDIRECTOR> allSpells = new List<SPELLDIRECTOR>();

    public SPELLDIRECTOR GetTapSpell (int axis, int taps)
    {
        return allSpells[GAME.MaxTaps * axis + taps - 1];
    }

    public SPELLDIRECTOR GetHoldSpell (int axis, int taps)
    {
        return allSpells[GAME.MaxTaps * (axis + 4) + taps - 1];
    }
    #endregion

    #region Spell Casting
    [HideInInspector]
    public enum FailCondition { NOSUCHSPELL, SPELLCANCELLED };
    private CastHand preCastHand;
    private GameObject preCastPrefab;
    private GameObject preCastEffect;
    private SPELLDIRECTOR holdingSpell;

    public void PreCast (CastHand hand)
    {
        preCastHand = hand;
        preCastEffect = Instantiate(preCastPrefab);
        preCastEffect.transform.SetParent(preCastHand.Hand.transform, false);
    }

    public void StartCast (SPELLDIRECTOR spellDef)
    {
        System.Diagnostics.Trace.Assert(preCastHand.Status == CastHand.HandState.PRECAST);
        Destroy(preCastEffect);
        if ( spellDef == null )
            FailCast(preCastHand, FailCondition.NOSUCHSPELL);
        else
        {
            SPELLDIRECTOR thisSpell = Instantiate(spellDef);
            thisSpell.CastingHand = preCastHand;
            if ( spellDef.SpellAttributes.Contains(SPELLDIRECTOR.SpellAttr.HOLDAIM) || spellDef.SpellAttributes.Contains(SPELLDIRECTOR.SpellAttr.HOLDCAST) )
                holdingSpell = thisSpell;
            else
                holdingSpell = null;
        }
        preCastHand = null;
        preCastEffect = null;
    }

    public void EndHold ()
    {
        if ( holdingSpell != null )
        {
            holdingSpell.CurrentEffect.EndHold();
            holdingSpell = null;
        }
    }

    public void FailCast (CastHand hand, FailCondition reason)
    {
        switch ( reason )
        {
            case FailCondition.NOSUCHSPELL:
                Debug.Log("Failed Cast with " + hand.name + " hand: NO SUCH SPELL");
                break;
            case FailCondition.SPELLCANCELLED:
                Debug.Log("Failed Cast with " + hand.name + " hand: SPELL CANCELLED");
                break;
        }
        player.SetHandStatus(hand, CastHand.HandState.IDLE);
    }

    #endregion

    private PLAYER player;
    private MANTLE mantle;

    void Awake ()
    {
        player = GAME.Player;
        mantle = GAME.Mantle;
        allSpells = aTapSpells.Concat(bTapSpells).Concat(xTapSpells).Concat(yTapSpells).Concat(aContSpells).Concat(bContSpells).Concat(xContSpells).Concat(yContSpells).ToList();
    }

    void Start ()
    {
        preCastPrefab = mantle.preCastFXPrefab;
    }








}
