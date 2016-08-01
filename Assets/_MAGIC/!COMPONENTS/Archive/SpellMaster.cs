using UnityEngine;

[System.Serializable]
public class SpellMaster : MonoBehaviour
{
    //public string SpellName;

    //public SpellEFF CurrentEffect { get; set; }
    //public CastHand CastingHand { get; set; }
    //public float HandSpeed { get { return CurrentEffect.handSpeedWhileCasting; } }

    //private int currentEffectIndex = 0;
    //private SpellEFF[] spellEffects;

    //public virtual void Initialize ()
    //{
    //    name = SpellName;
    //    CastingHand = GetComponentInParent<CastHand>();
    //    spellEffects = GetComponents<SpellEFF>();
    //    NextEffect();
    //}


    //public virtual void NextEffect ()
    //{
    //    if ( CurrentEffect )
    //        CurrentEffect.transform.DestroyAllChildren();
    //    if ( currentEffectIndex <= spellEffects.Length )
    //    {
    //        CurrentEffect = Instantiate(spellEffects[currentEffectIndex]);
    //        CurrentEffect.name = CurrentEffect.GetType().ToString();
    //        currentEffectIndex++;
    //        CurrentEffect.Initialize(this, false);
    //    }
    //    else
    //        End();
    //}

    //public virtual void End ()
    //{
    //    transform.DestroyAllChildren();
    //}


}
