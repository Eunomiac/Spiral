using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MAGIC : MonoBehaviour
{

    public SpellMaster[] aTapSpells = new SpellMaster[GAME.MaxTaps];
    public SpellMaster[] bTapSpells = new SpellMaster[GAME.MaxTaps];
    public SpellMaster[] xTapSpells = new SpellMaster[GAME.MaxTaps];
    public SpellMaster[] yTapSpells = new SpellMaster[GAME.MaxTaps];
    public SpellMaster[] aContSpells = new SpellMaster[GAME.MaxTaps];
    public SpellMaster[] bContSpells = new SpellMaster[GAME.MaxTaps];
    public SpellMaster[] xContSpells = new SpellMaster[GAME.MaxTaps];
    public SpellMaster[] yContSpells = new SpellMaster[GAME.MaxTaps];

    [HideInInspector]
    public enum FailCondition { NOSUCHSPELL };

    private List<SpellMaster> allSpells = new List<SpellMaster>();

    //private MANTLE mantle;
    private PLAYER player;

    void Awake ()
    {
        //mantle = GAME.Mantle;
        player = GAME.Player;
        allSpells = aTapSpells.Concat(bTapSpells).Concat(xTapSpells).Concat(yTapSpells).Concat(aContSpells).Concat(bContSpells).Concat(xContSpells).Concat(yContSpells).ToList();
    }

    public SpellMaster GetTapSpell (int axis, int taps)
    {
        SpellMaster thisSpell = allSpells[GAME.MaxTaps * axis + taps - 1];
        if ( thisSpell == null )
            FailCast(player.CurrentHand, FailCondition.NOSUCHSPELL);
        return thisSpell;
    }

    public SpellMaster GetHoldSpell (int axis, int taps)
    {
        Debug.Log("HoldSpell = " + (GAME.MaxTaps * (axis + 4) + taps - 1));
        SpellMaster thisSpell = allSpells[GAME.MaxTaps * (axis + 4) + taps - 1];
        if ( thisSpell == null )
            FailCast(player.CurrentHand, FailCondition.NOSUCHSPELL);
        return thisSpell;
    }

    public void FailCast (CastHand hand, FailCondition reason)
    {
        if ( reason == FailCondition.NOSUCHSPELL )
            Debug.Log("Failed Cast with " + hand.name + " hand: NO SUCH SPELL");
        hand.Status = CastHand.HandState.IDLE;
    }
}
