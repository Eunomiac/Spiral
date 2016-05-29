using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Waypoints;
using RAIN.Memory;

[RequireComponent (typeof(NavToWaypoint))]
public class EnemyAI : MonoBehaviour {

	/* USING RAIN AI FOR NAVIGATION
	 * - Can set waypoint network (hopefully) using: NavigationManager.Instance.GetWaypointSet(“waypointNetworkName”).Waypoints[index].position
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
	 * 
	 * */

	private RAINMemory memory;

	private THREATS threats;
	private ARENA arena;
	private PLAYER player;
	//private NavToWaypoint navigate;
	//private ProximityCheck proximityCheck;

	//private Transform waypoint;

	public Node MyNode { get; set; }

	void Awake()
	{
		arena = GAME.Arena;
		threats = GAME.Threats;
		player = GAME.Player;
		memory = GetComponentInChildren<AIRig>().AI.WorkingMemory;
		Debug.Log(memory.ToString());
		Invoke("AllInit", 1f);
		//navigate = GetComponent<NavToWaypoint>();
		//proximityCheck = GetComponentInChildren<ProximityCheck>(true);
	}

	void AllInit()
	{
		Debug.Log("All Init!");
		memory.SetItem("WaypointRig", arena.PlayerNavNetwork.WaypointRig.gameObject);
		//memory.SetItem("waypointpathchanged", true);
	}

	void Start ()
	{
		

		//StartCoroutine(DecisionTree());

	}

	//void Start ()
	//{
	//	FindNearbyNodes();
	//}

	//List<Node> FindNearbyNodes ()
	//{
	//	proximityCheck.gameObject.SetActive(true);
	//	proximityCheck.gameObject.SetActive(false);
	//	return proximityCheck.NearNodes;
	//} 


	/* BEHAVIOUR LIST:
	 * - Each Behaviour is an object instance with two primary methods and one property:
	 *		- public float Priority { get; set; }
	 *		- public bool IsDoable { < code to determine if this behaviour is appropriate > }
	 *		- public bool Action { < function to perform during this beat of the game > }
	 *		- each method is settable as a lambda function
	 * - After behaviours are added to an enemy's behaviour list, sort by priority.
	 * - Decision tree pops behaviours in order, checks IsDoable, and does first acceptable Action.
	 * */

	IEnumerator DecisionTree ()
	{
		yield return new WaitForSeconds(GAME.BeatDuration * 0.5f);
		do
		{

			yield return new WaitForSeconds(GAME.BeatDuration);
		} while ( true );
	}

	bool ClaimNode (Node node)
	{
		if (node.Claim(gameObject))
		{
			MyNode = node;
			return true;
		} else
			return false;
	}

	#region Debug Code
	//void Start ()
	//{
	//	StartCoroutine(SetRandomWaypoint());
	//}

	//IEnumerator SetRandomWaypoint ()
	//{
	//	float secsToWait = Random.Range(0f, 2f);
	//	while ( true )
	//	{
	//		yield return new WaitForSeconds(secsToWait);
	//		Node randomNode = arena.GetRandomNode();
	//		waypoint = randomNode.transform;
	//		secsToWait = (float) (randomNode.Tier + 1) * 2;
	//		GetComponent<NavToWaypoint>().Waypoint = waypoint;
	//	}
	//}

	//void OnDrawGizmos ()
	//{
	//	if ( threats.isShowingDestination && waypoint != null )
	//	{
	//		Gizmos.color = Color.green;
	//		Gizmos.DrawSphere(waypoint.position, 0.3f);
	//	}
	//}
	#endregion
}
