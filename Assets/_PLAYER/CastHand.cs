using UnityEngine;

public class CastHand : MonoBehaviour
{
	private GameObject hand;
	private SpriteRenderer handSprite;

	public GameObject Hand { get { return hand; } }
	public GameObject PreCastFX { get; set; }
	public int ButtonAxis { get; set; }
	public Vector3? StartDir { get; set; }

	void Awake ()
	{
		hand = transform.GetChild(0).gameObject;
		handSprite = hand.GetComponentInChildren<SpriteRenderer>();
	}

	public void FadeHand (bool isFadeOut)
	{
		Color thisColor = handSprite.color;
		thisColor.a = isFadeOut ? 0.3f : 1f;
		handSprite.color = thisColor;
	}
}
