using UnityEngine;

public class OneSecCounter : MonoBehaviour
{
    private int numSeconds = 0;
    private GameObject timeMessage;


    // Use this for initialization
    void Start ()
    {
        timeMessage = ShowTime();
    }

    void Update ()
    {
        if ( Time.time > numSeconds )
        {
            numSeconds++;
            timeMessage = ShowTime();
        }
    }

    GameObject ShowTime ()
    {
        if ( timeMessage )
        {
            timeMessage.transform.DestroyAllChildren(true);
        }
        GameObject thisShowTime = SpriteToText.ParsePhrase(numSeconds.ToString(), Color.green);
        thisShowTime.transform.SetParent(transform, false);
        return thisShowTime;
    }
}
