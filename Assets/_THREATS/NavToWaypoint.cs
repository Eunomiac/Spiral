using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	void Start ()
	{
		StartCoroutine(SetRandomWaypoint());
	}

	void Update ()
	{
		// Update destination if off from waypoint by more than one unit.
		if ( waypoint && Vector3.Distance(transform.position, waypoint.position) > 0.2f && (destination == null || Vector3.Distance((Vector3)destination, waypoint.position) > 1.0f ))
		{
			destination = waypoint.position;
			agent.SetDestination((Vector3)destination);
		}
	}

	IEnumerator SetRandomWaypoint()
	{
		while (true) 
		{
			yield return new WaitForSeconds(3);
			ARENA.NavNodes attackNodes = arena.attackNodes;
			List<Transform> nodeList = Random.Range(0f, 1f) < 0.2f ? attackNodes.close : attackNodes.far;
			Transform targetNode = nodeList[Random.Range(0, nodeList.Count)];
			SetWaypoint(targetNode);
		}
	}

	public void SetWaypoint(Transform target)
	{
		waypoint = target;
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
