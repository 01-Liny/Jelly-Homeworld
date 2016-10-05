using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class GameOverFSM : FSM
{
    public FSM m_FSM;
    public Text m_Text;
    public InputField m_InputField;
    public ConfigManager m_ConfigManager;

    private Canvas m_Canvas;
    private int scoreTemp;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("GameOver", this);
    }

    public override void OnEnter(string prevState = "")
    {
        scoreTemp = UIScore.GetScore();
        m_Canvas.enabled = true;

        //测试代码
        string a="";
        if (m_ConfigManager.IsEnableAdd(scoreTemp)==false)
            a = "Disable";
        m_Text.text = scoreTemp.ToString()+a;

    }

    public override void OnExit(string nextState = "")
    {
        for (int i = 0; i < m_States.Count; i++)
        {
            m_States.Values.ElementAt(i).OnExit();
        }
        m_Canvas.enabled = false;
    }

    public override void OnClick()
    {

    }

    //For UI
    public void Add()
    {
        string temp = m_InputField.text;
        m_ConfigManager.Add(temp, scoreTemp);
        m_ConfigManager.Save();
    }
}