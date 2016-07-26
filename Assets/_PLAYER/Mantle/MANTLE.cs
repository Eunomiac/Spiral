using UnityEngine;

public class MANTLE : MonoBehaviour
{
    public GameObject preCastFXPrefab;
    public GAME.Element element;
    public int numTiers;
    public int[] spirrusPerTier;

    public int Tier { get; set; }
    public int Spirrus { get; set; }

    private int maxSpirrus = 0;

    public GameObject[] tempTierSprites;

    private GameObject spirrusCount;



    void Awake ()
    {
        Spirrus = 0;
        Tier = -1;
        foreach ( int spirrus in spirrusPerTier )
            maxSpirrus += spirrus;

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
            spirrusCount = ChangeSpirrusCountImage();
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


    protected virtual GameObject ChangeSpirrusCountImage ()
    {
        if ( spirrusCount )
        {
            //Debug.Log("Destroying Spirrus Count!");
            spirrusCount.transform.DestroyAllChildren(true);
        }
        GameObject thisSpirrusCount = SpriteToText.ParsePhrase(Spirrus.ToString(), Color.red);
        thisSpirrusCount.transform.SetParent(transform, false);
        thisSpirrusCount.transform.localPosition = new Vector3(-1.5f, 1f, -1.5f);
        return thisSpirrusCount;
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
