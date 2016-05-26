using UnityEngine;
using System.Collections;

public class PLAYER : MonoBehaviour
{
	public int[] nodesPerTier = new int[3] { 8, 16, 32 };
	public float[] radiusOfTier = new float[3] { 3f, 6f, 9f };
	public float maxNeighbourDistMult = 1.5f;

	private ARENA arena;
	private Vector3 lastPosition;
	private NavNetwork navNetwork;


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
}
