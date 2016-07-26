using UnityEngine;

public class BeatAnimation : MonoBehaviour
{

    private float beatDuration;
    private Animator animator;
    private ParticleSystem particles;


    void Awake ()
    {
        beatDuration = GAME.BeatDuration;
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    void Start ()
    {
        AnimatorStateInfo beatState = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(beatState.IsName("BeatAnimation"));
        float conversionToBeat = beatState.length * animator.speed / beatDuration;
        Debug.Log("BeatState = " + beatState.length + ", BeatDur = " + beatDuration + ", ConversionToBeat = " + conversionToBeat);
        animator.speed = conversionToBeat;
        Debug.Log("New Duration = " + animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void FlashBeat ()
    {
        particles.Stop(true);
        particles.Play(true);
    }
}
