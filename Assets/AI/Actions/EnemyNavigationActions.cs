using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

//public override void Start (RAIN.Core.AI ai)
//{
//	base.Start(ai);
//}

//public override ActionResult Execute (RAIN.Core.AI ai)
//{
//	return ActionResult.SUCCESS;
//}

//public override void Stop (RAIN.Core.AI ai)
//{
//	base.Stop(ai);
//}

[RAINAction]
public class PopulateConstants : RAINAction
{
	public override void Start (AI ai)
	{
		ai.WorkingMemory.SetItem("BeatDuration", GAME.BeatDuration);
		ai.WorkingMemory.SetItem("IsAttacking", false);
		ai.WorkingMemory.SetItem("IsWaitingToAttack", false);
		base.Start(ai);
	}

	public override ActionResult Execute (RAIN.Core.AI ai)
	{
		//ai.WorkingMemory.SetItem("BeatDuration", GAME.BeatDuration);
		//ai.WorkingMemory.SetItem("IsAttacking", false);
		return ActionResult.SUCCESS;
	}

	public override void Stop (RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
}

[RAINAction]
public class UpdateVariables : RAINAction
{
	private Node node;

	public override ActionResult Execute (AI ai)
	{
		ai.WorkingMemory.SetItem("MyPosition", ai.Body.transform.position);
		if ( ai.WorkingMemory.GetItem<GameObject>("MyNode") != null)
		{
			node = ai.WorkingMemory.GetItem<GameObject>("MyNode").GetComponent<Node>();
			ai.WorkingMemory.SetItem("CurrentTier", node.Tier);
			ai.WorkingMemory.SetItem("Destination", node.transform.position);
			ai.WorkingMemory.SetItem("DistFromNode", Vector3.Distance(ai.Body.transform.position, node.transform.position));
		}
		return ActionResult.FAILURE;
	}
}

[RAINAction]
public class GetClosestUnoccupiedNode : RAINAction
{
	private EnemyAI enemy;

	public override void Start (AI ai)
	{
		enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
		base.Start(ai);
	}

	public override ActionResult Execute (RAIN.Core.AI ai)
	{
		for (int i = 0; i < 5; i++)
		{
			if ( enemy.ClaimNode(GAME.Player.NavNetwork.GetClosestNode(ai.Body.transform.position)) )
				return ActionResult.SUCCESS;
		}
		return ActionResult.SUCCESS;
	}
}

[RAINAction]
public class RegisterAttack : RAINAction
{
	private EnemyAI enemy;

	public override void Start (AI ai)
	{
		enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
		base.Start(ai);
	}

	public override ActionResult Execute (AI ai)
	{
		if (GAME.Threats.RegisterAttacker(enemy))
		{
			ai.WorkingMemory.SetItem("IsWaitingToAttack", true);
			return ActionResult.SUCCESS;
		}
		else
			return ActionResult.FAILURE;
	}
}

[RAINAction]
public class TryToAdvance : RAINAction
{
	private EnemyAI enemy;

	public override void Start (AI ai)
	{
		enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
		base.Start(ai);
	}

	public override ActionResult Execute (AI ai)
	{
		GameObject node = ai.WorkingMemory.GetItem<GameObject>("MyNode");

		if ( node == null )
			return ActionResult.FAILURE;
		else
		{
			Node myNode = node.GetComponent<Node>();
			List<Node> neighbours = myNode.GetNeighbours(Mathf.Max(0, myNode.Tier - 1), Mathf.Max(0, myNode.Tier - 1));
			Debug.Log("myNode.Tier = " + myNode.Tier + ", Mathf.Max = " + Mathf.Max(0, myNode.Tier - 1) + ", Min = " + Mathf.Max(0, myNode.Tier - 1) + ". Neighbour Count = " + neighbours.Count);
			while (neighbours.Count > 0)
			{
				if ( enemy.ClaimNode(neighbours.Pop(true)) )
					return ActionResult.SUCCESS;
			}
			return ActionResult.FAILURE;
		}
	}
}

[RAINAction]
public class TryToSpread : RAINAction
{
	private EnemyAI enemy;

	public override void Start (AI ai)
	{
		enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
		base.Start(ai);
	}

	public override ActionResult Execute (AI ai)
	{
		GameObject node = ai.WorkingMemory.GetItem<GameObject>("MyNode");

		if ( node == null )
			return ActionResult.FAILURE;
		else
		{
			Node myNode = node.GetComponent<Node>();
			List<Node> neighbours = node.GetComponent<Node>().GetNeighbours(myNode.Tier, myNode.Tier);
			while ( neighbours.Count > 0 )
			{
				if ( enemy.ClaimNode(neighbours.Pop(true)) )
					return ActionResult.SUCCESS;
			}
			return ActionResult.FAILURE;
		}
	}
}