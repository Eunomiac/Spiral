using UnityEngine;
using System.Collections;

// Component allowing any NavMeshAgent to be given a waypoint, and travel to it.
[RequireComponent (typeof(NavMeshAgent))]
public class NavToWaypoint : MonoBehaviour
{
	private Transform waypoint;
	private Vector3? destination;
	private NavMeshAgent agent;

	private ARENA arena;

	void Awake ()
	{
		arena = GameCache.Arena;
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

	#region Debug Code
	void OnDrawGizmos ()
	{
		if ( waypoint != null )
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(waypoint.position, 0.3f);
		}
	}
	#endregion
}
