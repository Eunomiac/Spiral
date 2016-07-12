using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SPELLS : MonoBehaviour
{

    public GameObject[] aTapSpells = new GameObject[3];
    public GameObject[] bTapSpells = new GameObject[3];
    public GameObject[] xTapSpells = new GameObject[3];
    public GameObject[] yTapSpells = new GameObject[3];
    public GameObject[] aContSpells = new GameObject[3];
    public GameObject[] bContSpells = new GameObject[3];
    public GameObject[] xContSpells = new GameObject[3];
    public GameObject[] yContSpells = new GameObject[3];

    private List<GameObject> allSpells = new List<GameObject>();
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
        allSpells = aTapSpells.Concat(bTapSpells).Concat(xTapSpells).Concat(yTapSpells).Concat(aContSpells).Concat(bContSpells).Concat(xContSpells).Concat(yContSpells).ToList();
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
        if ( allSpells[3 * preCastHand.ButtonAxis + taps - 1] )
        {
            GameObject spell = Instantiate(allSpells[3 * preCastHand.ButtonAxis + taps - 1].gameObject);
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
        Debug.Log("FailCast_NoSpell");
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
