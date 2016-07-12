using UnityEngine;

public class CastHand : MonoBehaviour
{
    public float rotateSpeed = 5f;

    private GameObject hand;
    private SpriteRenderer handSprite;
    private LTDescr rotateTween;

    public GameObject Hand { get { return hand; } }
    public GameObject PreCastFX { get; set; }
    public int ButtonAxis { get; set; }
    public Vector3? StartDir { get; set; }

    void Awake ()
    {
        hand = transform.GetChild(0).gameObject;
        handSprite = hand.GetComponentInChildren<SpriteRenderer>();
    }

    public void ToggleActive (bool isActive)
    {
        if ( isActive )
        {
            rotateTween = LeanTween.rotateLocal(gameObject, new Vector3(0f, transform.localRotation.eulerAngles.y, 0f), 0.1f).setFrom(new Vector3(0f, transform.localRotation.eulerAngles.y, 0f));
            rotateTween.setRepeat(-1).setOnComplete(haltTweening).setOnCompleteOnRepeat(true);
            //rotateTween.setEase(LeanTweenType.easeOutQuad);
        }
        else
        {
            rotateTween.setOnComplete(tweenFinished);
        }
    }

    void haltTweening ()
    {
        rotateTween.setFrom(new Vector3(0f, transform.localRotation.eulerAngles.y, 0f)).setTo(new Vector3(0f, transform.localRotation.eulerAngles.y, 0f)).setTime(0f);
    }

    void tweenFinished ()
    {
        LeanTween.cancel(rotateTween.id);
        rotateTween = null;
    }

    public void FadeHand (bool isFadeOut)
    {
        Color thisColor = handSprite.color;
        thisColor.a = isFadeOut ? 0.3f : 1f;
        handSprite.color = thisColor;
    }

    public void TweenRotate (Vector3? dir)
    {
        if ( dir == null )
            TweenHand();
        else
            TweenHand(((Vector3) dir).FacingAngle());
    }

    void TweenHand (float? thisAngle = null)
    {
        float currentAngle = transform.localRotation.eulerAngles.y;
        float angle = thisAngle ?? currentAngle;
        float rotateTime = thisAngle == null ? 0f : Mathf.Abs(0.005555f * currentAngle.Diff(angle)) * rotateSpeed;
        if ( currentAngle.Diff(angle) != 0f )
        {
            Debug.Log(gameObject.name + ": " + Mathf.RoundToInt(currentAngle).ToString() + " to " + Mathf.RoundToInt(angle).ToString() + ", delta: " + Mathf.RoundToInt(currentAngle.Diff(angle)).ToString() + ", speed: " + (rotateTime).ToString());
            rotateTween.setFrom(new Vector3(0f, currentAngle, 0f)).setTo(new Vector3(0f, angle, 0f)).setTime(rotateTime);
        }
    }
}
