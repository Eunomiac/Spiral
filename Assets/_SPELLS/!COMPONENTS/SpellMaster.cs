using UnityEngine;

[System.Serializable]
public class SpellMaster : MonoBehaviour
{
    public string SpellName;

    public SpellEffect CurrentEffect { get; set; }
    public CastHand CastingHand { get; set; }
    public float HandSpeed { get { return CurrentEffect.handSpeedWhileCasting; } }

    private int currentEffectIndex = -1;
    private SpellEffect[] spellEffects;
    private GameObject[] spellEffectObjects;

    public virtual void Initialize ()
    {
        name = SpellName;
        CastingHand = GetComponentInParent<CastHand>();
        spellEffects = CastingHand.SpellPrefab.GetComponents<SpellEffect>();
        //string dbString = "[SM] " + spellEffects.Length.ToString() + "x Effects: ";
        //foreach ( SpellEffect effect in spellEffects )
        //{
        //    //SpellEffect thisEffect = Instantiate(effect);
        //    //thisEffect.name = effect.GetType().ToString();
        //    dbString += effect.GetType() + ". ";
        //    //foreach ( SpellEffect comp in thisEffect.GetComponents<SpellEffect>() )
        //    //{
        //    //    if ( comp.GetType() != effect.GetType() )
        //    //    {
        //    //        Destroy(comp);
        //    //    }
        //    //}
        //}
        //Debug.Log(dbString);
        NextComponent();
    }


    public virtual void NextComponent (Transform prevTransform = null)
    {
        if ( prevTransform )
            prevTransform.DestroyAllChildren();
        //Debug.Log("Destroyed!");
        currentEffectIndex++;
        //string dbString = "[SM] " + spellEffects.Length.ToString() + "x Effects: ";
        //foreach ( SpellEffect effect in spellEffects )
        //{
        //    dbString += effect.GetType() + ". ";
        //}
        //Debug.Log(dbString);
        if ( currentEffectIndex < spellEffects.Length )
        {
            //Debug.Log("[SM] NEXT COMPONENT: Current Effect Index = " + currentEffectIndex + " SpellEffects = " + spellEffects[currentEffectIndex].ToString());
            //System.Type thisType = spellEffects[currentEffectIndex].GetType();
            //GameObject thisEffect = new GameObject(thisType.ToString(), thisType);
            CurrentEffect = Instantiate(spellEffects[currentEffectIndex]);
            //CurrentEffect = thisEffect.GetComponent<SpellEffect>();
            CurrentEffect.name = CurrentEffect.GetType().ToString();
            //foreach ( SpellEffect comp in CurrentEffect.GetComponents<SpellEffect>() )
            //    if ( comp.GetType() != CurrentEffect.GetType() )
            //        Destroy(comp);
            //Debug.Log("[SM] On Index " + currentEffectIndex + ": " + CurrentEffect.GetType() + " to " + transform.name);
            CurrentEffect.SetParent(transform);
            CurrentEffect.Initialize();
        }
        else
            End();
    }

    public virtual void End ()
    {
        //Debug.Log("ENDING!");
        if ( gameObject )
            transform.DestroyAllChildren();
    }


}
