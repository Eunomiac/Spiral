using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SPELLS : MonoBehaviour {

	public Spell[] aTapSpells = new Spell[3];
	public Spell[] bTapSpells = new Spell[3];
	public Spell[] xTapSpells = new Spell[3];
	public Spell[] yTapSpells = new Spell[3];
	public Spell[] aContSpells = new Spell[3];
	public Spell[] bContSpells = new Spell[3];
	public Spell[] xContSpells = new Spell[3];
	public Spell[] yContSpells = new Spell[3];

	private CastHand currentHand;
	private int currentAxis;

	private List<Spell> tapSpells = new List<Spell>();
	//private List<Spell> contSpells = new List<Spell>();
	private GameObject currentSpell, preCastFXPrefab, preCastFX;

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

	public void PreCast (CastHand spellHand, int axis, Vector3? startDirLS)
	{
		preCastFX = Instantiate(preCastFXPrefab);
		preCastFX.transform.SetParent(spellHand.Hand.transform, false);
		currentAxis = axis;
		currentHand = spellHand;
	}

	public void StartCast(int taps)
	{
		if ( tapSpells[currentAxis * (taps - 1)] )
		{
			currentSpell = Instantiate(tapSpells[currentAxis * (taps - 1)].gameObject);
			currentSpell.transform.SetParent(currentHand.Hand.transform, false);
			//currentSpell.transform.localPosition = preCastFX.transform.localPosition;
		}
	}

	public void FinishCast()
	{
		player.ReleaseHand(currentHand);
	}

	public void StopCast()
	{
		Destroy(preCastFX.gameObject);
	}

	public void StartHold(CastHand spellHand, int taps)
	{

	}

	public void EndHold(CastHand spellHand)
	{

	}

}
