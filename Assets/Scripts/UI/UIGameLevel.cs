using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameLevel : MonoBehaviour 
{
    static public int level { get; private set; }
    private static Text text;

    private void Start()
    {
        level = 0;
        text = GetComponent<Text>();
        text.text = level.ToString();
    }

    public static void AddLevel()
    {
        level++;
        text.text = level.ToString();
    }

    public static void ResetLevel()
    {
        level=0;
        text.text = level.ToString();
    }

    //给UI使用
    public void Reset()
    {
        level = 0;
        text.text = level.ToString();
    }

    //给UI使用
    public void Add()
    {
        level++;
        text.text = level.ToString();
    }
}
