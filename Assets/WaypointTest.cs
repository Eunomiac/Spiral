using UnityEngine;
using System.Collections;
using RAIN.Navigation;
using RAIN.Navigation.Waypoints;

public class WaypointTest : MonoBehaviour {

	private WaypointRig waypoints;

	// Use this for initialization
	void Awake () {
		waypoints = GetComponent<WaypointRig>();	
	}

	void Start ()
	{
		Debug.Log(waypoints.WaypointSet.Waypoints.Count);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
