using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProximityCheck : MonoBehaviour {

	private List<Node> nearNodes = new List<Node>();

	public List<Node> NearNodes { get { return nearNodes; } }

	void OnEnable ()
	{
		Debug.Log("ONENABLE: Clearing Near Nodes.");
		NearNodes.Clear();
		Invoke("DisableSelf", 0.1f);
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("COLLIDER FOUND!");
		Node thisNode = collider.gameObject.GetComponent<Node>();
		if ( thisNode )
		{
			Debug.Log("Adding to Near Nodes, Count = " + NearNodes.Count);
			NearNodes.Add(collider.gameObject.GetComponent<Node>());
		}
	}

	void DisableSelf()
	{
		gameObject.SetActive(false);
	}
}
