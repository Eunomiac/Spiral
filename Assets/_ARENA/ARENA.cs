using UnityEngine;
using System;
using System.Collections;

public class ARENA : MonoBehaviour
{
	public int[] nodesPerTier = new int[3] { 8, 16, 32 };
	public float[] distOfTier = new float[3] { 3f, 6f, 9f };
	public Node navNodePrefab;
	public bool showConnections = false;
	public bool showLabels = false;
	
	private Node[][] navNodes;
	private Wedge[] navWedges;
	private GameObject navNodeContainer;
	private PLAYER player;

	void Awake ()
	{
		player = GameCache.Player; 
		InitializeNavWedges();
		InitializeNavNodes();
		BuildNavNodes();
	}

	void InitializeNavWedges ()
	{
		navNodeContainer = new GameObject("NavNodes");
		navNodeContainer.transform.SetParent(transform);
		navWedges = new Wedge[nodesPerTier[0]];
		float angleSpread = 360f / nodesPerTier[0];
		float angleSplit = angleSpread / 2;
		for ( int i = 0; i < nodesPerTier[0]; i++ )
		{
			Wedge thisWedge = new GameObject().AddComponent<Wedge>();
			thisWedge.transform.SetParent(navNodeContainer.transform);
			thisWedge.MinAngle = (angleSpread * i - angleSplit).Clamp();
			thisWedge.MaxAngle = (angleSpread * i + angleSplit - Mathf.Epsilon).Clamp();
			thisWedge.name = "Wedge " + i + ": " + Mathf.RoundToInt(thisWedge.MinAngle) + "-" + Mathf.RoundToInt(thisWedge.MaxAngle);
			navWedges[i] = thisWedge;
		}
		for ( int i = 0; i < navWedges.Length; i++ )
		{
			Wedge thisWedge = navWedges[i];
			thisWedge.Neighbours.Add(GetWedgeByAngle((thisWedge.MinAngle - 1f).Clamp()));
			thisWedge.Neighbours.Add(GetWedgeByAngle((thisWedge.MaxAngle + 1f).Clamp()));
		}
	}

	void InitializeNavNodes ()
	{
		navNodes = new Node[nodesPerTier.Length][];
		for ( int tier = 0; tier < nodesPerTier.Length; tier++ )
		{
			navNodes[tier] = new Node[nodesPerTier[tier]];
			for ( int i = 0; i < navNodes[tier].Length; i++ )
			{
				Node thisNode = Instantiate(navNodePrefab);
				thisNode.name = "T" + tier.ToString() + " Node " + i.ToString();
				thisNode.Tier = tier;
				thisNode.Index = i;
				thisNode.RandomOffset = (UnityEngine.Random.Range(0.9f, 1.1f) + UnityEngine.Random.Range(0.9f, 1.1f)) / 2;
				navNodes[tier][i] = thisNode;
			}
		}
	}

	public void BuildNavNodes (Vector3? center = null)
	{
		Vector3 centerPos = center ?? player.transform.position;
		for ( int tier = 0; tier < nodesPerTier.Length; tier++ )
		{
			float tierRadius = distOfTier[tier];
			Vector3 startVector = Vector3.forward * tierRadius;
			float angleSpread = 360f / nodesPerTier[tier];

			// Determine offset angle.
			float angleOffset = 0f;
			if (tier > 0)
			{
				int nodesPerLowerNode = Mathf.RoundToInt(nodesPerTier[tier] / nodesPerTier[tier - 1]);
				if (nodesPerLowerNode % 2 == 0 && Mathf.RoundToInt(navNodes[tier-1][0].Angle) == 0)
					angleOffset = angleSpread / 2;
				else angleOffset = 0f;
			}

			for ( int i = 0; i < navNodes[tier].Length; i++ )
			{
				Node thisNode = navNodes[tier][i];
				thisNode.Angle = angleSpread * i + angleOffset;
				Vector3 nodePosition = Quaternion.Euler(0, thisNode.Angle, 0) * startVector * thisNode.RandomOffset + centerPos;
				thisNode.transform.position = nodePosition;
				Wedge thisWedge = GetWedgeByAngle(thisNode.Angle);
				thisNode.transform.SetParent(thisWedge.transform);
				thisWedge.Nodes.Add(thisNode);
if ( showLabels ) { thisNode.SetNodeLabel(); }
				FindNeighbours(tier, i);
			}
		}
		if ( showConnections )
		{
			foreach ( Node[] nodeTier in navNodes )
			{
				foreach ( Node node in nodeTier )
				{
					node.DrawNeighbourLines(lineMaterial);
				}
			}
		}
	}

	void FindNeighbours (int tier, int index)
	{
		Node[] thisTier = navNodes[tier];
		Node thisNode = thisTier[index];

		// Capture the two nodes to either side on the same tier.
		for ( int i = -1; i <= 1; i += 2 )
			ConnectNeighbours(thisNode, thisTier, index + i);

		if ( tier == 0 ) return;

		Node[] lowerTier = navNodes[tier - 1];

		// Capture the neighbouring nodes below this node.
		//	- 1) Find index of closest lower node to upper node AND add it as neighbour.
		float lowerNodesPerUpper = (float) nodesPerTier[tier] / nodesPerTier[tier - 1];
		int closestLowerNodeIndex = Mathf.FloorToInt(index / lowerNodesPerUpper);
		if (Mathf.RoundToInt(thisTier[0].Angle) != 0)
			closestLowerNodeIndex++;

		//	- 2) Add closest node, then add adjacent lower nodes, alternating sides until number of neighbour nodes reached.
		int numLowerNeighbours = Mathf.RoundToInt(lowerNodesPerUpper);
		int searchDist = 0, searchSide = -1;

		while (thisNode.Neighbours.Count < numLowerNeighbours + 2)
		{
			ConnectNeighbours(thisNode, lowerTier, closestLowerNodeIndex + searchDist * searchSide);
			searchSide *= -1;
			searchDist += (searchSide > 0) ? 0 : 1;
		}
	}

	void ConnectNeighbours(Node nodeA, Node[] tier, int index)
	{
		Node nodeB = tier.Wrap(index);

		if ( !nodeA.Neighbours.Contains(nodeB) )
			nodeA.Neighbours.Add(nodeB);
		if ( !nodeB.Neighbours.Contains(nodeA) )
			nodeB.Neighbours.Add(nodeA);
	}

	public Wedge GetWedgeByAngle(float angle)
	{
		angle = angle + 0.1f;
		for (int i = 0; i < navWedges.Length; i++)
		{
			if ( angle.IsBetween(navWedges[i].MinAngle, navWedges[i].MaxAngle))
				return navWedges[i];
		}
		Debug.LogError("No valid wedge found for angle " + angle);
		return null;
	}

	public Wedge GetWedgeByPosition(Vector3 position)
	{
		float absX = position.x * (position.x < 0f ? -1 : 1);
		float absZ = position.z * (position.z < 0f ? -1 : 1);
		float angle = Mathf.Atan2(absZ, absX);

		if ( position.x < 0f && position.z < 0f ) { angle = 270f - angle; }
		else if ( position.x < 0f ) { angle = 270f + angle; }
		else if ( position.z < 0f ) { angle = 90f + angle; }
		else { angle = 90f - angle; }

		return GetWedgeByAngle(angle);
	}

public Material lineMaterial;

public Wedge[] Wedges { get { return navWedges; } }
}
