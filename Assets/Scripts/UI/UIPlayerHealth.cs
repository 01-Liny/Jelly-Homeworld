using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    static public int health { get; private set; }
    public FSM temp;
    private static Text text;
    private static FSM m_FSM;

    private void Awake()
    {
        text = GetComponent<Text>();
        m_FSM = temp;
        ResetHealth();
    }

    public static void TakeHealth(int damage)
    {
        health-=damage;
        if (health <= 0)
            m_FSM.ChangeState("GameOver");
        text.text = health.ToString();
    }

    public static void ResetHealth()
    {
        health = 100;
        if(text!=null)
            text.text = health.ToString();
    }
}
