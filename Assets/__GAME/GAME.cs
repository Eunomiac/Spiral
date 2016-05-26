using UnityEngine;
using System.Collections;

// ***** SET TO EXECUTE FIRST IN SCRIPT EXECUTION ORDER *****
// Locates & caches major game objects and components, and provides references on request.

public class GAME : MonoBehaviour {

	private static float beatDuration = 2f;

	public static ARENA Arena { get; set; }
	public static PLAYER Player { get; set; }
	public static THREATS Threats { get; set; }

	public static float BeatDuration { get { return beatDuration + Random.Range(-beatDuration * 0.1f, beatDuration * 0.1f); } }
	
	void Awake () {
		Arena = FindObjectOfType<ARENA>();
		Player = FindObjectOfType<PLAYER>();
		Threats = FindObjectOfType<THREATS>();
	}
	
}
