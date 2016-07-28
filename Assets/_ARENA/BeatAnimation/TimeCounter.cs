using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{

    private int numSeconds = 0;
    private Text secDisplay;

    void Awake ()
    {
        secDisplay = GetComponentInChildren<Text>();
    }

    void Start ()
    {
        secDisplay.text = numSeconds.ToString();
    }

    void Update ()
    {
        if ( Time.time > numSeconds + 1 )
        {
            numSeconds++;
            secDisplay.text = numSeconds.ToString();
        }
    }
}
