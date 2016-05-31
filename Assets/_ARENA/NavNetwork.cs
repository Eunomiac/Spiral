﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RAIN.Navigation;
using RAIN.Navigation.Waypoints;

public class NavNetwork : MonoBehaviour {

	private GameObject navCore;
	private Wedge[] navWedges;
	private Node[][] navNodes;
	private Node centerNode;

	//private WaypointRig waypointRig;

	private ARENA arena;

	public GameObject Core { get { return navCore; } }
	public Wedge[] Wedges { get { return navWedges; } }
	public Node[][] Nodes { get { return navNodes; } }
	public Node CenterNode { get { return centerNode; } }
	//public WaypointRig WaypointRig { get { return waypointRig; } set { waypointRig = value; } }

	void Awake ()
	{
		arena = GAME.Arena;
	}

	public void Initialize (GameObject center, int[] nodesPerTier, float[] distOfTier, float maxNeighbourDistMult)
	{
		InitializeNavWedges(nodesPerTier);
		InitializeNavNodes(nodesPerTier);
		BuildNavNodes(center, nodesPerTier, distOfTier);
		ConnectAllNeighbours(Nodes, nodesPerTier, distOfTier, maxNeighbourDistMult);
		//BuildWaypointRig(center, nodesPerTier);
	}

	#region Nav Network Initial Creation
	void InitializeNavWedges (int[] nodesPerTier)
	{
		navWedges = new Wedge[nodesPerTier[0]];
		float angleSpread = 360f / nodesPerTier[0];
		float angleSplit = angleSpread / 2;
		for ( int i = 0; i < nodesPerTier[0]; i++ )
		{
			Wedge thisWedge = new GameObject().AddComponent<Wedge>();
			thisWedge.transform.SetParent(transform);
			thisWedge.MinAngle = (angleSpread * i - angleSplit).Clamp();
			thisWedge.MaxAngle = (angleSpread * i + angleSplit - Mathf.Epsilon).Clamp();
			thisWedge.name = "Wedge " + i + ": " + Mathf.RoundToInt(thisWedge.MinAngle) + "-" + Mathf.RoundToInt(thisWedge.MaxAngle);
			navWedges[i] = thisWedge;
		}
		for ( int i = 0; i < navWedges.Length; i++ )
		{
			Wedge thisWedge = navWedges[i];
			thisWedge.Neighbours.Add(GetWedgeFromAngle((thisWedge.MinAngle - 1f).Clamp()));
			thisWedge.Neighbours.Add(GetWedgeFromAngle((thisWedge.MaxAngle + 1f).Clamp()));
		}
	}

	void InitializeNavNodes (int[] nodesPerTier)
	{
		centerNode = Instantiate(arena.navNodePrefab);
		centerNode.name = "Center Node";
		centerNode.Tier = 0;
		centerNode.Index = 0;
		centerNode.RandomOffset = 1f;
		centerNode.Angle = 0f;
		centerNode.transform.SetParent(transform);
		navNodes = new Node[nodesPerTier.Length][];

		for ( int tier = 0; tier < nodesPerTier.Length; tier++ )
		{
			navNodes[tier] = new Node[nodesPerTier[tier]];
			for ( int i = 0; i < navNodes[tier].Length; i++ )
			{
				Node thisNode = Instantiate(arena.navNodePrefab);
				thisNode.name = "T" + tier.ToString() + " Node " + i.ToString();
				thisNode.Tier = tier;
				thisNode.Index = i;
				thisNode.RandomOffset = tier == 0 ? 1f : (Random.Range(0.9f, 1.1f) + Random.Range(0.9f, 1.1f)) / 2;
				navNodes[tier][i] = thisNode;
			}
		}
	}

