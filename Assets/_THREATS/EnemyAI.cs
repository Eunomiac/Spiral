using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavToWaypoint))]
public class EnemyAI : MonoBehaviour {

	private THREATS threats;
	private ARENA arena;
	private NavToWaypoint navigate;

	private Transform waypoint;

	public Node MyNode { get; set; }

	void Awake()
	{
		arena = GAME.Arena;
		threats = GAME.Threats;
		navigate = GetComponent<NavToWaypoint>();
	}

	//void Start()
	//{
	//	StartCoroutine(DecisionTree());
	//}

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
	void Start ()
	{
		StartCoroutine(SetRandomWaypoint());
	}

	IEnumerator SetRandomWaypoint ()
	{
		float secsToWait = Random.Range(0f, 2f);
		while ( true )
		{
			yield return new WaitForSeconds(secsToWait);
			Node randomNode = arena.GetRandomNode();
			waypoint = randomNode.transform;
			secsToWait = (float) (randomNode.Tier + 1) * 2;
			GetComponent<NavToWaypoint>().Waypoint = waypoint;
		}
	}

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
