using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ***** SET TO EXECUTE FIRST IN SCRIPT EXECUTION ORDER *****
// Locates & caches major game objects and components, and provides references on request.

public class GAME : MonoBehaviour {

	public const int BIGINT = 1000000; 

	public static float beatDuration = 2f;      // The length of a single Beat in seconds.
	public static int AllowedDownBeats = 2;     // The number of beats worth of inactivity before penalty.
	public static int MaxSimultaneousAttacks = 3;
	public static int maxTaps = 3;				// Maximum number of taps that will combine into a MultiTap.
	public static float leeway = 0.4f;          // Max time between taps to combine them into a MultiTap.
	public static bool isDebugging = true;


	#region Object Caching
	public static ARENA Arena { get; set; }
	public static PLAYER Player { get; set; }
	public static THREATS Threats { get; set; }

	void Awake ()
	{
		Arena = FindObjectOfType<ARENA>();
		Player = FindObjectOfType<PLAYER>();
		Threats = FindObjectOfType<THREATS>();
	}
	#endregion

	public static float BeatDuration
	{
		get { return beatDuration + UnityEngine.Random.Range(-beatDuration * 0.1f, beatDuration * 0.1f); }
	}
}