	public void BuildNavNodes (GameObject center, int[] nodesPerTier, float[] distOfTier)
	{
		navCore = center;
		Vector3 centerPos = center.transform.position;
		centerNode.transform.position = centerPos;
		for ( int tier = 0; tier < nodesPerTier.Length; tier++ )
		{
			float tierRadius = distOfTier[tier];
			Vector3 startVector = Vector3.forward * tierRadius + Vector3.up;
			float angleSpread = 360f / nodesPerTier[tier];

			// Determine offset angle.
			float angleOffset = 0f;
			if ( tier > 0 )
			{
				int nodesPerLowerNode = Mathf.RoundToInt(nodesPerTier[tier] / nodesPerTier[tier - 1]);
				if ( nodesPerLowerNode % 2 == 0 && Mathf.RoundToInt(navNodes[tier - 1][0].Angle) == 0 )
					angleOffset = angleSpread / 2;
				else angleOffset = 0f;
			}

			for ( int i = 0; i < navNodes[tier].Length; i++ )
			{
				Node thisNode = navNodes[tier][i];
				thisNode.Angle = angleSpread * i + angleOffset;
				Vector3 nodePosition = Quaternion.Euler(0, thisNode.Angle, 0) * startVector * thisNode.RandomOffset + centerPos;
				thisNode.transform.position = nodePosition;
				Wedge thisWedge = GetWedgeFromAngle(thisNode.Angle);
				thisNode.transform.SetParent(thisWedge.transform);
				thisWedge.Nodes.Add(thisNode);
				if ( arena.showLabels ) { thisNode.SetNodeLabel(); }                                          // Debug
				if ( arena.showConnections ) { thisNode.DrawNeighbourLines(arena.lineMaterial); }			  //
			}
		}
	}

	void ConnectAllNeighbours (Node[][] nodeList, int[] nodesPerTier, float[] distOfTier, float maxNeighbourDistMult)
	{
		foreach ( Node[] tier in nodeList )
		{
			foreach ( Node node in tier )
			{
				int tierNum = node.Tier;
				int index = node.Index;
				float maxDist = (distOfTier[tierNum] - (tierNum == 0 ? 0f : distOfTier[tierNum - 1])) * maxNeighbourDistMult;
				Node[] thisTier = navNodes[tierNum];

				// Capture the two nodes to either side on the same tier, regardless of distance.
				for ( int i = -1; i <= 1; i += 2 )
					ConnectNeighbours(node, thisTier.Wrap(index + i));

				if ( tierNum == 0 ) continue;

				// Capture the neighbouring nodes below this node.
				Node[] lowerTier = navNodes[tierNum - 1];

				//	- 1) Find index of closest lower node to upper node AND add it as neighbour, regardless of distance.
				float lowerNodesPerUpper = (float) nodesPerTier[tierNum] / nodesPerTier[tierNum - 1];
				int closestLowerNodeIndex = Mathf.FloorToInt(index / lowerNodesPerUpper);
				ConnectNeighbours(node, lowerTier.Wrap(closestLowerNodeIndex));

				//	- 2) Add adjacent lower nodes, alternating sides until max allowed distance to neighbour exceeded on both sides.
				for ( int i = 1; i >= 0; i++ )
				{
					Node[] theseLowNodes = new Node[2] { lowerTier.Wrap(closestLowerNodeIndex + i), lowerTier.Wrap(closestLowerNodeIndex - i) };
					if ( !ConnectNeighbours(node, theseLowNodes[0], maxDist)
					  & !ConnectNeighbours(node, theseLowNodes[1], maxDist) )
						break;
				}
				if ( arena.showConnections ) { node.DrawNeighbourLines(arena.lineMaterial); }
			}
		}
	}

	bool ConnectNeighbours (Node nodeA, Node nodeB, float? maxDist = null)
	{
		float maxDistance = maxDist ?? 0f;
		if ( maxDistance > 0 && Vector3.Distance(nodeA.transform.position, nodeB.transform.position) > maxDistance )
			return false;
		if ( !nodeA.Neighbours.Contains(nodeB) )
			nodeA.Neighbours.Add(nodeB);
		if ( !nodeB.Neighbours.Contains(nodeA) )
			nodeB.Neighbours.Add(nodeA);
		return true;
	}

