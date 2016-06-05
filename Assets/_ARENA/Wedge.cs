using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wedge : MonoBehaviour {

	[HideInInspector]
	public List<Node> Nodes = new List<Node>();
	public List<Wedge> Neighbours = new List<Wedge>();

	public float MinAngle { get; set; }
	public float MaxAngle { get; set; }

}
