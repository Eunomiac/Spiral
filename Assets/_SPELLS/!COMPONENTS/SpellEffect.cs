using UnityEngine;

// Superclass for any effect that can be instantiated and parented as expected by spell definition.
[System.Serializable]
public class SpellEffect : MonoBehaviour
{
    public CastHand.HandState castHandState;
    public float handSpeedWhileCasting = 0f;

    public CastHand CastingHand { get; set; }
    public SpellMaster SpellMaster { get; set; }

    private GameObject effectObject;

    public virtual void SetParent (Transform spellMasterTransform)
    {
        SpellMaster = spellMasterTransform.gameObject.GetComponent<SpellMaster>();
    }

    public virtual void Initialize ()
    {
        CastingHand = SpellMaster.CastingHand;
        CastingHand.ConfirmStatus(castHandState);
        //name = debugName;
        //Debug.Log("[SE] Initializing " + GetType() + " at " + Mathf.RoundToInt(Time.time));
    }

    protected virtual void End ()
    {
        SpellMaster.NextComponent(transform);
    }

}
