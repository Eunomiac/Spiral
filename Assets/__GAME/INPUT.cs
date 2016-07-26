using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class INPUT : MonoBehaviour {

	public static string[] ButtonAxes = { "A", "B", "X", "Y", "LB", "RB", "Back", "Start", "", "LT", "RT" };

	private bool[] lastInput = new bool[ButtonAxes.Length];
	private int[] tapCount = new int[ButtonAxes.Length];
	private float[] lastTap = new float[ButtonAxes.Length];
	private bool[] isHeldDown = new bool[ButtonAxes.Length];
	private Vector3? dirLS, dirRS, startDirLS, startDirRS;

	private PLAYER player = GAME.Player;

	void Awake ()
	{
		player = GAME.Player;
	}

	void Start ()
	{
		StartCoroutine(WatchLeftStick());
		StartCoroutine(WatchRightStick());
		foreach (string button in ButtonAxes.Where(item => item != ""))
			StartCoroutine(WatchButton(button));
	}

	#region Coroutines: Input Monitoring (Buttons & Sticks)
	IEnumerator WatchLeftStick ()
	{
		while ( true )
		{
			if ( Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
				dirLS = new Vector3(Input.GetAxis("Horizontal"), 0, -1 * Input.GetAxis("Vertical"));
			else
				dirLS = null;
			yield return null;
		}
	}

	IEnumerator WatchRightStick ()
	{
		while ( true )
		{
			if ( Input.GetAxis("RightStickHoriz") != 0 || Input.GetAxis("RightStickVert") != 0 )
				dirRS = new Vector3(Input.GetAxis("RightStickHoriz"), 0, -1 * Input.GetAxis("RightStickVert"));
			else
				dirRS = null;
			yield return null;
		}
	}

	IEnumerator WatchButton (string button)
	{
		int axis = Array.IndexOf(ButtonAxes, button);
		while ( true )
		{
			ProcessButtonInput(axis);
			yield return null;
		}
	}
	#endregion

	public Vector3? LSVector { get { return dirLS; } }
	public Vector3? RSVector { get { return dirRS; } }
	public float? LSAngle { get { return GetStickAngle("L"); } }
	public float? RSAngle { get { return GetStickAngle("R"); } }

	public float? GetStickAngle (string stick)
	{
		Vector3? thisDir = stick == "L" ? LSVector : RSVector;
		if ( thisDir == null )
			return null;
		return ((Vector3) thisDir).x >= 0 ? Vector3.Angle(Vector3.forward, (Vector3) thisDir) : (360f - Vector3.Angle(Vector3.forward, (Vector3) thisDir));
	}

	void ProcessButtonInput (int axisNum)
	{
		if ( Input.GetAxis(ButtonAxes[axisNum]) != 0 )  // IF Button HELD DOWN...
		{										
			if ( !lastInput[axisNum] )								// IF button NOT held down last input, this is a **TAP** (rising edge)
			{                                   
				lastTap[axisNum] = Time.time;
				lastInput[axisNum] = true;
				tapCount[axisNum]++;
				if ( tapCount[axisNum] == 1 )							// If this is the FIRST tap, alert PLAYER immediately for instant graphic response.
				{
					startDirLS = LSVector;
					player.FirstTap(axisNum, startDirLS);
				}
			}
			else if ( Time.time - lastTap[axisNum] > GAME.Leeway )	// ELSE IF button held for longer than leeway, this is a **HOLD**.
			{
				if ( !isHeldDown[axisNum] )								// Check if this is continuing a hold, or starting a new one.
				{
					player.StartHold(axisNum, tapCount[axisNum]);              
					isHeldDown[axisNum] = true;
				}
			}
		}
		else											// ELSE IF Button NOT Held Down...
		{
			lastInput[axisNum] = false;                                 
			if ( isHeldDown[axisNum] )								// If button WAS held down last input, this is an **END HOLD**.
			{
				player.EndHold();                      
				isHeldDown[axisNum] = false;
				tapCount[axisNum] = 0;
				lastTap[axisNum] = 0;
			}														// Else if leeway time passed and previous taps recorded, this is a **MULTITAP**.
			else if ( tapCount[axisNum] > 0 && Time.time - lastTap[axisNum] > GAME.Leeway || tapCount[axisNum] >= GAME.MaxTaps )
			{
				player.MultiTap(axisNum, tapCount[axisNum]);
				tapCount[axisNum] = 0;
				lastTap[axisNum] = 0;
			}
		}
	}
}