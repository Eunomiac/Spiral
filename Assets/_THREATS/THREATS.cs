using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class THREATS : MonoBehaviour
{

    public bool isShowingDestination = false;
    private List<EnemyAI> registeredAttackers = new List<EnemyAI>();

    private PLAYER player;

    void Awake ()
    {
        player = GAME.Player;
    }
    void Start ()
    {
        StartCoroutine(EnemyManagement());
    }

    IEnumerator EnemyManagement ()
    {
        while ( true )
        {
            TriggerAttack();
            yield return new WaitForSeconds(GAME.BeatDuration * 2);
        }
    }

    public bool RegisterAttacker (EnemyAI enemy)
    {
        return registeredAttackers.Push(enemy);
    }

    void TriggerAttack ()
    {
        int attackerCount = 0;
        while ( registeredAttackers.Count > 0 && attackerCount < GAME.MaxSimultaneousAttacks )
        {
            EnemyAI thisAttacker = registeredAttackers.Pop();
            thisAttacker.Attack();
            attackerCount++;
        }
    }

    public List<EnemyAI> GetEnemies (float minAngle = 0f, float maxAngle = 360f, bool isDistSorting = true)
    {
        List<EnemyAI> theseEnemies = GetComponentsInChildren<EnemyAI>().ToList();
        if ( minAngle != 0f || maxAngle != 360f )
        {
            EnemyAI[] enemyArray = new EnemyAI[theseEnemies.Count];
            theseEnemies.CopyTo(enemyArray);
            foreach ( EnemyAI enemy in enemyArray )
                if ( !enemy.transform.position.FacingAngle().IsBetween(minAngle, maxAngle) )
                    theseEnemies.Remove(enemy);
        }
        Vector3 playerPos = player.transform.position;
        if ( isDistSorting )
            theseEnemies.Sort((x, y) => x.transform.position.Distance2D(playerPos).CompareTo(y.transform.position.Distance2D(playerPos)));
        return theseEnemies;
    }

    public List<EnemyAI> GetClosestEnemies (float minAngle = 0f, float maxAngle = 360f, int numEnemies = 0)
    {
        List<EnemyAI> theseEnemies = GetEnemies(minAngle, maxAngle);
        if ( numEnemies == 0 || numEnemies >= theseEnemies.Count )
            return theseEnemies;
        else
            return theseEnemies.GetRange(0, numEnemies);
    }
}
