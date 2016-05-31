using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class THREATS : MonoBehaviour
{

	public bool isShowingDestination = false;
	private List<EnemyAI> registeredAttackers = new List<EnemyAI>();

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
}
