using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	private THREATS threats;
	private ARENA arena;

	void Awake()
	{
		arena = GameCache.Arena;
		threats = GameCache.Threats;
	}

	#region Debug Code

	void Start ()
	{
		StartCoroutine(SetRandomWaypoint());
	}

	IEnumerator SetRandomWaypoint ()
	{
		float secsToWait = Random.Range(0f, 2f);
		while ( true )
		{
			yield return new WaitForSeconds(secsToWait);
			Wedge randomWedge = arena.Wedges[Random.Range(0, arena.Wedges.Length)];
			Node randomNode = randomWedge.Nodes[Random.Range(0, randomWedge.Nodes.Count)];
			secsToWait = (float) (randomNode.Tier + 1) * 2;
			GetComponent<NavToWaypoint>().SetWaypoint(randomNode.transform);
		}
	}



	#endregion
}
