using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScore : MonoBehaviour 
{
    static private Text m_Text;

    static private float score=0;

    //积分偏移量
    private const float offset = 9.5f;

    private void Start()
    {
        m_Text = GetComponent<Text>();
    }

    static public void Add(float newScore)
    {
        //只要有攻击，至少都会得1分
        score += newScore+1;

        UpdateUI();
    }

    static public void Reset()
    {
        score = 0;
        UpdateUI();
    }

    static private void UpdateUI()
    {
        //更新得分UI，乘以固定数值防止内存修改
        m_Text.text = ((int)(score * offset)).ToString();
    }
}
