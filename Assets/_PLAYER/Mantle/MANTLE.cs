using UnityEngine;
using UnityEngine.UI;

public class MANTLE : MonoBehaviour
{
    public GameObject preCastFXPrefab;
    public GAME.Element element;
    public int numTiers;
    public int[] spirrusPerTier;
    public AudioClip cancelSpellSound;

    public int Tier { get; set; }
    public int Spirrus { get; set; }

    private int maxSpirrus = 0;

    public GameObject[] tempTierSprites;

    private Text spirrusDisplay;

    void Awake ()
    {
        Spirrus = 0;
        Tier = -1;
        foreach ( int spirrus in spirrusPerTier )
            maxSpirrus += spirrus;
        foreach ( Transform child in transform )
            spirrusDisplay = child.GetComponentInChildren<Text>() ?? spirrusDisplay;

    }

    void Start ()
    {
        UpdateSpirrus(0);
    }

    public virtual void UpdateSpirrus (int change)
    {
        Spirrus += change;
        if ( Spirrus < 0 )
            TriggerBurnout();
        else if ( Spirrus > maxSpirrus )
            TriggerOverload();
        else
        {
            spirrusDisplay.text = Spirrus.ToString();
            GetTierFromSpirrus();
        }
    }

    public virtual void TakeHit (int strength)
    {

    }

    protected virtual void GetTierFromSpirrus ()
    {
        int newSpirrus = 0;
        for ( int i = 0; i < spirrusPerTier.Length; i++ )
        {
            newSpirrus += spirrusPerTier[i];
            if ( Spirrus <= newSpirrus )
            {
                if ( i != Tier )
                    ChangeTier(Tier, i);
                return;
            }
        }
        Debug.LogError("Update Tier ran out of Tiers!");
    }

    protected virtual void ChangeTier (int fromTier, int toTier)
    {
        Tier = toTier;
        if ( fromTier >= 0 && tempTierSprites[fromTier] )
            tempTierSprites[fromTier].SetActive(false);
        tempTierSprites[toTier].SetActive(true);
    }

    protected virtual void TriggerOverload ()
    {
        Debug.Log("OVERLOAD!");
    }

    protected virtual void TriggerBurnout ()
    {
        Debug.Log("BURNOUT!");
    }





}
