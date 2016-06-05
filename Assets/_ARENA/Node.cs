using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

public Text nodeLabel;

	//[HideInInspector]
	public List<Node> Neighbours = new List<Node>();
	
	public float Angle { get; set; }
	public int Tier { get; set; }
	public int Index { get; set; }
	public float RandomOffset { get; set; }
	public GameObject occupant;

	public GameObject Occupant { get { return occupant; } set { occupant = value; } }

	private ARENA arena;
	private Text label;

	void Awake ()
	{
		arena = GAME.Arena;
	}

	void Start ()
	{
		if ( arena.showNodes ) GetComponent<SpriteRenderer>().color = colorsByTier[Tier];			// Debug
	}

	public bool Claim(GameObject occupant)
	{
		if ( Occupant == null )
		{
			Occupant = occupant;
			return true;
		} else
			return false;
	}

	public void Unclaim()
	{
		Occupant = null;
	}

	public List<Node> GetNeighbours (int minTier = 0, int? max = null, bool unoccupiedOnly = true)
	{
		int maxTier = max ?? GAME.Player.nodesPerTier.Length;
		List<Node> theseNodes = new List<Node>();
		foreach ( Node node in Neighbours )
		{
			//Debug.Log("... Neighbour " + node.name + ", Tier " + node.Tier + ", Occupant " + node.Occupant.ToString());
			if ( node.Tier >= minTier && node.Tier <= maxTier && (!unoccupiedOnly || node.Occupant == null) )
				theseNodes.Add(node);
		}
		return theseNodes;
	}

	#region Debug Code

	Color[] colorsByTier = new Color[8] { Color.red, Color.magenta, Color.cyan, Color.green, Color.gray, Color.yellow, Color.blue, Color.black };



	public void DrawNeighbourLines(Material material)
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		foreach (Node neighbour in Neighbours)
		{
			Color color = colorsByTier.Random();
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

	public void SetNodeLabel()
	{
		label = label ?? Instantiate(nodeLabel);
		label.rectTransform.SetParent(arena.GetComponentInChildren<Canvas>().transform, false);
		label.rectTransform.anchoredPosition = new Vector2(transform.position.x, transform.position.z);
		label.text = Index.ToString() + "\n" + Mathf.RoundToInt(Angle).ToString();
	}

	//void OnDrawGizmos ()
	//{ 
	//	Gizmos.color = colorsByTier[Tier];
	//	Gizmos.DrawSphere(transform.position, 0.5f);
	//}
	#endregion

}
