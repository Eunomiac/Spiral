﻿using UnityEngine;

public class Spell_Hold : MonoBehaviour
{

    public string spellName;
    public float castDurationInBeats;
    public bool isContinuous = false;
    public GameObject startCastFXPrefab, spellEffectPrefab;
    private GameObject startCastFX, spellEffect;
    private CastHand castingHand;

    private PLAYER player;
    private SPELLS spells;


    void Awake ()
    {
        player = GAME.Player;
        spells = GAME.Spells;
    }

    void Start ()
    {
        castingHand = GetComponentInParent<CastHand>();
        startCastFX = Instantiate(startCastFXPrefab);
        startCastFX.transform.SetParent(transform, false);
        Invoke("LaunchSpell", castDurationInBeats * GAME.BeatDuration);
    }

    void LaunchSpell ()
    {
        spellEffect = Instantiate(spellEffectPrefab, startCastFX.transform.position, startCastFX.transform.rotation) as GameObject;
        spellEffect.transform.SetParent(spells.transform, true);
        if ( !isContinuous )
            player.ReleaseHand(castingHand);
        Destroy(castingHand.PreCastFX);
        Destroy(startCastFX);
        Destroy(gameObject);
    }
}
