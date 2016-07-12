using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SPELLS : MonoBehaviour
{

    public Spell[] aTapSpells = new Spell[3];
    public Spell[] bTapSpells = new Spell[3];
    public Spell[] xTapSpells = new Spell[3];
    public Spell[] yTapSpells = new Spell[3];
    public Spell[] aContSpells = new Spell[3];
    public Spell[] bContSpells = new Spell[3];
    public Spell[] xContSpells = new Spell[3];
    public Spell[] yContSpells = new Spell[3];

    private List<Spell> tapSpells = new List<Spell>();
    //private List<Spell> contSpells = new List<Spell>();
    private GameObject preCastFXPrefab;
    private CastHand preCastHand;

    public bool isPreCasting = false;

    private MANTLE mantle;
    private PLAYER player;

    void Awake ()
    {
        mantle = GAME.Mantle;
        player = GAME.Player;
        preCastFXPrefab = mantle.preCastFXPrefab;
        tapSpells = aTapSpells.Concat(bTapSpells).Concat(xTapSpells).Concat(yTapSpells).ToList();
        //contSpells = aContSpells.Concat(bContSpells).Concat(xContSpells).Concat(yContSpells).ToList();
    }

    public void PreCast (int axis, Vector3? startDirLS)
    {
        isPreCasting = true;
        player.ActiveHand.PreCastFX = Instantiate(preCastFXPrefab);
        player.ActiveHand.PreCastFX.transform.SetParent(player.ActiveHand.Hand.transform, false);
        player.ActiveHand.ButtonAxis = axis;
        player.ActiveHand.StartDir = startDirLS;
        preCastHand = player.ActiveHand;
        player.LockHand();
    }

    public void StartCast (int taps)
    {
        isPreCasting = false;
        if ( tapSpells[preCastHand.ButtonAxis * (taps - 1)] )
        {
            GameObject spell = Instantiate(tapSpells[preCastHand.ButtonAxis * (taps - 1)].gameObject);
            spell.transform.SetParent(preCastHand.Hand.transform, false);
        }
        else
        {
            FailCast_NoSpell(preCastHand);
        }
        preCastHand = null;
    }

    public void FailCast_NoSpell (CastHand hand)
    {
        player.ReleaseHand(hand);
    }

    public void FinishCast (CastHand hand)
    {
        player.ReleaseHand(hand);
    }

    public void StartHold (CastHand spellHand, int taps)
    {

    }

    public void EndHold (CastHand spellHand)
    {

    }

}
