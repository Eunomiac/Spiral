using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class NavToWaypoint : MonoBehaviour
{
	private Transform waypoint;
	private Vector3? destination;
	private NavMeshAgent agent;

	private ARENA arena;

	void Awake ()
	{
		arena = FindObjectOfType<ARENA>();
		agent = GetComponent<NavMeshAgent>();
	}

	void Update ()
	{
		// Update destination if off from waypoint by more than one unit.
		if ( waypoint && 
			Vector3.Distance(transform.position, waypoint.position) > 0.2f && 
			(destination == null || Vector3.Distance((Vector3)destination, waypoint.position) > 1.0f ))
		{
			destination = waypoint.position;
			agent.SetDestination((Vector3)destination);
		}
	}

	public void SetWaypoint(Transform target)
	{
		waypoint = target;
	}

	// **** DEBUGGING ****

	void Start () { StartCoroutine(SetRandomWaypoint()); }

	IEnumerator SetRandomWaypoint ()
	{
		float secsToWait = Random.Range(0f, 2f);
		while ( true )
		{
			yield return new WaitForSeconds(secsToWait);
			int randomTier = Random.Range(0, arena.navNodes.Length);
			secsToWait = (float) (randomTier + 1) * 2;
			NavNode[] tierList = arena.navNodes[randomTier];
			SetWaypoint(tierList[Random.Range(0, tierList.Length)].transform);
		}
	}

	void OnDrawGizmos ()
	{
		if ( waypoint != null )
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(waypoint.position, 0.3f);
		}
	}

}
