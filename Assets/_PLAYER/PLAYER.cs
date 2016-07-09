using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RAIN.Navigation;
using RAIN.Navigation.Waypoints;

public class PLAYER : MonoBehaviour
{
	public int[] nodesPerTier = new int[3] { 8, 16, 32 };
	public float[] radiusOfTier = new float[3] { 3f, 6f, 9f };
	public float maxNeighbourDistMult = 1.5f;
	public float armLength = 2f;

	private List<CastHand> idleHands = new List<CastHand>();
	private List<CastHand> busyHands = new List<CastHand>();
	private CastHand activeHand, currentHand;
	private List<string> spellButtons = new List<string>( new string[] { "A", "B", "X", "Y" } );

	private ARENA arena;
	private INPUT input;
	//private MANTLE mantle;
	private SPELLS spells;

	public NavNetwork NavNetwork { get; set; }

	void Awake ()
	{
		arena = GAME.Arena;
		input = GAME.Input;
		//mantle = GAME.Mantle;
		spells = GAME.Spells;
	}

	void Start ()
	{
		NavNetwork = arena.InitializeNavNetwork(gameObject, nodesPerTier, radiusOfTier, maxNeighbourDistMult);
		idleHands = GetComponentsInChildren<CastHand>().ToList();
		foreach (CastHand hand in idleHands)
			hand.FadeHand(true);
	}

	void Update()
	{
		if (transform.hasChanged)
		{
			NavNetwork.BuildNavNodes(gameObject, nodesPerTier, radiusOfTier);
			transform.hasChanged = false;
		}
		AimHand();
	}

	bool AimHand()
	{
		if ( !activeHand && !ClaimHand() )
			return false;
		if ((input.LSVector == null) == activeHand.gameObject.activeSelf)
			activeHand.gameObject.SetActive(input.LSVector != null);
		if ( input.LSVector != null )
			activeHand.transform.localRotation = Quaternion.Euler(0, (float) input.LSAngle, 0);
		return true;
	}

	// Get an idle hand and set it active for aiming spells with Left Stick.  Returns false if no idle hand available.
	bool ClaimHand()
	{
		if ( idleHands.Count == 0 )
			return false;
		activeHand = idleHands.Pop();
		activeHand.FadeHand(false);
		return true;
	}

	// Lock the active hand after it begins casting a spell.  Returns false if there is no active hand.
	public bool LockHand()
	{
		if ( activeHand == null )
			return false;
		busyHands.Push(activeHand);
		activeHand = null;
		return true;
	}

	// Release the given hand, returning it to the idle hands pool.  Returns false if given hand is not in the busy pool.
	public bool ReleaseHand(CastHand hand)
	{
		if ( busyHands.Remove(hand) )
		{
			idleHands.Push(hand);
			hand.FadeHand(true);
			return true;
		}
		return false;
	}

	public void FirstTap (int axis, Vector3? startDirLS)
	{
		currentHand = activeHand;
		if (spellButtons.Contains(INPUT.ButtonAxes[axis]))
			LockHand();
		spells.PreCast(currentHand, axis, startDirLS);
	}

	public void MultiTap (int axis, int taps)
	{
		spells.StartCast(taps);
		currentHand = null;

	}

	public void StartHold (int axis, int taps)
	{
		spells.StartHold(currentHand, taps);
	}

	public void EndHold ()
	{
		spells.EndHold(currentHand);
		currentHand = null;
	}

	public void TakeHit (float strength)
	{
		Debug.Log("Took hit of " + strength + " strength!");
	}

}
