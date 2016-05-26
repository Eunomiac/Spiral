using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ARENA : MonoBehaviour
{
	public Node navNodePrefab;
	public bool showNodes = false;
	public bool showConnections = false;
	public bool showLabels = false;
	public Material lineMaterial;											// Debug

	private List<NavNetwork> navNetworks = new List<NavNetwork>();
	private PLAYER player;

	[HideInInspector]
	public List<NavNetwork> NavNetworks { get { return navNetworks; } }
	public NavNetwork PlayerNavNetwork { get { return navNetworks[0]; } }
	
	void Awake ()
	{
		player = GAME.Player; 
	}

	public NavNetwork InitializeNavNetwork(GameObject networkCore, int[] nodesPerTier, float[] distOfTier, float maxNeighbourDistMult)
	{
		NavNetwork thisNetwork = new GameObject(networkCore.name + "_NavNet", typeof(NavNetwork)).GetComponent<NavNetwork>();
		NavNetworks.Add(thisNetwork);
		thisNetwork.transform.SetParent(transform);
		thisNetwork.Initialize(networkCore, nodesPerTier, distOfTier, maxNeighbourDistMult);
		return thisNetwork;
	}

	public NavNetwork GetNavNetwork(GameObject networkCore)
	{
		foreach (NavNetwork network in NavNetworks)
			if ( network.Core == networkCore )
				return network;
		Debug.LogError("No navigation network found for network core '" + networkCore.name + "'.");
		return null;
	}

	// Though the ARENA class supports multiple nav networks attached to various GameObjects, 
	// the most commonly used is the player's nav network.  These functions are shortcuts that
	// allow calling the player's nav network directly from the ARENA game object.
	#region Player Nav Network Lookup Functions

	public Wedge GetWedgeByAngle(float angle)
	{
		return PlayerNavNetwork.GetWedgeByAngle(angle);
	}

	public Wedge GetWedgeByPosition(Vector3 position)
	{
		return PlayerNavNetwork.GetWedgeByPosition(position);
	}

	public List<Wedge> GetWedgesBetweenAngles (float minAngle = 0f, float maxAngle = 360f)
	{
		return PlayerNavNetwork.GetWedgesBetweenAngles(minAngle, maxAngle);
	}

	public Wedge GetRandomWedge(float minAngle = 0f, float maxAngle = 360f)
	{
		return PlayerNavNetwork.GetRandomWedge(minAngle, maxAngle);
	}

	public List<Node> GetNodes (float minAngle = 0f, float maxAngle = 360f, int? tier = null)
	{
		return PlayerNavNetwork.GetNodes(minAngle, maxAngle, tier);
	}

	public Node GetRandomNode(float minAngle = 0f, float maxAngle = 360f, int? tier = null)
	{
		return PlayerNavNetwork.GetRandomNode(minAngle, maxAngle, tier);
	}
	#endregion
}
