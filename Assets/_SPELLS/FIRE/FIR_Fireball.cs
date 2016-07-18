using UnityEngine;

[System.Serializable]
public class FIR_Fireball : SpellMaster
{
    public float minDistance, maxDistance;
    public float growthRate;
    public float minDamage, maxDamage;
    public float knockback;

    public AudioClip travelSFX, explosionSFX;
    public GameObject targetingPrefab, travelPrefab, explosionPrefab;

    private GameObject targeter;

    private float startDist = 0.15f;
    private float endDist = 1f;
    private float curDist = 0.15f;
    private float secsToFull = 3;
    private float scalePerSec;

    private float castDuration;

    //public override void Start ()
    //{
    //    base.Start();
    //    StartCasting(3f);
    //    castingHand.Status = CastHand.HandState.HOLDCASTING;
    //    targeter = Instantiate(targetingPrefab);
    //    targeter.transform.SetParent(castingHand.transform, false);
    //    scalePerSec = (endDist - startDist) / secsToFull;
    //    targeter.transform.localScale = new Vector3(curDist, 1f, curDist);
    //}

    //void Update ()
    //{
    //    if ( castingHand.Status == CastHand.HandState.HOLDCASTING )
    //    {
    //        curDist += scalePerSec * Time.deltaTime;
    //        targeter.transform.localScale = new Vector3(curDist, 1f, curDist);
    //    }
    //    else if ( castingHand.Status == CastHand.HandState.ENDHOLDCAST )
    //        InitializeSpellEffect();
    //}

    //public override void InitializeSpellEffect ()
    //{
    //    base.InitializeSpellEffect();
    //    Destroy(targeter.gameObject);
    //    castingHand.Status = CastHand.HandState.IDLE;
    //    Debug.Log("Firing Fireball!");
    //    Destroy(gameObject);

    //}



}
