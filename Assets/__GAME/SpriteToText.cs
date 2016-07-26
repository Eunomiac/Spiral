using System.Collections.Generic;
using UnityEngine;

public class SpriteToText : MonoBehaviour
{

    public Sprite[] numbers = new Sprite[10];
    public Sprite[] letters = new Sprite[26];
    public float widthOverride = 0f;

    private string[] upperCaseLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    private static float maxSpriteWidth = 0f;

    static Dictionary<string, Sprite> number = new Dictionary<string, Sprite>();
    static Dictionary<string, Sprite> letter = new Dictionary<string, Sprite>();

    void Awake ()
    {
        for ( int i = 0; i < 10; i++ )
        {
            number.Add(i.ToString(), numbers[i]);
            maxSpriteWidth = Mathf.Max(maxSpriteWidth, numbers[i].bounds.size.x);
        }
        for ( int i = 0; i < 26; i++ )
        {
            letter.Add(upperCaseLetters[i], letters[i]);
            maxSpriteWidth = Mathf.Max(maxSpriteWidth, letters[i].bounds.size.x);
        }
        if ( widthOverride > 0f )
            maxSpriteWidth = widthOverride;
    }

    public static Sprite GetNumberSprite (int num)
    {
        return GetNumberSprite(num.ToString());
    }

    public static Sprite GetNumberSprite (string num)
    {
        if ( number.ContainsKey(num) )
            return number[num];
        else
            return null;
    }

    public static Sprite GetLetterSprite (string let)
    {
        if ( number.ContainsKey(let) )
            return number[let];
        else if ( letter.ContainsKey(let.ToUpper()) )
            return letter[let.ToUpper()];
        else
            return null;
    }

    public static GameObject ParsePhrase (string phrase, Color? color)
    {
        Color thisColor = color ?? Color.white;
        GameObject parentObject = new GameObject("\"" + phrase + "\"");
        float currentOffset = 0f;
        foreach ( char letter in phrase )
        {
            GameObject letterSprite = new GameObject("\"" + letter.ToString() + "\"", typeof(SpriteRenderer));
            letterSprite.transform.SetParent(parentObject.transform, false);
            letterSprite.transform.Rotate(new Vector3(90f, 0f, 0f));
            letterSprite.transform.localPosition = new Vector3(currentOffset, 0f, 0f);
            SpriteRenderer renderer = letterSprite.GetComponent<SpriteRenderer>();
            renderer.sprite = GetLetterSprite(letter.ToString());
            renderer.color = thisColor;
            //if ( GetNumberSprite(letter.ToString()) )
            //    currentOffset += maxSpriteWidth * 0.25f;
            //else if ( letter.ToString() == " " )
            //    currentOffset += maxSpriteWidth * 0.5f;
            //else
            currentOffset += maxSpriteWidth;
        }
        return parentObject;
    }


}
