using UnityEngine;

// Superclass for any effect that can be instantiated and parented as expected by spell definition.
[System.Serializable]
public class SpellEffect : MonoBehaviour
{
    public CastHand.HandState castHandState;
    public float handSpeedWhileCasting = 0f;
    public float durationOverride = 0f;

    public CastHand CastingHand { get; set; }
    public SpellMaster SpellMaster { get; set; }

    private GameObject effectObject;
    private float startTime;

    public virtual void SetParent (Transform spellMasterTransform)
    {
        SpellMaster = spellMasterTransform.gameObject.GetComponent<SpellMaster>();
    }

    public virtual void Initialize ()
    {
        CastingHand = SpellMaster.CastingHand;
        CastingHand.ConfirmStatus(castHandState);
        startTime = Time.time;
        //name = debugName;
        //Debug.Log("[SE] Initializing " + GetType() + " at " + Mathf.RoundToInt(Time.time));
    }

    protected virtual void End ()
    {
        if ( Time.time - startTime < durationOverride )
        {
            Invoke("SpellMasterNext", durationOverride - (Time.time - startTime));
        }
        else
            SpellMaster.NextComponent(transform);
    }

    protected void SpellMasterNext ()
    {
        SpellMaster.NextComponent(transform);
    }
}
