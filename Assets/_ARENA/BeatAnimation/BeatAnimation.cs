using UnityEngine;
using UnityEngine.UI;

public class BeatAnimation : MonoBehaviour
{

    private float beatDuration;

    private Animator animator;
    private ParticleSystem particles;
    private Text secDisplay;


    void Awake ()
    {
        beatDuration = GAME.BeatDuration;
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    void Start ()
    {

        AnimatorStateInfo beatState = animator.GetCurrentAnimatorStateInfo(0);
        float conversionToBeat = beatState.length * animator.speed / beatDuration;
        animator.speed = conversionToBeat;
    }

    public void FlashBeat ()
    {
        particles.Stop(true);
        particles.Play(true);
    }
}