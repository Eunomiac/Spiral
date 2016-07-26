using UnityEngine;

// ***** SET TO EXECUTE FIRST IN SCRIPT EXECUTION ORDER *****
// Locates & caches major game objects and components, and provides references on request.

public class GAME : MonoBehaviour
{

    public float beatDuration = 1f;             // The length of a single Beat in seconds.
    public int allowedDownBeats = 2;            // The number of beats worth of inactivity before penalty.
    public int maxSimultaneousAttacks = 3;
    public int maxTaps = 3;               // Maximum number of taps that will combine into a MultiTap.
    public float leeway = 0.4f;              // Max time between taps to combine them into a MultiTap.
    public bool isDebugging = true;
    public Material defaultMaterial;
    public GameObject indicatorPrefab;

    public const int BIGINT = 1000000;
    public enum Element { FIRE, ARCANE }

    public static float BeatDuration { get; set; }
    public static int AllowedDownBeats { get; set; }
    public static int MaxSimultaneousAttacks { get; set; }
    public static int MaxTaps { get; set; }
    public static float Leeway { get; set; }
    public static bool IsDebugging { get; set; }
    public static Material DefaultMaterial { get; set; }
    public static GameObject IndicatorPrefab { get; set; }

    private static Color[] colorList = new Color[7] { Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.yellow };

    #region Object Caching
    public static GAME Game { get; set; }
    public static AUDIO Audio { get; set; }
    public static ARENA Arena { get; set; }
    public static INPUT Input { get; set; }
    public static MANTLE Mantle { get; set; }
    public static PLAYER Player { get; set; }
    public static MAGIC Magic { get; set; }
    public static THREATS Threats { get; set; }

    void Awake ()
    {
        Game = FindObjectOfType<GAME>();
        BeatDuration = Game.beatDuration;
        AllowedDownBeats = Game.allowedDownBeats;
        MaxSimultaneousAttacks = Game.maxSimultaneousAttacks;
        MaxTaps = Game.maxTaps;
        Leeway = Game.leeway;
        IsDebugging = Game.isDebugging;
        DefaultMaterial = Game.defaultMaterial;
        IndicatorPrefab = Game.indicatorPrefab;
        Audio = FindObjectOfType<AUDIO>();
        Arena = FindObjectOfType<ARENA>();
        Input = FindObjectOfType<INPUT>();
        Mantle = FindObjectOfType<MANTLE>();
        Player = FindObjectOfType<PLAYER>();
        Magic = FindObjectOfType<MAGIC>();
        Threats = FindObjectOfType<THREATS>();
    }
    #endregion

    //public static float BeatDuration
    //{
    //    get { return beatDuration + Random.Range(-beatDuration * 0.1f, beatDuration * 0.1f); }
    //}

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
            GameObject thisIndicator = Instantiate(GAME.IndicatorPrefab, vectors[i], Quaternion.identity) as GameObject;
            thisIndicator.name = "Point " + (i + 1);
            thisIndicator.GetComponentInChildren<SpriteRenderer>().color = thisColor;
            thisIndicator.SetParent(theseIndicators, true);
        }
    }
}
