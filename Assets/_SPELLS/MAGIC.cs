using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MAGIC : MonoBehaviour
{

    public SpellDef[] aTapSpells = new SpellDef[GAME.maxTaps];
    public SpellDef[] bTapSpells = new SpellDef[GAME.maxTaps];
    public SpellDef[] xTapSpells = new SpellDef[GAME.maxTaps];
    public SpellDef[] yTapSpells = new SpellDef[GAME.maxTaps];
    public SpellDef[] aContSpells = new SpellDef[GAME.maxTaps];
    public SpellDef[] bContSpells = new SpellDef[GAME.maxTaps];
    public SpellDef[] xContSpells = new SpellDef[GAME.maxTaps];
    public SpellDef[] yContSpells = new SpellDef[GAME.maxTaps];

    [HideInInspector]
    public enum FailCondition { NOSUCHSPELL };

    private List<SpellDef> allSpells = new List<SpellDef>();

    //private MANTLE mantle;
    private PLAYER player;

    void Awake ()
    {
        //mantle = GAME.Mantle;
        player = GAME.Player;
        allSpells = aTapSpells.Concat(bTapSpells).Concat(xTapSpells).Concat(yTapSpells).Concat(aContSpells).Concat(bContSpells).Concat(xContSpells).Concat(yContSpells).ToList();
    }

    public SpellDef GetTapSpell (int axis, int taps)
    {
        SpellDef thisSpell = allSpells[GAME.maxTaps * axis + taps - 1];
        if ( thisSpell == null )
            FailCast(player.CurrentHand, FailCondition.NOSUCHSPELL);
        return thisSpell;
    }

    public SpellDef GetHoldSpell (int axis, int taps)
    {
        Debug.Log("HoldSpell = " + (GAME.maxTaps * (axis + 4) + taps - 1));
        SpellDef thisSpell = allSpells[GAME.maxTaps * (axis + 4) + taps - 1];
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
