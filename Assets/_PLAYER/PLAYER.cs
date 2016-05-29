using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Navigation;
using RAIN.Navigation.Waypoints;

public class PLAYER : MonoBehaviour
{
	public int[] nodesPerTier = new int[3] { 8, 16, 32 };
	public float[] radiusOfTier = new float[3] { 3f, 6f, 9f };
	public float maxNeighbourDistMult = 1.5f;

	private ARENA arena;
	private Vector3 lastPosition;
	private NavNetwork navNetwork;
	//private WaypointSet navNetwork;


	void Awake ()
	{
		arena = GAME.Arena;
	}

	void Start()
	{
		navNetwork = arena.InitializeNavNetwork(gameObject, nodesPerTier, radiusOfTier, maxNeighbourDistMult);
	}

	void Update()
	{
		if (transform.position != lastPosition)
		{
			navNetwork.BuildNavNodes(gameObject, nodesPerTier, radiusOfTier);
			lastPosition = transform.position;
		}
	}

	public void FirstTap (int axis, Vector3? startDirLS)
	{

	}

	public void MultiTap (int axis, int taps)
	{

	}

	public void StartHold (int axis, int taps)
	{

	}

	public void EndHold ()
	{

	}



}
