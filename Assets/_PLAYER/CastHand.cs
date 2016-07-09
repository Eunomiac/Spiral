using UnityEngine;
using System.Collections;

public class CastHand : MonoBehaviour
{
	private GameObject hand;
	private SpriteRenderer handSprite;

	public GameObject Hand { get { return hand; } }

	void Awake ()
	{
		hand = transform.GetChild(0).gameObject;
		handSprite = hand.GetComponentInChildren<SpriteRenderer>();
	}

	public void FadeHand(bool isFadeOut)
	{
		Color thisColor = handSprite.color;
		thisColor.a = isFadeOut ? 127f : 255f;
		handSprite.color = thisColor;
	}
}
