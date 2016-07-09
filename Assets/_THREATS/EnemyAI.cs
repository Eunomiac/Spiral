using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.BehaviorTrees;
using RAIN.Navigation;
using RAIN.Navigation.Waypoints;
using RAIN.Memory;
using RAIN.Minds;


public class EnemyAI : MonoBehaviour {

	const string ATTACKNODE = "Attack";

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

	private AIRig aiRig;
	private RAINMemory memory;
	//private BasicMind mind;
	//private BTPriorityNode mainPriorityNode;
	//private int attackNodeIndex;

	private Node myNode;
	private Vector3? myDest;
	private bool isAttacking;


	private PLAYER player = GAME.Player;

	public Node MyNode {
		get { return myNode; }
		set {
			if (myNode != null && myNode != value)
				myNode.Unclaim();
			myNode = value;
			memory.SetItem("MyNode", value == null ? null : value.gameObject);
			if ( myNode != null )
			{
				memory.SetItem("CurrentTier", myNode.Tier);
				MyDestination = myNode.transform.position;
			}
			else
			{
				memory.SetItem<int?>("CurrentTier", null);
				MyDestination = null;
			}
		}
	}

	public Vector3? MyDestination
	{
		get { return myDest; }
		set {
			myDest = value;
			memory.SetItem("Destination", value);
			if (myDest != null)
				memory.SetItem("DistFromDest", transform.position.Distance2D((Vector3)myDest));
			else
				memory.SetItem<float?>("DistFromDest", null);
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

	public void UpdatePosition ()
	{
		memory.SetItem("MyPosition", transform.position);
		MyDestination = MyDestination;
	}

	void Awake()
	{
		// player = GAME.Player;
		aiRig = GetComponentInChildren<AIRig>();
		memory = aiRig.AI.WorkingMemory;
		//mind = aiRig.AI.Mind as BasicMind;
		memory.SetItem("AttackStartPriority", 0);
		memory.SetItem("AttackRunPriority", 0);
	}

	void Start ()
	{
		//BTNode rootNode = mind.BehaviorRoot;
		//for ( int i = 0; i < rootNode.GetChildCount(); i++ )
		//{
		//	if ( rootNode.GetChild(i).GetType() == typeof(BTPriorityNode) )
		//	{
		//		mainPriorityNode = rootNode.GetChild(i) as BTPriorityNode;
		//		break;
		//	}
		//}
		//attackNodeIndex = mainPriorityNode.GetChildIndex(ATTACKNODE);
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
		SetPriority(ATTACKNODE, 100);
	}

	void SetPriority(string nodeName, int start, int? run = null)
	{
		memory.SetItem(nodeName + "StartPriority", start);
		memory.SetItem(nodeName + "RunPriority", run ?? start);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.GetComponent<PLAYER>())
		{
			player.TakeHit(strength);
			IsAttacking = false;
			//SetPriority(ATTACKNODE, 0);
			MyDestination = null;
			//float myAngle = player.NavNetwork.GetAngleFromPosition(transform.position);
			//int tier = player.nodesPerTier.Length;
			//Node targetNode = null;
			//while ( targetNode == null && tier > 0 )
			//{
			//	float angleSpread = 30f;
			//	while ( targetNode == null && angleSpread <= 90f )
			//	{
			//		targetNode = player.NavNetwork.GetRandomNode((myAngle - angleSpread).Clamp(), (myAngle + angleSpread).Clamp(), Mathf.RoundToInt(Mathf.Infinity));
			//		angleSpread += 30f;
			//	}
			//	tier -= 1;
			//}
			//if ( targetNode == null )
			//{
			//	Debug.Log("No unoccupied node to retreat to!");
			//	Destroy(gameObject);
			//} else
			//{
			//	ClaimNode(targetNode);
			//}
		}
	}
}
