using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIR_FireBolt : MonoBehaviour
{

    public int numBolts = 1;
    public float searchAngle = 60f;
    public GameObject boltPrefab;

    void Start ()
    {
        float facingAngle = transform.forward.FacingAngle();
        float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
        float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
        List<EnemyAI> enemyTargets = GAME.Threats.GetClosestEnemies(minAngle, maxAngle, numBolts);
        foreach ( EnemyAI enemy in enemyTargets )
        {
            FIR_FireBolt1_Bolt thisBolt = Instantiate(boltPrefab).GetComponent<FIR_FireBolt1_Bolt>();
            thisBolt.transform.SetParent(transform, false);
            thisBolt.setTarget(enemy);
        }
        StartCoroutine(DestroyCheck());
    }

    IEnumerator DestroyCheck ()
    {
        if ( GetComponentsInChildren<FIR_FireBolt1_Bolt>().Length == 0 )
            Destroy(gameObject);
        yield return new WaitForSeconds(0.5f);
    }

}
