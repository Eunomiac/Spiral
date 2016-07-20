using UnityEngine;

// ***** SET TO EXECUTE FIRST IN SCRIPT EXECUTION ORDER *****
// Locates & caches major game objects and components, and provides references on request.

public class GAME : MonoBehaviour
{

    public const int BIGINT = 1000000;

    public static float beatDuration = 1f;      // The length of a single Beat in seconds.
    public static int AllowedDownBeats = 2;     // The number of beats worth of inactivity before penalty.
    public static int MaxSimultaneousAttacks = 3;
    public static int maxTaps = 3;              // Maximum number of taps that will combine into a MultiTap.
    public static float leeway = 0.4f;          // Max time between taps to combine them into a MultiTap.
    public static bool isDebugging = true;

    public Material defaultMaterial;
    public GameObject indicatorPrefab;

    public static GameObject Indicator { get; set; }

    private static Color[] colorList = new Color[7] { Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.yellow };

    #region Object Caching
    public static AUDIO Audio { get; set; }
    public static ARENA Arena { get; set; }
    public static INPUT Input { get; set; }
    public static MANTLE Mantle { get; set; }
    public static PLAYER Player { get; set; }
    public static MAGIC Magic { get; set; }
    public static THREATS Threats { get; set; }

    void Awake ()
    {
        Audio = FindObjectOfType<AUDIO>();
        Arena = FindObjectOfType<ARENA>();
        Input = FindObjectOfType<INPUT>();
        Mantle = FindObjectOfType<MANTLE>();
        Player = FindObjectOfType<PLAYER>();
        Magic = FindObjectOfType<MAGIC>();
        Threats = FindObjectOfType<THREATS>();
        Indicator = indicatorPrefab;
    }
    #endregion

    public static float BeatDuration
    {
        get { return beatDuration + Random.Range(-beatDuration * 0.1f, beatDuration * 0.1f); }
    }

    public static Color RandomColor
    {
        get { return colorList.Random(); }
    }

    public static void DrawIndicators (Vector3[] vectors, string objName = "Indicator Set")
    {
        Color thisColor = RandomColor;
        GameObject theseIndicators = new GameObject(objName);
        for ( int i = 0; i < vectors.Length; i++ )
        {
            GameObject thisIndicator = Instantiate(GAME.Indicator, vectors[i], Quaternion.identity) as GameObject;
            thisIndicator.name = "Point " + (i + 1);
            thisIndicator.GetComponentInChildren<SpriteRenderer>().color = thisColor;
            thisIndicator.SetParent(theseIndicators, true);
        }
    }
}
