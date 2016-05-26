using UnityEngine;
using System.Collections;

// ***** SET TO EXECUTE FIRST IN SCRIPT EXECUTION ORDER *****
// Locates & caches major game objects and components, and provides references on request.

public class GameCache : MonoBehaviour {

	public static ARENA Arena { get; set; }
	public static PLAYER Player { get; set; }
	public static THREATS Threats { get; set; }
	
	void Awake () {
		Arena = FindObjectOfType<ARENA>();
		Player = FindObjectOfType<PLAYER>();
		Threats = FindObjectOfType<THREATS>();
	}
	
}
