using UnityEngine;
using System;
using System.Collections;

public class ARENA : MonoBehaviour
{
	public int[] nodesPerTier = new int[3] { 8, 16, 32 };
	public float[] distOfTier = new float[3] { 3f, 6f, 9f };
	

	[HideInInspector]
	public NavNode[][] navNodes;
	private GameObject navNodeContainer;
	private PLAYER player;

	// Use this for initialization
	void Awake ()
	{
		player = FindObjectOfType<PLAYER>();
		InitializeNavNodes();
		BuildNavNodes();
	}

	void InitializeNavNodes ()
	{
		navNodeContainer = new GameObject("NavNodes");
		navNodeContainer.transform.SetParent(transform);
		navNodes = new NavNode[nodesPerTier.Length][];
		for ( int tier = 0; tier < nodesPerTier.Length; tier++ )
		{
			navNodes[tier] = new NavNode[nodesPerTier[tier]];
			for ( int i = 0; i < navNodes[tier].Length; i++ )
			{
				navNodes[tier][i] = new GameObject("T" + tier.ToString() + " Node " + i.ToString(), typeof(NavNode)).GetComponent<NavNode>();
				navNodes[tier][i].Tier = tier;
				navNodes[tier][i].Index = i;
				navNodes[tier][i].RandomOffset = (UnityEngine.Random.Range(0.9f, 1.1f) + UnityEngine.Random.Range(0.9f, 1.1f)) / 2;
				navNodes[tier][i].transform.SetParent(navNodeContainer.transform);
			}
		}
	}

	public void BuildNavNodes (Vector3? center = null)
	{
		Vector3 centerPos = center ?? player.transform.position;
		float angleOffset = 0f;
		for ( int tier = 0; tier < nodesPerTier.Length; tier++ )
		{
			float tierRadius = distOfTier[tier];
			float angleSpread = 360f / nodesPerTier[tier];
			Vector3 startVector = Vector3.forward * tierRadius;
			if ( tier > 0 && Mathf.RoundToInt(nodesPerTier[tier] / nodesPerTier[tier - 1]) % 2 == 0 )
				angleOffset += (angleOffset > 0f ? -1 : 1) * angleSpread / 2;
			else angleOffset = 0f;
			for ( int i = 0; i < navNodes[tier].Length; i++ )
			{
				navNodes[tier][i].Angle = angleSpread * i + angleOffset;
				Vector3 nodePosition = Quaternion.Euler(0, navNodes[tier][i].Angle, 0) * startVector * navNodes[tier][i].RandomOffset + centerPos;
				navNodes[tier][i].transform.position = nodePosition;
				FindNeighbours(tier, i);
			}
		}
	}

	void FindNeighbours (int tier, int index)
	{

		NavNode[] thisTier = navNodes[tier];
		NavNode[] lowerTier = tier == 0 ? null : navNodes[tier - 1];
		NavNode thisNode = thisTier[index];

		// Helper function to wrap any index around an array.
		Func<Array, int, int> idx = (array, i) => {
			int thisIndex = i;
			while ( thisIndex >= i )
				thisIndex -= array.Length;
			while ( thisIndex < 0 )
				thisIndex += array.Length;
			return thisIndex;
		};

		// Capture the two nodes to either side on the same tier.
		for ( int i = -1; i <= 1; i += 2 )
			thisNode.Neighbours.Add(thisTier[idx(thisTier, index + i)]);

		if ( tier == 0 ) return;

		// Capture the neighbouring nodes below this node.
		float lowerToUpperNodeRatio = (float) nodesPerTier[tier - 1] / (float) nodesPerTier[tier];
		float centerPoint = index * lowerToUpperNodeRatio;
		int firstNodeIndex = Mathf.RoundToInt(centerPoint - 0.00001f) - Mathf.RoundToInt(lowerToUpperNodeRatio * 0.5f + 0.00001f);
		for ( int i = 0; i < 1/lowerToUpperNodeRatio; i++ )
		{
			thisNode.Neighbours.Add(navNodes[tier - 1][idx(lowerTier, firstNodeIndex + i)]);
			navNodes[tier - 1][idx(lowerTier, firstNodeIndex + i)].Neighbours.Add(thisNode);
		}
	}

	public Material lineMaterial;
}
