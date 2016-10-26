using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    static public int health { get; private set; }
    private static Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        ResetHealth();
    }

    public static void TakeHealth(int damage)
    {
        health-=damage;
        text.text = health.ToString();
    }

    public static void ResetHealth()
    {
        health = 100;
        text.text = health.ToString();
    }
}
