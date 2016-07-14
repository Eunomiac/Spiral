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

    private List<SpellDef> allSpells = new List<SpellDef>();

    private MANTLE mantle;
    private PLAYER player;

    void Awake ()
    {
        mantle = GAME.Mantle;
        player = GAME.Player;
        allSpells = aTapSpells.Concat(bTapSpells).Concat(xTapSpells).Concat(yTapSpells).Concat(aContSpells).Concat(bContSpells).Concat(xContSpells).Concat(yContSpells).ToList();
    }

    public SpellDef GetTapSpell (int axis, int taps)
    {
        return allSpells[GAME.maxTaps * axis + taps - 1];
    }

    public SpellDef GetHoldSpell (int axis, int taps)
    {
        return allSpells[4 * GAME.maxTaps * axis + taps - 1];
    }

}
