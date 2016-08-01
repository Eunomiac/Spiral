using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class SPELLEFFECT : MonoBehaviour
{
    // INSPECTOR VARIABLES:
    public bool isDirectable = false;
    public List<CastHand.HandState> HandStates = (new CastHand.HandState[2]).ToList();
    public List<float> HandSpeeds = (new float[2]).ToList();

    // PUBLIC VARIABLES (PROPERTIES):
    protected SPELLDIRECTOR SpellDirector { get; set; }
    protected float StartTime { get; private set; }
    protected CastHand.HandState HandState
    {
        get { return HandStates[0]; }
        set { HandStates[0] = value; }
    }
    protected float HandSpeed
    {
        get { return HandSpeeds[0]; }
        set { HandSpeeds[0] = value; }
    }
    protected List<SPELLEFFECT> SubEffects { get; set; }

    // ENUMS:
    public enum AllowedParents { MAGIC, HAND, ENEMY };

    // TOP-LEVEL REFERENCES:
    protected ARENA arena;
    protected PLAYER player;
    protected MANTLE mantle;
    protected MAGIC magic;

    protected virtual void Awake ()
    {
        arena = GAME.Arena;
        player = GAME.Player;
        mantle = GAME.Mantle;
        magic = GAME.Magic;
        name = GetType().ToString();
    }

    public virtual void Initialize (SPELLDIRECTOR spellDir)
    {
        Initialize(spellDir, AllowedParents.MAGIC);
    }

    public virtual void Initialize (SPELLDIRECTOR spellDir, AllowedParents parentType, Vector3? altPosition = null, Quaternion? altRotation = null)
    {
        Initialize(spellDir, parentType, null, altPosition);
    }

    public virtual void Initialize (SPELLDIRECTOR spellDir, AllowedParents parentType, GameObject parentObject, Vector3? altPosition = null, Quaternion? altRotation = null)
    {
        SpellDirector = spellDir;
        SubEffects = gameObject.GetChildrenOfType<SPELLEFFECT>();
        SetParent(parentType, parentObject, altPosition, altRotation);
        SetCastingHands();
        StartTime = Time.time;
        SpellDirector.EffectStarting(this);
    }

    public virtual void SetParent (AllowedParents parentType = AllowedParents.MAGIC, GameObject parentObject = null, Vector3? altPosition = null, Quaternion? altRotation = null)
    {
        switch ( parentType )
        {
            case AllowedParents.MAGIC:
                transform.SetParent(magic.transform, true);
                break;
            case AllowedParents.HAND:
                transform.SetParent(SpellDirector.CastingHand.Hand.transform, false);
                break;
            case AllowedParents.ENEMY:
                Debug.Assert(parentObject.GetType() == typeof(EnemyAI));
                transform.SetParent(parentObject.transform, true);
                break;
        }
        SetPosition(altPosition, altRotation);
    }

    public virtual void SetPosition (Vector3? newPosition, Quaternion? newRotation)
    {
        if ( newPosition != null )
            transform.position = (Vector3) newPosition;
        if ( newRotation != null )
            transform.rotation = (Quaternion) newRotation;
    }

    public virtual void SetCastingHands ()
    {
        for ( int i = 0; i < SpellDirector.CastingHands.Count; i++ )
        {
            if ( i < HandStates.Count && HandStates[i] >= 0f )
                SpellDirector.CastingHands[i].SetStatus(HandStates[i]);
            if ( i < HandSpeeds.Count && HandSpeeds[i] >= 0f )
                SpellDirector.CastingHands[i].Speed = HandSpeeds[i];
        }
    }

    public virtual void EndHold ()
    {
        EndEffect();
    }

    public abstract void CancelEffect ();

    public virtual void EndEffect ()
    {
        SpellDirector.EffectEnding(this);
        transform.DestroyAllChildren();
    }
}
