using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ARENA : MonoBehaviour
{
	public int numCloseNodes = 6, numFarNodes = 12;
	public float closeDistance = 1.5f, closeFarDistance = 3f;

	private GameObject navNodes;
	public NavNodes attackNodes;
	private Vector3 centerNodePos;

	private PLAYER player;

	// Use this for initialization
	void Awake ()
	{
		player = FindObjectOfType<PLAYER>();
		centerNodePos = player.transform.position;
		navNodes = new GameObject("Nav Nodes");
		navNodes.transform.SetParent(transform);
	}

	void Start ()
	{
		SetNavNodes(player.transform.position);
	}

	public void SetNavNodes (Vector3? centerPos = null)
	{
		centerNodePos = centerPos ?? player.transform.position;
		attackNodes = MakeNavNodes(numCloseNodes, numFarNodes, closeDistance, closeDistance + closeFarDistance);
		ParentNavNodes(attackNodes);
	}

	NavNodes MakeNavNodes (int numClose, int numFar, float closeDist, float farDist)
	{
		// Lambda function takes number of nodes and radius, and returns List<Vector3> of node positions.
		Func<int, float, float, List<Vector3>> GetNodePositions = (int numNodes, float radius, float offsetRatio) => {
			List<Vector3> nodes = new List<Vector3>();
			float angleSpread = 360f / numNodes;
			Vector3 startVector = Vector3.forward * radius;
			while ( nodes.Count < numNodes )
			{
				float thisNodeAngle = angleSpread * nodes.Count + angleSpread * offsetRatio;
				Vector3 nodePosition = Quaternion.Euler(0, thisNodeAngle, 0) * startVector + centerNodePos;
				nodes.Add(nodePosition);
			}
			return nodes;
		};

		return new NavNodes(GetNodePositions(numClose, closeDist, 0f), GetNodePositions(numFar, farDist, (float) numClose / numFar));
	}

	// Object instance has two properties listing navigation nodes (as List<Transform>()'s): one for "close" nodes and one for "far" nodes, relative to the player.
	// Constructor takes List<Vector3> parameters describing positions of nodes, and instantiates GameObjects to store the navigation node transforms.
	public class NavNodes
	{
		public List<Transform> close { get; set; }
		public List<Transform> far { get; set; }

		// Constructor
		public NavNodes (List<Vector3> closeNodePos, List<Vector3> farNodePos)
		{
			close = close ?? new List<Transform>();
			for ( int i = 0; i < closeNodePos.Count; i++ )
			{
				if ( close.Count <= i )
					close.Add(new GameObject("Close Node").transform);
				close[i].position = closeNodePos[i];
			}

			far = far ?? new List<Transform>();
			for ( int i = 0; i < farNodePos.Count; i++ )
			{
				if ( far.Count <= i )
					far.Add(new GameObject("Far Node").transform);
				far[i].position = farNodePos[i];
			}
		}

		// Enumerator over all positions
		public IEnumerable<Transform> AllNavNodes ()
		{
			foreach ( Transform node in close )
				yield return node;
			foreach ( Transform node in far )
				yield return node;
		}

	}

	public void ParentNavNodes(NavNodes nodes)
	{
		foreach (Transform node in nodes.AllNavNodes())
			node.SetParent(navNodes.transform);
	}

	void OnDrawGizmos ()
	{
		if ( attackNodes != null )
		{
			foreach ( Transform node in attackNodes.close )
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(node.position, 0.5f);
			}
			foreach ( Transform node in attackNodes.far )
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere(node.position, 0.5f);
			}
		}
	}
}
