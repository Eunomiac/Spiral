using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Waypoints;
using RAIN.Memory;

public class EnemyAI : MonoBehaviour {

	/* USING RAIN AI FOR NAVIGATION
	 * - 
	 * - 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * */
	public float strength = 1f;

	private RAINMemory memory;
	private Node myNode;
	private bool isAttacking;

	private ARENA arena;
	private PLAYER player;

	
	public Node MyNode {
		get { return myNode; }
		set {
			if (myNode != null)
				myNode.Unclaim();
			myNode = value;
			memory.SetItem<GameObject>("MyNode", value.gameObject);
			if ( myNode != null )
			{
				memory.SetItem("Destination", myNode.transform.position);
				memory.SetItem("CurrentTier", myNode.Tier);
			}
			else
			{
				memory.SetItem<Vector3?>("Destination", null);
				memory.SetItem<int?>("CurrentTier", null);
			}
		}
	}

	public bool IsAttacking
	{
		get { return isAttacking; }
		set {
			isAttacking = value;
			memory.SetItem("IsAttacking", value);
			if (isAttacking)
				memory.SetItem("IsWaitingToAttack", false);
		}
	}

	void Awake()
	{
		arena = GAME.Arena;
		player = GAME.Player;
		memory = GetComponentInChildren<AIRig>().AI.WorkingMemory;
	}

	public bool ClaimNode (Node node)
	{
		if (node.Claim(gameObject))
		{
			MyNode = node;
			return true;
		} else
			return false;
	}

	void UnclaimNode ()
	{
		MyNode = null;
	}

	public void Attack ()
	{
		MyNode = player.NavNetwork.CenterNode;
		IsAttacking = true;
		
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.GetComponent<PLAYER>())
		{
			player.TakeHit(strength);
			IsAttacking = false;
			float myAngle = player.NavNetwork.GetAngleFromPosition(transform.position);
			int tier = player.nodesPerTier.Length;
			Node targetNode = null;
			while ( targetNode == null && tier > 0 )
			{
				float angleSpread = 30f;
				while ( targetNode == null && angleSpread <= 90f )
				{
					targetNode = arena.GetRandomNode((myAngle - angleSpread).Clamp(), (myAngle + angleSpread).Clamp(), tier, true);
					angleSpread += 30f;
				}
				tier -= 1;
			}
			if ( targetNode == null )
			{
				Debug.Log("No unoccupied node to retreat to!");
				Destroy(gameObject);
			} else
			{
				ClaimNode(targetNode);
			}
		}
	}
}
