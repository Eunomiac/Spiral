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

	public NavNetwork NavNetwork { get; set; }
	
	void Awake ()
	{
		arena = GAME.Arena;
	}

	void Start()
	{
		NavNetwork = arena.InitializeNavNetwork(gameObject, nodesPerTier, radiusOfTier, maxNeighbourDistMult);
	}

	void Update()
	{
		if (transform.hasChanged)
		{
			NavNetwork.BuildNavNodes(gameObject, nodesPerTier, radiusOfTier);
			transform.hasChanged = false;
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

	public void TakeHit (float strength)
	{
		Debug.Log("Took hit of " + strength + " strength!");
	}

}
