using UnityEngine;
using System.Collections;

public class PLAYER : MonoBehaviour
{
	private ARENA arena;
	private Vector3 lastPosition;

	void Awake()
	{
		arena = GameCache.Arena;
		lastPosition = transform.position;
	}

	void Update()
	{
		if (transform.position != lastPosition)
		{
			arena.BuildNavNodes();
			lastPosition = transform.position;
		}
	}
}
