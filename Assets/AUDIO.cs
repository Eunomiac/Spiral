using System;
using UnityEngine;

public class AUDIO : MonoBehaviour
{
    public AudioClip[] AmbientMusic;

    void Start ()
    {
        PlaySound(AmbientMusic[0], gameObject, true);
    }

    public GameObject PlaySound (AudioClip clip, GameObject parentObj = null, bool isLooping = false)
    {
        AudioSource thisSound = new GameObject("Clip: " + clip.name, typeof(AudioSource)).GetComponent<AudioSource>();
        thisSound.transform.SetParent((parentObj ?? gameObject).transform, false);
        thisSound.clip = clip;
        thisSound.loop = isLooping;
        thisSound.Play();
        if ( !isLooping )
            Destroy(thisSound.gameObject, Mathf.Max(5f, clip.length * 2f));
        return thisSound.gameObject;
    }

    public void VolumeTween (GameObject audioObject, float time, float startVol = 1f, float endVol = 0f, LeanTweenType easing = LeanTweenType.easeInQuad)
    {
        Action<float, object> updateFunc = updateVolume;
        LeanTween.value(audioObject, updateFunc, startVol, endVol, time).setEase(easing);
    }

    void updateVolume (float val, object obj)
    {

        AudioSource thisPlayer = ((GameObject) obj).GetComponent<AudioSource>();
        thisPlayer.volume = val;
    }

}
