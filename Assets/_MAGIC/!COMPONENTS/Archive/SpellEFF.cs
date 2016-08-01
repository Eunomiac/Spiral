using UnityEngine;

// Superclass for any effect that can be instantiated and parented as expected by spell definition.
[System.Serializable]
public class SpellEFF : MonoBehaviour
{
    //public CastHand.HandState castHandState;
    //public float handSpeedWhileCasting = 0f;
    //public float durationOverride = 0f;

    //public CastHand CastingHand { get; set; }
    //public SpellMaster SpellMaster { get; set; }

    //private float startTime;

    //public virtual void Initialize (SpellMaster spellMaster, bool isDetaching = false)
    //{
    //    SpellMaster = spellMaster;
    //    transform.SetParent(SpellMaster.transform, false);
    //    SpellMaster.CastingHand.ConfirmStatus(castHandState);
    //    startTime = Time.time;
    //    if ( isDetaching )
    //        SpellMaster.transform.SetParent(GAME.Magic.transform, true);
    //}

    //public virtual void SetPosition (Transform newTransform)
    //{
    //    transform.position = newTransform.position;
    //    transform.rotation = newTransform.rotation;
    //}

    //protected virtual void End ()
    //{
    //    SpellMaster.NextEffect();
    //}
}