	//void BuildWaypointRig (GameObject center, int[] nodesPerTier)
	//{
	//	WaypointRig = Instantiate(arena.waypointRigPrefab, Vector3.zero, Quaternion.identity) as WaypointRig;
	//	WaypointRig.transform.SetParent(arena.transform);
	//	WaypointRig.name = center.name + " Waypoint Rig";
	//	WaypointSet theseWaypoints = WaypointRig.WaypointSet;
	//	for ( int i = 0; i < Nodes.Length; i++ )
	//	{
	//		Node[] thisTier = Nodes[i];
	//		for ( int j = 0; j < thisTier.Length; j++ )
	//		{
	//			Node thisNode = thisTier[j];
	//			Waypoint thisWaypoint = new Waypoint();
	//			thisWaypoint.Position = thisNode.transform.position;
	//			thisWaypoint.WaypointName = thisNode.name + " (WP)";
	//			theseWaypoints.AddWaypoint(thisWaypoint);
	//			thisNode.WaypointIndex = theseWaypoints.IndexOfWaypoint(thisWaypoint);
	//		}
	//	}
	//	for ( int i = 0; i < Nodes.Length; i++ )
	//	{
	//		Node[] thisTier = Nodes[i];
	//		for ( int j = 0; j < thisTier.Length; j++ )
	//		{
	//			Node thisNode = thisTier[j];
	//			foreach ( Node neighbour in thisNode.Neighbours )
	//			{
	//				int neighbourIndex = neighbour.WaypointIndex;
	//				theseWaypoints.AddConnection(thisNode.WaypointIndex, neighbourIndex, true);
	//			}
	//		}
	//	}
	//}

	#endregion

	public float GetAngleFromPosition (Vector3 position)
	{
		float absX = position.x * (position.x < 0f ? -1 : 1);
		float absZ = position.z * (position.z < 0f ? -1 : 1);
		float angle = Mathf.Atan2(absZ, absX);

		if ( position.x < 0f && position.z < 0f ) { angle = 270f - angle; }
		else if ( position.x < 0f ) { angle = 270f + angle; }
		else if ( position.z < 0f ) { angle = 90f + angle; }
		else { angle = 90f - angle; }

		return angle;
	}

	public Wedge GetWedgeFromAngle (float angle)
	{
		angle = angle + 0.1f;
		for ( int i = 0; i < Wedges.Length; i++ )
		{
			if ( angle.IsBetween(Wedges[i].MinAngle, Wedges[i].MaxAngle) )
				return Wedges[i];
		}
		Debug.LogError("No valid wedge found for angle " + angle);
		return null;
	}

	public Wedge GetWedgeFromPosition (Vector3 position)
	{
		return GetWedgeFromAngle(GetAngleFromPosition(position));
	}

	public List<Wedge> GetWedgesBetweenAngles (float minAngle = 0f, float maxAngle = 360f)
	{
		List<Wedge> theseWedges = Wedges.ToList();
		if ( !(minAngle == 0f && maxAngle == 360f) )
		{
			foreach ( Wedge wedge in Wedges )
			{
				if ( !(minAngle.IsBetween(wedge.MinAngle, wedge.MaxAngle) || maxAngle.IsBetween(wedge.MinAngle, wedge.MaxAngle)) )
					theseWedges.Remove(wedge);
			}
		}
		return theseWedges;
	}

	public Wedge GetRandomWedge (float minAngle = 0f, float maxAngle = 360f)
	{
		List<Wedge> theseWedges = GetWedgesBetweenAngles(minAngle, maxAngle);
		return theseWedges.Count == 0 ? null : theseWedges.Random();
	}

	public List<Node> GetNodes (float minAngle = 0f, float maxAngle = 360f, int? tier = null, bool unoccupiedOnly = false, Vector3? distSortVector = null)
	{
		List<Wedge> theseWedges = GetWedgesBetweenAngles(minAngle, maxAngle);
		List<Node> theseNodes = new List<Node>();
		foreach ( Wedge wedge in theseWedges )
			foreach ( Node node in wedge.Nodes )
				if ( (!unoccupiedOnly || node.Occupant == null) && (tier == null || tier == node.Tier) )
					theseNodes.Add(node);
		if ( distSortVector != null && theseNodes.Count > 1 )
		{
			Vector3 distVec = (Vector3) distSortVector;
			theseNodes.Sort((x, y) => Vector3.Distance(x.transform.position, distVec).CompareTo(Vector3.Distance(y.transform.position, distVec)));
		}
		return theseNodes;
	}

	public Node GetClosestNode (Vector3 distVec, float minAngle = 0f, float maxAngle = 360f, int? tier = null, bool unoccupiedOnly = true)
	{
		List<Node> nodes = GetNodes(minAngle, maxAngle, tier, unoccupiedOnly, distVec);
		return nodes.Count == 0 ? null : nodes[0];
	}

	public Node GetRandomNode (float minAngle = 0f, float maxAngle = 360f, int? tier = null, bool unoccupiedOnly = true)
	{
		List<Node> nodes = GetNodes(minAngle, maxAngle, tier, unoccupiedOnly);
		return nodes.Count == 0 ? null : nodes.Random();
	}
}
