[System.Serializable]
public class Projectiles : SpellEFF
{










    //private int boltsFired = 0;
    //private int boltsToFire;

    //public override void SetParent (Transform masterTransform)
    //{
    //    base.SetParent(masterTransform);
    //    masterTransform.SetParent(GAME.Magic.transform, true);
    //    //Debug.Log("[SE*] Setting Parent to " + GAME.Magic.transform.name);
    //    transform.SetParent(masterTransform, false);
    //}

    //public override void Initialize ()
    //{
    //    base.Initialize();
    //    //impactPrefab = SpellMaster.GetComponent<Explosion>();
    //    yValue = transform.position.y;
    //    float facingAngle = transform.forward.FacingAngle();
    //    float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
    //    float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
    //    //Debug.Log("Details: " + minAngle + ", " + maxAngle + ", " + minDist + ", " + maxDist + ", " + numBolts);
    //    List<EnemyAI> enemyTargets = GAME.Threats.GetClosestEnemies(minAngle, maxAngle, minDist, maxDist, numBolts);
    //    startPos = transform.position.Flatten(yValue);
    //    boltsToFire = enemyTargets.Count;
    //    StartCoroutine(FireBolts(enemyTargets, bolts));
    //    GAME.Player.SetHandStatus(CastingHand, CastHand.HandState.IDLE);
    //}

    //IEnumerator FireBolts (List<EnemyAI> enemies, List<GameObject> bolts)
    //{
    //    //Debug.Log("Coroutine Running.  Enemies = " + enemies.Count + ", Bolts = " + bolts.Count);
    //    foreach ( EnemyAI enemy in enemies )
    //    {
    //        boltsToFire--;
    //        if ( enemy )
    //            SetTarget(enemy);
    //        yield return new WaitForSeconds(timeBetweenBolts);
    //    }
    //    StartCoroutine(CheckForEnd());
    //}

    //IEnumerator CheckForEnd ()
    //{
    //    while ( true )
    //    {
    //        Debug.Log("bolts.Count = " + bolts.Count + ", BoltChildren = " + transform.GetComponentsInChildren<Projectiles>().Length + ", ExplosionChildren = " + transform.GetComponentsInChildren<Explosion>().Length);
    //        if ( bolts.Count == 0 && boltsToFire == 0 )
    //        {
    //            //Debug.Log("Count: " + bolts.Count + ", Comps: " + transform.GetComponentsInChildren<Explosion>().Length);
    //            End();
    //        }
    //        yield return new WaitForSeconds(timeBetweenBolts);
    //    }
    //}

    //void SetTarget (EnemyAI enemy)
    //{
    //    //Debug.Log("Targeting enemy.");
    //    enemy.IsImmobile = true;
    //    GameObject thisBolt = Instantiate(boltPrefab, startPos, transform.rotation) as GameObject;
    //    GAME.Audio.PlaySound(boltLaunchSFX.Random(), gameObject);
    //    boltsFired++;
    //    bolts.Add(thisBolt);
    //    thisBolt.gameObject.SetParent(gameObject, true);


    //    endPos = enemy.transform.position.Flatten(yValue);
    //    float distance = startPos.Distance2D(endPos);
    //    Debug.Log("Distance = " + distance);
    //    int numBends = (numDeviations == -1) ? Mathf.RoundToInt(distance / 2) : numDeviations;
    //    float[] deviations = new float[numBends];
    //    deviations[0] = 0f;
    //    float devStep = 1f / deviations.Length;
    //    float absDev = devStep;
    //    float prevDev;
    //    for ( int i = 0; i < deviations.Length; i += 2 )
    //    {
    //        prevDev = absDev;
    //        absDev = Mathf.Min(1f, prevDev + devStep * (pathMax == PathMaximum.ATMIDDLE && i > (deviations.Length * 0.5f) ? -1 : 1));
    //        deviations[i] = absDev;
    //        if ( i != (deviations.Length - 1) )
    //            deviations[i + 1] = absDev * -1;
    //    }
    //    if ( pathMax == PathMaximum.ATSTART )
    //        deviations.Reverse();
    //    deviations = deviations.Normalize();
    //    //foreach ( float dev in deviations )
    //    //{
    //    //    Debug.Log(dev);
    //    //}
    //    //float[] newDeviations = deviations.CalcDeviations(1f, 3f);
    //    //foreach ( float dev in newDeviations )
    //    //{
    //    //    Debug.Log(dev);
    //    //}
    //    Vector3[] thisPath = startPos.TweenPathTo(endPos, deviations.CalcDeviations(minDev, maxDev), isTurningToFaceTarget, isRandomizingStepDistance, boltsFired % 2 == 0);
    //    //GAME.DrawIndicators(thisPath);
    //    LTDescr thisTween = LeanTween.moveSpline(thisBolt, thisPath, travelTime).setEase(easingMode).setOrientToPath(true);
    //    thisTween.setOnComplete(TriggerImpact).setOnCompleteParam(new object[] { enemy, thisBolt, thisTween });
    //}

    //void TriggerImpact (object val)
    //{
    //    object[] parameters = (object[]) val;
    //    EnemyAI enemy = (EnemyAI) parameters[0];
    //    GameObject bolt = (GameObject) parameters[1];
    //    LTDescr tween = (LTDescr) parameters[2];
    //    enemy.IsImmobile = false;
    //    bolts.Remove(bolt);
    //    LeanTween.cancel(tween.id);
    //    Explosion thisImpact = Instantiate(SpellMaster.GetComponent<Explosion>());
    //    thisImpact.SetParent(SpellMaster.transform, gameObject, bolt);
    //    Destroy(bolt);
    //    thisImpact.Initialize(enemy.gameObject, endPos - startPos);

    //}

    //protected override void End ()
    //{
    //    //Debug.Log("[SE*] Ending " + name);
    //    //base.End();
    //    //transform.DestroyAllChildren();
    //}

}