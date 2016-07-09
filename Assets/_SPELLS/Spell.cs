using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	public string spellName;
	public float castDuration;
	public bool isContinuous = false;
	public GameObject startCastFXPrefab, spellEffectPrefab;
	private GameObject startCastFX, spellEffect;

	private PLAYER player;
	private SPELLS spells;


	void Awake()
	{
		player = GAME.Player;
		spells = GAME.Spells;
	}

	void Start ()
	{
		startCastFX = Instantiate(startCastFXPrefab);
		//startCastFX = Instantiate(startCastFXPrefab, transform.position, Quaternion.identity) as GameObject;
		startCastFX.transform.SetParent(transform, false);
		Invoke("LaunchSpell", castDuration);
	}
	
	void LaunchSpell ()
	{
		spells.StopCast();
		spellEffect = Instantiate(spellEffectPrefab, startCastFX.transform.position, startCastFX.transform.rotation) as GameObject;
		Destroy(startCastFX);
		spellEffect.transform.SetParent(spells.transform, true);
		if ( !isContinuous )
			player.ReleaseHand(GetComponentInParent<CastHand>());
		Destroy(gameObject);
	}
}
