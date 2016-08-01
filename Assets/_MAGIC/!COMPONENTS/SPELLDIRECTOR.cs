using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SPELLDIRECTOR : MonoBehaviour
{
    // INSPECTOR VARIABLES:
    public string SpellName;
    public int SpellTier;
    public List<SpellAttr> SpellAttributes = new List<SpellAttr>();

    // PROPERTIES:
    public SPELLEFFECT CurrentEffect
    {
        get { return currentEffect; }
    }
    private SPELLEFFECT currentEffect;
    public List<CastHand> CastingHands
    {
        get { return castingHands; }
    }
    public CastHand CastingHand
    {
        get { return castingHands.Count > 0 ? castingHands[0] : null; }
        set { castingHands.Add(value); }
    }
    private List<CastHand> castingHands = new List<CastHand>();

    // ENUMS:
    public enum SpellAttr { TAPCAST, HOLDAIM, HOLDCAST, COUNTER, TWOHAND }

    // CLASS VARIABLES:
    private List<SPELLEFFECT> allEffects;
    private List<SPELLEFFECT> activeEffects = new List<SPELLEFFECT>();

    // TOP-LEVEL REFERENCES:
    private ARENA arena;
    private PLAYER player;
    private MANTLE mantle;
    private MAGIC magic;

    protected virtual void Awake ()
    {
        arena = GAME.Arena;
        player = GAME.Player;
        mantle = GAME.Mantle;
        magic = GAME.Magic;
    }

    void Start ()
    {
        Debug.Assert(castingHands.Count > 0);
        name = SpellName;
        transform.SetParent(magic.transform, false);
        allEffects = GetComponentsInChildren<SPELLEFFECT>().ToList();
        foreach ( SPELLEFFECT effect in allEffects )
        {
            effect.gameObject.SetActive(false);
        }
        ActivateNextEffect().Initialize(this);
    }

    public virtual SPELLEFFECT ActivateNextEffect ()
    {
        SPELLEFFECT thisEffect = allEffects[0];
        do
        {
            if ( allEffects.Count == 0 )
            {
                CheckForEnd();
                break;
            }
            thisEffect = allEffects[0];
            allEffects.RemoveAt(0);
        } while ( !thisEffect.isDirectable );
        if ( thisEffect.isDirectable )
            EffectStarting(thisEffect);
        else
            CheckForEnd();
        return CurrentEffect;
    }

    public virtual void EffectStarting (SPELLEFFECT effect)
    {
        effect.gameObject.SetActive(true);
        activeEffects.Push(effect, false);
        currentEffect = effect;
    }

    public virtual void CancelSpell ()
    {
        foreach ( CastHand hand in CastingHands )
            player.SetHandStatus(hand, CastHand.HandState.IDLE);
        allEffects.Clear();
        while ( activeEffects.Count > 0 )
            activeEffects.Pop().CancelEffect();
    }

    public virtual void EffectEnding (SPELLEFFECT effect)
    {
        //Debug.Log("Before " + effect.name + " ends: allEffects = " + allEffects.Count + ", activeEffects = " + activeEffects.Count);
        allEffects.Remove(effect);
        activeEffects.Remove(effect);
        //Debug.Log("After " + effect.name + " ends: allEffects = " + allEffects.Count + ", activeEffects = " + activeEffects.Count);
        CheckForEnd();
    }

    protected virtual void CheckForEnd ()
    {
        if ( allEffects.Count == 0 && activeEffects.Count == 0 )
            transform.DestroyAllChildren();
    }




}

