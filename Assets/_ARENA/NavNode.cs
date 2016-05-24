using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavNode : MonoBehaviour {

	public List<NavNode> Neighbours = new List<NavNode>();
	
	public float Angle { get; set; }
	public int Tier { get; set; }
	public int Index { get; set; }
	public float RandomOffset { get; set; }

	// **** DEBUGGING ****

	Color[] colorsByTier = new Color[8] { Color.red, Color.magenta, Color.cyan, Color.green, Color.gray, Color.yellow, Color.blue, Color.black };

	void Start () { DrawNeighbourLines(FindObjectOfType<ARENA>().lineMaterial); }

	public void DrawNeighbourLines(Material material)
	{
		foreach (NavNode neighbour in Neighbours)
		{
			Color color = colorsByTier[Random.Range(0, 8)];
			LineRenderer thisLine = new GameObject("Line T" + Tier + Index + " >> T" + neighbour.Tier + neighbour.Index, typeof(LineRenderer)).GetComponent<LineRenderer>();
			thisLine.SetColors(color, color);
			Vector3[] targetVertices = new Vector3[2];
			targetVertices[0] = transform.position;
			targetVertices[1] = neighbour.transform.position;
			thisLine.SetPositions(targetVertices);
			thisLine.material = material;
			thisLine.SetWidth(0.15f, 0.01f);
			thisLine.transform.SetParent(transform);
		}
	}
	
	void OnDrawGizmos ()
	{ 
		Gizmos.color = colorsByTier[Tier];
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}
